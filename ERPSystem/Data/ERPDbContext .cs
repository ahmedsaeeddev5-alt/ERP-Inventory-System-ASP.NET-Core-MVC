using ERPSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ERPSystem.Data
{
    public class ERPDbContext : IdentityDbContext<ApplicationUser>
    {
        public ERPDbContext(DbContextOptions<ERPDbContext> options) : base(options)
        { }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Unit> Units { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Warehouse> Warehouses { get; set; }

        public DbSet<Stock> Stocks { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<PurchaseInvoice> PurchaseInvoices { get; set; }

        public DbSet<PurchaseInvoiceItem> PurchaseInvoiceItems { get; set; }

        public DbSet<SalesInvoice> SalesInvoices { get; set; }

        public DbSet<SalesInvoiceItem> SalesInvoiceItems { get; set; }
     

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<IdentityRole>().HasData(

      new IdentityRole
      {
          Id = "11111111-1111-1111-1111-111111111111",
          Name = "Admin",
          NormalizedName = "ADMIN",
          ConcurrencyStamp = "STATIC_ADMIN_STAMP"
      },
      new IdentityRole
      {
          Id = "22222222-2222-2222-2222-222222222222",
          Name = "User",
          NormalizedName = "USER",
          ConcurrencyStamp = "STATIC_USER_STAMP"
      }
  );

            modelBuilder.Entity<Category>().HasData(

     new Category
     {
         Id = 1,
         Name = "Electronics",
         Description = "Electronic devices and accessories"
     },

     new Category
     {
         Id = 2,
         Name = "Office",
         Description = "Office supplies and stationery"
     },

     new Category
     {
         Id = 3,
         Name = "Furniture",
         Description = "Office and home furniture"
     },

     new Category
     {
         Id = 4,
         Name = "Accessories",
         Description = "Computer and mobile accessories"
     }

 );

            modelBuilder.Entity<Unit>().HasData(
                new Unit { Id = 1, Name = "Piece" },
                new Unit { Id = 2, Name = "Box" },
                new Unit { Id = 3, Name = "Carton" },
                new Unit { Id = 4, Name = "Kg" }
            );
            base.OnModelCreating(modelBuilder);
            //------------------------------------
            // Product -> Category
            //------------------------------------
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            //------------------------------------
            // Product -> Unit
            //------------------------------------
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Unit)
                .WithMany(u => u.Products)
                .HasForeignKey(p => p.UnitId)
                .OnDelete(DeleteBehavior.Restrict);

            //------------------------------------
            // Stock -> Product
            //------------------------------------
            modelBuilder.Entity<Stock>()
                .HasOne(s => s.Product)
                .WithMany(p => p.Stocks)
                .HasForeignKey(s => s.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            //------------------------------------
            // Stock -> Warehouse
            //------------------------------------
            modelBuilder.Entity<Stock>()
                .HasOne(s => s.Warehouse)
                .WithMany(w => w.Stocks)
                .HasForeignKey(s => s.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            //------------------------------------
            // PurchaseInvoice -> Supplier
            //------------------------------------
            modelBuilder.Entity<PurchaseInvoice>()
                .HasOne(p => p.Supplier)
                .WithMany(s => s.PurchaseInvoices)
                .HasForeignKey(p => p.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            //------------------------------------
            // PurchaseInvoiceItem -> PurchaseInvoice
            //------------------------------------
            modelBuilder.Entity<PurchaseInvoiceItem>()
                .HasOne(i => i.PurchaseInvoice)
                .WithMany(p => p.Items)
                .HasForeignKey(i => i.PurchaseInvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            //------------------------------------
            // PurchaseInvoiceItem -> Product
            //------------------------------------
            modelBuilder.Entity<PurchaseInvoiceItem>()
                .HasOne(i => i.Product)
                .WithMany(p => p.PurchaseInvoiceItems)
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            //------------------------------------
            // SalesInvoice -> Customer
            //------------------------------------
            modelBuilder.Entity<SalesInvoice>()
                .HasOne(s => s.Customer)
                .WithMany(c => c.SalesInvoices)
                .HasForeignKey(s => s.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            //------------------------------------
            // SalesInvoiceItem -> SalesInvoice
            //------------------------------------
            modelBuilder.Entity<SalesInvoiceItem>()
                .HasOne(i => i.SalesInvoice)
                .WithMany(s => s.Items)
                .HasForeignKey(i => i.SalesInvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            //------------------------------------
            // SalesInvoiceItem -> Product
            //------------------------------------
            modelBuilder.Entity<SalesInvoiceItem>()
                .HasOne(i => i.Product)
                .WithMany(p => p.SalesInvoiceItems)
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Restrict);


           modelBuilder.Entity<Product>()
              .HasIndex(p => p.Code)
               .IsUnique();

            modelBuilder.Entity<Product>()
              .HasIndex(p => p.Barcode)
              .IsUnique();

            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique();

            modelBuilder.Entity<Product>()
              .Property(p => p.Name)
              .HasMaxLength(200);

            modelBuilder.Entity<Product>()
       .Property(p => p.PurchasePrice)
       .HasPrecision(18, 2);

            modelBuilder.Entity<Product>()
                .Property(p => p.SalePrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PurchaseInvoice>()
                .Property(p => p.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PurchaseInvoiceItem>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PurchaseInvoiceItem>()
                .Property(p => p.Quantity)
                .HasPrecision(18, 2);

            modelBuilder.Entity<SalesInvoice>()
                .Property(p => p.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<SalesInvoiceItem>()
                .Property(p => p.UnitPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<SalesInvoiceItem>()
                .Property(p => p.Quantity)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Stock>()
                .Property(p => p.Quantity)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PurchaseInvoice>()
    .HasOne(p => p.Warehouse)
    .WithMany(w => w.PurchaseInvoices)
    .HasForeignKey(p => p.WarehouseId)
    .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);

        }
    }
}
