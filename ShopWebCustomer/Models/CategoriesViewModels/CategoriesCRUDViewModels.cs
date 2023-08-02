using ShopWebCustomer.Models.ProductsViewModels;

namespace ShopWebCustomer.Models.CategoriesViewModels
{
    public class CategoriesCRUDViewModels
    {
        public int CategoryID { get; set; }
        public string? CategoryName { get; set; }
        public string? ImageCategory { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public IFormFile? PrPath { get; set; }


        public static implicit operator CategoriesCRUDViewModels(Categories _user)
        {
            return new CategoriesCRUDViewModels
            {
                CategoryID = _user.CategoryID,
                CategoryName = _user.CategoryName,
                ImageCategory = _user.ImageCategory,
                CreatedDate = _user.CreatedDate,
                ModifiedDate = _user.ModifiedDate,

            };
        }
        public static implicit operator Categories(CategoriesCRUDViewModels vm)
        {
            return new Categories
            {
                CategoryID = vm.CategoryID,
                CategoryName = vm.CategoryName,
                ImageCategory = vm.ImageCategory,
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
            };
        }
    }
}
