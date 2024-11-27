using Microsoft.AspNetCore.Mvc;
using ShopHunter.Models.BDConn;

namespace ShopHunter.Controllers;

public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;

    private readonly ILogger<HomeController> _logger;

    public AdminController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }


    public IActionResult AddUser()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddUser(UserSite usersSite)
    {
        if (ModelState.IsValid)
        {
            var user = new Users()
            {
                Login = usersSite.Login,
                Password = usersSite.Password,
                Roles_ID = usersSite.Roles_ID,
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Admin", "Home");
        }

        return BadRequest(ModelState);
    }


    [HttpPost]
    public async Task<IActionResult> EditUser(UserSite model, int id)
    {
        if (ModelState.IsValid)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.Login = model.Login;
                user.Password = model.Password;
                user.Roles_ID = model.Roles_ID;
                await _context.SaveChangesAsync();
                return RedirectToAction("Admin", "Home");
            }

            return NotFound();
        }

        return View(model);
    }


    public async Task<IActionResult> EditUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            var usersSite = new UserSite
            {
                Login = user.Login,
                Password = user.Password,
                Roles_ID = user.Roles_ID,
            };

            return View(usersSite);
        }
        

        return BadRequest();
    }

    public async Task<IActionResult> DeleteUser(int id)
    {
        var users = await _context.Users.FindAsync(id);
        if (users != null)
        {
            _context.Users.Remove(users);
            await _context.SaveChangesAsync();
            return RedirectToAction("Admin", "Home");
        }

        return NotFound();
    }
}