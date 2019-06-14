using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Aim.BusinessRules
{
    [DataContract]
    public class CommandInfo
    {
        public CommandInfo()
        {
            CommandValues = new List<object>();
        }

        [DataMember]
        public FeatureType FeatureType { get; set; }

        [DataMember]
        public string Command { get; set; }

        [DataMember]
        public List<object> CommandValues { get; private set; }
    }

    [DataContract]
    public class CommandInfoList
    {
        public CommandInfoList()
        {
            Items = new List<CommandInfo>();
        }

        public CommandInfoList(List<CommandInfo> items)
        {
            Items = items;
        }

        [DataMember]
        public List<CommandInfo> Items { get; set; }

        public string ToJson()
        {
            using (var stream = new MemoryStream())
            {
                var ser = new DataContractJsonSerializer(typeof(CommandInfoList));
                ser.WriteObject(stream, this);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        public static CommandInfoList FromJson(string json)
        {
            var cmdInfoList = new CommandInfoList();
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                var ser = new DataContractJsonSerializer(cmdInfoList.GetType());
                return ser.ReadObject(stream) as CommandInfoList;
            }
        }

    }
}
