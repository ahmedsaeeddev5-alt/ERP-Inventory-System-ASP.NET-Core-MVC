namespace ERPSystem.ViewModels.Reports
{
    public class InventoryReportVM
    {
        public string ProductName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string UnitName { get; set; } = string.Empty;
        public string WarehouseName { get; set; } = string.Empty;

        public decimal Quantity { get; set; }

        public decimal PurchasePrice { get; set; }

        public decimal TotalValue => Quantity * PurchasePrice;
    }
}
