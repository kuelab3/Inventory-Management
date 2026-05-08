using InventoryManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Controllers;

public sealed class InventoryController : Controller
{
    private readonly IInventoryService _inventoryService;

    public InventoryController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var model = await _inventoryService.GetDashboardAsync();
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Products()
    {
        var products = await _inventoryService.GetProductsAsync();
        return View(products);
    }

    [HttpGet]
    public async Task<IActionResult> Suppliers()
    {
        var suppliers = await _inventoryService.GetSuppliersAsync();
        return View(suppliers);
    }

    [HttpGet]
    public async Task<IActionResult> Events()
    {
        var model = await _inventoryService.GetDashboardAsync();
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Receive()
    {
        var model = await _inventoryService.GetProductsAsync();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Receive(int productId, int quantity, string receivedBy)
    {
        try
        {
            await _inventoryService.ReceiveProductAsync(productId, quantity, receivedBy);
            TempData["Message"] = "Stock received successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Message"] = ex.Message;
            return RedirectToAction(nameof(Receive));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ScanLowStock()
    {
        await _inventoryService.ScanLowStockAsync();
        TempData["Message"] = "Low stock scan completed.";
        return RedirectToAction(nameof(Index));
    }
}
