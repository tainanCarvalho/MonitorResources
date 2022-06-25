namespace Interval.MonitorResource
{
    public interface IMonitorResources
    {
        double GetProcessTimeUsage(string processName);

        public double GetProcessMemoryUsage(string processName);

    }
}
