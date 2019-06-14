////using Aerodrome.Context;
//using Aerodrome.Features;
////using Aerodrome.Features.Context;
//using ESRI.ArcGIS.Carto;
//using ESRI.ArcGIS.Framework;
//using ESRI.ArcGIS.Geodatabase;
//using ESRI.ArcGIS.Geometry;
//using Framework.Stasy.Context;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Aerodrome.DataCash
//{
//    public class AerodromeEnvironment
//    {
//        #region UI properties 

//        private IApplication m_application;
//        public IApplication mxApplication
//        {
//            get { return m_application; }
//            set { m_application = value; }
//        }

//        private IMap _mapControl;
//        public IMap pMap
//        {
//            get { return _mapControl; }
//            set
//            {
//                _mapControl = value;
//            }
//        }
//        #endregion

//        private Dictionary<Type, ITable> _tableDictionary;
//        public Dictionary<Type, ITable> TableDictionary
//        {
//            get { return _tableDictionary ?? (_tableDictionary = new Dictionary<Type, ITable>()); }
//            set { _tableDictionary = value; }
//        }


//        public  ApplicationContext Context;

//        public ISpatialReference SpatialReference { get; set; }
//        public string MapDocumentName { get; set; }
//        public string CurProjectName { get; set; }

//    }
//}
