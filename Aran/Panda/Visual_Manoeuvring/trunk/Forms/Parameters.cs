using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Aran.Panda.Common;

namespace Aran.Panda.VisualManoeuvring.Forms
{
    public partial class Parameters : Form
    {
        MainForm mainForm;

        public Parameters(MainForm mf)
        {
            InitializeComponent();
            mainForm = mf;
        }

        public void FillParameterList(List<ParameterObject> ParameterObjects)
        {
            System.Windows.Forms.ListViewItem itmX;
            lstVw_Parameters.Items.Clear();
            int n = ParameterObjects.Count;
            if (n == 0) return;

            for (int i = 0; i < n; i++)
            {
                var poItem = ParameterObjects[i];
                itmX = lstVw_Parameters.Items.Add(poItem.property);
                switch (ParameterObjects[i].uom)
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
                        itmX.SubItems.Insert(1, new ListViewItem.ListViewSubItem(null, GlobalVars.unitConverter.HeightToDisplayUnits(poItem.doubleValue, eRoundMode.NERAEST).ToString()));
                        itmX.SubItems.Insert(2, new ListViewItem.ListViewSubItem(null, GlobalVars.unitConverter.HeightUnit));
                        break;
                    case UoM.Angle:
                        itmX.SubItems.Insert(1, new ListViewItem.ListViewSubItem(null, Math.Round(poItem.doubleValue, 0).ToString() + "°"));
                        itmX.SubItems.Insert(2, new ListViewItem.ListViewSubItem(null, ""));
                        break;
                    case UoM.RateOfDescent:
                        itmX.SubItems.Insert(1, new ListViewItem.ListViewSubItem(null, GlobalVars.unitConverter.HeightToDisplayUnits(poItem.doubleValue, eRoundMode.NERAEST).ToString()));
                        itmX.SubItems.Insert(2, new ListViewItem.ListViewSubItem(null, GlobalVars.unitConverter.HeightUnit + "/min"));
                        break;
                    case UoM.Time:
                        itmX.SubItems.Insert(1, new ListViewItem.ListViewSubItem(null, poItem.doubleValue.ToString()));
                        itmX.SubItems.Insert(2, new ListViewItem.ListViewSubItem(null, "s"));
                        break;
                    case UoM.Gradient:
                        itmX.SubItems.Insert(1, new ListViewItem.ListViewSubItem(null, Math.Round(poItem.doubleValue, 1).ToString()));
                        itmX.SubItems.Insert(2, new ListViewItem.ListViewSubItem(null, "%"));
                        break;
                    case UoM.RateOfTurn:
                        itmX.SubItems.Insert(1, new ListViewItem.ListViewSubItem(null, Math.Round(poItem.doubleValue, 0).ToString()));
                        itmX.SubItems.Insert(2, new ListViewItem.ListViewSubItem(null, "°/min"));
                        break;
                    case UoM.Empty:
                        if (!poItem.stringValue.Equals(""))
                            itmX.SubItems.Insert(1, new ListViewItem.ListViewSubItem(null, poItem.stringValue));
                        else
                        {
                            itmX.SubItems.Insert(1, new ListViewItem.ListViewSubItem(null, ""));
                            itmX.BackColor = Color.LightBlue;
                        }
                        itmX.SubItems.Insert(2, new ListViewItem.ListViewSubItem(null, ""));
                        break;
                    default:
                        itmX.SubItems.Insert(1, new ListViewItem.ListViewSubItem(null, poItem.doubleValue.ToString()));
                        break;
                }
            }
        }

        private void Parameters_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm.isParameterFormOpen = false;
        }
    }
}
