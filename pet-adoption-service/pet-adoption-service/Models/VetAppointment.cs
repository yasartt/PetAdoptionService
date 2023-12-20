using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pet_adoption_service.Models
{
    [Table("vet_appointments")]
    public class VetAppointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("appointment_id")]
        public int AppointmentId { get; set; }

        [Required]
        [Column("vet_id")]
        [ForeignKey("Veterinarian")]
        public int VetId { get; set; }

        [Required]
        [Column("pet_id")]
        [ForeignKey("Pet")]
        public int PetId { get; set; }

        [Required]
        [Column("appointment_date")]
        public DateTime? AppointmentDate { get; set; }

        // Navigation properties for related entities
        public Veterinarian? Veterinarian { get; set; }
        public Pet? Pet { get; set; }
    }
}