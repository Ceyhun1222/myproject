using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PDM;
using System.Windows.Forms;
using ARINC_DECODER_CORE.AIRTRACK_Objects;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ARENA.Project;
using ESRI.ArcGIS.Geometry;
using EsriWorkEnvironment;
using ESRI.ArcGIS.esriSystem;
using AranSupport;
using System.Globalization;
using System.Threading;

namespace ARENA.DataLoaders
{
    public class ARINC_DataConverter : IARENA_DATA_Converter
    {
        private Environment.Environment _CurEnvironment;

        public Environment.Environment CurEnvironment
        {
            get { return _CurEnvironment; }
            set { _CurEnvironment = value; }
        }

        public ARINC_DataConverter()
        {
        }

        public ARINC_DataConverter(Environment.Environment env)
        {
            this.CurEnvironment = env;
        }

        public bool Convert_Data(IFeatureClass _FeatureClass)
        {
            var frm = new ARINC_DECODER_UI.Form1();

            if (frm.ShowDialog() == DialogResult.OK)
            {
                if (frm.Tag != null)
                {


                    CultureInfo oldCI = Thread.CurrentThread.CurrentCulture;
                    Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

                    AlertForm alrtForm = new AlertForm();
                    alrtForm.FormBorderStyle = FormBorderStyle.None;
                    alrtForm.Opacity = 0.5;
                    alrtForm.BackgroundImage = ArenaToolBox.Properties.Resources.ArenaSplash;
                    //alrtForm.TopMost = true;

                    if (!System.Diagnostics.Debugger.IsAttached) alrtForm.Show();

                    var lst = frm.Tag as List<Object_AIRTRACK>;
                    var fileSourceName = System.IO.Path.GetFileName(frm.textBox1.Text);

                    Application.DoEvents();

                    //// Workspace Geo
                    var workspaceEdit = (IWorkspaceEdit)_FeatureClass.FeatureDataset.Workspace;
                    workspaceEdit.StartEditing(false);
                    workspaceEdit.StartEditOperation();

                    #region

                    CurEnvironment.FillAirtrackTableDic(workspaceEdit);



                    #endregion


                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    if (lst != null)
                    {
                        alrtForm.progressBar1.Visible = true;
                        alrtForm.progressBar1.ForeColor = System.Drawing.Color.FromArgb(255, 22, 76, 108);
                        alrtForm.progressBar1.Maximum = 20;
                        alrtForm.progressBar1.Value = 0;

                        alrtForm.label1.Visible = true;
                        alrtForm.label1.BackColor = System.Drawing.Color.FromArgb(255, 23, 76, 107);
                        alrtForm.label1.Text = "Loading...";

                        alrtForm.progressBar1.Maximum = lst.Count;
                        alrtForm.progressBar1.Value = 0;

                        foreach (var obj in lst)
                        {

                            var pdmObj = AIRTRACK_PDM_Converter.AIRTRAC_Object_Convert(obj);

                            if (pdmObj != null)
                            {
                                alrtForm.label1.Text = "Loading..." + pdmObj.PDM_Type.ToString();

                                pdmObj.SourceDetail = "File name " + fileSourceName + " (ARINC 424)";


                                #region Obj is Airspace


                                if (pdmObj is Airspace)
                                {

                                    var arsps = obj as Airspace_AIRTRACK;
                                    for (var i = 0; i <= (pdmObj as Airspace).AirspaceVolumeList.Count - 1; i++)
                                    {
                                        var vol = arsps.AirspaceVolumeList[i];
                                        if (vol.Shape == null)
                                        {
                                            (pdmObj as Airspace).AirspaceVolumeList[i].Geo = BuildAirspaceBorder(vol, CurEnvironment, _FeatureClass);
                                            (pdmObj as Airspace).AirspaceVolumeList[i].BrdrGeometry = HelperClass.SetObjectToBlob((pdmObj as Airspace).AirspaceVolumeList[i].Geo, "Border");
                                        }
                                    }
                                }

                                #endregion

                                #region Obj is Obsracle

                                if (pdmObj is VerticalStructure)
                                {
                                    foreach (var item in ((VerticalStructure)pdmObj).Parts)
                                    {
                                        item.Vertex = HelperClass.SetObjectToBlob(item.Geo, "Vertex");

                                    }
                                }



                                #endregion

                                #region Enroute/RouteSegment

                                if (pdmObj is Enroute)
                                {
                                    foreach (RouteSegment item in ((Enroute)pdmObj).Routes)
                                    {
                                        item.LegBlobGeometry = HelperClass.SetObjectToBlob(item.Geo, "Leg");

                                    }
                                }

                                #endregion

                                if (pdmObj.StoreToDB(CurEnvironment.Data.TableDictionary))
                                {
                                    CurEnvironment.Data.PdmObjectList.Add(pdmObj);
                                }

                                #region obj is AIRPORT_AIRTRACK

                                if (pdmObj is AirportHeliport)
                                {
                                    var arp = (AirportHeliport)pdmObj;

                                    if ((arp.RunwayList != null) && (arp.RunwayList.Count > 0))
                                    {
                                        foreach (var rwy in arp.RunwayList)
                                        {

                                            if ((rwy.RunwayDirectionList == null) || (rwy.RunwayDirectionList.Count <= 0)) continue;
                                            foreach (var rdn in rwy.RunwayDirectionList)
                                            {
                                                if ((rdn.Related_NavaidSystem == null) || (rdn.Related_NavaidSystem.Count <= 0)) continue;
                                                foreach (NavaidSystem nav in rdn.Related_NavaidSystem)
                                                    CurEnvironment.Data.PdmObjectList.Add(nav);

                                            }


                                        }
                                    }

                                }

                                #endregion

                            }

                            alrtForm.progressBar1.Value++;
                            Application.DoEvents();
                        }
                    }
                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    workspaceEdit.StopEditOperation();
                    workspaceEdit.StopEditing(true);

                    CurEnvironment.FillAirportDictionary();

                    foreach (var pair in CurEnvironment.Data.AirdromeHeliportDictionary)
                    {
                        var adhpID = pair.Key;
                        var adhp = pair.Value;

                        _FeatureClass.FeatureDataset.Workspace.ExecuteSQL("UPDATE WayPoint SET WayPoint.ID_AirportHeliport = '" + adhpID + "' WHERE (WayPoint.ID_AirportHeliport='" + adhp.Designator + "')");
                        _FeatureClass.FeatureDataset.Workspace.ExecuteSQL("UPDATE NavaidSystem SET NavaidSystem.ID_AirportHeliport = '" + adhpID + "' WHERE (NavaidSystem.ID_AirportHeliport='" + adhp.Designator + "')");

                        if (adhp.RunwayList != null)
                        {
                            foreach (Runway rwy in adhp.RunwayList)
                            {
                                if (rwy.RunwayDirectionList != null)
                                {
                                    foreach (RunwayDirection rdn in rwy.RunwayDirectionList)
                                    {
                                        _FeatureClass.FeatureDataset.Workspace.ExecuteSQL("UPDATE NavaidSystem SET NavaidSystem.ID_RunwayDirection = '" + rdn.ID + "' WHERE (NavaidSystem.ID_RunwayDirection='" + rdn.Designator + "')");

                                    }
                                }
                            }
                        }

                    }

                    System.Threading.Thread.CurrentThread.CurrentCulture = oldCI;

                    alrtForm.Close();
                }
                return true;
            }
            else
                return false;

            
        }

