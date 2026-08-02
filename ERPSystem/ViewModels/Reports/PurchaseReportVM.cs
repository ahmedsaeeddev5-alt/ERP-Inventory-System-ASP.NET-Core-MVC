namespace ERPSystem.ViewModels.Reports
{
    public class PurchaseReportVM
    {
        public string InvoiceNumber { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        public string SupplierName { get; set; } = string.Empty;

        public decimal Total { get; set; }
    }
}
