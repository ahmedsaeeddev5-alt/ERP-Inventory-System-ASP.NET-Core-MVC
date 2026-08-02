using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSystem.Models
{
    public class SalesInvoiceItem
    {
        public int Id { get; set; }

        [ForeignKey("SalesInvoice")]
        public int SalesInvoiceId { get; set; }

        public SalesInvoice? SalesInvoice { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public Product? Product { get; set; }

        public decimal Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Total => Quantity * UnitPrice;
    }
}
