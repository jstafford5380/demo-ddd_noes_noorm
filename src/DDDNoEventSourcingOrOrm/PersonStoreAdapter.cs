using System.Threading.Tasks;
using Domain.Model;
using Domain.Model.ManagePet;
using Domain.Shared;
using Interfaces;

namespace DDDNoEventSourcingOrOrm
{
    public class PersonStoreAdapter : IPersonStore
    {
        private readonly IPersonRepository _repository;

        public PersonStoreAdapter(IPersonRepository repository)
        {
            _repository = repository;
        }

        public async Task<PersonId> SavePersonAsync(Person person)
        {
            await _repository.SavePersonAsync(person.GetState());
            return person.PersonId;
        }

        public async Task<Person> GetAsync(PersonId id)
        {
            var state = await _repository.GetAsync(id.Id);
            return Person.Load(state);
        }
    }
}
