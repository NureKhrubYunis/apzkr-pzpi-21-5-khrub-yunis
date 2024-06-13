using System.ComponentModel.DataAnnotations;

namespace Medoxity.Models
{
    public class Document
    {
        [Key]
        public int DocumentID { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        public string DocumentName { get; set; }

        [StringLength(50)]
        public string DocumentType { get; set; }

        [StringLength(255)]
        public string DocumentPath { get; set; }

        [Required]
        public DateTime UploadDate { get; set; } = DateTime.UtcNow;
    }
}
