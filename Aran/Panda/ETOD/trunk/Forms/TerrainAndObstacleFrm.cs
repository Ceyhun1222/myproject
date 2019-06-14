//3186801021

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ETOD;
using ETOD.Modules;

using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;

using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Converters;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Geodatabase;

namespace ETOD.Forms
{
	public partial class CTerrainAndObstacleFrm : Form
	{
		#region variables
		OrganisationType[] OrganisationList;
		ADHPType[] ADHPList;
		RWYType[] RWYDirList;
		Airspace[] AirspaceList;
		ReportFrm reportFrm = null;

		OrganisationType SelectedOrganisation;
		ADHPType SelectedADHP;

		//1===================================================
		IPolygon Area1Poly;
		List<ObstacleType> ObstacleArea1List;

		//2===================================================
		//D3DPolygon Area2D;
		IPolygon Area2DPoly;
		List<ObstacleType> ObstacleArea2DList;
		List<Area2DATA> area2Data = new List<Area2DATA>();

		//3===================================================
		List<TaxiwayElement> taxiwayElementList = new List<TaxiwayElement>();
		List<ApronElement> apronElementList = new List<ApronElement>();
		List<IPolygon> RWYElementList = new List<IPolygon>();

		IPolygon MovementPoly;
		IPolygon RWYPoly;
		//IPolygon Area3Poly;
		ObstacleType[] ObstacleArea3List;

		//4===================================================
		List<IPolygon> Area4List;
		List<RWYType> SelectedRWYDirs = new List<RWYType>();
		List<ObstacleType> ObstacleArea4List;

		//====================================================================
		bool eventsEnabled = false;
		double[] Gridsizes = new double[] { 50, 5, 0.5, 2.5 };

		List<IRasterLayer> rasterLayers;
		#endregion

		//====================================================================

		#region Form initialization
		public CTerrainAndObstacleFrm()
		{
			InitializeComponent();
			//Functions.ConvertDistance
			string toolTipText = Gridsizes[0].ToString() + " x " + Gridsizes[0].ToString();
			toolTip1.SetToolTip(Area1Combo, toolTipText);
			toolTip1.SetToolTip(label5, toolTipText);
			toolTip1.SetToolTip(label9, toolTipText);

			toolTipText = Gridsizes[1].ToString() + " x " + Gridsizes[1].ToString();
			toolTip1.SetToolTip(Area2Combo, toolTipText);
			toolTip1.SetToolTip(label6, toolTipText);
			toolTip1.SetToolTip(label10, toolTipText);

			toolTipText = Gridsizes[2].ToString() + " x " + Gridsizes[2].ToString();
			toolTip1.SetToolTip(Area3Combo, toolTipText);
			toolTip1.SetToolTip(label7, toolTipText);
			toolTip1.SetToolTip(label11, toolTipText);

			toolTipText = Gridsizes[3].ToString() + " x " + Gridsizes[3].ToString();
			toolTip1.SetToolTip(Area4Combo, toolTipText);
			toolTip1.SetToolTip(label8, toolTipText);
			toolTip1.SetToolTip(label12, toolTipText);
		}

		private void CTerrainAndObstacleFrm_Load(object sender, EventArgs e)
		{
			OrganisationCombo.Items.Clear();

			if (DBGDBModule.FillOrganisationList(out OrganisationList) < 1)
			{
				AerodromeCombo.Items.Clear();
				//comboBox3.Items.Clear();
				return;
			}

			int i, n = OrganisationList.Length;

			for (i = 0; i < n; i++)
				OrganisationCombo.Items.Add(OrganisationList[i].Name);

			OrganisationCombo.SelectedIndex = 0;

			rasterLayers = ETODFunctions.GetValidRasterLayers();
			Area1Combo.Items.Clear();
			Area2Combo.Items.Clear();
			Area3Combo.Items.Clear();
			Area4Combo.Items.Clear();
			n = rasterLayers.Count;
			int[] GridRecoments = new int[4] { -1, -1, -1, -1 };
			double[] GridRecomentSize = new double[4];

			for (i = 0; i < n; i++)
			{
				IRasterProps rasterProps = rasterLayers[i].Raster as IRasterProps;
				IPnt Pnt = rasterProps.MeanCellSize();
				double d0 = 0.5 * (Pnt.X + Pnt.Y);	//Math.Sqrt(0.5*(Pnt.X*Pnt.X + Pnt.Y*Pnt.Y))

				for (int j = 0; j < 4; j++)
				{
					if (GridRecoments[j] < 0)
					{
						GridRecoments[j] = i;
						GridRecomentSize[j] = d0;
					}
					else
					{
						double d1 = Math.Abs(Gridsizes[j] - d0);
						double d2 = Math.Abs(Gridsizes[j] - GridRecomentSize[j]);
						if (d1 < d2)
						{
							GridRecoments[j] = i;
							GridRecomentSize[j] = d0;
						}
					}
				}

				Area1Combo.Items.Add(rasterLayers[i].Name);
				Area2Combo.Items.Add(rasterLayers[i].Name);
				Area3Combo.Items.Add(rasterLayers[i].Name);
				Area4Combo.Items.Add(rasterLayers[i].Name);
			}

			if (n > 0)
			{
				Area1Combo.SelectedIndex = GridRecoments[0];
				Area2Combo.SelectedIndex = GridRecoments[1];
				Area3Combo.SelectedIndex = GridRecoments[2];
				Area4Combo.SelectedIndex = GridRecoments[3];
			}
		}

