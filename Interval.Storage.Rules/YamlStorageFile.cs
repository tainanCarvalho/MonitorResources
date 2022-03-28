using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Interval.Storage.Rules
{
    public sealed class YamlStorageFile : StoreManager
    {
        private const string EXTENSION = ".yaml";

        private StreamWriter writer = null;

        private readonly List<double> list;

        public YamlStorageFile(string path, string nameFile) : base(path, nameFile, EXTENSION)
        {
            list = new();
        }

        public override Task StorageData(string data)
        {
            list.Add(Convert.ToDouble(data));
            return Task.CompletedTask;
        }

        public override void CloseData()
        {
            var yamlString = ConvertInYamlString(new DataVO { data = list });

            WriteInFile(yamlString).Wait();
        }

        private string ConvertInYamlString<T>(T data)
        {
            var serializer = new SerializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .Build();

            return serializer.Serialize(data);
        }

        private async Task WriteInFile(string data)
        {
            writer ??= new StreamWriter(pathOfFile);
            if (writer.BaseStream == null)
                return;

            await writer.WriteLineAsync(data);
            await writer.FlushAsync();

            writer.Close();

        }
    }
}
