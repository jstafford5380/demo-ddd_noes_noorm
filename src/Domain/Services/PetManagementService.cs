using System.Threading.Tasks;
using Domain.Model;
using Domain.Model.ManagePet;
using Domain.Shared;

namespace Domain.Services
{
    public interface IPetManagementService
    {
        Task AddPetAsync(Person person, Pet pet);
        Task DeletePetAsync(Person person, PetId petId);
    }

    public class PetManagementService : IPetManagementService
    {
        private readonly IPersonStore _personStore;

        public PetManagementService(IPersonStore personStore)
        {
            _personStore = personStore;
        }

        public async Task AddPetAsync(Person person, Pet pet)
        {
            person.AddPet(pet);
            await _personStore.SavePersonAsync(person);
        }

        public async Task DeletePetAsync(Person person, PetId petId)
        {
            person.DeletePet(petId);
            await _personStore.SavePersonAsync(person);
        }
    }
}
