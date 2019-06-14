using ARENA;
using ArenaStatic;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SigmaChart
{
    public static class AerodromeCharCreator
    {
        public static void CreateAerodromeTypeChart(IApplication m_application, IHookHelper m_hookHelper, int chartType)
        {
            if (!((IMxDocument)m_application.Document).CurrentContentsView.Name.StartsWith("TOCLayerFilter"))
            {
                ArenaStaticProc.BringToFrontToc((IMxDocument)m_application.Document, "TOCLayerFilter");
                Application.DoEvents();

                if (DataCash.ProjectEnvironment != null && DataCash.ProjectEnvironment.Data != null && DataCash.ProjectEnvironment.Data.CurProjectName == null)
                {
                    ArenaStaticProc.AutoSaveArenaProject(m_application);
                    Application.DoEvents();
                }

            }

            CultureInfo oldCI = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            AerodromeChartClass _Chart = new AerodromeChartClass { SigmaHookHelper = m_hookHelper, SigmaApplication = m_application, ChartType = chartType };
            _Chart.CreateChart();
            m_application.SaveDocument();
            Thread.CurrentThread.CurrentCulture = oldCI;
        }
    }
}
