using Interval.Storage.Interface;
using Interval.Storage.Tools;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Interval.MonitorResource
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

            monitor = monitorResources ?? new MonitorResources();
            cancellationToken = new CancellationTokenSource();
        }


        public MonitorController
            (
                DateTime time,
                int interval,
                string processName,
                IMonitorResources monitorResources
            )
        {
            this.time = time;
            this.interval = interval;
            this.processName = processName;

            monitor = monitorResources ?? new MonitorResources();
            cancellationToken = new CancellationTokenSource();


        }

        public async Task StartStorageResourceDataAsync()
        {
            while ((DateTime.Now - time < TimeSpan.FromMinutes(interval)) && !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var date = DateTime.Now;
                    var result = monitor.GetProcessTimeUsage(processName);
                    await storeManagerProc.StorageData((result * 100 * 2).ToString(), date);

                    logger.AddDebug(string.Format("{0:P}", result * 2));

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

        public async Task RunMonitorResourcesData(int delayMonitor = 1000)
        {
            while ((interval == -1 || (DateTime.Now - time < TimeSpan.FromMinutes(interval))) && !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var date = DateTime.Now;
                    var result = monitor.GetProcessTimeUsage(processName);
                    ResourceDataEventHub.Instance.ProcessorDataHub.Raise(this, new ResourceDataEventArgs
                                                    (
                                                        new ResourceData()
                                                        {
                                                            Date = date,
                                                            Time = date.TimeOfDay.ToString(),
                                                            Value = result
                                                        }
                                                    ));
                    logger.AddDebug(string.Format("{0:P}", result * 2));

                    result = monitor.GetProcessMemoryUsage(processName);
                    ResourceDataEventHub.Instance.MemoryDataHub.Raise(this, new ResourceDataEventArgs
                                                   (
                                                       new ResourceData()
                                                       {
                                                           Date = date,
                                                           Time = date.TimeOfDay.ToString(),
                                                           Value = result
                                                       }
                                                   ));
                    logger.AddDebug($@"{result.ToString("0.##")} MB");

                    await Task.Delay(delayMonitor);
                }
                catch (ObjectDisposedException)
                {
                }
            }
        }

        public void Stop() => cancellationToken.CancelAfter(500);

        public void CloseEvent(object sender, ConsoleCancelEventArgs e)
        {
            cancellationToken.CancelAfter(500);
            storeManagerMem.CloseData();
            storeManagerProc.CloseData();
        }

        public void Close()
        {
            storeManagerMem?.CloseData();
            storeManagerProc?.CloseData();
        }
    }
}
