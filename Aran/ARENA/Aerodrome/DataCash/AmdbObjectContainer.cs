//using Aerodrome.DataType;
//using Aerodrome.Features;
//using ESRI.ArcGIS.Geometry;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Xml.Serialization;

//namespace Aerodrome.DataCash
//{   
//    public class AmdbObjectContainer
//    {

//        private DateTime _date;
//        public DateTime Date
//        {
//            get { return _date; }
//            set { _date = value; }
//        }

//        private List<AM_AbstractFeature> _object_list;
 
//        public List<AM_AbstractFeature> ObjectList
//        {
//            get { return _object_list; }
//            set { _object_list = value; }
//        }

//        private string _projectType;
//        public string ProjectType
//        {
//            get { return _projectType; }
//            set { _projectType = value; }
//        }

//        public string VersionInfo { get; set; }

//        public AmdbObjectContainer()
//        {
//            this.Date = DateTime.Now;
//            this.ObjectList = new List<AM_AbstractFeature>();
//            this.ProjectType = "Aerodrome";
//            this.VersionInfo = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
//        }

//        public AmdbObjectContainer(List<AM_AbstractFeature> objects, string prjType)
//        {
//            this.Date = DateTime.Now;
//            this.ObjectList = new List<AM_AbstractFeature>();
//            this.ObjectList.AddRange(objects.GetRange(0, objects.Count));
//            this.ProjectType = prjType;
//            this.VersionInfo = this.VersionInfo == null || this.VersionInfo.Length <= 0 ? System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() : this.VersionInfo;
//        }


//    }
//}
