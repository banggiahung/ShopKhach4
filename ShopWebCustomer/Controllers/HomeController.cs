using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopWebCustomer.Data;
using ShopWebCustomer.Models;
using ShopWebCustomer.Models.UsersLoginViewModels;
using ShopWebCustomer.Services;
using System.Data;
using System.Diagnostics;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Text.Json;
using ShopWebCustomer.Models.OrdersViewModels;
using MailKit.Search;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;


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

            var userClaims = HttpContext.User.Claims.ToList();

            // Lấy tên người dùng
            var userName = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            // Lấy email người dùng
            var email = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            // Lấy hình ảnh người dùng (avatar)
            var pictureUrl = userClaims.FirstOrDefault(c => c.Type == "picture")?.Value;

            // ... Bổ sung các thông tin khác tùy theo yêu cầu

            if (!string.IsNullOrEmpty(email))
            {
                var contextUser = _context.UsersLogin.FirstOrDefault(x => x.UserName == email);
                if (userClaims != null)
                {
                    if (contextUser != null)
                    {
                        HttpContext.Session.SetInt32("Id", (int)contextUser.ID);

                    }
                    else
                    {
                        UsersLogin user = new UsersLogin();
                        user.CashUser = 0;
                        user.UserName = email;
                        // Kiểm tra giá trị trả về từ _iCommon.GetMD5(userName) có null hay không
                        var passwordHash = _iCommon.GetMD5(email);
                        if (!string.IsNullOrEmpty(passwordHash))
                        {
                            user.Password = passwordHash;
                            _context.Add(user);
                            _context.SaveChanges();

                            HttpContext.Session.SetInt32("Id", (int)user.ID);

                        }
                        else
                        {
                            // Xử lý nếu giá trị trả về từ _iCommon.GetMD5(userName) là null
                        }
                    }
                }
            }
            var a = HttpContext.Session.GetInt32("Id");
            ViewBag.LinkUser = a;

            ViewBag.Email = email;
            ViewBag.PictureUrl = pictureUrl;
            // Gửi thông tin người dùng đến View để hiển thị
            ViewBag.UserName = userName;
            ViewBag.Email = email;
            ViewBag.PictureUrl = pictureUrl;
            var e = _context.UsersLogin.ToList();
            return View(e);
        }
        public IActionResult LoginGoogle()
        {

            return Challenge(new AuthenticationProperties { RedirectUri = "/" }, OpenIdConnectDefaults.AuthenticationScheme);
        }

        [Authorize]
        public IActionResult LogoutGoogle()
        {
            return SignOut(new AuthenticationProperties { RedirectUri = "/" },
                CookieAuthenticationDefaults.AuthenticationScheme,
                OpenIdConnectDefaults.AuthenticationScheme);
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult CategoryIndex()
        {

            return View();
        }
        public IActionResult ProductShop()
        {

            return View();
        }
        public IActionResult AboutUs()
        {

            return View();
        }

        [HttpGet]
        public IActionResult GetUserDataId(int? id)
        {
            int userId = id ?? HttpContext.Session.GetInt32("Id") ?? 0;

            return Ok(userId);
        }
        [HttpGet]
        public IActionResult DetailsProducts(int id)
        {
            var a = HttpContext.Session.GetInt32("Id");
            ViewBag.LinkUser = a;
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
        [HttpGet]
        public IActionResult GetProductsByCate(int id)
        {

            if (id > 0)
            {
                try
                {
                    var vm = from pr in _context.Products
                             join cate in _context.Categories on pr.CategoryID equals cate.CategoryID
                             where pr.CategoryID == id
                             select new
                             {
                                 ID = pr.ID,
                                 Quantity = pr.Quantity,
                                 ProductName = pr.ProductName,
                                 Description = pr.Description,
                                 Slug = pr.Slug,
                                 Price = pr.Price,
                                 CategoryID = pr.CategoryID,
                                 CreatedDate = pr.CreatedDate,
                                 ImageMain = pr.ImageMain
                             };

                    if (vm == null)
                    {
                        return BadRequest("Không tìm thấy đối tượng với ID tương ứng");
                    }
                    return Ok(vm.ToList().OrderByDescending(x=>x.ID));

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
        public IActionResult CartDetails()
        {
            var a = HttpContext.Session.GetInt32("Id");
            ViewBag.LinkUser = a;
            return View();
        }
        public IActionResult CheckOut()
        {
            var a = HttpContext.Session.GetInt32("Id");
            ViewBag.LinkUser = a;
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
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            //Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
            //Response.Headers.Add("Pragma", "no-cache");
            //Response.Headers.Add("Expires", "0");
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        [AllowAnonymous]
        // Add to card
        [HttpPost]
        public async Task<IActionResult> AddToCart(OrdersCRUDViewModels model)
        {
            try
            {
                if (HttpContext.Session.GetInt32("Id") != null)
                {

                    var userId = HttpContext.Session.GetInt32("Id");

                    var user = await _context.UsersLogin.SingleOrDefaultAsync(u => u.ID == userId);

                    var orderCheck = await _context.Orders
     .Where(o => o.Customer_Id == user.ID)
     .OrderByDescending(o => o.CreateDate) // Sắp xếp theo CreateDate giảm dần
     .FirstOrDefaultAsync();

                    var product = await _context.Products.SingleOrDefaultAsync(p => p.ID == model.ProductIds);
                    if (orderCheck != null)
                    {
                        if (orderCheck.Order_Status == true)
                        {
                            Orders nl = new Orders();

                            if (user != null)
                            {
                                nl.Customer_Id = userId;
                                nl.Customer_Name = user.UserName;
                                nl.Phone_Number = user.Phone;
                                nl.Address = user.Address;
                                nl.Order_Status = false;
                                nl.Total_Price = model.Product_Quantity * product.Price;
                                nl.CreateDate = DateTime.Now;
                            }
                            _context.Orders.Add(nl);
                            await _context.SaveChangesAsync();

                            int orderId = nl.OrderId;

                            OrderDetails _nl = new OrderDetails();
                            _nl.Product_Name = product.ProductName;
                            _nl.ProductId = product.ID;
                            _nl.Product_Price = product.Price;
                            _nl.Product_Quantity = model.Product_Quantity;
                            _nl.Product_Image = product.ImageMain;
                            _nl.Order_Id = orderId;
                            _nl.OrderId = orderId;
                            _context.Add(_nl);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            int orderIdTrue = orderCheck.OrderId;
                            int? checkQuan = model.Product_Quantity * product.Price;
                            orderCheck.Total_Price += checkQuan;
                            _context.Orders.Update(orderCheck);
                            await _context.SaveChangesAsync();
                            OrderDetails _nl = new OrderDetails();
                            _nl.Product_Name = product.ProductName;
                            _nl.ProductId = product.ID;
                            _nl.Product_Price = product.Price;
                            _nl.Product_Quantity = model.Product_Quantity;
                            _nl.Product_Image = product.ImageMain;
                            _nl.Order_Id = orderIdTrue;
                            _nl.OrderId = orderIdTrue;
                            _context.Add(_nl);
                            await _context.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        Orders nl = new Orders();

                        if (user != null)
                        {
                            nl.Customer_Id = userId;
                            nl.Customer_Name = user.UserName;
                            nl.Phone_Number = user.Phone;
                            nl.Address = user.Address;
                            nl.Order_Status = false;
                            nl.Total_Price = model.Product_Quantity * product.Price;
                            nl.CreateDate = DateTime.Now;
                        }
                        _context.Orders.Add(nl);
                        await _context.SaveChangesAsync();

                        int orderId = nl.OrderId;

                        OrderDetails _nl = new OrderDetails();
                        _nl.Product_Name = product.ProductName;
                        _nl.ProductId = product.ID;
                        _nl.Product_Price = product.Price;
                        _nl.Product_Quantity = model.Product_Quantity;
                        _nl.Product_Image = product.ImageMain;
                        _nl.Order_Id = orderId;
                        _nl.OrderId = orderId;
                        _context.Add(_nl);
                        await _context.SaveChangesAsync();

                    }
                    var options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve
                    };

                    // Chuyển đối tượng model thành JSON
                    var json = System.Text.Json.JsonSerializer.Serialize(model, options);

                    // Trả về kết quả dưới dạng JsonResult
                    return new JsonResult(json);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ComfirmOrder(int? orderId)
        {
            try
            {
                if (orderId != null)
                {
                    Orders find = await _context.Orders.FirstOrDefaultAsync(x => x.OrderId == orderId);
                    if (find != null)
                    {

                        find.Order_Status = true;
                        _context.Orders.Update(find);
                        await _context.SaveChangesAsync();
                        return Ok(find);
                    }
                    else
                    {
                        return NotFound();

                    }

                }
                else
                {
                    return NotFound();

                }
            }
            catch (Exception ex)
            {
                return BadRequest("Lỗi: " + ex.Message);
            }
        }
        [AllowAnonymous]

        [HttpGet]
        public IActionResult GetCart()
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("Id");


                if (userId != null)
                {
                    var vm = from a in _context.Orders
                             join b in _context.OrderDetails on a.OrderId equals b.OrderId
                             where a.Customer_Id == userId
                             where a.Order_Status == false
                             group new { a, b } by new { a.OrderId, a.Customer_Name, a.Customer_Id, a.Phone_Number, a.Address, a.CreateDate, a.Order_Status, a.Total_Price, b.ProductId, b.Product_Name, b.Product_Price, b.Product_Image } into g
                             select new
                             {
                                 OrderId = g.Key.OrderId,
                                 Customer_Name = g.Key.Customer_Name,
                                 Customer_Id = g.Key.Customer_Id,
                                 Phone_Number = g.Key.Phone_Number,
                                 Address = g.Key.Address,
                                 CreateDate = g.Key.CreateDate,
                                 Order_Status = g.Key.Order_Status,
                                 Total_Price = g.Key.Total_Price,
                                 Product_Name = g.Key.Product_Name,
                                 Product_Price = g.Key.Product_Price,
                                 Product_Quantity = g.Sum(x => x.b.Product_Quantity),
                                 Product_Image = g.Key.Product_Image,
                                 Product_Id = g.Key.ProductId
                             };

                    return Ok(vm.ToList());
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetCartNoFalse()
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("Id");

                if (userId != null)
                {
                    var vm = from a in _context.Orders
                             where a.Customer_Id == userId
                             select new
                             {
                                 OrderId = a.OrderId,
                                 Customer_Name = a.Customer_Name,
                                 Customer_Id = a.Customer_Id,
                                 Phone_Number = a.Phone_Number,
                                 Address = a.Address,
                                 CreateDate = a.CreateDate,
                                 Order_Status = a.Order_Status,
                                 Total_Price = a.Total_Price,
                             };
                    return Ok(vm.ToList());
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [AllowAnonymous]

        [HttpGet]
        public IActionResult GetCartUser()
        {
            try
            {
                var vm = from a in _context.Orders
                         select new
                         {
                             OrderId = a.OrderId,
                             Customer_Name = a.Customer_Name,
                             Customer_Id = a.Customer_Id,
                             Phone_Number = a.Phone_Number,
                             Address = a.Address,
                             CreateDate = a.CreateDate,
                             Order_Status = a.Order_Status,
                             Total_Price = a.Total_Price,
                         };
                return Ok(vm.ToList().OrderByDescending(x => x.CreateDate));

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet]
        public IActionResult GetCartDetails(int orderId)
        {
            try
            {

                if (orderId != 0)
                {
                    var vm = from a in _context.Orders
                             join b in _context.OrderDetails on a.OrderId equals b.OrderId
                             where a.Order_Status == false
                             group new { a, b } by new { a.OrderId, a.Customer_Name, a.Customer_Id, a.Phone_Number, a.Address, a.CreateDate, a.Order_Status, a.Total_Price, b.ProductId, b.Product_Name, b.Product_Price, b.Product_Image } into g
                             select new
                             {
                                 OrderId = g.Key.OrderId,
                                 Customer_Name = g.Key.Customer_Name,
                                 Customer_Id = g.Key.Customer_Id,
                                 Phone_Number = g.Key.Phone_Number,
                                 Address = g.Key.Address,
                                 CreateDate = g.Key.CreateDate,
                                 Order_Status = g.Key.Order_Status,
                                 Total_Price = g.Key.Total_Price,
                                 Product_Name = g.Key.Product_Name,
                                 Product_Price = g.Key.Product_Price,
                                 Product_Quantity = g.Sum(x => x.b.Product_Quantity),
                                 Product_Image = g.Key.Product_Image,
                                 Product_Id = g.Key.ProductId
                             };
                    return Ok(vm.ToList());
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet]
        public IActionResult GetCartDetailsNoFlase(int orderId)
        {
            try
            {

                if (orderId != 0)
                {
                    var vm = from a in _context.Orders
                             join b in _context.OrderDetails on a.OrderId equals b.OrderId
                             where a.OrderId == orderId
                             group new { a, b } by new { a.OrderId, a.Customer_Name, a.Customer_Id, a.Phone_Number, a.Address, a.CreateDate, a.Order_Status, a.Total_Price, b.ProductId, b.Product_Name, b.Product_Price, b.Product_Image } into g
                             select new
                             {
                                 OrderId = g.Key.OrderId,
                                 Customer_Name = g.Key.Customer_Name,
                                 Customer_Id = g.Key.Customer_Id,
                                 Phone_Number = g.Key.Phone_Number,
                                 Address = g.Key.Address,
                                 CreateDate = g.Key.CreateDate,
                                 Order_Status = g.Key.Order_Status,
                                 Total_Price = g.Key.Total_Price,
                                 Product_Name = g.Key.Product_Name,
                                 Product_Price = g.Key.Product_Price,
                                 Product_Quantity = g.Sum(x => x.b.Product_Quantity),
                                 Product_Image = g.Key.Product_Image,
                                 Product_Id = g.Key.ProductId
                             };
                    return Ok(vm.ToList());
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Profile(UsersLoginCRUDViewModels _data)
        {
            try
            {

                int? userId = HttpContext.Session.GetInt32("Id");
                UsersLogin userData = await _context.UsersLogin.FirstOrDefaultAsync(u => u.ID == userId);

                if (userData != null)
                {

                    if (_data.AnhDaiDien != null)
                    {
                        var PrPath = await _iCommon.UploadedFile(_data.AnhDaiDien);
                        userData.UserImage = "/upload/" + PrPath;
                    }

                    else
                    {
                        _data.AnhDaiDien = _data.AnhDaiDien;
                    }
                    _context.Update(userData);
                    await _context.SaveChangesAsync();

                }
                else
                {
                    return NotFound();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return Ok(_data);

        }
        [HttpPost]
        public async Task<IActionResult> ProfileInfo(UsersLoginCRUDViewModels _data)
        {
            try
            {

                int? userId = HttpContext.Session.GetInt32("Id");
                UsersLogin userData = await _context.UsersLogin.FirstOrDefaultAsync(u => u.ID == userId);

                if (userData != null)
                {

                    userData.FirstName = _data.FirstName;
                    userData.Phone = _data.Phone;
                    userData.LastName = _data.LastName;
                    _context.Update(userData);
                    await _context.SaveChangesAsync();

                }
                else
                {
                    return NotFound();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return Ok(_data);

        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}