using CarPartsStore.Models;
using System.Collections.Generic;

namespace CarPartsStore.Repositories.Interfaces
{
    public interface IAdminRepository
    {
        IEnumerable<AdminUser> GetAll();
        AdminUser Get(string email);
        void Add(AdminUser admin);
        void Update(AdminUser admin);
        void Delete(string email);
        bool Exists(string email);
    }
}
