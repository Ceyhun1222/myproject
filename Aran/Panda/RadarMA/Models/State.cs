using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.PANDA.Common;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;

namespace Aran.Panda.RadarMA.Models
{
    public class State
    {
        private int _stateGeoHandle;
        private int _stateMaxElevPtHandle;

        public State(ISymbol symbol)
        {
            Symbol = symbol;
            JoinSectorList = new List<Sector>();
        }

        public int SectorNumber { get; set; }

        public int StateNumber { get; set; }

        public IPolygon StateGeo { get; set; }

        public IPoint  StateMaxElevPoint { get; set; }

        public OperationType  OperType { get; set; }

        public ISymbol Symbol { get; set; }

        public double BufferValue { get; set; }

        public double Moc { get; set; }

        public double Altitude { get; set; }

        public double CircleRadius { get; set; }

        public bool DiffirentFromOtherSectorIsChecked { get; set; }

        public bool DiffirentFromRadOperAreaIsChecked { get; set; }

        public List<ObstacleReport> ObstacleReports { get; set; }

        public IPolygon BufferGeo { get; set; }

        public IPolygon UnionSectors { get; set; }

        public List<Sector> JoinSectorList { get; set; }

        public void Draw(bool isBuffer)
        {
            Clear();
            var geo = StateGeo;
            if (isBuffer && OperType!=OperationType.CreateSector)
                geo = BufferGeo;
            if (geo != null && !geo.IsEmpty)
            {
                _stateGeoHandle = GlobalParams.UI.DrawEsriPolygon(geo, Symbol);

                ShowHeighestPoint();
            }
        }

        public void ShowHeighestPoint()
        {
            ClearHeighestPoint();
            var displayAltititude = GlobalParams.UnitConverter.HeightToDisplayUnits(Altitude,eRoundMode.CEIL);

            if (OperType == OperationType.CreateSector || OperType == OperationType.JoinSectors)
            {
                var area = StateGeo as IArea;
                var centroid = area.Centroid;
                _stateMaxElevPtHandle = GlobalParams.UI.DrawText(centroid, displayAltititude + " "+GlobalParams.UnitConverter.HeightUnit, 20,
                    ARANFunctions.RGB(0, 0, 255));
            }
            else
            {
                if (StateMaxElevPoint != null && !StateMaxElevPoint.IsEmpty)
                {
                    var displayText = "Heighest Point " + displayAltititude + " " +
                                      GlobalParams.UnitConverter.HeightUnit;
                    _stateMaxElevPtHandle = GlobalParams.UI.DrawPointWithText(StateMaxElevPoint,
                        displayText, 8, ARANFunctions.RGB(255, 0, 0));
                }
            }
        }

        public void ClearHeighestPoint()
        {
            GlobalParams.UI.SafeDeleteGraphic(_stateMaxElevPtHandle);
        }

        public void Clear()
        {
            ClearHeighestPoint();
            GlobalParams.UI.SafeDeleteGraphic(_stateGeoHandle);
        }
    }
}
