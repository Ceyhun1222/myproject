using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using Accent.MapCore;
using System.Drawing.Design;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
//using Accent.MapCore;

namespace Accent.MapElements
{

      [XmlType]
    [Serializable()]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class GraphicsChartElement_NorthArrow : GraphicsChartElement
        {
            //private AcntPoint _position;
            private AcntPoint _textShift;
            private double _angle;
            private hemiSphere _hemiSphere;
            private AcntColor _clr;
            private AcntFont _fnt;
            private int _arrowwLen;


            public GraphicsChartElement_NorthArrow()
            {
                //_position = new AcntPoint(2,26);
                _textShift = new AcntPoint(-0.1,1);
                _angle = 280;
                _hemiSphere = hemiSphere.EasternHemisphere;
                _clr = new AcntColor(0,0,0);
                _fnt = new AcntFont();
                _arrowwLen = 90;


                /////////////////////////////////////////////////////////////////////////////////////////////
                this.TextContents = new List<List<AcntChartElementWord>>();

                List<AcntChartElementWord> txtLine = new List<AcntChartElementWord>(); // создаем строку
                AcntChartElementWord wrd = new AcntChartElementWord();//создаем слово
                wrd.TextValue = "Value";
                wrd.Font.Bold = true;
                wrd.StartSymbol = new AcntSymbol("+");
                wrd.EndSymbol = new AcntSymbol("+");
                //wrd.Morse = true;
                txtLine.Add(wrd);
                this.TextContents.Add(txtLine);  // добавим его в строку

                //////////////////////////////////////////////////////////////////////////////////////////

            }


            [XmlElement]
            public virtual hemiSphere hemiSphere
            {
                get { return _hemiSphere; }
                set { _hemiSphere = value; }
            }


            //[XmlElement]
            //[ReadOnly(false)]
            //[Browsable(false)]
            //public virtual AcntPoint Position
            //{
            //    get { return _position; }
            //    set { _position = value; }
            //}


            [XmlElement]
            public virtual AcntPoint TextShift
            {
                get { return  _textShift; }
                set { _textShift = value; }
            }


            [XmlElement]
            [Browsable(false)]
            public virtual double Angle
            {
                get { return _angle; }
                set { _angle = value; }
            }



            [XmlElement]
            private List<List<AcntChartElementWord>> _textContents;
            [Editor(typeof(MyTextContextEditor), typeof(UITypeEditor))]
            public List<List<AcntChartElementWord>> TextContents
            {
                get { return _textContents; }
                set { _textContents = value; }
            }


            [XmlElement]
            [Editor(typeof(MyColorEdotor), typeof(UITypeEditor))]
            public virtual AcntColor Color
            {
                get { return _clr; }
                set { _clr = value; }
            }


            [XmlElement]
            [Browsable(false)]
            public virtual AcntFont Font
            {
                get { return _fnt; }
                set { _fnt = value; }
            }


            [XmlElement]
            public virtual int ArrowwLen
            {
                get { return _arrowwLen; }
                set { _arrowwLen = value; }
            }



            public override object ConvertToIElement()
            {
                try
                {
                    #region формирование стрелки

                    GraphicsChartElement_NorthArrow NArrow = this;

                    ITextElement pTextElementArrow = new TextElementClass();
                    int value = 0;
                    if (NArrow.hemiSphere == hemiSphere.EasternHemisphere)
                    {
                        //value = Convert.ToInt32("F06B", 16);
                        value = Convert.ToInt32("F06C", 16);
                    }
                    else
                    {
                        //value = Convert.ToInt32("F06C", 16);
                        value = Convert.ToInt32("F06B", 16);
                    }

                    pTextElementArrow.Text = Char.ConvertFromUtf32(value);

                    TextSymbolClass pTextSymbolArrow = new TextSymbolClass();

                    // параметры шрифта
                    stdole.IFontDisp pFontDisp = new stdole.StdFont() as stdole.IFontDisp;
                    pFontDisp.Name = "RISK Aero";
                    pFontDisp.Size = NArrow.ArrowwLen;
                    pTextSymbolArrow.Font = pFontDisp;


                    IRgbColor myColor = new RgbColor();
                    myColor.Red = NArrow.Color.Red;
                    myColor.Blue = NArrow.Color.Green;
                    myColor.Green = NArrow.Color.Blue;

                    pTextSymbolArrow.Color = myColor;


                    pTextSymbolArrow.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
                    //pTextSymbolArrow.Angle = NArrow.Angle;
                    pTextElementArrow.Symbol = pTextSymbolArrow;


                    IElement pElArrow = (IElement)pTextElementArrow;

                    IPoint pointArrow = new PointClass();
                    pointArrow.PutCoords(0, 0);

                    pElArrow.Geometry = pointArrow;

                    #endregion;


                    #region формирование подписи к стрелке

                    ITextElement pTextElementLegend = new TextElementClass();
                    pTextElementLegend.Text = HelperClass.TextConstructor(this.TextContents);


                    TextSymbolClass pTextSymbolLegend = new TextSymbolClass();

                    // параметры шрифта
                    stdole.IFontDisp pFontDisp1 = new stdole.StdFont() as stdole.IFontDisp;
                    pFontDisp1.Name = NArrow.Font.Name;
                    pFontDisp1.Bold = NArrow.Font.Bold;
                    pFontDisp1.Italic = NArrow.Font.Italic;
                    pFontDisp1.Underline = NArrow.Font.UnderLine;
                    pFontDisp1.Size = (decimal)NArrow.Font.Size;
                    pTextSymbolLegend.Font = pFontDisp1;

                    IRgbColor myColor1 = new RgbColor();
                    myColor1.Red = NArrow.Color.Red;
                    myColor1.Blue = NArrow.Color.Green;
                    myColor1.Green = NArrow.Color.Blue;

                    pTextSymbolLegend.Color = myColor1;


                    pTextSymbolLegend.HorizontalAlignment = esriTextHorizontalAlignment.esriTHARight;
                    pTextSymbolLegend.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;


                    if (NArrow.hemiSphere == hemiSphere.EasternHemisphere)
                    {
                        pTextSymbolLegend.Angle = NArrow.Angle;
                        
                    }
                    else
                    {
                        pTextSymbolLegend.Angle = Math.Abs(360 - NArrow.Angle);
                    }






                    pTextElementLegend.Symbol = pTextSymbolLegend;


                    IElement pElTextLegend = (IElement)pTextElementLegend;
                    IPoint pointLegend = new PointClass();
                    pointLegend.PutCoords(0, 0);
                    pElTextLegend.Geometry = pointLegend;
                    //return pElTextLegend;

                    #endregion;


                    ITransform2D trans = (ITransform2D)pElArrow;
                    trans.Move(NArrow.Position.X, NArrow.Position.Y);

                    trans = (ITransform2D)pElTextLegend;

                    if (NArrow.hemiSphere == hemiSphere.EasternHemisphere)
                    {
                        trans.Move(NArrow.Position.X + NArrow.TextShift.X + 0.7, NArrow.Position.Y + NArrow.TextShift.Y);
                    }
                    else
                    {
                        trans.Move(NArrow.Position.X + NArrow.TextShift.X, NArrow.Position.Y + NArrow.TextShift.Y);
                    }

                    IGroupElement3 GrpEl = new GroupElementClass();

                    GrpEl.AddElement(pElTextLegend);
                    GrpEl.AddElement(pElArrow);
                    IElementProperties prp = (IElementProperties)GrpEl;
                    prp.Name = "AccentNothArrow";

                    return GrpEl as IGroupElement3;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }

        }


}
