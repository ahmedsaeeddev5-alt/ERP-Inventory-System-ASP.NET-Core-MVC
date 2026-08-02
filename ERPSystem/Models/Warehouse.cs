using System.Collections;

namespace ERPSystem.Models
{
    public class Warehouse
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Address { get; set; }

        public ICollection<Stock> Stocks { get; set; } = new List<Stock>();
        public ICollection<PurchaseInvoice> PurchaseInvoices { get; set; }
    = new List<PurchaseInvoice>();
    }
}
