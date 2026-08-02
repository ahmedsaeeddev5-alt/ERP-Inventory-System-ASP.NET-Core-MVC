using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSystem.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;

        public string Code { get; set; } = null!;

        public string Barcode { get; set; } = null!;

        public decimal PurchasePrice { get; set; }

        public decimal SalePrice { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [NotMapped]
        public IFormFile clientfile { get; set; }
        public byte[]? dbImage { get; set; }         //وليست مسارًا للصورة (Binary Data)  تُستخدم لتخزين صورة داخل قاعدة البيانات على هيئة مصفوفة من البايتات 
        [NotMapped]
        public string? imagesrc
        {
            get                //  get يقوم البرنامج بتنفيذ كل الكود الموجود داخل item.imagesrc أي عندما تكتب
            {
                if (dbImage != null)       // dbImage هنا يتم التأكد أولا هل يوجد صورة داخل
                {
                    string base64string = Convert.ToBase64String(dbImage, 0, dbImage.Length);    // string  الي  byte[] تحويل ال  
                                                                                                 // dbImage مصفوفة البايتات
                                                                                                 //dbImage.Length اقرأ كل البايتات
                                                                                                 // 0   Byte يعني ابدأ من أول 
                    return $"data:image/jpeg;base64,{Convert.ToBase64String(dbImage)}";      //              للتأكيد للمتصفح أن هذا النص يمثل صورة
                }
                else
                {
                    return string.Empty;
                }
            }

        }

        [ForeignKey("category")]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }
        [ForeignKey("Unit")]
        public int UnitId { get; set; }

        public Unit? Unit { get; set; }

        // Stock
        public ICollection<Stock> Stocks { get; set; } = new List<Stock>();

        // Purchase Invoice Details
        public ICollection<PurchaseInvoiceItem> PurchaseInvoiceItems { get; set; } = new List<PurchaseInvoiceItem>();

        // Sales Invoice Details
        public ICollection<SalesInvoiceItem> SalesInvoiceItems { get; set; } = new List<SalesInvoiceItem>();
    }
}
