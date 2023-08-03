using System.ComponentModel.DataAnnotations;

namespace ShopWebCustomer.Models
{
    public class UsersLogin
    {
        [Key]
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int? CashUser { get; set; }

    }
}
