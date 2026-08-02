using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSystem.Models
{
    public class Stock
    {
        public int Id { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public Product? Product { get; set; }

        [ForeignKey("Warehouse")]
        public int WarehouseId { get; set; }

        public Warehouse? Warehouse { get; set; }

        public decimal Quantity { get; set; }
    }
}
