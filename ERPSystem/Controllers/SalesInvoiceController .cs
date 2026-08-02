using ERPSystem.Models;
using ERPSystem.Repository.Base;
using ERPSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERPSystem.Controllers
{
    [Authorize]
    public class SalesInvoiceController : Controller
    {
    
        private readonly IUnitOfWork _unitOfWork;

        public SalesInvoiceController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        // GET: SalesInvoice
        public async Task<IActionResult> Index()
        {
            var invoices = await _unitOfWork.SalesInvoices
                .GetAllWithDetailsAsync();

            return View(invoices);
        }



        // GET: SalesInvoice/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var invoice = await _unitOfWork.SalesInvoices
                .GetByIdWithDetailsAsync(id);


            if (invoice == null)
                return NotFound();


            return View(invoice);
        }



        // GET: SalesInvoice/New
        public async Task<IActionResult> New()
        {
            var products = await _unitOfWork.products.GetAllAsync();

            var vm = new SalesInvoiceVM
            {
                Customers = await _unitOfWork.customers.GetAllAsync(),
                Products = products,
                Warehouses = await _unitOfWork.warehouse.GetAllAsync(),

                Items = products.Select(p => new SalesInvoiceItemVM
                {
                    ProductId = p.Id,
                    UnitPrice = p.SalePrice,
                    Quantity = 1
                }).ToList()
            };

            return View(vm);
        }



        // POST: SalesInvoice/New

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(SalesInvoiceVM vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Customers = await _unitOfWork.customers.GetAllAsync();
                vm.Products = await _unitOfWork.products.GetAllAsync();
                vm.Warehouses = await _unitOfWork.warehouse.GetAllAsync();

                return View(vm);
            }

            vm.Invoice.Items = new List<SalesInvoiceItem>();

            foreach (var item in vm.Items.Where(x => x.Selected))
            {
                var product = await _unitOfWork.products.GetByIdAsync(item.ProductId);

                if (product == null)
                    continue;

                vm.Invoice.Items.Add(new SalesInvoiceItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = product.SalePrice
                });

                var stock = await _unitOfWork.Stocks
                    .GetByProductAndWarehouseAsync(item.ProductId, vm.Invoice.WarehouseId);

                if (stock == null)
                {
                    ModelState.AddModelError("", $"المنتج {product.Name} غير موجود في هذا المخزن.");

                    vm.Customers = await _unitOfWork.customers.GetAllAsync();
                    vm.Products = await _unitOfWork.products.GetAllAsync();
                    vm.Warehouses = await _unitOfWork.warehouse.GetAllAsync();

                    return View(vm);
                }

                if (stock.Quantity < item.Quantity)
                {
                    ModelState.AddModelError("", $"الكمية المتاحة من {product.Name} هي {stock.Quantity} فقط.");

                    vm.Customers = await _unitOfWork.customers.GetAllAsync();
                    vm.Products = await _unitOfWork.products.GetAllAsync();
                    vm.Warehouses = await _unitOfWork.warehouse.GetAllAsync();

                    return View(vm);
                }

                stock.Quantity -= item.Quantity;
                _unitOfWork.Stocks.Update(stock);
            }
            vm.Invoice.TotalAmount = vm.Invoice.Items.Sum(x => x.Total);

            await _unitOfWork.SalesInvoices.AddAsync(vm.Invoice);

            await _unitOfWork.SaveAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: SalesInvoice/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var invoice = await _unitOfWork.SalesInvoices
                .GetByIdWithDetailsAsync(id);


            if (invoice == null)
                return NotFound();


            return View(invoice);
        }



        // POST: SalesInvoice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoice = await _unitOfWork.SalesInvoices
                .GetByIdAsync(id);


            if (invoice == null)
                return NotFound();


            _unitOfWork.SalesInvoices.Delete(invoice);

            await _unitOfWork.SaveAsync();


            return RedirectToAction(nameof(Index));
        }
    }
}

