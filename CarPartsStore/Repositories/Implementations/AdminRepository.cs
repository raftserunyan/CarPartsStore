using CarPartsStore.Models;
using CarPartsStore.Repositories.Interfaces;
using CarPartsStore.Repositories.Data;
using System.Collections.Generic;
using System.Linq;
using System;

namespace CarPartsStore.Repositories.Implementations
{
    public class AdminRepository : IAdminRepository
    {
        private readonly StoreDbContext db;

        public AdminRepository(StoreDbContext _db)
        {
            db = _db;
        }

        public void Add(AdminUser admin)
        {
            db.Add(admin);
            db.SaveChanges();
        }

        public void Delete(string email)
        {
            AdminUser user = db.Admins.Where(ad => ad.Email == email).First();
            if (user != null)
            {
                db.Remove(user);
                db.SaveChanges();
            }
            else
            {
                throw new NullReferenceException();
            }
        }

        public AdminUser Get(string email)
        {
            AdminUser user = db.Admins.Where(ad => ad.Email == email).First();
            if (user != null)
                return user;
            else
                throw new NullReferenceException();
        }

        public IEnumerable<AdminUser> GetAll()
        {
            IEnumerable<AdminUser> list = db.Admins;
            return list;
        }

        public void Update(AdminUser admin)
        {
            AdminUser mem = db.Admins.Find(admin.Id);

            mem.UserName = admin.UserName;
            mem.Email = admin.Email;
            mem.PasswordHash = admin.PasswordHash;

            db.SaveChanges();
        }

        public bool Exists(string email)
        {
            var user = db.Admins.FirstOrDefault(ad => ad.Email == email);
            
            return user != null ? true : false;
        }
    }
}
