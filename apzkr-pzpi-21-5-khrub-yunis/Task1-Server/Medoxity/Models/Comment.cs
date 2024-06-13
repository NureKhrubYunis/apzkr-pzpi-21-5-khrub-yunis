using System.ComponentModel.DataAnnotations;

namespace Medoxity.Models
{
    public class Comment
    {
        [Key]
        public int CommentID { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        public int DrugID { get; set; }

        public string Text { get; set; }

        [Required]
        public DateTime CommentDate { get; set; } = DateTime.UtcNow;
    }
}
