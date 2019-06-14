using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Aran.AranEnvironment;
using System.Collections.Generic;
using Aran.Aim;
using Holding.Forms;
using Aran.Aim.Data;

namespace Holding
{
    public class HoldingService : AranPlugin
    {
        private frmHoldingMain rnav;
        public static AranTool ByClickToolButton;
        public static bool ToolClicked { get; set; }

        public override void Startup(IAranEnvironment aranEnv)
        {
            ToolStripMenuItem menuItem = new ToolStripMenuItem();
            menuItem.Text = "RNAV Holding";
            menuItem.Click += new EventHandler(HoldingMenuItem_Click);
            aranEnv.AranUI.AddMenuItem(AranMapMenu.Applications, menuItem);

            InitHolding.Win32Window = new Win32Windows(aranEnv.Win32Window.Handle.ToInt32());
            GlobalParams.AranEnvironment = aranEnv;
        }

        void HoldingMenuItem_Click(object sender, EventArgs e)
        {
            if (rnav == null || !rnav.Visible) {

                if (!SlotSelector())
                    return;

                if (InitHolding.InitCommand()) {
                    try {
                        AranTool aranToolItem = new AranTool();
                        aranToolItem.Visible = false;
                        aranToolItem.Cursor = Cursors.Cross;
                        ByClickToolButton = aranToolItem;
                        rnav = new frmHoldingMain();
                        ByClickToolButton.MouseClickedOnMap += rnav.OnMouseClickedOnMap;
                        //ByClickToolButton.mou
                        //  ByClickToolButton.Deactivated += rnav.OnDeacitvatedPointPickerTool;
                        GlobalParams.AranEnvironment.AranUI.AddMapTool(ByClickToolButton);
                        rnav.Show(InitHolding.Win32Window);

                    }
                    catch (Exception ex) {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        public override Guid Id
        {
            get { return new Guid("2d1790c1-db87-4b6d-be29-ef2f2c0167f8"); }
        }

        public override string Name
        {
            get { return "RNAV Holding"; }
        }

        public override List<FeatureType> GetLayerFeatureTypes()
        {
            var list = new List<FeatureType>();
            list.Add(FeatureType.AirportHeliport);
            list.Add(FeatureType.DesignatedPoint);
            list.Add(FeatureType.VOR);
            list.Add(FeatureType.DME);
            list.Add(FeatureType.Localizer);
            list.Add(FeatureType.VerticalStructure);
            list.Add(FeatureType.HoldingAssessment);
            list.Add(FeatureType.HoldingPattern);
            return list;
        }
        public bool SlotSelector()
        {
            var slotSelector = true;

            var dbProvider = GlobalParams.AranEnvironment.DbProvider  as DbProvider;
            if (dbProvider != null && dbProvider.ProviderType == DbProviderType.TDB)
            {
                dynamic methodResult = new System.Dynamic.ExpandoObject();
                dbProvider.CallSpecialMethod("SelectSlot", methodResult);
                slotSelector = methodResult.Result;
            }

            if (!slotSelector)
            {
                MessageBox.Show("Please first select slot!", "Slot Selector", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return slotSelector;
            }
            return slotSelector;
        }
    }
}