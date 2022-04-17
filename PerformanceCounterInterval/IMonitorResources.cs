using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceCounterInterval
{
    public interface IMonitorResources
    {
        double GetProcessTimeUsage(string processName);

        public double GetProcessMemoryUsage(string processName);

    }
}
