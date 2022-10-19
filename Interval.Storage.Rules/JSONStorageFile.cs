using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;

namespace Interval.Storage
{

    [ExcludeFromCodeCoverage]
    public class JsonStorageFile : StoreManager
    {
        private const string EXTENSION = ".json";

        private readonly List<DataVO> list;

        public JsonStorageFile(string path, string nameFile) : base(path, nameFile, EXTENSION) => list = new();


        public override Task StorageData(string data, DateTime timeColleteced)
        {
            list.Add(CreateData(data, timeColleteced));
            return Task.CompletedTask;
        }

        public override void CloseData()
        {
            var data = new MeasureDataVO<DataVO>() { data = list };

            JsonSerializer serializer = new JsonSerializer();

            WriteInFile(data, serializer);
        }

        private void WriteInFile<T>(T data, JsonSerializer serializer)
        {
            using StreamWriter writer = new(file);

            serializer.Serialize(writer, data);
        }
    }
}
