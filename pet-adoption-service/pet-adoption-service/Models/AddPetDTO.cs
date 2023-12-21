using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace pet_adoption_service.Models
{
    public class AddPetDTO
    {
        public string? Breed { get; set; }
        public int? Age { get; set; }
        public string? Name { get; set; }
        public string? Status { get; set; }
        public string? Gender { get; set; }
        public int? IsAvailable { get; set; }
        public string? photo_id { get; set; }
        public int shelterId { get; set; }

    }
}