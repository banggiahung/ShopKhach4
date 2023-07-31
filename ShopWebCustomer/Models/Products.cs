using System.ComponentModel.DataAnnotations;

namespace ShopWebCustomer.Models
{
    public class Products
    {
        [Key]
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public string? Slug { get; set; }
        public int? Price { get; set; }
        public int? CategoryID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ImageMain { get; set; }
    }
}
