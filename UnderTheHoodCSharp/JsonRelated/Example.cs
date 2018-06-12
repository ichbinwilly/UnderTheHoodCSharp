using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace UnderTheHoodCSharp.JsonRelated
{
    public class Example
    {
        public Example()
        {
            ElectricPower power = new ElectricPower()
            {
                Phase = Phase.L1L2
            };

            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new List<JsonConverter> { new StringEnumConverter { CamelCaseText = false } }, //Specified CamelCaseText StringConverter
                NullValueHandling = NullValueHandling.Ignore
            };
            
            Console.WriteLine(JsonConvert.SerializeObject(power, settings));
            //Console output {"phase":"L1-L2"}
        }
    }
}
