using System;
using WeakEvent;

namespace Interval.MonitorResource
{
    public class ResourceDataEventHub
    {
        private static ResourceDataEventHub eventHub;

        public  readonly WeakEventSource<ResourceDataEventArgs> MemoryDataHub = new WeakEventSource<ResourceDataEventArgs>();
        public  readonly WeakEventSource<ResourceDataEventArgs> ProcessorDataHub = new WeakEventSource<ResourceDataEventArgs>();


        private ResourceDataEventHub() { }

        public static ResourceDataEventHub Instance
        {
            get 
            {
                if(eventHub is null)
                    eventHub = new();
                
                return eventHub;
            }
        }
    }


    public class ResourceDataEventArgs : EventArgs
    {
        private readonly ResourceData data;

        public ResourceData Data { get { return data; } }

        public ResourceDataEventArgs(ResourceData data) => this.data = data;
    }

    public class ResourceData
    {
        public double Value { get; set; }

        public DateTime Date { get; set; }

        public string Time { get; set; }
    }
}
