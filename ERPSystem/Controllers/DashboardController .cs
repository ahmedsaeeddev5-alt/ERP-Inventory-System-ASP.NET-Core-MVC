using ERPSystem.Models;
using ERPSystem.Repository.Base;
using ERPSystem.ViewModels.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERPSystem.Controllers
{
    [Authorize(Roles = clsRoles.roleAdmin)]
    public class DashboardController : Controller
    {
        private readonly IProductRepository _productRepo;
        private readonly ICustomerRepository _customerRepo;
        private readonly ISupplierRepository _supplierRepo;
        private readonly ISalesInvoiceRepository _salesRepo;
        private readonly IPurchaseInvoiceRepository _purchaseRepo;
        private readonly IStockRepository _stockRepo;


        public DashboardController(
            IProductRepository productRepo,
            ICustomerRepository customerRepo,
            ISupplierRepository supplierRepo,
            ISalesInvoiceRepository salesRepo,
            IPurchaseInvoiceRepository purchaseRepo,
            IStockRepository stockRepo)
        {
            _productRepo = productRepo;
            _customerRepo = customerRepo;
            _supplierRepo = supplierRepo;
            _salesRepo = salesRepo;
            _purchaseRepo = purchaseRepo;
            _stockRepo = stockRepo; 
        }


        public async Task<IActionResult> Index()
        {
            var monthlyData = await _purchaseRepo.GetMonthlyPurchasesAsync();

            DashboardVM model = new DashboardVM
            {
                ProductsCount = await _productRepo.CountAsync(),

                CustomersCount = await _customerRepo.CountAsync(),

                SuppliersCount = await _supplierRepo.CountAsync(),

                SalesInvoicesCount = await _salesRepo.CountAsync(),

                PurchaseInvoicesCount = await _purchaseRepo.CountAsync(),

                TotalSales = await _salesRepo.GetTotalSalesAsync(),

                TotalPurchases = await _purchaseRepo.GetTotalPurchasesAsync(),

                StockValue = await _stockRepo.GetStockValueAsync(),
                LowStockProducts =
        await _stockRepo.GetLowStockProductsAsync(),

                Months = monthlyData.Select(x => x.Month).ToList(),

                SalesData = monthlyData.Select(x => x.TotalSales).ToList(),

                PurchasesData = monthlyData.Select(x => x.TotalPurchases).ToList()
            };

            return View(model);
        }
    }
}
