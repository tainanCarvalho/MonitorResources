using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Interval.Storage.Rules
{
    public class JSONStorageFile : StoreManager
    {
        private const string EXTENSION = ".json";

        private readonly List<double> list;
        public JSONStorageFile(string path, string nameFile) : base(path, nameFile, EXTENSION)
        {
            list = new List<double>();
        }

        public override Task StorageData(string data, DateTime timeColleteced)
        {
            list.Add(Convert.ToDouble(data));
            return Task.CompletedTask;
        }

        public override void CloseData()
        {
            try
            {
                var data = new DataVO()
                {
                    data = list
                };

                using (var file = File.Create(pathOfFile))
                using (StreamWriter writer = new StreamWriter(file))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(writer, data);
                }
            }
            finally
            {

            }
        }
    }
}
