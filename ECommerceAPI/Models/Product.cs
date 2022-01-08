using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceAPI.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public double ProductPrice { get; set; }
        public byte? ProductRate { get; set; }
        public int? ProductOffer { get; set; }
        public string? ProductPhoto { get; set; }
        public int? NumberInStock { get; set; }

        [ForeignKey("Category")]
        public int? CatId { get; set; }
        public virtual Category? Category { get; set; }

    }
}
