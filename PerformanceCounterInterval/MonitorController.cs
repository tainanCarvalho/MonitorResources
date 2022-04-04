using Interval.Storage.Interface;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PerformanceCounterInterval
{
    public sealed class MonitorController
    {
        private readonly CancellationTokenSource cancellationToken;

        private readonly IStorageData storeManagerProc; 
        
        private readonly IStorageData storeManagerMem;

        private readonly MonitorResources monitor;

        private readonly DateTime time;

        private readonly string processName;

        private readonly int interval;

        public MonitorController(
                IStorageData storeManagerProc,
                IStorageData storeManagerMem, 
                DateTime time, 
                int interval, 
                string processName
            )
        {
            this.time = time;
            this.interval = interval;
            this.processName = processName;
            this.storeManagerMem = storeManagerMem;
            this.storeManagerProc = storeManagerProc;

            this.monitor = new MonitorResources();
            this.cancellationToken = new CancellationTokenSource();
        }

        public async Task StartMonitorResources()
        {
            while ((DateTime.Now - time < TimeSpan.FromMinutes(interval)) && !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var date = DateTime.Now;
                    var result = monitor.GetProcessTimeUsage(processName);
                    await storeManagerProc.StorageData((result * 100 * 2).ToString(), date);

                    Console.WriteLine(string.Format("{0:P}", result*2));

                    result = monitor.GetProcessMemoryUsage(processName);
                    await storeManagerMem.StorageData(result.ToString(), date);

                    Console.WriteLine($@"{result.ToString("0.##")} MB");

                    await Task.Delay(1000);
                }
                catch (ObjectDisposedException)
                {
                }
            }

        }

        internal void CloseEvent(object sender, ConsoleCancelEventArgs e)
        {
            cancellationToken.CancelAfter(500);
            storeManagerMem.CloseData();
            storeManagerProc.CloseData();
        }


        public void Close()
        {
            storeManagerMem.CloseData();
            storeManagerProc.CloseData();
        }
    }
}
