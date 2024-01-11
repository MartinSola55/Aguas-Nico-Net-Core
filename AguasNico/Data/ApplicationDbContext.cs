using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AguasNico.Data.Seeding;
using AguasNico.Models;

namespace AguasNico.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuraciones adicionales aquí

            new DbInitializer(modelBuilder).Seed();

            modelBuilder.Entity<CartPaymentMethod>()
                .HasKey(entity => new { entity.CartID, entity.PaymentMethodID });
            modelBuilder.Entity<CartProduct>()
                .HasKey(entity => new { entity.Type, entity.CartID });
            modelBuilder.Entity<ClientProduct>()
                .HasKey(entity => new { entity.ProductID, entity.ClientID });
            modelBuilder.Entity<ReturnedProduct>()
                .HasKey(entity => new { entity.Type, entity.CartID });
            modelBuilder.Entity<DispatchedProduct>()
                .HasKey(entity => new { entity.RouteID, entity.Type });
            modelBuilder.Entity<AbonoProduct>()
                .HasKey(entity => new { entity.AbonoID, entity.Type });
            modelBuilder.Entity<ClientAbono>()
                .HasKey(entity => new { entity.ClientID, entity.AbonoID });

            modelBuilder.Entity<Cart>()
                .HasQueryFilter(entity => entity.DeletedAt == null);
            modelBuilder.Entity<CartPaymentMethod>()
                .HasQueryFilter(entity => entity.DeletedAt == null);
            modelBuilder.Entity<Expense>()
                .HasQueryFilter(entity => entity.DeletedAt == null);
            modelBuilder.Entity<CartProduct>()
                .HasQueryFilter(entity => entity.DeletedAt == null);
            modelBuilder.Entity<ClientProduct>()
                .HasQueryFilter(entity => entity.DeletedAt == null);
            modelBuilder.Entity<ReturnedProduct>()
                .HasQueryFilter(entity => entity.DeletedAt == null);
            modelBuilder.Entity<Models.Route>()
                .HasQueryFilter(entity => entity.DeletedAt == null);
            modelBuilder.Entity<Transfer>()
                .HasQueryFilter(entity => entity.DeletedAt == null);
            modelBuilder.Entity<DispatchedProduct>()
                .HasQueryFilter(entity => entity.DeletedAt == null);
            modelBuilder.Entity<Abono>()
                .HasQueryFilter(entity => entity.DeletedAt == null);
            modelBuilder.Entity<AbonoProduct>()
                .HasQueryFilter(entity => entity.DeletedAt == null);
            modelBuilder.Entity<ClientAbono>()
                .HasQueryFilter(entity => entity.DeletedAt == null);
        }

        public DbSet<ApplicationUser> User { get; set; }
        public DbSet<Abono> Abonos { get; set; }
        public DbSet<AbonoProduct> AbonoProducts { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartPaymentMethod> CartPaymentMethods { get; set; }
        public DbSet<CartProduct> CartProducts { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientAbono> ClientAbonos { get; set; }
        public DbSet<ClientProduct> ClientProducts { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ReturnedProduct> ReturnedProducts { get; set; }
        public DbSet<DispatchedProduct> DispatchedProducts { get; set; }
        public DbSet<Models.Route> Routes { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
    }
}