using CarPartsStore.Models;
using CarPartsStore.Repositories.Interfaces;
using CarPartsStore.Repositories.Data;
using System.Collections.Generic;

namespace CarPartsStore.Repositories.Implementations
{
    public class TeamMemberRepository : ITeamMemberRepository
    {
        private readonly StoreDbContext db;

        public TeamMemberRepository(StoreDbContext _db)
        {
            db = _db;
        }

        public void Add(TeamMember member)
        {
            db.Add(member);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            db.Remove(db.TeamMembers.Find(id));
            db.SaveChanges();
        }

        public TeamMember Get(int id)
        {
            return db.TeamMembers.Find(id);
        }

        public IEnumerable<TeamMember> GetAll()
        {
            IEnumerable<TeamMember> list = db.TeamMembers;
            return list;
        }

        public void Update(TeamMember member)
        {
            TeamMember mem = db.TeamMembers.Find(member.Id);

            mem.Name = member.Name;
            mem.ImageUrl = member.ImageUrl;
            mem.Position = member.Position;
            mem.FacebookLink = member.FacebookLink;
            mem.TwitterLink = member.TwitterLink;
            mem.LinkedinLink = member.LinkedinLink;

            db.SaveChanges();
        }
    }
}
