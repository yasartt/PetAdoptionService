using Microsoft.EntityFrameworkCore;
using pet_adoption_service.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pet_adoption_service.Models
{
    [Table("expert_advice")]
    public class ExpertAdvice
    {
        [Key]
        [Column("advice_id")]
        [Required]
        public int adviceId { get; set; }

        [ForeignKey("Veterinarian")]
        public int veterinarian_id { get; set; }

        [ForeignKey("Pet")]
        public int pet_id { get; set; }

        [Required]
        public DateTime advice_date { get; set; }

        [StringLength(2000)]
        public string advice { get; set; }

        // Navigation properties for related entities
        public Veterinarian? Veterinarian { get; set; }

        public Pet? Pet { get; set; }

    }
}