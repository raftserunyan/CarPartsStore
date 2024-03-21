using System.Collections.Generic;
using CarPartsStore.Models;

namespace CarPartsStore.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAll();
        Category Get(int id);
        void Add(Category cat);
        void Update(Category cat);
        void Delete(int id);
    }
}
