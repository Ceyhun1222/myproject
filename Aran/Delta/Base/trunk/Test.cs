using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Delta
{
//    public class DbModule
//    {
//        private List<PDM.Enroute> _routeList;
//        public DbModule()
//        {
//            try
//            {
//                var _fileName = @"D:\AirNav\PDM\LatviaAll.pdm";
//                var tempDirName = System.IO.Path.GetTempPath();
//                var dInf = Directory.CreateDirectory(tempDirName + @"\PDM\" + System.IO.Path.GetFileNameWithoutExtension(_fileName));
//                tempDirName = dInf.FullName;
//                var tempPdmFilename = System.IO.Path.Combine(tempDirName, "pdm.pdm");
//                var tempMxdFilename = System.IO.Path.Combine(tempDirName, "pdm.mxd");


//                ZzArchivatorClass.ExtractFromArchive(ArenaStaticProc.GetPathToTemplateFile() + @"\Utils\7z.exe", _fileName, tempDirName);
//                tempMxdFilename = System.IO.Path.Combine(tempDirName, "arena_PDM.mxd");
//                _pdmObjectList = ArenaDataModule.GetObjectsFromPdmFile(tempDirName);


                
//            }
//            catch (Exception e)
//            {

//                throw new Exception("Database error!"+e.Message);
//            }
           
//        }

//        public List<Enroute> RouteList
//        {
//            get
//            {
//                if (_routeList == null)
//                    _routeList = _pdmObjectList.Where(pdmObject => pdmObject is PDM.Enroute).Cast<PDM.Enroute>().ToList();
//                return _routeList;
//            }
//        }

//        public List<RouteSegment> GetRouteSegmentList(Guid routeIdentifier)
//        {
//            var selRoute = _routeList.First(route => route.ID == routeIdentifier.ToString());
//            if (selRoute != null)
//                return selRoute.Routes;
//            return new List<RouteSegment>();
//        }
        
//        public AirportHeliport AirportHeliport { get;private set; }

//        public List<PDM.WayPoint> DesignatedPointList { get; private set; }

//        public List<PDM.NavaidSystem> NavaidList { get; private set; }

//        private List<Airspace> _airspaceList;
//        private List<PDMObject> _pdmObjectList;

//        public List<Airspace> GetAirspaceList
//        {
//            get
//            {
//                if (_airspaceList == null)
//                    _airspaceList = _pdmObjectList.Where(pdmObject => pdmObject is PDM.Airspace).
//                        Cast<PDM.Airspace>().ToList();
                
//                return _airspaceList;
//            }
//        }


//        private AirportHeliport GetAirportHeliport()
//        {
//            var adhp =
//                _pdmObjectList.First(
//                    pdmObject =>
//                        pdmObject.ID == GlobalParams.Settings.DeltaQuery.Aeroport.ToString() &&
//                        pdmObject is PDM.AirportHeliport);

//            return adhp as PDM.AirportHeliport;
//        }

//// ReSharper disable once InconsistentNaming
        
//    }
//}
}
