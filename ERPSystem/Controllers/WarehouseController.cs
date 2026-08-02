using ERPSystem.Models;
using ERPSystem.Repository.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERPSystem.Controllers
{
    [Authorize]
    public class WarehouseController : Controller
    {
    

        private readonly ILogger<WarehouseController> _logger;
        private readonly IUnitOfWork _myUnit;
        public WarehouseController(IUnitOfWork myUnit, ILogger<WarehouseController> logger)
        {
            _myUnit = myUnit;
            _logger = logger;
        }
        public async Task<IActionResult> Index(string? search)
        {
            IEnumerable<Warehouse> warehouses;

            if (string.IsNullOrWhiteSpace(search))
            {
                warehouses = await _myUnit.warehouse.GetAllWithDetailsAsync();
            }
            else
            {
                warehouses = await _myUnit.warehouse.SearchAsync(search);
            }

            ViewBag.Search = search;

            return View(warehouses);
        }

        //Get
        public IActionResult New()
        {
            return View();
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(Warehouse warehouse)
        {
            if (!ModelState.IsValid)
            {
                return View(warehouse);
            }

            var existingwarehouse = await _myUnit.warehouse.GetByNameAsync(warehouse.Name);

            if (existingwarehouse != null)
            {
                ModelState.AddModelError(nameof(Warehouse.Name),
                   "Warehouse name already exists.");
                return View(warehouse);
            }

            await _myUnit.warehouse.AddAsync(warehouse);
            await _myUnit.SaveAsync();

            _logger.LogInformation(
            "Warehouse '{WarehouseName}' created successfully.",
                warehouse.Name);

            TempData["SuccessData"] = "warehouse has been added successfully.";

            return RedirectToAction(nameof(Index));
        }

        //Get
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var warehouse = await _myUnit.warehouse.GetByIdAsync(id.Value);
            if (warehouse == null)
            {
                return NotFound();
            }
            return View(warehouse);
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Warehouse warehouse)
        {
            if (!ModelState.IsValid)
                return View(warehouse);

            var existingwarehouse = await _myUnit.warehouse.GetByNameAsync(warehouse.Name);

            if (existingwarehouse != null && existingwarehouse.Id != warehouse.Id)
            {
                ModelState.AddModelError(nameof(Warehouse.Name),
     "Warehouse name already exists.");
                return View(warehouse);
            }

            var warehouseFromDb = await _myUnit.warehouse.GetByIdAsync(warehouse.Id);

            if (warehouseFromDb == null)
                return NotFound();

            warehouseFromDb.Name = warehouse.Name;
            warehouseFromDb.Address = warehouse.Address;

            await _myUnit.SaveAsync();
            _logger.LogInformation(
    "Warehouse '{WarehouseName}' updated successfully.",
    warehouse.Name);

            TempData["SuccessData"] = "warehouse updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        //Get
        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var warehouse = await _myUnit.warehouse.GetByIdAsync(Id.Value);
            if (warehouse == null)
            {
                return NotFound();
            }
            return View(warehouse);
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var warehouse = await _myUnit.warehouse.GetByIdAsync(id);

            if (warehouse == null)
                return NotFound();

            _myUnit.warehouse.Delete(warehouse);

            await _myUnit.SaveAsync();
            _logger.LogInformation(
    "Warehouse '{WarehouseName}' deleted successfully.",
    warehouse.Name);

            TempData["SuccessData"] = "Warehouse deleted successfully.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var warehouse = await _myUnit.warehouse.GetByIdWithDetailsAsync(id.Value);

            if (warehouse == null)
            {
                return NotFound();
            }

            return View(warehouse);
        }

    }
}

