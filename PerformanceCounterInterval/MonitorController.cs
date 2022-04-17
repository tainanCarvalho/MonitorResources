using Interval.Storage.Interface;
using Interval.Storage.Tools;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace PerformanceCounterInterval
{
    [ExcludeFromCodeCoverage]
    public sealed class MonitorController
    {
        private static Ilogger logger = new Logger(typeof(MonitorController));

        private readonly CancellationTokenSource cancellationToken;

        private readonly IStorageData storeManagerProc; 
        
        private readonly IStorageData storeManagerMem;

        private readonly IMonitorResources monitor;

        private readonly DateTime time;

        private readonly string processName;

        private readonly int interval;

        public MonitorController(
                IStorageData storeManagerProc,
                IStorageData storeManagerMem,
                IMonitorResources monitorResources,
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

            this.monitor = monitorResources ?? new MonitorResources();
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

                    logger.AddDebug(string.Format("{0:P}", result*2));

                    result = monitor.GetProcessMemoryUsage(processName);
                    await storeManagerMem.StorageData(result.ToString(), date);

                    logger.AddDebug($@"{result.ToString("0.##")} MB");

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
