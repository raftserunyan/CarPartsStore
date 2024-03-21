using System.Collections.Generic;
using CarPartsStore.Models;

namespace CarPartsStore.Repositories.Interfaces
{
    public interface IAdminCandidateRepository
    {
        IEnumerable<AdminCandidate> GetAll();
        AdminCandidate Get(string id);
        void Add(AdminCandidate candidate);
        void Update(AdminCandidate candidate);
        void Delete(string id);
        bool Exists(string email);
        bool ExistsId(string id);
    }
}
