using System;
using System.Threading.Tasks;
using Demo.Domain.ManagePetContext.Model;
using Demo.Domain.Shared;

namespace Demo.Domain.ManagePetContext.Services
{
    public interface IPetManagementService
    {
        Task<Exception> AddPetAsync(Person person, Pet pet);

        Task<Exception> DeletePetAsync(Person person, PetId petId);
    }
}