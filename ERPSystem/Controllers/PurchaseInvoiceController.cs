using ERPSystem.Models;
using ERPSystem.Repository.Base;
using ERPSystem.Services;
using ERPSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ERPSystem.Controllers
{
    [Authorize]
    public class PurchaseInvoiceController : Controller
    {
        private readonly IPurchaseInvoiceService _purchaseInvoiceService;
        private readonly IUnitOfWork _unitOfWork;

        public PurchaseInvoiceController(
            IPurchaseInvoiceService purchaseInvoiceService,
            IUnitOfWork unitOfWork)
        {
            _purchaseInvoiceService = purchaseInvoiceService;
            _unitOfWork = unitOfWork;
        }


        // GET: PurchaseInvoice
        public async Task<IActionResult> Index()
        {
            var invoices = await _purchaseInvoiceService.GetAllAsync();

            return View(invoices);
        }


        // GET: PurchaseInvoice/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var invoice = await _purchaseInvoiceService.GetByIdAsync(id);

            if (invoice == null)
                return NotFound();

            return View(invoice);
        }


        // GET: PurchaseInvoice/Create
        public async Task<IActionResult> Create()
        {
            await LoadData();

            return View(new PurchaseInvoiceVM());
        }


        // POST: PurchaseInvoice/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PurchaseInvoiceVM model)
        {
            if (!ModelState.IsValid)
            {
                await LoadData();
                return View(model);
            }


            var invoice = new PurchaseInvoice
            {
                InvoiceNumber = model.InvoiceNumber,

                InvoiceDate = model.InvoiceDate,

                SupplierId = model.SupplierId,

                WarehouseId = model.WarehouseId,


                Items = model.Items.Select(x => new PurchaseInvoiceItem
                {
                    ProductId = x.ProductId,

                    Quantity = x.Quantity,

                    Price = x.Price

                }).ToList()
            };


            await _purchaseInvoiceService.CreateAsync(invoice);


            return RedirectToAction(nameof(Index));
        }



        // GET: PurchaseInvoice/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var invoice = await _purchaseInvoiceService.GetByIdAsync(id);


            if (invoice == null)
                return NotFound();



            var model = new PurchaseInvoiceVM
            {
                InvoiceNumber = invoice.InvoiceNumber,

                InvoiceDate = invoice.InvoiceDate,

                SupplierId = invoice.SupplierId,

                WarehouseId = invoice.WarehouseId,


                Items = invoice.Items.Select(x => new PurchaseInvoiceItemVM
                {
                    ProductId = x.ProductId,

                    Quantity = x.Quantity,

                    Price = x.Price

                }).ToList()
            };


            await LoadData();


            return View(model);
        }



        // POST: PurchaseInvoice/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PurchaseInvoiceVM model)
        {
            if (!ModelState.IsValid)
            {
                await LoadData();
                return View(model);
            }


            var invoice = new PurchaseInvoice
            {
                Id = id,

                InvoiceNumber = model.InvoiceNumber,

                InvoiceDate = model.InvoiceDate,

                SupplierId = model.SupplierId,

                WarehouseId = model.WarehouseId,


                Items = model.Items.Select(x => new PurchaseInvoiceItem
                {
                    ProductId = x.ProductId,

                    Quantity = x.Quantity,

                    Price = x.Price

                }).ToList()
            };


            await _purchaseInvoiceService.UpdateAsync(invoice);


            return RedirectToAction(nameof(Index));
        }



        // GET: PurchaseInvoice/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var invoice = await _purchaseInvoiceService.GetByIdAsync(id);


            if (invoice == null)
                return NotFound();


            return View(invoice);
        }



        // POST: PurchaseInvoice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _purchaseInvoiceService.DeleteAsync(id);


            return RedirectToAction(nameof(Index));
        }



        private async Task LoadData()
        {
            ViewBag.Suppliers = new SelectList(
                await _unitOfWork.Suppliers.GetAllAsync(),
                "Id",
                "Name"
            );


            ViewBag.Warehouses = new SelectList(
                await _unitOfWork.warehouse.GetAllAsync(),
                "Id",
                "Name"
            );


            ViewBag.Products = new SelectList(
                await _unitOfWork.products.GetAllAsync(),
                "Id",
                "Name"
            );
        }
    }
}
