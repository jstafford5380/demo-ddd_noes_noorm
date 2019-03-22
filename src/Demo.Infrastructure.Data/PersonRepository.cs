using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Demo.Application.Infrastructure;
using Demo.Domain.ManagePetContext.Model;

namespace Demo.Infrastructure.Data
{
    public class PersonRepository : IPersonRepository
    {
        private readonly StaticStorage<string, PersonState> _staticStorage;
        private readonly IMapper _mapper;
        
        public PersonRepository(StaticStorage<string, PersonState> staticStorage, IMapper mapper)
        {
            _staticStorage = staticStorage;
            _mapper = mapper;
        }

        public Task<string> SavePersonAsync(Person person)
        {
            var state = _mapper.Map<PersonState>(person);
            if(!_staticStorage.ContainsKey(state.PersonId))
                _staticStorage.Add(state.PersonId, null);

            _staticStorage[state.PersonId] = state;

            return Task.FromResult(state.PersonId);
        }

        public Task<Person> GetAsync(string id)
        {
            if (!_staticStorage.ContainsKey(id))
                return null;

            var state = _staticStorage[id];
            var person = _mapper.Map<Person>(state);
            return Task.FromResult(person);
        }

        public async Task Setup()
        {
            if (_staticStorage.ContainsKey("person.foo"))
                return;

            var person = new PersonState { PersonId = "person.foo", Pets = new List<PetState>() };
            var newPerson = _mapper.Map<Person>(person);
            await SavePersonAsync(newPerson).ConfigureAwait(false);
        }
    }


    public class StaticStorage<T, K> : Dictionary<T, K>
    {

    }
}