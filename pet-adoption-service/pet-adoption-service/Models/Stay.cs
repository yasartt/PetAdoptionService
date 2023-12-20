using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace pet_adoption_service.Models
{
    [Table("stay")]
    public class Stay
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("stay_id")]
        public int StayId { get; set; }

        [Required]
        [ForeignKey("Pet")]
        [Column("pet_id")]
        public int PetId { get; set; }

        [Required]
        [ForeignKey("Shelter")]
        [Column("shelter_id")]
        public int ShelterId { get; set; }

        [Required]
        public DateTime start_date { get; set; }

        public DateTime? end_date { get; set; }

        public bool is_Current { get; set; }

        public Pet? Pet { get; set; }

        public Shelter? Shelter { get; set; }

    }
}