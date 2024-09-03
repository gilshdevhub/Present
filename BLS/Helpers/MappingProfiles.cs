using AutoMapper;
using BLS.Dtos;
using Core.Entities.MotUpdates;

namespace BLS.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<MotUpdateRequestDto, MotUpdateRequest>();
    }
}
