using ERPSystem.Models;
using ERPSystem.Repository;
using ERPSystem.Repository.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERPSystem.Controllers
{
    [Authorize]
    public class SupplierController : Controller
    {
        
        private readonly IUnitOfWork _myUnit;
        public SupplierController(IUnitOfWork myUnit)
        {
            _myUnit = myUnit;
        }

        // GET: Supplier
        public async Task<IActionResult> Index(string? search)
        {
            var suppliers = await _myUnit.Suppliers.SearchAsync(search);
            return View(suppliers);
        }

        // GET: Supplier/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var supplier = await _myUnit.Suppliers.GetByIdAsync(id);

            if (supplier == null)
                return NotFound();

            return View(supplier);
        }

        // GET: Supplier/New
        public IActionResult New()
        {
            return View();
        }

        // POST: Supplier/New
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(Supplier supplier)
        {
            if (!ModelState.IsValid)
                return View(supplier);

            await _myUnit.Suppliers.AddAsync(supplier);
            await _myUnit.SaveAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Supplier/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var supplier = await _myUnit.Suppliers.GetByIdAsync(id);

            if (supplier == null)
                return NotFound();

            return View(supplier);
        }

        // POST: Supplier/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Supplier supplier)
        {
            if (id != supplier.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(supplier);

            _myUnit.Suppliers.Update(supplier);
            await _myUnit.SaveAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Supplier/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var supplier = await _myUnit.Suppliers.GetByIdAsync(id);

            if (supplier == null)
                return NotFound();

            return View(supplier);
        }

        // POST: Supplier/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var supplier = await _myUnit.Suppliers.GetByIdAsync(id);

            if (supplier == null)
                return NotFound();

            _myUnit.Suppliers.Delete(supplier);
            await _myUnit .SaveAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}

