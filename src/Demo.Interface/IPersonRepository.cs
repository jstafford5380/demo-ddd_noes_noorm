using System.Threading.Tasks;

namespace Demo.Interface
{
    public interface IPersonRepository
    {
        Task<string> SavePersonAsync(PersonState person);

        Task<PersonState> GetAsync(string id);
    }
}