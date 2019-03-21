using System.Collections.Generic;

namespace Interfaces
{
    public class PersonState
    {
        public string PersonId { get; set; }

        public List<PetState> Pets { get; set; }
    }
}