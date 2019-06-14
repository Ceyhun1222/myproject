using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using ESRI.ArcGIS.Display;
using System.Drawing.Design;

namespace ANCOR.MapCore
{

    [XmlType]
    [Serializable()]
    [TypeConverter(typeof(PropertySorter))]
    public class AncorColor : AbstractChartClass
    {
        [Browsable(false)]
        public int AncorColor_CMYK { get; set; }

        [Browsable(false)]
        public bool AncorColor_NullColor { get; set; }

        [Browsable(false)]
        public int AncorColor_RGB { get; set; }

        [Browsable(false)]
        public byte AncorColor_Transparency { get; set; }

        [Browsable(false)]
        public bool AncorColor_UseWindowsDithering { get; set; }

        public AncorColor()
        {

        }

        public AncorColor(int _R, int _G, int _B)
        {
            RgbColor _clr = new RgbColorClass();
            _clr.Red = _R;
            _clr.Green = _G;
            _clr.Blue = _B;

            this.AncorColor_CMYK = _clr.CMYK;
            this.AncorColor_NullColor = _clr.NullColor;
            this.AncorColor_RGB = _clr.RGB;
            this.AncorColor_Transparency = _clr.Transparency;
            this.AncorColor_UseWindowsDithering = _clr.UseWindowsDithering;

        }

        public AncorColor(int _C, int _M, int _Y, int _K)
        {
            CmykColor _clr = new CmykColorClass();
            _clr.Cyan = _C;
            _clr.Magenta = _M;
            _clr.Yellow = _Y;
            _clr.Black = _K;

            this.AncorColor_CMYK = _clr.CMYK;
            this.AncorColor_NullColor = _clr.NullColor;
            this.AncorColor_RGB = _clr.RGB;
            this.AncorColor_Transparency = _clr.Transparency;
            this.AncorColor_UseWindowsDithering = _clr.UseWindowsDithering;

        }
   
        public IColor GetColor(bool CMYK = false)
        {
            if (CMYK) return GetCmyk();
            else return GetRGB();
            
        }

        [PropertyOrder(10)]
        [Browsable(false)]
        public int Red { get { return (GetColor() as RgbColor).Red; } }

        [PropertyOrder(20)]
        [Browsable(false)]
        public int Green { get { return (GetColor() as RgbColor).Green; } }

        [PropertyOrder(30)]
        [Browsable(false)]
        public int Blue { get { return (GetColor() as RgbColor).Blue; } }

        [PropertyOrder(40)]
        [Browsable(false)]
        public int Cyan { get { return (GetColor(true) as CmykColor).Cyan; } }

        [PropertyOrder(50)]
        [Browsable(false)]
        public int Magenta { get { return (GetColor(true) as CmykColor).Magenta; } }

        [PropertyOrder(60)]
        [Browsable(false)]
        public int Yellow { get { return (GetColor(true) as CmykColor).Yellow; } }

        [PropertyOrder(70)]
        [Browsable(false)]
        public int Black { get { return (GetColor(true) as CmykColor).Black; } }

        [Browsable(false)]
        public string RGB { get { return GetRGBCode(); } }
        [Browsable(false)]
        public string CMYK { get { return GetCMYKCode(); } }


        private IColor GetRGB()
        {
            IColor _Clr = new RgbColorClass();
            _Clr.RGB = this.AncorColor_RGB;
            _Clr.CMYK = this.AncorColor_CMYK;
            _Clr.NullColor = this.AncorColor_NullColor;
            _Clr.Transparency = this.AncorColor_Transparency;

            return _Clr;
        }

        private string GetRGBCode()
        {
            return this.AncorColor_RGB + " (" + this.Red.ToString() + "," + this.Green.ToString() + "," + this.Blue.ToString() + ")";
            
        }
        private string GetCMYKCode()
        {
            return this.AncorColor_CMYK + " (" + this.Cyan.ToString() + "," + this.Magenta.ToString() + "," + this.Yellow.ToString() + "," + this.Black.ToString() + ")";

        }

        private IColor GetCmyk()
        {
            IColor _Clr = new CmykColorClass();
            _Clr.RGB = this.AncorColor_RGB;
            _Clr.CMYK = this.AncorColor_CMYK;
            _Clr.NullColor = this.AncorColor_NullColor;
            _Clr.Transparency = this.AncorColor_Transparency;

            return _Clr;
        }

        public override object Clone()
        {
            AncorColor o = (AncorColor)base.Clone();
            return o;
        }

        public override string ToString()
        {
            return "Color palette";
        }
    }



    [TypeConverter(typeof(PropertySorter))]
    public class AncorColorselector
    {

       
        public AncorColorselector()
        {

        }

        AncorColor _SelectedColor;
        [Editor(typeof(SigmaColorEdotor), typeof(UITypeEditor))]
        [DisplayName("Selected Color")]
        [Category("Decoration")]
        [PropertyOrder(1)]
        public AncorColor SelectedColor { get => _SelectedColor; set => _SelectedColor = value; }

        Scroll _scrolList;
        [DisplayName("Apply color to")]
        [Category("Decoration")]
        [PropertyOrder(2)]
        public Scroll ScrolList { get => _scrolList; set => _scrolList = value; }


        public override string ToString()
        {
            return "selected Color palette";
        }
    }

}