		private void CTerrainAndObstacleFrm_FormClosing(object sender, FormClosingEventArgs e)
		{
			int i, n;

			if (reportFrm != null)
				reportFrm.Close();

			GlobalVars.CurrCmd = 0;

			n = GlobalVars.Area2Elem.Count;
			for (i = 0; i < n; i++)
				Functions.DeleteGraphicsElement(GlobalVars.Area2Elem[i]);
			GlobalVars.Area2Elem.Clear();

			Functions.DeleteGraphicsElement(GlobalVars.Area1Elem);
			Functions.DeleteGraphicsElement(GlobalVars.Area2dElem);
			Functions.DeleteGraphicsElement(GlobalVars.Area3Elem);
			Functions.DeleteGraphicsElement(GlobalVars.RWYElem);

			GlobalVars.Area1Elem = null;
			GlobalVars.Area2dElem = null;
			GlobalVars.Area3Elem = null;
			GlobalVars.RWYElem = null;

			//n = area2Data.Count;
			//for (j = 0; j < n; j++)
			//    for (i = 0; i < area2Data[j].Area2elements.Count; i++)
			//        Functions.DeleteGraphicsElement(area2Data[j].Area2elements[i]);

			n = GlobalVars.Area4Elem.Count;
			for (i = 0; i < n; i++)
				Functions.DeleteGraphicsElement(GlobalVars.Area4Elem[i]);
			GlobalVars.Area4Elem.Clear();

			GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
			DBGDBModule.CloseDB();
		}

		#endregion

		private void OrganisationCombo_SelectedIndexChanged(object sender, EventArgs e)
		{
			AerodromeCombo.Items.Clear();

			if (OrganisationCombo.SelectedIndex < 0)
			{
				lvRWY.Items.Clear();
				lvRWYDir.Items.Clear();
				//comboBox3.Items.Clear();
				return;
			}

			SelectedOrganisation = OrganisationList[OrganisationCombo.SelectedIndex];

			if (DBGDBModule.FillADHPList(out ADHPList, SelectedOrganisation.Identifier) < 1)
			{
				lvRWY.Items.Clear();
				lvRWYDir.Items.Clear();
				//comboBox3.Items.Clear();
				return;
			}

			int i, n = ADHPList.Length;

			for (i = 0; i < n; i++)
				AerodromeCombo.Items.Add(ADHPList[i].Name);

			AerodromeCombo.SelectedIndex = 0;
		}

