using System.ComponentModel.DataAnnotations;

namespace Medoxity.Models
{
    public class Rating
    {
        [Key]
        public int RatingID { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        public int DrugID { get; set; }

        [Range(1, 5)]
        public int Value { get; set; }
    }
}
