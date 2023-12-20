using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pet_adoption_service.Models
{
    [Table("pet")]
    public class Pet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("pet_id")]
        public int PetId { get; set; }

        [Column("breed")]
        [MaxLength(255)]
        public string? Breed { get; set; }

        [Column("age")]
        public int? Age { get; set; }

        [Column("name")]
        [MaxLength(255)]
        public string? Name { get; set; }

        [Column("status")]
        [MaxLength(255)]
        public string? Status { get; set; }

        [Column("gender")]
        [MaxLength(50)]
        public string? Gender { get; set; }

        [Column("is_available")]
        public int? IsAvailable { get; set; }

        public int? photo_id { get; set; }
    }
}