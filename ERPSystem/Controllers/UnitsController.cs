using ERPSystem.Models;
using ERPSystem.Repository.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERPSystem.Controllers
{
    [Authorize]
    public class UnitsController : Controller
    {
       

        private readonly ILogger<UnitsController> _logger;
        private readonly IUnitOfWork _myUnit;
        public UnitsController(IUnitOfWork myUnit, ILogger<UnitsController> logger)
        {
            _myUnit = myUnit;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string? search)
        {
            IEnumerable<Unit> units;

            if (string.IsNullOrWhiteSpace(search))
            {
                units = await _myUnit.units.GetAllAsync();
            }
            else
            {
                units = await _myUnit.units.SearchAsync(search);
            }

            ViewBag.Search = search;

            return View(units);
        }

        // GET
        public IActionResult New()
        {
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(Unit unit)
        {
            if (!ModelState.IsValid)
                return View(unit);

            var existingUnit = await _myUnit.units.GetByNameAsync(unit.Name);

            if (existingUnit != null)
            {
                ModelState.AddModelError("Name", "Unit name already exists.");
                return View(unit);
            }

            await _myUnit.units.AddAsync(unit);
            await _myUnit.SaveAsync();

            _logger.LogInformation("Unit '{UnitName}' created successfully.", unit.Name);

            TempData["SuccessData"] = "Unit has been added successfully.";

            return RedirectToAction(nameof(Index));
        }

        // GET
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var unit = await _myUnit.units.GetByIdAsync(id.Value);

            if (unit == null)
                return NotFound();

            return View(unit);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Unit unit)
        {
            if (!ModelState.IsValid)
                return View(unit);

            var existingUnit = await _myUnit.units.GetByNameAsync(unit.Name);

            if (existingUnit != null && existingUnit.Id != unit.Id)
            {
                ModelState.AddModelError("Name", "Unit name already exists.");
                return View(unit);
            }

            var unitFromDb = await _myUnit.units.GetByIdAsync(unit.Id);

            if (unitFromDb == null)
                return NotFound();

            unitFromDb.Name = unit.Name;

            await _myUnit.SaveAsync();

            TempData["SuccessData"] = "Unit updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        // GET
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var unit = await _myUnit.units.GetByIdAsync(id.Value);

            if (unit == null)
                return NotFound();

            return View(unit);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Unit unit)
        {
            var unitFromDb = await _myUnit.units.GetByIdAsync(unit.Id);

            if (unitFromDb == null)
                return NotFound();

            _myUnit.units.Delete(unitFromDb);
            await _myUnit.SaveAsync();

            TempData["SuccessData"] = "Unit deleted successfully.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var unit = await _myUnit.units.GetByIdAsync(id.Value);

            if (unit == null)
                return NotFound();

            return View(unit);
        }
    }
}
