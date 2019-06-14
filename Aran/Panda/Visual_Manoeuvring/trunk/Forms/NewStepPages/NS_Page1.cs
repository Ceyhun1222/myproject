using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Aran.AranEnvironment;
using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.Panda.Common;

namespace Aran.Panda.VisualManoeuvring.Forms
{
    public partial class NS_Page1 : UserControl
    {
        private int initialDirectionElement = -1;
        private FormHelpers.NS_Page1_Helper _pageHelper;
        #region Private Vars        

        enum Cat { A = 0, B = 1, C = 2, D = 3, E = 5 };
        AranTool pickInitialPnt;
        String[] LonSubFix = new String[] { "E", "W" };
        String[] LatSubFix = new String[] { "N", "S" };

        Point initialPntPrj;
        Point initialPntGeo;

        double initialPntX;
        double initialPntY;
        #endregion

        public NS_Page1()
        {
            InitializeComponent();
            _pageHelper = new FormHelpers.NS_Page1_Helper();
            pickInitialPnt = new AranTool();
            pickInitialPnt.Visible = true;
            pickInitialPnt.Cursor = Cursors.Cross;
            pickInitialPnt.MouseClickedOnMap += new MouseClickedOnMapEventHandler(pickInitialPnt_Click);
            GlobalVars.gAranEnv.AranUI.AddMapTool(pickInitialPnt);
            cmbBox_initialPntLatSide.Items.Add(LatSubFix[0]);
            cmbBox_initialPntLatSide.Items.Add(LatSubFix[1]);
            cmbBox_initialPntLonSide.Items.Add(LonSubFix[0]);
            cmbBox_initialPntLonSide.Items.Add(LonSubFix[1]);
            txtBox_initialDirection.Enabled = false;
            if (VMManager.Instance.TrackStepList != null /*&& VMManager.Instance.TrackStepList.Count > 0*/)
            {
                //if (VMManager.Instance.TrackStepList.Count > 0)
                //{
                    cmbBox_initialPntLatSide.Enabled = false;
                    cmbBox_initialPntLonSide.Enabled = false;
                    btn_pickInitialPnt.Enabled = false;
                    showCurrentPositionAndDirection();
                //}
            }
            txtBox_maxDivergenceAngle.Text = "45";
            txtBox_maxConvergenceAngle.Text = "90";
            VMManager.Instance.isFinalStep = false;
        }