		private void CreateMovementArea()
		{
			int i, n;
			int j, m;

			IPolygon tmpPoly1 = new Polygon() as IPolygon;
			ITopologicalOperator2 TopoOper = tmpPoly1 as ITopologicalOperator2;

			List<Taxiway> taxiwayList = DBGDBModule.GetTaxiwayList(SelectedADHP.ID);
			n = taxiwayList.Count;
			taxiwayElementList.Clear();

			for (i = 0; i < n; i++)
			{
				List<TaxiwayElement> tweList = DBGDBModule.GetTaxiwayElementList(taxiwayList[i].Identifier);
				taxiwayElementList.AddRange(tweList);
				m = tweList.Count;
				for (j = 0; j < m; j++)
				{
					IPolygon pPgnPrj = Functions.ToPrj(ConvertToEsriGeom.FromMultiPolygon(tweList[j].Extent.Geo)) as IPolygon;
					IPolygon tmpPoly0 = TopoOper.Union(pPgnPrj) as IPolygon;
					TopoOper = tmpPoly0 as ITopologicalOperator2;
					TopoOper.IsKnownSimple_2 = false;
					TopoOper.Simplify();
					tmpPoly1 = tmpPoly0;
				}
			}

			List<Apron> apronList = DBGDBModule.GetApronList(SelectedADHP.ID);
			n = apronList.Count;
			apronElementList.Clear();

			for (i = 0; i < n; i++)
			{
				List<ApronElement> aeList = DBGDBModule.GetApronElementList(apronList[i].Identifier);
				apronElementList.AddRange(aeList);
				m = aeList.Count;
				for (j = 0; j < m; j++)
				{
					IPolygon pPgnPrj = Functions.ToPrj(ConvertToEsriGeom.FromMultiPolygon(aeList[j].Extent.Geo)) as IPolygon;
					IPolygon tmpPoly0 = TopoOper.Union(pPgnPrj) as IPolygon;
					TopoOper = tmpPoly0 as ITopologicalOperator2;
					TopoOper.IsKnownSimple_2 = false;
					TopoOper.Simplify();
					tmpPoly1 = tmpPoly0;
				}
			}

			TopoOper = tmpPoly1 as ITopologicalOperator2;
			MovementPoly = TopoOper.Buffer(ETODFunctions.Area3MovementWidth) as IPolygon;

			Functions.DeleteGraphicsElement(GlobalVars.Area3Elem);
			GlobalVars.Area3Elem = Functions.DrawPolygon(MovementPoly, 255);

			//==========================================================
			RWYElementList.Clear();
			IPolygon rwyPoly;
			IPointCollection rwyPolyPoints;
			IPolyline rwyLine = new Polyline() as IPolyline;

			n = RWYDirList.Length;

			tmpPoly1.SetEmpty();

			for (i = 0; i < n; i++)
			{
				lvRWYDir.Items.Add(RWYDirList[i].Name);

				if (i > 0 && (i & 1) != 0 && RWYDirList[i].PairID == RWYDirList[i - 1].ID)
					continue;

				rwyPoly = new Polygon() as IPolygon;
				rwyPolyPoints = rwyPoly as IPointCollection;

				if (rwyPolyPoints.PointCount > 0)
					rwyPolyPoints.RemovePoints(0, rwyPolyPoints.PointCount);

				rwyLine.FromPoint = Functions.PointAlongPlane(RWYDirList[i].pPtPrj[eRWY.PtStart], RWYDirList[i].pPtPrj[eRWY.PtStart].M + 180.0, ETODFunctions.Area3MovementWidth);
				rwyLine.ToPoint = Functions.PointAlongPlane(RWYDirList[i + 1].pPtPrj[eRWY.PtStart], RWYDirList[i + 1].pPtPrj[eRWY.PtStart].M + 180.0, ETODFunctions.Area3MovementWidth);

				rwyPolyPoints.AddPoint(Functions.PointAlongPlane(rwyLine.FromPoint, RWYDirList[i].pPtPrj[eRWY.PtStart].M + 90.0, ETODFunctions.Area3RunWayWidth));
				rwyPolyPoints.AddPoint(Functions.PointAlongPlane(rwyLine.FromPoint, RWYDirList[i].pPtPrj[eRWY.PtStart].M - 90.0, ETODFunctions.Area3RunWayWidth));

				rwyPolyPoints.AddPoint(Functions.PointAlongPlane(rwyLine.ToPoint, RWYDirList[i + 1].pPtPrj[eRWY.PtStart].M + 90.0, ETODFunctions.Area3RunWayWidth));
				rwyPolyPoints.AddPoint(Functions.PointAlongPlane(rwyLine.ToPoint, RWYDirList[i + 1].pPtPrj[eRWY.PtStart].M - 90.0, ETODFunctions.Area3RunWayWidth));

				TopoOper = rwyPolyPoints as ITopologicalOperator2;
				TopoOper.IsKnownSimple_2 = false;
				TopoOper.Simplify();
				RWYElementList.Add(rwyPoly);

				tmpPoly1 = TopoOper.Union(tmpPoly1) as IPolygon;

				lvRWY.Items.Add(RWYDirList[i].Name + "/" + RWYDirList[i].PairName);
			}

			RWYPoly = tmpPoly1;

			Functions.DeleteGraphicsElement(GlobalVars.RWYElem);
			GlobalVars.RWYElem = Functions.DrawPolygon(RWYPoly, 255);
		}

