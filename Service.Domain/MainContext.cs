using Microsoft.EntityFrameworkCore;
using Service.Common.Enums;
using Service.Domain.Models;
using System.Security.Cryptography;
using System.Text;

namespace Service.Domain
{
    public class MainContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        
        public DbSet<Customer> Customers { get; set; }
        
        public DbSet<Order> Orders { get; set; }
        
        public DbSet<Product> Products { get; set; }
        
        public DbSet<ProductCategory> ProductCategories { get; set; }
        
        public DbSet<OrderedProduct> OrderedProducts { get; set; }
        
        public DbSet<OrderChangeStatusLog> OrderChangeStatusLogs { get; set; }
        public DbSet<DeliveryService> DeliveryServices { get; set; }


        public MainContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(s => s.Login)
                .HasMaxLength(128)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(s => s.PasswordSalt)
                .IsRequired();
            
            modelBuilder.Entity<User>()
                .Property(s => s.PasswordHash)
                .IsRequired();

            modelBuilder.Entity<User>()
                .HasIndex(s => s.Login)
                .IsUnique()
                .IncludeProperties(p => new { p.PasswordHash, p.PasswordSalt });

            modelBuilder.Entity<User>()
                .Property(s => s.Email)
                .HasMaxLength(128)
                .IsRequired();


            modelBuilder.Entity<Customer>()
                .Property(s => s.Name).HasMaxLength(40).IsRequired();
            modelBuilder.Entity<Customer>()
                .Property(s => s.LastName).HasMaxLength(40).IsRequired();
            modelBuilder.Entity<Customer>()
                .Property(s => s.PhoneNumber).HasMaxLength(16).IsRequired();
            modelBuilder.Entity<Customer>()
                .Property(s => s.Address).HasMaxLength(128).IsRequired();

            modelBuilder.Entity<Product>()
                .Property(s => s.Name).HasMaxLength(128).IsRequired();
            modelBuilder.Entity<Product>()
                .Property(s => s.Description).HasMaxLength(500).IsRequired(false);
            modelBuilder.Entity<Product>()
                .Property(s => s.Price).HasDefaultValue(0.0m).IsRequired();
            modelBuilder.Entity<Product>()
                .Property(s => s.Count).HasDefaultValue(0).IsRequired();

            modelBuilder.Entity<Product>().HasIndex(i => i.Name);


            modelBuilder.Entity<ProductCategory>()
                .Property(s => s.Name).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<ProductCategory>()
                .Property(s => s.Description).HasMaxLength(500).IsRequired(false);


            modelBuilder.Entity<Order>()
                .Property(s => s.Status).HasDefaultValue(OrderStatus.Created).IsRequired();
            modelBuilder.Entity<Order>()
                .Property(s => s.TotalPrice).HasDefaultValue(0m).IsRequired();
            modelBuilder.Entity<Order>().OwnsOne(o => o.Delivery)
                .Property(d => d.Address).IsRequired().HasMaxLength(150);


            modelBuilder.Entity<OrderedProduct>()
                .Property(s => s.Count).HasDefaultValue(1).IsRequired();
            modelBuilder.Entity<OrderedProduct>()
                .HasOne(i => i.Product)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<OrderedProduct>()
                .HasIndex(i => new { i.ProductId, i.OrderId})
                .IsUnique()
                .IncludeProperties(s => s.Count);


            modelBuilder.Entity<OrderChangeStatusLog>()
                .Property(s => s.DateChange).HasDefaultValueSql("GETDATE()").IsRequired();
            modelBuilder.Entity<OrderChangeStatusLog>()
                .HasIndex(i => i.OrderId);

            modelBuilder.Entity<DeliveryService>().Property(d => d.DeliveryName).IsRequired().HasMaxLength(100);


            using HMACSHA512 hmac = new HMACSHA512();

            var hashedPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes("123456"));
            var saltPassword = hmac.Key;
            
            modelBuilder.Entity<User>()
                .HasData(new User() 
                {   Id = 1, 
                    Login = "Admin", 
                    PasswordHash = hashedPassword, 
                    PasswordSalt = saltPassword, 
                    Email = "admin@gmail.com", 
                    Role = RoleType.Admin 
                });

            modelBuilder.Entity<DeliveryService>()
                .HasData(
                new DeliveryService { Id = 1, DeliveryName = "Nova Poshta" },
                new DeliveryService { Id = 2, DeliveryName = "Ukr Poshta" },
                new DeliveryService { Id = 3, DeliveryName = "Meest Express" },
                new DeliveryService { Id = 4, DeliveryName = "Samoviviz" }
                );

            modelBuilder.Entity<ProductCategory>()
                .HasData(
                new ProductCategory { Id = 1, Name = "Food" },
                new ProductCategory { Id = 2, Name = "Technical" },
                new ProductCategory { Id = 3, Name = "Magazines" },
                new ProductCategory { Id = 4, Name = "Stationery" }
                );


            base.OnModelCreating(modelBuilder);
        }

    }
}