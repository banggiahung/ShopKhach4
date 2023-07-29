using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Routing;
using ShopWebCustomer.Data;
using ShopWebCustomer.Services;

namespace ShopWebCustomer.Areas.AdminShop.Controllers
{
    [Area("AdminShop")]

    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        public DashboardController(ILogger<DashboardController> logger, ApplicationDbContext context, ICommon common, IUrlHelperFactory urlHelperFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _iCommon = common;
            _urlHelperFactory = urlHelperFactory;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                // Lưu lại URL hiện tại để chuyển hướng sau khi đăng nhập thành công
                string returnUrl = Url.Action("Index", "Dashboard");
                return RedirectToAction("Login", "Dashboard", new { returnUrl });
            }

            // Đã đăng nhập và có quyền "Admin", tiếp tục hiển thị trang Index
            ViewBag.checkActive = "trangChu";
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
            {
                // Already logged in and is an "Admin," redirect to dashboard
                return RedirectToAction("Index", "Dashboard");
            }

            // Not logged in or not an "Admin," show login view
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password,bool RememberMe)
        {
            
                if (userName == _configuration["AdminShopMain:Username"] && password == _configuration["AdminShopMain:Password"])
                {
                    var claims = new List<Claim>
                    {
                     new Claim(ClaimTypes.Name,userName),
                        new Claim(ClaimTypes.Role, "Admin")
                    };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = (bool)RememberMe
                };

                await HttpContext.SignInAsync(
                          CookieAuthenticationDefaults.AuthenticationScheme,
                          new ClaimsPrincipal(claimsIdentity),
                          authProperties);
                return Redirect("/Dashboard/Index");
                }
                ModelState.AddModelError(string.Empty, "Sai mật khẩu,admin");
                ViewBag.LoginAdmin = "Sai mật khẩu,admin";
            return View();
            
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Dashboard");
        }
    }
}
