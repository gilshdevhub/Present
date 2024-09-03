using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Reflection;
using System.Security;
using System.Xml;

namespace Infrastructure.Services;

public class PaymantsService : IPaymantsService
{
    private readonly RailDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<MessengerService> _logger;
    private readonly IHttpClientService _httpClient;

    public PaymantsService(ILogger<MessengerService> logger, RailDbContext context, IConfiguration configuration, IHttpClientService httpClient)
    {
        _context = context;
        _configuration = configuration;
        _logger = logger;
        _httpClient = httpClient;   
    }

    public async Task<Object> TxnSetup(TxnInternalRequestDto requestData)
    {
        Assembly asm = Assembly.GetExecutingAssembly();
        string resourceName = "Infrastructure.Templates.TxnSetupTemplate.xml";

        XmlDocument xmlDocument = new();
        using (Stream stream = asm.GetManifestResourceStream(resourceName))
        {
            using XmlTextReader reader = new(stream);
            xmlDocument.Load(reader);
        }
        Random random = new Random();
        string xmlTemplate = xmlDocument.InnerXml;
        xmlTemplate = xmlTemplate.Replace("{terminal}", SecurityElement.Escape(_configuration.GetSection("Paymants:terminal").Value));
        xmlTemplate = xmlTemplate.Replace("{mpi_mid}", SecurityElement.Escape(_configuration.GetSection("Paymants:mpimid").Value));
        xmlTemplate = xmlTemplate.Replace("{total}", SecurityElement.Escape(requestData.totalAmount.ToString()));
        xmlTemplate = xmlTemplate.Replace("{language}", SecurityElement.Escape(requestData.language.ToString()));
        xmlTemplate = xmlTemplate.Replace("{uniqueid}", random.NextDouble().ToString());
        xmlTemplate = xmlTemplate.Replace("{successUrl}", SecurityElement.Escape(requestData.successUrl.ToString()));
        xmlTemplate = xmlTemplate.Replace("{cancelUrl}", SecurityElement.Escape(requestData.cancelUrl.ToString()));
        xmlTemplate = xmlTemplate.Replace("{errorUrl}", SecurityElement.Escape(requestData.errorUrl.ToString()));
        xmlTemplate = xmlTemplate.Replace("{ppsJSONConfig}", SecurityElement.Escape(requestData.ppsJSONConfig.ToString()));
        try
        {
            var res = await CguarHttpClient(xmlTemplate);
            return res;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "free seats failer");
                       return false;
        }

        return true;
    }

    public async Task<string> InquireTransaction(InquireTransactionsRequestDto requestData)
    {
        Assembly asm = Assembly.GetExecutingAssembly();
        string resourceName = "Infrastructure.Templates.inquireTransactions.xml";

        XmlDocument xmlDocument = new();
        using (Stream stream = asm.GetManifestResourceStream(resourceName))
        {
            using XmlTextReader reader = new(stream);
            xmlDocument.Load(reader);
        }
               string xmlTemplate = xmlDocument.InnerXml;
        xmlTemplate = xmlTemplate.Replace("{terminal}", SecurityElement.Escape(_configuration.GetSection("Paymants:terminal").Value));
        xmlTemplate = xmlTemplate.Replace("{mpi_mid}", SecurityElement.Escape(_configuration.GetSection("Paymants:mpimid").Value));
        xmlTemplate = xmlTemplate.Replace("{txId}", SecurityElement.Escape(requestData.txId.ToString()));
        xmlTemplate = xmlTemplate.Replace("{language}", SecurityElement.Escape(requestData.language.ToString()));


        try
        {
            var res = (await CguarHttpClient(xmlTemplate)).ToString().Trim();
            XmlDocument document= new XmlDocument();
            document.InnerXml = res;
                       XmlNode ashrait = document.SelectSingleNode("ashrait");
            XmlNode response = ashrait.SelectSingleNode("response");
            XmlNode inquireTransactions = response.SelectSingleNode("inquireTransactions");
            XmlNode row = inquireTransactions.SelectSingleNode("row");

            string ress = JsonConvert.SerializeXmlNode(row);
            return ress;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "free seats failer");
                       return "";
        }

      
    }

    public async Task<Object> AutoComm(AutoCommRequestDto requestData)
    {
        Assembly asm = Assembly.GetExecutingAssembly();
        string resourceName = "Infrastructure.Templates.autoComm.xml";

        XmlDocument xmlDocument = new();
        using (Stream stream = asm.GetManifestResourceStream(resourceName))
        {
            using XmlTextReader reader = new(stream);
            xmlDocument.Load(reader);
        }
        Random random = new Random();
        string xmlTemplate = xmlDocument.InnerXml;
        xmlTemplate = xmlTemplate.Replace("{terminal}", SecurityElement.Escape(_configuration.GetSection("Paymants:terminal").Value));
        xmlTemplate = xmlTemplate.Replace("{mpi_mid}", SecurityElement.Escape(_configuration.GetSection("Paymants:mpimid").Value));
        xmlTemplate = xmlTemplate.Replace("{cardId}", SecurityElement.Escape(requestData.cardId.ToString()));
        xmlTemplate = xmlTemplate.Replace("{total}", SecurityElement.Escape(requestData.total.ToString()));
        xmlTemplate = xmlTemplate.Replace("{sessionCD}", SecurityElement.Escape(requestData.sessionCD.ToString()));
        xmlTemplate = xmlTemplate.Replace("{currentDate}", SecurityElement.Escape(DateTime.Now.ToString()));
        xmlTemplate = xmlTemplate.Replace("{language}", SecurityElement.Escape(requestData.language.ToString()));


        try
        {
            var res = await CguarHttpClient(xmlTemplate);
            return res;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "free seats failer");
                       return false;
        }

        return true;
    }

    private async Task<Object> CguarHttpClient(string xmlTemplate)
    {
        string requestUri = _configuration.GetSection("Paymants:url").Value;
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
        var collection = new List<KeyValuePair<string, string>>
        {
            new("user", _configuration.GetSection("Paymants:cgUser").Value),
            new("password", _configuration.GetSection("Paymants:cgPassword").Value),
            new("int_in", xmlTemplate)
        };
        var content = new FormUrlEncodedContent(collection);
        request.Content = content;
        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var res = await response.Content.ReadAsStringAsync();
        return res;
    }
}
