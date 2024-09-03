using Core.Entities.AccurecyIndex;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Xml;


namespace Infrastructure.Services.AccurecyIndexService;

public class AccurecyIndexService : IAccurecyIndexService
{
    private readonly IConfiguration _accurecyIndexConfig;

    public AccurecyIndexService(IConfiguration accurecyIndexConfig)
    {
        _accurecyIndexConfig = accurecyIndexConfig;
    }
    public async Task<AccurecyIndexFilteredData> GetAccurecyIndexDataAsync()
    {
        AccurecyIndexFilteredData data = new(); ;
        string dt = System.DateTime.Now.ToString("yyyy-MM-dd");
        System.Uri url = new(_accurecyIndexConfig.GetSection("AccurecyIndex:url").Value + dt);
        using (var client = new System.Net.Http.HttpClient())
        {
            client.DefaultRequestHeaders.Add("SOAPAction", _accurecyIndexConfig.GetSection("AccurecyIndex:url").Value);
            using var response = await client.GetAsync(url);
            XmlDocument doc = new();
            doc.LoadXml(await response.Content.ReadAsStringAsync());
            XmlNodeList xmlNodeList = doc.GetElementsByTagName("Record");
            XmlElement xmlElement = (XmlElement)xmlNodeList.Item(0);

            data.DIUK = (xmlElement.GetAttribute("DIUK"));
            data.IHRU = (xmlElement.GetAttribute("IHRU"));
            data.POAL = (xmlElement.GetAttribute("POAL"));
            data.CANCEL = (xmlElement.GetAttribute("CANCEL"));
            data.THDIUK = (xmlElement.GetAttribute("THDIUK"));
        }
        return data;
    }
}
