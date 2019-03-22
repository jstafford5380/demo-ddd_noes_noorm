using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Demo.Application.Infrastructure;
using Demo.Domain.ManagePetContext.Model;

namespace Demo.Infrastructure.Data
{
    public class PersonRepository : IPersonRepository
    {
        private readonly IMapper _mapper;
        private readonly Dictionary<string, PersonState> _fakeDatabase = new Dictionary<string, PersonState>();

        public PersonRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Task<string> SavePersonAsync(Person person)
        {
            var state = _mapper.Map<PersonState>(person);
            if(!_fakeDatabase.ContainsKey(state.PersonId))
                _fakeDatabase.Add(state.PersonId, null);

            _fakeDatabase[state.PersonId] = state;

            return Task.FromResult(state.PersonId);
        }

        public Task<Person> GetAsync(string id)
        {
            if (!_fakeDatabase.ContainsKey(id))
                return null;

            var state = _fakeDatabase[id];
            var person = _mapper.Map<Person>(state);
            return Task.FromResult(person);
        }
    }
}