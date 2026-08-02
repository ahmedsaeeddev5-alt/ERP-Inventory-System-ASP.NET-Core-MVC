using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSystem.Models
{
    public class PurchaseInvoice
    {
        public int Id { get; set; }

         public string InvoiceNumber { get; set; } = string.Empty;

        public DateTime InvoiceDate { get; set; } = DateTime.Now;
        [ForeignKey("Supplier")]
        public int SupplierId { get; set; }

        public Supplier? Supplier { get; set; }

        public decimal TotalAmount { get; set; }
        public int WarehouseId { get; set; }

        public Warehouse? Warehouse { get; set; }

        public ICollection<PurchaseInvoiceItem> Items { get; set; } = new List<PurchaseInvoiceItem>();
    }
}
