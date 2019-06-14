using Aran.AranEnvironment.Symbols;
using Aran.PANDA.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Aran.PANDA.Vss
{
    public class SymbolSettings
    {
        private Dictionary<DrawElementType, BaseSymbol> _symbols;

        public SymbolSettings()
        {
            _symbols = new Dictionary<DrawElementType, BaseSymbol>();

            var methods = GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var method in methods) {
                var attrs = method.GetCustomAttributes(typeof(DrawElementMethodAttribute), false);
                if (attrs != null && attrs.Length > 0) {
                    var baseSymbol = method.Invoke(this, null) as BaseSymbol;
                    var elementType = (attrs[0] as DrawElementMethodAttribute).ElementType;
                    _symbols.Add(elementType, baseSymbol);
                }
            }
        }

        public BaseSymbol this[DrawElementType elemType]
        {
            get { return _symbols[elemType]; }
        }

        [DrawElementMethod(DrawElementType.GuidanceFacilityCourse)]
        private BaseSymbol FillGuidanceFacilityCourse()
        {
            var lineSymbol = new LineSymbol();
            lineSymbol.Color = Color.FromArgb(0, Color.Green).ToArgb();
            lineSymbol.Width = 1;
            lineSymbol.Style = eLineStyle.slsSolid;

            return lineSymbol;
        }

        [DrawElementMethod(DrawElementType.RwyDirCourse)]
        private BaseSymbol FillRwyDirCourse()
        {
            var lineSymbol = new LineSymbol();
            lineSymbol.Color = Color.Green.ToAranRgb();
            lineSymbol.Width = 1;
            lineSymbol.Style = eLineStyle.slsDash;

            return lineSymbol;
        }

        [DrawElementMethod(DrawElementType.FafPoint)]
        private BaseSymbol FillFafPoint()
        {
            var pointSymbol = new PointSymbol();
            pointSymbol.Color = Color.Red.ToAranRgb();
            pointSymbol.Size = 8;
            pointSymbol.Style = ePointStyle.smsCircle;

            return pointSymbol;
        }

        [DrawElementMethod(DrawElementType.TrackLine)]
        private BaseSymbol FillTrackLine()
        {
            var lineSymbol = new LineSymbol();
            lineSymbol.Color = Color.Blue.ToAranRgb();
            lineSymbol.Width = 1;
            lineSymbol.Style = eLineStyle.slsSolid;

            return lineSymbol;
        }

        [DrawElementMethod(DrawElementType.FicTHR)]
        private BaseSymbol FillFicTHR()
        {
            var pointSymbol = new PointSymbol();
            pointSymbol.Color = Color.YellowGreen.ToAranRgb();
            pointSymbol.Size = 8;
            pointSymbol.Style = ePointStyle.smsCircle;

            return pointSymbol;
        }

        [DrawElementMethod(DrawElementType.TrackCourseIntersect)]
        private BaseSymbol FillTrackCourseIntersect()
        {
            var pointSymbol = new PointSymbol();
            pointSymbol.Color = Color.Tomato.ToAranRgb();
            pointSymbol.Size = 8;
            pointSymbol.Style = ePointStyle.smsCircle;

            return pointSymbol;
        }

        [DrawElementMethod(DrawElementType.Point1400FromThr)]
        private BaseSymbol FillPoint1400FromThr()
        {
            var pointSymbol = new PointSymbol();
            pointSymbol.Color = Color.Violet.ToAranRgb();
            pointSymbol.Size = 8;
            pointSymbol.Style = ePointStyle.smsCircle;

            return pointSymbol;
        }

        [DrawElementMethod(DrawElementType.Point60FromThr)]
        private BaseSymbol FillPoint60FromThr()
        {
            var pointSymbol = new PointSymbol();
            pointSymbol.Color = Color.LightGreen.ToAranRgb();
            pointSymbol.Size = 8;
            pointSymbol.Style = ePointStyle.smsCircle;

            return pointSymbol;
        }

        [DrawElementMethod(DrawElementType.Point60FromThrRight)]
        private BaseSymbol FillPoint60FromThrRight()
        {
            var pointSymbol = new PointSymbol();
            pointSymbol.Color = Color.LightGreen.ToAranRgb();
            pointSymbol.Size = 8;
            pointSymbol.Style = ePointStyle.smsCircle;

            return pointSymbol;
        }

        [DrawElementMethod(DrawElementType.Point60FromThrLeft)]
        private BaseSymbol FillPoint60FromThrLeft()
        {
            var pointSymbol = new PointSymbol();
            pointSymbol.Color = Color.LightGreen.ToAranRgb();
            pointSymbol.Size = 8;
            pointSymbol.Style = ePointStyle.smsCircle;

            return pointSymbol;
        }

        [DrawElementMethod(DrawElementType.VssArea)]
        private BaseSymbol FillPointVssArea()
        {
            var ls = new LineSymbol();
            ls.Width = 1;
            ls.Color = Color.LightBlue.ToAranRgb();
            ls.Style = eLineStyle.slsSolid;
            var fillSymbol = new FillSymbol();
            fillSymbol.Style = eFillStyle.sfsNull;
            fillSymbol.Outline = ls;
            return fillSymbol;
        }
    }
}
