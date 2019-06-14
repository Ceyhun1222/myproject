using Europe_ICAO015;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICAO015.Draw_Remove_Lists
{
    public class Draw_Remove_Command
    {
        public static List<Draw_RemoveMarker_NDB_DMEN_DVOR_CVORList> Draw_RemoveList { get; set; }
        public static void DrawRemoveILS_OR_DME(List<Draw_RemoveMarker_NDB_DMEN_DVOR_CVORList> Drawlist, string Childnode, string Parentnode)
        {
            for (int i = 0; i < Drawlist.Count; i++)
            {
                if (Drawlist[i].ChildNode == Childnode && Drawlist[i].ParentNode == Parentnode)
                {
                    if (Drawlist[i].MiddleRadius == 0)
                    {
                        GlobalParams.AranEnvironment.Graphics.SafeDeleteGraphic(Drawlist[i].SmallRadius);
                        GlobalParams.AranEnvironment.Graphics.SafeDeleteGraphic(Drawlist[i].LargeRadius);
                        Drawlist.RemoveAt(i);
                    }
                    else if (Drawlist[i].MiddleRadius != 0)
                    {
                        GlobalParams.AranEnvironment.Graphics.SafeDeleteGraphic(Drawlist[i].SmallRadius);
                        GlobalParams.AranEnvironment.Graphics.SafeDeleteGraphic(Drawlist[i].MiddleRadius);
                        GlobalParams.AranEnvironment.Graphics.SafeDeleteGraphic(Drawlist[i].LargeRadius);
                        Drawlist.RemoveAt(i);
                    }
                }
            }
        }
        public static void DrawRemoveILS_OR_DME(List<Draw_Remove_ILS_List> Drawlist, string Childnode, string Parentnode)
        {
            for (int i = 0; i < Drawlist.Count; i++)
            {
                if (Drawlist[i].ChildNode == Childnode && Drawlist[i].ParentNode == Parentnode)
                {
                    GlobalParams.AranEnvironment.Graphics.SafeDeleteGraphic(Drawlist[i].Square);
                    GlobalParams.AranEnvironment.Graphics.SafeDeleteGraphic(Drawlist[i].FirstCornerPolygon);
                    GlobalParams.AranEnvironment.Graphics.SafeDeleteGraphic(Drawlist[i].SecondCornerPolygon);
                    GlobalParams.AranEnvironment.Graphics.SafeDeleteGraphic(Drawlist[i].SegmentPolygon);
                    Drawlist.RemoveAt(i);
                }
            }
        }
    }
}
