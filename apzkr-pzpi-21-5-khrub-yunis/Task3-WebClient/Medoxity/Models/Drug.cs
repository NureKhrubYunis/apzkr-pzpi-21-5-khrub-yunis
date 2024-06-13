using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Medoxity.Models
{
    public class Drug
    {
        [Key]
        public int DrugID { get; set; }

        [Required]
        [StringLength(100)]
        public string DrugName { get; set; }

        public string Description { get; set; }

        [StringLength(100)]
        public string Manufacturer { get; set; }

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        public DateTime ReleaseDate { get; set; } 
    }
}
