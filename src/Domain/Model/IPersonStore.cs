using System.Threading.Tasks;
using Domain.Model.ManagePet;
using Domain.Shared;

namespace Domain.Model
{
    public interface IPersonStore
    {
        Task<PersonId> SavePersonAsync(Person person);

        Task<Person> GetAsync(PersonId id);
    }
}