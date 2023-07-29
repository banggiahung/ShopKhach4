using System.ComponentModel.DataAnnotations;

namespace ShopWebCustomer.Models
{
    public class ImagesProduct
    {
        [Key]
        public int ImageID { get; set; }
        public int ProductID { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImageUrl_1 { get; set; }
        public string? ImageUrl_2 { get; set; }
        public string? ImageUrl_3 { get; set; }
        public string? ImageUrl_4 { get; set; }
    }
}
