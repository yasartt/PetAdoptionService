using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pet_adoption_service.Models
{
    [Table("shelter_appointments")]
    public class ShelterAppointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("appointment_id")]
        public int AppointmentId { get; set; }

        [Required]
        [Column("shelter_id")]
        [ForeignKey("Shelter")]
        public int? ShelterId { get; set; }

        [Required]
        [Column("pet_adopter_id")]
        [ForeignKey("Pet")]
        public int? PetAdopterId { get; set; }

        [Required]
        [Column("appointment_date")]
        public DateTime? AppointmentDate { get; set; }

        // Navigation properties for related entities
        public Shelter? Shelter { get; set; }
        public PetAdopter? PetAdopter { get; set; }
    }
}