		private void CreateArea2DPoly()
		{
			int i, j;

			IPolygon CurrPoly, Unions1 = null, Unions2 = null, Unions3 = null, tmpPoly;
			ITopologicalOperator2 TopoOper;
			DBGDBModule.GetAirspaceList(SelectedOrganisation.Identifier, out AirspaceList);

			for (i = 0; i < AirspaceList.Length; i++)
			{
				if (AirspaceList[i].Type == CodeAirspace.TMA)
					for (j = 0; j < AirspaceList[i].GeometryComponent.Count; j++)
					{
						if (AirspaceList[i].GeometryComponent[j].TheAirspaceVolume.HorizontalProjection != null)
						{
							Aran.Geometries.MultiPolygon pARANPolygon = AirspaceList[i].GeometryComponent[j].TheAirspaceVolume.HorizontalProjection.Geo;
							CurrPoly = Functions.ToPrj(ConvertToEsriGeom.FromMultiPolygon(pARANPolygon)) as IPolygon;

							TopoOper = (ITopologicalOperator2)CurrPoly;
							TopoOper.IsKnownSimple_2 = false;
							TopoOper.Simplify();

							IRelationalOperator relation = CurrPoly as IRelationalOperator;
							if (relation.Contains(SelectedADHP.pPtPrj))
							{
								if (Unions1 == null)
									Unions1 = CurrPoly;
								else
								{
									tmpPoly = Unions1;
									TopoOper = (ITopologicalOperator2)tmpPoly;
									Unions1 = TopoOper.Union(CurrPoly) as IPolygon;

									TopoOper = (ITopologicalOperator2)Unions1;
									TopoOper.IsKnownSimple_2 = false;
									TopoOper.Simplify();
								}
							}
						}
					}
				else if (AirspaceList[i].Type == CodeAirspace.P)
					for (j = 0; j < AirspaceList[i].GeometryComponent.Count; j++)
					{
						Aran.Geometries.MultiPolygon pARANPolygon = AirspaceList[i].GeometryComponent[j].TheAirspaceVolume.HorizontalProjection.Geo;
						CurrPoly = Functions.ToPrj(ConvertToEsriGeom.FromMultiPolygon(pARANPolygon)) as IPolygon;

						TopoOper = (ITopologicalOperator2)CurrPoly;
						TopoOper.IsKnownSimple_2 = false;
						TopoOper.Simplify();

						if (Unions2 == null)
							Unions2 = CurrPoly;
						else
						{
							tmpPoly = Unions2;
							TopoOper = (ITopologicalOperator2)tmpPoly;
							Unions2 = TopoOper.Union(CurrPoly) as IPolygon;

							TopoOper = (ITopologicalOperator2)Unions2;
							TopoOper.IsKnownSimple_2 = false;
							TopoOper.Simplify();
						}
					}
				else if (AirspaceList[i].Type == CodeAirspace.FIR)
					for (j = 0; j < AirspaceList[i].GeometryComponent.Count; j++)
					{
						if (AirspaceList[i].GeometryComponent[j].TheAirspaceVolume.HorizontalProjection != null)
						{
							Aran.Geometries.MultiPolygon pARANPolygon = AirspaceList[i].GeometryComponent[j].TheAirspaceVolume.HorizontalProjection.Geo;
							CurrPoly = Functions.ToPrj(ConvertToEsriGeom.FromMultiPolygon(pARANPolygon)) as IPolygon;

							TopoOper = (ITopologicalOperator2)CurrPoly;
							TopoOper.IsKnownSimple_2 = false;
							TopoOper.Simplify();

							IRelationalOperator relation = CurrPoly as IRelationalOperator;
							if (relation.Contains(SelectedADHP.pPtPrj))
							{
								if (Unions3 == null)
									Unions3 = CurrPoly;
								else
								{
									tmpPoly = Unions3;
									TopoOper = (ITopologicalOperator2)tmpPoly;
									Unions1 = TopoOper.Union(CurrPoly) as IPolygon;

									TopoOper = (ITopologicalOperator2)Unions1;
									TopoOper.IsKnownSimple_2 = false;
									TopoOper.Simplify();
								}
							}
						}
					}
			}

			Area2DPoly = Unions1;

			if (Unions1 == null)
				return;

			IPolygon circle = Functions.CreatePrjCircle(SelectedADHP.pPtPrj, ETODFunctions.Area2DRadius);
			TopoOper = (ITopologicalOperator2)Unions1;
			Unions1 = TopoOper.Intersect(circle, esriGeometryDimension.esriGeometry2Dimension) as IPolygon;

			TopoOper = (ITopologicalOperator2)Unions1;
			TopoOper.IsKnownSimple_2 = false;
			TopoOper.Simplify();

			if (Unions2 != null)
			{
				TopoOper = (ITopologicalOperator2)Unions1;
				Unions1 = TopoOper.Difference(Unions2) as IPolygon;

				TopoOper = (ITopologicalOperator2)Unions1;
				TopoOper.IsKnownSimple_2 = false;
				TopoOper.Simplify();
			}

			Area2DPoly = Unions1;

			Functions.DeleteGraphicsElement(GlobalVars.Area2dElem);
			GlobalVars.Area2dElem = Functions.DrawPolygon(Area2DPoly, -1, true, 4);

			Area1Poly = Unions3;
			Functions.DeleteGraphicsElement(GlobalVars.Area1Elem);
			GlobalVars.Area1Elem = null;

			if (Area1Poly != null)
				GlobalVars.Area1Elem = Functions.DrawPolygon(Area1Poly, -1, true, 4);
		}

