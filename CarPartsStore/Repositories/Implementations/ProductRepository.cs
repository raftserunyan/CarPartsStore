using CarPartsStore.Models;
using CarPartsStore.Repositories.Interfaces;
using CarPartsStore.Repositories.Data;
using System.Collections.Generic;

namespace CarPartsStore.Repositories.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreDbContext db;

        public ProductRepository(StoreDbContext _db)
        {
            db = _db;
        }

        public void Add(Product prod)
        {
            db.Add(prod);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            db.Remove(db.Products.Find(id));
            db.SaveChanges();
        }

        public IEnumerable<Product> GetAll()
        {
            IEnumerable<Product> list = db.Products;
            return list;
        }

        public Product Get(int id)
        {
            return db.Products.Find(id);
        }

        public void Update(Product prod)
        {
            Product product = db.Products.Find(prod.ProductID);

            product.Name = prod.Name;
            product.ImageUrl = prod.ImageUrl;
            product.Description = prod.Description;
            product.Price = prod.Price;
            product.Category = prod.Category;

            db.SaveChanges();
        }
    }
}
