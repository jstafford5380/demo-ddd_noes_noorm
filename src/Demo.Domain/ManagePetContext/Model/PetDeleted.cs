using Demo.Domain.Shared;

namespace Demo.Domain.ManagePetContext.Model
{
    public class PetDeleted : DomainEvent
    {
        public PetId PetId { get; set; }

        public PetDeleted() { }

        public PetDeleted(PetId deleted)
        {
            PetId = deleted;
        }
    }
}