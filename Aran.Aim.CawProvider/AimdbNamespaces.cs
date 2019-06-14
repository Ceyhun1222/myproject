/// ***
/// ***
/// define in Project Properties->Build Conditional symbols: 
///     COMSOFT_NAMESPACE, PANDA_NAMESPACE or EUROCONTROL_NAMESPACE
/// ***
/// ***

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Aim.CAWProvider
{
    public static class AimdbNamespaces
    {
        public static readonly NamespaceInfo AIXM51 = new NamespaceInfo("aixm-5.1", "http://www.aixm.aero/schema/5.1");
        public static readonly NamespaceInfo AIXM51Message = new NamespaceInfo ("aixm-message-5.1", "http://www.aixm.aero/schema/5.1/message");
        public static readonly NamespaceInfo GML = new NamespaceInfo ("gml", "http://www.opengis.net/gml/3.2");
        public static readonly NamespaceInfo SoapEnv = new NamespaceInfo ("SOAP-ENV", "http://schemas.xmlsoap.org/soap/envelope/");
        public static readonly NamespaceInfo XLINK = new NamespaceInfo ("xlink", "http://www.w3.org/1999/xlink");
        public static readonly NamespaceInfo XSI = new NamespaceInfo ("xsi", "http://www.w3.org/2001/XMLSchema-instance");

#if COMSOFT_NAMESPACE
        public static readonly NamespaceInfo CAW = new NamespaceInfo ("caw", "http://www.comsoft.aero/cadas-aimdb/caw");
        public static readonly NamespaceInfo CAE = new NamespaceInfo ("cae", "http://www.comsoft.aero/cadas-aimdb/extension");
        public static readonly NamespaceInfo SDP = new NamespaceInfo ("sdp", "http://www.comsoft.aero/cadas-aimdb/sdp");
        public static readonly string IdentifierCodeSpace = "http://www.comsoft.aero/cadas-aimdb/caw";
#endif

#if PANDA_NAMESPACE
        public static readonly NamespaceInfo CAW = new NamespaceInfo ("caw", "http://www.pandanavigation.com/aran-aimdb");
        public static readonly NamespaceInfo CAE = new NamespaceInfo ("cae", "http://www.pandanavigation.com/aran-aimdb/extension");
        public static readonly NamespaceInfo SDP = new NamespaceInfo ("sdp", "http://www.pandanavigation.com/aran-aimdb/sdp");
#if EUROCONTROL_NAMESPACE
        public static readonly string IdentifierCodeSpace = "urn:uuid:";
#else
        public static readonly string IdentifierCodeSpace = "http://www.pandanavigation.com/aran-aimdb";
#endif
#endif
    }

    public class NamespaceInfo
    {
        public NamespaceInfo ()
        {
        }

        public NamespaceInfo (string prefix, string aNamespace)
        {
            Prefix = prefix;
            Namespace = aNamespace;
        }

        public string Prefix { get; set; }
        public string Namespace { get; set; }
    }
}
