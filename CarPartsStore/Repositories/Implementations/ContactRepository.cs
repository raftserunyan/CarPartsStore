using System.Collections.Generic;
using CarPartsStore.Repositories.Interfaces;
using CarPartsStore.Repositories.Data;
using CarPartsStore.Models;

namespace CarPartsStore.Repositories.Implementations
{
    public class ContactRepository : IContactRepository
    {
        private readonly StoreDbContext db;

        public ContactRepository(StoreDbContext _db)
        {
            db = _db;
        }

        public void Add(ContactUsModel contact)
        {
            db.Add(contact);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            db.Remove(db.Contacts.Find(id));
            db.SaveChanges();
        }

        public ContactUsModel Get(int id)
        {
            return db.Contacts.Find(id);
        }

        public IEnumerable<ContactUsModel> GetAll()
        {
            IEnumerable<ContactUsModel> list = db.Contacts;
            return list;
        }

        public void Update(ContactUsModel _contact)
        {
            ContactUsModel contact = db.Contacts.Find(_contact.Id);

            contact.Name = _contact.Name;
            contact.Email = _contact.Email;
            contact.Phone = _contact.Phone;
            contact.Message = _contact.Message;

            db.SaveChanges();
        }
    }
}
