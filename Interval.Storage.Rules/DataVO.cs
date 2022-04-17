using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Interval.Storage
{
    [ExcludeFromCodeCoverage]
    [DataContract]
    public sealed class MeasureDataVO<T>

    {
        [DataMember(Name = "list")]
        public List<T> data { get; set; }
    }

    [ExcludeFromCodeCoverage]
    [DataContract]
    public sealed class DataVO
    {
        [DataMember(Name = "Value")]
        public double Value { get; set; }

        [DataMember(Name = "date")]
        public string Date { get; set; }

        [DataMember(Name = "time")]
        public string Time { get; set; }
    }
}
