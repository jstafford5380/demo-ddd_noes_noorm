using Demo.Domain.ManagePetContext.Model;
using Demo.Domain.Shared;

namespace Demo.Domain.ManagePetContext.Services
{
    public interface IPetManagementService
    {
        ErrorEvent AddPet(Person person, Pet pet);

        ErrorEvent DeletePet(Person person, PetId petId);
    }
}