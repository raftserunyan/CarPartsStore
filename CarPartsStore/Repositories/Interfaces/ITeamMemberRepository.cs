using CarPartsStore.Models;
using System.Collections.Generic;

namespace CarPartsStore.Repositories.Interfaces
{
    public interface ITeamMemberRepository
    {
        IEnumerable<TeamMember> GetAll();
        TeamMember Get(int id);
        void Add(TeamMember member);
        void Update(TeamMember member);
        void Delete(int id);
    }
}