        private void showCurrentPositionAndDirection()
        {
            if (VMManager.Instance.TrackStepList.Count == 0)
            {
                VMManager.Instance.InitialDirection = GlobalVars.pspatialReferenceOperation.AztToDirPrj(VMManager.Instance.FinalNavaid.pPtPrj, ARANMath.RadToDeg(VMManager.Instance.TrueBRGAngle));
                if (VMManager.Instance.InitialDirection < 0)
                    VMManager.Instance.InitialDirection = ARANMath.C_2xPI + VMManager.Instance.InitialDirection;
                double _distToPoly;
                Line line;
                GeometryOperators geomOper = new GeometryOperators();
                geomOper.CurrentGeometry = VMManager.Instance.ConvexPoly;
                if (geomOper.Contains(VMManager.Instance.FinalNavaid.pPtPrj))
                {                    
                    VMManager.Instance.InitialPosition = ARANFunctions.PolygonVectorIntersect(VMManager.Instance.ConvexPoly[0], VMManager.Instance.FinalNavaid.pPtPrj,
                            ARANMath.Modulus(VMManager.Instance.InitialDirection - ARANMath.C_PI, ARANMath.C_2xPI), out _distToPoly, true);
                }
                else
                {
                    line = new Line(VMManager.Instance.FinalNavaid.pPtPrj, VMManager.Instance.InitialDirection);
                    VMManager.Instance.InitialPosition = ARANFunctions.PolygonVectorIntersect(VMManager.Instance.ConvexPoly[0], line, out _distToPoly, true);
                    if (VMManager.Instance.InitialPosition == null)
                    {
                        VMManager.Instance.InitialDirection = ARANMath.Modulus(VMManager.Instance.InitialDirection - ARANMath.C_PI, ARANMath.C_2xPI);
                        line = new Line(VMManager.Instance.FinalNavaid.pPtPrj, VMManager.Instance.InitialDirection);
                        VMManager.Instance.InitialPosition = ARANFunctions.PolygonVectorIntersect(VMManager.Instance.ConvexPoly[0], line, out _distToPoly, true);
                    }
                }
            }
            else
            {
                VMManager.Instance.InitialPosition = VMManager.Instance.TrackStepList[VMManager.Instance.TrackStepList.Count - 1].EndPointPrj;
                VMManager.Instance.InitialDirection = VMManager.Instance.TrackStepList[VMManager.Instance.TrackStepList.Count - 1].FinalDirectionDir;
            }
            
            double xDeg;
            double xMin;
            double xSec;

            double yDeg;
            double yMin;
            double ySec;

            initialPntGeo = GlobalVars.pspatialReferenceOperation.ToGeo<Point>(VMManager.Instance.InitialPosition);

            Functions.DD2DMS(initialPntGeo.X, out xDeg, out xMin, out xSec, System.Math.Sign(initialPntGeo.X));
            Functions.DD2DMS(initialPntGeo.Y, out yDeg, out yMin, out ySec, System.Math.Sign(initialPntGeo.Y));

            txtBox_initialPntLatDegree.Text = yDeg.ToString();
            txtBox_initialPntLatMinute.Text = yMin.ToString();
            txtBox_initialPntLatSecond.Text = System.Math.Round(ySec, 2).ToString();
            if (System.Math.Sign(initialPntGeo.Y) < 0)
                cmbBox_initialPntLatSide.SelectedIndex = 1;
            else
                cmbBox_initialPntLatSide.SelectedIndex = 0;

            txtBox_initialPntLonDegree.Text = xDeg.ToString();
            txtBox_initialPntLonMinute.Text = xMin.ToString();
            txtBox_initialPntLonSecond.Text = System.Math.Round(xSec, 2).ToString();

            if (System.Math.Sign(initialPntGeo.X) < 0)
                cmbBox_initialPntLonSide.SelectedIndex = 1;
            else
                cmbBox_initialPntLonSide.SelectedIndex = 0;

            if (VMManager.Instance.InitialPositionElement > -1)
            {
                GlobalVars.gAranEnv.Graphics.DeleteGraphic(VMManager.Instance.InitialPositionElement);
                VMManager.Instance.InitialPositionElement = -1;
            }
            VMManager.Instance.InitialPositionElement = GlobalVars.gAranEnv.Graphics.DrawPoint(VMManager.Instance.InitialPosition, 255);

            txtBox_initialDirection.Text = Math.Round(GlobalVars.pspatialReferenceOperation.DirToAztPrj(VMManager.Instance.InitialPosition, VMManager.Instance.InitialDirection)).ToString();
        }

        private void btn_pickInitialPnt_Click(object sender, EventArgs e)
        {
            GlobalVars.gAranEnv.AranUI.SetCurrentTool(this.pickInitialPnt);
        }

        private void pickInitialPnt_Click(object sender, MapMouseEventArg args)
        {
            if (!this.Visible)
                return;
            double xDeg;
            double xMin;
            double xSec;

            double yDeg;
            double yMin;
            double ySec;

            GeometryOperators geomOper = new GeometryOperators();

            initialPntX = args.X;
            initialPntY = args.Y;
            initialPntPrj = new Point();

            initialPntPrj.X = initialPntX;
            initialPntPrj.Y = initialPntY;

            initialPntGeo = GlobalVars.pspatialReferenceOperation.ToGeo<Point>(initialPntPrj);
            initialPntX = initialPntGeo.X;
            initialPntY = initialPntGeo.Y;

            Functions.DD2DMS(initialPntX, out xDeg, out xMin, out xSec, System.Math.Sign(initialPntX));
            Functions.DD2DMS(initialPntY, out yDeg, out yMin, out ySec, System.Math.Sign(initialPntY));

            txtBox_initialPntLatDegree.Text = yDeg.ToString();
            txtBox_initialPntLatMinute.Text = yMin.ToString();
            txtBox_initialPntLatSecond.Text = System.Math.Round(ySec, 2).ToString();
            if (System.Math.Sign(initialPntY) < 0)
                cmbBox_initialPntLatSide.SelectedIndex = 1;
            else
                cmbBox_initialPntLatSide.SelectedIndex = 0;

            txtBox_initialPntLonDegree.Text = xDeg.ToString();
            txtBox_initialPntLonMinute.Text = xMin.ToString();
            txtBox_initialPntLonSecond.Text = System.Math.Round(xSec, 2).ToString();

            if (System.Math.Sign(initialPntX) < 0)
                cmbBox_initialPntLonSide.SelectedIndex = 1;
            else
                cmbBox_initialPntLonSide.SelectedIndex = 0;

            VMManager.Instance.InitialPosition = initialPntPrj;
            if (VMManager.Instance.InitialPositionElement > -1)
            {
                GlobalVars.gAranEnv.Graphics.DeleteGraphic(VMManager.Instance.InitialPositionElement);
                VMManager.Instance.InitialPositionElement = -1;
            }

            VMManager.Instance.InitialPositionElement = GlobalVars.gAranEnv.Graphics.DrawPoint(VMManager.Instance.InitialPosition, 255);
            txtBox_initialDirection.Enabled = true;
            txtBox_initialDirection.Text = "0";
        }

