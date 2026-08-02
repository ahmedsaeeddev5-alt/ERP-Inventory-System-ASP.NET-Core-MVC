using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ERPSystem.ViewModels
{
    public class OpeningStockViewModel
    {
        [Required]
        [Display(Name = "Product")]
        public int ProductId { get; set; }

        [Required]
        [Display(Name = "Warehouse")]
        public int WarehouseId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue,
            ErrorMessage = "Quantity must be greater than zero.")]
        public decimal Quantity { get; set; }

        public IEnumerable<SelectListItem> Products { get; set; }
            = new List<SelectListItem>();

        public IEnumerable<SelectListItem> Warehouses { get; set; }
            = new List<SelectListItem>();
    }
}
