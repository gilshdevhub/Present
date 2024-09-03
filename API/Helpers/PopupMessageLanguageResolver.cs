using API.Dtos.PopupMessages;
using AutoMapper;
using Core.Entities.AppMessages;
using Core.Enums;

namespace API.Helpers;

public class PopupMessageLanguageResolver : IValueResolver<PopUpMessages, PopupMessagesResponseDto, string>
{
    public string Resolve(PopUpMessages source, PopupMessagesResponseDto destination, string destMember, ResolutionContext context)
    {
        Languages langiage = (Languages)context.Items["languageId"];

        if (langiage == Languages.Hebrew)
            return source.MessageBodyHebrew;
        else if (langiage == Languages.English)
            return source.MessageBodyEnglish;
        else if (langiage == Languages.Arabic)
            return source.MessageBodyArabic;
        else if (langiage == Languages.Russian)
            return source.MessageBodyRussian;
        else return string.Empty;
    }
}

public class PopupMessageStationLanguageResolver : IValueResolver<PopUpMessages, PopupMessagesWithStationsResponseDto, string>
{
    public string Resolve(PopUpMessages source, PopupMessagesWithStationsResponseDto destination, string destMember, ResolutionContext context)
    {
        Languages langiage = (Languages)context.Items["languageId"];

        if (langiage == Languages.Hebrew)
            return source.MessageBodyHebrew;
        else if (langiage == Languages.English)
            return source.MessageBodyEnglish;
        else if (langiage == Languages.Arabic)
            return source.MessageBodyArabic;
        else if (langiage == Languages.Russian)
            return source.MessageBodyRussian;
        else return string.Empty;
    }
}


