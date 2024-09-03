using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Infrastructure.Services;

public class EligibilityService : IEligibilityService
{
    private readonly RailDbContext _context;

    public readonly Dictionary<string, string> map = new(StringComparer.OrdinalIgnoreCase)
    {
        [".csv"] = "text/*"
    };
    public EligibilityService(RailDbContext context)
    {
        _context = context;
    }


    public async Task<bool> UploadFileAsync(IFormFile FileToLoad)
    {
        DataTable dt = new();
        dt.Clear();
        using (Stream fs = FileToLoad.OpenReadStream())
        {
            using StreamReader sr = new(fs);

            string[] headers = sr.ReadLine().Split(',');
            foreach (string header in headers)
            {
                _ = dt.Columns.Add(header);
            }
            while (!sr.EndOfStream)
            {
                string[] rows = sr.ReadLine().Split(',');
                DataRow dr = dt.NewRow();
                for (int i = 0; i < headers.Length; i++)
                {
                    dr[i] = (rows[i]);
                }
                dt.Rows.Add(dr);
            }
            var d = dt;
            try
            {
                List<RavKav> ravkavs = dt.AsEnumerable().Select(row => new RavKav
                {
                    RavKavNumber = row.Field<string>("SmartcardID"),
                    Refundable = Convert.ToBoolean(row.Field<string>("Refundable")),
                    Amount = float.Parse(row.Field<string>("Amount"))//,
                }).ToList();

                _context.Database.ExecuteSqlRaw("TRUNCATE TABLE RavKav");
                               await _context.RavKav.AddRangeAsync(ravkavs);
                bool success = await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {

            }

        }
        return true;
    }

    public async Task<RavKav> Checking(string ravKavNumber)
    {
              RavKav res = await _context.RavKav.FirstOrDefaultAsync(x => x.RavKavNumber == ravKavNumber).ConfigureAwait(false);
        if (res == null)
        {
            return new RavKav();
        }
        return res;
     
    }


}


//string[] csvfilerecord = sr.ReadLine().Split(',');
////foreach (string header in headers)
////{
////    dt.Columns.Add(header);
////}

////using (var csv = new CsvReader(sr))
////{
////    csv.Configuration.RegisterClassMap<StudentMap>();
////    var records = csv.GetRecords<Student>().ToList();
////    return records;
////}

//foreach (var row in csvfilerecord)
//{
//    if (!string.IsNullOrEmpty(row))
//    {
//        var cells = row.Split(',');
//        RavKav ravKav = new RavKav
//        {
//            RavKavNumber = Convert.ToInt32(cells[0])
//        };

//    }
//}