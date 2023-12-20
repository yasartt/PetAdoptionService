using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pet_adoption_service.Models
{
    public class HealthRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("health_record_id")]
        [Required]
        public int HealthRecordId { get; set; }

        [Required]
        [ForeignKey("Pet")]
        [Column("pet_id")]
        public int PetId { get; set; }

        [Required]
        [ForeignKey("Veterinarian")]
        [Column("veterinarian_id")]
        public int VetId { get; set; }

        [Required]
        public bool isHealthy { get; set; }

        [Required]
        public DateTime health_record_date { get; set; }

        [MaxLength(1000)]
        public string? notes { get; set; }

        // Navigation properties for related entities
        public Veterinarian? Veterinarian { get; set; }

        public Pet? Pet { get; set; }

    }
}
