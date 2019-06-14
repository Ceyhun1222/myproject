using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.AranEnvironment;
using Aran.Geometries.Operators;
using Aran.Geometries;
using Aran.AranEnvironment.Symbols;
using Aran.PANDA.Common;

namespace Holding
{
    public class HoldingGeometry
    {
        #region :>Fields
        List<int> _areasHandle;
        private IAranGraphics _ui;
        private GeometryOperators _geomOperators;
        #endregion

        #region Constructor

        public HoldingGeometry()
        {
            _geomOperators = GlobalParams.GeomOperators;
            _ui = GlobalParams.UI;
            Areas = new List<Geometry>();
            _areasHandle = new List<int>();
            HoldingTrackIsChecked = true;
            ToleranceIsChecked = true;
        }

        #endregion

        #region :>Property

        public int HoldingAreaHandle { get; set; }

        public int HoldingTrackHandle { get; set; }

        public int FullAreaHandle { get; set; }

        public int SectorProtectionHandle { get; set; }

        public int ToleranceAreaHandle { get; set; }

        public int ShablonHandle { get; set; }

        public int BufferHandle { get; set; }

        public int Turn180Handle { get; set; }

        public Aran.Geometries.Polygon HoldingArea { get; set; }

        public Aran.Geometries.LineString HoldingTrack { get; set; }

        public Aran.Geometries.MultiPolygon FullArea { get; set; }

        public Aran.Geometries.MultiPolygon SectorProtection { get; set; }

        public Aran.Geometries.Ring ToleranceArea { get; set; }

        public Aran.Geometries.Polygon Shablon { get; set; }

        public Aran.Geometries.MultiPolygon Buffer { get; set; }

        public Aran.Geometries.MultiPolygon AreaWithSectors { get; set; }

        public List<Aran.Geometries.Geometry> Areas { get; private set; }

        public Aran.Geometries.LineString Turn180 { get; set; }

        public bool HoldingAreaIsChecked { get; set; }

        public bool HoldingTrackIsChecked { get; set; }

        public bool BufferIsChecked { get; set; }

        public bool ToleranceIsChecked { get; set; }

        public bool ShablonIsChecked { get; set; }

        public bool Turn180IsChecked { get; set; }

        public bool SectorProtectionIsChecked { get; set; }

        #endregion

        #region :>Methods

        public void Draw()
        {
            ClearGraph();
           // if (HoldingAreaIsChecked)
            DrawHoldingArea();
            
            if (ToleranceIsChecked)
                DrawToleranceArea();
            
            if (HoldingTrackIsChecked)
                DrawHoldingTrack();
             
            if (SectorProtectionIsChecked)
                DrawSectorEntries();
            
            if (ShablonIsChecked)
                DrawShablon();

            if (BufferIsChecked)
                DrawBuffers();

            if (Turn180IsChecked)
                DrawTurn180();
        }

        public void ClearGraph()
        {
            _ui.SafeDeleteGraphic(HoldingAreaHandle);
            _ui.SafeDeleteGraphic(HoldingTrackHandle);
            _ui.SafeDeleteGraphic(FullAreaHandle);
            _ui.SafeDeleteGraphic(SectorProtectionHandle);
            _ui.SafeDeleteGraphic(ToleranceAreaHandle);
            _ui.SafeDeleteGraphic(ShablonHandle);
            _ui.SafeDeleteGraphic(Turn180Handle);

            for (int i = 0; i < _areasHandle.Count; i++)
            {
                int areaHandle = _areasHandle[i];
                _ui.SafeDeleteGraphic(areaHandle);
            }
            _areasHandle.Clear();
        }

        public void Clear()
        {
            HoldingArea = null;
            HoldingTrack = null;
            FullArea = null;
            SectorProtection = null;
            ToleranceArea = null;
            Shablon = null;
            Buffer = null;
            Turn180 = null;
            Areas.Clear();
        }
            
        public void DrawShablon()
        {
            _ui.SafeDeleteGraphic(ShablonHandle);
			if (Shablon != null)
				ShablonHandle = _ui.DrawPolygon(Shablon, eFillStyle.sfsHorizontal, ARANFunctions.RGB(0, 255, 0), true, false);
        }

        public void DrawHoldingArea()
        {
            _ui.SafeDeleteGraphic(HoldingAreaHandle);
			if (HoldingArea != null)
				HoldingAreaHandle = _ui.DrawMultiPolygon(AreaWithSectors, eFillStyle.sfsNull, ARANFunctions.RGB(255, 0, 0), true, false);
        }
        public void DrawToleranceArea()
        {
            _ui.SafeDeleteGraphic(ToleranceAreaHandle);
			if (ToleranceArea != null)
				ToleranceAreaHandle = _ui.DrawRing(ToleranceArea, eFillStyle.sfsDiagonalCross, 239, true, false);
        }

        public void DrawSectorEntries()
        {
            _ui.SafeDeleteGraphic(SectorProtectionHandle);
			if (SectorProtection != null)
				SectorProtectionHandle = _ui.DrawMultiPolygon(SectorProtection, eFillStyle.sfsCross, ARANFunctions.RGB(255, 0, 0), true, false);
        }

        public void DrawHoldingTrack()
        {
            _ui.SafeDeleteGraphic(HoldingTrackHandle);
			if (HoldingTrack != null)
				HoldingTrackHandle = _ui.DrawLineString(HoldingTrack, 1, 239, true, false);
        }

        public void DrawTurn180()
        {
            _ui.SafeDeleteGraphic(Turn180Handle);
			if (Turn180 != null)
				Turn180Handle = _ui.DrawLineString(Turn180, 1, 200, true, false);
        }

        public void DrawBuffers()
        {
            ClearBuffers();
            
            foreach (var area in Areas)
            {
                foreach (var poly in area)
                {

					_areasHandle.Add(_ui.DrawPolygon(poly as Polygon, eFillStyle.sfsNull, ARANFunctions.RGB(0, 0, 255), true, false));
                }
            }
        }

        public void ClearBuffers()
        {
            for (int i = 0; i < _areasHandle.Count; i++)
            {
                int areaHandle = _areasHandle[i];
                _ui.SafeDeleteGraphic(areaHandle);
            }
            _areasHandle.Clear();
        }

        #endregion
    }
}
