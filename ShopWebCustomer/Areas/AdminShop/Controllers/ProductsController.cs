using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using ShopWebCustomer.Data;
using ShopWebCustomer.Models;
using ShopWebCustomer.Models.CategoriesViewModels;
using ShopWebCustomer.Models.ProductsViewModels;
using ShopWebCustomer.Services;
using System.Data;

namespace ShopWebCustomer.Areas.AdminShop.Controllers
{
    [Area("AdminShop")]

    public class ProductsController : Controller
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        // GET: ProductsController
        public ProductsController(ILogger<ProductsController> logger, ApplicationDbContext context, ICommon common, IUrlHelperFactory urlHelperFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _iCommon = common;
            _urlHelperFactory = urlHelperFactory;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Categories()
        {
            return View();
        }

        // GET: ProductsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        [AllowAnonymous]
        [HttpGet]

        public IActionResult GetAllProduct()
        {

            var pr = from _pr in _context.Products
                     join _cate in _context.Categories on _pr.CategoryID equals _cate.CategoryID
                     select new ProductsCRUDViewModels
                     {
                         ID = _pr.ID,
                         ProductID = _pr.ProductID,
                         ProductName = _pr.ProductName,
                         Description = _pr.Description,
                         Slug = _pr.Slug,
                         Price = _pr.Price,
                         CategoryID = _pr.CategoryID,
                         CreatedDate = _pr.CreatedDate,
                         ModifiedDate = _pr.ModifiedDate,
                         ImageMain = _pr.ImageMain,
                         CategoryName = _cate.CategoryName,
                     };

            return Ok(pr.ToList());
        }
        //Thêm sản phẩm
        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductsCRUDViewModels model)
        {
            try
            {


                if (model.PrPath != null)
                {
                    var PrPath = await _iCommon.UploadedFile(model.PrPath);
                    model.ImageMain = "/upload/" + PrPath;
                }
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                _context.Products.Add(model);
                await _context.SaveChangesAsync();
                return Ok(model);



            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [HttpGet]
        public async Task<IActionResult> getIdProducts(int id)
        {
            try
            {
                Products vm = new Products();
                if (id > 0)
                {
                    try
                    {
                        vm = await _context.Products.FirstOrDefaultAsync(x => x.ID == id);

                        if (vm == null)
                        {
                            return BadRequest("Không tìm thấy đối tượng với ID tương ứng");
                        }
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }

                }
                else
                {
                    return NotFound();
                }


                return Ok(vm);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> deleteProducts(ProductsCRUDViewModels model)
        {
            try
            {
                var existingProduct = await _context.Products.FirstOrDefaultAsync(x => x.ID == model.ID);

                if (existingProduct == null)
                {
                    return NotFound();

                }
                _context.Products.Remove(existingProduct);
                await _context.SaveChangesAsync();

                return Ok(existingProduct);


            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [HttpPost]
        public async Task<IActionResult> UpdateProduct(ProductsCRUDViewModels model)
        {
            try
            {
                var existingProduct = await _context.Products.FindAsync(model.ID);

                if (existingProduct == null)
                {
                    return NotFound("Không tìm thấy");
                }

                if (model.PrPath != null)
                {
                    var PrPath = await _iCommon.UploadedFile(model.PrPath);
                    existingProduct.ImageMain = "/upload/" + PrPath;
                }


                // Cập nhật các thuộc tính khác của existingProduct nếu cần
                existingProduct.ProductName = model.ProductName;
                existingProduct.Description = model.Description;
                existingProduct.Slug = model.Slug;
                existingProduct.Price = model.Price;
                existingProduct.CategoryID = model.CategoryID;
                existingProduct.ModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync();

                return Ok(existingProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        //danh mục

        [AllowAnonymous]
        [HttpGet]

        public IActionResult GetAllCategory()
        {
            var pr = _context.Categories.ToList();
            return Ok(pr);
        }

        //thêm danh mục
        [HttpPost]
        public async Task<IActionResult> AddCategory(CategoriesCRUDViewModels model)
        {
            try
            {
                if (model.PrPath != null)
                {
                    var PrPath = await _iCommon.UploadedFile(model.PrPath);
                    model.ImageCategory = "/upload/" + PrPath;
                }
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                _context.Categories.Add(model);
                await _context.SaveChangesAsync();
                return Ok(model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [HttpGet]
        public async Task<IActionResult> getIdCategory(int id)
        {
            try
            {
                Categories vm = new Categories();
                if (id > 0)
                {
                    try
                    {
                        vm = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryID == id);

                        if (vm == null)
                        {
                            return BadRequest("Không tìm thấy đối tượng với ID tương ứng");
                        }
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }

                }
                else
                {
                    return NotFound();
                }


                return Ok(vm);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCategory(CategoriesCRUDViewModels model)
        {
            try
            {
                var existingProduct = await _context.Categories.FindAsync(model.CategoryID);

                if (existingProduct == null)
                {
                    return NotFound("Không tìm thấy");
                }

                if (model.PrPath != null)
                {
                    var PrPath = await _iCommon.UploadedFile(model.PrPath);
                    existingProduct.ImageCategory = "/upload/" + PrPath;
                }

                existingProduct.CategoryName = model.CategoryName;
                existingProduct.ModifiedDate = DateTime.Now;
                await _context.SaveChangesAsync();

                return Ok(existingProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCategory(CategoriesCRUDViewModels model)
        {
            try
            {
                var existingProduct = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryID == model.CategoryID);

                if (existingProduct == null)
                {
                    return NotFound();

                }
                _context.Categories.Remove(existingProduct);
                await _context.SaveChangesAsync();

                return Ok(existingProduct);


            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }


    }
}
