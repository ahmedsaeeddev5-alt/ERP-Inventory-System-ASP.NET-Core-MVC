using ERPSystem.Models;

namespace ERPSystem.ViewModels
{
    public class SalesInvoiceVM
    {
        public SalesInvoice Invoice { get; set; } = new();

        public IEnumerable<Customer> Customers { get; set; } = new List<Customer>();

        public IEnumerable<Product> Products { get; set; } = new List<Product>();
        public IEnumerable<Warehouse> Warehouses { get; set; } = new List<Warehouse>();

        public List<int> ProductIds { get; set; } = new();

        public List<int> Quantities { get; set; } = new();
        public List<SalesInvoiceItemVM> Items { get; set; } = new();

    }
}
