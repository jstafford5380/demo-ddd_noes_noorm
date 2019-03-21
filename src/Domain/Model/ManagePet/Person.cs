using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Shared;
using Interfaces;

namespace Domain.Model.ManagePet
{
    /// <summary>
    /// Aggregate root for the Person-Pet aggregate
    /// </summary>
    public class Person
    {
        public PersonId PersonId { get; private set; }

        private List<Pet> _pets = new List<Pet>();
        public IReadOnlyCollection<Pet> Pets => _pets;

        private Person() { }

        public static Person Create()
        {
            var person = new Person
            {
                PersonId = new PersonId(Guid.NewGuid().ToString())
            };
            return person;
        }

        public void AddPet(Pet petToAdd)
        {
            if(Pets.Any(p => p.Equals(petToAdd)))
                throw new InvalidOperationException("Cannot add a pet that is already loaded against the person.");

            _pets.Add(petToAdd);
        }

        public void DeletePet(PetId petId)
        {
            if(!_pets.Any(p => p.PetId.Equals(petId)))
                throw new InvalidOperationException("Cannot delete a pet that does not exist.");

            _pets.RemoveAll(p => p.PetId.Equals(petId));
        }

        public PersonState GetState()
        {
            return new PersonState
            {
                PersonId = PersonId.Id,
                Pets = Pets.Select(p => p.GetState()).ToList()
            };
        }

        public static Person Load(PersonState state)
        {
            var person = new Person
            {
                PersonId = new PersonId(state.PersonId),
                _pets = state.Pets.Select(Pet.Load).ToList()
            };

            return person;
        }
    }
}