		private void AerodromeCombo_SelectedIndexChanged(object sender, EventArgs e)
		{
			//IPolygon Unions1 = null, Unions2 = null, CurrPoly, tmpPoly;
			//ITopologicalOperator2 TopoOper;

			eventsEnabled = false;

			lvRWY.Clear();
			lvRWYDir.Clear();

			if (AerodromeCombo.SelectedIndex < 0)
				return;

			SelectedADHP = ADHPList[AerodromeCombo.SelectedIndex];

			if (DBGDBModule.FillRWYList(out RWYDirList, SelectedADHP) < 1)
				return;

			//==========================================================
			/*

			IPolygon tmpPoly1 = new Polygon() as IPolygon;
			TopoOper = tmpPoly1 as ITopologicalOperator2;

			List<Taxiway> taxiwayList = DBGDBModule.GetTaxiwayList(SelectedADHP.ID);
			n = taxiwayList.Count;
			taxiwayElementList.Clear();


			for (i = 0; i < n; i++)
			{
				List<TaxiwayElement> tweList = DBGDBModule.GetTaxiwayElementList(taxiwayList[i].Identifier);
				taxiwayElementList.AddRange(tweList);
				m = tweList.Count;
				for (j = 0; j < m; j++)
				{
					IPolygon pPgnPrj = Functions.ToPrj(ConvertToEsriGeom.FromPolygon(tweList[j].Extent.Geo)) as IPolygon;
					IPolygon tmpPoly0 = TopoOper.Union(pPgnPrj) as IPolygon;
					TopoOper = tmpPoly0 as ITopologicalOperator2;
					TopoOper.IsKnownSimple_2 = false;
					TopoOper.Simplify();
					tmpPoly1 = tmpPoly0;

				}
			}

			List<Apron> apronList = DBGDBModule.GetApronList(SelectedADHP.ID);
			n = apronList.Count;
			apronElementList.Clear();

			for (i = 0; i < n; i++)
			{
				List<ApronElement> aeList = DBGDBModule.GetApronElementList(apronList[i].Identifier);
				apronElementList.AddRange(aeList);
				m = aeList.Count;
				for (j = 0; j < m; j++)
				{
					IPolygon pPgnPrj = Functions.ToPrj(ConvertToEsriGeom.FromPolygon(aeList[j].Extent.Geo)) as IPolygon;
					IPolygon tmpPoly0 = TopoOper.Union(pPgnPrj) as IPolygon;
					TopoOper = tmpPoly0 as ITopologicalOperator2;
					TopoOper.IsKnownSimple_2 = false;
					TopoOper.Simplify();
					tmpPoly1 = tmpPoly0;
				}
			}

			TopoOper = tmpPoly1 as ITopologicalOperator2;
			MovementPoly = TopoOper.Buffer(ETODFunctions.Area3MovementWidth) as IPolygon;

			if (Area3Elem != null)
				Functions.DeleteGraphicsElement(Area3Elem);
			Area3Elem = Functions.DrawPolygon(MovementPoly, 255);

			//==========================================================
			RWYElementList.Clear();
			IPolygon rwyPoly;
			IPointCollection rwyPolyPoints;
			IPolyline rwyLine = new Polyline() as IPolyline;

			n = RWYDirList.Length;

			tmpPoly1.SetEmpty();

			for (i = 0; i < n; i++)
			{
				lvRWYDir.Items.Add(RWYDirList[i].Name);

				if (i > 0 && (i & 1) != 0 && RWYDirList[i].PairID == RWYDirList[i - 1].ID)
					continue;

				rwyPoly = new Polygon() as IPolygon;
				rwyPolyPoints = rwyPoly as IPointCollection;

				if (rwyPolyPoints.PointCount>0)
					rwyPolyPoints.RemovePoints(0, rwyPolyPoints.PointCount);

				rwyLine.FromPoint = Functions.PointAlongPlane(RWYDirList[i].pPtPrj[eRWY.PtStart], RWYDirList[i].pPtPrj[eRWY.PtStart].M+180.0, ETODFunctions.Area3MovementWidth);
				rwyLine.ToPoint = Functions.PointAlongPlane(RWYDirList[i + 1].pPtPrj[eRWY.PtStart], RWYDirList[i + 1].pPtPrj[eRWY.PtStart].M + 180.0, ETODFunctions.Area3MovementWidth);

				rwyPolyPoints.AddPoint(Functions.PointAlongPlane(rwyLine.FromPoint, RWYDirList[i].pPtPrj[eRWY.PtStart].M + 90.0, ETODFunctions.Area3RunWayWidth));
				rwyPolyPoints.AddPoint(Functions.PointAlongPlane(rwyLine.FromPoint, RWYDirList[i].pPtPrj[eRWY.PtStart].M - 90.0, ETODFunctions.Area3RunWayWidth));

				rwyPolyPoints.AddPoint(Functions.PointAlongPlane(rwyLine.ToPoint, RWYDirList[i + 1].pPtPrj[eRWY.PtStart].M + 90.0, ETODFunctions.Area3RunWayWidth));
				rwyPolyPoints.AddPoint(Functions.PointAlongPlane(rwyLine.ToPoint, RWYDirList[i + 1].pPtPrj[eRWY.PtStart].M - 90.0, ETODFunctions.Area3RunWayWidth));

				TopoOper = rwyPolyPoints as ITopologicalOperator2;
				TopoOper.IsKnownSimple_2 = false;
				TopoOper.Simplify();
				RWYElementList.Add(rwyPoly);

				tmpPoly1 = TopoOper.Union(tmpPoly1) as IPolygon;

				lvRWY.Items.Add(RWYDirList[i].Name + "/" + RWYDirList[i].PairName);
			}

			RWYPoly = tmpPoly1;

			if (RWYElem != null)
				Functions.DeleteGraphicsElement(RWYElem);
			RWYElem = Functions.DrawPolygon(RWYPoly, 255);

			// Area2D ===========================================

			int i, j, n;

			DBGDBModule.GetAirspaceList(out AirspaceList);

			for (i = 0; i < AirspaceList.Length; i++)
			{
				if (AirspaceList[i].Type == CodeAirspace.TMA)
					for (j = 0; j < AirspaceList[i].GeometryComponent.Count; j++)
					{
						if (AirspaceList[i].GeometryComponent[j].TheAirspaceVolume.HorizontalProjection != null)
						{
							Aran.Geometries.MultiPolygon pARANPolygon = AirspaceList[i].GeometryComponent[j].TheAirspaceVolume.HorizontalProjection.Geo;
							CurrPoly = Functions.ToPrj(ConvertToEsriGeom.FromPolygon(pARANPolygon)) as IPolygon;

							TopoOper = (ITopologicalOperator2)CurrPoly;
							TopoOper.IsKnownSimple_2 = false;
							TopoOper.Simplify();

							IRelationalOperator relation = CurrPoly as IRelationalOperator;
							if (relation.Contains(SelectedADHP.pPtPrj))
							{
								if (Unions1 == null)
									Unions1 = CurrPoly;
								else
								{
									tmpPoly = Unions1;
									TopoOper = (ITopologicalOperator2)tmpPoly;
									Unions1 = TopoOper.Union(CurrPoly) as IPolygon;

									TopoOper = (ITopologicalOperator2)Unions1;
									TopoOper.IsKnownSimple_2 = false;
									TopoOper.Simplify();
								}
							}
						}
					}
				else if (AirspaceList[i].Type == CodeAirspace.P)
					for (j = 0; j < AirspaceList[i].GeometryComponent.Count; j++)
					{
						Aran.Geometries.MultiPolygon pARANPolygon = AirspaceList[i].GeometryComponent[j].TheAirspaceVolume.HorizontalProjection.Geo;
						CurrPoly = Functions.ToPrj(ConvertToEsriGeom.FromPolygon(pARANPolygon)) as IPolygon;

						TopoOper = (ITopologicalOperator2)CurrPoly;
						TopoOper.IsKnownSimple_2 = false;
						TopoOper.Simplify();

						if (Unions2 == null)
							Unions2 = CurrPoly;
						else
						{
							tmpPoly = Unions2;
							TopoOper = (ITopologicalOperator2)tmpPoly;
							Unions2 = TopoOper.Union(CurrPoly) as IPolygon;

							TopoOper = (ITopologicalOperator2)Unions2;
							TopoOper.IsKnownSimple_2 = false;
							TopoOper.Simplify();
						}
					}
			}

			Area2DPoly = Unions1;

			if (Unions1 == null)
				return;

			IPolygon circle = Functions.CreatePrjCircle(SelectedADHP.pPtPrj, ETODFunctions.Area2DRadius);
			TopoOper = (ITopologicalOperator2)Unions1;
			Unions1 = TopoOper.Intersect(circle, esriGeometryDimension.esriGeometry2Dimension) as IPolygon;

			TopoOper = (ITopologicalOperator2)Unions1;
			TopoOper.IsKnownSimple_2 = false;
			TopoOper.Simplify();

			if (Unions2 != null)
			{
				TopoOper = (ITopologicalOperator2)Unions1;
				Unions1 = TopoOper.Difference(Unions2) as IPolygon;

				TopoOper = (ITopologicalOperator2)Unions1;
				TopoOper.IsKnownSimple_2 = false;
				TopoOper.Simplify();
			}

			Area2DPoly = Unions1;

			if (Area2dElem != null)
				Functions.DeleteGraphicsElement(Area2dElem);

			Area2dElem = Functions.DrawPolygon(Area2DPoly, -1, true, 4);*/
			//=============================================== Area2D

			CreateMovementArea();
			CreateArea2DPoly();

			ObstacleArea1List = DBGDBModule.GetObstaclesOrgID(SelectedOrganisation.Identifier);
			ObstacleArea2DList = ETODFunctions.ExtractAre2Obstacles(ObstacleArea1List, Area2DPoly);

			int i, n = lvRWY.Items.Count;
			ListViewItem itmX;

			for (i = 0; i < n; i++)
			{
				itmX = lvRWY.Items[i];
				itmX.Checked = true;
			}

			n = lvRWYDir.Items.Count;
			for (i = 0; i < n; i++)
			{
				itmX = lvRWYDir.Items[i];
				itmX.Checked = true;
			}
			eventsEnabled = true;

			lvRWY_ItemChecked(lvRWY, new ItemCheckedEventArgs(null));
			lvRWYDir_ItemChecked(lvRWYDir, new ItemCheckedEventArgs(null));
		}

