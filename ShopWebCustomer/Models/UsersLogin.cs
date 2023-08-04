using System.ComponentModel.DataAnnotations;

namespace ShopWebCustomer.Models
{
    public class UsersLogin
    {
        [Key]
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? UserImage { get; set; }
        public string? Phone { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public int? CashUser { get; set; }

    }
}
