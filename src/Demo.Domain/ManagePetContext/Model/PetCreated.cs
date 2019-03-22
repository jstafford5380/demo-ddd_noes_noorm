using Demo.Domain.Shared;

namespace Demo.Domain.ManagePetContext.Model
{
    public class PetCreated : DomainEvent
    {
        public Pet Created { get; set; }
    }
}