        private void Page1_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
            }
            else
            {
                GlobalVars.gAranEnv.Graphics.SetMapTool(null);
                if (initialDirectionElement > -1)
                    GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(initialDirectionElement);
                initialDirectionElement = -1;
            }
        }

        private void txtBox_initialDirection_TextChanged(object sender, EventArgs e)
        {
            if (txtBox_initialDirection.Text.Equals(""))
                VMManager.Instance.InitialDirection = 0;
            else
            {
                VMManager.Instance.InitialDirection = GlobalVars.pspatialReferenceOperation.AztToDirPrj(VMManager.Instance.InitialPosition, double.Parse(txtBox_initialDirection.Text));
                if (VMManager.Instance.InitialDirection < 0)
                    VMManager.Instance.InitialDirection = ARANMath.C_2xPI + VMManager.Instance.InitialDirection;

                LineString ls = new LineString();
                ls.Add(VMManager.Instance.InitialPosition);
                ls.Add(ARANFunctions.PointAlongPlane(VMManager.Instance.InitialPosition, VMManager.Instance.InitialDirection, 1500));
                ls.Add(ARANFunctions.PointAlongPlane(ls[1], ARANMath.Modulus(VMManager.Instance.InitialDirection + ARANMath.C_PI_2, ARANMath.C_2xPI), 150));
                ls.Add(ARANFunctions.PointAlongPlane(VMManager.Instance.InitialPosition, VMManager.Instance.InitialDirection, 2000));
                ls.Add(ARANFunctions.PointAlongPlane(ls[1], ARANMath.Modulus(VMManager.Instance.InitialDirection - ARANMath.C_PI_2, ARANMath.C_2xPI), 150));
                ls.Add(ARANFunctions.PointAlongPlane(VMManager.Instance.InitialPosition, VMManager.Instance.InitialDirection, 1500));

                if (initialDirectionElement > -1)
                    GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(initialDirectionElement);
                initialDirectionElement = GlobalVars.gAranEnv.Graphics.DrawLineString(ls, ARANFunctions.RGB(0, 0, 255), 1);
            }
        }

        private void txtBox_initialDirection_KeyPress(object sender, KeyPressEventArgs e)
        {
            int isNumber = 0;
            e.Handled = !(int.TryParse(e.KeyChar.ToString(), out isNumber) || e.KeyChar == '\b');
        }

        private void txtBox_maxDivergenceAngle_TextChanged(object sender, EventArgs e)
        {
            if(txtBox_maxDivergenceAngle.Text.Equals(""))
                txtBox_maxDivergenceAngle.Text = "0";
            else
                VMManager.Instance.MaxDivergenceAngle = ARANMath.DegToRad(double.Parse(txtBox_maxDivergenceAngle.Text));
        }

        private void txtBox_maxDivergenceAngle_KeyPress(object sender, KeyPressEventArgs e)
        {
            int isNumber = 0;
            e.Handled = !(int.TryParse(e.KeyChar.ToString(), out isNumber) || e.KeyChar == '\b');
        }

        private void txtBox_maxConvergenceAngle_TextChanged(object sender, EventArgs e)
        {
            if (txtBox_maxConvergenceAngle.Text.Equals(""))
                txtBox_maxConvergenceAngle.Text = "0";
            else
                VMManager.Instance.MaxConvergenceAngle = ARANMath.DegToRad(double.Parse(txtBox_maxConvergenceAngle.Text));
        }

        private void txtBox_maxConvergenceAngle_KeyPress(object sender, KeyPressEventArgs e)
        {            
            int isNumber = 0;
            e.Handled = !(int.TryParse(e.KeyChar.ToString(), out isNumber) || e.KeyChar == '\b');
        }

        private void chkBox_isFinalStep_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBox_isFinalStep.Checked)
                VMManager.Instance.isFinalStep = true;
            else
                VMManager.Instance.isFinalStep = false;
        }
    }
}
