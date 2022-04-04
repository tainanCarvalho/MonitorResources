﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Interval.Storage.Rules
{
    [DataContract]
    public sealed class MeasureDataVO<T>

    {
        [DataMember(Name = "list")]
        public List<T> data { get; set; }
    }

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
