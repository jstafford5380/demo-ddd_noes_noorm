using System.Threading.Tasks;

namespace Demo.Application.Infrastructure
{
    public interface IPersonRepository
    {
        Task<string> SavePersonAsync(PersonState person);

        Task<PersonState> GetAsync(string id);
    }
}