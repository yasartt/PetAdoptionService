using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pet_adoption_service.Models
{
    [Table("pet_care_info")]
    public class PetCareInfo
    {
        [Key]
        [Column("pet_care_id")]
        public int CareId { get; set;}

        [Required]
        [ForeignKey("Pet")]
        [Column("pet_id")]
        public int PetId { get; set; }

        [Required]
        [Column("update_date")]
        public DateTime UpdateTime { get; set;}

        [Required]
        [MaxLength(1000)]
        public string? explanation { get; set;}

        public Pet? Pet { get; set; }

    }
}
