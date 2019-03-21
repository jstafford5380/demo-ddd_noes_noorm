using System.Collections.Generic;

namespace Demo.Interface
{
    public class PersonState
    {
        public string PersonId { get; set; }

        public List<PetState> Pets { get; set; }
    }
}