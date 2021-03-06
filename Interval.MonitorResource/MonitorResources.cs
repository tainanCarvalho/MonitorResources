using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Management;
using System.Threading;

namespace Interval.MonitorResource
{
    [ExcludeFromCodeCoverage]
    public sealed class MonitorResources : IMonitorResources
    {
        private Dictionary<object, ulong> runProcessOne;
        private Dictionary<object, ulong> runProcessTwo;
        public MonitorResources()
        {

        }

        public double GetProcessTimeUsage(string processName)
        {
#pragma warning disable CA1416 // Validate platform compatibility

            var mos = new ManagementObjectSearcher("SELECT * FROM Win32_PerfRawData_PerfProc_Process");
            runProcessOne = mos.Get().Cast<ManagementObject>().ToDictionary(mo => mo.Properties["Name"].Value, mo => (ulong)mo.Properties["PercentProcessorTime"].Value);
            Thread.Sleep(570); // can be an arbitrary number
            runProcessTwo = mos.Get().Cast<ManagementObject>().ToDictionary(mo => mo.Properties["Name"].Value, mo => (ulong)mo.Properties["PercentProcessorTime"].Value);
#pragma warning restore CA1416 // Validate platform compatibility

            var total = runProcessTwo["_Total"] - runProcessOne["_Total"];

            var p1 = runProcessOne[processName];

            if (runProcessTwo.ContainsKey(processName))
            {
                var p2 = runProcessTwo[processName];
                return (double)(p2 - p1) / total;
            }

            return 0;
        }

#pragma warning disable CA1416 // Validate platform compatibility
        public double GetProcessMemoryUsage(string processName)
        {            
            var process = new PerformanceCounter("Process", "Working Set - Private", processName, true);
            try
            {
                return process.NextValue() / (int)(1024 * 1024);
            }
            catch
            {
                return 0;
            }
            finally
            {
                process.Close();
                process.Dispose();
            }
        }
#pragma warning restore CA1416 // Validate platform compatibility

    }
}
