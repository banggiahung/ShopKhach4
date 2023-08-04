using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopWebCustomer.Controllers;
using ShopWebCustomer.Data;
using ShopWebCustomer.Services;

namespace ShopWebCustomer.Controllers
{
    public class AccountUserController : Controller
    {
        private readonly ILogger<AccountUserController> _logger;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ICommon _iCommon;
        public AccountUserController(ILogger<AccountUserController> logger, IWebHostEnvironment hostEnvironment, ApplicationDbContext context, IConfiguration configuration, ICommon iCommon)
        {
            _logger = logger;
            _hostEnvironment = hostEnvironment;
            _context = context;
            _configuration = configuration;
            _iCommon = iCommon;
        }
        public async Task<IActionResult> Index()
        {
            var a = HttpContext.Session.GetInt32("Id");
            ViewBag.LinkUser = a;

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
			var a = HttpContext.Session.GetInt32("Id");
            try
            {
				if (a != 0)
				{

					var user = await _context.UsersLogin
						.Where(u =>u.ID  == a)// Điều kiện tìm kiếm
						.Select(u => new
						{
							u.ID,
							u.UserName,
							u.CashUser,
                            u.UserImage,
                            u.FirstName,
                            u.LastName,
                            u.Phone,
                            u.Address
						})
						.FirstOrDefaultAsync();

					return Ok(user);

				}
                else
                {
                    return NotFound();
                     
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
          
        }
    }
}
