using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Interval.Storage.Rules
{
    public sealed class YamlStorageFile : StoreManager
    {
        private const string EXTENSION = ".yaml";

        private readonly List<DataVO> list;

        [ExcludeFromCodeCoverage]
        public YamlStorageFile(string path, string nameFile) : base(path, nameFile, EXTENSION) => list = new();

        [ExcludeFromCodeCoverage]
        public override Task StorageData(string data, DateTime timeColleteced)
        {            
            list.Add(CreateData(data, timeColleteced));
            return Task.CompletedTask;
        }

        [ExcludeFromCodeCoverage]
        public override void CloseData() => WriteYamlInFile(new MeasureDataVO<DataVO> { data = list });

        [ExcludeFromCodeCoverage]
        private void WriteYamlInFile<T>(T data)
        {
            var serializer = new SerializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .Build();

             WriteInFile(data, serializer);
        }

        [ExcludeFromCodeCoverage]
        private void WriteInFile<T>(T data, ISerializer serializer)
        {
            using var writer = new StreamWriter(file);

            serializer.Serialize(writer, data);
        }
    }
}
