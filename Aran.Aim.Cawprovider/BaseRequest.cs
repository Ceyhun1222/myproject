using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace Aran.Aim.CAWProvider
{
    /// <summary>
    /// BaseRequest ICD 2.2
    /// </summary>
    public abstract class BaseRequest : AbstractRequest
    {
        public BaseRequest ()
        {
            Service = "AIXM-WFS";
            Version = "5.0";
        }

        /// <summary>
        /// Service type identifier, where the string value is the OWS type abbreviation, such as "WMS" or "WFS",
        /// Default is "AIXM-WFS"
        /// </summary>
        public string Service { get; set; }

        public string Version { get; set; }
    }
}
