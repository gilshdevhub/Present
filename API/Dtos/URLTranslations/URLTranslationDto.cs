using Core.Enums;
using Core.Helpers.Validators;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos.URLTranslations;

public class URLTranslationRequestDto
{
    [Required]
    [EnumRangeValidator]
    public SystemTypes SystemType { get; set; }
    [Range(minimum: 1, maximum: 4)]
    public Languages? LanguageId { get; set; }
}

public class URLTranslationResponseDto
{
    public URLTranslationResponseDto()
    {
        this.He = new Collection<KeyValuePair<string, string>>();
        this.En = new Collection<KeyValuePair<string, string>>();
        this.Ar = new Collection<KeyValuePair<string, string>>();
        this.Ru = new Collection<KeyValuePair<string, string>>();
    }

    public ICollection<KeyValuePair<string, string>> He { get; set; }
    public ICollection<KeyValuePair<string, string>> En { get; set; }
    public ICollection<KeyValuePair<string, string>> Ru { get; set; }
    public ICollection<KeyValuePair<string, string>> Ar { get; set; }
}

public class URLTranslationByLanguageResponseDto
{
    public ICollection<KeyValuePair<string, string>> URLTranslations { get; set; }
}
