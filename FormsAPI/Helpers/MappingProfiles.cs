using AutoMapper;
using Core.Entities.Forms;
using FormsAPI.Dtos;
using System.Collections.Generic;

namespace FormsAPI.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<LostAndFoundDto, LostAndFound>();
            CreateMap<IEnumerable<LostAndFoundDto>, IEnumerable<LostAndFound>>();
            CreateMap<PhotoShootDto, PhotoShoot>();
            CreateMap<IEnumerable<PhotoShootDto>, IEnumerable<PhotoShoot>>();
            CreateMap<DamageCompansationDto, DamageCompansation>();
            CreateMap<IEnumerable<DamageCompansationDto>, IEnumerable<DamageCompansation>>();
            CreateMap<ContactFormDto, ContactForm>();
            CreateMap<IEnumerable<ContactFormDto>, IEnumerable<ContactForm>>();
            CreateMap<CompansationFormDto, CompansationForm>();
            CreateMap<IEnumerable<CompansationFormDto>, IEnumerable<CompansationForm>>();
            CreateMap<BasicFormDto, BasicForm>();
            CreateMap<IEnumerable<BasicFormDto>, IEnumerable<BasicForm>>();

            CreateMap<LostAndFound,LostAndFoundDto >();
            CreateMap<PhotoShoot,PhotoShootDto>();
            CreateMap<DamageCompansation,DamageCompansationDto>();
            CreateMap<ContactForm,ContactFormDto>();
            CreateMap<CompansationForm,CompansationFormDto>();
        }
    }
}
