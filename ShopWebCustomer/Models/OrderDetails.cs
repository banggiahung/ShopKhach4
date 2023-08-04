using System.ComponentModel.DataAnnotations;

namespace ShopWebCustomer.Models
{
    public class OrderDetails
    {
        [Key]
        public int OrderDetailId { get; set; }
        public int Order_Id { get; set; }
        public string? Product_Name { get; set; }
        public int? Product_Price { get; set; }
        public int? Product_Quantity { get; set; }
        public int ProductId { get; set; }
        public string? Product_Image { get; set; }
        public int OrderId { get; set; }

    }
}
