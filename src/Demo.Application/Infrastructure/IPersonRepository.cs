using System.Threading.Tasks;
using Demo.Domain.ManagePetContext.Model;

namespace Demo.Application.Infrastructure
{
    public interface IPersonRepository
    {
        Task<string> SavePersonAsync(Person person);

        Task<Person> GetAsync(string id);
    }
}