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
        public ErrorEvent AddPet(Person person, Pet pet)
        {
            var result = person.AddPet(pet);
            return result;
        }

        public ErrorEvent DeletePet(Person person, PetId petId)
        {
            var result = person.DeletePet(petId);
            return result;
        }
    }
}
