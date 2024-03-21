using CarPartsStore.Repositories.Interfaces;
using CarPartsStore.Repositories.Data;
using CarPartsStore.Models;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace CarPartsStore.Repositories.Implementations
{
    public class HeadSettingRepository : IHeadSettingRepository
    {
        private readonly ILogger<HeadSettingRepository> _logger;
        StoreDbContext db;

        public HeadSettingRepository(StoreDbContext _db, ILogger<HeadSettingRepository> logger)
        {
            db = _db;
            _logger = logger;
        }

        public void Add(HeadSetting setting)
        {
            db.Add(setting);
            db.SaveChanges();
        }

        public HeadSetting Get()
        {
            return db.HeadSettings.OrderBy(x => x.Id).Last();
        }

        public void Update(HeadSetting setting)
        {
            HeadSetting newSetting = db.HeadSettings.Find(setting.Id);

            newSetting.ImageUrl = setting.ImageUrl;
            newSetting.FirstText = setting.FirstText;
            newSetting.SecondText = setting.SecondText;
            newSetting.ButtonText = setting.ButtonText;
            newSetting.FirstTextColor = setting.FirstTextColor;
            newSetting.SecondTextColor = setting.SecondTextColor;

            db.SaveChanges();
        }
    }
}
