using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using ANCOR.MapCore;
using System.Drawing.Design;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
//using Accent.MapCore;

namespace ANCOR.MapElements
{

      [XmlType]
    [Serializable()]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class GraphicsChartElement_NorthArrow : GraphicsChartElement
        {
            //private AcntPoint _position;
            private AncorPoint _textShift;
            private double _angle;
            private hemiSphere _hemiSphere;
            private AncorColor _clr;
            private AncorFont _fnt;
            private int _arrowwLen;


            public GraphicsChartElement_NorthArrow()
            {
            }

            public GraphicsChartElement_NorthArrow(string _text1)
            {
                //_position = new AcntPoint(2,26);
                _textShift = new AncorPoint(-0.38,0.58);
                _angle = 81;
                _hemiSphere = hemiSphere.EasternHemisphere;
                _clr = new AncorColor(0,0,0);
                _fnt = new AncorFont { Bold = false, FontColor = new AncorColor(0, 0, 0), Italic = false, Name = "Arial Narrow", Size = 6, UnderLine = false };
                _arrowwLen = 130;


                /////////////////////////////////////////////////////////////////////////////////////////////
                this.TextContents = new List<List<AncorChartElementWord>>();

                List<AncorChartElementWord> txtLine = new List<AncorChartElementWord>(); // создаем строку
                AncorChartElementWord wrd = new AncorChartElementWord(_text1, _fnt);//создаем слово
                wrd.Font.Bold = true;
                wrd.StartSymbol = new AncorSymbol("");
                wrd.EndSymbol = new AncorSymbol("");
                //wrd.Morse = true;
                txtLine.Add(wrd);

               

                //////////////////////////////////////////////////////////////////////////////////////////

            }

            public GraphicsChartElement_NorthArrow(string _text, hemiSphere _HemiSphere, string _text2=null)
            {
                if (_HemiSphere == MapCore.hemiSphere.EasternHemisphere)
                    _textShift = new AncorPoint(-0.36, 0.58);
                else
                    _textShift = new AncorPoint(0.16, 0.48);

                _angle = 81;
                _hemiSphere = _HemiSphere;
                _clr = new AncorColor(0, 0, 0);
                _fnt = new AncorFont { Bold = false, FontColor = new AncorColor(0, 0, 0), Italic = false, Name = "Arial Narrow", Size = 6, UnderLine = false };
                _arrowwLen = 130;


                /////////////////////////////////////////////////////////////////////////////////////////////
                this.TextContents = new List<List<AncorChartElementWord>>();

                List<AncorChartElementWord> txtLine = new List<AncorChartElementWord>(); // создаем строку
                AncorChartElementWord wrd = new AncorChartElementWord("Annual Rate", _fnt);//создаем слово
                wrd.Font.Bold = true;
                wrd.StartSymbol = new AncorSymbol("");
                wrd.EndSymbol = new AncorSymbol("");
                //wrd.Morse = true;
                txtLine.Add(wrd);
                this.TextContents.Add(txtLine);  // добавим его в строку

                txtLine = new List<AncorChartElementWord>(); // создаем строку
                wrd = new AncorChartElementWord("of Change", _fnt);//создаем слово
                wrd.Font.Bold = true;
                wrd.StartSymbol = new AncorSymbol("");
                wrd.EndSymbol = new AncorSymbol("");
                //wrd.Morse = true;
                txtLine.Add(wrd);
                this.TextContents.Add(txtLine); 


                this.SubTextContents = new AncorChartElementWord(_text, _fnt);

                    wrd = new AncorChartElementWord(_text2, _fnt);//создаем слово
                    wrd.Font.Bold = true;
                    wrd.StartSymbol = new AncorSymbol("");
                    wrd.EndSymbol = new AncorSymbol("");

                  

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
           // [Browsable(false)]
            public virtual AncorPoint TextShift
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
            private List<List<AncorChartElementWord>> _textContents;
            [Editor(typeof(MyTextContextEditor), typeof(UITypeEditor))]
            public List<List<AncorChartElementWord>> TextContents
            {
                get { return _textContents; }
                set { _textContents = value; }
            }

            [XmlElement]
            private AncorChartElementWord _subTextContents;
            public AncorChartElementWord SubTextContents
            {
                get { return _subTextContents; }
                set { _subTextContents = value; }
            }


            [XmlElement]
            [Editor(typeof(SigmaColorEdotor), typeof(UITypeEditor))]
            public virtual AncorColor Color
            {
                get { return _clr; }
                set { _clr = value; }
            }


            [XmlElement]
            [Browsable(false)]
            public virtual AncorFont Font
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
                        value = Convert.ToInt32("F06C", 16);
                        //_textShift = new AncorPoint(-0.36, 0.58);
                    }
                    else
                    {
                        value = Convert.ToInt32("F06B", 16);
                        //_textShift = new AncorPoint(0.16, 0.48);
                    }
                        

                    pTextElementArrow.Text = Char.ConvertFromUtf32(value);

                    TextSymbolClass pTextSymbolArrow = new TextSymbolClass();

                    // параметры шрифта
                    stdole.IFontDisp pFontDisp = new stdole.StdFont() as stdole.IFontDisp;
                    pFontDisp.Name = "AeroSigma";
                    pFontDisp.Size = NArrow.ArrowwLen;
                    pTextSymbolArrow.Font = pFontDisp;

                    pTextSymbolArrow.Color = NArrow.Color.GetColor();
                    pTextSymbolArrow.Angle = NArrow.Angle;

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
                    pTextElementLegend.Text = this.SubTextContents.ToString();


                    TextSymbolClass pTextSymbolLegend = new TextSymbolClass();

                    // форматирование текста
                HelperClass.FormatText(ref pTextSymbolLegend, textPosition.Normal, -2, textCase.Normal, horizontalAlignment.Center, verticalAlignment.Top, 0, 0, 50);
                    HelperClass.CreateFont(ref pTextSymbolLegend, this.Font);


                    if (NArrow.hemiSphere == hemiSphere.EasternHemisphere)
                    {
                        pTextSymbolLegend.Angle = NArrow.Angle +90;

                    }
                    else
                    {
                        pTextSymbolLegend.Angle = Math.Abs(360 - NArrow.Angle) +90;
                    }

                    pTextElementLegend.Symbol = pTextSymbolLegend;


                    IElement pElTextLegend = (IElement)pTextElementLegend;
                    IPoint pointLegend = new PointClass();
                    pointLegend.PutCoords(0, 0);
                    pElTextLegend.Geometry = pointLegend;
                    //return pElTextLegend;

                    #endregion;

                    #region формирование подписи к стрелке

                    IElement pElTextLegend2 = null;

                    if (this.SubTextContents != null)
                    {

                        ITextElement pTextElementLegend2 = new TextElementClass();
                        pTextElementLegend2.Text = HelperClass.TextConstructor(this.TextContents);
                   

                        TextSymbolClass pTextSymbolLegend2 = new TextSymbolClass();
                   // pTextSymbolLegend2.Angle = NArrow.Angle;

                        // форматирование текста
                    HelperClass.FormatText(ref pTextSymbolLegend2, textPosition.Normal, 0, textCase.Normal, horizontalAlignment.Left, verticalAlignment.Top, 0, 0, 100);
                        HelperClass.CreateFont(ref pTextSymbolLegend2, this.Font);


                        pTextElementLegend2.Symbol = pTextSymbolLegend2;


                        pElTextLegend2 = (IElement)pTextElementLegend2;
                        pointLegend = new PointClass();
                        pointLegend.PutCoords(NArrow.Position.X, NArrow.Position.Y);
                        pElTextLegend2.Geometry = pointLegend;
                    }

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
                    GrpEl.AddElement(pElTextLegend2);
                    IElementProperties prp = (IElementProperties)GrpEl;
                    prp.Name = "AccentNothArrow";

                    IElementProperties3 docElementProperties = GrpEl as IElementProperties3;
                    docElementProperties.AnchorPoint = esriAnchorPointEnum.esriCenterPoint;

                    return GrpEl as IGroupElement3;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }


