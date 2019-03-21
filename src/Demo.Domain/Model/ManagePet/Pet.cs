using System;
using Demo.Domain.Shared;

// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace Demo.Domain.Model.ManagePet
{
    public class Pet
    {
        public PetId PetId { get; private set; }

        public string Name { get; private set; }

        public SpeciesId SpeciesId { get; private set; }

        public bool IsActive { get; private set; }

        private Pet() { }

        public static Pet Create(string name, SpeciesId speciesId)
        {
            var pet = new Pet
            {
                PetId = new PetId(Guid.NewGuid().ToString()),
                SpeciesId = speciesId,
                Name = name
            };

            return pet;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Pet);
        }

        protected bool Equals(Pet other)
        {
            return other == null ||
                   PetId.Equals(other.PetId) ||
                   (string.Equals(Name, other.Name) 
                    && SpeciesId.Equals(other.SpeciesId)
                    && IsActive == other.IsActive);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = PetId.GetHashCode();
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ SpeciesId.GetHashCode();
                hashCode = (hashCode * 397) ^ IsActive.GetHashCode();
                return hashCode;
            }
        }
    }
}