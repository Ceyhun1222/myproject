using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using EnrouteChartCompare;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ARENA.Enums_Const;
using System.Collections.Generic;
using DataModule;
using ArenaStatic;
using System.Windows.Forms;
using ESRI.ArcGIS.Framework;
using ARENA;
using ESRI.ArcGIS.ArcMapUI;
using ANCOR.MapElements;
using PDM;
using System.Linq;
using ESRI.ArcGIS.Carto;
using ChartCompare;
using LogChartView = EnrouteChartCompare.View.LogChartView;
using System.ComponentModel;
using ANCOR.MapCore;
using ESRI.ArcGIS.Geometry;
using EsriWorkEnvironment;
using ESRI.ArcGIS.Geodatabase;

namespace SigmaChart
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout
    /// </summary>
    [Guid("7f144565-e24d-4b53-a48d-8c4da6d6391d")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("SigmaChart.UpdateChart")]
    public sealed class UpdateChart : BaseCommand
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
        private IApplication m_application;
        List<PDM.PDMObject> oldPdmList;
        List<PDM.PDMObject> newPdmList;

        private string selectedChart;

        public UpdateChart()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "SigmaChart"; //localizable text
            base.m_caption = "Update Enroute Chart";  //localizable text 
            base.m_message = "This should work in ArcMap/MapControl/PageLayoutControl";  //localizable text
            base.m_toolTip = "Enroute Chart";  //localizable text
            base.m_name = "UpdateEnrouteChart";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

            try
            {
                //
                // TODO: change bitmap name if necessary
                //
                //string bitmapResourceName = GetType().Name + ".bmp";
                //base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
                base.m_bitmap = global::SigmaChart.Properties.Resources.Sigma;
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
            m_application = hook as IApplication;
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
            if (DataCash.ProjectEnvironment == null || DataCash.ProjectEnvironment.Data.PdmObjectList.Count <= 0)
            {
                MessageBox.Show("Open Arena project ");
                return;
            }

            var EnroutePDMTypeList = new[] { PDM.PDM_ENUM.AirportHeliport, PDM_ENUM.Airspace,PDM_ENUM.WayPoint, PDM_ENUM.NavaidSystem, PDM_ENUM.HoldingPattern,PDM_ENUM.Enroute,PDM_ENUM.RouteSegment};


            string ff = (m_application.Document as IDocumentInfo2).Path;

            var openFileDialog1 = new OpenFileDialog { Filter = @"Sigma chart (*.mxd)|*.mxd" };
            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;


            List<DetailedItem> UpdateList = new List<DetailedItem>();


            ArenaProjectType prjType = ArenaProjectType.ARENA;
            selectedChart = openFileDialog1.FileName;

            List<string> CEFID_List = ChartsHelperClass.Read_CEFID(System.IO.Path.GetDirectoryName(selectedChart));
            chartInfo ci = EsriUtils.GetChartIno(System.IO.Path.GetDirectoryName(selectedChart));

            newPdmList = DataCash.ProjectEnvironment.Data.PdmObjectList;
            newPdmList.RemoveAll(x => (!EnroutePDMTypeList.Contains(x.PDM_Type)));
            //newPdmList.RemoveAll(x => (x is PDM.VerticalStructure));
            if (ci.RouteLevel != null)
            {
                CODE_ROUTE_SEGMENT_CODE_LVL _lvl;

                Enum.TryParse<CODE_ROUTE_SEGMENT_CODE_LVL>(ci.RouteLevel, out _lvl);

                newPdmList.RemoveAll(x => (x is PDM.RouteSegment) && (((PDM.RouteSegment)x).CodeLvl != _lvl));


                foreach (var item in newPdmList)
                {
                    if (item.PDM_Type == PDM_ENUM.Enroute && !item.ID.StartsWith("00000000-0000-0000-0000-000000000000"))
                    {
                        Enroute ENRT = (Enroute)item;
                        ENRT.Routes.RemoveAll(x => (x is PDM.RouteSegment) && (((PDM.RouteSegment)x).CodeLvl != _lvl));
                    }
                }


            }


            //newPdmList.RemoveAll(x => !(x is PDM.Airspace)); // Í


            oldPdmList = ArenaDataModule.GetObjectsFromPdmFile(System.IO.Path.GetDirectoryName(selectedChart), ref prjType);
            oldPdmList.RemoveAll(x => (x is PDM.Airspace) && (((PDM.Airspace)x).CodeType == PDM.AirspaceType.OTHER));

            oldPdmList.RemoveAll(x => (x is PDM.Enroute) && (((PDM.Enroute)x).Routes == null || ((PDM.Enroute)x).Routes.Count == 0));


            

            var routesList = DataCash.GetRouteSegmentsList(oldPdmList);


            ILayer _Layer = EsriUtils.getLayerByName(m_hookHelper.ActiveView.FocusMap, "AirportHeliport");
            if (_Layer == null) _Layer = EsriUtils.getLayerByName(m_hookHelper.ActiveView.FocusMap, "AirportCartography");

            var fc = ((IFeatureLayer)_Layer).FeatureClass;

            ISpatialReference pSpatialReference = (fc as IGeoDataset).SpatialReference;

            #region Construct AirspaceGeometry

            var arspList = newPdmList.FindAll(arsp => arsp.PDM_Type == PDM_ENUM.Airspace && ((Airspace)arsp).VolumeGeometryComponents != null && ((Airspace)arsp).AirspaceVolumeList.Count > 1).ToList();

            foreach (Airspace item in arspList)
            {

                try
                {
                    IGeometry gm = ConstructVolumeGeometry(item,newPdmList,oldPdmList, pSpatialReference);
                    if (gm != null)
                    {
                        item.AirspaceVolumeList[0].Geo = gm;
                        item.AirspaceVolumeList[0].BrdrGeometry = AranSupport.HelperClass.SetObjectToBlob(item.AirspaceVolumeList[0].Geo, "Border");
                    }
                    if (item.AirspaceVolumeList.Count > 1) item.AirspaceVolumeList.RemoveRange(1, item.AirspaceVolumeList.Count - 1);
                }
                catch (Exception ex)
                {


                }

            }

            #endregion


            List<string> ChangedFeaturesId = new List<string>();
            List<string> RemovedFeaturesId = new List<string>();
            List<string> AddedFeaturesId = new List<string>();

            if (CEFID_List !=null && CEFID_List.Count >0)
            {

                foreach (var pdmO in newPdmList)
                {
                    //if (!pdmO.ID.StartsWith("00000000-0000-0000-0000-000000000000"))
                    //    continue;

                    List<string> idsL = pdmO.GetIdList();

                    foreach (var id in idsL)
                    {
                       var tmpVar =  from element in CEFID_List where element != null && element.StartsWith(id) select element.ToList();

                       // if (CEFID_List.Contains(id))
                       if (tmpVar!=null && tmpVar.Count() > 0)
                            ChangedFeaturesId.Add(id);
                    }



                }


                List<PDMObject> removed_featureList = (from element in newPdmList
                                                       where (element != null && element.FeatureLifeTime!=null && element.FeatureLifeTime.EndPosition!=null 
                                                       && element.FeatureLifeTime.EndPosition.HasValue && element.FeatureLifeTime.EndPosition.Value < DateTime.Now)
                                                    select element).ToList();

                if (removed_featureList != null && removed_featureList.Count > 0)
                {
                    foreach (var item in removed_featureList)
                    {
                        if (!RemovedFeaturesId.Contains(item.ID))
                        {
                            RemovedFeaturesId.Add(item.ID);
                        }
                    }
                }

                removed_featureList = (from element in newPdmList
                                                       where (element != null && element.PDM_Type == PDM_ENUM.Enroute && (((Enroute)element).Routes != null))
                                                       select element).ToList();
                foreach (var item in removed_featureList)
                {
                    foreach (var rt in ((Enroute)item).Routes)
                    {
                        if (rt.FeatureLifeTime != null && rt.FeatureLifeTime.EndPosition != null)
                        {
                            if (!RemovedFeaturesId.Contains(rt.ID))
                                RemovedFeaturesId.Add(rt.ID);
                        }
                    }
                }

            }


            if (ChangedFeaturesId.Count >= 0)
            {


                foreach (var oldPdmObj in oldPdmList)
                {

                    //if (oldPdmObj.ID.StartsWith("29ffb614-872d-4e56-9d0d-3edf6597f887"))
                    //    System.Diagnostics.Debug.WriteLine(oldPdmObj.ID + oldPdmObj.GetObjectLabel()); //

                    foreach (var id in ChangedFeaturesId)
                    {

                        if (oldPdmObj.CompareId(id))
                        {

                            DetailedItem dIt = new DetailedItem();
                            dIt.FieldLogList = new List<FieldLog>();
                            dIt.Name = oldPdmObj.GetObjectLabel();
                            dIt.IsChecked = true;
                            dIt.ChangedStatus = Status.Changed;
                            
                            if (oldPdmObj.PDM_Type == PDM_ENUM.Enroute)
                            {
                                Enroute enrtPermdelta = (Enroute)oldPdmObj;

                                PDMObject res = (from element in enrtPermdelta.Routes
                                                 where (element != null) && (element.ID.StartsWith(id))
                                                 select element).FirstOrDefault();

                                if (res != null)
                                {
                                    dIt.Name = dIt.Name + "- - " + res.GetObjectLabel();
                                    var lst = GetPermObj(newPdmList, id, oldPdmObj);
                                    if (lst != null && lst.logList.Count >0) { dIt.FieldLogList.AddRange(lst.logList); dIt.Feature = lst.obj; }
                                }
                            }
                            else if (oldPdmObj.PDM_Type == PDM_ENUM.Airspace)
                            {
                                var lst = GetPermObj(newPdmList, id, oldPdmObj);
                                if (lst != null && lst.logList.Count > 0) { dIt.FieldLogList.AddRange(lst.logList); dIt.Feature = lst.obj; }
                            }
                            else
                            {

                                var lst = GetPermObj(newPdmList, id, oldPdmObj);
                                if (lst != null && lst.logList.Count > 0) { dIt.FieldLogList.AddRange(lst.logList); dIt.Feature = lst.obj; }
                            }


                            if (dIt.FieldLogList !=null && dIt.FieldLogList.Count > 0 && UpdateList.FindAll(d => d.Id.CompareTo(dIt.Id) == 0).FirstOrDefault() == null)
                                UpdateList.Add(dIt);
                        }

                    }
                    
                }

                #region Removed

                foreach (var rem_item in RemovedFeaturesId)
                {
                    PDMObject removed_feature = (from element in oldPdmList
                                                 where (element != null && element.ID != null && element.ID.CompareTo(rem_item) == 0)
                                                           select element).FirstOrDefault();

                    if (removed_feature !=null)
                    {
                        DetailedItem dIt = new DetailedItem();
                        dIt.FieldLogList = new List<FieldLog>();
                        dIt.Name = removed_feature.GetObjectLabel();
                        dIt.IsChecked = true;
                        dIt.ChangedStatus = Status.Deleted;
                        dIt.Feature = removed_feature;

                        FieldLog fl = new FieldLog(removed_feature.GetObjectLabel(), "Deleted", "");
                        fl.ChangeText = "Changed";
                        dIt.FieldLogList.Add(fl);


                        if (dIt.FieldLogList != null && dIt.FieldLogList.Count > 0)
                            UpdateList.Add(dIt);

                    }
                    else
                    {
                        PDMObject removed_route = (from element in routesList
                                                     where (element != null && element.ID != null && element.ID.CompareTo(rem_item) == 0)
                                                     select element).FirstOrDefault();

                        if (removed_route != null)
                        {
                            DetailedItem dIt = new DetailedItem();
                            dIt.FieldLogList = new List<FieldLog>();
                            dIt.Name = ((RouteSegment)removed_route).RouteFormed +" "+ removed_route.GetObjectLabel();
                            dIt.IsChecked = true;
                            dIt.ChangedStatus = Status.Deleted;
                            dIt.Feature = removed_route;

                            FieldLog fl = new FieldLog(removed_route.GetObjectLabel(), "Deleted", "");
                            fl.ChangeText = "Changed";
                            dIt.FieldLogList.Add(fl);

                            if (dIt.FieldLogList != null && dIt.FieldLogList.Count > 0)
                                UpdateList.Add(dIt);

                        }
                    }



                    if (CEFID_List.Contains(rem_item)) CEFID_List.Remove(rem_item);

                }

                #endregion


                #region Added

                foreach (var new_item in newPdmList)
                {
  
                    var tmpVar = from element in CEFID_List where element != null && element.StartsWith(new_item.ID) select element.ToList();

                    //if (!CEFID_List.Contains(new_item.ID))
                    if (tmpVar != null && tmpVar.Count() == 0)
                    {
                        DetailedItem dIt = new DetailedItem();
                        dIt.FieldLogList = new List<FieldLog>();
                        dIt.Name = new_item.GetObjectLabel();
                        if (dIt.Name.Trim().Length == 0) continue;
                        dIt.IsChecked = true;
                        dIt.ChangedStatus = Status.New;
                        dIt.Feature = new_item;

                        FieldLog fl = new FieldLog(new_item.GetObjectLabel(), "New", "");
                        fl.ChangeText = "Added";
                        dIt.FieldLogList.Add(fl);

                        if (dIt.FieldLogList != null && dIt.FieldLogList.Count > 0)
                            UpdateList.Add(dIt);

                        CEFID_List.Add(new_item.ID);
                    }

                }

                #endregion


                #region Enroute

                List<PDMObject> Enroutes_featureList = (from element in newPdmList
                                                        where (element != null
                                                        && element.PDM_Type == PDM_ENUM.Enroute
                                                        && ((Enroute)element).TxtDesig != null
                                                        && ((Enroute)element).TxtDesig.CompareTo("permdeltaRoutes") != 0)
                                                        select element).ToList();
                if (Enroutes_featureList != null)
                {
                    foreach (var enrt in Enroutes_featureList)
                    {
                        Enroute enroute = (Enroute)enrt;

                        var rtSeg = (from element in routesList
                                     where (element != null && ((RouteSegment)element).ID_Route.CompareTo(enroute.ID) == 0)
                                     select element).ToList();
                        if (rtSeg != null)
                        {
                            foreach (var item in rtSeg)
                            {
                                
                                DetailedItem dIt = new DetailedItem();
                                dIt.FieldLogList = new List<FieldLog>();
                                dIt.Name = item.GetObjectLabel();
                                dIt.IsChecked = true;
                                dIt.ChangedStatus = Status.Changed;
                                dIt.Feature = item;
                                
                                FieldLog fl = new FieldLog("RouteFormed", ((RouteSegment)item).RouteFormed, enroute.TxtDesig);
                                //((RouteSegment)item).ID_Route
                                fl.ChangeText = "Changed";
                                dIt.FieldLogList.Add(fl);

                                if (dIt.FieldLogList != null && dIt.FieldLogList.Count > 0)
                                    UpdateList.Add(dIt);
                            }
                        }

                    }
                }



                var PermEnroutes_featureList = (from element in newPdmList
                                                where (element != null
                                                && element.PDM_Type == PDM_ENUM.Enroute
                                                && ((Enroute)element).TxtDesig != null
                                                && ((Enroute)element).TxtDesig.CompareTo("permdeltaRoutes") == 0)
                                                select element).FirstOrDefault();
                if (PermEnroutes_featureList != null)
                {


                    //ÔÂÂ·Â‡ÂÏ RS ˜ÚÓ·˚ ÓÔÂ‰ÂÎËÚ¸ Ëı "ıÓÁˇËÌ‡"
                    foreach (RouteSegment seg in ((Enroute)PermEnroutes_featureList).Routes)
                    {
                        if (seg.FeatureLifeTime.EndPosition != null) continue; // Œ¡–¿¡Œ“¿“‹ ”ƒ¿À≈Õ»≈ —≈√Ã≈Õ“¿ Ã¿–ÿ–”“¿
                        
                        var tmpVar = from element in CEFID_List where element != null && element.StartsWith(seg.ID)  select element.ToList();

                        if (tmpVar != null && tmpVar.Count() == 0)
                        {
                            PDMObject masterRoute = DefineEnrouteName(seg.RouteFormed, newPdmList);
                            if (masterRoute == null) masterRoute = DefineEnrouteName(seg.RouteFormed, oldPdmList);

                            if(masterRoute != null)
                            {
                                seg.RouteFormed = ((Enroute)masterRoute).TxtDesig;
                                seg.ID_Route = masterRoute.ID;
                            }
                            if (seg.StartPoint != null)
                            {
                                if (seg.StartPoint.PointChoice == PointChoice.OTHER) seg.StartPoint.PointChoice = PointChoice.DesignatedPoint;

                                PDMObject _obj = DefinePointSegmentDesignator(((SegmentPoint)seg.StartPoint).PointChoice, seg.StartPoint.ID, newPdmList);
                                if (_obj == null)
                                    _obj = DefinePointSegmentDesignator(((SegmentPoint)seg.StartPoint).PointChoice, seg.StartPoint.ID, oldPdmList);

                                if (_obj == null) continue;
                                if (_obj.Geo == null) _obj.RebuildGeo();

                                var repAtc = seg.StartPoint.ReportingATC;

                                seg.StartPoint = new RouteSegmentPoint
                                {
                                    ID = seg.StartPoint!=null && seg.StartPoint.ID !=null ? seg.StartPoint.ID :  seg.SourceDetail, //Guid.NewGuid().ToString(),
                                    Route_LEG_ID = seg.ID,
                                    PointRole = ProcedureFixRoleType.ENRT,
                                    PointUse = ProcedureSegmentPointUse.START_POINT,
                                    IsWaypoint = ((SegmentPoint)seg.StartPoint).IsWaypoint,
                                    PointChoice = ((SegmentPoint)seg.StartPoint).PointChoice,
                                    SegmentPointDesignator = ((SegmentPoint)seg.StartPoint).SegmentPointDesignator,
                                    Geo = _obj.Geo,
                                    Lat = _obj.Lat,
                                    Lon = _obj.Lon,
                                    X = _obj.X,
                                    Y = _obj.Y,
                                    ReportingATC = repAtc,
                                    PointChoiceID = _obj.ID,
                                    
                                };
                            }

                            if (seg.EndPoint != null)
                            {
                                if (seg.EndPoint.PointChoice == PointChoice.OTHER) seg.EndPoint.PointChoice = PointChoice.DesignatedPoint;

                                PDMObject _obj = DefinePointSegmentDesignator(((SegmentPoint)seg.EndPoint).PointChoice, seg.EndPoint.ID, newPdmList);
                                if (_obj == null)
                                    _obj = DefinePointSegmentDesignator(((SegmentPoint)seg.EndPoint).PointChoice, seg.EndPoint.ID, oldPdmList);

                                if (_obj.Geo == null) _obj.RebuildGeo();

                                var repAtc = seg.EndPoint.ReportingATC;

                                seg.EndPoint = new RouteSegmentPoint
                                {
                                    ID = seg.EndPoint != null && seg.EndPoint.ID != null ? seg.EndPoint.ID : seg.SourceDetail, //Guid.NewGuid().ToString(),
                                    Route_LEG_ID = seg.ID,
                                    PointRole = ProcedureFixRoleType.ENRT,
                                    PointUse = ProcedureSegmentPointUse.END_POINT,
                                    IsWaypoint = ((SegmentPoint)seg.EndPoint).IsWaypoint,
                                    PointChoice = ((SegmentPoint)seg.EndPoint).PointChoice,
                                    SegmentPointDesignator = ((SegmentPoint)seg.EndPoint).SegmentPointDesignator,
                                    Geo = _obj.Geo,
                                    Lat = _obj.Lat,
                                    Lon = _obj.Lon,
                                    X = _obj.X,
                                    Y = _obj.Y,
                                    ReportingATC = repAtc,
                                    PointChoiceID = _obj.ID,
                                };
                            }


                            DetailedItem dIt = new DetailedItem();
                            dIt.FieldLogList = new List<FieldLog>();
                            dIt.Name = seg.RouteFormed +" "+ seg.GetObjectLabel();
                            dIt.IsChecked = true;
                            dIt.ChangedStatus = Status.New;
                            dIt.Feature = seg;

                            FieldLog fl = new FieldLog(seg.GetObjectLabel(), "New", "");
                            fl.ChangeText = "Added";
                            dIt.FieldLogList.Add(fl);

                            if (dIt.FieldLogList != null && dIt.FieldLogList.Count > 0)
                                UpdateList.Add(dIt);

                            CEFID_List.Add(seg.ID);
                        }

                    }




                }


                #endregion




            }

            ArenaStaticProc.SetTargetDB("");


            if (UpdateList.Count > 0)
            {

                foreach (var item in UpdateList)
                {
                    foreach (var fl in item.FieldLogList)
                    {
                        if (fl.ChangeText.CompareTo("Changed") == 0)
                        {
                            item.IsChecked = true;
                            break;
                        }
                    }
                }

                EnrouteUpdateForm updtFrm = new EnrouteUpdateForm(UpdateList);
                if (updtFrm.ShowDialog() == DialogResult.OK)
                {

                    SigmaDataCash.ChangeList = updtFrm._updateList;
                    SigmaDataCash.UpdateState = 11;
                    SigmaDataCash.oldPdmList = oldPdmList;
                    SigmaDataCash.newPdmList = newPdmList;

                    
                    m_application.OpenDocument(selectedChart);
                    Application.DoEvents();

                    

                    m_application.SaveDocument();

                    m_hookHelper.ActiveView.Refresh();

                }

            }

            
        }


        private updtTempClass GetPermObj(List<PDMObject> newPdmList, string id, PDMObject OldObj)
        {
            //AirspaceType[] arspcTypes = {AirspaceType.TMA, AirspaceType.TMA_P, AirspaceType.ATZ, AirspaceType.ATZ_P,AirspaceType.FIR, AirspaceType.FIR_P, AirspaceType.CTR, AirspaceType.CTR_P, AirspaceType.D,
            //                                AirspaceType.P,AirspaceType.R, AirspaceType.UIR, AirspaceType.UIR_P, AirspaceType.CTA, AirspaceType.CTA_P};


            List<FieldLog> flLst = new List<FieldLog>();
            string id2 = id;
            if (id.Length >36)
            id2 = id.Substring(0,36);
            


            PDMObject obj = (from element in newPdmList
                   where (element != null) && (element.ID.StartsWith(id) || element.ID.StartsWith(id2))
                   select element).FirstOrDefault();

            if (obj == null)
            {
               var enrtres = (from element in newPdmList
                       where (element != null) && (element.PDM_Type == PDM_ENUM.Enroute)&& (((Enroute)element).Routes !=null) 
                              select element).ToList();

                if (enrtres != null)
                {
                    foreach (var enrt in enrtres)
                    {

                        Enroute enrtpermdelta = (Enroute)enrt;

                        obj = (from element in enrtpermdelta.Routes
                               where (element != null) && (element.ID.StartsWith(id))
                               select element).FirstOrDefault();
                        if (OldObj.PDM_Type == PDM_ENUM.Enroute)
                        {
                            var OldObj2 = (from element in ((Enroute)OldObj).Routes
                                           where (element != null) && (element.ID.StartsWith(id))
                                           select element).FirstOrDefault();

                            if (obj != null) { OldObj = OldObj2; break; }
                        }
                        
                    }
                }
            }



            if (obj != null)
            {


                PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(obj);

                foreach (PropertyDescriptor pd in pdc)
                {
                    Attribute attributeBrowsable = pd.Attributes[typeof(BrowsableAttribute)];
                    if (((BrowsableAttribute)attributeBrowsable).Browsable)
                    {
                        //System.Diagnostics.Debug.WriteLine(pd.Name);

                        if (pd.Name.StartsWith("Interpritation")) continue;
                        if (pd.Name.StartsWith("ValidTime")) continue;
                        if (pd.Name.StartsWith("FeatureLifeTime")) continue;
                        if (pd.Name.StartsWith("ID")) continue;
                        if (pd.Name.StartsWith("ActualDate")) continue;
                        if (pd.Name.StartsWith("SourceDetail")) continue;
                        if (pd.Name.StartsWith("VolumeGeometryComponents")) continue;
                        if (pd.Name.StartsWith("NonStandardHolding")) continue;
                        if (pd.Name.Contains("Metadata")) continue;
                        
                        //if (pd.Name.StartsWith("")) continue;
                        //if (pd.Name.StartsWith("")) continue;
                        string objVal_new = ArenaStaticProc.GetObjectValueAsString(obj, pd.Name);
                        string objVal_old = ArenaStaticProc.GetObjectValueAsString(OldObj, pd.Name);
                        if (objVal_new == null) continue;
                        if (objVal_new.Length <= 0) continue;
                        if (objVal_new.StartsWith("OTHER")) continue;
                        if (objVal_new.StartsWith("NaN")) continue;

                        bool checkFlag = objVal_old == null || objVal_old.CompareTo(objVal_new) != 0;
                        FieldLog fl = new FieldLog(pd.Name, objVal_old, objVal_new);
                        fl.ChangeText = checkFlag ? "Changed" : "Not changed";

                        flLst.Add(fl);

                    }

                }

                if (obj.PDM_Type == PDM_ENUM.Airspace)
                {
                    Airspace arspc = (Airspace)obj;
                    Airspace OldArsps = (Airspace)OldObj;

                    if (arspc.CodeType == AirspaceType.OTHER)
                        arspc.CodeType = OldArsps.CodeType;

                    //if (arspcTypes.Contains(OldArsps.CodeType))
                    {
                        if (arspc.AirspaceVolumeList != null)
                        {
                            foreach (var vol in arspc.AirspaceVolumeList)
                            {
                                var arspc_OldObj = (from element in OldArsps.AirspaceVolumeList
                                                    where (element != null) &&
                                                        (element.ID.CompareTo(vol.ID) == 0)
                                                    select element).FirstOrDefault();
                                if (arspc_OldObj == null) continue;
                                pdc = TypeDescriptor.GetProperties(vol);

                                foreach (PropertyDescriptor pd in pdc)
                                {
                                    Attribute attributeBrowsable = pd.Attributes[typeof(BrowsableAttribute)];
                                    if (((BrowsableAttribute)attributeBrowsable).Browsable)
                                    {
                                        //System.Diagnostics.Debug.WriteLine(pd.Name);
                                        if (pd.Name.StartsWith("Interpritation")) continue;
                                        if (pd.Name.StartsWith("ValidTime")) continue;
                                        if (pd.Name.StartsWith("FeatureLifeTime")) continue;
                                        if (pd.Name.StartsWith("ID")) continue;
                                        if (pd.Name.StartsWith("ActualDate")) continue;
                                        if (pd.Name.StartsWith("SourceDetail")) continue;
                                        if (pd.Name.StartsWith("VolumeGeometryComponents")) continue;
                                        if (pd.Name.Contains("Metadata")) continue;
                                        //if (pd.Name.StartsWith("")) continue;
                                        //if (pd.Name.StartsWith("")) continue;
                                        string objVal_new = ArenaStaticProc.GetObjectValueAsString(vol, pd.Name);
                                        string objVal_old = ArenaStaticProc.GetObjectValueAsString(arspc_OldObj, pd.Name);
                                        if (objVal_new == null) continue;
                                        if (objVal_new.Length <= 0) continue;
                                        if (objVal_new.StartsWith("OTHER")) continue;
                                        if (objVal_new.StartsWith("NaN")) continue;
                                        if ((objVal_old!=null) && objVal_new.StartsWith(objVal_old)) continue;

                                        bool checkFlag = objVal_old == null || objVal_old.CompareTo(objVal_new) != 0;
                                        FieldLog fl = new FieldLog(pd.Name, objVal_old, objVal_new);
                                        fl.ChangeText = checkFlag ? "Changed" : "Not changed";

                                        flLst.Add(fl);

                                    }

                                }
                            }
                        }

                    }

                }

                if (obj.PDM_Type == PDM_ENUM.RouteSegment)
                {
                    RouteSegment RouteSegmantNew = (RouteSegment)obj;
                    RouteSegment RouteSegmantOLD = (RouteSegment)OldObj;

                    if (RouteSegmantNew.StartPoint != null)
                    {
                        var segPnt_OldObj = RouteSegmantOLD.StartPoint;
                        if (segPnt_OldObj != null)
                        {
                            pdc = TypeDescriptor.GetProperties(RouteSegmantNew.StartPoint);

                            foreach (PropertyDescriptor pd in pdc)
                            {
                                Attribute attributeBrowsable = pd.Attributes[typeof(BrowsableAttribute)];
                                if (((BrowsableAttribute)attributeBrowsable).Browsable)
                                {
                                    System.Diagnostics.Debug.WriteLine(pd.Name);
                                    if (pd.Name.StartsWith("Interpritation")) continue;
                                    if (pd.Name.StartsWith("ValidTime")) continue;
                                    if (pd.Name.StartsWith("FeatureLifeTime")) continue;
                                    if (pd.Name.StartsWith("ID")) continue;
                                    if (pd.Name.StartsWith("ActualDate")) continue;
                                    if (pd.Name.StartsWith("SourceDetail")) continue;
                                    if (pd.Name.StartsWith("VolumeGeometryComponents")) continue;
                                    if (pd.Name.Contains("Metadata")) continue;
                                    //if (pd.Name.StartsWith("")) continue;
                                    //if (pd.Name.StartsWith("")) continue;
                                    string objVal_new = ArenaStaticProc.GetObjectValueAsString(RouteSegmantNew.StartPoint, pd.Name);
                                    string objVal_old = ArenaStaticProc.GetObjectValueAsString(segPnt_OldObj, pd.Name);
                                    if (objVal_new == null) continue;
                                    if (objVal_new.Length <= 0) continue;
                                    if (objVal_new.StartsWith("OTHER")) continue;
                                    if (objVal_new.StartsWith("NaN")) continue;
                                    if (objVal_new.StartsWith(objVal_old)) continue;

                                    bool checkFlag = objVal_old.CompareTo(objVal_new) != 0;
                                    FieldLog fl = new FieldLog(pd.Name, objVal_old, objVal_new);
                                    fl.ChangeText = checkFlag ? "StartPoint Changed" : "StartPoint Not changed";

                                    flLst.Add(fl);

                                }

                            }
                        }


                    }

                    if (RouteSegmantNew.EndPoint != null)
                    {
                        var segPnt_OldObj = RouteSegmantOLD.EndPoint;
                        if (segPnt_OldObj != null)
                        {
                            pdc = TypeDescriptor.GetProperties(RouteSegmantNew.EndPoint);

                            foreach (PropertyDescriptor pd in pdc)
                            {
                                Attribute attributeBrowsable = pd.Attributes[typeof(BrowsableAttribute)];
                                if (((BrowsableAttribute)attributeBrowsable).Browsable)
                                {
                                    System.Diagnostics.Debug.WriteLine(pd.Name);
                                    if (pd.Name.StartsWith("Interpritation")) continue;
                                    if (pd.Name.StartsWith("ValidTime")) continue;
                                    if (pd.Name.StartsWith("FeatureLifeTime")) continue;
                                    if (pd.Name.StartsWith("ID")) continue;
                                    if (pd.Name.StartsWith("ActualDate")) continue;
                                    if (pd.Name.StartsWith("SourceDetail")) continue;
                                    if (pd.Name.StartsWith("VolumeGeometryComponents")) continue;
                                    if (pd.Name.Contains("Metadata")) continue;
                                    //if (pd.Name.StartsWith("")) continue;
                                    //if (pd.Name.StartsWith("")) continue;
                                    string objVal_new = ArenaStaticProc.GetObjectValueAsString(RouteSegmantNew.EndPoint, pd.Name);
                                    string objVal_old = ArenaStaticProc.GetObjectValueAsString(segPnt_OldObj, pd.Name);
                                    if (objVal_new == null) continue;
                                    if (objVal_new.Length <= 0) continue;
                                    if (objVal_new.StartsWith("OTHER")) continue;
                                    if (objVal_new.StartsWith("NaN")) continue;
                                    if (objVal_new.StartsWith(objVal_old)) continue;

                                    bool checkFlag = objVal_old.CompareTo(objVal_new) != 0;
                                    FieldLog fl = new FieldLog(pd.Name, objVal_old, objVal_new);
                                    fl.ChangeText = checkFlag ? "EndPoint Changed" : "EndPoint Not changed";

                                    flLst.Add(fl);

                                }

                            }
                        }


                    }

                }

                if (obj.Geo == null) obj.RebuildGeo();
                if (obj.Geo != null)
                {
                    FieldLog fl = new FieldLog("Geometry", "old Geo", "new Geo");
                    if (obj.Geo == null) obj.RebuildGeo();
                    fl.OldGeometry = obj.Geo;
                    fl.ChangeText = "Changed";
                    flLst.Add(fl);
                }

                if (obj.PDM_Type == PDM_ENUM.Airspace)
                {
                    Airspace arspc = (Airspace)obj;

                    //if (arspcTypes.Contains(arspc.CodeType))
                    {
                        foreach (AirspaceVolume vol in arspc.AirspaceVolumeList)
                        {
                            FieldLog fl = new FieldLog("Geometry", "old Geo", "new Geo");
                            if (vol.Geo == null) vol.RebuildGeo2();
                            fl.OldGeometry = vol.Geo;
                            fl.ChangeText = "Changed";
                            flLst.Add(fl);
                        }
                    }

                }

            }



            //return flLst.Count > 0 ? flLst : null;
            updtTempClass res = new updtTempClass { logList = flLst , obj = OldObj };


            res.logList.RemoveAll(x => x.ChangeText !=null &&   x.ChangeText.Contains("Not changed"));


            return res;
        }

        public void ClosedEnrouteUpdateWindow ( object sender, EventArgs arg )
		{
			List<DetailedItem> resultList = ( List<DetailedItem> ) sender;


           string ArchiveName = ArenaStaticProc.SendMapToArchive(selectedChart);

            MessageBox.Show("Archived copy saved " + System.IO.Path.GetFileNameWithoutExtension(ArchiveName), "Sigma", MessageBoxButtons.OK, MessageBoxIcon.Information);

           
            // load selected file
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////
            m_application.OpenDocument(selectedChart);

            IMxDocument pNewDocument = (IMxDocument)m_application.Document;
            m_application.RefreshWindow();
            SigmaDataCash.environmentWorkspaceEdit = null;

            Application.DoEvents();

            // update SigmaDataCash.environmentWorkspaceEdit
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            for (int i = 0; i < ((IMxDocument)m_application.Document).ContentsViewCount; i++)
            {
                IContentsView cnts = ((IMxDocument)m_application.Document).get_ContentsView(i);

                string cntxtName = ((IMxDocument)m_application.Document).ContentsView[i].Name;

                if (cntxtName.StartsWith("ANCORTOCLayerView")) ((IMxDocument)m_application.Document).ContentsView[i].Refresh(cntxtName);

            }


            SigmaDataCash.environmentWorkspaceEdit.StartEditing(false);
            SigmaDataCash.environmentWorkspaceEdit.StartEditOperation();

            EnrouteChartClass _Chart = new EnrouteChartClass { SigmaHookHelper = m_hookHelper, SigmaApplication = m_application };
            _Chart.UpdateChart(resultList, oldPdmList, newPdmList);

          
            SigmaDataCash.environmentWorkspaceEdit.StopEditOperation();
            SigmaDataCash.environmentWorkspaceEdit.StopEditing(true);



            string destPath2 = System.IO.Path.GetDirectoryName(selectedChart);
            ChartsHelperClass.SaveSourcePDMFiles(destPath2, newPdmList); 

            for (int i = 0; i < ((IMxDocument)m_application.Document).ContentsViewCount; i++)
            {
                IContentsView cnts = ((IMxDocument)m_application.Document).get_ContentsView(i);

                string cntxtName = ((IMxDocument)m_application.Document).ContentsView[i].Name;

                if (cntxtName.StartsWith("ANCORTOCLayerView")) ((IMxDocument)m_application.Document).ContentsView[i].Refresh(cntxtName);

            }


            ArenaMessageForm msgFrm = new ArenaMessageForm("SIGMA", "The Chart is updated successfully", global::SigmaChart.Properties.Resources.SigmaMessage);
            msgFrm.TopMost = true;
            msgFrm.checkBox1.Visible = false;
            msgFrm.ShowDialog();

		}


        private PDMObject DefinePointSegmentDesignator(PointChoice pointChoice, string Identifier, List<PDMObject> pDMObjects)
        {
            PDMObject res = new PDMObject();
            IGeometry pntgeo = null;
            try
            {
                switch (pointChoice)
                {
                    case PointChoice.DesignatedPoint:

                        PDMObject _dpn = (from element in pDMObjects where (element != null) && (element is WayPoint) && (((WayPoint)element).ID.CompareTo(Identifier) == 0) select element).FirstOrDefault();

                        if (_dpn != null)
                        {
                            res.SourceDetail = ((WayPoint)_dpn).Designator;
                            
                            if ((((WayPoint)_dpn).Geo != null) && (((WayPoint)_dpn).Geo.GeometryType == esriGeometryType.esriGeometryPoint))
                            {
                                res.Lon = ((IPoint)(_dpn.Geo)).X.ToString();
                                res.Lat = ((IPoint)(_dpn.Geo)).Y.ToString();
                                pntgeo = _dpn.Geo;
                            }
                            else
                            {
                                res.Lon = _dpn.X.ToString();
                                res.Lat = _dpn.Y.ToString();

                            }


                        }
                        else res = null;

                        break;

                    case PointChoice.Navaid:

                        res = (from element in pDMObjects where (element != null) && (element is NavaidSystem) && (((NavaidSystem)element).ID.CompareTo(Identifier) == 0) select element).FirstOrDefault();

                        if (res == null)
                        {
                            bool nav_flag = false;
                            var _adhpLst = (from element in pDMObjects where (element != null) && (element is AirportHeliport) && (((AirportHeliport)element).RunwayList != null) select element).ToList();

                            if (_adhpLst != null)
                            {
                                foreach (AirportHeliport adhp in _adhpLst)
                                {
                                    if (adhp.RunwayList.Count <= 0) continue;
                                    foreach (Runway rwy in adhp.RunwayList)
                                    {
                                        if (rwy.RunwayDirectionList == null || rwy.RunwayDirectionList.Count <= 0) continue;
                                        foreach (RunwayDirection rdn in rwy.RunwayDirectionList)
                                        {
                                            if (rdn.Related_NavaidSystem == null || rdn.Related_NavaidSystem.Count <= 0) continue;

                                            res = (from element in rdn.Related_NavaidSystem where (element != null) && (element is NavaidSystem) && (element.ID.CompareTo(Identifier) == 0) select element).FirstOrDefault();

                                            if (res != null) { nav_flag = true; break; }
                                        }

                                        if (nav_flag) break;
                                    }
                                    if (nav_flag) break;
                                }

                            }
                        }

                        if (res != null)
                        {
                            //res.ID = ((NavaidSystem)res).Designator;
                            if ((((NavaidSystem)res).Components != null) && (((NavaidSystem)res).Components.Count > 0))
                            {
                                res.Lon = ((IPoint)(((NavaidSystem)res).Components[0].Geo)).X.ToString();
                                res.Lat = ((IPoint)(((NavaidSystem)res).Components[0].Geo)).Y.ToString();
                                pntgeo = ((NavaidSystem)res).Components[0].Geo;
                                if (((NavaidSystem)res).Designator != null) res.ID = ((NavaidSystem)res).Designator;
                            }
                        }

                        break;

                    case PointChoice.RunwayCentrelinePoint:

                        PDMObject _rcp = null;
                        bool flag = false;
                        var _objLst = (from element in pDMObjects where (element != null) && (element is AirportHeliport) && (((AirportHeliport)element).RunwayList != null) select element).ToList();

                        if (_objLst != null)
                        {
                            foreach (AirportHeliport adhp in _objLst)
                            {
                                if (adhp.RunwayList.Count <= 0) continue;
                                foreach (Runway rwy in adhp.RunwayList)
                                {
                                    if (rwy.RunwayDirectionList == null || rwy.RunwayDirectionList.Count <= 0) continue;
                                    foreach (RunwayDirection rdn in rwy.RunwayDirectionList)
                                    {
                                        if (rdn.CenterLinePoints == null || rdn.CenterLinePoints.Count <= 0) continue;

                                        _rcp = (from element in rdn.CenterLinePoints where (element != null) && (element is RunwayCenterLinePoint) && (((RunwayCenterLinePoint)element).ID.CompareTo(Identifier) == 0) select element).FirstOrDefault();

                                        if (_rcp != null) { flag = true; break; }
                                    }

                                    if (flag) break;
                                }
                                if (flag) break;
                            }

                        }

                        if (_rcp != null)
                        {
                            res.SourceDetail = ((RunwayCenterLinePoint)_rcp).Designator;
                            if ((((RunwayCenterLinePoint)_rcp).Geo != null) && (((RunwayCenterLinePoint)_rcp).Geo.GeometryType == esriGeometryType.esriGeometryPoint))
                            {
                                res.Lon = ((IPoint)(_rcp.Geo)).X.ToString();
                                res.Lat = ((IPoint)(_rcp.Geo)).Y.ToString();
                                pntgeo = _rcp.Geo;

                            }
                            else
                            {
                                res.Lon = _rcp.X.ToString();
                                res.Lat = _rcp.Y.ToString();

                            }


                        }
                        else res = null;
                

                        break;

                    case PointChoice.AirportHeliport:

                        PDMObject _ahp = (from element in pDMObjects where (element != null) && (element is AirportHeliport) && (((AirportHeliport)element).ID.CompareTo(Identifier) == 0) select element).FirstOrDefault();

                        if (_ahp != null)
                        {
                            res.SourceDetail = ((AirportHeliport)_ahp).Designator;
                            if ((((AirportHeliport)_ahp).Geo != null) && (((AirportHeliport)_ahp).Geo.GeometryType == esriGeometryType.esriGeometryPoint))
                            {
                                res.Lon = ((IPoint)(_ahp.Geo)).X.ToString();
                                res.Lat = ((IPoint)(_ahp.Geo)).Y.ToString();
                                pntgeo = _ahp.Geo;

                            }
                            else
                            {
                                res.Lon = _ahp.X.ToString();
                                res.Lat = _ahp.Y.ToString();

                            }


                        }
                        else res = null;

                        break;

                    default:
                        res.ID = "";
                        res.Lon = "0";
                        res.Lat = "0";
                        break;
                }

                res.Geo = (res != null && pntgeo != null) ? pntgeo : null;

                return res;
            }
            catch (Exception ex)
            {
                
                return null;
            }
        }

        private PDMObject DefineEnrouteName (string Identifier, List<PDMObject> pDMObjects)
        {
            return (from element in pDMObjects where (element != null) && (element is Enroute) && (element.ID.CompareTo(Identifier) == 0) select element).FirstOrDefault();

        }

        private IGeometry ConstructVolumeGeometry(Airspace item, List<PDMObject> NEW_PdmList, List<PDMObject> OLD_PdmList, ISpatialReference _SpatialReference)
        {
            IGeometry baseGeom = null;
            try
            {
                if (item.AirspaceVolumeList != null && item.AirspaceVolumeList.Count > 0)
                {
                    if (item.AirspaceVolumeList[0].Geo == null) item.AirspaceVolumeList[0].RebuildGeo();
                    baseGeom = item.AirspaceVolumeList[0].Geo;
                }

                IGeometry sndGeom = null;
                PDMObject arspBase = null;

                if (item.VolumeGeometryComponents != null)
                {
                    List<VolumeGeometryComponent> orderedGeoComp =
                        item.VolumeGeometryComponents.OrderBy(order => order.operationSequence).ToList();

                    if (baseGeom == null)
                    {
                        var baseGeoComp = orderedGeoComp.FindAll(g => g.operation == PDM.CodeAirspaceAggregation.BASE)
                            .FirstOrDefault();

                        if (baseGeoComp != null)
                        {
                            arspBase = NEW_PdmList.FindAll(arsp =>
                                    arsp.PDM_Type == PDM_ENUM.Airspace && arsp.ID.StartsWith(baseGeoComp.theAirspace))
                                .FirstOrDefault();
                            if (arspBase != null)
                                arspBase = OLD_PdmList.FindAll(arsp =>
                                    arsp.PDM_Type == PDM_ENUM.Airspace && arsp.ID.StartsWith(baseGeoComp.theAirspace))
                                .FirstOrDefault();

                            if (arspBase != null)
                            {
                                if (((Airspace)arspBase).AirspaceVolumeList[0].Geo == null)
                                    ((Airspace)arspBase).AirspaceVolumeList[0].RebuildGeo();
                                baseGeom = ((Airspace)arspBase).AirspaceVolumeList[0].Geo;
                                if (baseGeom == null) return null;
                            }
                        }
                    }

                    if (baseGeom == null) return null;

                    baseGeom.SpatialReference = _SpatialReference;
                    ITopologicalOperator2 topoOper2 = (ITopologicalOperator2)baseGeom;

                    foreach (var geoCom in orderedGeoComp)
                    {
                        //if (geoCom.airspaceDependency != PDM.CodeAirspaceDependency.FULL_GEOMETRY) continue;
                        arspBase = NEW_PdmList.FindAll(arsp =>
                                arsp.PDM_Type == PDM_ENUM.Airspace && arsp.ID.StartsWith(geoCom.theAirspace))
                            .FirstOrDefault();

                        arspBase = OLD_PdmList.FindAll(arsp =>
                                arsp.PDM_Type == PDM_ENUM.Airspace && arsp.ID.StartsWith(geoCom.theAirspace))
                            .FirstOrDefault();


                        if (arspBase != null)
                        {
                            if (((Airspace)arspBase).AirspaceVolumeList[0].Geo == null)
                                ((Airspace)arspBase).AirspaceVolumeList[0].RebuildGeo();
                            sndGeom = ((Airspace)arspBase).AirspaceVolumeList[0].Geo;
                            if (sndGeom == null) geoCom.operation = PDM.CodeAirspaceAggregation.OTHER;
                            else sndGeom.SpatialReference = _SpatialReference;
                            geoCom.theAirspaceName =
                                ((Airspace)arspBase).TxtName != null && ((Airspace)arspBase).TxtName.Length > 0
                                    ? ((Airspace)arspBase).TxtName
                                    : "";
                        }

                        if (arspBase == null) continue;


                        switch (geoCom.operation)
                        {
                            case PDM.CodeAirspaceAggregation.OTHER:
                            case PDM.CodeAirspaceAggregation.BASE:
                            case PDM.CodeAirspaceAggregation.INTERS:
                            default:
                                break;

                            case PDM.CodeAirspaceAggregation.UNION:
                                baseGeom = topoOper2.Union(sndGeom);
                                break;

                            case PDM.CodeAirspaceAggregation.SUBTR:
                                baseGeom = topoOper2.Difference(sndGeom);
                                break;

                        }

                        if (baseGeom != null)
                        {
                            ITopologicalOperator2 simpTopoOper2 = (ITopologicalOperator2)baseGeom;
                            simpTopoOper2.IsKnownSimple_2 = false;
                            simpTopoOper2.Simplify();
                            topoOper2 = (ITopologicalOperator2)baseGeom;
                        }
                    }

                }

                return baseGeom;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        #endregion
    }

    public class updtTempClass
    {
        public updtTempClass() { }

        public List<FieldLog> logList { get; set; }
        public PDMObject obj { get; set; }

    }
}
