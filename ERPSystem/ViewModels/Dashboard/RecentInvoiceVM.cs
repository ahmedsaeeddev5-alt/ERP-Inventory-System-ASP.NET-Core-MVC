namespace ERPSystem.ViewModels.Dashboard
{
    public class RecentInvoiceVM
    {
        public int Id { get; set; }

        public string InvoiceNumber { get; set; }

        public DateTime Date { get; set; }

        public decimal Total { get; set; }
    }
}
