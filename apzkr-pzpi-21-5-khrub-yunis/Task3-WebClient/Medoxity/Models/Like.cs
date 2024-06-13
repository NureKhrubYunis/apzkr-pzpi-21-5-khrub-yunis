using System.ComponentModel.DataAnnotations;

namespace Medoxity.Models
{
    public class Like
    {
        [Key]
        public int LikeID { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        public int CommentID { get; set; }
    }
}
