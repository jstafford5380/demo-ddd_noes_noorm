using System.Threading.Tasks;
using Demo.Domain.Model.ManagePet;
using Demo.Domain.Shared;

namespace Demo.Domain.Model
{
    public interface IPersonStore
    {
        Task<PersonId> SavePersonAsync(Person person);

        Task<Person> GetAsync(PersonId id);
    }
}