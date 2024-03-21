using System.Collections.Generic;
using CarPartsStore.Models;

namespace CarPartsStore.Repositories.Interfaces
{
    public interface ITimelineRepository
    {
        IEnumerable<TimelineModel> GetAll();
        TimelineModel Get(int id);
        void Add(TimelineModel tmlnModel);
        void Update(TimelineModel tmlnModel);
        void Delete(int id);
    }
}
