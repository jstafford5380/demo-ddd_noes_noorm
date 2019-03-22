using AutoMapper;
using Demo.Application;
using Demo.Domain.ManagePetContext.Model;

namespace Demo.MappingProfiles
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

            CreateMap<Pet, PetState>()
                .ForMember(dest => dest.PetId, o =>
                {
                    o.MapFrom(src => src.PetId);
                    o.ConvertUsing(new PetIdConverter(), src => src.PetId);
                })
                .ForMember(dest => dest.SpeciesId, o =>
                {
                    o.MapFrom(src => src.SpeciesId);
                    o.ConvertUsing(new SpeciesIdConverter(), src => src.SpeciesId);
                });
        }
    }
}