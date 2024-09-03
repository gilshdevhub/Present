using Core.Enums;
using Core.Helpers.Validators;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos.Translations;

public class TranslationRequestDto
{
    [Required]
    [EnumRangeValidator]
    public SystemTypes SystemType { get; set; }
    [Range(minimum: 1, maximum: 4)]
    public Languages? LanguageId { get; set; }
}

public class TranslationResponseDto
{
    public TranslationResponseDto()
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

public class TranslationByLanguageResponseDto
{
    public ICollection<KeyValuePair<string, string>> Translations { get; set; }
}
