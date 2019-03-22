using System;
using System.Collections.Generic;
using System.Linq;
using Demo.Domain.Shared;

// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace Demo.Domain.ManagePetContext.Model
{
    /// <summary>
    /// Aggregate root for the Person-Pet aggregate in the "Manage Pet" bounded context
    /// </summary>
    public class Person : AggregateRoot
    {
        private List<Pet> _pets = new List<Pet>();

        public IReadOnlyCollection<Pet> Pets => _pets;

        public PersonId PersonId { get; private set; }
        
        private Person() { }

        public static Person Create()
        {
            var person = new Person
            {
                PersonId = new PersonId(Guid.NewGuid().ToString())
            };
            person.Id = person.PersonId.Id;
            return person;
        }

        public bool CanAddPet(Pet petToAdd) => _pets.All(p => !p.Equals(petToAdd));

        public bool HasPet(PetId petId) => _pets.Any(p => p.PetId.Equals(petId));

        public ErrorEvent AddPet(Pet petToAdd)
        {
            if (Pets.Any(p => p.Equals(petToAdd)))
                return new ErrorEvent(1234, new InvalidOperationException("Cannot add a pet that is already loaded against the person."));

            _pets.Add(petToAdd);
            Raise<PetAdded>(added => added.NewPet = petToAdd);
            return null;
        }

        public ErrorEvent DeletePet(PetId petId)
        {
            if (!_pets.Any(p => p.PetId.Equals(petId)))
                return new ErrorEvent(1234, new InvalidOperationException("Cannot delete a pet that does not exist."));

            _pets.RemoveAll(p => p.PetId.Equals(petId));
            Raise<PetDeleted>(deleted => deleted.PetId = petId);
            return null;
        }

        public ErrorEvent UpdatePet(Pet updatedPet)
        {
            var petToUpdate = _pets.SingleOrDefault(p => p.PetId.Equals(updatedPet.PetId));

            if (petToUpdate == null)
                return new ErrorEvent(1234, new InvalidOperationException("Cannot update a pet that does not exist."));

            if (!petToUpdate.IsActive)
                return new ErrorEvent(1234, new InvalidOperationException("Cannot update an inactive pet."));

            if(!petToUpdate.SpeciesId.Equals(updatedPet.SpeciesId))
                return new ErrorEvent(1234, new InvalidOperationException("Cannot change the species of a pet."));

            var duplicatePet = _pets.SingleOrDefault(p => !p.PetId.Equals(updatedPet.PetId) && p.Equals(updatedPet));

            if (!updatedPet.Name.Equals(petToUpdate.Name, StringComparison.OrdinalIgnoreCase) && duplicatePet != null && duplicatePet.IsActive)
                return new ErrorEvent(1234, new InvalidOperationException("Updating this pet would make it identical to another active pet that is already loaded against the customer."));

            // if we made it this far then go ahead and replace the pet.
            _pets.RemoveAll(p => p.PetId.Equals(updatedPet.PetId));
            AddPet(updatedPet);

            Raise<PetUpdated>(updated => updated.UpdatedPet = updatedPet);
            return null;
        }
    }
}
