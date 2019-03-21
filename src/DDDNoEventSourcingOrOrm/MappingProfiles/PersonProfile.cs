using AutoMapper;
using Domain.Model.ManagePet;
using Interfaces;

namespace DDDNoEventSourcingOrOrm.MappingProfiles
{
    public class PersonProfile : Profile
    {
        public PersonProfile()
        {
            CreateMap<PersonState, Person>()
                .ForMember(dest => dest.PersonId, o =>
                {
                    o.MapFrom(src => src.PersonId);
                    o.ConvertUsing(new PersonIdConverter(), src => src.PersonId);
                })
                .ForMember("_pets", expression => expression.MapFrom(src => src.Pets));
        }
    }
}