		//private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
		//{
			//int i;

			//for (i = 0; i < Area2elements.Count; i++)
			//    Functions.DeleteGraphicsElement(Area2elements[i]);

			//for (i = 0; i < Area4elements.Count; i++)
			//    Functions.DeleteGraphicsElement(Area4elements[i]);

			//Area2elements.Clear();

			//i = 2 * comboBox3.SelectedIndex;
			//int j = i + 1;

			//SelectedRWYDirs.Clear();
			//SelectedRWYDirs.Add(RWYDirList[i]);
			//SelectedRWYDirs.Add(RWYDirList[j]);

			//Area2 = ETODFunctions.CreateArea2(RWYDirList[i], RWYDirList[j]);
			//ETODFunctions.AnaliseArea2Obstacles(ref ObstacleArea2DList, Area2);
			//Area4List = ETODFunctions.CreateArea4(SelectedRWYDirs);

			//ObstacleArea4List = ETODFunctions.GetArea4Obstacles(ObstacleArea2DList, Area4List, SelectedRWYDirs);

			//for (i = 0; i < Area2.Count; i++)
			//{
			//    Area2elements.Add(Functions.DrawPolygon(Area2[i].Polygon, -1, true, 2));
			//    if (Area2[i].IsComplex)
			//        Area2elements.Add(Functions.DrawPolygon((Area2[i] as D3DComplex).ConusPolygon));
			//}

