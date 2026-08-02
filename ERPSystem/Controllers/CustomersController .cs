using ERPSystem.Models;
using ERPSystem.Repository.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERPSystem.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        private readonly IUnitOfWork _myUnit;
        public CustomersController(IUnitOfWork myUnit)
        {
            _myUnit = myUnit;
        }
        // GET: Customer
        public async Task<IActionResult> Index(string? search)
        {
            var Customers = await _myUnit.customers.SearchAsync(search);
            return View(Customers);
        }

        // GET: Customer/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var Customer = await _myUnit.customers.GetByIdAsync(id);

            if (Customer == null)
                return NotFound();

            return View(Customer);
        }

        // GET: Customer/New
        public IActionResult New()
        {
            return View();
        }

        // POST: Customer/New
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(Customer customer)
        {
            if (!ModelState.IsValid)
                return View(customer);

            await _myUnit.customers.AddAsync(customer);
            await _myUnit.SaveAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Customer/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var Customer = await _myUnit.customers.GetByIdAsync(id);

            if (Customer == null)
                return NotFound();

            return View(Customer);
        }

        // POST: Customer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Customer customer)
        {
            if (id != customer.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(customer);

            _myUnit.customers.Update(customer);
            await _myUnit.SaveAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Customer/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _myUnit.customers.GetByIdAsync(id);

            if (customer == null)
                return NotFound();

            return View(customer);
        }

        // POST: Supplier/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _myUnit.customers.GetByIdAsync(id);

            if (customer == null)
                return NotFound();

            _myUnit.customers.Delete(customer);
            await _myUnit.SaveAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
