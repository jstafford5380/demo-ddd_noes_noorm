using System;
using System.Threading.Tasks;
using Demo.Domain.ManagePetContext.Model;
using Demo.Domain.Shared;

namespace Demo.Domain.ManagePetContext.Services
{

    /* DOMAIN SERVICES simply enforce business rules between entities when you have a situation
     * where enforcing the case within a single entity would compromise cohesion. Domain services
     * mediate between entities.
     * This doesn't show a good use case; it is just here to show structure
     */

    public class PetManagementService : IPetManagementService
    {
        public Task<Exception> AddPetAsync(Person person, Pet pet)
        {
            var error = person.AddPet(pet);
            return Task.FromResult(error);
        }

        public Task<Exception> DeletePetAsync(Person person, PetId petId)
        {
            var error = person.DeletePet(petId);
            return Task.FromResult(error);
        }
    }
}
