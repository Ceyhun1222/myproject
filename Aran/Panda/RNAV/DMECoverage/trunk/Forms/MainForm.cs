using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Aran.Aim.Data.Filters;
using Aran.Aim.Features;
using Aran.AranEnvironment.Symbols;
using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.PANDA.Common;
using Aran.PANDA.RNAV.DMECoverage.Modules;

namespace Aran.PANDA.RNAV.DMECoverage
{
	public partial class MainForm : Form
	{
		private const double _MinimalAltitude = 100.0;
		private const double _MaximalAltitude = 9000.0;

		#region  Common variables

		#endregion

		#region Page I

		private List<NavaidType> _fullList;
		private List<NavaidType> _extraDMEList;
		private List<NavaidType> _availableDMEList;

		private List<Segment> _plinLegs;
		private List<Segment> _pgonLegs;

		private List<Procedure> _plinPrc;
		private List<Procedure> _pgonPrc;

		private List<int> _extraDMEElem;

		private MultiPolygon _requestAreaPolygon;

		private double _maxDistance;
		private double _minAltitude;

		private int _prevChoose;
		private int _elemAtSelected;
		private int _elemRequestArea;

		private int _elem2DMECoveragePoly;
		private int _elem3DMECoveragePoly;
		private bool _initialised;

		private AddDMEForm addDMEForm;

		#endregion

		#region Page II
		private List<NavaidType> _selectedDMEList;
		private List<CDMECoverage> _coverageList;
		private MultiPolygon _2DMECoveragePoly;
		private MultiPolygon _3DMECoveragePoly;

	    private List<ThreeDmeCoverage> _threeDmeCoverageList;
		#endregion

		#region Page III
		private List<CriticalDME> _DMEwithPairsList;
		//private List<NavaidType> _DMEwithPairsList;

		#endregion

		#region FORM

		public MainForm()
		{
			InitializeComponent();
			InitForm();

			Size = new System.Drawing.Size(620, 480);
			PageControl.Location = new System.Drawing.Point(3, -22);
		}

		public void InitForm()
		{
			_initialised = false;

		    ExportBtn.Enabled = false;

			radioButtonPoint.Tag = 0;
			radioButtonSegment.Tag = 1;
			radioButtonPolygon.Tag = 2;
			radioButtonAll.Tag = 3;
			rgCoverageAtSelected.Tag = 0;

			addDMEForm = null;

			_elemAtSelected = -1;
			_elemRequestArea = -1;
			_elem2DMECoveragePoly = -1;
			_elem3DMECoveragePoly = -1;
			_prevChoose = 0;            // -1;

			_2DMECoveragePoly = null;
			_3DMECoveragePoly = null;

			_coverageList = new List<CDMECoverage>();
            _threeDmeCoverageList = new List<ThreeDmeCoverage>();

			_extraDMEList = new List<NavaidType>();
			_extraDMEElem = new List<int>();
			_selectedDMEList = new List<NavaidType>();
			_availableDMEList = new List<NavaidType>();
			_DMEwithPairsList = new List<CriticalDME>();

			_plinLegs = new List<Segment>();
			_pgonLegs = new List<Segment>();

			_plinPrc = new List<Procedure>();
			_pgonPrc = new List<Procedure>();

			labelMinimalAltitudeUnit.Text = GlobalVars.unitConverter.HeightUnit;
			labOperationalCoverageUnit.Text = GlobalVars.unitConverter.DistanceUnit;

			listAvailableFacilities.Columns[1].Text = "Distance (" + GlobalVars.unitConverter.DistanceUnit + ")";
			listAvailableFacilities.Columns[2].Text = "Min. covering altitude (" + GlobalVars.unitConverter.HeightUnit + ")";
			listAvailablePairs.Columns[2].Text = listAvailableFacilities.Columns[1].Text;

			//_maxDistance = GlobalVars.settings.Radius;
			_minAltitude = _MaximalAltitude;
			_maxDistance = Math.Sqrt(_minAltitude - GlobalVars.CurrADHP.Elev) * CDMECoverage.NavSignalReception;

			editMinimalAltitude.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_minAltitude).ToString();
			editOperationalCoverage.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_maxDistance).ToString();

			InitPageI();

			//if (cbPointList.Items.Count > 0)
			//	cbPointList.SelectedIndex = 0;

