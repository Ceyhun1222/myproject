using Aran.Aim.AixmMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Aim.CAWProvider
{
    public static class CadasNamespaces
    {
        public static readonly NamespaceInfo CAW = new NamespaceInfo() { Prefix = "caw", Namespace = "http://www.comsoft.aero/cadas-aimdb/caw" };
        public static readonly NamespaceInfo CAE = new NamespaceInfo() { Prefix = "cae", Namespace = "http://www.comsoft.aero/cadas-aimdb/extension" };
        public static readonly NamespaceInfo SDP = new NamespaceInfo() { Prefix = "sdp", Namespace = "http://www.comsoft.aero/cadas-aimdb/sdp" };
    }
}
