using Microsoft.EntityFrameworkCore;
using AguasNico.Models;

namespace AguasNico.Data.Seeding
{
    public class DbInitializer(ModelBuilder modelBuilder)
    {
        private readonly ModelBuilder modelBuilder = modelBuilder;

        public void Seed()
        {
            modelBuilder.Entity<Product>().HasData(
                new Product() { ID = 1, Name = "Máquina frío/calor", Price = 7800 },
                new Product() { ID = 2, Name = "B12L", Bottle = Bottle.B12L, Price = 1800 },
                new Product() { ID = 3, Name = "B20L", Bottle = Bottle.B20L, Price = 2400 },
                new Product() { ID = 4, Name = "Soda 1/2", Price = 600 },
                new Product() { ID = 5, Name = "B20L BAJADO", Bottle = Bottle.B20L, Price = 2800 },
                new Product() { ID = 6, Name = "B20L", Bottle = Bottle.B20L, Price = 1331 },
                new Product() { ID = 7, Name = "Dispenser", Price = 3500 },
                new Product() { ID = 8, Name = "MAQUINA SIN CARGO", Price = 0 },
                new Product() { ID = 9, Name = "B20L SIN CARGO", Bottle = Bottle.B20L, Price = 0 },
                new Product() { ID = 10, Name = "B20L", Bottle = Bottle.B20L, Price = 2000 },
                new Product() { ID = 11, Name = "B20L", Bottle = Bottle.B20L, Price = 1800 }
            );
            
            modelBuilder.Entity<Client>().HasData(
                new Client() { ID = 1, Name = "Martín Sola", Address = "Rivadavia 1097", Phone = "3404123123", Observations = "Cuidado con el perro", Debt = 1500, HasInvoice = true, InvoiceType = InvoiceType.B, TaxCondition = TaxCondition.MO, CUIT = "20123123127" },
                new Client() { ID = 2, Name = "Agustín Bettig", Address = "A la vuelta de la cristalería", Phone = "3404123123", Debt = 0, HasInvoice = false }
            );

            modelBuilder.Entity<PaymentMethod>().HasData(
                new PaymentMethod() { ID = 1, Name = "Efectivo" },
                new PaymentMethod() { ID = 2, Name = "Transferencia" },
                new PaymentMethod() { ID = 3, Name = "Mercado Pago" }
            );
        }
    }

}