			_initialised = true;
		}

		public void FormClose(System.Object Sender, System.EventArgs e)
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_elemAtSelected);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_elemRequestArea);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_elem2DMECoveragePoly);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_elem3DMECoveragePoly);

			//========================================================
			ListView.SelectedListViewItemCollection selectionList = this.listAvailablePairs.SelectedItems;
			foreach (ListViewItem item in selectionList)
			{
				CDMECoverage selectedDMECoverage = _coverageList[item.Index];
				//if (selectedDMECoverage != null)
				selectedDMECoverage.ClearImages();
			}
		    //========================================================
            var selectionThreeDmeList = this.listThreeDmePairs.SelectedItems;
		    foreach (ListViewItem item in selectionThreeDmeList)
		    {
		        var selectedDMECoverage = _threeDmeCoverageList[item.Index];
		        //if (selectedDMECoverage != null)
		        selectedDMECoverage.ClearImages();
		    }
            //========================================================
            selectionList = this.listCriticalDME.SelectedItems;
			foreach (ListViewItem item in selectionList)
			{
				CriticalDME currCritical = _DMEwithPairsList[item.Index];
				currCritical.ClearImages();
			}
			//========================================================

			for (int i = 0; i < _extraDMEElem.Count; i++)
				GlobalVars.gAranGraphics.SafeDeleteGraphic(_extraDMEElem[i]);
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			// Get a handle to a copy of this form's system (window) menu
			IntPtr hSysMenu = HelperUnit.GetSystemMenu(this.Handle, false);
			// Add a separator
			HelperUnit.AppendMenu(hSysMenu, GlobalVars.MF_SEPARATOR, 0, string.Empty);
			// Add the About menu item
			HelperUnit.AppendMenu(hSysMenu, GlobalVars.MF_STRING, GlobalVars.SYSMENU_ABOUT_ID, "&About…");
		}

		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);

			if ((m.Msg == GlobalVars.WM_SYSCOMMAND) && ((int)m.WParam == GlobalVars.SYSMENU_ABOUT_ID))
			{
				AboutForm about = new AboutForm();
				about.ShowDialog(this);
				about = null;
			}
		}

		#endregion

		#region  Common
		public void PrevBtnClick(System.Object Sender, System.EventArgs e)
		{
			switch (PageControl.SelectedIndex)
			{
				case 1:
					PrevToPageI();
					PageControl.SelectedIndex = 0;
					PrevBtn.Enabled = false;
					break;

				case 2:
					PrevToPageII();
					PageControl.SelectedIndex = 1;
					PrevBtn.Enabled = true;
					break;
			}
			NextBtn.Enabled = true;
		    ExportBtn.Enabled = false;
		}

		public void NextBtnClick(System.Object Sender, System.EventArgs e)
		{
			switch (PageControl.SelectedIndex)
			{
				case 0:
					NextToPageII();
					PageControl.SelectedIndex = 1;
					NextBtn.Enabled = true;
					break;

				case 1:
					NextToPageIII();
					PageControl.SelectedIndex = 2;
					NextBtn.Enabled = false;
				    ExportBtn.Enabled = true;
					break;
			}
			PrevBtn.Enabled = true;
		}

		private void TextBoxes_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			//
		}

		#endregion

		#region Page I

		private void InitPageI()
		{
			int i, n = GlobalVars.DMEList.Length;

			if (n == 0)
				throw new Exception();

			_fullList = new List<NavaidType>();
			_fullList.AddRange(GlobalVars.DMEList);
			//cbPointList.Items.AddRange(GlobalVars.WPTList);

			n = GlobalVars.WPTList.Length;
			for (i = 0; i < n; i++)
				cbPointList.Items.Add(GlobalVars.WPTList[i]);

			if (cbPointList.Items.Count > 0)
				cbPointList.SelectedIndex = 0;

			FillRoutsComboBox();

			radioButtonPoint.Enabled = cbPointList.Items.Count > 0;
			radioButtonSegment.Enabled = _plinPrc.Count > 0;
			radioButtonPolygon.Enabled = _pgonPrc.Count > 0;

			if (cbPointList.Items.Count == 0 && _plinLegs.Count == 0 && _pgonPrc.Count == 0)
				throw new Exception();

			radioButtonPoint.Checked = radioButtonPoint.Enabled;

			if (!radioButtonPoint.Checked)
			{
				radioButtonAll.Checked = true;
				rgCoverageAtSelectedClick(radioButtonAll, null);
			}
			else
				rgCoverageAtSelectedClick(radioButtonPoint, null);


			PageControl.SelectedIndex = 0;
		}

		private void PrevToPageI()
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_elem2DMECoveragePoly);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_elem3DMECoveragePoly);

			_2DMECoveragePoly = null;
			_3DMECoveragePoly = null;

			//========================================================
			ListView.SelectedListViewItemCollection selectionList = this.listAvailablePairs.SelectedItems;
			foreach (ListViewItem item in selectionList)
			{
				CDMECoverage selectedDMECoverage = _coverageList[item.Index];
				//if (selectedDMECoverage != null)
				selectedDMECoverage.ClearImages();
			}
            //========================================================

		    //========================================================
		    var selection3DmeList = this.listThreeDmePairs.SelectedItems;
		    foreach (ListViewItem item in selection3DmeList)
		    {
		        var selected3DMECoverage = _threeDmeCoverageList[item.Index];
                //if (selectedDMECoverage != null)
		        selected3DMECoverage.ClearImages();
		    }
		    //========================================================

            listAvailablePairs.SelectedItems.Clear();
            listThreeDmePairs.SelectedItems.Clear();
		}

		private static int CompareRouteByName(Route r1, Route r2)
		{
			if (r1 == null)
			{
				if (r2 == null)
					return 0;

				return -1;
			}
			else if (r2 == null)
				return 1;

			if (r1.Name == null)
			{
				if (r2.Name == null)
					return 0;

				return -1;
			}
			else if (r2.Name == null)
				return 1;

			return r1.Name.CompareTo(r2.Name);
		}

		public void FillRoutsComboBox()
		{
			List<Route> pProcedureList = DBModule.pObjectDir.GetRouteList(Guid.Empty);      //GlobalVars.CurrADHP.OrgID

			pProcedureList.Sort(CompareRouteByName);

			ComparisonOps cmpOps = new ComparisonOps(ComparisonOpType.EqualTo, "RouteFormed");
			OperationChoice opChoice = new OperationChoice(cmpOps);
			Aim.Data.Filters.Filter filters = new Aim.Data.Filters.Filter(opChoice);

			_plinLegs.Clear();
			_plinPrc.Clear();

			_pgonLegs.Clear();
			_pgonPrc.Clear();

			foreach (Route rt in pProcedureList)
			{
				cmpOps.Value = rt.Identifier;
				opChoice.ComparisonOps = cmpOps;
				filters.Operation = opChoice;

				//List<RouteSegment> pSegmentLegList = (List<RouteSegment>)DBModule.pObjectDir.GetFeatureList(Aim.FeatureType.RouteSegment, filters);
				var tmpRouteList = DBModule.pObjectDir.GetFeatureList(Aim.FeatureType.RouteSegment, filters);
				if (tmpRouteList == null) continue;

				List<RouteSegment> pSegmentLegList = tmpRouteList.Cast<RouteSegment>().ToList();
				if (pSegmentLegList == null || pSegmentLegList.Count == 0)
					continue;

				Procedure prcPln = new Procedure { Name = rt.Name };
				prcPln.startLeg = _plinLegs.Count;

				Procedure prcPgn = new Procedure { Name = rt.Name };
				prcPgn.startLeg = _pgonLegs.Count;

				foreach (RouteSegment rs in pSegmentLegList)
				{
					if (rs.CurveExtent == null && rs.EvaluationArea == null)
						continue;

					Segment sgm = new Segment();

					//if (rs.Start == null || rs.End == null)
					//	continue;

					//if (rs.Start.PointChoice == null || rs.End.PointChoice == null)
					//	continue;


					sgm.Start = new FIX(GlobalVars.gAranEnv);
					sgm.Start.Visible = false;

					if (rs.Start != null && rs.Start.PointChoice != null)
					{
						if (rs.Start.PointChoice.Choice == Aim.SignificantPointChoice.DesignatedPoint)
						{
							Guid dptGuid = rs.Start.PointChoice.FixDesignatedPoint.Identifier;
							DesignatedPoint dpt = (DesignatedPoint)DBModule.pObjectDir.GetFeature(Aim.FeatureType.DesignatedPoint, dptGuid); //segPt.PointChoice.FixDesignatedPoint.Identifier;

							if (dpt == null)
								continue;
							if (dpt.Location == null)
								continue;
							if (dpt.Location.Geo == null)
								continue;

							Geometries.Point pPtGeo = new Geometries.Point(dpt.Location.Geo.X, dpt.Location.Geo.Y, dpt.Location.Geo.Z);
							sgm.Start.PrjPt = (Point)GlobalVars.pspatialReferenceOperation.ToPrj<Point>(pPtGeo);
							sgm.Start.Name = dpt.Designator;
						}
						else if (rs.Start.PointChoice.Choice == Aim.SignificantPointChoice.Navaid)
						{
							Guid dptGuid = rs.Start.PointChoice.NavaidSystem.Identifier;
							Navaid dpt = (Navaid)DBModule.pObjectDir.GetFeature(Aim.FeatureType.Navaid, dptGuid);                               //segPt.PointChoice.FixDesignatedPoint.Identifier;

							if (dpt == null)
								continue;
							if (dpt.Location == null)
								continue;
							if (dpt.Location.Geo == null)
								continue;

							Geometries.Point pPtGeo = new Geometries.Point(dpt.Location.Geo.X, dpt.Location.Geo.Y, dpt.Location.Geo.Z);
							sgm.Start.PrjPt = (Point)GlobalVars.pspatialReferenceOperation.ToPrj<Point>(pPtGeo);
							sgm.Start.Name = dpt.Designator;
						}
					}
					//else
					//	continue;

					sgm.End = new FIX(GlobalVars.gAranEnv);
					sgm.End.Visible = false;

					if (rs.End != null && rs.End.PointChoice != null)
					{
						if (rs.End.PointChoice.Choice == Aim.SignificantPointChoice.DesignatedPoint)
						{
							Guid dptGuid = rs.End.PointChoice.FixDesignatedPoint.Identifier;
							DesignatedPoint dpt = (DesignatedPoint)DBModule.pObjectDir.GetFeature(Aim.FeatureType.DesignatedPoint, dptGuid);    //segPt.PointChoice.FixDesignatedPoint.Identifier;

							if (dpt == null)
								continue;
							if (dpt.Location == null)
								continue;
							if (dpt.Location.Geo == null)
								continue;

							Geometries.Point pPtGeo = new Geometries.Point(dpt.Location.Geo.X, dpt.Location.Geo.Y, dpt.Location.Geo.Z);
							sgm.End.PrjPt = (Point)GlobalVars.pspatialReferenceOperation.ToPrj<Point>(pPtGeo);
							sgm.End.Name = dpt.Designator;
						}
						else if (rs.End.PointChoice.Choice == Aim.SignificantPointChoice.Navaid)
						{
							Guid dptGuid = rs.End.PointChoice.NavaidSystem.Identifier;
							Navaid dpt = (Navaid)DBModule.pObjectDir.GetFeature(Aim.FeatureType.Navaid, dptGuid);                               //segPt.PointChoice.FixDesignatedPoint.Identifier;

							if (dpt == null)
								continue;
							if (dpt.Location == null)
								continue;
							if (dpt.Location.Geo == null)
								continue;

							Geometries.Point pPtGeo = new Geometries.Point(dpt.Location.Geo.X, dpt.Location.Geo.Y, dpt.Location.Geo.Z);
							sgm.End.PrjPt = (Point)GlobalVars.pspatialReferenceOperation.ToPrj<Geometries.Point>(pPtGeo);
							sgm.End.Name = dpt.Designator;
						}
					}
					//else
					//	continue;

					sgm.NominalTrack = null;
					if (rs.CurveExtent != null)
						sgm.NominalTrack = (MultiLineString)GlobalVars.pspatialReferenceOperation.ToPrj<MultiLineString>(rs.CurveExtent.Geo);

					sgm.ProtectionArea = null;
					if (rs.EvaluationArea != null)
						sgm.ProtectionArea = (MultiPolygon)GlobalVars.pspatialReferenceOperation.ToPrj<MultiPolygon>(rs.EvaluationArea.Surface.Geo);

					if (sgm.NominalTrack != null)
						_plinLegs.Add(sgm);

					if (sgm.ProtectionArea != null)
						_pgonLegs.Add(sgm);
				}

				//===========================================================+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

				if (prcPln.startLeg < _plinLegs.Count)
				{
					prcPln.endleg = _plinLegs.Count - 1;
					_plinPrc.Add(prcPln);
					//comboBox101.Items.Add(prcPln);
				}

				if (prcPgn.startLeg < _pgonLegs.Count)
				{
					prcPgn.endleg = _pgonLegs.Count - 1;
					_pgonPrc.Add(prcPgn);
				}
			}
		}

		public void AddBtnClick(System.Object Sender, System.EventArgs e)
		{
			NavaidType DME = default(NavaidType);

			if (addDMEForm == null || addDMEForm.IsDisposed)
				addDMEForm = new AddDMEForm();

			Point pPoint;
			if (_extraDMEList.Count == 0)
				pPoint = new Point(GlobalVars.CurrADHP.pPtGeo);
			else
				pPoint = new Point(_extraDMEList[_extraDMEList.Count - 1].pPtGeo);

			if (addDMEForm.ShowForm(ref pPoint) != System.Windows.Forms.DialogResult.OK)
				return;

			DME.CallSign = addDMEForm.Edit7.Text;   // +"(Extra_" + _extraDMEList.Count.ToString() + ")";
			DME.pPtGeo = pPoint;
			DME.pPtPrj = GlobalVars.pspatialReferenceOperation.ToPrj<Point>(DME.pPtGeo);

			_extraDMEElem.Add(GlobalVars.gAranGraphics.DrawPointWithText(DME.pPtPrj, DME.CallSign, 255));
			_extraDMEList.Add(DME);
			_fullList.Add(DME);

			CalculateSelection();
		}

		public void LoadBtnClick(System.Object Sender, System.EventArgs e)
		{
			if (LoadDME.ShowDialog() != System.Windows.Forms.DialogResult.OK)
				return;

			DMEListSerialiser DMEReader = new DMEListSerialiser(LoadDME.FileName);
			List<NavaidType> DMEList = new List<NavaidType>();
			DMEReader.Read(DMEList);

			try
			{
				int n = DMEList.Count;
				for (int i = 0; i < n; i++)
				{
					NavaidType DME = DMEList[i];
					DME.pPtPrj = GlobalVars.pspatialReferenceOperation.ToPrj<Point>(DME.pPtGeo);

					_extraDMEElem.Add(GlobalVars.gAranGraphics.DrawPointWithText(DME.pPtPrj, DME.CallSign, 255));
					_extraDMEList.Add(DME);
					_fullList.Add(DME);
				}
			}
			finally
			{
				CalculateSelection();
			}
		}

		public void SaveBtnClick(System.Object Sender, System.EventArgs e)
		{
			if (SaveDME.ShowDialog() != System.Windows.Forms.DialogResult.OK)
				return;

			DMEListSerialiser DMEReader = new DMEListSerialiser(SaveDME.FileName);
			List<NavaidType> DMEList = new List<NavaidType>();
			DMEReader.Save(_extraDMEList);
		}

		public void rgCoverageAtSelectedClick(System.Object Sender, System.EventArgs e)
		{
			if (!((RadioButton)Sender).Checked)
				return;

			int variant = (int)((RadioButton)Sender).Tag;
			rgCoverageAtSelected.Tag = variant;

			switch (variant)
			{
				case 0:
					labelPointList.Enabled = true;
					labelPointType.Enabled = true;
					cbPointList.Enabled = true;             //cbPointList.Items.Count > 0;
					cbExclude.Enabled = true;

					label101.Enabled = false;
					comboBox101.Enabled = false;

					if (cbPointList.Items.Count == 0)
						return;

					if (cbPointList.SelectedIndex < 0)
						cbPointList.SelectedIndex = 0;
					else
						GetPointCover();

					break;
				case 1:
				case 2:
					labelPointList.Enabled = false;
					labelPointType.Enabled = false;
					cbPointList.Enabled = false;
					cbExclude.Enabled = false;

					label101.Enabled = true;

					if (_prevChoose != variant)
					{
						comboBox101.Items.Clear();
						if (variant == 1)
							for (int i = 0; i < _plinPrc.Count; i++)
								comboBox101.Items.Add(_plinPrc[i]);
						else
							for (int i = 0; i < _pgonPrc.Count; i++)
								comboBox101.Items.Add(_pgonPrc[i]);
					}

					comboBox101.Enabled = true;
					_prevChoose = variant;

					if (comboBox101.SelectedIndex < 0)
						comboBox101.SelectedIndex = 0;
					else
						GetPlineOrPgonCover();

					break;
				case 3:
					labelPointList.Enabled = false;
					labelPointType.Enabled = false;
					cbPointList.Enabled = false;
					cbExclude.Enabled = false;

					label101.Enabled = false;
					comboBox101.Enabled = false;

					GetAlllPosibleCovers();

					break;
			}

			cbExcludeWide_CheckedChanged(null, null);
		}

		public void cbPointList_SelectedIndexChanged(Object Sender, EventArgs e)
		{
			if (!_initialised)
				return;

			GetPointCover();
		}

		private void comboBox101_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!_initialised)
				return;

			GetPlineOrPgonCover();
		}

		private void cbExcludeWide_CheckedChanged(object sender, EventArgs e)
		{
			int n = _availableDMEList.Count;

			for (int i = 0; i < n; i++)
			{
				NavaidType NavDME = _availableDMEList[i];

				bool notCheked = (cbExcludeILS.Checked && (NavDME.Tag & 8) != 0) || (cbExcludeNarrow.Checked && (NavDME.Tag & 1) != 0)
					|| (cbExcludePrecision.Checked && (NavDME.Tag & 2) != 0) || (cbExcludeWide.Checked && (NavDME.Tag & 4) != 0);

				if (notCheked)
					listAvailableFacilities.Items[i].Checked = false;
			}
		}

		public void CalculateSelection()
		{
			switch ((int)rgCoverageAtSelected.Tag)
			{
				case 0:
					rgCoverageAtSelectedClick(radioButtonPoint, null);
					break;
				case 1:
					rgCoverageAtSelectedClick(radioButtonSegment, null);
					break;
				case 2:
					rgCoverageAtSelectedClick(radioButtonPolygon, null);
					break;
				case 3:
					rgCoverageAtSelectedClick(radioButtonAll, null);
					break;
			}
		}

		public void GetPointCover()
		{
			listAvailableFacilities.Items.Clear();
			_availableDMEList.Clear();

			int k = cbPointList.SelectedIndex;
			if (k < 0)
				return;

			WPT_FIXType sigPoint = (WPT_FIXType)cbPointList.SelectedItem;
			labelPointType.Text = sigPoint.TypeCode.Tostring();

			_requestAreaPolygon = new MultiPolygon();

			Polygon tmpPoly = new Polygon();
			tmpPoly.ExteriorRing = ARANFunctions.CreateCirclePrj(sigPoint.pPtPrj, _maxDistance);
			_requestAreaPolygon.Add(tmpPoly);

			GlobalVars.gAranGraphics.SafeDeleteGraphic(_elemAtSelected);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_elemRequestArea);
			_elemRequestArea = GlobalVars.gAranGraphics.DrawMultiPolygon(_requestAreaPolygon, eFillStyle.sfsNull, ARANFunctions.RGB(0, 0, 255));
			_elemAtSelected = GlobalVars.gAranGraphics.DrawPointWithText(sigPoint.pPtPrj, sigPoint.Name, ARANFunctions.RGB(0, 0, 255));

			//==============================================================================

			foreach (NavaidType NavDME in _fullList)
			{
				double Dist = ARANMath.Hypot(NavDME.pPtPrj.Y - sigPoint.pPtPrj.Y, NavDME.pPtPrj.X - sigPoint.pPtPrj.X);

				if (Dist <= _maxDistance && Dist > CDMECoverage.DMENoUpdateZoneRadius)
				{
					_availableDMEList.Add(NavDME);

					ListViewItem ListItem = listAvailableFacilities.Items.Add(NavDME.ToString());

					double fTmp = Dist / CDMECoverage.NavSignalReception;
					double MinH = fTmp * fTmp;

					Dist = GlobalVars.unitConverter.DistanceToDisplayUnits(Dist);
					MinH = GlobalVars.unitConverter.HeightToDisplayUnits(MinH);

					ListItem.SubItems.Add(Dist.ToString());
					ListItem.SubItems.Add(MinH.ToString());
					ListItem.Checked = true;
				}
			}

			NextBtn.Enabled = _availableDMEList.Count > 1;
		}

		public void GetPlineOrPgonCover()
		{
			listAvailableFacilities.Items.Clear();
			_availableDMEList.Clear();

			int k = comboBox101.SelectedIndex;
			if (k < 0)
				return;

			Procedure prc = (Procedure)comboBox101.SelectedItem;

			Geometry SuperPolygon;
			int i, j, n = _plinLegs.Count;

			if ((int)rgCoverageAtSelected.Tag == 1)
			{
				MultiLineString super = new MultiLineString();

				for (i = prc.startLeg; i <= prc.endleg; i++)
				{
					MultiLineString Poly = _plinLegs[i].NominalTrack;
					if (Poly == null)
						continue;

					for (j = 0; j < Poly.Count; j++)
						super.Add(Poly[j]);
				}

				SuperPolygon = super;
			}
			else
			{
				MultiPolygon super = new MultiPolygon();

				for (i = prc.startLeg; i <= prc.endleg; i++)
				{
					MultiPolygon Poly = _pgonLegs[i].ProtectionArea;
					if (Poly == null)
						continue;

					for (j = 0; j < Poly.Count; j++)
						super.Add(Poly[j]);
				}

				SuperPolygon = super;
			}

			GeometryOperators geomOp = new GeometryOperators();
			_requestAreaPolygon = (MultiPolygon)geomOp.Buffer(SuperPolygon, _maxDistance);

			//==============================================================================

			listAvailableFacilities.Items.Clear();
			_availableDMEList.Clear();

			GlobalVars.gAranGraphics.SafeDeleteGraphic(_elemAtSelected);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_elemRequestArea);

			_elemRequestArea = GlobalVars.gAranGraphics.DrawMultiPolygon(_requestAreaPolygon, eFillStyle.sfsNull, ARANFunctions.RGB(0, 0, 255));
			if ((int)rgCoverageAtSelected.Tag == 1)
				_elemAtSelected = GlobalVars.gAranGraphics.DrawMultiLineString((MultiLineString)SuperPolygon, 1, ARANFunctions.RGB(0, 0, 255));
			else
				_elemAtSelected = GlobalVars.gAranGraphics.DrawMultiPolygon((MultiPolygon)SuperPolygon, eFillStyle.sfsNull, ARANFunctions.RGB(0, 0, 255));

			if (SuperPolygon.IsEmpty)
			{
				MessageBox.Show(GlobalVars.Win32Window, "No feature was selected.", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
				NextBtn.Enabled = false;
				return;
			}

			//==============================================================================

			foreach (NavaidType NavDME in _fullList)
			{
				double Dist = geomOp.GetDistance(SuperPolygon, NavDME.pPtPrj);

				if (Dist <= _maxDistance)
				{
					_availableDMEList.Add(NavDME);

					ListViewItem ListItem = listAvailableFacilities.Items.Add(NavDME.CallSign);

					double fTmp = Dist / CDMECoverage.NavSignalReception;
					double MinH = fTmp * fTmp;

					Dist = GlobalVars.unitConverter.DistanceToDisplayUnits(Dist);
					MinH = GlobalVars.unitConverter.HeightToDisplayUnits(MinH);

					ListItem.SubItems.Add(Dist.ToString());
					ListItem.SubItems.Add(MinH.ToString());
					ListItem.Checked = true;
				}
			}

			NextBtn.Enabled = _availableDMEList.Count > 1;
		}

		public void GetAlllPosibleCovers()
		{
			listAvailableFacilities.Items.Clear();
			_availableDMEList.Clear();

			_requestAreaPolygon = new MultiPolygon();

			//int i, j = 0, n = cbPointList.Items.Count;

			Polygon tmpPoly = new Polygon();
			tmpPoly.ExteriorRing = ARANFunctions.CreateCirclePrj(GlobalVars.CurrADHP.pPtPrj, GlobalVars.settings.Radius);
			_requestAreaPolygon.Add(tmpPoly);

			GlobalVars.gAranGraphics.SafeDeleteGraphic(_elemAtSelected);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_elemRequestArea);

			_elemRequestArea = GlobalVars.gAranGraphics.DrawMultiPolygon(_requestAreaPolygon, eFillStyle.sfsNull, ARANFunctions.RGB(0, 0, 255));
			_elemAtSelected = GlobalVars.gAranGraphics.DrawPointWithText(GlobalVars.CurrADHP.pPtPrj, GlobalVars.CurrADHP.Name, ARANFunctions.RGB(0, 0, 255));

			//==============================================================================
			//int n = _allList.Count;
			//for (int i = 0; i < n; i++)
			//NavaidType NavDME = _allList[i];

			foreach (NavaidType NavDME in _fullList)
			{
				_availableDMEList.Add(NavDME);

				ListViewItem ListItem = listAvailableFacilities.Items.Add(NavDME.ToString());

				//double Dist = ARANMath.Hypot(NavDME.pPtPrj.Y - GlobalVars.CurrADHP.pPtPrj.Y, NavDME.pPtPrj.X - GlobalVars.CurrADHP.pPtPrj.X);
				//double fTmp = Dist / TDMECoverage.C_NavSignalReception;
				//double MinH = fTmp * fTmp;

				//Dist = GlobalVars.unitConverter.DistanceToDisplayUnits(Dist);
				//MinH = GlobalVars.unitConverter.HeightToDisplayUnits(MinH);

				//ListItem.SubItems.Add(Dist.ToString());
				//ListItem.SubItems.Add(MinH.ToString());

				ListItem.SubItems.Add("-");
				ListItem.SubItems.Add("-");

				ListItem.Checked = true;
			}

			NextBtn.Enabled = _availableDMEList.Count > 1;
		}

		private void listAvailableFacilities_ItemChecked(object sender, ItemCheckedEventArgs e)
		{
			int nChecked = 0;

			foreach (ListViewItem item in listAvailableFacilities.Items)
				if (item.Checked)
				{
					nChecked++;

					if (nChecked > 1)
					{
						NextBtn.Enabled = true;
						return;
					}
				}

			NextBtn.Enabled = false;
		}

		private void editMinimalAltitude_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				editMinimalAltitude_Validating(editMinimalAltitude, new System.ComponentModel.CancelEventArgs());
			else
				HelperUnit.TextBoxFloat(ref eventChar, editMinimalAltitude.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void editMinimalAltitude_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			double fTmp;

			if (double.TryParse(editMinimalAltitude.Text, out fTmp))
			{
				if (editMinimalAltitude.Tag != null && editMinimalAltitude.Tag.ToString() == editMinimalAltitude.Text)
					return;

				_minAltitude = fTmp = GlobalVars.unitConverter.HeightToInternalUnits(fTmp);

				double min = Math.Max(GlobalVars.CurrADHP.Elev, _MinimalAltitude);

				if (_minAltitude < min)
					_minAltitude = min;

				if (_minAltitude > _MaximalAltitude)
					_minAltitude = _MaximalAltitude;

				if (_minAltitude != fTmp)
					editMinimalAltitude.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_minAltitude).ToString();
			}
			else if (double.TryParse(editMinimalAltitude.Tag.ToString(), out fTmp))
				editMinimalAltitude.Text = editMinimalAltitude.Tag.ToString();
			else
				editMinimalAltitude.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_minAltitude).ToString();

			editMinimalAltitude.Tag = editMinimalAltitude.Text;


			//=============================================================================================================================
			_maxDistance = Math.Sqrt(_minAltitude - GlobalVars.CurrADHP.Elev) * CDMECoverage.NavSignalReception;
			editOperationalCoverage.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_maxDistance).ToString();

			CalculateSelection();
		}

		#endregion

		#region Page II

		private void NextToPageII()         // init page 2
		{
			_selectedDMEList.Clear();
			int n = _availableDMEList.Count;

			for (int i = 0; i < n; i++)
				if (listAvailableFacilities.Items[i].Checked)
					_selectedDMEList.Add(_availableDMEList[i]);

			switch ((int)rgCoverageAtSelected.Tag)
			{
				case 0:
					if (cbPointList.SelectedIndex < 0)
						return;

					WPT_FIXType sigPoint = (WPT_FIXType)(cbPointList.SelectedItem);
					FillPointCoverageList(sigPoint.pPtPrj);
					break;
				case 1:
				case 2:
					FillPolygonCoverageList();
					break;
				case 3:
					FillAllCoverageList();
					break;
			}
		}

		private void PrevToPageII()
		{
			ListView.SelectedListViewItemCollection selectionList = this.listCriticalDME.SelectedItems;
			foreach (ListViewItem item in selectionList)
			{
				CriticalDME currCritical = _DMEwithPairsList[item.Index];
				currCritical.ClearImages();
			}

			if (_2DMECoveragePoly != null && cb2DME.Checked)
				_elem2DMECoveragePoly = GlobalVars.gAranGraphics.DrawMultiPolygon(_2DMECoveragePoly, eFillStyle.sfsDiagonalCross, 255);

			if (_3DMECoveragePoly != null && cb3DME.Checked)
				_elem3DMECoveragePoly = GlobalVars.gAranGraphics.DrawMultiPolygon(_3DMECoveragePoly, eFillStyle.sfsDiagonalCross, 0);
		}

		private void FillAllCoverageList()
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_elem2DMECoveragePoly);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_elem3DMECoveragePoly);
			listAvailablePairs.Items.Clear();
            listThreeDmePairs.Items.Clear();
			_coverageList.Clear();
            _threeDmeCoverageList.Clear();

			int n = _selectedDMEList.Count;
			if (n <= 0)
				return;

			Gauge1.Value = 0;
			Gauge1.Maximum = ((n - 1) * n) / 2;
			Gauge1.Visible = true;

			GeometryOperators geomOp = new GeometryOperators();

			for (int i = 0; i < n - 1; i++)
			{
				NavaidType DME1 = _selectedDMEList[i];
				for (int j = i + 1; j < n; j++)
				{
					NavaidType DME2 = _selectedDMEList[j];
					double Dist = ARANMath.Hypot(DME2.pPtPrj.Y - DME1.pPtPrj.Y, DME2.pPtPrj.X - DME1.pPtPrj.X);

					//Dir = ArcTan2(DME2.pPtPrj.Y - DME.pPtPrj.Y, DME2.pPtPrj.X - DME.pPtPrj.X);

					//if (cbExclude.Checked)
					//{
					//	Point Center1 = ARANFunctions.LocalToPrj(DME.pPtPrj, Dir, 0.5 * Dist, TDMECoverage.C_CATET * Dist);
					//	Point Center2 = ARANFunctions.LocalToPrj(DME.pPtPrj, Dir, 0.5 * Dist, -TDMECoverage.C_CATET * Dist);

					//	Dist1 = ARANMath.Hypot(point.Y - Center1.Y, point.X - Center1.X);
					//	Dist2 = ARANMath.Hypot(point.Y - Center2.Y, point.X - Center2.X);
					//	bPass = (Dist1 < Dist) ^ (Dist2 < Dist);
					//}
					//else
					//	bPass = Dist > 2 * TDMECoverage.C_DMENoUpdateZoneRadius;

					bool bPass = (Dist < 2.0 * _maxDistance - 1000.0) && (Dist > 2 * CDMECoverage.DMENoUpdateZoneRadius);

					if (bPass)
					{
						

						CDMECoverage NewDMECoverage = new CDMECoverage(_minAltitude, DME1, DME2);

					    if (NewDMECoverage.AvailableZone.IsEmpty) continue;
                        //NewDMECoverage.Altitude = _minAltitude;
                        //NewDMECoverage.DME1 = DME1;
					    //NewDMECoverage.DME2 = DME2;

					    ListViewItem ListItem = listAvailablePairs.Items.Add(DME1.CallSign);
					    ListItem.SubItems.Add(DME2.CallSign);

					    double fTmp = GlobalVars.unitConverter.DistanceToDisplayUnits(Dist);
					    ListItem.SubItems.Add(fTmp.ToString());

					    NewDMECoverage.Distance = fTmp;
                        _coverageList.Add(NewDMECoverage);

						if (_coverageList.Count == 1)
							_2DMECoveragePoly = (MultiPolygon)NewDMECoverage.AvailableZone.Clone();
						else
							_2DMECoveragePoly = (MultiPolygon)geomOp.UnionGeometry(NewDMECoverage.AvailableZone, _2DMECoveragePoly);
					}

					Gauge1.Value++;
				}
			}

			n = _coverageList.Count;
			if (n > 0)
			{
				Gauge1.Value = 0;
				Gauge1.Maximum = ((n - 1) * n) >> 1;

				for (int i = 0; i < n - 1; i++)
				{
					CDMECoverage DMECoverage0 = _coverageList[i];
					for (int j = i + 1; j < n; j++)
					{
						CDMECoverage DMECoverage1 = _coverageList[j];
						Geometry TmpPoly0 = geomOp.Intersect(DMECoverage0.AvailableZone, DMECoverage1.AvailableZone);

					    if (!TmpPoly0.IsEmpty && (TmpPoly0.Type == GeometryType.Polygon || TmpPoly0.Type == GeometryType.MultiPolygon))
                        {
						    //Added by Agshin
						    var availableZone = TmpPoly0;
						    if (TmpPoly0.Type == GeometryType.Polygon)
						        availableZone = new MultiPolygon { TmpPoly0 as Polygon };
						    var tmpThreeCoverage = new ThreeDmeCoverage(DMECoverage0, DMECoverage1, availableZone as MultiPolygon);
						    _threeDmeCoverageList.Add(tmpThreeCoverage);

						    ListViewItem ListItem = listThreeDmePairs.Items.Add(DMECoverage0.DME1.CallSign + "-" + DMECoverage0.DME2.CallSign);
						    ListItem.SubItems.Add(DMECoverage1.DME1.CallSign + "-" + DMECoverage1.DME2.CallSign);


                            //
                            if (_3DMECoveragePoly != null)
								_3DMECoveragePoly = (MultiPolygon)geomOp.UnionGeometry(TmpPoly0, _3DMECoveragePoly);
							else
								_3DMECoveragePoly = (MultiPolygon)TmpPoly0;
						}

						Gauge1.Value++;
					}
				}
			}

			if (_2DMECoveragePoly != null && cb2DME.Checked)
				_elem2DMECoveragePoly = GlobalVars.gAranGraphics.DrawMultiPolygon(_2DMECoveragePoly, eFillStyle.sfsDiagonalCross, 255);

			if (_3DMECoveragePoly != null && cb3DME.Checked)
				_elem3DMECoveragePoly = GlobalVars.gAranGraphics.DrawMultiPolygon(_3DMECoveragePoly, eFillStyle.sfsDiagonalCross, 0);

			Gauge1.Visible = false;
		}

		private void FillPolygonCoverageList()
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_elem2DMECoveragePoly);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_elem3DMECoveragePoly);
			listAvailablePairs.Items.Clear();
            listThreeDmePairs.Items.Clear();

			_coverageList.Clear();
            _threeDmeCoverageList.Clear();

			int n = _selectedDMEList.Count;
			if (n <= 0)
				return;

			Gauge1.Value = 0;
			Gauge1.Maximum = ((n - 1) * n) / 2;
			Gauge1.Visible = true;

			GeometryOperators geomOp = new GeometryOperators();

			for (int i = 0; i < n - 1; i++)
			{
				NavaidType DME1 = _selectedDMEList[i];
				for (int j = i + 1; j < n; j++)
				{
					NavaidType DME2 = _selectedDMEList[j];
					double Dist = ARANMath.Hypot(DME2.pPtPrj.Y - DME1.pPtPrj.Y, DME2.pPtPrj.X - DME1.pPtPrj.X);

					//Dir = ArcTan2(DME2.pPtPrj.Y - DME.pPtPrj.Y, DME2.pPtPrj.X - DME.pPtPrj.X);

					//if (cbExclude.Checked)
					//{
					//	Point Center1 = ARANFunctions.LocalToPrj(DME.pPtPrj, Dir, 0.5 * Dist, TDMECoverage.C_CATET * Dist);
					//	Point Center2 = ARANFunctions.LocalToPrj(DME.pPtPrj, Dir, 0.5 * Dist, -TDMECoverage.C_CATET * Dist);

					//	Dist1 = ARANMath.Hypot(point.Y - Center1.Y, point.X - Center1.X);
					//	Dist2 = ARANMath.Hypot(point.Y - Center2.Y, point.X - Center2.X);
					//	bPass = (Dist1 < Dist) ^ (Dist2 < Dist);
					//}
					//else
					//	bPass = Dist > 2 * TDMECoverage.C_DMENoUpdateZoneRadius;

					bool bPass = (Dist < 2.0 * _maxDistance - 1000.0) && (Dist > 2 * CDMECoverage.DMENoUpdateZoneRadius);

					if (bPass)
					{
						

						CDMECoverage NewDMECoverage = new CDMECoverage(_minAltitude, DME1, DME2);

					    if (NewDMECoverage.AvailableZone.IsEmpty) continue;

					    ListViewItem ListItem = listAvailablePairs.Items.Add(DME1.CallSign);
					    ListItem.SubItems.Add(DME2.CallSign);

					    double fTmp = GlobalVars.unitConverter.DistanceToDisplayUnits(Dist);
					    ListItem.SubItems.Add(fTmp.ToString());
					    NewDMECoverage.Distance = fTmp;

                        //NewDMECoverage.Altitude = _minAltitude;
                        //NewDMECoverage.DME1 = DME1;
                        //NewDMECoverage.DME2 = DME2;
                        _coverageList.Add(NewDMECoverage);

						if (_coverageList.Count == 1)
							_2DMECoveragePoly = (MultiPolygon)NewDMECoverage.AvailableZone.Clone();
						else
							_2DMECoveragePoly = (MultiPolygon)geomOp.UnionGeometry(NewDMECoverage.AvailableZone, _2DMECoveragePoly);
					}

					Gauge1.Value++;
				}
			}

			n = _coverageList.Count;
			if (n > 0)
			{
				Gauge1.Value = 0;
				Gauge1.Maximum = ((n - 1) * n) >> 1;

				for (int i = 0; i < n - 1; i++)
				{
					CDMECoverage DMECoverage0 = _coverageList[i];
					for (int j = i + 1; j < n; j++)
					{
						CDMECoverage DMECoverage1 = _coverageList[j];
						Geometry TmpPoly0 = geomOp.Intersect(DMECoverage0.AvailableZone, DMECoverage1.AvailableZone);

						if (!TmpPoly0.IsEmpty && (TmpPoly0.Type == GeometryType.Polygon || TmpPoly0.Type == GeometryType.MultiPolygon))
						{
                            //Added by Agshin
						    var availableZone = TmpPoly0;
						    if (TmpPoly0.Type == GeometryType.Polygon)
						        availableZone = new MultiPolygon {TmpPoly0 as Polygon};
                            var tmpThreeCoverage = new ThreeDmeCoverage(DMECoverage0,DMECoverage1,availableZone as MultiPolygon);
                            _threeDmeCoverageList.Add(tmpThreeCoverage);

						    ListViewItem ListItem = listThreeDmePairs.Items.Add(DMECoverage0.DME1.CallSign + "-" + DMECoverage0.DME2.CallSign);
						    ListItem.SubItems.Add(DMECoverage1.DME1.CallSign + "-" + DMECoverage1.DME2.CallSign);

                            //
                            if (_3DMECoveragePoly != null)
								_3DMECoveragePoly = (MultiPolygon)geomOp.UnionGeometry(TmpPoly0, _3DMECoveragePoly);
							else
								_3DMECoveragePoly = (MultiPolygon)TmpPoly0;
						}

						Gauge1.Value++;
					}
				}
			}

			if (_2DMECoveragePoly != null && cb2DME.Checked)
				_elem2DMECoveragePoly = GlobalVars.gAranGraphics.DrawMultiPolygon(_2DMECoveragePoly, eFillStyle.sfsDiagonalCross, 255);

			if (_3DMECoveragePoly != null && cb3DME.Checked)
				_elem3DMECoveragePoly = GlobalVars.gAranGraphics.DrawMultiPolygon(_3DMECoveragePoly, eFillStyle.sfsDiagonalCross, 0);

			Gauge1.Visible = false;
		}

		private void FillPointCoverageList(Point point)
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_elem2DMECoveragePoly);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_elem3DMECoveragePoly);
			listAvailablePairs.Items.Clear();
            listThreeDmePairs.Items.Clear();

			_coverageList.Clear();
            _threeDmeCoverageList.Clear();

			int n = _selectedDMEList.Count;
			if (n <= 0)
				return;

			Gauge1.Value = 0;
			Gauge1.Maximum = ((n - 1) * n) / 2;
			Gauge1.Visible = true;

			GeometryOperators geomOp = new GeometryOperators();

			for (int i = 0; i < n - 1; i++)
			{
				NavaidType DME1 = _selectedDMEList[i];

				for (int j = i + 1; j < n; j++)
				{
					NavaidType DME2 = _selectedDMEList[j];
					double Dist = ARANMath.Hypot(DME2.pPtPrj.Y - DME1.pPtPrj.Y, DME2.pPtPrj.X - DME1.pPtPrj.X);
					double Dir = Math.Atan2(DME2.pPtPrj.Y - DME1.pPtPrj.Y, DME2.pPtPrj.X - DME1.pPtPrj.X);
					bool bPass;

					//GlobalVars.gAranGraphics.DrawPointWithText(point, -1, "pt");
					//GlobalVars.gAranGraphics.DrawPointWithText(DME1.pPtPrj, -1, "DME1");
					//GlobalVars.gAranGraphics.DrawPointWithText(DME2.pPtPrj, -1, "DME2");
					//Application.DoEvents();

					if (cbExclude.Checked)
					{
						Point Center1 = ARANFunctions.LocalToPrj(DME1.pPtPrj, Dir, 0.5 * Dist, CDMECoverage.Catet * Dist);
						Point Center2 = ARANFunctions.LocalToPrj(DME1.pPtPrj, Dir, 0.5 * Dist, -CDMECoverage.Catet * Dist);

						//GlobalVars.gAranGraphics.DrawPointWithText(DME1.pPtPrj, -1, "DME1");
						//GlobalVars.gAranGraphics.DrawPointWithText(DME2.pPtPrj, -1, "DME2");
						//GlobalVars.gAranGraphics.DrawPointWithText(Center1, -1, "Center1");
						//GlobalVars.gAranGraphics.DrawPointWithText(Center2, -1, "Center2");
						//while (true)
						//Application.DoEvents();

						double Dist1 = ARANMath.Hypot(point.Y - Center1.Y, point.X - Center1.X);
						double Dist2 = ARANMath.Hypot(point.Y - Center2.Y, point.X - Center2.X);

						bPass = (Dist1 < Dist) ^ (Dist2 < Dist);
					}
					else
						bPass = (Dist < 2.0 * _maxDistance - 1000.0) && (Dist > 2 * CDMECoverage.DMENoUpdateZoneRadius);

					if (bPass)
					{
					
						CDMECoverage NewDMECoverage = new CDMECoverage(_minAltitude, DME1, DME2);
					    if (NewDMECoverage.AvailableZone.IsEmpty) continue;

                        ListViewItem ListItem = listAvailablePairs.Items.Add(DME1.CallSign);
					    ListItem.SubItems.Add(DME2.CallSign);

					    double fTmp = GlobalVars.unitConverter.DistanceToDisplayUnits(Dist);
					    ListItem.SubItems.Add(fTmp.ToString());

					    NewDMECoverage.Distance = fTmp;
                        //NewDMECoverage.Altitude = _minAltitude;
                        //NewDMECoverage.DME1 = DME1;
                        //NewDMECoverage.DME2 = DME2;
                        _coverageList.Add(NewDMECoverage);

						if (_coverageList.Count == 1)
							_2DMECoveragePoly = (MultiPolygon)NewDMECoverage.AvailableZone.Clone();
						else
							_2DMECoveragePoly = (MultiPolygon)geomOp.UnionGeometry(NewDMECoverage.AvailableZone, _2DMECoveragePoly);
					}

					Gauge1.Value++;
				}
			}

			n = _coverageList.Count;
			if (n > 0)
			{
				Gauge1.Value = 0;
				Gauge1.Maximum = ((n - 1) * n) / 2;

				for (int i = 0; i < n - 1; i++)
				{
					CDMECoverage DMECoverage0 = _coverageList[i];
					for (int j = i + 1; j < n; j++)
					{
						CDMECoverage DMECoverage1 = _coverageList[j];
						Geometry TmpPoly0 = geomOp.Intersect(DMECoverage0.AvailableZone, DMECoverage1.AvailableZone);

						if (!TmpPoly0.IsEmpty && (TmpPoly0.Type == GeometryType.Polygon || TmpPoly0.Type == GeometryType.MultiPolygon))
						{
						    //Added by Agshin
						    var availableZone = TmpPoly0;
						    if (TmpPoly0.Type == GeometryType.Polygon)
						        availableZone = new MultiPolygon { TmpPoly0 as Polygon };
						    var tmpThreeCoverage = new ThreeDmeCoverage(DMECoverage0, DMECoverage1, availableZone as MultiPolygon);
						    _threeDmeCoverageList.Add(tmpThreeCoverage);

                            ListViewItem ListItem = listThreeDmePairs.Items.Add(DMECoverage0.DME1.CallSign+"-"+DMECoverage0.DME2.CallSign);
						    ListItem.SubItems.Add(DMECoverage1.DME1.CallSign+"-"+DMECoverage1.DME2.CallSign);

                            //

                            if (_3DMECoveragePoly != null)
								_3DMECoveragePoly = (MultiPolygon)geomOp.UnionGeometry(TmpPoly0, _3DMECoveragePoly);
							else
								_3DMECoveragePoly = (MultiPolygon)TmpPoly0;
						}

						Gauge1.Value++;
					}
				}
			}

			if (cb2DME.Checked && (_2DMECoveragePoly != null))
				_elem2DMECoveragePoly = GlobalVars.gAranGraphics.DrawMultiPolygon(_2DMECoveragePoly, eFillStyle.sfsDiagonalCross, 255);

			if (cb3DME.Checked && (_3DMECoveragePoly != null))
				_elem3DMECoveragePoly = GlobalVars.gAranGraphics.DrawMultiPolygon(_3DMECoveragePoly, eFillStyle.sfsDiagonalCross, 0);

			Gauge1.Visible = false;
		}

		private void cb2DME_CheckedChanged(object sender, EventArgs e)
		{
			if (_2DMECoveragePoly != null && cb2DME.Checked)
				_elem2DMECoveragePoly = GlobalVars.gAranGraphics.DrawMultiPolygon(_2DMECoveragePoly, eFillStyle.sfsDiagonalCross, 255);
			else
				GlobalVars.gAranGraphics.SafeDeleteGraphic(_elem2DMECoveragePoly);
		}

		private void cb3DME_CheckedChanged(object sender, EventArgs e)
		{
			if (_3DMECoveragePoly != null && cb3DME.Checked)
				_elem3DMECoveragePoly = GlobalVars.gAranGraphics.DrawMultiPolygon(_3DMECoveragePoly, eFillStyle.sfsDiagonalCross, 0);
			else
				GlobalVars.gAranGraphics.SafeDeleteGraphic(_elem3DMECoveragePoly);
		}

		private void listAvailablePairs_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
		{
			int i = e.Item.Index;

			if (i < 0 && i >= _coverageList.Count)
				return;

			CDMECoverage selectedDMECoverage = _coverageList[i];

			if (!e.Item.Selected)
				selectedDMECoverage.ClearImages();
			else
				selectedDMECoverage.DrawPolygons();
		}

	    private void listThreeDmePairs_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
	    {
	        int i = e.Item.Index;

	        if (i < 0 && i >= _threeDmeCoverageList.Count)
	            return;

	       var selected3DMECoverage = _threeDmeCoverageList[i];

	        if (!e.Item.Selected)
	            selected3DMECoverage.ClearImages();
	        else
	            selected3DMECoverage.DrawPolygons();
        }

        #endregion

        #region Page III
        private void NextToPageIII()            // init page 3
		{
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_elem2DMECoveragePoly);
			GlobalVars.gAranGraphics.SafeDeleteGraphic(_elem3DMECoveragePoly);

			//========================================================
			ListView.SelectedListViewItemCollection selectionList = this.listAvailablePairs.SelectedItems;
			foreach (ListViewItem item in selectionList)
			{
				CDMECoverage selectedDMECoverage = _coverageList[item.Index];
				//if (selectedDMECoverage != null)
				selectedDMECoverage.ClearImages();
			}
            //========================================================

		    //========================================================
		    var selection3DmeList = this.listThreeDmePairs.SelectedItems;
		    foreach (ListViewItem item in selectionList)
		    {
		        var selectedDMECoverage = _threeDmeCoverageList[item.Index];
		        //if (selectedDMECoverage != null)
		        selectedDMECoverage.ClearImages();
		    }
		    //========================================================

            FillCriticalList();
			if (checkBox4.Checked)
				_elem2DMECoveragePoly = GlobalVars.gAranGraphics.DrawMultiPolygon(_2DMECoveragePoly, eFillStyle.sfsDiagonalCross, ARANFunctions.RGB(128, 128, 128));
		}

		private void FillCriticalList()
		{
			_DMEwithPairsList.Clear();

			int n = _selectedDMEList.Count;
			int m = _coverageList.Count;

			CriticalDME currCritical = default(CriticalDME);
			listCriticalDME.Items.Clear();

			Gauge1.Value = 0;
			Gauge1.Maximum = n * m;         // / 2;
			Gauge1.Visible = true;

			GeometryOperators geoop = new GeometryOperators();

			for (int i = 0; i < n; i++)
			{
				NavaidType currDME = _selectedDMEList[i];
				//currCritical = new CriticalDME() { DMEStation = currDME, Coverages = new List<CDMECoverage>() };
				currCritical.Initialize(currDME);
				MultiPolygon CoverArea = new MultiPolygon();
				MultiPolygon ExemptArea = new MultiPolygon();

				for (int j = 0; j < m; j++)
				{
					CDMECoverage currCover = _coverageList[j];
					if (currCover.DME1.Identifier == currDME.Identifier || currCover.DME2.Identifier == currDME.Identifier)
					{
						CoverArea = (MultiPolygon)geoop.UnionGeometry(currCover.AvailableZone, CoverArea);
						currCritical.Coverages.Add(currCover);
					}
					else
						ExemptArea = (MultiPolygon)geoop.UnionGeometry(currCover.AvailableZone, ExemptArea);

					Gauge1.Value++;
				}

				if (currCritical.Coverages.Count > 0)
				{
					ListViewItem ListItem = listCriticalDME.Items.Add(currDME.CallSign);
					ListItem.Tag = currDME;
					ListItem.SubItems.Add(currCritical.Coverages.Count.ToString());

					currCritical.CoverArea = CoverArea;
					currCritical.ExemptArea = ExemptArea;		// (MultiPolygon)geoop.Difference(_2DMECoveragePoly, CoverArea);
					currCritical.CriticalArea = (MultiPolygon)geoop.Difference(_2DMECoveragePoly, ExemptArea);

					//currCritical.CriticalArea1 = (MultiPolygon)geoop.Difference(CoverArea, _3DMECoveragePoly);

					_DMEwithPairsList.Add(currCritical);

					currCritical = new CriticalDME();
				}
			}

			Gauge1.Visible = false;
		}

		private void listCriticalDME_SelectedIndexChanged(object sender, EventArgs e)
		{
			//ListView.SelectedListViewItemCollection dmeList = this.listCriticalDME.SelectedItems;

			//foreach (ListViewItem item in dmeList)
			//{
			//	CriticalDME currCritical = _DMEwithPairsList[item.Index];
			//	currCritical.DrawPolygons();
			//}
		}

		private void listCriticalDME_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
		{
			int i = e.Item.Index;

			if (i < 0 && i >= _DMEwithPairsList.Count)
				return;

			CriticalDME currCritical = _DMEwithPairsList[i];

			if (!e.Item.Selected)
				currCritical.ClearImages();
			else
			{
				int flags = (checkBox1.Checked ? 1 : 0) + (checkBox2.Checked ? 2 : 0) + (checkBox3.Checked ? 4 : 0);
				currCritical.DrawPolygons(flags);
			}

			_DMEwithPairsList[i] = currCritical;
		}

		private void drawFlagsChanged(object sender, EventArgs e)
		{
			ListView.SelectedListViewItemCollection selectionList = this.listCriticalDME.SelectedItems;
			int flags = (checkBox1.Checked ? 1 : 0) + (checkBox2.Checked ? 2 : 0) + (checkBox3.Checked ? 4 : 0);

			foreach (ListViewItem item in selectionList)
			{
				CriticalDME currCritical = _DMEwithPairsList[item.Index];
				currCritical.ClearImages();
				currCritical.DrawPolygons(flags);
				_DMEwithPairsList[item.Index] = currCritical;
			}
		}

		private void checkBox4_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBox4.Checked)
			{
				_elem2DMECoveragePoly = GlobalVars.gAranGraphics.DrawMultiPolygon(_2DMECoveragePoly, eFillStyle.sfsDiagonalCross, ARANFunctions.RGB(128, 128, 128));
				drawFlagsChanged(null, null);
			}
			else
				GlobalVars.gAranGraphics.SafeDeleteGraphic(_elem2DMECoveragePoly);
		}


        #endregion

        private void btnExport_Click(object sender, EventArgs e)
        {
            var export = new ExportToGeoDatabase(_selectedDMEList,_coverageList,_threeDmeCoverageList, _DMEwithPairsList, _2DMECoveragePoly,_3DMECoveragePoly);
            if (export.Export())
                MessageBox.Show("All data saved successfully!", "Dme Dme Coverage", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            else
                MessageBox.Show("Error!Please check logs!", "Dme Dme Coverage", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
        }
    } // end TMainForm
}
