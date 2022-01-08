using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceAPI.Models
{
    public class UsersProducts
    {

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public int OrderId { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Product Product { get; set; }


    }
}
