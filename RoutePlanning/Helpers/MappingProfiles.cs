using AutoMapper;
using Core.Entities.TimeTable;
using RoutePlanning.Dtos;

namespace RoutePlanning.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<RoutePlanningRequest, TrainTimeTableRequest>()
            .ForMember(dest => dest.Date, src => src.MapFrom(p => p.Date.ToString("dd/MM/yyyy")))
            .ForMember(dest => dest.Hours, src => src.MapFrom(p => TimeSpan.Parse(p.Hour)));

        CreateMap<RoutePlanningByTrainNumberRequest, TrainTimeTableByTrainNumberRequest>()
            .ForMember(dest => dest.Date, src => src.MapFrom(p => p.Date.ToString("dd/MM/yyyy")));
    }
}
