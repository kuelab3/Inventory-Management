using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Controllers;

public sealed class HomeController : Controller
{
    public IActionResult Index()
    {
        return RedirectToAction("Index", "Inventory");
    }

    [HttpGet]
    public IActionResult Error()
    {
        return View();
    }
}
