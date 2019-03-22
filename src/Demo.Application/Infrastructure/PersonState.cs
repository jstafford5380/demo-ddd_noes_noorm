using System.Collections.Generic;

namespace Demo.Application.Infrastructure
{
    public class PersonState
    {
        public string PersonId { get; set; }

        public List<PetState> Pets { get; set; }
    }
}