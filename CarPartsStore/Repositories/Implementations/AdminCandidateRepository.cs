using CarPartsStore.Models;
using CarPartsStore.Repositories.Interfaces;
using CarPartsStore.Repositories.Data;
using System.Collections.Generic;
using System.Linq;
using System;

namespace CarPartsStore.Repositories.Implementations
{
    public class AdminCandidateRepository : IAdminCandidateRepository
    {
        private readonly StoreDbContext db;

        public AdminCandidateRepository(StoreDbContext _db)
        {
            db = _db;
        }

        public void Add(AdminCandidate candidate)
        {
            db.Add(candidate);
            db.SaveChanges();
        }

        public void Delete(string id)
        {
            AdminCandidate candidate = db.Candidates.Where(ad => ad.GuID == id).First();
            if (candidate != null)
            {
                db.Remove(candidate);
                db.SaveChanges();
            }
            else
            {
                throw new NullReferenceException();
            }
        }

        public bool Exists(string email)
        {
            var candidate = db.Candidates.FirstOrDefault(ad => ad.Email == email);

            return candidate != null ? true : false;
        }

        public bool ExistsId(string id)
        {
            var candidate = db.Candidates.FirstOrDefault(ad => ad.GuID == id);

            return candidate != null ? true : false;
        }

        public AdminCandidate Get(string id)
        {
            AdminCandidate candidate = db.Candidates.Where(ad => ad.GuID == id).First();
            if (candidate != null)
                return candidate;
            else
                throw new NullReferenceException();
        }

        public IEnumerable<AdminCandidate> GetAll()
        {
            IEnumerable<AdminCandidate> list = db.Candidates;
            return list;
        }

        public void Update(AdminCandidate candidate)
        {
            AdminCandidate cand = db.Candidates.Find(candidate.Id);

            cand.Name = candidate.Name;
            cand.Email = cand.Email;
            cand.Password = cand.Password;

            db.SaveChanges();
        }
    }
}
