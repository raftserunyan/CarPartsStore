using CarPartsStore.Models;
using System.Collections.Generic;

namespace CarPartsStore.Repositories.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();
        Product Get(int id);
        void Add(Product prod);
        void Update(Product prod);
        void Delete(int id);
    }
}
