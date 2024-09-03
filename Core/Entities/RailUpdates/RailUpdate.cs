using Core.Enums;

namespace Core.Entities.RailUpdates;

public class RailUpdate
{
    public IEnumerable<UpdateStationNode> Data { get; set; }

    public IEnumerable<UpdateStationNode> Filter(Languages languageId)
    {
        IEnumerable<UpdateStationNode> filteredItems = null;

        if (languageId == Languages.Hebrew)
        {
            filteredItems = this.Data.Where(p => !string.IsNullOrEmpty(p.UpdateContentHeb)).ToArray();
        }
        else if (languageId == Languages.English)
        {
            filteredItems = this.Data.Where(p => !string.IsNullOrEmpty(p.UpdateContentEng)).ToArray();
        }
        else if (languageId == Languages.Arabic)
        {
            filteredItems = this.Data.Where(p => !string.IsNullOrEmpty(p.UpdateContentArb)).ToArray();
        }
        else if (languageId == Languages.Russian)
        {
            filteredItems = this.Data.Where(p => !string.IsNullOrEmpty(p.UpdateContentRus)).ToArray();
        }

        return filteredItems;
    }
}

public class DataNode
{
    public List<UpdateStationNode> UpdateStations { get; set; }
}

public class UpdateStationNode
{
    public DateTime EndValidationOfReport { get; set; }
    public string? NameArb { get; set; }
    public string? NameEng { get; set; }
    public string? NameHeb { get; set; }
    public string? NameRus { get; set; }
    public int Order { get; set; }
    public string? ReportImage { get; set; }
    public DateTime StartValidationOfReport { get; set; }
    [Newtonsoft.Json.JsonProperty(propertyName: "Station")]
    public required IEnumerable<string> Stations { get; set; }
    public string? UpdateContentArb { get; set; }
    public string? UpdateContentEng { get; set; }
    public string? UpdateContentHeb { get; set; }
    public string? UpdateContentRus { get; set; }
    public string? UpdateLinkArb { get; set; }
    public string? UpdateLinkEng { get; set; }
    public string? UpdateLinkHeb { get; set; }
    public string? UpdateLinkRus { get; set; }
    [Newtonsoft.Json.JsonProperty(propertyName: "Float")]
    public bool IsFloat { get; set; }
}

public class RailUpdateResponseUmbracoDto
{
    public string? UpdateHeader { get; set; }
    public string? UpdateContent { get; set; }
    public string? UpdateLink { get; set; }
    public required IEnumerable<string> Stations { get; set; }
    public string? LinkText { get; set; }
    public string? UpdateType { get; set; }
    public RailUpdateResponseUmbracoDto()
    {

    }
    [System.Diagnostics.CodeAnalysis.SetsRequiredMembersAttribute]
    public RailUpdateResponseUmbracoDto(string updateHeader, string updateContent, string updateLink, IEnumerable<string> stations, string linkText, string updateType)
    {
        UpdateHeader = updateHeader;
        UpdateContent = updateContent;
        UpdateLink = updateLink;
        Stations = stations;
        LinkText = linkText ?? throw new ArgumentNullException(nameof(linkText));
        UpdateType = updateType ?? throw new ArgumentNullException(nameof(updateType));
    }

                        }

public class ChildrenDto
{
    public string? header { get; set; }
    public string? contentdata { get; set; }
    public DateTime startDate { get; set; }
    public DateTime endDate { get; set; }
    public string? updateType { get; set; }
    public object? updatePage { get; set; }
    public List<string> stations { get; set; }
    public string? linkText { get; set; }
}

public class UpdatePageDto
{
    public string? id { get; set; }
}
public class RailUpdateResponseFromUmbracoDto
{
    public string? UpdateHeader { get; set; }
    public string? UpdateContent { get; set; }
    public string? UpdateLink { get; set; }
    public string? LinkText { get; set; }
    public string? UpdateType { get; set; }
    public string? header { get; set; }
    public string? contentdata { get; set; }
    public DateTime startDate { get; set; }
    public DateTime endDate { get; set; }
    public string updateType { get; set; }
    public object? updatePage { get; set; }
    public List<string> stations { get; set; }
    public string? linkText { get; set; }
}

public class RailUpdateAllLanguagesDto
{
    public IEnumerable<RailUpdateResponseUmbracoDto> Hebrew { get; set; }
    public IEnumerable<RailUpdateResponseUmbracoDto> English { get; set; }
    public IEnumerable<RailUpdateResponseUmbracoDto> Arabic { get; set; }
    public IEnumerable<RailUpdateResponseUmbracoDto> Russian { get; set; }
}
