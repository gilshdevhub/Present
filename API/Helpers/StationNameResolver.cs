using API.Dtos;
using AutoMapper;
using Core.Entities.Vouchers;

namespace API.Helpers;

public class StationNameResolver : IValueResolver<Station, StationResponseDto, string>
{
    public string Resolve(Station source, StationResponseDto destination, string destMember, ResolutionContext context)
    {
        int langiageId = (int)context.Items["languageId"];

        if (langiageId == 1)
            return source.HebrewName;
        else if (langiageId == 2)
            return source.EnglishName;
        else if (langiageId == 3)
            return source.ArabicName;
        else if (langiageId == 4)
            return source.RussianName;
        else return string.Empty;
    }
}
