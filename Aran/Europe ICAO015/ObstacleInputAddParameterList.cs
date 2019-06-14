using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICAO015
{


    public partial class ObstacleInputAddParameterList
    {
        //Calculate for only Radius {
        static List<ParameterForDmeN> ParamListDMEN = new List<ParameterForDmeN>();
        static List<ParameterForCVOR> ParamListCVOR = new List<ParameterForCVOR>();
        static List<ParameterForDVOR> ParamListDVOR = new List<ParameterForDVOR>();
        static List<ParameterForMarkers> ParamListMarker = new List<ParameterForMarkers>();
        static List<ParameterForNDB> ParamListNDB = new List<ParameterForNDB>();
        //Calculate for only Radius }
        //Calculate for only 2D Graphic {
        static List<Obstacle_ParamListPolygons> Obstcl_Input_ParamList_2DGrpahic = new List<Obstacle_ParamListPolygons>();
        //Calculate for only 2D Graphic }
        public ObstacleInputAddParameterList()
        {
            ParamListDMEN.Clear();
            ParamListCVOR.Clear();
            ParamListDVOR.Clear();
            ParamListMarker.Clear();
            ParamListNDB.Clear();
        }
        public static void AddParameter(List<ParameterForDmeN> ListDMEN, List<ParameterForCVOR> ListCVOR, List<ParameterForDVOR> ListDVOR, List<ParameterForMarkers> ListMarker, List<ParameterForNDB> ListNDB, List<Obstacle_ParamListPolygons> List2DGraphics)
        {
            ParamListDMEN = ListDMEN;
            ParamListCVOR = ListCVOR;
            ParamListDVOR = ListDVOR;
            ParamListMarker = ListMarker;
            ParamListNDB = ListNDB;
            Obstcl_Input_ParamList_2DGrpahic = List2DGraphics;
        }

        public static List<ParameterForDmeN> GetListParamDMEN()
        {
            return ParamListDMEN;
        }
        public static List<ParameterForCVOR> GetListParamCVOR()
        {
            return ParamListCVOR;
        }
        public static List<ParameterForDVOR> GetListParamDVOR()
        {
            return ParamListDVOR;
        }
        public static List<ParameterForMarkers> GetListParamMarker()
        {
            return ParamListMarker;
        }
        public static List<ParameterForNDB> GetListParamNDB()
        {
            return ParamListNDB;
        }
        public static List<Obstacle_ParamListPolygons> GetListParamList2DGraphic()
        {
            return Obstcl_Input_ParamList_2DGrpahic;
        }
    }
}
