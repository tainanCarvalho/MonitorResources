using Interval.Storage.Factory;
using System;
using System.Threading;

namespace PerformanceCounterInterval
{
    class Program
    {
        public static void Main(string[] args)
        {
            string temp;
            Console.WriteLine("Quantidade de tempo em minutos para o monitoramento do consumo da cpu");
            temp = Console.ReadLine();

            if (!int.TryParse(temp, out int interval))
                interval = 1;

            Console.WriteLine("Nome do processo da ser monitorado:");
            temp = Console.ReadLine();

            var name = string.IsNullOrWhiteSpace(temp) ? "_Total" : temp;

            Console.WriteLine("Tipo de arquivo a ser salvo:\r\n \"csv\"\r\n \"json\"\r\n \"yaml\"\r\n");
            var extension = Console.ReadLine(); 

            Console.WriteLine("Local para armazenar as informações:");
            temp = Console.ReadLine();

            var start = DateTime.Now;
            var path = string.IsNullOrWhiteSpace(temp) ? $@"D:\{ name }_{start.ToString("dd-MM-yyyy") }" : temp;
            

            var nameFileProcessor = name + "." + start.ToString("s").Replace(":", "_") + "_process";
            var nameFileMem = name + "." + start.ToString("s").Replace(":", "_") + "_memory";
            
            var storeMem = StorageFactory.Factory(extension, path, nameFileMem);
            var storeProcess = StorageFactory.Factory(extension, path, nameFileProcessor);

            var controller = new MonitorController(storeProcess, storeMem, start, interval, name);
           
            Console.CancelKeyPress += new ConsoleCancelEventHandler(controller.CloseEvent);

            controller.StartMonitorResources().Wait(TimeSpan.FromMinutes(interval + 2));

            controller.Close();

            Console.WriteLine($@"Dados salvos em { path }");            
        }
    }    
}

