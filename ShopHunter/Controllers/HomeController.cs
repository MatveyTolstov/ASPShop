using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopHunter.Models;
using ShopHunter.Models.BDConn;

namespace ShopHunter.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    // Главная страница
    public async Task<IActionResult> Index()
    {
        var products = await _context.Product.ToListAsync();
        // foreach (var product in products)
        // {
        //    
        //     var base64Image = Convert.ToBase64String(product.PictureProduct.Cover);
        //     product.PictureProduct.Base64Cover = $"data:image/jpeg;base64,{base64Image}";
        // }
        return View(products);
    }

    // Магазин
    public async Task<IActionResult> Shop()
    {
        var products = await _context.Product.ToListAsync();
        return View(products);
    }

    // Страница входа
    public IActionResult SignIn()
    {
        if (!HttpContext.Session.Keys.Contains("AuthUser")) return View();
        ViewData["AuthUser"] = HttpContext.Session.GetString("AuthUser");
        return RedirectToAction("Index", "Home");
    }

    // Обработка входа
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignIn(LoginModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var user = await _context.Users.FirstOrDefaultAsync(u =>
            u.Login == model.Login && u.Password == model.Password);
        if (user != null)
        {
            HttpContext.Session.SetString("AuthUser", model.Login);
            await Authenticate(model.Login);

            // Роли пользователей
            var userRole = user.Roles_ID;
            if (userRole == 1)
                return RedirectToAction("Admin", "Home");
            if (userRole == 2)
                return RedirectToAction("Index", "Home");
            if (userRole == 3)
                return RedirectToAction("Manager", "Home");
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError("", "Неверный логин или пароль");
        return View(model);
    }

    // Аутентификация пользователя
    public async Task Authenticate(string userName)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == userName);
        if (user != null)
        {
            var claims = new List<Claim>
            {
                new(ClaimsIdentity.DefaultNameClaimType, userName),
                new(ClaimTypes.Role, user.Roles_ID.ToString())
            };

            var identity = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));
        }
    }

    // Выход из системы
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        HttpContext.Session.Remove("AuthUser");
        ViewData["AuthUser"] = null;
        return RedirectToAction("SignIn");
    }

    // Страница регистрации
    public IActionResult SignUp()
    {
        return View();
    }

    // Обработка регистрации
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignUp(Users user)
    {
        if (!ModelState.IsValid) return View(user);
        var userRole = await _context.Roles.FirstOrDefaultAsync(role => role.ID_Roles == 2);
        if (userRole != null) user.Roles_ID = userRole.ID_Roles;
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return RedirectToAction("SignIn");
    }

    // Страница конфиденциальности
    public IActionResult Privacy()
    {
        return View();
    }

    // Страница администратора
    public async Task<IActionResult> Admin()
    {
        var users = await _context.Users.ToListAsync();
        return View(users);
    }

    // Страница менеджера
    public async Task<IActionResult> Manager()
    {
        var products = await _context.Product.ToListAsync();
        return View(products);
    }

    public IActionResult CreateProduct()
    {
        return View();
    }

    // Добавление товара
    [HttpPost]
    public async Task<IActionResult> CreateProduct(ProductSite productSite)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var product = new Product();

                product.ProductName = productSite.ProductName;
                product.Quantity = productSite.Quantity;
                product.Price = productSite.Price;
                product.Picture_ID = productSite.Picture_ID;
                product.CategoryProduct_ID = productSite.CategoryProduct_ID;

                _context.Product.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Manager"); // Перенаправляем на страницу управления продуктами
            }

            // Если валидация не прошла, возвращаем объект product обратно на форму
            return BadRequest();
        }
        catch (Exception ex)
        {
            // Обрабатываем исключения, если они возникли
            return StatusCode(500, $"Ошибка: {ex.Message}");
        }
    }


    [HttpPost]
    public async Task<IActionResult> Edit(ProductSite model, int id)
    {
        if (ModelState.IsValid)
        {
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                product.ProductName = model.ProductName;
                product.Price = model.Price;
                product.Quantity = model.Quantity;
                product.CategoryProduct_ID = model.CategoryProduct_ID;
                product.Picture_ID = model.Picture_ID;

                await _context.SaveChangesAsync();
                return RedirectToAction("Manager");
            }

            return NotFound();
        }

        return View(model);
    }


    public async Task<IActionResult> Edit(int id)
    {
        var product = await _context.Product.FindAsync(id);
        if (product != null)
        {
            var productSite = new ProductSite
            {
                ProductName = product.ProductName,
                Price = product.Price,
                Quantity = product.Quantity,
                CategoryProduct_ID = product.CategoryProduct_ID,
                Picture_ID = product.Picture_ID
            };


            return View(productSite);
        }

        return BadRequest();
    }

    // Удаление товара

    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Product.FindAsync(id);
        if (product != null)
        {
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Manager");
        }

        return NotFound("Товар не найден");
    }

    public IActionResult Cart()
    {
        Cart cart = new Cart();
        if (HttpContext.Session.Keys.Contains("Cart"))
        {
            cart = JsonSerializer.Deserialize<Cart>(HttpContext.Session.GetString("Cart"));
        }
        return View(cart);
    }
    
    public IActionResult AddtoCart(int id)
    {
        int ID = id;
        
        
        Cart cart = new Cart();

        if (HttpContext.Session.Keys.Contains("Cart"))
        {
            cart = JsonSerializer.Deserialize<Cart>(HttpContext.Session.GetString("Cart"));
        }
        
        cart.CarLines.Add(_context.Product.Find(ID));
        
        HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));
        
        return RedirectToAction("Index");
    }

    public IActionResult RemeoveAllFromCart(int id)
    {
        Cart cart = new Cart();

        if (HttpContext.Session.Keys.Contains("Cart"))
        {
            cart = JsonSerializer.Deserialize<Cart>(HttpContext.Session.GetString("Cart"));
        }
        
        cart.CarLines.RemoveAt(id);
        
        HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));
        
        return RedirectToAction("Cart");
    }
    
    public IActionResult RemeoveFromCart(int id)
    {
        Cart cart = new Cart();

        if (HttpContext.Session.Keys.Contains("Cart"))
        {
            cart = JsonSerializer.Deserialize<Cart>(HttpContext.Session.GetString("Cart"));
        }
        
        cart?.CarLines.RemoveAll(item => item.ID_Product == id);
        
        HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));
        
        return RedirectToAction("Cart");
    }

    public IActionResult RemoveFullCart()
    {
        Cart cart = new Cart();

        if (HttpContext.Session.Keys.Contains("Cart"))
        {
            cart = JsonSerializer.Deserialize<Cart>(HttpContext.Session.GetString("Cart"));
        }
        
        cart?.CarLines.Clear();
        
        HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));
        
        return RedirectToAction("Cart");
    }
}