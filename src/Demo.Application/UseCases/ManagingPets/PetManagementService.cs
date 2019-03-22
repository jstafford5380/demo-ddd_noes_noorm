using System.Threading.Tasks;
using AutoMapper;
using Demo.Application.Infrastructure;
using Demo.Domain.ManagePetContext.Model;
using Demo.Domain.ManagePetContext.Services;
using Demo.Domain.Shared;

namespace Demo.Application.UseCases.ManagingPets
{
    public interface IManagePets
    {
        Task<ApplicationResponse<PetState>> AddPetAsync(string personId, PetState petState);

        Task<ApplicationResponse<PetState>> UpdatePetAsync(string personId, PetState updatedPet);

        Task<ApplicationResponse<PetState>> DeletePetAsync(string personId, string petId);
    }

    /* The APPLICATION SERVICE is responsible for executing the use cases based on feedback from the domain
     * It works with business rules passively but DOES NOT enforce them.
     * */

    public class PetManagementService : IManagePets
    {
        private readonly IPersonRepository _personRepository;
        private readonly IPetManagementService _domainService;
        private readonly IMapper _mapper;

        public PetManagementService(IPersonRepository personRepository, IPetManagementService domainService, IMapper mapper)
        {
            _personRepository = personRepository;
            _domainService = domainService;
            _mapper = mapper;
        }

        public async Task<ApplicationResponse<PetState>> AddPetAsync(string personId, PetState petState)
        {
            // this demonstrates the application layer's responsibility to make decisions based on stuff coming out of the domain
            // this would also be the place where additional actions would take place (e.g. call out to a service bus or web services, etc.)

            var personState = await _personRepository.GetAsync(personId);
            
            if(personState == null)
                return new ApplicationResponse<PetState>(false, ResponseType.EntityNotFound, $"Could not find person {personId}");

            var person = _mapper.Map<Person>(personState);

            var newPet = Pet.Create(petState.Name, new SpeciesId(petState.SpeciesId));
            if (!person.CanAddPet(newPet))
                return new ApplicationResponse<PetState>(false, ResponseType.BusinessRuleViolation, $"Could not add pet (domain rejected)");

            var error = await _domainService.AddPetAsync(person, newPet);
            if (error != null) // we checked earlier so this is an exceptional exception
                throw error;

            var updatedPersonState = _mapper.Map<PersonState>(person);
            await _personRepository.SavePersonAsync(updatedPersonState);

            var resultPet = _mapper.Map<PetState>(newPet);
            return new ApplicationResponse<PetState>(true, ResponseType.Success, resultPet, "ok");
        }

        public async Task<ApplicationResponse<PetState>> UpdatePetAsync(string personId, PetState updatedPet)
        {
            // demonstrate updating an entity through the aggregate root that is not the root itself

            var personState = await _personRepository.GetAsync(personId);
            if (personState == null)
                return new ApplicationResponse<PetState>(false, ResponseType.EntityNotFound, $"Could not find person {personId}");

            var person = _mapper.Map<Person>(personState);

            var ptId = new PetId(updatedPet.PetId);
            if (!person.HasPet(ptId))
                return new ApplicationResponse<PetState>(false, ResponseType.EntityNotFound, $"The customer does not own this pet.");

            var replacementPet = _mapper.Map<Pet>(updatedPet);
            var updateError = person.UpdatePet(replacementPet);

            if(updateError != null)
                return new ApplicationResponse<PetState>(false, ResponseType.BusinessRuleViolation, updateError.Message);

            var updatedPersonState = _mapper.Map<PersonState>(person);
            await _personRepository.SavePersonAsync(updatedPersonState);

            return new ApplicationResponse<PetState>(true, ResponseType.Success, updatedPet, "ok");
        }

        public async Task<ApplicationResponse<PetState>> DeletePetAsync(string personId, string petId)
        {
            var personState = await _personRepository.GetAsync(personId);
            if (personState == null)
                return new ApplicationResponse<PetState>(false, ResponseType.EntityNotFound, $"Could not find person {personId}");

            var person = _mapper.Map<Person>(personState);

            var ptId = new PetId(petId);
            if(!person.HasPet(ptId))
                return new ApplicationResponse<PetState>(false, ResponseType.EntityNotFound, $"The customer does not own this pet or it is already inactive.");

            await _domainService.DeletePetAsync(person, ptId);

            var updatePersonState = _mapper.Map<PersonState>(person);
            await _personRepository.SavePersonAsync(updatePersonState);

            return new ApplicationResponse<PetState>(true, ResponseType.Success, "ok");
        }
    }
}
