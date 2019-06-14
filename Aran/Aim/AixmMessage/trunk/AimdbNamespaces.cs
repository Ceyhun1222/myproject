using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Aim.AixmMessage
{
    public static class AimDbNamespaces
    {
        public static readonly NamespaceInfo AIXM51 = new NamespaceInfo() { Prefix = "aixm-5.1", Namespace = "http://www.aixm.aero/schema/5.1" };
        public static readonly NamespaceInfo AIXM51Message = new NamespaceInfo() { Prefix = "aixm-message-5.1", Namespace = "http://www.aixm.aero/schema/5.1/message" };
        public static readonly NamespaceInfo GML = new NamespaceInfo() { Prefix = "gml", Namespace = "http://www.opengis.net/gml/3.2" };
        public static readonly NamespaceInfo SoapEnv = new NamespaceInfo() { Prefix = "SOAP-ENV", Namespace = "http://schemas.xmlsoap.org/soap/envelope/" };
        public static readonly NamespaceInfo XLINK = new NamespaceInfo() { Prefix = "xlink", Namespace = "http://www.w3.org/1999/xlink" };
        public static readonly NamespaceInfo XSI = new NamespaceInfo() { Prefix = "xsi", Namespace = "http://www.w3.org/2001/XMLSchema-instance" };
    }

    public class NamespaceInfo
    {
        public string Prefix { get; set; }
        public string Namespace { get; set; }
    }
}
