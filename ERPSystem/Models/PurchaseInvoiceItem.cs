using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSystem.Models
{
    public class PurchaseInvoiceItem
    {
        public int Id { get; set; }
        [ForeignKey("PurchaseInvoice")]
        public int PurchaseInvoiceId { get; set; }

        public PurchaseInvoice? PurchaseInvoice { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public Product? Product { get; set; }

        public decimal Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal Total => Quantity * Price;
    }
}
