using Microsoft.EntityFrameworkCore;
using PROGPOE.Models;

namespace PROGPOE.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Carts> Carts { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<Farmers> Farmers { get; set; }
        public DbSet<Employees> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure unique indexes for Email and UserName
            modelBuilder.Entity<Users>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Users>()
                .HasIndex(u => u.UserName)
                .IsUnique();

            // Configure properties for Users
            modelBuilder.Entity<Users>()
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Users>()
                .Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(100);

            // Configure CategoryID as foreign key for Product
            modelBuilder.Entity<Products>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryID);

            base.OnModelCreating(modelBuilder);
        }
    }
}
