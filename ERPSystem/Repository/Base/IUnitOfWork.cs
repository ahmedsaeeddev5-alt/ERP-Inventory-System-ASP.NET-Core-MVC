using ERPSystem.Models;

namespace ERPSystem.Repository.Base
{
    public interface IUnitOfWork
    {
        ICategoryRepository categories { get; }
        IProductRepository products { get; }
        IUnitRepository units { get; }
        IWarehouseRepository warehouse { get; }
        IStockRepository Stocks { get; }
        ISupplierRepository Suppliers { get; }
        IPurchaseInvoiceRepository PurchaseInvoices { get; }
        ICustomerRepository customers { get; }
        ISalesInvoiceRepository SalesInvoices { get; }
        Task<int> SaveAsync();
    }
}