			//for (i = 0; i < Area4List.Count; i++)
			//    Area4elements.Add(Functions.DrawPolygon(Area4List[i], -1, true, 2));

			//if (reportFrm == null)
			//{
			//    reportFrm = new ReportFrm();
			//    reportFrm.SetBtn(ReportBtn, 0);
			//}

			//reportFrm.FillArea2ObstacleList(ObstacleArea2DList);
			//reportFrm.FillArea4ObstacleList(ObstacleArea4List);
		//}

		private void lvRWY_ItemChecked(object sender, ItemCheckedEventArgs e)
		{
			if (!eventsEnabled)
				return;

			int i, j, k, n;

			n = GlobalVars.Area2Elem.Count;
			for (i = 0; i < n; i++)
				Functions.DeleteGraphicsElement(GlobalVars.Area2Elem[i]);
			GlobalVars.Area2Elem.Clear();

			//for (j = 0; j < area2Data.Count; j++)
			//    for (i = 0; i < area2Data[j].Area2elements.Count; i++)
			//        Functions.DeleteGraphicsElement(area2Data[j].Area2elements[i]);

			area2Data.Clear();

			n = lvRWY.Items.Count;

			for (k = 0; k < n; k++)
			{
				if (lvRWY.Items[k].Checked)
				{
					i = 2 * k;
					j = i + 1;

					Area2DATA tmpArea2Data = new Area2DATA();

					tmpArea2Data.RWYName = RWYDirList[i].Name + "/" + RWYDirList[i].PairName;

					tmpArea2Data.Area2 = ETODFunctions.CreateArea2(RWYDirList[i], RWYDirList[j]);
					ETODFunctions.AnaliseArea2Obstacles(ObstacleArea2DList, out tmpArea2Data.ObstacleList, tmpArea2Data.Area2);

					for (i = 0; i < tmpArea2Data.Area2.Count; i++)
					{
						GlobalVars.Area2Elem.Add(Functions.DrawPolygon(tmpArea2Data.Area2[i].Polygon, -1, true, 2));
						if (tmpArea2Data.Area2[i].IsComplex)
							GlobalVars.Area2Elem.Add(Functions.DrawPolygon((tmpArea2Data.Area2[i] as D3DComplex).ConusPolygon));
					}

					area2Data.Add(tmpArea2Data);
				}
			}

			if (reportFrm == null)
			{
				reportFrm = new ReportFrm();
				reportFrm.SetBtn(ReportBtn, 0);
			}

			reportFrm.FillArea2ObstacleList(area2Data);
		}

