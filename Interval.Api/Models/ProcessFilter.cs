using System.Runtime.Serialization;

namespace Interval.Api
{
    [DataContract]
    public class ProcessFilter
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
