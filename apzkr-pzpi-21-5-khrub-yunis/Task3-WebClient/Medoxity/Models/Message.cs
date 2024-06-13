using System.ComponentModel.DataAnnotations;

namespace Medoxity.Models
{
    public class Message
    {
        [Key]
        public int MessageID { get; set; }

        [Required]
        [StringLength(50)]
        public string SenderUsername { get; set; }

        [Required]
        [StringLength(50)]
        public string ReceiverUsername { get; set; }

        public string Text { get; set; }

        [Required]
        public DateTime SentDate { get; set; } = DateTime.UtcNow;

    }
}
