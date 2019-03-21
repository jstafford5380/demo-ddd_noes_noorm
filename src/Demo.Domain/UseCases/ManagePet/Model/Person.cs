using System;
using System.Collections.Generic;
using System.Linq;
using Demo.Domain.Shared;

// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace Demo.Domain.UseCases.ManagePet.Model
{
    /// <summary>
    /// Aggregate root for the Person-Pet aggregate
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
    }
}
