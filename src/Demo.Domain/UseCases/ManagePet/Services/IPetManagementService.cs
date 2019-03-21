using System.Threading.Tasks;
using Demo.Domain.Shared;
using Demo.Domain.UseCases.ManagePet.Model;

namespace Demo.Domain.UseCases.ManagePet.Services
{
    public interface IPetManagementService
    {
        Task AddPetAsync(Person person, Pet pet);
        Task DeletePetAsync(Person person, PetId petId);
    }
}