using Core.Entities.Vouchers;
using DIP.Templates;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DIP.Services
{
    public class VouchersService : IVouchersService
    {
        private readonly RailDbContext _context;

        public VouchersService(RailDbContext context)
        {
            _context = context;
        }
        public async Task ProcessXmlAsync(string folder)
        {
            IEnumerable<string> xmlfiles = Directory.EnumerateFiles(folder, "*.xml");

            if (xmlfiles.Count() == 0)
            {
                return;
            }

            foreach (string filePath in xmlfiles)
            {
                await ProcessXmlFile(filePath);
            }
        }

        private async Task ProcessXmlFile(string filePath)
        {
            XmlDocument xmlDocument = GetXml(filePath);

            string fileName = Path.GetFileNameWithoutExtension(filePath);
            switch (Path.GetFileName(fileName).ToLower())
            {
                case "fshana":
                    await ProcessRailCalendarAsync(xmlDocument);
                    break;
                case "fthnot":
                    await ProcessStationsAsync(xmlDocument);
                    break;
            }
        }

        private async Task ProcessRailCalendarAsync(XmlDocument xml)
        {
            string json = JsonConvert.SerializeXmlNode(xml.SelectSingleNode("DB4002XML"));
            XmlTemplate<FSHANA> fshana = JsonConvert.DeserializeObject<XmlTemplate<FSHANA>>(json);

            IEnumerable<DateTime> railCalendarDates = await _context.RailCalendar.Select(p => p.Date).ToArrayAsync();

            foreach (var node in fshana.DB4002XML.File.Record)
            {
                var railCalendar = new RailCalendar
                {
                    Id = node.Number,
                    Date = new DateTime(node.Year, node.Month, node.Date),
                    DayInWeek = node.DayOnw.Trim(),
                    NumberOfDayInWeek = node.DayMis,
                    NameOfHoliday = node.HagNam.Trim(),
                    CMO60 = node.CMO60
                };

                if (railCalendarDates.Any(p => p == railCalendar.Date))
                {
                    _context.Set<RailCalendar>().Attach(railCalendar);
                    _context.Entry(railCalendar).State = EntityState.Modified;
                }
                else
                {
                    _context.RailCalendar.Add(railCalendar);
                }
            }

            await _context.SaveChangesAsync();
        }
        private async Task ProcessStationsAsync(XmlDocument xml)
        {
            string json = JsonConvert.SerializeXmlNode(xml.SelectSingleNode("DB4002XML"));
            XmlTemplate<FTHNOT> fthnot = JsonConvert.DeserializeObject< XmlTemplate<FTHNOT>>(json);

            IEnumerable<int> stationIds = await _context.Stations.Select(p => p.StationId).ToArrayAsync();

            foreach (var node in fthnot.DB4002XML.File.Record)
            {
                var station = new Station
                {
                    StationId = node.StationId,
                    RjpaName = node.RjpaName.Trim(),
                    HebrewName = node.RjpaName.Trim(),
                    MetropolinId = 2
                };

                if (stationIds.Any(p => p == station.StationId)) {
                    _context.Set<Station>().Attach(station);
                    _context.Entry(station).State = EntityState.Modified;
                }
                else
                {
                    _context.Stations.Add(station);
                }
            }

            await _context.SaveChangesAsync();
        }

        private XmlDocument GetXml(string filePath)
        {
            XmlDocument xmlDocument = new XmlDocument();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding encoding = Encoding.GetEncoding(1255);

            using (StreamReader reader = new StreamReader(filePath, encoding))
            {
                xmlDocument.LoadXml(reader.ReadToEnd());
            }

            return xmlDocument;
        }
    }
}
