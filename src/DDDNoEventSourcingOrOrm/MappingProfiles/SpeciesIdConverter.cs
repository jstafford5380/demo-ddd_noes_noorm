using AutoMapper;
using Domain.Shared;

namespace DDDNoEventSourcingOrOrm.MappingProfiles
{
    public class SpeciesIdConverter : IValueConverter<int, SpeciesId>, IValueConverter<SpeciesId, int>
    {
        public SpeciesId Convert(int sourceMember, ResolutionContext context)
        {
            return new SpeciesId(sourceMember);
        }

        public int Convert(SpeciesId sourceMember, ResolutionContext context)
        {
            return sourceMember.Id;
        }
    }
}