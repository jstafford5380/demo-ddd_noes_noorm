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
    public class Person
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
            return person;
        }

        public bool CanAddPet(Pet petToAdd) => _pets.All(p => !p.Equals(petToAdd));

        public bool HasPet(PetId petId) => _pets.Any(p => p.PetId.Equals(petId));

        public Exception AddPet(Pet petToAdd)
        {
            if(Pets.Any(p => p.Equals(petToAdd)))
                return new InvalidOperationException("Cannot add a pet that is already loaded against the person.");

            _pets.Add(petToAdd);
            return null;
        }

        public Exception DeletePet(PetId petId)
        {
            if(!_pets.Any(p => p.PetId.Equals(petId)))
                return new InvalidOperationException("Cannot delete a pet that does not exist.");

            _pets.RemoveAll(p => p.PetId.Equals(petId));
            return null;
        }

        public Exception UpdatePet(Pet updatedPet)
        {
            var petToUpdate = _pets.SingleOrDefault(p => p.PetId.Equals(updatedPet.PetId));

            if(petToUpdate == null)
                return new InvalidOperationException("Cannot update a pet that does not exist.");

            if(!petToUpdate.IsActive)
                return new InvalidOperationException("Cannot update an inactive pet.");

            if(!petToUpdate.SpeciesId.Equals(updatedPet.SpeciesId))
                return new InvalidOperationException("Cannot change the species of a pet.");

            var duplicatePet = _pets.SingleOrDefault(p => !p.PetId.Equals(updatedPet.PetId) && p.Equals(updatedPet));

            if (!updatedPet.Name.Equals(petToUpdate.Name, StringComparison.OrdinalIgnoreCase) && duplicatePet != null && duplicatePet.IsActive)
                return new InvalidOperationException("Updating this pet would make it identical to another active pet that is already loaded against the customer.");

            // if we made it this far then go ahead and replace the pet.
            _pets.RemoveAll(p => p.PetId.Equals(updatedPet.PetId));
            AddPet(updatedPet);

            return null;
        }
    }
}
