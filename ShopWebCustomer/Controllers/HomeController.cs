using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopWebCustomer.Data;
using ShopWebCustomer.Models;
using ShopWebCustomer.Models.UsersLoginViewModels;
using ShopWebCustomer.Services;
using System.Data;
using System.Diagnostics;

namespace ShopWebCustomer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
		private readonly ICommon _iCommon;


		public HomeController(ILogger<HomeController> logger, IWebHostEnvironment hostEnvironment, ApplicationDbContext context, IConfiguration configuration, ICommon iCommon)
        {
            _logger = logger;
            _hostEnvironment = hostEnvironment;
            _context = context;
            _configuration = configuration;
			_iCommon = iCommon;
        }

        public IActionResult Index()
        {
            var a = HttpContext.Session.GetInt32("Id");
            ViewBag.LinkUser = a;
            var e = _context.UsersLogin.ToList();
            return View(e);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult CategoryIndex()
        {
           
            return View();
        }
		[HttpGet]
		public IActionResult DetailsProducts(int id)
        {

			if (id > 0)
			{
                try
                {
                    var vm = _context.Products.FirstOrDefault(x => x.ID == id);

                    if (vm == null)
					{
						return BadRequest("Không tìm thấy đối tượng với ID tương ứng");
					}
					return View(vm);

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
		}
		public IActionResult Login()
		{
			if (HttpContext.Session.GetInt32("Id") != null)
			{
				return RedirectToAction("Index", "Home");
			}
			else
			{
				return View();
			}
		}

		[HttpPost]
		public async Task<IActionResult> Login(UsersLoginCRUDViewModels model)
		{
			var user = await _context.UsersLogin.SingleOrDefaultAsync(u => u.UserName == model.UserName);
			model.Password = _iCommon.GetMD5(model.Password);
			if (user != null && user.Password == model.Password)
			{
				// Đăng nhập thành công
				HttpContext.Session.SetInt32("Id", (int)user.ID);
				HttpContext.Session.SetString("UserName", user.UserName);
				return RedirectToAction("Index", "Home");
			}
			else
			{
				ViewBag.Login = "Sai tài khoản hoặc mật khẩu";
				// Không đăng nhập được
				ModelState.AddModelError("", "Lỗi");
				return View(model);
			}
		}
        // đăng ký
        public IActionResult Register()
        {
            if (HttpContext.Session.GetInt32("Id") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(UsersLoginCRUDViewModels model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem UserName đã có trong cơ sở dữ liệu chưa
                var existingUser = await _context.UsersLogin.FirstOrDefaultAsync(u => u.UserName == model.UserName);
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "Tài khoản đã có");
                    ViewBag.UserName = "Tài Khoản đã có";
                    return View(model);
                }
                // Thêm người dùng mới vào cơ sở dữ liệu
                model.Password = _iCommon.GetMD5(model.Password);
                model.CashUser = 0;
                UsersLogin user = new();
                user = model;
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login", "Home");
            }
            // Hiển thị thông báo lỗi nếu dữ liệu đăng ký không hợp lệ
            return View(model);
        }
        public IActionResult Logout()
        {
            Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
            Response.Headers.Add("Pragma", "no-cache");
            Response.Headers.Add("Expires", "0");
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}