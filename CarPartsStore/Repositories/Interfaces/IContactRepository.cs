using System.Collections.Generic;
using CarPartsStore.Models;

namespace CarPartsStore.Repositories.Interfaces
{
    public interface IContactRepository
    {
        IEnumerable<ContactUsModel> GetAll();
        ContactUsModel Get(int id);
        void Add(ContactUsModel contact);
        void Update(ContactUsModel contact);
        void Delete(int id);
    }
}
