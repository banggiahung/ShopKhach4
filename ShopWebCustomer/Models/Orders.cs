using System.ComponentModel.DataAnnotations;

namespace ShopWebCustomer.Models
{
    public class Orders
    {
        [Key]
        public int OrderId { get; set; }
        public int? Customer_Id { get; set; }
        public string? Customer_Name { get; set; }
        public string? Phone_Number { get; set; }
        public string? Address { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool? Order_Status { get; set; }
        public int? Total_Price { get; set; }

    }
}
