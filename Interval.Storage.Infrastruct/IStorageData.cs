using System;
using System.Threading.Tasks;

namespace Interval.Storage.Interface
{
    public interface IStorageData
    {
        Task StorageData(string data, DateTime timeColleteced);

        void CloseData();
    }
}
