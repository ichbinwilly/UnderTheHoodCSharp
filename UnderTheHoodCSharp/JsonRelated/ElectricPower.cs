using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UnderTheHoodCSharp.JsonRelated
{
    public class ElectricPower
    {
        [JsonProperty(PropertyName = "phase", Required = Required.Always)]
        public Phase Phase { get; set; }
    }

    /***
    public enum Phase
    {
        L1,
        L2,
        L3,
        N,
        L1-N, //Syntax Error '-' is not invalid
        L2-N, //Syntax Error '-' is not invalid
        L3-N, //Syntax Error '-' is not invalid
        L1-L2, //Syntax Error '-' is not invalid
        L2-L3, //Syntax Error '-' is not invalid
        L3-L1 //Syntax Error '-' is not invalid
    }

    ***/

    public enum Phase
    {
        L1,
        L2,
        L3,
        N,
        [EnumMember(Value = "L1-N")]
        L1N,
        [EnumMember(Value = "L2-N")]
        L2N,
        [EnumMember(Value = "L3-N")]
        L3N,
        [EnumMember(Value = "L1-L2")]
        L1L2,
        [EnumMember(Value = "L2-L3")]
        L2L3,
        [EnumMember(Value = "L3-L1")]
        L3L1
    }
}
