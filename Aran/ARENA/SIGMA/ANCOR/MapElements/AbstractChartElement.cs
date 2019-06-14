using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using ANCOR.MapCore;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
using System.Collections;
using System.IO;

namespace ANCOR.MapElements
{

    [TypeConverter(typeof(PropertySorter))]
    public class AbstractChartElement : ICloneable, IDisposable
    {
        #region fields

            //protected IDisplayFeedback _feedback;
            private bool disposed = false;

        #endregion;


        //[XmlElement]
        private Guid _id;
        [Category("ID")]
        //[Browsable(false)]
        [ReadOnly(true)]
        [SkipAttribute(true)]
        [PropertyOrder(1)]
        public Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }

        //[XmlElement]

        private string _name;
        [Browsable(false)]
        [SkipAttribute(true)]
        [ReadOnly(true)]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _relatedFeature;
        //[XmlElement]
        [Browsable(false)]
        [SkipAttribute(true)]
        [ReadOnly(true)]
        public string RelatedFeature
        {
            get { return _relatedFeature; }
            set { _relatedFeature = value; }
        }

        private bool _placed;
        [Browsable(false)]
        [SkipAttribute(true)]
        public bool Placed
        {
            get { return _placed; }
            set { _placed = value; }
        }

        private bool _reflectionHidden;
        [Browsable(false)]
        [SkipAttribute(true)]
        public bool ReflectionHidden
        {
            get { return _reflectionHidden; }
            set { _reflectionHidden = value; }
        }

        private string _linckedGeoId;
        //[Browsable(false)]
        [ReadOnly(true)]
        [SkipAttribute(true)]
        [DisplayName("PDM FeatureID")]
        public string LinckedGeoId
        {
            get { return _linckedGeoId; }
            set { _linckedGeoId = value; }
        }

        private string _tag;
        [Browsable(false)]
        [SkipAttribute(true)]
        public string Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        private string _logTxt;
        [Browsable(false)]
        [SkipAttribute(true)]
        public string LogTxt
        {
            get { return _logTxt; }
            set { _logTxt = value; }
        }
        public AbstractChartElement()
        {
            this.RelatedFeature = "";
            this.Id = Guid.NewGuid();
            this.Name = "AcntElement:"+this.ToString() + ":" + this.Id.ToString();
            Placed = true;
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

        virtual public object Clone()
        {
            object  o =  this.MemberwiseClone();
            ((AbstractChartElement)o).Id = Guid.NewGuid();
            return o;
        }

        public override string ToString()
        {
            return this.GetType().Name;
        }

        virtual public IDisplayFeedback GetFeedback()
        {
            return null;
        }

        virtual public void StartFeedback(IDisplayFeedback feedBack, IPoint _position, double scale, IGeometry LinkedGeometry)
        {
            
        }

        virtual public IGeometry StopFeedback(IDisplayFeedback feedBack, int X, int  Y, IGeometry LinkedGeometry, int Shift)
        {
            return null;
        }

        virtual public void MoveFeedback(IDisplayFeedback feedBack, IPoint _position, IGeometry LinkedGeometry, int Shift)
        {

        }


    }


  
}
