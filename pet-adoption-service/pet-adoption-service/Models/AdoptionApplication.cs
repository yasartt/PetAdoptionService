using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pet_adoption_service.Models
{
    [Table("adoption_application")]
    public class AdoptionApplication
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("adoption_application_id")]
        public int AdoptionApplicationId { get; set; }

        [Required]
        [ForeignKey("PetAdopter")]
        [Column("pet_adopter_id")]
        public int PetAdopterId { get; set; }

        [Required]
        [ForeignKey("Shelter")]
        [Column("shelter_id")]
        public int ShelterId { get; set; }

        [Required]
        [ForeignKey("Pet")]
        [Column("pet_id")]
        public int PetId { get; set; }

        [Required]
        [Column("application_date")]
        public DateTime ApplicationDate { get; set; }

        [Required]
        [Column("status")]
        public int? Status { get; set; }

        // Navigation properties for related entities
        public PetAdopter? PetAdopter { get; set; }

        public Shelter? Shelter { get; set; }

        public Pet? Pet { get; set; }
    }
}
