using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PDM;
using ARENA;

namespace ChartTypeA
{
    class ArenaDbModule
    {
        private List<AirportHeliport> _airportHeliportList;
        private List<PDMObject> _pdmObjectList;
        private List<VerticalStructure> _vsList;
        private List<TaxiwayElement> _taxiwayList;
        private List<RunwayElement> _rwyElementList;

        public ArenaDbModule()
        {
            if (DataCash.ProjectEnvironment == null)
            {
                //MessageBox.Show("Error loading Database!");
                throw new Exception("Error loading Db");
            }

            _pdmObjectList = DataCash.ProjectEnvironment.Data.PdmObjectList;// 
        }

        public List<PDM.AirportHeliport> AirportHeliportList
        {
            get
            {
                if (_airportHeliportList == null)
                {
                    if (_pdmObjectList != null)
                    {
                        _airportHeliportList =
                             _pdmObjectList.Where(pdmObject => pdmObject is PDM.AirportHeliport)
                             .Cast<PDM.AirportHeliport>()
                             .OrderBy(adhp=>adhp.Name)
                             .ToList();

                    }
                }
                return _airportHeliportList;
            }
        }

        public List<Runway> GetRunwayList(AirportHeliport adhp)
        {
            if (_pdmObjectList != null)
            {
                return adhp.RunwayList;
            }
            return new List<Runway>();
        }

        public List<RunwayDirection> GetRunwayDirectionList(Runway rwy)
        {
            if (_pdmObjectList != null)
            {
                return rwy.RunwayDirectionList;
            }
            return new List<RunwayDirection>();
        }
        public List<RunwayCenterLinePoint> GetRunwayCntLineList(RunwayDirection rwyDir)
        {
            if (_pdmObjectList != null)
            {
                var tmpRunwayCntLineList = _pdmObjectList.Where
                    (pdmObject => pdmObject is PDM.RunwayDirection && (pdmObject as RunwayCenterLinePoint).ID_RunwayDirection == rwyDir.ID).
                    Cast<PDM.RunwayCenterLinePoint>().ToList();
                return tmpRunwayCntLineList;
            }
            return new List<RunwayCenterLinePoint>();
        }

        public List<PDM.VerticalStructure> GetObstacleList
        {
             get
            {
                if (_vsList == null)
                {
                    _vsList =
                        _pdmObjectList.Where(pdmObject => pdmObject is PDM.VerticalStructure)
                            .Cast<PDM.VerticalStructure>()
                            .ToList();
                }
                return _vsList;
            }
        }

        public List<PDM.TaxiwayElement> GetTaxiwayList(string idAdhp)
        {
            if (_taxiwayList == null)
            {
                _taxiwayList =
                    _pdmObjectList.Where(pdmObject => pdmObject is PDM.TaxiwayElement)// && (pdmObject as PDM.TaxiwayElement).ID_AirportHeliport == idAdhp)
                        .Cast<PDM.TaxiwayElement>()
                        .ToList();
            }
            return _taxiwayList;

        }

        public List<PDM.RunwayElement> GetRunwayElementList(string runwayId)
        {
            if (_rwyElementList == null)
            {
                _rwyElementList =
                    _pdmObjectList.Where(pdmObject => pdmObject is PDM.RunwayElement && (pdmObject as PDM.RunwayElement).ID_Runway == runwayId)
                        .Cast<PDM.RunwayElement>()
                        .ToList();
            }
            return _rwyElementList;

        }
    }
}
