using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChoosePointNS;
using Delib.Classes.Features.Navaid;
using ARAN.Contracts.UI;
using Delib.Classes.Codes;
using ARAN.Contracts.GeometryOperators;
using ARAN.Common;
using ARAN.Contracts.Constants;
using System.Diagnostics;


namespace Holding
{
	public partial class frmRnavInitial : Form
	{
		private const double constDoc = 370400;		
		
		private ARAN.GeometryClasses.Point ptGeo, ptPrj;
		private double altitude, att,xtt,ptDistance;
        private int ptHandle;
		private FlightCondition flightCondition;
		private PhaseRecieverCondition _curReciever;
		private PhaseRecieverCondition _curPhase;
		private PBNCondition _curPBN;
		private EnumArray<string, flightReciever> _recieverValue;
		private EnumArray<string, flightPhase> _phaseValue;
		private string[] recievers;
		private DBModule _database;
		private UIContract _ui;
        private GeometryOperators _geomOperators;
		private SpatialReferenceOperation _spatialOperation;
        private bool tool_Clicked,initialized = false;
		
		public frmRnavInitial()
		{
			InitializeComponent();
			
		}
																 
		private void frmRnavInitial_Load(object sender, EventArgs e)
		{
            InitFrom();          
            initialized = true;
			
		}

        private void InitFrom()
        {
            flightCondition = new FlightCondition();
            _database = GlobalParams.Database;
            _ui = GlobalParams.UI;
            _spatialOperation = GlobalParams.SpatialRefOperation;
            _geomOperators = GlobalParams.GeomOperators;
                       
            cmbFlightPhases.DataSource = flightCondition.GetFlightPhases(flightPhase.Enroute | flightPhase.STARUpTo30 | flightPhase.STARDownTo30);
            cmbFlightPhases.DisplayMember = "FlightPhaseName";

            significantPointBox1.OrganisationAuthorityList = GlobalParams.Database.HoldingQpi.GetOrganisationAuthorityList();
            
        }

        //private void FillVorDmeList()
        //{
        ////    lwDme.Items.Clear();
        ////    if (altitude == 0.0)
        ////        return;

        //////	dmeHoldingList = FeatureConvert.GetVorDmeList(ptGeo, docMax, docMin);
        ////    foreach (var dmeHolding in dmeHoldingList)
        ////    {
        ////        ListViewItem item = new ListViewItem(dmeHolding.Desinator);
        ////        item.SubItems.Add(Enum.GetName(typeof(NavaidServiceType), dmeHolding.NavType));
        ////        lwDme.Items.Add(item);
        ////        //chLBDmeList.Items.Add(dmeHolding.Desinator +Enum.GetName(typeof(NavaidServiceType),dmeHolding.NavType));
        ////    }
        //}

        //private void CreateDmeCoverage()
        //{
        //    //DmeCoverageOperation dmeOperation = new DmeCoverageOperation();
        //    //_ui.SafeDeleteGraphic(ref dmeCovHandle);
        //    //availableDmeHoldingList = new List<DmeHolding>();
        //    //dmeCovGeo = new ARAN.GeometryClasses.Polygon();
        //    //bool flag = false;

        //    //if (lwDme.CheckedItems.Count > 0)
        //    //{
        //    //    foreach (ListViewItem lvItem in lwDme.CheckedItems)
        //    //    {
        //    //        availableDmeHoldingList.Add(dmeHoldingList[lvItem.Index]);
        //    //    }
        //    //    if (availableDmeHoldingList.Count > 2 && dmeCovType == DmeCoverageType.threDme)
        //    //    {

        //    //        _dmeCoverageList = dmeOperation.ThreeDmeCoverageList(availableDmeHoldingList, ptPrj, docMax, ref dmeCovGeo);
        //    //        if (_dmeCoverageList.Count > 0)
        //    //            flag = true;
        //    //    }
        //    //    else if (availableDmeHoldingList.Count > 1 && dmeCovType == DmeCoverageType.twoDme)
        //    //    {
        //    //        _dmeCoverageList = dmeOperation.TwoDmeCoverageList(availableDmeHoldingList, ptPrj, docMax);
        //    //        dmeCovGeo = new ARAN.GeometryClasses.Polygon();

        //    //        foreach (DmeCoverage dmeCoverage in _dmeCoverageList)
        //    //        {
        //    //            if (dmeCoverage.IsValidated)
        //    //            {
        //    //                dmeCovGeo = _geomOperators.UnionGeometry(dmeCovGeo, dmeCoverage.geom).AsPolygon;
        //    //                flag = true;
        //    //            }
        //    //        }