        private IGeometry BuildAirspaceBorder(AirspaceVolume_AIRTRACK arsps, Environment.Environment Environment, IFeatureClass fc)
        {
            PolygonClass arspBorder;
            try
            {
                var wksPointsList = new List<WKSPoint>();

                Environment.Data.SpatialReference = ((IGeoDataset)fc).SpatialReference;

                IPointCollection Crcl = new MultipointClass();

                arspBorder = new PolygonClass();
                IGeometryBridge2 geometryBridge = new GeometryEnvironmentClass();

                var i = 0;

                foreach (var segment in arsps.GeomComponent)
                {
                    var segCoords = new airspaceSupport(segment);


                    var geotype = segment.Boundary_Via.Remove(1, 1);
                    WKSPoint pWks;
                    IPoint pPnt;
                    switch (geotype)
                    {
                        case ("G"):
                        case ("H"):
                            pWks = new WKSPoint { X = segCoords.Lon, Y = segCoords.Lat };
                            wksPointsList.Add(pWks);
                            break;
                        case ("C"):
                            pPnt = new PointClass();
                            pPnt.PutCoords(segCoords.Arc_Origin_Longitude, segCoords.Arc_Origin_Latitude);
                            pPnt = EsriUtils.ToProject(pPnt, Environment.pMap, Environment.Data.SpatialReference) as IPoint;

                            Crcl = Environment.Data.GeometryFunctions.CreatePrjCircle(pPnt, segCoords.Arc_Distance);

                            for (int j = 0; j <= Crcl.PointCount - 1; j++)
                            {
                                pPnt = new PointClass();
                                pPnt.PutCoords(Crcl.Point[j].X, Crcl.Point[j].Y);
                                pPnt = EsriUtils.ToGeo(pPnt, Environment.pMap, Environment.Data.SpatialReference) as IPoint;

                                pWks = new WKSPoint { X = pPnt.X, Y = pPnt.Y };
                                wksPointsList.Add(pWks);
                            }
                            break;

                        case ("A"):
                        case ("L"):
                        case ("R"):

                            IPoint pPntCntr = new PointClass();
                            pPntCntr.PutCoords(segCoords.Arc_Origin_Longitude, segCoords.Arc_Origin_Latitude);
                            pPntCntr = EsriUtils.ToProject(pPntCntr, Environment.pMap, Environment.Data.SpatialReference) as IPoint;

                            IPoint pPntFrom = new PointClass();
                            pPntFrom.PutCoords(segCoords.Lon, segCoords.Lat);
                            pPntFrom = EsriUtils.ToProject(pPntFrom, Environment.pMap, Environment.Data.SpatialReference) as IPoint;

                            var nextSegCoords = new airspaceSupport(arsps.GeomComponent[i + 1]);

                            IPoint pPntTo = new PointClass();
                            pPntTo.PutCoords(nextSegCoords.Lon, nextSegCoords.Lat);
                            pPntTo = EsriUtils.ToProject(pPntTo, Environment.pMap, Environment.Data.SpatialReference) as IPoint;

                            var arcCurse = 1;
                            if (geotype.StartsWith("R")) arcCurse = -1;

                            Crcl = Environment.Data.GeometryFunctions.CreateArcPrj2(pPntCntr, pPntFrom, pPntTo, arcCurse);

                            for (int j = 0; j <= Crcl.PointCount - 1; j++)
                            {
                                pPnt = new PointClass();
                                pPnt.PutCoords(Crcl.Point[j].X, Crcl.Point[j].Y);
                                pPnt = EsriUtils.ToGeo(pPnt, Environment.pMap, Environment.Data.SpatialReference) as IPoint;

                                pWks = new WKSPoint();
                                pWks.X = pPnt.X;
                                pWks.Y = pPnt.Y;
                                wksPointsList.Add(pWks);
                            }

                            break;

                    }
                    i++;
                }

                WKSPoint[] pointArray = wksPointsList.ToArray();

                geometryBridge.AddWKSPoints(arspBorder, ref pointArray);
                arspBorder.Simplify();

                var zAware = arspBorder as IZAware;
                zAware.ZAware = true;
                arspBorder.SetConstantZ(0);


                var mAware = arspBorder as IMAware;
                mAware.MAware = true;



            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace + " ERROR " + ex.Message);
                throw;
            }

            return arspBorder;
        }

    }

}
