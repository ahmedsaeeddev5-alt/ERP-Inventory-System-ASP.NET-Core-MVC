namespace ERPSystem.ViewModels
{
    public class SalesInvoiceItemVM
    {
        public bool Selected { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }
    }
}
