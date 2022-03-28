using System.Threading.Tasks;

namespace Interval.Storage.Infrastruct
{
    public interface IStorageData
    {
        Task StorageData(string data);

        void CloseData();
    }
}
