using ERPSystem.Models;
using ERPSystem.Repository.Base;
using ERPSystem.ViewModels.Reports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ERPSystem.Controllers
{
    [Authorize(Roles = clsRoles.roleAdmin)]
    public class ReportsController : Controller
    {
        private readonly IStockRepository _stockRepository;
        private readonly ISalesInvoiceRepository _SalesInvoiceRepository;
        private readonly IPurchaseInvoiceRepository _purchaseRepo;


        public ReportsController(IStockRepository stockRepository , ISalesInvoiceRepository SalesInvoiceRepository , IPurchaseInvoiceRepository purchaseRepo)
        {
            _stockRepository = stockRepository;
            _SalesInvoiceRepository = SalesInvoiceRepository;
            _purchaseRepo = purchaseRepo;
        }

        public async Task<IActionResult> Inventory()
        {
            var model = await _stockRepository.GetInventoryReportAsync();

            return View(model);
        }

        public async Task<IActionResult> Sales()
        {
            var data = await _SalesInvoiceRepository.GetSalesReportAsync();

            return View(data);
        }

        public async Task<IActionResult> PurchaseReport()
        {
            var data = await _purchaseRepo.GetPurchaseReportAsync();

            return View(data);
        }


    }
}
