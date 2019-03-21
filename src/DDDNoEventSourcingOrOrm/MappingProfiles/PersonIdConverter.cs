using AutoMapper;
using Domain.Shared;

namespace DDDNoEventSourcingOrOrm.MappingProfiles
{
    public class PersonIdConverter : IValueConverter<string, PersonId>, IValueConverter<PersonId, string>
    {
        public PersonId Convert(string sourceMember, ResolutionContext context)
        {
            return new PersonId(sourceMember);
        }

        public string Convert(PersonId sourceMember, ResolutionContext context)
        {
            return sourceMember.Id;
        }
    }
}