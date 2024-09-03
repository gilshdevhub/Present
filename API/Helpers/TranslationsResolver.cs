using API.Dtos.Translations;
using AutoMapper;
using Core.Entities.Translation;
using Core.Enums;

namespace API.Helpers;

public class TranslationsResolver : IValueResolver<IEnumerable<Translation>, TranslationByLanguageResponseDto, ICollection<KeyValuePair<string, string>>>
{
    public ICollection<KeyValuePair<string, string>> Resolve(IEnumerable<Translation> source, TranslationByLanguageResponseDto destination, ICollection<KeyValuePair<string, string>> destMember, ResolutionContext context)
    {
        Languages languageId = (Languages)Enum.Parse(typeof(Languages), context.Items["languageId"].ToString());

        if (languageId == Languages.Hebrew)
        {
            destination.Translations = source.Select(p => new KeyValuePair<string, string>(p.Key, p.Hebrew)).ToArray();
        }
        else if (languageId == Languages.English)
        {
            destination.Translations = source.Select(p => new KeyValuePair<string, string>(p.Key, p.English)).ToArray();
        }
        else if (languageId == Languages.Arabic)
        {
            destination.Translations = source.Select(p => new KeyValuePair<string, string>(p.Key, p.Arabic)).ToArray();
        }
        else if (languageId == Languages.Russian)
        {
            destination.Translations = source.Select(p => new KeyValuePair<string, string>(p.Key, p.Russian)).ToArray();
        }

        return destination.Translations;
    }
}
