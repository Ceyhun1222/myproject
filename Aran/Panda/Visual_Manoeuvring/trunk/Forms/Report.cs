using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Aran.Geometries;
using Aran.Panda.Common;

namespace Aran.Panda.VisualManoeuvring.Forms
{
    public partial class Report : Form
    {
        MainForm myMF;
        private ListViewColumnSorter lvwColumnSorter;

        int selectedVSColor = ARANFunctions.RGB(128, 0, 256);

        private List<int> drawElemList;

        public Report(MainForm mForm)
        {
            InitializeComponent();
            myMF = mForm;
            lvwColumnSorter = new ListViewColumnSorter();
            drawElemList = new List<int>();
            this.lstVw_VisualFeatures.ListViewItemSorter = lvwColumnSorter;
            this.lstVw_CirclingAreaObstacles.ListViewItemSorter = lvwColumnSorter;
            this.lstVw_LeftCircuitObstacles.ListViewItemSorter = lvwColumnSorter;
            this.lstVw_RightCircuitObstacles.ListViewItemSorter = lvwColumnSorter;
            this.lstVw_TrackObstacles.ListViewItemSorter = lvwColumnSorter;
            lbl_listItemsCount.Text = "Count: " + lstVw_VisualFeatures.Items.Count.ToString();

            lstVw_CirclingAreaObstacles.Columns[1].Text = "Elev. (" + GlobalVars.settings.HeightUnit + ")";
            lstVw_CirclingAreaObstacles.Columns[2].Text = "MOC (" + GlobalVars.settings.HeightUnit + ")";
            lstVw_CirclingAreaObstacles.Columns[3].Text = "Req. Elev. (" + GlobalVars.settings.HeightUnit + ")";

            lstVw_LeftCircuitObstacles.Columns[1].Text = "Elev. (" + GlobalVars.settings.HeightUnit + ")";
            lstVw_LeftCircuitObstacles.Columns[2].Text = "MOC (" + GlobalVars.settings.HeightUnit + ")";
            lstVw_LeftCircuitObstacles.Columns[3].Text = "Req. Elev. (" + GlobalVars.settings.HeightUnit + ")";

            lstVw_RightCircuitObstacles.Columns[1].Text = "Elev. (" + GlobalVars.settings.HeightUnit + ")";
            lstVw_RightCircuitObstacles.Columns[2].Text = "MOC (" + GlobalVars.settings.HeightUnit + ")";
            lstVw_RightCircuitObstacles.Columns[3].Text = "Req. Elev. (" + GlobalVars.settings.HeightUnit + ")";

            lstVw_TrackObstacles.Columns[2].Text = "Elev. (" + GlobalVars.settings.HeightUnit + ")";
            lstVw_TrackObstacles.Columns[3].Text = "MOC (" + GlobalVars.settings.HeightUnit + ")";
            lstVw_TrackObstacles.Columns[4].Text = "Req. Elev. (" + GlobalVars.settings.HeightUnit + ")";

            lbl_SegmentLength.Text = "Length" + System.Environment.NewLine + GlobalVars.unitConverter.DistanceUnit;
            lbl_SegmentMinFlightAltitude.Text = "Min. Flight Altitude" + System.Environment.NewLine + GlobalVars.unitConverter.HeightUnit;
        }

        public void FillPageVisualFeatures(List<VM_VisualFeature> VisualFeatures)
        {
            System.Windows.Forms.ListViewItem itmX;
            lstVw_VisualFeatures.Items.Clear();

            for (int i = 0; i < VisualFeatures.Count; i++)
            {
                itmX = lstVw_VisualFeatures.Items.Add(VisualFeatures[i].Name);
                //itmX.SubItems.Insert(1, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, VisualFeatures[i].Type));
                itmX.SubItems.Insert(1, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, VisualFeatures[i].Description));
                itmX.Tag = VisualFeatures[i];
            }
        }

