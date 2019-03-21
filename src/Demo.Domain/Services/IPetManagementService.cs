using System.Threading.Tasks;
using Demo.Domain.Model.ManagePet;
using Demo.Domain.Shared;

namespace Demo.Domain.Services
{
    public interface IPetManagementService
    {
        Task AddPetAsync(Person person, Pet pet);
        Task DeletePetAsync(Person person, PetId petId);
    }
}