		private void lvRWYDir_ItemChecked(object sender, ItemCheckedEventArgs e)
		{
			if (!eventsEnabled)
				return;

			int i, n = GlobalVars.Area4Elem.Count;

			for (i = 0; i < n; i++)
				Functions.DeleteGraphicsElement(GlobalVars.Area4Elem[i]);

			SelectedRWYDirs.Clear();

			n = lvRWYDir.Items.Count;

			for (i = 0; i < n; i++)
				if (lvRWYDir.Items[i].Checked)
					SelectedRWYDirs.Add(RWYDirList[i]);

			Area4List = ETODFunctions.CreateArea4(SelectedRWYDirs);
			ObstacleArea4List = ETODFunctions.GetArea4Obstacles(ObstacleArea2DList, Area4List, SelectedRWYDirs);

			for (i = 0; i < Area4List.Count; i++)
				GlobalVars.Area4Elem.Add(Functions.DrawPolygon(Area4List[i], -1, true, 2));

			if (reportFrm == null)
			{
				reportFrm = new ReportFrm();
				reportFrm.SetBtn(ReportBtn, 0);
			}

			reportFrm.FillArea4ObstacleList(ObstacleArea4List);
		}

		private void ReportBtn_CheckedChanged(object sender, EventArgs e)
		{
			if (reportFrm == null)
				return;

			if (ReportBtn.Checked)
				reportFrm.Show(GlobalVars.m_Win32Window);
			else
				reportFrm.Hide();
		}

		private void button1_Click(object sender, EventArgs e)
		{

		}


		#region Grid Combos

		private string GetResolution(IRaster raster)
		{
			IRasterProps rasterProps = raster as IRasterProps;
			IPnt Pnt = rasterProps.MeanCellSize();
			return Math.Round(Pnt.X, 4).ToString() + " x " + Math.Round(Pnt.Y, 4).ToString();

			//int GridWidth = rasterProps.Width;
			//int GridHeight = rasterProps.Height;
			//cellsize := Max(Pnt.X, Pnt.Y);

			//IRaster2 raster2 = rasterLayers[index] as IRaster2;
			//PixelBlockCursor := CoPixelBlockCursor.Create as IPixelBlockCursor;
			//PixelBlockCursor.InitByRaster(raster2 as IRaster);
			//PixelBlockCursor.ScanMode := 0;

			//PixelBlockCursor.UpdateBlockSize(GridWidth, GridHeight);
			//PixelBlock := PixelBlockCursor.NextBlock(L, T, W, H);
		}

		private void Area1Combo_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (Area1Combo.SelectedIndex < 0)
				return;
			label9.Text = GetResolution(rasterLayers[Area1Combo.SelectedIndex].Raster);
		}

		private void Area2Combo_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (Area2Combo.SelectedIndex < 0)
				return;
			label10.Text = GetResolution(rasterLayers[Area2Combo.SelectedIndex].Raster);
		}

		private void Area3Combo_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (Area3Combo.SelectedIndex < 0)
				return;
			label11.Text = GetResolution(rasterLayers[Area3Combo.SelectedIndex].Raster);
		}

		private void Area4Combo_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (Area4Combo.SelectedIndex < 0)
				return;
			label12.Text = GetResolution(rasterLayers[Area4Combo.SelectedIndex].Raster);
		}
		#endregion
	}
}
