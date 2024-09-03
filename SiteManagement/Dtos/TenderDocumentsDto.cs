using Core.Entities;
using Microsoft.AspNetCore.Http;

namespace SiteManagement.Dtos;

public class TenderDocumentsDto
{
    public int DocId { get; set; }
    public string DocName { get; set; }
    public int DocType { get; set; }
    public string DocDisplay { get; set; }
    [Newtonsoft.Json.JsonIgnore]
    public ExemptionNotices ExemptionNotices { get; set; }
    [Newtonsoft.Json.JsonIgnore]
    public Tenders Tenders { get; set; }
    [Newtonsoft.Json.JsonIgnore]
    public SingleSupplier SingleSupplier { get; set; }
}
public class UploadFile
{
    public IFormFile FileToLoad { get; set; }
}
