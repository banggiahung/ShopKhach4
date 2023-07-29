using Microsoft.AspNetCore.Mvc;
using ShopWebCustomer.Data;
using ShopWebCustomer.Models;
using System.Diagnostics;

namespace ShopWebCustomer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment hostEnvironment, ApplicationDbContext context, IConfiguration configuration)
        {
            _logger = logger;
            _hostEnvironment = hostEnvironment;
            _context = context;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var e = _context.UsersLogin.ToList();
            return View(e);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}