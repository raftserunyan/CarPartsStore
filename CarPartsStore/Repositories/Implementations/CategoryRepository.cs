using System.Collections.Generic;
using CarPartsStore.Repositories.Interfaces;
using CarPartsStore.Models;
using CarPartsStore.Repositories.Data;

namespace CarPartsStore.Repositories.Implementations
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly StoreDbContext db;

        public CategoryRepository(StoreDbContext _db)
        {
            db = _db;
        }

        public void Add(Category cat)
        {
            db.Add(cat);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            db.Remove(db.Categories.Find(id));
            db.SaveChanges();
        }

        public Category Get(int id)
        {
            return db.Categories.Find(id);
        }

        public IEnumerable<Category> GetAll()
        {
            IEnumerable<Category> list = db.Categories;
            return list;
        }

        public void Update(Category cat)
        {
            Category category = db.Categories.Find(cat.Id);

            category.Name = cat.Name;

            db.SaveChanges();
        }
    }
}
