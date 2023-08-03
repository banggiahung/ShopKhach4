using ShopWebCustomer.Models.ProductsViewModels;
using System.ComponentModel.DataAnnotations;

namespace ShopWebCustomer.Models.UsersLoginViewModels
{
    public class UsersLoginCRUDViewModels
    {
		public int ID { get; set; }
		[Required(ErrorMessage = "Vui lòng không để trống")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "Vui lòng không để trống")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		public int? CashUser { get; set; }

		[Required(ErrorMessage = "Vui lòng không để trống")]
		[Compare("Password", ErrorMessage = "Mật khẩu không giống")]
		[DataType(DataType.Password)]
		public string? ConfirmPassword { get; set; }

		public static implicit operator UsersLoginCRUDViewModels(UsersLogin _user)
		{
			return new UsersLoginCRUDViewModels
			{
				
				ID = _user.ID,
				UserName = _user.UserName,
				Password = _user.Password,
				CashUser = _user.CashUser
			};
		}
		public static implicit operator UsersLogin(UsersLoginCRUDViewModels vm)
		{
			return new UsersLogin
			{
				
				ID = vm.ID,
				UserName = vm.UserName,
				Password = vm.Password,
				CashUser = vm.CashUser
			};
		}
	}
}
