using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Aran.Panda.Common;

namespace Aran.Panda.VisualManoeuvring.Forms
{
    public partial class NS_Page4 : UserControl
    {
        bool _callEventHandler;
        public int prevPageNumber { get; set; }
        FormHelpers.NS_Page4_Helper pageHelper;
        double maxDirectionAngle;
        double minDirectionAngle;

        public NS_Page4()
        {
            InitializeComponent();
            pageHelper = new FormHelpers.NS_Page4_Helper();            
            VMManager.Instance.ConvergenceLineElements.Add(-1);
        }

        private void cmbBox_VFsWithinPoly_SelectedIndexChanged(object sender, EventArgs e)
        {
            _callEventHandler = false;
            if (cmbBox_VFsWithinPoly.SelectedIndex == 0)
            {
                nmrcUpDown_finalDirection.ReadOnly = false;
                minDirectionAngle = Math.Ceiling(GlobalVars.pspatialReferenceOperation.DirToAztPrj(VMManager.Instance.ConvergenceFlyByPoint, VMManager.Instance.MaxConvergenceDirection));
                maxDirectionAngle = Math.Floor(GlobalVars.pspatialReferenceOperation.DirToAztPrj(VMManager.Instance.ConvergenceFlyByPoint, VMManager.Instance.MinConvergenceDirection));
                if (maxDirectionAngle < minDirectionAngle)
                {
                    maxDirectionAngle += 360;
                    nmrcUpDown_finalDirection.Minimum = -1;
                }
                else
                    nmrcUpDown_finalDirection.Minimum = (decimal)minDirectionAngle;
                nmrcUpDown_finalDirection.Maximum = (decimal)maxDirectionAngle;
                _callEventHandler = true;
                if (nmrcUpDown_finalDirection.Value != (decimal)minDirectionAngle)
                    nmrcUpDown_finalDirection.Value = (decimal)minDirectionAngle;
                else
                    nmrcUpDown_finalDirection_ValueChanged(nmrcUpDown_finalDirection, null);
                lbl_finalDirectionRange.Text = Math.Ceiling(GlobalVars.pspatialReferenceOperation.DirToAztPrj(VMManager.Instance.ConvergenceFlyByPoint, VMManager.Instance.MaxConvergenceDirection)) + " - " + (decimal)Math.Floor(GlobalVars.pspatialReferenceOperation.DirToAztPrj(VMManager.Instance.ConvergenceFlyByPoint, VMManager.Instance.MinConvergenceDirection));
                lbl_finalDirectionRange.Visible = true;
            }
            else
            {
                nmrcUpDown_finalDirection.ReadOnly = true;
                nmrcUpDown_finalDirection.Minimum = (decimal)Math.Round(GlobalVars.pspatialReferenceOperation.DirToAztPrj(VMManager.Instance.ConvergenceFlyByPoint, VMManager.Instance.FinalDirections[cmbBox_VFsWithinPoly.SelectedIndex - 1]));
                nmrcUpDown_finalDirection.Maximum = nmrcUpDown_finalDirection.Minimum;
                _callEventHandler = true;
                if (nmrcUpDown_finalDirection.Value != nmrcUpDown_finalDirection.Minimum)
                    nmrcUpDown_finalDirection.Value = nmrcUpDown_finalDirection.Minimum;
                else
                    nmrcUpDown_finalDirection_ValueChanged(nmrcUpDown_finalDirection, null);
                lbl_finalDirectionRange.Visible = false;
            }

            if (VMManager.Instance.SelectedVFElement > -1)
            {
                GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.SelectedVFElement);
            }
            if(cmbBox_VFsWithinPoly.SelectedIndex > 0)
                VMManager.Instance.SelectedVFElement = GlobalVars.gAranEnv.Graphics.DrawPoint(VMManager.Instance.VFsWithinPoly[cmbBox_VFsWithinPoly.SelectedIndex - 1].pShape, ARANFunctions.RGB(51, 0, 102));
        }

        private void NS_Page4_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                if (prevPageNumber == 3)
                {
                    cmbBox_VFsWithinPoly.DataSource = pageHelper.GetVFsWithinPoly();
                }
            }
            else
            {
                pageHelper.onClose();
            }
            if (VMManager.Instance.SelectedVFElement > -1)
            {
                GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.SelectedVFElement);
                VMManager.Instance.SelectedVFElement = -1;
            }
        }

        private void nmrcUpDown_finalDirection_ValueChanged(object sender, EventArgs e)
        {
            if (!_callEventHandler)
                return;
            double val = (double)nmrcUpDown_finalDirection.Value;
            if (val < minDirectionAngle && val > ARANMath.Modulus(maxDirectionAngle, 360))
            {
                nmrcUpDown_finalDirection.Value = (decimal)minDirectionAngle;
                return;
            }
            else if (val >= 360 || val < 0)
            {
                nmrcUpDown_finalDirection.Value = (decimal)ARANMath.Modulus(val, 360);
                return;
            }

            VMManager.Instance.FinalDirection = ARANMath.Modulus(GlobalVars.pspatialReferenceOperation.AztToDirPrj(VMManager.Instance.ConvergenceFlyByPoint, (double)nmrcUpDown_finalDirection.Value), ARANMath.C_2xPI);
            pageHelper.ConstructConvergenceLine(cmbBox_VFsWithinPoly.SelectedIndex);
            VMManager.Instance.DivergenceAngle = Math.Abs(VMManager.Instance.InitialDirection - VMManager.Instance.IntermediateDirection);
            if (VMManager.Instance.DivergenceAngle > ARANMath.C_PI)
                VMManager.Instance.DivergenceAngle = ARANMath.C_2xPI - VMManager.Instance.DivergenceAngle;
            VMManager.Instance.ConvergenceAngle = Math.Abs(VMManager.Instance.IntermediateDirection - VMManager.Instance.FinalDirection);
            if (VMManager.Instance.ConvergenceAngle > ARANMath.C_PI)
                VMManager.Instance.ConvergenceAngle = ARANMath.C_2xPI - VMManager.Instance.ConvergenceAngle;

            txtBox_divergenceAngle.Text = Math.Round(ARANMath.RadToDeg(VMManager.Instance.DivergenceAngle)).ToString();
            txtBox_convergenceAngle.Text = Math.Round(ARANMath.RadToDeg(VMManager.Instance.ConvergenceAngle)).ToString();
        }
    }
}