        //    //    }
        //    //    if (flag)
        //    //    {
        //    //        att = Math.Round(dmeOperation.ATT(dmeCovType, docMax, _curPBN.PBNName), 2);
        //    //        xtt = Math.Round(dmeOperation.XTT(dmeCovType, docMax, _curPBN.PBNName), 2);
        //    //        lblTolerance.Text = att + " / " + xtt;
        //    //        dmeCovHandle = _ui.DrawPolygon(dmeCovGeo, 1, eFillStyle.sfsCross);
        //    //    }//DrawDmeCoverage(dmeCoverageList, treeCov);
        //    //}

        //}

      
        //private void textBox1_Leave(object sender, EventArgs e)
        //{
        //    //if (txtHeight.Text == "")
        //    //    return;
        //    //double tmp;
        //    //if (double.TryParse(txtHeight.Text, out tmp))
        //    //{
        //    //    if (altitude != Common.DeConvertHeight(tmp))
        //    //    {
        //    //        altitude = Common.DeConvertHeight(tmp);
        //    //        docMax = 4110 * Math.Sqrt(altitude);
        //    //        docMax = docMax > constDoc ? constDoc : docMax;
        //    //        docMin = altitude * Math.Tan(35 * Math.PI / 180);
        //    //        //FillPoints(pointDistance);
        //    //    }
        //    //    if (_curReciever.RecieverName == _recieverValue[flightReciever.DMEDME])
        //    //    {
        //    //        FillDmeList();
        //    //        lwDme.MultiSelect = true;
        //    //    }
        //    //    else if (_curReciever.RecieverName ==_recieverValue[flightReciever.VORDME])
        //    //    {
        //    //        lwDme.MultiSelect = false;
        //    //        FillVorDmeList();
        //    //    }
        //    //}
        //    //else
        //    //    txtHeight.Focus();
						
        //}

		
	
        		
		private void pointPicker1_ByClickChanged(object sender, EventArgs e)
		{
			tool_Clicked = pointPicker1.ByClick;

			if (HoldingService.ByClickToolButton != null)
			{
				if (tool_Clicked)
				{
					HoldingService.ToolClickedCount++;
					HoldingService.ByClickToolButton.SetDownState(true);
				}
				else
				{
					HoldingService.ToolClickedCount--;
					if (HoldingService.ToolClickedCount == 0)
					{
						HoldingService.ByClickToolButton.SetDownState(false);
					}
				}

			}
		}

		private void significantPointBox1_PointChanged(object sender, EventArgs e)
		{
			_ui.SafeDeleteGraphic(ref ptHandle);
			ptGeo = GeomFunctions.GmlToAranPoint(significantPointBox1.SelectedSignificantPoint as Delib.Classes.Feature);
			ptPrj = GlobalParams.SpatialRefOperation.GeoToPrjPoint(ptGeo);
			if (significantPointBox1.SelectedSignificantPoint != null)
				ptHandle =_ui.DrawPointWithText(ptPrj, 1, (significantPointBox1.SelectedSignificantPoint as DesignatedPoint).designator);
            pointPicker1.Longitude = ptGeo.X;
            pointPicker1.Latitude = ptGeo.Y;
			 
		}
      

        //private void lwDme_ItemChecked(object sender, ItemCheckedEventArgs e)
        //{
        //    //int count = lwDme.CheckedItems.Count;
        //    //if (count > 0)
        //    //{
        //    //    if (_curReciever.RecieverName == _recieverValue[flightReciever.DMEDME])
        //    //    {
        //    //        if (dmeCovType == DmeCoverageType.twoDme && count > 1)
        //    //            btnToleranceArea.Enabled = true;
        //    //        else if (dmeCovType == DmeCoverageType.threDme && count > 2)
        //    //            btnToleranceArea.Enabled = true;
        //    //        else
        //    //            btnToleranceArea.Enabled = false;
        //    //    }
        //    //    else if (_curReciever.RecieverName == _recieverValue[flightReciever.VORDME])
        //    //    {
        //    //        if (lwDme.CheckedItems.Count > 1)
        //    //            lwDme.Items[checkNumber].Checked = false;
        //    //        else
        //    //            btnToleranceArea.Enabled = true;
        //    //    }
                    

        //    //}
        //    //else
        //    //    btnToleranceArea.Enabled = false;
        //    //checkNumber = e.Item.Index;
                
        //}


       
     
        

	}
}
