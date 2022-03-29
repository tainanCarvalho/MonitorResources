using System;
using System.IO;
using System.Threading.Tasks;

namespace Interval.Storage.Rules
{
    public sealed class CsvStorageFile : StoreManager
    {
        private const string EXTENSION = ".csv";

        private StreamWriter writer = null;

        public CsvStorageFile(string path, string nameFile) : base(path, nameFile, EXTENSION)
        {
        }

        public override async Task StorageData(string data, DateTime timeColleteced)
        {
            writer ??= new StreamWriter(pathOfFile);
            if (writer.BaseStream == null)
                return;

            await writer.WriteLineAsync(data);
            await writer.FlushAsync();
        }

        public override void CloseData()
        {
            try
            {
                writer.Close();
                writer.Dispose();
            }
            catch (Exception)
            {
            }
            finally
            {
                Console.WriteLine($@"Arquivo salvo como:{ pathOfFile }");
            }
        }
    }
}
