using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Interval.Storage
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
            if (writer == null)
            {
                writer = new(file);
                await writer.WriteLineAsync("data;valor");
            }

            if (writer.BaseStream == null)
                return;

            await writer.WriteLineAsync(BuildRow(data, timeColleteced));
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

        private string BuildRow(string data, DateTime dateTime)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(dateTime.ToString()).Append(";").Append(data);
            return stringBuilder.ToString();
        }
    }
}
