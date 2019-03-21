using System.Collections.Generic;
using System.Threading.Tasks;
using Interfaces;

namespace DDDNoEventSourcingOrOrm.Infrastructure
{
    public class PersonRepository : IPersonRepository
    {
        private readonly Dictionary<string, PersonState> _fakeDatabase = new Dictionary<string, PersonState>();

        public Task<string> SavePersonAsync(PersonState state)
        {
            if(!_fakeDatabase.ContainsKey(state.PersonId))
                _fakeDatabase.Add(state.PersonId, null);

            _fakeDatabase[state.PersonId] = state;

            return Task.FromResult(state.PersonId);
        }

        public Task<PersonState> GetAsync(string id)
        {
            if (!_fakeDatabase.ContainsKey(id))
                return null;

            var state = _fakeDatabase[id];
            return Task.FromResult(state);
        }
    }

    
}