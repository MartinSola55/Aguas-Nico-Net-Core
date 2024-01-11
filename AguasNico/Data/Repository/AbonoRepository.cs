using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using AguasNico.Data.Repository.IRepository;
using AguasNico.Models;
using System.Security.Cryptography.Xml;

namespace AguasNico.Data.Repository
{
    public class AbonoRepository(ApplicationDbContext db) : Repository<Abono>(db), IAbonoRepository
    {
        private readonly ApplicationDbContext _db = db;

        public void Update(Abono abono)
        {
            try
            {
                Abono dbObject = _db.Abonos.First(x => x.ID == abono.ID) ?? throw new Exception("No se ha encontrado el abono");
                dbObject.Name = abono.Name;
                dbObject.Price = abono.Price;
                dbObject.UpdatedAt = DateTime.UtcNow.AddHours(-3);
                _db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SoftDelete(long id)
        {
            try
            {
                _db.Database.BeginTransaction();

                Abono abono = _db.Abonos.Find(id) ?? throw new Exception("No se encontró el abono");
                abono.DeletedAt = DateTime.UtcNow.AddHours(-3);

                foreach (ClientAbono clientAbono in _db.ClientAbonos.Where(x => x.AbonoID == id))
                {
                    clientAbono.DeletedAt = DateTime.UtcNow.AddHours(-3);
                }

                foreach (AbonoProduct abonoProduct in _db.AbonoProducts.Where(x => x.AbonoID == id))
                {
                    abonoProduct.DeletedAt = DateTime.UtcNow.AddHours(-3);
                }

                _db.SaveChanges();
                _db.Database.CommitTransaction();
            }
            catch (Exception)
            {
                _db.Database.RollbackTransaction();
                throw;
            }
        }
    }
}