using ERPSystem.Data;
using ERPSystem.Repository.Base;

namespace ERPSystem.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ERPDbContext _context;
        public ICategoryRepository categories { get; private set; }
        public IProductRepository products { get; private set; }
        public IUnitRepository units { get; private set; }

        public IWarehouseRepository warehouse { get; private set; }

        public IStockRepository Stocks { get; private set; }
        public ISupplierRepository Suppliers { get; private set; }
        public IPurchaseInvoiceRepository PurchaseInvoices { get; private set; }
        public ICustomerRepository customers { get; private set; }
        public ISalesInvoiceRepository SalesInvoices { get; private set; }

        public UnitOfWork(ERPDbContext context)
        {
            _context = context;
            categories = new CategoryRepository(_context);
            products = new ProductRepository(_context);
            units = new UnitRepository(_context);
            warehouse = new WarehouseRepository(_context);
            Stocks = new StockRepository(_context);
            Suppliers = new SupplierRepository(_context);
            PurchaseInvoices = new PurchaseInvoiceRepository(_context);
            customers = new CustomerRepository(_context);
            SalesInvoices = new SalesInvoiceRepository(_context);

        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
