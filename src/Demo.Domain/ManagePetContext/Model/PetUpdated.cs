using Demo.Domain.Shared;

namespace Demo.Domain.ManagePetContext.Model
{
    public class PetUpdated : DomainEvent
    {
        public Pet UpdatedPet { get; set; }

        public PetUpdated(Pet updatedPet)
        {
            UpdatedPet = updatedPet;
        }
    }
}