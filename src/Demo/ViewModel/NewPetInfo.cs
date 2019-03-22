using System.ComponentModel.DataAnnotations;

namespace Demo.ViewModel
{
    public class NewPetInfo
    {
        [Required, MinLength(1), MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public int SpeciesId { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
