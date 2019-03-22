using AutoMapper;
using Demo.Application;
using Demo.Application.Infrastructure;
using Demo.Domain.ManagePetContext.Model;
using Demo.ViewModel;

namespace Demo.MappingProfiles
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

            CreateMap<Person, PersonState>()
                .ForMember(dest => dest.PersonId, o =>
                {
                    o.MapFrom(src => src.PersonId);
                    o.ConvertUsing(new PersonIdConverter(), src => src.PersonId);
                });
        }
    }

    public class NewPetViewModelProfile : Profile
    {
        public NewPetViewModelProfile()
        {
            CreateMap<NewPetInfo, PetState>();
        }
    }
}