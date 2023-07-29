using ShopWebCustomer.Models.ProductsViewModels;

namespace ShopWebCustomer.Models.ImagesProductViewModels
{
    public class ImagesProductCRUDViewModels
    {
        public int ImageID { get; set; }
        public int ProductID { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImageUrl_1 { get; set; }
        public string? ImageUrl_2 { get; set; }
        public string? ImageUrl_3 { get; set; }
        public string? ImageUrl_4 { get; set; }

        public static implicit operator ImagesProductCRUDViewModels(ImagesProduct _user)
        {
            return new ImagesProductCRUDViewModels
            {
                ImageID = _user.ImageID,
                ProductID = _user.ProductID,
                ImageUrl = _user.ImageUrl,
                ImageUrl_1 = _user.ImageUrl_1,
                ImageUrl_2 = _user.ImageUrl_2,
                ImageUrl_3 = _user.ImageUrl_3,
                ImageUrl_4 = _user.ImageUrl_4,
            };
        }
        public static implicit operator ImagesProduct(ImagesProductCRUDViewModels vm)
        {
            return new ImagesProduct
            {
                ImageID = vm.ImageID,
                ProductID = vm.ProductID,
                ImageUrl = vm.ImageUrl,
                ImageUrl_1 = vm.ImageUrl_1,
                ImageUrl_2 = vm.ImageUrl_2,
                ImageUrl_3 = vm.ImageUrl_3,
                ImageUrl_4 = vm.ImageUrl_4,
            };
        }
    }
}
