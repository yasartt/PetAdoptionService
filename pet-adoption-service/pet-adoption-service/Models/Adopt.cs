using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pet_adoption_service.Models
{
    [Table("adopt")]
    public class Adopt
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int adopt_id { get; set; }

        [Required]
        [ForeignKey("PetAdopter")]
        [Column("pet_adopter_id")]
        public int PetAdopterId { get; set; }

        [Required]
        [ForeignKey("Pet")]
        [Column("pet_id")]
        public int PetId { get; set; }

        [Required]
        public DateTime start_date { get; set; }

        public DateTime? end_date { get; set; }

        [Required]
        public bool is_Current { get; set; }

        public PetAdopter? PetAdopter { get; set; }

        public Pet? Pet { get; set; }
    }
}
