using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;

namespace ChartTypeA.Models
{
    public class TextCreater
    {
        private int _fontSize;
        public TextCreater(int fontSize)
        {
            _fontSize = fontSize;
        }

        public IElement CreateHorizontalText(string text, IPoint pnt,  int side)
        {
            var textElement = CreateTextElement(text);

            IPoint tmp = new PointClass();
            tmp.PutCoords(pnt.X, pnt.Y - 0.4);

            var element = (IElement)textElement;
            element.Geometry = tmp;
            return element;
        }

        public IElement CreateVerticalText(string text, IPoint pnt, int side)
        {
            var textElement = CreateTextElement(text);

            IPoint tmp = new PointClass();
            tmp.PutCoords(pnt.X + side * (0.3 + 0.1 * (text.Length - 2)), pnt.Y);

            var element = (IElement)textElement;
            element.Geometry = tmp;
            return element;
        }

        private ITextElement CreateTextElement(string text)
        {
            var textElement = new TextElementClass();
            TextSymbolClass pTextSymbol = new TextSymbolClass { Size = _fontSize };

            textElement.Text = text;
            textElement.Symbol = pTextSymbol;
            return textElement;
        }


        //private void AddText ( string text, IPoint pnt, bool horizontal, int side, IGroupElement grpElement = null )
        //{
        //	var textElement = new TextElementClass ( );
        //	TextSymbolClass pTextSymbol = new TextSymbolClass ( );
        //	pTextSymbol.Size = fontSize;

        //	textElement.Text = text;
        //	textElement.Symbol = pTextSymbol;
        //	IPoint tmp = new PointClass ( );
        //	if ( horizontal )
        //		tmp.PutCoords ( pnt.X, pnt.Y - 0.4 );
        //	else
        //		tmp.PutCoords ( pnt.X + side * ( 0.3 + 0.1 * ( text.Length - 2 ) ), pnt.Y );

        //    var element = (IElement) textElement;
        //          element.Geometry = tmp;
        //	if ( grpElement == null )
        //		grpElement = _textGrp;
        //	grpElement.AddElement ( element );
        //}
    }
}
