using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSystem.Models
{
    public class SalesInvoice
    {
        public int Id { get; set; }

        public DateTime InvoiceDate { get; set; } = DateTime.Now;
        public int WarehouseId { get; set; }

        public Warehouse? Warehouse { get; set; }
        [Required]
        [StringLength(50)]
        public string InvoiceNumber { get; set; }
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        public Customer? Customer { get; set; }

        public decimal TotalAmount { get; set; }

        public ICollection<SalesInvoiceItem> Items { get; set; } = new List<SalesInvoiceItem>();
    }
}
