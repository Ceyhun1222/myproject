using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Temporality.Common.TossConverter;

namespace TossConverter.Model
{
    public class LogInfo
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public MessageCauseType Type { get; set; }
        public DateTime dateTime { get; set; } = DateTime.Now;

        public LogInfo(MessageCauseType type, string title, string description = "")
        {
            Type = type;
            Title = title;
            Description = description;
        }
    }
}
