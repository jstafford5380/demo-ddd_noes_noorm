using System.Threading.Tasks;

namespace Interfaces
{
    public interface IPersonRepository
    {
        Task<string> SavePersonAsync(PersonState person);

        Task<PersonState> GetAsync(string id);
    }
}