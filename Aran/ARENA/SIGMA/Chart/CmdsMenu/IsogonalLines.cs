using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.IO.Compression;
using System.IO;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using EsriWorkEnvironment;
using AranSupport;
using System.Windows.Forms;

namespace SigmaChart.CmdsMenu
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout
    /// </summary>
    [Guid("1787a598-0e20-4fbd-9281-4897145dcd6b")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("SigmaChart.IsogonalLines")]
    public sealed class IsogonalLines : BaseCommand
    {
        #region COM Registration Function(s)
        [ComRegisterFunction()]
        [ComVisible(false)]
        static void RegisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryRegistration(registerType);

            //
            // TODO: Add any COM registration code here
            //
        }

        [ComUnregisterFunction()]
        [ComVisible(false)]
        static void UnregisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryUnregistration(registerType);

            //
            // TODO: Add any COM unregistration code here
            //
        }

        #region ArcGIS Component Category Registrar generated code
        /// <summary>
        /// Required method for ArcGIS Component Category registration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryRegistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommands.Register(regKey);
            ControlsCommands.Register(regKey);
        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommands.Unregister(regKey);
            ControlsCommands.Unregister(regKey);
        }

        #endregion
        #endregion

        private IHookHelper m_hookHelper = null;
        public IsogonalLines()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "SigmaChart"; //localizable text
            base.m_caption = "create Sigma Chart Isogonal Lines";  //localizable text 
            base.m_message = "create Sigma Chart Isogonal Lines";  //localizable text
            base.m_toolTip = "Sigma Chart";  //localizable text
            base.m_name = "Sigma Chart";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")
            try
            {
                //
                // TODO: change resource name if necessary 
                //
                base.m_bitmap = global::SigmaChart.Properties.Resources.SpatialAnalystContourTool16;
                //base.m_cursor = new System.Windows.Forms.Cursor(GetType(), GetType().Name + ".cur");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
        }

        #region Overridden Class Methods

        /// <summary>
        /// Occurs when this command is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            if (hook == null)
                return;

            try
            {
                m_hookHelper = new HookHelperClass();
                m_hookHelper.Hook = hook;
                if (m_hookHelper.ActiveView == null)
                    m_hookHelper = null;
            }
            catch
            {
                m_hookHelper = null;
            }

            if (m_hookHelper == null)
                base.m_enabled = false;
            else
                base.m_enabled = true;

            // TODO:  Add other initialization code
        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            // TODO: Add IsogonalLines.OnClick implementation

            //string COFFile = Path.Combine( ArenaStatic.ArenaStaticProc.GetPathToTemplateFile(), "WMM.COF");
            //string Gzip_COFFile = Path.Combine(ArenaStatic.ArenaStaticProc.GetPathToTemplateFile(), "WMM.CNF");

            //ArenaStatic.ArenaStaticProc.CompressFile(Gzip_COFFile,COFFile);

            //return;

            #region Подготовительная работа

            
            //найти MagIsogonalLines
            if (SigmaDataCash.environmentWorkspaceEdit == null) return;
            ITable isonalLinesTable = EsriWorkEnvironment.EsriUtils.getTableByname((IFeatureWorkspace)SigmaDataCash.environmentWorkspaceEdit, "MagIsogonalLines");

            if (isonalLinesTable == null) return;

            ILayer isonalLines = EsriWorkEnvironment.EsriUtils.getLayerByName(m_hookHelper.FocusMap, "MagIsogonalLines");

            if (isonalLines == null)
            {
                ILayer nlayer = (ILayer)(new FeatureLayer());
                nlayer.Name = "MagIsogonalLines";
                IFeatureLayer newlayer = (IFeatureLayer)nlayer;
                newlayer.FeatureClass = (IFeatureClass)isonalLinesTable;
                newlayer.Name = "MagIsogonalLines";

                m_hookHelper.FocusMap.AddLayer(newlayer);
            }


            //очистить таблицу
            IFeatureClass isonalLinesIFeatureClass = (IFeatureClass)isonalLinesTable;

            string qry = isonalLinesIFeatureClass.OIDFieldName + " >= 0";
            IQueryFilter featFilter = new QueryFilterClass();

            featFilter.WhereClause = qry;

            IFeatureCursor featCur = isonalLinesIFeatureClass.Search(featFilter, false);

            IFeature _Feature = null;
            while ((_Feature = featCur.NextFeature()) != null)
            {
                _Feature.Delete();
            }

            Marshal.ReleaseComObject(featCur);
            #endregion

            MagIsogonalSettingsForm magFrm = new MagIsogonalSettingsForm();
            if (magFrm.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            //найти файл с коэффициентами


            FileInfo fileToDecompress = new FileInfo(System.IO.Path.Combine(ArenaStatic.ArenaStaticProc.GetPathToTemplateFile(), "WMM.CNF"));
            string tmp = System.IO.Path.GetTempFileName();
            ArenaStatic.ArenaStaticProc.Decompress(fileToDecompress, tmp);



            //создать изолинии
            Isogons.IContour MagLinesList = new Isogons.MagContour(tmp);


            MagLinesList.Altitude = (double)magFrm.numericUpDown2.Value;
            MagLinesList.Time = magFrm.dateTimePicker1.Value;
            double magStep = (double)magFrm.numericUpDown1.Value;
            magFrm.Dispose();
            
            //aa.wmm(45, 45, aa.Altitude, aa.Time);

            //инициализация исходного Extent'a

           double stLon =   (m_hookHelper.FocusMap as IActiveView).Extent.UpperLeft.X;
           double stLat = (m_hookHelper.FocusMap as IActiveView).Extent.UpperLeft.Y;

           double endLon = (m_hookHelper.FocusMap as IActiveView).Extent.LowerRight.X;
           double endLat = (m_hookHelper.FocusMap as IActiveView).Extent.LowerRight.Y;


           IPoint UpperLeft = new PointClass();
           UpperLeft.PutCoords(stLon, stLat);

           IPoint LowerRight = new PointClass();
           LowerRight.PutCoords(endLon, endLat);

           ILayer _Layer = EsriUtils.getLayerByName(m_hookHelper.FocusMap, "AirportHeliport");
            if (_Layer == null)
                _Layer = EsriUtils.getLayerByName(m_hookHelper.FocusMap, "AirportHeliportCartography"); 
            if (_Layer == null)
                _Layer = EsriUtils.getLayerByName(m_hookHelper.FocusMap, "AirportCartography");
            if (_Layer == null) return;
            var fc = ((IFeatureLayer)_Layer).FeatureClass;


           IPoint UpperLeft_geoPoint = (IPoint)EsriUtils.ToGeo(UpperLeft, m_hookHelper.FocusMap, (fc as IGeoDataset).SpatialReference);
           IPoint LowerRight_geoPoint = (IPoint)EsriUtils.ToGeo(LowerRight, m_hookHelper.FocusMap, (fc as IGeoDataset).SpatialReference);

           stLon = UpperLeft_geoPoint.X <= LowerRight_geoPoint.X ? UpperLeft_geoPoint.X : LowerRight_geoPoint.X;
           stLat = UpperLeft_geoPoint.Y <= LowerRight_geoPoint.Y ? UpperLeft_geoPoint.Y : LowerRight_geoPoint.Y;

           endLon = LowerRight_geoPoint.X >= UpperLeft_geoPoint.X ? LowerRight_geoPoint.X : UpperLeft_geoPoint.X;
           endLat = LowerRight_geoPoint.Y >= UpperLeft_geoPoint.Y ? LowerRight_geoPoint.Y : UpperLeft_geoPoint.Y;


            AlertForm alrtForm = new AlertForm();

            alrtForm.FormBorderStyle = FormBorderStyle.None;
            alrtForm.Opacity = 0.5;
            alrtForm.BackgroundImage = SigmaChart.Properties.Resources.SigmaSplash;

            alrtForm.progressBar1.Visible = true;
            alrtForm.progressBar1.ForeColor = System.Drawing.Color.FromArgb(255, 26, 119, 85);
            alrtForm.progressBar1.Maximum = 20;
            alrtForm.progressBar1.Value = 0;
            alrtForm.progressBar1.Visible = false;
            alrtForm.label1.BackColor = System.Drawing.Color.FromArgb(255, 26, 119, 85);
            alrtForm.label1.Text = "Isogonal Lines calculating";
            alrtForm.label1.Visible = true;
            if(!System.Diagnostics.Debugger.IsAttached) alrtForm.Show();
            alrtForm.BringToFront();
            int MagLinesCount = MagLinesList.CreatePolylines(stLon, endLon, 0.1, stLat, endLat, 0.1, -15, 15, magStep);



           //сохранить их в таблице
            SigmaDataCash.environmentWorkspaceEdit.StartEditing(false);
            SigmaDataCash.environmentWorkspaceEdit.StartEditOperation();

            alrtForm.progressBar1.Maximum = MagLinesCount;
            alrtForm.progressBar1.Value = 0;
            alrtForm.progressBar1.Visible = true;
            alrtForm.label1.Text = "Storing data";
            alrtForm.BringToFront();

            for (int i = 0; i <= MagLinesCount - 1; i++)
            {
                try
                {
                    IPolyline magLnGeo = MagLinesList.GetPolyline(i);

                    var zAware = magLnGeo as IZAware;
                    zAware.ZAware = true;

                    var mAware = magLnGeo as IMAware;
                    mAware.MAware = true;

                    IFeature newFT = isonalLinesIFeatureClass.CreateFeature();
                    int c = newFT.Fields.FindField("magValue");
                    if (c > 0) newFT.set_Value(c, magLnGeo.FromPoint.Z);
                    c = newFT.Fields.FindField("elevValue");
                    if (c > 0) newFT.set_Value(c, MagLinesList.Altitude);
                    newFT.Shape = magLnGeo;
                    newFT.Store();
                    alrtForm.progressBar1.Value ++;
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                    System.Windows.Forms.MessageBox.Show(ex.Source);
                    alrtForm.Close();
                }

            }

            SigmaDataCash.environmentWorkspaceEdit.StopEditOperation();
            SigmaDataCash.environmentWorkspaceEdit.StopEditing(true);

            alrtForm.Close();

            m_hookHelper.ActiveView.Refresh();

        }


        #endregion
    }
}
