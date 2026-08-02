using ERPSystem.Models;
using ERPSystem.Repository.Base;
using ERPSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ERPSystem.Controllers
{
    [Authorize]
    public class StockController : Controller
    {
        private readonly IUnitOfWork _myUnit;
        private readonly ILogger<StockController> _logger;

        public StockController(
            IUnitOfWork myUnit,
            ILogger<StockController> logger)
        {
            _myUnit = myUnit;
            _logger = logger;
        }

        private async Task LoadDataAsync(OpeningStockViewModel model)
        {
            var products = await _myUnit.products.GetAllAsync();

            var warehouses = await _myUnit.warehouse.GetAllAsync();

            model.Products = products.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            });

            model.Warehouses = warehouses.Select(w => new SelectListItem
            {
                Value = w.Id.ToString(),
                Text = w.Name
            });
        }

        public async Task<IActionResult> Index(string? search)
        {
            IEnumerable<Stock> stocks;

            if (string.IsNullOrWhiteSpace(search))
            {
                stocks = await _myUnit.Stocks.GetAllWithDetailsAsync();
            }
            else
            {
                stocks = await _myUnit.Stocks.SearchAsync(search);
            }

            ViewBag.Search = search;

            return View(stocks);
        }
       
        //Get
        public async Task<IActionResult> Create()
        {
            var model = new OpeningStockViewModel();

            await LoadDataAsync(model);

            return View(model);
        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OpeningStockViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadDataAsync(model);
                return View(model);
            }

            var stock = await _myUnit.Stocks
                .GetByProductAndWarehouseAsync(
                    model.ProductId,
                    model.WarehouseId);

            if (stock != null)
            {
                ModelState.AddModelError(
                    "",
                    "This product already exists in this warehouse.");

                await LoadDataAsync(model);

                return View(model);
            }

            Stock newStock = new()
            {
                ProductId = model.ProductId,
                WarehouseId = model.WarehouseId,
                Quantity = model.Quantity
            };

            await _myUnit.Stocks.AddAsync(newStock);

            await _myUnit.SaveAsync();

            _logger.LogInformation(
                "Opening Stock added. ProductId:{ProductId}, WarehouseId:{WarehouseId}, Quantity:{Quantity}",
                model.ProductId,
                model.WarehouseId,
                model.Quantity);

            TempData["SuccessData"] =
                "Opening Stock added successfully.";

            return RedirectToAction(nameof(Index));
        }

    }
}
