using API.Dtos.PopupMessages;
using AutoMapper;
using Core.Entities.AppMessages;
using Core.Enums;

namespace API.Helpers;

public class PopupTitleLanguageResolver : IValueResolver<PopUpMessages, PopupMessagesResponseDto, string>
{
    public string Resolve(PopUpMessages source, PopupMessagesResponseDto destination, string destMember, ResolutionContext context)
    {
        Languages langiage = (Languages)context.Items["languageId"];

        if (langiage == Languages.Hebrew)
            return source.TitleHebrew;
        else if (langiage == Languages.English)
            return source.TitleEnglish;
        else if (langiage == Languages.Arabic)
            return source.TitleArabic;
        else if (langiage == Languages.Russian)
            return source.TitleRussian;
        else return string.Empty;
    }
}

public class PopupTitleStationLanguageResolver : IValueResolver<PopUpMessages, PopupMessagesWithStationsResponseDto, string>
{
    public string Resolve(PopUpMessages source, PopupMessagesWithStationsResponseDto destination, string destMember, ResolutionContext context)
    {
        Languages langiage = (Languages)context.Items["languageId"];

        if (langiage == Languages.Hebrew)
            return source.TitleHebrew;
        else if (langiage == Languages.English)
            return source.TitleEnglish;
        else if (langiage == Languages.Arabic)
            return source.TitleArabic;
        else if (langiage == Languages.Russian)
            return source.TitleRussian;
        else return string.Empty;
    }
}
