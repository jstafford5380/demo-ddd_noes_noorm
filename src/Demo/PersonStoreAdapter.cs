using System.Threading.Tasks;
using AutoMapper;
using Demo.Domain.Shared;
using Demo.Domain.UseCases;
using Demo.Domain.UseCases.ManagePet;
using Demo.Domain.UseCases.ManagePet.Model;
using Demo.Interface;

namespace Demo
{
    public class PersonStoreAdapter : IPersonStore
    {
        private readonly IPersonRepository _repository;
        private readonly IMapper _mapper;

        public PersonStoreAdapter(IPersonRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PersonId> SavePersonAsync(Person person)
        {
            var state = _mapper.Map<PersonState>(person);
            await _repository.SavePersonAsync(state);
            return person.PersonId;
        }

        public async Task<Person> GetAsync(PersonId id)
        {
            var state = await _repository.GetAsync(id.Id);
            return _mapper.Map<Person>(state);
        }
    }
}