        public void FillPageCirclingAreaObstacles(List<VM_VerticalStructure> CirclingAreaObstacles)
        {
            ListViewItem itmX;
            lstVw_CirclingAreaObstacles.Items.Clear();
            foreach(var vsItem in CirclingAreaObstacles)
            {
                itmX = lstVw_CirclingAreaObstacles.Items.Add(vsItem.VerticalStructure.Name);
                itmX.Tag = vsItem;
                itmX.SubItems.Insert(1, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, GlobalVars.unitConverter.HeightToDisplayUnits(vsItem.Elevation, eRoundMode.NERAEST).ToString()));
                itmX.SubItems.Insert(2, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, GlobalVars.unitConverter.HeightToDisplayUnits(VMManager.Instance.MOC, eRoundMode.NERAEST).ToString()));
                itmX.SubItems.Insert(3, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, GlobalVars.unitConverter.HeightToDisplayUnits(vsItem.Elevation + VMManager.Instance.MOC, eRoundMode.NERAEST).ToString()));
            }
        }

        public void FillPageLeftCircuitObstacles(List<VM_VerticalStructure> LeftCircuitObstacles)
        {
            System.Windows.Forms.ListViewItem itmX;
            lstVw_LeftCircuitObstacles.Items.Clear();

            foreach(var vsItem in LeftCircuitObstacles)
            {
                itmX = lstVw_LeftCircuitObstacles.Items.Add(vsItem.VerticalStructure.Name);
                itmX.Tag = vsItem;
                itmX.SubItems.Insert(1, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, GlobalVars.unitConverter.HeightToDisplayUnits(vsItem.Elevation, eRoundMode.NERAEST).ToString()));
                itmX.SubItems.Insert(2, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, GlobalVars.unitConverter.HeightToDisplayUnits(VMManager.Instance.MOC, eRoundMode.NERAEST).ToString()));
                itmX.SubItems.Insert(3, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, GlobalVars.unitConverter.HeightToDisplayUnits(vsItem.Elevation + VMManager.Instance.MOC, eRoundMode.NERAEST).ToString()));
            }
        }

        public void FillPageRightCircuitObstacles(List<VM_VerticalStructure> RightCircuitObstacles)
        {
            System.Windows.Forms.ListViewItem itmX;
            lstVw_RightCircuitObstacles.Items.Clear();

            foreach(var vsItem in RightCircuitObstacles)
            {
                itmX = lstVw_RightCircuitObstacles.Items.Add(vsItem.VerticalStructure.Name);
                itmX.Tag = vsItem;
                itmX.SubItems.Insert(1, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, GlobalVars.unitConverter.HeightToDisplayUnits(vsItem.Elevation, eRoundMode.NERAEST).ToString()));
                itmX.SubItems.Insert(2, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, GlobalVars.unitConverter.HeightToDisplayUnits(VMManager.Instance.MOC, eRoundMode.NERAEST).ToString()));
                itmX.SubItems.Insert(3, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, GlobalVars.unitConverter.HeightToDisplayUnits(vsItem.Elevation + VMManager.Instance.MOC, eRoundMode.NERAEST).ToString()));
            }
        }

        public void FillPageTrackSteps(System.ComponentModel.BindingList<VM_TrackSegment> TrackSteps)
        {
            /*System.Windows.Forms.ListViewItem itmX;
            lstVw_TrackSteps.Items.Clear();      

            foreach (var tsItem in TrackSteps)
            {
                itmX = lstVw_TrackSteps.Items.Add(tsItem.Name);
                itmX.SubItems.Insert(1, new ListViewItem.ListViewSubItem(null, tsItem.InitialDirectionAzt + "°"));
                itmX.SubItems.Insert(2, new ListViewItem.ListViewSubItem(null, tsItem.IntermediateDirectionAzt + "°"));
                itmX.SubItems.Insert(3, new ListViewItem.ListViewSubItem(null, tsItem.FinalDirectionAzt + "°"));
                itmX.SubItems.Insert(4, new ListViewItem.ListViewSubItem(null, tsItem.Description));
                itmX.Tag = tsItem;
            }*/

            flwLytPnl_trackSegments.Controls.Clear();

            foreach (var tsItem in TrackSteps)
            {
                SegmentDetailsControl control = new SegmentDetailsControl();
                control.Name = tsItem.Name;
                control.Length = tsItem.Length_GUI;
                control.InitialDirection = (tsItem.InitialDirectionAzt - GlobalVars.CurrADHP.MagVar);
                control.IntermediateDirection = (tsItem.IntermediateDirectionAzt - GlobalVars.CurrADHP.MagVar);
                control.FinalDirection = (tsItem.FinalDirectionAzt - GlobalVars.CurrADHP.MagVar);
                control.FlightAltitude = tsItem.FlightAltitude_GUI;
                control.Description = tsItem.Description;
                flwLytPnl_trackSegments.Controls.Add(control);
            }

        }        

        public void FillPageTrackObstacles(List<VM_VerticalStructure> TrackObstacles)
        {
            System.Windows.Forms.ListViewItem itmX;
            lstVw_TrackObstacles.Items.Clear();

            foreach(var vsItem in TrackObstacles)
            {
                itmX = lstVw_TrackObstacles.Items.Add(vsItem.VerticalStructure.Name);
                itmX.Tag = vsItem;
                itmX.SubItems.Insert(1, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, vsItem.StepNumber.ToString()));
                itmX.SubItems.Insert(2, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, GlobalVars.unitConverter.HeightToDisplayUnits(vsItem.Elevation, eRoundMode.NERAEST).ToString()));
                itmX.SubItems.Insert(3, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, GlobalVars.unitConverter.HeightToDisplayUnits(VMManager.Instance.MOC, eRoundMode.NERAEST).ToString()));
                itmX.SubItems.Insert(4, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, GlobalVars.unitConverter.HeightToDisplayUnits(vsItem.Elevation + VMManager.Instance.MOC, eRoundMode.NERAEST).ToString()));
            }
        }

        

        public void FillPageTrackResults(List<ParameterObject> ParameterObjects)
        {
            System.Windows.Forms.ListViewItem itmX;
            lstVw_TrackResults.Items.Clear();

            foreach (var poItem in ParameterObjects)
            {
                itmX = lstVw_TrackResults.Items.Add(poItem.property);
                switch (poItem.uom)
                {
                    case UoM.Distance:
                        itmX.SubItems.Insert(1, new ListViewItem.ListViewSubItem(null, Math.Round(GlobalVars.unitConverter.DistanceToDisplayUnits(poItem.doubleValue, eRoundMode.NONE), 3).ToString()));
                        itmX.SubItems.Insert(2, new ListViewItem.ListViewSubItem(null, GlobalVars.unitConverter.DistanceUnit));
                        break;
                    case UoM.Speed:
                        itmX.SubItems.Insert(1, new ListViewItem.ListViewSubItem(null, GlobalVars.unitConverter.SpeedToDisplayUnits(poItem.doubleValue, eRoundMode.NERAEST).ToString()));
                        itmX.SubItems.Insert(2, new ListViewItem.ListViewSubItem(null, GlobalVars.unitConverter.SpeedUnit));
                        break;
                    case UoM.Height:
                        itmX.SubItems.Insert(1, new ListViewItem.ListViewSubItem(null, GlobalVars.unitConverter.HeightToDisplayUnits(poItem.doubleValue, eRoundMode.CEIL).ToString()));
                        itmX.SubItems.Insert(2, new ListViewItem.ListViewSubItem(null, GlobalVars.unitConverter.HeightUnit));
                        break;
                    case UoM.Angle:
                        itmX.SubItems.Insert(1, new ListViewItem.ListViewSubItem(null, Math.Round(poItem.doubleValue, 0).ToString()));
                        itmX.SubItems.Insert(2, new ListViewItem.ListViewSubItem(null, "°"));
                        break;
                    case UoM.RateOfDescent:
                        itmX.SubItems.Insert(1, new ListViewItem.ListViewSubItem(null, GlobalVars.unitConverter.HeightToDisplayUnits(poItem.doubleValue, eRoundMode.NERAEST).ToString()));
                        itmX.SubItems.Insert(2, new ListViewItem.ListViewSubItem(null, GlobalVars.unitConverter.HeightUnit + "/min"));
                        break;
                    case UoM.Time:
                        itmX.SubItems.Insert(1, new ListViewItem.ListViewSubItem(null, Math.Round(poItem.doubleValue, 0).ToString()));
                        itmX.SubItems.Insert(2, new ListViewItem.ListViewSubItem(null, "s"));
                        break;
                    case UoM.Gradient:                        
                        itmX.SubItems.Insert(1, new ListViewItem.ListViewSubItem(null, Math.Round(poItem.doubleValue, 1).ToString()));
                        itmX.SubItems.Insert(2, new ListViewItem.ListViewSubItem(null, "%"));
                        if (poItem.doubleValue > VMManager.Instance.MaxDescentGradient)
                        {
                            itmX.Font = new System.Drawing.Font(itmX.Font, System.Drawing.FontStyle.Bold);
                            itmX.ForeColor = System.Drawing.Color.FromArgb(255, 0, 0);
                        }
                        break;
                    case UoM.Empty:
                        if (!poItem.stringValue.Equals(""))
                            itmX.SubItems.Insert(1, new ListViewItem.ListViewSubItem(null, poItem.stringValue));
                        else
                            itmX.SubItems.Insert(1, new ListViewItem.ListViewSubItem(null, poItem.doubleValue.ToString()));
                        itmX.SubItems.Insert(2, new ListViewItem.ListViewSubItem(null, ""));
                        break;
                    default:
                        itmX.SubItems.Insert(1, new ListViewItem.ListViewSubItem(null, poItem.doubleValue.ToString()));
                        break;
                }

                if (!poItem.message.Equals(""))
                {
                    itmX.SubItems.Insert(3, new ListViewItem.ListViewSubItem(null, poItem.message));
                    if (poItem.messageType == MessageType.errorMsg)
                    {
                        itmX.Font = new System.Drawing.Font(itmX.Font, System.Drawing.FontStyle.Bold);
                        itmX.ForeColor = System.Drawing.Color.FromArgb(255, 0, 0);
                    }
                }
            }
        }

        private void lstVw_VisualFeatures_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstVw_VisualFeatures.SelectedItems.Count == 0)
                return;

            var lwItem = lstVw_VisualFeatures.SelectedItems[0];
            var visualFeature = lwItem.Tag as VM_VisualFeature;

            if (!Visible)
                return;

            for (int i = 0; i < drawElemList.Count; i++)
            {
                GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(drawElemList[i]);
            }
            drawElemList = new List<int>();
            drawElemList.Add(GlobalVars.gAranEnv.Graphics.DrawPointWithText(visualFeature.pShape, 255, visualFeature.Name));
        }

        private void lstVw_CirclingAreaObstacles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstVw_CirclingAreaObstacles.SelectedItems.Count == 0)
                return;

            var lwItem = lstVw_CirclingAreaObstacles.SelectedItems[0];
            var vs = lwItem.Tag as VM_VerticalStructure;
            if (!Visible)
                return;

            for (int i = 0; i < drawElemList.Count; i++)
            {
                GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(drawElemList[i]);
            }
            drawElemList = new List<int>();            

            if (vs != null)
            {
                foreach (var vsPart in vs.VerticalStructure.Part)
                {
                    if (vsPart != null)
                    {
                        var horProjection = vsPart.HorizontalProjection;
                        if (horProjection != null)
                        {
                            switch (horProjection.Choice)
                            {
                                case Aran.Aim.VerticalStructurePartGeometryChoice.ElevatedPoint:
                                    if (vs.VerticalStructure.Name != null)
                                        drawElemList.Add(GlobalVars.gAranEnv.Graphics.DrawPointWithText(GlobalVars.pspatialReferenceOperation.ToPrj<Point>(horProjection.Location.Geo), selectedVSColor, vs.VerticalStructure.Name.ToString()));
                                    else
                                        drawElemList.Add(GlobalVars.gAranEnv.Graphics.DrawPointWithText(GlobalVars.pspatialReferenceOperation.ToPrj<Point>(horProjection.Location.Geo), selectedVSColor, "null"));
                                    break;
                                case Aran.Aim.VerticalStructurePartGeometryChoice.ElevatedCurve:
                                    drawElemList.Add(GlobalVars.gAranEnv.Graphics.DrawMultiLineString(GlobalVars.pspatialReferenceOperation.ToPrj<MultiLineString>(horProjection.LinearExtent.Geo), selectedVSColor, 2));
                                    drawElemList.Add(GlobalVars.gAranEnv.Graphics.DrawPointWithText(GlobalVars.pspatialReferenceOperation.ToPrj<Point>(horProjection.LinearExtent.Geo.Centroid), selectedVSColor, vs.VerticalStructure.Name));
                                    break;
                                case Aran.Aim.VerticalStructurePartGeometryChoice.ElevatedSurface:
                                    drawElemList.Add(GlobalVars.gAranEnv.Graphics.DrawMultiPolygon(GlobalVars.pspatialReferenceOperation.ToPrj<MultiPolygon>(horProjection.SurfaceExtent.Geo), selectedVSColor, AranEnvironment.Symbols.eFillStyle.sfsCross));
                                    drawElemList.Add(GlobalVars.gAranEnv.Graphics.DrawPointWithText(GlobalVars.pspatialReferenceOperation.ToPrj<Point>(horProjection.SurfaceExtent.Geo.Centroid), selectedVSColor, vs.VerticalStructure.Name));
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private void lstVw_LeftCircuitObstacles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstVw_LeftCircuitObstacles.SelectedItems.Count == 0) return;

            var lwItem = lstVw_LeftCircuitObstacles.SelectedItems[0];
            var vs = lwItem.Tag as VM_VerticalStructure;

            if (!Visible)
                return;
            
            for (int i = 0; i < drawElemList.Count; i++)
            {
                GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(drawElemList[i]);
            }
            drawElemList = new List<int>();

            if (vs != null)
            {
                foreach (var vsPart in vs.VerticalStructure.Part)
                {
                    if (vsPart != null)
                    {
                        var horProjection = vsPart.HorizontalProjection;
                        if (horProjection != null)
                        {
                            switch (horProjection.Choice)
                            {
                                case Aran.Aim.VerticalStructurePartGeometryChoice.ElevatedPoint:
                                    if (vs.VerticalStructure.Name != null)
                                        drawElemList.Add(GlobalVars.gAranEnv.Graphics.DrawPointWithText(GlobalVars.pspatialReferenceOperation.ToPrj<Point>(horProjection.Location.Geo), selectedVSColor, vs.VerticalStructure.Name.ToString()));
                                    else
                                        drawElemList.Add(GlobalVars.gAranEnv.Graphics.DrawPointWithText(GlobalVars.pspatialReferenceOperation.ToPrj<Point>(horProjection.Location.Geo), selectedVSColor, "null"));
                                    break;
                                case Aran.Aim.VerticalStructurePartGeometryChoice.ElevatedCurve:
                                    drawElemList.Add(GlobalVars.gAranEnv.Graphics.DrawMultiLineString(GlobalVars.pspatialReferenceOperation.ToPrj<MultiLineString>(horProjection.LinearExtent.Geo), selectedVSColor, 2));
                                    drawElemList.Add(GlobalVars.gAranEnv.Graphics.DrawPointWithText(GlobalVars.pspatialReferenceOperation.ToPrj<Point>(horProjection.LinearExtent.Geo.Centroid), selectedVSColor, vs.VerticalStructure.Name));
                                    break;
                                case Aran.Aim.VerticalStructurePartGeometryChoice.ElevatedSurface:
                                    drawElemList.Add(GlobalVars.gAranEnv.Graphics.DrawMultiPolygon(GlobalVars.pspatialReferenceOperation.ToPrj<MultiPolygon>(horProjection.SurfaceExtent.Geo), selectedVSColor, AranEnvironment.Symbols.eFillStyle.sfsCross));
                                    drawElemList.Add(GlobalVars.gAranEnv.Graphics.DrawPointWithText(GlobalVars.pspatialReferenceOperation.ToPrj<Point>(horProjection.SurfaceExtent.Geo.Centroid), selectedVSColor, vs.VerticalStructure.Name));
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private void lstVw_RightCircuitObstacles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstVw_RightCircuitObstacles.SelectedItems.Count == 0) return;

            var lwItem = lstVw_RightCircuitObstacles.SelectedItems[0];
            var vs = lwItem.Tag as VM_VerticalStructure;

            if (!Visible)
                return;            

            for (int i = 0; i < drawElemList.Count; i++)
            {
                GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(drawElemList[i]);
            }
            drawElemList = new List<int>();

            if (vs != null)
            {
                foreach (var vsPart in vs.VerticalStructure.Part)
                {
                    if (vsPart != null)
                    {
                        var horProjection = vsPart.HorizontalProjection;
                        if (horProjection != null)
                        {
                            switch (horProjection.Choice)
                            {
                                case Aran.Aim.VerticalStructurePartGeometryChoice.ElevatedPoint:
                                    if (vs.VerticalStructure.Name != null)
                                        drawElemList.Add(GlobalVars.gAranEnv.Graphics.DrawPointWithText(GlobalVars.pspatialReferenceOperation.ToPrj<Point>(horProjection.Location.Geo), selectedVSColor, vs.VerticalStructure.Name.ToString()));
                                    else
                                        drawElemList.Add(GlobalVars.gAranEnv.Graphics.DrawPointWithText(GlobalVars.pspatialReferenceOperation.ToPrj<Point>(horProjection.Location.Geo), selectedVSColor, "null"));
                                    break;
                                case Aran.Aim.VerticalStructurePartGeometryChoice.ElevatedCurve:
                                    drawElemList.Add(GlobalVars.gAranEnv.Graphics.DrawMultiLineString(GlobalVars.pspatialReferenceOperation.ToPrj<MultiLineString>(horProjection.LinearExtent.Geo), selectedVSColor, 2));
                                    drawElemList.Add(GlobalVars.gAranEnv.Graphics.DrawPointWithText(GlobalVars.pspatialReferenceOperation.ToPrj<Point>(horProjection.LinearExtent.Geo.Centroid), selectedVSColor, vs.VerticalStructure.Name));
                                    break;
                                case Aran.Aim.VerticalStructurePartGeometryChoice.ElevatedSurface:
                                    drawElemList.Add(GlobalVars.gAranEnv.Graphics.DrawMultiPolygon(GlobalVars.pspatialReferenceOperation.ToPrj<MultiPolygon>(horProjection.SurfaceExtent.Geo), selectedVSColor, AranEnvironment.Symbols.eFillStyle.sfsCross));
                                    drawElemList.Add(GlobalVars.gAranEnv.Graphics.DrawPointWithText(GlobalVars.pspatialReferenceOperation.ToPrj<Point>(horProjection.SurfaceExtent.Geo.Centroid), selectedVSColor, vs.VerticalStructure.Name));
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private void lstVw_TrackSteps_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*if (lstVw_TrackSteps.SelectedItems.Count == 0) return;

            var lwItem = lstVw_TrackSteps.SelectedItems[0];
            var trackStep = lwItem.Tag as VM_TrackSegment;

            if (!Visible)
                return;

            for (int i = 0; i < drawElemList.Count; i++)
            {
                GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(drawElemList[i]);
            }
            drawElemList = new List<int>();
            drawElemList.Add(GlobalVars.gAranEnv.Graphics.DrawLineString(trackStep.Centreline, selectedVSColor, 2));*/
        }

        private void lstVw_TrackObstacles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstVw_TrackObstacles.SelectedItems.Count == 0) return;

            var lwItem = lstVw_TrackObstacles.SelectedItems[0];
            var vs = lwItem.Tag as VM_VerticalStructure;

            if (!Visible)
                return;

            for (int i = 0; i < drawElemList.Count; i++)
            {
                GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(drawElemList[i]);
            }
            drawElemList = new List<int>();

            if (vs != null)
            {
                foreach (var vsPart in vs.VerticalStructure.Part)
                {
                    if (vsPart != null)
                    {
                        var horProjection = vsPart.HorizontalProjection;
                        if (horProjection != null)
                        {
                            switch (horProjection.Choice)
                            {
                                case Aran.Aim.VerticalStructurePartGeometryChoice.ElevatedPoint:
                                    if (vs.VerticalStructure.Name != null)
                                        drawElemList.Add(GlobalVars.gAranEnv.Graphics.DrawPointWithText(GlobalVars.pspatialReferenceOperation.ToPrj<Point>(horProjection.Location.Geo), selectedVSColor, vs.VerticalStructure.Name.ToString()));
                                    else
                                        drawElemList.Add(GlobalVars.gAranEnv.Graphics.DrawPointWithText(GlobalVars.pspatialReferenceOperation.ToPrj<Point>(horProjection.Location.Geo), selectedVSColor, "null"));
                                    break;
                                case Aran.Aim.VerticalStructurePartGeometryChoice.ElevatedCurve:
                                    drawElemList.Add(GlobalVars.gAranEnv.Graphics.DrawMultiLineString(GlobalVars.pspatialReferenceOperation.ToPrj<MultiLineString>(horProjection.LinearExtent.Geo), selectedVSColor, 2));
                                    drawElemList.Add(GlobalVars.gAranEnv.Graphics.DrawPointWithText(GlobalVars.pspatialReferenceOperation.ToPrj<Point>(horProjection.LinearExtent.Geo.Centroid), selectedVSColor, vs.VerticalStructure.Name));
                                    break;
                                case Aran.Aim.VerticalStructurePartGeometryChoice.ElevatedSurface:
                                    drawElemList.Add(GlobalVars.gAranEnv.Graphics.DrawMultiPolygon(GlobalVars.pspatialReferenceOperation.ToPrj<MultiPolygon>(horProjection.SurfaceExtent.Geo), selectedVSColor, AranEnvironment.Symbols.eFillStyle.sfsCross));
                                    drawElemList.Add(GlobalVars.gAranEnv.Graphics.DrawPointWithText(GlobalVars.pspatialReferenceOperation.ToPrj<Point>(horProjection.SurfaceExtent.Geo.Centroid), selectedVSColor, vs.VerticalStructure.Name));
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private void Report_FormClosed(object sender, FormClosedEventArgs e)
        {
            for (int i = 0; i < drawElemList.Count; i++)
            {
                GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(drawElemList[i]);
            }

            myMF.btn_Report.Checked = false;
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {

        }

        private void tbCntrl_Report_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < drawElemList.Count; i++)
            {
                GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(drawElemList[i]);
            }

            lbl_listItemsCount.Visible = true;
            switch (tbCntrl_Report.SelectedIndex)
            {
                case 0:                    
                    lbl_listItemsCount.Text = "Count: " + lstVw_VisualFeatures.Items.Count.ToString();
                    break;
                case 1:
                    lbl_listItemsCount.Text = "Count: " + lstVw_CirclingAreaObstacles.Items.Count.ToString();
                    break;
                case 2:
                    lbl_listItemsCount.Text = "Count: " + lstVw_LeftCircuitObstacles.Items.Count.ToString();
                    break;
                case 3:
                    lbl_listItemsCount.Text = "Count: " + lstVw_RightCircuitObstacles.Items.Count.ToString();
                    break;
                case 4:
                    lbl_listItemsCount.Text = "Count: " + flwLytPnl_trackSegments.Controls.Count.ToString();
                    break;
                case 5:
                    lbl_listItemsCount.Text = "Count: " + lstVw_TrackObstacles.Items.Count.ToString();
                    break;
                case 6:
                    lbl_listItemsCount.Visible = false;
                    break;
            }
            
        }

        private void lstVw_VisualFeatures_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == 2) //Description column is not sortable
                return;
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            this.lstVw_VisualFeatures.Sort();
        }

        private void lstVw_CirclingAreaObstacles_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            this.lstVw_CirclingAreaObstacles.Sort();
        }

        private void lstVw_LeftCircuitObstacles_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            this.lstVw_LeftCircuitObstacles.Sort();
        }

        private void lstVw_RightCircuitObstacles_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            this.lstVw_RightCircuitObstacles.Sort();
        }

        private void lstVw_TrackObstacles_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            this.lstVw_TrackObstacles.Sort();
        }
        
    }
}
