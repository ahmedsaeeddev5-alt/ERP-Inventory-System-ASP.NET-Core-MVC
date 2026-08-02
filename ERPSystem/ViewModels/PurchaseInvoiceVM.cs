using ERPSystem.Models;

namespace ERPSystem.ViewModels
{
    public class PurchaseInvoiceVM
    {
        public string InvoiceNumber { get; set; } = string.Empty;

        public DateTime InvoiceDate { get; set; }

        public int SupplierId { get; set; }

        public int WarehouseId { get; set; }

        public List<PurchaseInvoiceItemVM> Items { get; set; }
            = new();
    }
}
