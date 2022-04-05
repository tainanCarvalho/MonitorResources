using Interval.Storage.Interface;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;

namespace Interval.Storage.Rules
{
    [ExcludeFromCodeCoverage]
    public abstract class StoreManager : IStorageData
    {

        private readonly string path;

        protected readonly string pathOfFile;

        protected readonly Stream file;

        public string PathFile { get => path; }

        public StoreManager(string path, string nameFile, string extension)
        {
            this.path = path;

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            nameFile += extension;
            pathOfFile = Path.Combine(path, nameFile);

            file = File.Create(pathOfFile);   
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
