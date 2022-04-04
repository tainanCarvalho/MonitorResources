using Interval.Storage.Interface;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Interval.Storage.Rules
{
    public abstract class StoreManager : IStorageData
    {

        private readonly string path;

        protected readonly string pathOfFile;

        public string PathFile { get => path; }

        public StoreManager(string path, string nameFile, string extension)
        {
            this.path = path;

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            nameFile += extension;
            pathOfFile = Path.Combine(path, nameFile);
        }

        public abstract Task StorageData(string data, DateTime timeColleteced);

        public abstract void CloseData();

        protected DataVO CreateData(string value, DateTime timeColleteced) =>
             new()
             {
                 Value = Convert.ToDouble(value),
                 Date = timeColleteced.ToString("dd/MM/yyyy"),
                 Time = timeColleteced.TimeOfDay.ToString(@"hh\:mm\:ss")
             };
    }
}
