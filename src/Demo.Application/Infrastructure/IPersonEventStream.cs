using System.Threading.Tasks;
using Demo.Domain.ManagePetContext.Model;

namespace Demo.Application.Infrastructure
{
    public interface IPersonEventStream
    {
        Task SaveAsync(Person person);
    }
}
