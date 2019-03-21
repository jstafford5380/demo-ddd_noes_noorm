using AutoMapper;
using Demo.Domain.Shared;

namespace Demo.MappingProfiles
{
    public class PetIdConverter : IValueConverter<string, PetId>, IValueConverter<PetId, string>
    {
        public PetId Convert(string sourceMember, ResolutionContext context)
        {
            return new PetId(sourceMember);
        }

        public string Convert(PetId sourceMember, ResolutionContext context)
        {
            return sourceMember.Id;
        }
    }
}