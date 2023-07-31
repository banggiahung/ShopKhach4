using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using ShopWebCustomer.Data;
using ShopWebCustomer.Models;
using ShopWebCustomer.Models.ProductsViewModels;
using ShopWebCustomer.Services;

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
        //Thêm sản phẩm
        [AllowAnonymous]
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
                        vm = await _context.Products.FirstOrDefaultAsync(x => x.ProductID == id);

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
        [AllowAnonymous]

        [HttpPost]
        public async Task<IActionResult> deleteProducts([FromForm] ProductsCRUDViewModels model)
        {
            try
            {
                var existingProduct = await _context.Products.FindAsync(model.ProductID);

                if (existingProduct == null)
                {
                    return NotFound();

                }
                _context.Products.Remove(existingProduct);
                await _context.SaveChangesAsync();

                return Ok(model);


            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
