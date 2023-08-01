namespace ShopWebCustomer.Models.ProductsViewModels
{
    public class ProductsCRUDViewModels
    {
        public int ID { get; set; }
        public string? ProductID { get; set; }
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public string? Slug { get; set; }
        public int? Price { get; set; }
        public int? CategoryID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ImageMain { get; set; }
        public IFormFile? PrPath { get; set; }


        public static implicit operator ProductsCRUDViewModels(Products _user)
        {
            return new ProductsCRUDViewModels
            {
                ProductID = _user.ProductID,
                ProductName = _user.ProductName,
                Description = _user.Description,
                Slug = _user.Slug,
                Price = _user.Price,
                CategoryID = _user.CategoryID,
                CreatedDate = _user.CreatedDate,
                ModifiedDate = _user.ModifiedDate,
                ImageMain = _user.ImageMain,
                ID = _user.ID,
            };
        }
        public static implicit operator Products(ProductsCRUDViewModels vm)
        {
            return new Products
            {
                ProductID = vm.ProductID,
                ProductName = vm.ProductName,
                Description = vm.Description,
                Slug = vm.Slug,
                Price = vm.Price,
                CategoryID = vm.CategoryID,
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                ImageMain = vm.ImageMain,
                ID = vm.ID,
            };
        }
    }
}
