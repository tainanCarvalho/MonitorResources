using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Interval.MonitorResource
{
    [ExcludeFromCodeCoverage]
    [DataContract]

    public class ProcessNameVO
    {
        private readonly string name;

        private readonly int pid;

        [DataMember(Name = "name_process")]

        public string Name { get => name; }

        [DataMember(Name = "pid_process")]
        public int PID { get => pid; }
        
        public ProcessNameVO(string name, int pid)
        {
            this.name = name;
            this.pid = pid;
        }

        public override string ToString() => $@"{ name } [{ pid }]";

    }

    [DataContract]
    public sealed class ProcessNameWrapper
    {
        [DataMember(Name = "processes")]
        public IList<ProcessNameVO> Processes { get; set; }
    }
}
