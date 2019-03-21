using AutoMapper;
using Domain.Model.ManagePet;
using Interfaces;

namespace DDDNoEventSourcingOrOrm.MappingProfiles
{
    public class PetProfile : Profile
    {
        public PetProfile()
        {
            CreateMap<PetState, Pet>()
                .ForMember(dest => dest.PetId, o =>
                {
                    o.MapFrom(src => src.PetId);
                    o.ConvertUsing(new PetIdConverter(), state => state.PetId);
                })
                .ForMember(dest => dest.SpeciesId, o =>
                {
                    o.MapFrom(src => src.SpeciesId);
                    o.ConvertUsing(new SpeciesIdConverter(), state => state.SpeciesId);
                });
        }
    }
}