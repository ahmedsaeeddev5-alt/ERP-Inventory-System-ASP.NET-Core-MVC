using ERPSystem.Models;
using ERPSystem.Repository.Base;

namespace ERPSystem.Services
{
    public class PurchaseInvoiceService : IPurchaseInvoiceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PurchaseInvoiceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task CreateAsync(PurchaseInvoice invoice)
        {
            if (invoice.Items == null || !invoice.Items.Any())
                throw new Exception("Invoice must contain items.");

            // حساب إجمالي الفاتورة
            invoice.TotalAmount = invoice.Items.Sum(x => x.Quantity * x.Price);

            // إضافة الفاتورة
            await _unitOfWork.PurchaseInvoices.AddAsync(invoice);

            // تحديث المخزون
            foreach (var item in invoice.Items)
            {
                var stock = await _unitOfWork.Stocks
                    .GetByProductAndWarehouseAsync(
                        item.ProductId,
                        invoice.WarehouseId
                    );

                if (stock == null)
                {
                    var newStock = new Stock
                    {
                        ProductId = item.ProductId,
                        WarehouseId = invoice.WarehouseId,
                        Quantity = item.Quantity
                    };

                    await _unitOfWork.Stocks.AddAsync(newStock);
                }
                else
                {
                    stock.Quantity += item.Quantity;

                    _unitOfWork.Stocks.Update(stock);
                }
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var invoice =
                await _unitOfWork.PurchaseInvoices
                .GetByIdWithDetailsAsync(id);

            if (invoice == null)
                throw new Exception("Invoice not found.");

            _unitOfWork.PurchaseInvoices.Delete(invoice);

            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<PurchaseInvoice>> GetAllAsync()
        {
            return await _unitOfWork.PurchaseInvoices.GetAllWithDetailsAsync();
        }

        public async Task<PurchaseInvoice?> GetByIdAsync(int id)
        {
            return await _unitOfWork.PurchaseInvoices.GetByIdWithDetailsAsync(id);
        }
        public async Task UpdateAsync(PurchaseInvoice invoice)
        {
            var existingInvoice =
                await _unitOfWork.PurchaseInvoices
                .GetByIdWithDetailsAsync(invoice.Id);

            if (existingInvoice == null)
                throw new Exception("Invoice not found.");

            invoice.TotalAmount =
                invoice.Items.Sum(x => x.Quantity * x.Price);

            _unitOfWork.PurchaseInvoices.Update(invoice);

            await _unitOfWork.SaveAsync();
        }
    }
}
