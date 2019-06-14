using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using Accent.MapCore;
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
    public class AbstractChartElement : ICloneable, IDisposable
    {
        #region fields

            protected IDisplayFeedback _feedback;
            private bool disposed = false;

        #endregion;


        [XmlElement]
        [Browsable(false)]
        [XmlIgnore]
        private object _graphicsElement;
        public object GraphicsElement
        {
            get { return _graphicsElement; }
            set { _graphicsElement = value; }
        }

        [XmlElement]
        [Browsable(false)]
        //[XmlIgnore]
        private Guid _id;
        public Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }

        [XmlElement]
        [Browsable(false)]
        //[XmlIgnore]
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [XmlElement]
        [Browsable(false)]
        //[XmlIgnore]
        private string _relatedFeature;
        public string RelatedFeature
        {
            get { return _relatedFeature; }
            set { _relatedFeature = value; }
        }

        [XmlElement]
        [Browsable(false)]
        [XmlIgnore]
        private object _tag;
        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        public AbstractChartElement()
        {
            this.RelatedFeature = "";
            this.Id = Guid.NewGuid();
            this.GraphicsElement = null;
            this.Tag = null;
            this.Name = "AcntElement:"+this.ToString() + ":" + this.Id.ToString();
        }

        ~AbstractChartElement()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Free other state (managed objects).
                }
                // Free your own state (unmanaged objects).
                // Set large fields to null.
                disposed = true;
            }
        }

        virtual public object ConvertToIElement()
        {
            return null;
        }

        virtual public object ChartElementChanged()
        {
            throw new System.NotImplementedException();
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public override string ToString()
        {
            return this.GetType().Name;
        }

        virtual public void _OnMouseDown(object sender, ESRI.ArcGIS.Controls.IPageLayoutControlEvents_OnMouseDownEvent e, IScreenDisplay Dispaly, ISymbol Symbol, double ReferenceScale)
        {
        }

        virtual public void _OnMouseMove(object sender, ESRI.ArcGIS.Controls.IPageLayoutControlEvents_OnMouseMoveEvent e)
        {
        }

        virtual public void _OnMouseUp(object sender, ESRI.ArcGIS.Controls.IPageLayoutControlEvents_OnMouseUpEvent e)
        {
        }

    }


  
}
