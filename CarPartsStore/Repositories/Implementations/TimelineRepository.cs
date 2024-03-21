using System.Collections.Generic;
using CarPartsStore.Models;
using CarPartsStore.Repositories.Interfaces;
using CarPartsStore.Repositories.Data;

namespace CarPartsStore.Repositories.Implementations
{
    public class TimelineRepository : ITimelineRepository
    {
        private readonly StoreDbContext db;

        public TimelineRepository(StoreDbContext _db)
        {
            db = _db;
        }

        public void Add(TimelineModel tmlnModel)
        {
            db.Add(tmlnModel);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            db.Remove(db.TimelineObjects.Find(id));
            db.SaveChanges();
        }

        public TimelineModel Get(int id)
        {
            return db.TimelineObjects.Find(id);
        }

        public IEnumerable<TimelineModel> GetAll()
        {
            IEnumerable<TimelineModel> list = db.TimelineObjects;
            return list;
        }

        public void Update(TimelineModel tmlnModel)
        {
            TimelineModel tmObject = db.TimelineObjects.Find(tmlnModel.Id);

            tmObject.ImageUrl = tmlnModel.ImageUrl;
            tmObject.Date = tmlnModel.Date;
            tmObject.Title = tmlnModel.Title;
            tmObject.Description = tmlnModel.Description;
            tmObject.Side = tmlnModel.Side;

            db.SaveChanges();
        }
    }
}
