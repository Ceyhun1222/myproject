using PVT.Engine.Graphics;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using PVT.Engine.Common.Graphics;
using Aran.Geometries;
using Aran.AranEnvironment;
using Aran.AranEnvironment.Symbols;
using PVT.Drawing.Symbols;
using System.Collections.Generic;

namespace PVT.Engine.IAIM.Graphics
{
    class Graphics : CommonGraphics
    {

        private IHookHelper _hookHelper;
        private IAranGraphics _graphics;
        private IAranLayoutViewGraphics _layoutGraphics;
        private IAranEnvironment _aranEnv;

        public override void Init()
        {
            if (IsInit) return;
            _hookHelper = new HookHelper();
            _aranEnv = ((IAIMEnvironment)Environment.Current).AranEnv;
            _hookHelper.Hook = _aranEnv.HookObject;
            _graphics = _aranEnv.Graphics;
            _layoutGraphics = _aranEnv.LayoutGraphics;
            IsInit = true;
        }


        public IActiveView GetActiveView()
        {
            return _hookHelper.ActiveView;
        }

        public string GetMapFileName()
        {
            if (_hookHelper.Hook is ESRI.ArcGIS.Framework.IApplication)
            {
                var app = (ESRI.ArcGIS.Framework.IApplication)_hookHelper.Hook;
                return app.Templates.get_Item(app.Templates.Count - 1);
            }
            else
                return _aranEnv.DocumentFileName;
        }

        public int GetApplicationHWnd()
        {
            if (_hookHelper.Hook is ESRI.ArcGIS.Framework.IApplication)
                return ((ESRI.ArcGIS.Framework.IApplication)_hookHelper.Hook).hWnd;
            else
                return _aranEnv.Win32Window.Handle.ToInt32();
        }

        public override int Draw(MultiPolygon multipolygon, int color, int width)
        {
            return Draw(multipolygon, LineStyles.slsSolid, color, width);
        }

        public override int Draw(MultiPolygon multipolygon, LineStyles style, int color, int width)
        {
            var fillSymbol = new FillSymbol
            {
                Color = 242424,
                Outline = new LineSymbol((eLineStyle)style, color, width)
            };
            fillSymbol.Size = width;
            fillSymbol.Style = eFillStyle.sfsHollow;

            return _graphics.DrawMultiPolygon(multipolygon, fillSymbol);
        }

        public override int Draw(MultiLineString multiline, int color, int width)
        {
            return _graphics.DrawMultiLineString(multiline, width, color);
        }

        public override int Draw(MultiLineString multiline, LineStyles style, int color, int width)
        {
            return _graphics.DrawMultiLineString(multiline, new LineSymbol((eLineStyle)style, color, width));
        }

        public override int Draw(Point point, int color, int size)
        {
            return _graphics.DrawPoint(point, new PointSymbol(color, size));
        }

        public override int Draw(Point point, PointStyles style, int color, int size)
        {
            return _graphics.DrawPoint(point, new PointSymbol((ePointStyle) style, color, size));
        }

        public override int Draw(Point point, string text, int color, int size)
        {
            return _graphics.DrawPointWithText(point, text, new PointSymbol(color, size));
        }

        public override int Draw(Point point, string text, PointStyles style, int color, int size)
        {
            return _graphics.DrawPointWithText(point, text, new PointSymbol((ePointStyle)style, color, size));
        }

        public override void Delete(int handler)
        {
            _graphics.SafeDeleteGraphic(handler);
        }

        public override void SetLayoutText(List<string> text)
        {
            _layoutGraphics.Text = text ;
        }

        public override void CreateLayout()
        {
            _layoutGraphics.Visible = true;
        }

        public override void DeleteLayout()
        {
            _layoutGraphics.Visible = false;
        }

    }
}
