using System.ComponentModel.DataAnnotations;

namespace ShopWebCustomer.Models
{
    public class UsersLogin
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
