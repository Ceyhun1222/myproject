using Aran.Aim.Metadata.ISO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Metadata.Utils
{
    public static class ReferenceSystemUtil
    {
        private const int WgsEpsgDefault = 32634;
        private const string CodeSpace = "EPSG";
        private const string Version = "6.18.3";

        public static void SetCoordinateSystemReference(MdMetadata metadata, string coordinateSystemReferenceName)
        {
            int code = WgsEpsgDefault;
            if (WgsEpsg.ContainsKey(coordinateSystemReferenceName))
                code = WgsEpsg[coordinateSystemReferenceName];

            var referenceSystemInfo = new RsIdentifier
            {
                Code = $"urn:ogc:def:crs:{CodeSpace}:{code}",
                CodeSpace = CodeSpace,
                Version = Version
            };

            metadata.ReferenceSystemInfo.Clear();
            metadata.ReferenceSystemInfo.Add(referenceSystemInfo);
        }
        
        private static Dictionary<string, int> WgsEpsg = new Dictionary<string, int>
        {
            {"WGS_1984_UTM_ZONE_1N", 32601},
            {"WGS_1984_UTM_ZONE_2N", 32602},
            {"WGS_1984_UTM_ZONE_3N", 32603},
            {"WGS_1984_UTM_ZONE_4N", 32604},
            {"WGS_1984_UTM_ZONE_5N", 32605},
            {"WGS_1984_UTM_ZONE_6N", 32606},
            {"WGS_1984_UTM_ZONE_7N", 32607},
            {"WGS_1984_UTM_ZONE_8N", 32608},
            {"WGS_1984_UTM_ZONE_9N", 32609},
            {"WGS_1984_UTM_ZONE_10N", 32610},
            {"WGS_1984_UTM_ZONE_11N", 32611},
            {"WGS_1984_UTM_ZONE_12N", 32612},
            {"WGS_1984_UTM_ZONE_13N", 32613},
            {"WGS_1984_UTM_ZONE_14N", 32614},
            {"WGS_1984_UTM_ZONE_15N", 32615},
            {"WGS_1984_UTM_ZONE_16N", 32616},
            {"WGS_1984_UTM_ZONE_17N", 32617},
            {"WGS_1984_UTM_ZONE_18N", 32618},
            {"WGS_1984_UTM_ZONE_19N", 32619},
            {"WGS_1984_UTM_ZONE_20N", 32620},
            {"WGS_1984_UTM_ZONE_21N", 32621},
            {"WGS_1984_UTM_ZONE_22N", 32622},
            {"WGS_1984_UTM_ZONE_23N", 32623},
            {"WGS_1984_UTM_ZONE_24N", 32624},
            {"WGS_1984_UTM_ZONE_25N", 32625},
            {"WGS_1984_UTM_ZONE_26N", 32626},
            {"WGS_1984_UTM_ZONE_27N", 32627},
            {"WGS_1984_UTM_ZONE_28N", 32628},
            {"WGS_1984_UTM_ZONE_29N", 32629},
            {"WGS_1984_UTM_ZONE_30N", 32630},
            {"WGS_1984_UTM_ZONE_31N", 32631},
            {"WGS_1984_UTM_ZONE_32N", 32632},
            {"WGS_1984_UTM_ZONE_33N", 32633},
            {"WGS_1984_UTM_ZONE_34N", 32634},
            {"WGS_1984_UTM_ZONE_35N", 32635},
            {"WGS_1984_UTM_ZONE_36N", 32636},
            {"WGS_1984_UTM_ZONE_37N", 32637},
            {"WGS_1984_UTM_ZONE_38N", 32638},
            {"WGS_1984_UTM_ZONE_39N", 32639},
            {"WGS_1984_UTM_ZONE_40N", 32640},
            {"WGS_1984_UTM_ZONE_41N", 32641},
            {"WGS_1984_UTM_ZONE_42N", 32642},
            {"WGS_1984_UTM_ZONE_43N", 32643},
            {"WGS_1984_UTM_ZONE_44N", 32644},
            {"WGS_1984_UTM_ZONE_45N", 32645},
            {"WGS_1984_UTM_ZONE_46N", 32646},
            {"WGS_1984_UTM_ZONE_47N", 32647},
            {"WGS_1984_UTM_ZONE_48N", 32648},
            {"WGS_1984_UTM_ZONE_49N", 32649},
            {"WGS_1984_UTM_ZONE_50N", 32650},
            {"WGS_1984_UTM_ZONE_51N", 32651},
            {"WGS_1984_UTM_ZONE_52N", 32652},
            {"WGS_1984_UTM_ZONE_53N", 32653},
            {"WGS_1984_UTM_ZONE_54N", 32654},
            {"WGS_1984_UTM_ZONE_55N", 32655},
            {"WGS_1984_UTM_ZONE_56N", 32656},
            {"WGS_1984_UTM_ZONE_57N", 32657},
            {"WGS_1984_UTM_ZONE_58N", 32658},
            {"WGS_1984_UTM_ZONE_59N", 32659},
            {"WGS_1984_UTM_ZONE_1S", 32701},
            {"WGS_1984_UTM_ZONE_2S", 32702},
            {"WGS_1984_UTM_ZONE_3S", 32703},
            {"WGS_1984_UTM_ZONE_4S", 32704},
            {"WGS_1984_UTM_ZONE_5S", 32705},
            {"WGS_1984_UTM_ZONE_6S", 32706},
            {"WGS_1984_UTM_ZONE_7S", 32707},
            {"WGS_1984_UTM_ZONE_8S", 32708},
            {"WGS_1984_UTM_ZONE_9S", 32709},
            {"WGS_1984_UTM_ZONE_10S", 32710},
            {"WGS_1984_UTM_ZONE_11S", 32711},
            {"WGS_1984_UTM_ZONE_12S", 32712},
            {"WGS_1984_UTM_ZONE_13S", 32713},
            {"WGS_1984_UTM_ZONE_14S", 32714},
            {"WGS_1984_UTM_ZONE_15S", 32715},
            {"WGS_1984_UTM_ZONE_16S", 32716},
            {"WGS_1984_UTM_ZONE_17S", 32717},
            {"WGS_1984_UTM_ZONE_18S", 32718},
            {"WGS_1984_UTM_ZONE_19S", 32719},
            {"WGS_1984_UTM_ZONE_20S", 32720},
            {"WGS_1984_UTM_ZONE_21S", 32721},
            {"WGS_1984_UTM_ZONE_22S", 32722},
            {"WGS_1984_UTM_ZONE_23S", 32723},
            {"WGS_1984_UTM_ZONE_24S", 32724},
            {"WGS_1984_UTM_ZONE_25S", 32725},
            {"WGS_1984_UTM_ZONE_26S", 32726},
            {"WGS_1984_UTM_ZONE_27S", 32727},
            {"WGS_1984_UTM_ZONE_28S", 32728},
            {"WGS_1984_UTM_ZONE_29S", 32729},
            {"WGS_1984_UTM_ZONE_30S", 32730},
            {"WGS_1984_UTM_ZONE_31S", 32731},
            {"WGS_1984_UTM_ZONE_32S", 32732},
            {"WGS_1984_UTM_ZONE_33S", 32733},
            {"WGS_1984_UTM_ZONE_34S", 32734},
            {"WGS_1984_UTM_ZONE_35S", 32735},
            {"WGS_1984_UTM_ZONE_36S", 32736},
            {"WGS_1984_UTM_ZONE_37S", 32737},
            {"WGS_1984_UTM_ZONE_38S", 32738},
            {"WGS_1984_UTM_ZONE_39S", 32739},
            {"WGS_1984_UTM_ZONE_40S", 32740},
            {"WGS_1984_UTM_ZONE_41S", 32741},
            {"WGS_1984_UTM_ZONE_42S", 32742},
            {"WGS_1984_UTM_ZONE_43S", 32743},
            {"WGS_1984_UTM_ZONE_44S", 32744},
            {"WGS_1984_UTM_ZONE_45S", 32745},
            {"WGS_1984_UTM_ZONE_46S", 32746},
            {"WGS_1984_UTM_ZONE_47S", 32747},
            {"WGS_1984_UTM_ZONE_48S", 32748},
            {"WGS_1984_UTM_ZONE_49S", 32749},
            {"WGS_1984_UTM_ZONE_50S", 32750},
            {"WGS_1984_UTM_ZONE_51S", 32751},
            {"WGS_1984_UTM_ZONE_52S", 32752},
            {"WGS_1984_UTM_ZONE_53S", 32753},
            {"WGS_1984_UTM_ZONE_54S", 32754},
            {"WGS_1984_UTM_ZONE_55S", 32755},
            {"WGS_1984_UTM_ZONE_56S", 32756},
            {"WGS_1984_UTM_ZONE_57S", 32757},
            {"WGS_1984_UTM_ZONE_58S", 32758},
            {"WGS_1984_UTM_ZONE_59S", 32759},
            {"WGS_1984_UTM_ZONE_60S", 32760},
        };
    }
}
