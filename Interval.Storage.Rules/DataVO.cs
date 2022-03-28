using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Interval.Storage.Rules
{
    [DataContract]
    public sealed class DataVO
    {
        [DataMember(Name = "list")]
        public List<double> data { get; set; }
    }
}
