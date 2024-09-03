using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Vouchers;

public class StationActivityHoursTemplates
{
    public int TemplateId { get; set; }
    [MaxLength(50)]
    [Required]
    public string TemplateName { get; set; }
    [ForeignKey("StationActivityTemplatesTypes")]
    public int TemplateTypeId { get; set; }
    public StationActivityTemplatesTypes StationActivityTemplatesTypes { get; set; }

}

public class StationActivityHoursTemplateLinesDto
{
    public int TemplateId { get; set; }
    public string TemplateName { get; set; }
    public int TemplateTypeId { get; set; }
    public int TemplateLineId { get; set; }
    public string ActivityDaysNumbers { get; set; }
    public string ActivityHoursReplaceTextKey { get; set; }
    public TimeSpan StartHour { get; set; }
    public string EndHourPostfixTextKey { get; set; }
    public string EndHourPrefixTextKey { get; set; }
    public TimeSpan EndHour { get; set; }
    public string EndHourReplaceTextKey { get; set; }
    public string StartHourPrefixTextKey { get; set; }
    public string StartHourReplaceTextKey { get; set; }
}
public class TemplatesTranslationDto
{
    public string Key { get; set; }
    public string Hebrew { get; set; }
}

public class StationActivityHoursTemplatesPostDto
{
    public TemplatesPostDto template { get; set; }
    public IEnumerable<StationActivityHoursTemplatesLinePostDto> StationActivityHoursTemplatesLines { get; set; }

}

public class StationActivityHoursTemplatesUpdateDto
{
    public TemplatesDto template { get; set; }
    public IEnumerable<StationActivityHoursTemplatesLineDto> StationActivityHoursTemplatesLines { get; set; }
    public List<int> DeletedIds { get; set; }
}

public class TemplatesPostDto
{
    public string TemplateName { get; set; }
    public int TemplateTypeId { get; set; }

}

public class TemplatesDto
{
    public int TemplateId { get; set; }
    public string TemplateName { get; set; }
    public int TemplateTypeId { get; set; }

}

public class StationActivityHoursTemplatesLineDto
{
    public int Id { get; set; }
    public int TemplateId { get; set; }
    public string ActivityDaysNumbers { get; set; }
    public string StartHourPrefixTextKey { get; set; }
    public string StartHour { get; set; }
    public string StartHourReplaceTextKey { get; set; }
    public string EndHourPrefixTextKey { get; set; }
    public string EndHour { get; set; }
    public string EndHourReplaceTextKey { get; set; }
    public string EndHourPostfixTextKey { get; set; }
    public string ActivityHoursReplaceTextKey { get; set; }
}


public class StationActivityHoursTemplatesLinePostDto
{
    public string ActivityDaysNumbers { get; set; }
    public string StartHourPrefixTextKey { get; set; }
    public string StartHour { get; set; }
    public string StartHourReplaceTextKey { get; set; }
    public string EndHourPrefixTextKey { get; set; }
    public string EndHour { get; set; }
    public string EndHourReplaceTextKey { get; set; }
    public string EndHourPostfixTextKey { get; set; }
    public string ActivityHoursReplaceTextKey { get; set; }
}
