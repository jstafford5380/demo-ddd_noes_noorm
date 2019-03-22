using System.ComponentModel.DataAnnotations;

namespace Demo.Api.ViewModel
{
    public class PetInfo
    {
        [Required, StringLength(50, MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public int SpeciesId { get; set; }
    }
}
