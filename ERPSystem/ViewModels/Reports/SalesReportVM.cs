namespace ERPSystem.ViewModels.Reports
{
    public class SalesReportVM
    {
        public int InvoiceNumber { get; set; }

        public DateTime InvoiceDate { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public string ProductName { get; set; } = string.Empty;

        public decimal Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Total
        {
            get
            {
                return Quantity * UnitPrice;
            }
        }
    }
}
