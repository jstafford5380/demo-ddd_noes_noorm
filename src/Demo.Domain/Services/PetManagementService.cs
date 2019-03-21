using System.Diagnostics;
using System.Threading.Tasks;
using Demo.Domain.Model;
using Demo.Domain.Model.ManagePet;
using Demo.Domain.Shared;

namespace Demo.Domain.Services
{
    public class PetManagementService : IPetManagementService
    {
        private readonly IPersonStore _personStore;

        public PetManagementService(IPersonStore personStore)
        {
            _personStore = personStore;
        }

        public async Task AddPetAsync(Person person, Pet pet)
        {
            // example of the domain service deciding to throw
            var error = person.AddPet(pet);
            if (error != null)
                throw error;

            await _personStore.SavePersonAsync(person);
        }

        public async Task DeletePetAsync(Person person, PetId petId)
        {
            // example of the domain service deciding not to throw
            var error = person.DeletePet(petId);
            if(error != null)
                Trace.TraceError(error.Message);

            await _personStore.SavePersonAsync(person);
        }
    }
}
