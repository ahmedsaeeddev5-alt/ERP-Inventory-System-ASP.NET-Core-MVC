namespace ERPSystem.ViewModels.Dashboard
{
    public class DashboardVM
    {
        // Counts
        public int ProductsCount { get; set; }

        public int CustomersCount { get; set; }

        public int SuppliersCount { get; set; }

        public int SalesInvoicesCount { get; set; }

        public int PurchaseInvoicesCount { get; set; }


        // Financial Summary
        public decimal TotalSales { get; set; }

        public decimal TotalPurchases { get; set; }

        public decimal StockValue { get; set; }


        // Chart Data
        public List<string> Months { get; set; } = new();

        public List<decimal> SalesData { get; set; } = new();

        public List<decimal> PurchasesData { get; set; } = new();
        public List<LowStockVM> LowStockProducts { get; set; } = new();
    }
}

