using ChartCompare;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Geodatabase;
using PDM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigmaChart
{
    public class AbstaractSigmaChart
    {

        private IHookHelper _sigmaHookHelper = null;

        public IHookHelper SigmaHookHelper
        {
            get { return _sigmaHookHelper; }
            set { _sigmaHookHelper = value; }
        }
        private IApplication _sigmaApplication;

        public IApplication SigmaApplication
        {
            get { return _sigmaApplication; }
            set { _sigmaApplication = value; }
        }

        private DateTime _effectiveDate;

        public DateTime EffectiveDate
        {
            get { return _effectiveDate; }
            set { _effectiveDate = value; }
        }


        public string airacCircle { get; set; }
        public string ADHP { get; set; }
        public string organization { get; set; }
        public DateTime efectiveDate { get; set; }
        public List<string> RunwayDirectionsList { get; set; }
        public int ChartType { get; set; }

        public AbstaractSigmaChart()
        {
        }

        virtual public void CreateChart()
        {
           
        }

        virtual public void UpdateChart(List<DetailedItem> UpdateList, List<PDMObject> oldPdmList, List<PDMObject> newPdmList)
        {

        }


        virtual public IWorkspaceEdit InitEnvironment_Workspace(string pathToTemplateFileXML, IMap ActiveMap)
        {
            return null;
        }
    }
}
