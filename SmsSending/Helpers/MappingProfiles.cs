using AutoMapper;
using Core.Entities.SmsExternalService;
using SmsSending.Dtos;

namespace SmsSending.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        _ = CreateMap<MessageInfoDto, MessageInfoEnt>()
            .ForMember(dest => dest.numbers, src => src.MapFrom(p => p.numbers));

        _ = CreateMap<MessageInfoEnt, MessageInfoDto>()
            .ForMember(dest => dest.numbers, src => src.MapFrom(p => p.numbers));
    }
}
