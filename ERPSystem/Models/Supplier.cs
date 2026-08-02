using System.ComponentModel.DataAnnotations;

namespace ERPSystem.Models
{
    public class Supplier
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? Address { get; set; }
        public string? TaxNumber { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<PurchaseInvoice> PurchaseInvoices { get; set; } = new List<PurchaseInvoice>();
    }
}
