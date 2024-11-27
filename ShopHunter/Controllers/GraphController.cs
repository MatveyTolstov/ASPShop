using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace ShopHunter.Controllers;

public class GraphController : Controller
{
    private readonly ApplicationDbContext _context;

    private readonly ILogger<HomeController> _logger;

    public GraphController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    public IActionResult BarChart()
    {
        var salesData = _context.Orders
            .GroupBy(o => o.Date.Date) 
            .Select(g => new
            {
                Date = g.Key.ToShortDateString(),
                TotalSales = g.Count()
            })
            .ToList();


        return Json(salesData); 
    }
    
    public IActionResult PieChart()
    {
        var productData = _context.Orders
            .GroupBy(o => o.Product_ID)
            .Select(g => new
            {
                Product = _context.Product
                    .Where(p => p.ID_Product == g.Key)
                    .Select(p => p.ProductName)
                    .FirstOrDefault(),
                Sales = g.Count()
            })
            .ToList();

        return Json(productData);
    }

    public IActionResult Graphs()
    {
        return View();
    }
    
}