            public override IDisplayFeedback GetFeedback()
            {
                GraphicsChartElement_NorthArrow symbol = (GraphicsChartElement_NorthArrow)this.Clone();
                IElement iEl = (IElement)symbol.ConvertToIElement();
                ITextSymbol txtS = null;

                if (iEl is IGroupElement)
                {
                    txtS = ((ITextElement)((IGroupElement)iEl).get_Element(1)).Symbol;
                }
                else
                {
                    txtS = ((ITextElement)iEl).Symbol;
                }

                IDisplayFeedback _feedback = new NewTextFeedbackClass();
                NewTextFeedbackClass mvPtFeed = (NewTextFeedbackClass)_feedback;
                mvPtFeed.Symbol = (ISymbol)txtS;

                return mvPtFeed;

            }

            public override void StartFeedback(IDisplayFeedback feedBack, IPoint _position, double scale, IGeometry LinkedGeometry)
            {
                ((NewTextFeedbackClass)feedBack).Start(_position, scale);
            }

            public override IGeometry StopFeedback(IDisplayFeedback feedBack, int X, int Y, IGeometry LinkedGeometry, int Shift)
            {
                IPoint res = new PointClass();
                res.PutCoords(X, Y);

                res.PutCoords(X, Y);
                

                ((NewTextFeedbackClass)feedBack).Stop();

                return res;
            }

            public override void MoveFeedback(IDisplayFeedback feedBack, IPoint _position, IGeometry LinkedGeometry, int Shift)
            {
                feedBack.MoveTo(_position);


            }
 
        }


}
