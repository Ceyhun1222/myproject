using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Integration;
using System.Windows.Interop;
using Aran.Panda.RadarMA.View;
using Aran.PANDA.Common;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;

namespace Aran.Panda.RadarMA.Models
{
    public class Sector : NotifyableBase
    {
        private int _handle;
        private readonly ISymbol _symbol;
        private int _selectedHandle;
        private int _stateMaxElevPtHandle;
        private UnitConverter _unitConverter;

        public Sector(IPolygon geoPolygon, ISymbol symbol,List<ObstacleReport> reportList,UnitConverter unitConverter)
        {
            _symbol = symbol;
            Geo = geoPolygon;

            Reports = reportList;
            _unitConverter = unitConverter;
        }

        public RelayCommand ReportCommand => new RelayCommand((comm) =>
        {
            var obstacleReportView = new ObstacleReportView(Reports);
            var parentHandle = new IntPtr(GlobalParams.Handle);
            var helper = new WindowInteropHelper(obstacleReportView) { Owner = parentHandle };
            ElementHost.EnableModelessKeyboardInterop(obstacleReportView);
            obstacleReportView.ShowInTaskbar = false; // hide from taskbar and alt-tab list
            obstacleReportView.Show();

        });

        public string HeaderName => "More";

        public List<ObstacleReport> Reports { get; }

        public int Number { get; set; }

        public IPolygon UnionGeometry { get; set; }

        public int StateNumber { get; set; }

        public string Name => "Sector " + Number;

        public double MOC { get; set; }

        public double Height { get; set; }

        public double BufferValue { get; set; }

        public double Area { get; set; }

        public IPolygon Geo { get;}

        public IPolygon Buffer { get; set; }

        public IPoint StateMaxElevPoint { get; set; }

        public void Draw()
        {
            if (Buffer != null && !Buffer.IsEmpty)
            {
                GlobalParams.UI.SafeDeleteGraphic(_handle);
                _handle = GlobalParams.UI.DrawEsriPolygon(Buffer, _symbol);

                GlobalParams.UI.SafeDeleteGraphic(_stateMaxElevPtHandle);
                if (StateMaxElevPoint != null && !StateMaxElevPoint.IsEmpty)
                {
                    var ptPrj = StateMaxElevPoint;
                    _stateMaxElevPtHandle = GlobalParams.UI.DrawPointWithText(ptPrj,
                        "Heighest Point " + _unitConverter.HeightToDisplayUnits(Height) + _unitConverter.HeightUnit, 8, ARANFunctions.RGB(255, 0, 0));
                }
            }
        }

        public void Clear()
        {
            ClearSelected();
            GlobalParams.UI.SafeDeleteGraphic(_stateMaxElevPtHandle);
            GlobalParams.UI.SafeDeleteGraphic(_handle);
        }

        internal void ClearSelected()
        {
            GlobalParams.UI.SafeDeleteGraphic(_selectedHandle);
        }

        internal void DrawWithOtherSymbol(ISymbol symbol, bool drawBuffer = false)
        {
            ClearSelected();
            _selectedHandle = GlobalParams.UI.DrawEsriPolygon(drawBuffer ? Buffer : Geo, symbol);
        }

        public void Update()
        {
            NotifyPropertyChanged("");
        }
    }
}
