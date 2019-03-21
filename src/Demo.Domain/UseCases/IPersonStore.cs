using System.Threading.Tasks;
using Demo.Domain.Shared;
using Demo.Domain.UseCases.ManagePet;
using Demo.Domain.UseCases.ManagePet.Model;

namespace Demo.Domain.UseCases
{
    public interface IPersonStore
    {
        Task<PersonId> SavePersonAsync(Person person);

        Task<Person> GetAsync(PersonId id);
    }
}