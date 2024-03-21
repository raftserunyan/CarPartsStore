using CarPartsStore.Models;

namespace CarPartsStore.Repositories.Interfaces
{
    public interface IHeadSettingRepository
    {
        HeadSetting Get();
        void Add(HeadSetting setting);
        void Update(HeadSetting setting);
    }
}
