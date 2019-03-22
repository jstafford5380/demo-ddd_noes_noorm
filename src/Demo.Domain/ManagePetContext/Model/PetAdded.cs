using Demo.Domain.Shared;

namespace Demo.Domain.ManagePetContext.Model
{
    public class PetAdded : DomainEvent
    {
        public Pet NewPet { get; set; }

        public PetAdded() { }

        public PetAdded(Pet added)
        {
            NewPet = added;
        }
    }
}