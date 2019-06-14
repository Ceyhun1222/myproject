using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Objects;
using Aran.Queries;

using Aran.AranEnvironment.Symbols;
using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.PANDA.Common;
using Aran.PANDA.Constants;

using Aran.PANDA.RNAV.Departure.Properties;
using Aran.AranEnvironment;
using Aran.Aim.Data;

namespace Aran.PANDA.RNAV.Departure
{
	public partial class DepartureForm : Form
	{
		const double maxTurnAngle = 135.0;
		private const int EM_SETREADONLY = 0x00CF;
		private const int GW_CHILD = 5;
		private const int GW_HWNDNEXT = 2;

		private const int saveFeatureCount = 5;

		private IScreenCapture screenCapture;
		private StandardInstrumentDeparture Procedure;

		#region variables

		private double[] EnrouteMOCValues = { 300.0, 450.0, 600.0 };

		private ePBNClass[] firstTurnPBNClass = new ePBNClass[] { ePBNClass.RNAV1, ePBNClass.RNAV2, ePBNClass.RNP1 };
		private ePBNClass[] regularTurnPBNClass = new ePBNClass[] { ePBNClass.RNAV1, ePBNClass.RNAV2, ePBNClass.RNP1, ePBNClass.RNP4, ePBNClass.RNAV5 };

		private CodeSegmentPath[] AtHeightPathAndTerminations = new CodeSegmentPath[] { CodeSegmentPath.CF, CodeSegmentPath.DF };
		private CodeSegmentPath[] FlyOverPathAndTerminations = new CodeSegmentPath[] { CodeSegmentPath.TF, CodeSegmentPath.DF };
		private CodeSegmentPath[] FlyByPathAndTerminations = new CodeSegmentPath[] { CodeSegmentPath.TF };

		private double[] heights = new double[] { 304.8, 914.4, double.MaxValue };
		private double[] banks = new double[] { 15, 20, 25 };

		private int CurrPage
		{
			set
			{
				MultiPage1.SelectedIndex = value;
				Text = Resources.str00033 + "   [" + Label1[value].Text + "]";
				this.HelpContextID = 4000 + 100 * (value + 1);

				PrevBtn.Enabled = value > 0;
				NextBtn.Enabled = value < MultiPage1.TabPages.Count - 1;
				OkBtn.Enabled = value == MultiPage1.TabPages.Count - 1;

				for (int i = 0; i < Label1.Length; i++)
				{
					Label1[i].ForeColor = System.Drawing.Color.Silver;
					Label1[i].Font = new System.Drawing.Font(Label1[i].Font, System.Drawing.FontStyle.Regular);
				}

				Label1[value].ForeColor = System.Drawing.Color.FromArgb(0XFF8000);
				Label1[value].Font = new System.Drawing.Font(Label1[value].Font, System.Drawing.FontStyle.Bold);
			}

			get { return MultiPage1.SelectedIndex; }
		}

		private bool bFormInitialised = false;
		private bool Report = false;
		private int HelpContextID = 4100;

		private Label[] Label1;
		CReports ReportsFrm;

		private ReportFile AccurRep;
		private ReportFile GuidLogRep;
		private ReportFile GuidProtRep;
		private ReportFile GuidGeomRep;

		private aircraftCategory _aircat;
		private double _accelerationAltitude;
		private double RMin;
		private double DepDir;
		private double DepAzt;

		private double MOCLimit;
		private double minDistPDG;
		private double MinTermDist;
		private double additionR;
		private double _IAS;
		private double _BankAngle;

		private RWYType SelectedRWY;
		private Point ptDerPrj;
		private Point ptDerGeo;
		private Point ptCenter;
		private MultiPolygon pCircle;

		private Interval speedLimits;

		private decimal SpinButton1Min,
						SpinButton1Max;

		private Straight straight;
		private Segment1Term segment1Term;
		private Transitions transitions;

		private Interval UpDown301Range;

		WayPoint _AddedFIX;
		List<WayPoint> _SignificantPoints;

		#endregion

		#region properties

		//double EPTMin;

		TerminationType _terminationType;
		private TerminationType terminationType
		{
			get { return _terminationType; }
			set
			{
				_terminationType = value;
				segment1Term.terminationType = value;

				LegDep leg = segment1Term.Leg;
				RadioButton rbDistance;

				double fTmp = segment1Term.AppliedAltitude;
				double kkToFixheight = 0.0;

				switch (_terminationType)
				{
					default:
					case TerminationType.AtHeight:
						segment1Term.ConstructAltitude = segment1Term.AppliedAltitude;
						segment1Term.NomLineAltitude = segment1Term.AppliedAltitude;

						for (int i = 0; i < heights.Length; i++)
							if (segment1Term.NomLineAltitude - ptDerPrj.Z < heights[i])
							{
								comboBox206.SelectedIndex = i;
								break;
							}

						rbDistance = radioButton202;

						if (comboBox201.SelectedIndex != 0)
							fTmp -= ptDerPrj.Z;

						label231.Visible = false;
						textBox214.Visible = false;

						label232.Visible = false;
						label233.Visible = false;
						textBox215.Visible = false;

						textBox201.Text = GlobalVars.unitConverter.HeightToDisplayUnits(fTmp, eRoundMode.NEAREST).ToString();
						textBox201.Tag = textBox201.Text;

						textBox202.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(segment1Term.TermDistance, eRoundMode.NEAREST).ToString();

						break;

					case TerminationType.AtWPT:
						label232.Visible = comboBox203.SelectedIndex == 0;
						label233.Visible = comboBox203.SelectedIndex == 0;
						textBox215.Visible = comboBox203.SelectedIndex == 0;

						rbDistance = radioButton205;
						if (comboBox202.SelectedIndex != 0)
							fTmp -= ptDerPrj.Z;

						textBox206.Text = GlobalVars.unitConverter.HeightToDisplayUnits(fTmp, eRoundMode.NEAREST).ToString();
						textBox206.Tag = textBox206.Text;

						textBox212.Text = GlobalVars.unitConverter.HeightToDisplayUnits(segment1Term.ConstructAltitude, eRoundMode.NEAREST).ToString();
						textBox212.Tag = textBox212.Text;

						textBox207.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(segment1Term.TermDistance).ToString();

						textBox211.Tag = null;
						textBox211_Validating(textBox211, new CancelEventArgs());
						//fillComboBox205();
						kkToFixheight = leg.EndFIX.EPT * _appliedPDG;
						break;
				}

				if (rbDistance.Checked)
					HeightAtEndFIX = segment1Term.AppliedAltitude + kkToFixheight - ptDerPrj.Z;     //_heightAtEndFIX;
				else
					terminationDistance = segment1Term.TermDistance;                                // _terminationDistance;
			}
		}

		double _heightAtEndFIX;
		double HeightAtEndFIX
		{
			get { return _heightAtEndFIX; }

			set
			{
				//ComboBox cbHeigtAltitude;
				RadioButton rbDistance, rbAppliedPDG;
				TextBox tbHeigtAltitude;
				LegDep leg = segment1Term.Leg;

				double minHeightLimit = GlobalVars.constants.Pansops[ePANSOPSData.dpGui_Ar1].Value;
				double maxHeightLimit;
				double kkToFixheight = 0.0;

				switch (_terminationType)
				{
					default:
					case TerminationType.AtHeight:
						rbDistance = radioButton202;
						rbAppliedPDG = radioButton203;
						//cbHeigtAltitude = comboBox201;
						tbHeigtAltitude = textBox201;

						if (rbDistance.Checked)
							maxHeightLimit = GlobalVars.constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value + 1.5 * GlobalVars.constants.Pansops[ePANSOPSData.dpStr_Gui_dist].Value * appliedPDG;
						else if (rbAppliedPDG.Checked)
						{
							maxHeightLimit = GlobalVars.constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value + _terminationDistance * GlobalVars.constants.Pansops[ePANSOPSData.dpMaxPosPDG].Value;
							//minHeightLimit = GlobalVars.constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value + _terminationDistance  * GlobalVars.constants.Pansops[ePANSOPSData.dpPDG_Nom].Value;
							minHeightLimit = Math.Max(minHeightLimit, GlobalVars.constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value + _terminationDistance * segment1Term.MinPDG);
						}
						else
							maxHeightLimit = GlobalVars.constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value + 1.5 * GlobalVars.constants.Pansops[ePANSOPSData.dpStr_Gui_dist].Value * GlobalVars.constants.Pansops[ePANSOPSData.dpMaxPosPDG].Value;

						ObstacleContainer obsstMaxReqTNH;
						obsstMaxReqTNH.Obstacles = new Obstacle[1];
						obsstMaxReqTNH.Parts = new ObstacleData[1];
						obsstMaxReqTNH.Obstacles[0].ID = -1;
						obsstMaxReqTNH.Parts[0].Index = -1;

						double maxReqTNH = double.MinValue;

						foreach (ObstacleData obst in segment1Term.InnerObstacleList.Parts)
							if (obst.ReqTNH > maxReqTNH)
							{
								obsstMaxReqTNH.Parts[0] = obst;
								obsstMaxReqTNH.Obstacles[0] = segment1Term.InnerObstacleList.Obstacles[obst.Owner];
								maxReqTNH = obst.ReqTNH;
							}
						//if (checkBox201.Checked)
						{
							if (obsstMaxReqTNH.Parts[0].Dist < GlobalVars.constants.Pansops[ePANSOPSData.dpObsClr].Value / GlobalVars.constants.Pansops[ePANSOPSData.dpMOC].Value)
								minHeightLimit = Math.Max(minHeightLimit, obsstMaxReqTNH.Parts[0].Height + GlobalVars.constants.Pansops[ePANSOPSData.dpObsClr].Value);
						}

						break;

					case TerminationType.AtWPT:
						rbDistance = radioButton205;
						rbAppliedPDG = radioButton206;
						//cbHeigtAltitude = comboBox202;
						tbHeigtAltitude = textBox206;

						kkToFixheight = Functions.MinFlybyDistByHeightAtKKLine(GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value, ptDerPrj.Z, _appliedPDG, segment1Term.PlannedMaxTurnAngle, leg.EndFIX) * _appliedPDG;
						minHeightLimit += kkToFixheight;

						maxHeightLimit = 3000.0;

						break;
				}

				if (value > maxHeightLimit)
					value = maxHeightLimit;

				if (value < minHeightLimit)
					value = minHeightLimit;

				//if (_heightAtEndFIX != value)
				//{

				//	if (cbHeigtAltitude.SelectedIndex == 0)
				//		value += ptDerPrj.Z;

				//	tbHeigtAltitude.Text = GlobalVars.unitConverter.HeightToDisplayUnits(value, eRoundMode.NERAEST).ToString();
				//	tbHeigtAltitude.Tag = tbHeigtAltitude.Text;
				//}

				double EPT = 0.0;
				double hAtKK = value - kkToFixheight;

				if (_terminationType == TerminationType.AtWPT)
				{
					EPT = Functions.MinFlybyDistByHeightAtFIX(GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value, ptDerPrj.Z, _appliedPDG, segment1Term.PlannedMaxTurnAngle, leg.EndFIX, value);
					kkToFixheight = EPT * _appliedPDG;
					hAtKK = value - kkToFixheight;

					if (hAtKK < GlobalVars.constants.Pansops[ePANSOPSData.dpNGui_Ar1].Value)
					{
						hAtKK = GlobalVars.constants.Pansops[ePANSOPSData.dpNGui_Ar1].Value;
						value = kkToFixheight + hAtKK;
						EPT = Functions.MinFlybyDistByHeightAtFIX(GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value, ptDerPrj.Z, _appliedPDG, segment1Term.PlannedMaxTurnAngle, leg.EndFIX, value);
					}

					//while (hAtKK < GlobalVars.constants.Pansops[ePANSOPSData.dpNGui_Ar1].Value)
					//{
					//	value = kkToFixheight + GlobalVars.constants.Pansops[ePANSOPSData.dpNGui_Ar1].Value;

					//	EPT = Functions.MinFlybyDistByHeightAtFIX(GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value, ptDerPrj.Z, _appliedPDG, segment1Term.PlannedMaxTurnAngle, leg.EndFIX, value);
					//	kkToFixheight = EPT * _appliedPDG;
					//	hAtKK = value - kkToFixheight;
					//	if (Math.Abs(hAtKK - GlobalVars.constants.Pansops[ePANSOPSData.dpNGui_Ar1].Value) < ARANMath.EpsilonDistance)
					//		break;
					//}
				}
				//else if (hAtKK < GlobalVars.constants.Pansops[ePANSOPSData.dpNGui_Ar1].Value)
				//{
				//	hAtKK = GlobalVars.constants.Pansops[ePANSOPSData.dpNGui_Ar1].Value;
				//	value = hAtKK;
				//}

				_heightAtEndFIX = value;

				segment1Term.AppliedAltitude = hAtKK + ptDerPrj.Z;

				// = (height - GlobalVars.constants.Pansops[ePANSOPSData.dpH_abv_DER].Value) / straight.PDG

				if (rbDistance.Checked)
					terminationDistance = (_heightAtEndFIX - GlobalVars.constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value) / _appliedPDG;
				else if (rbAppliedPDG.Checked)
				{
					minDistPDG = (_heightAtEndFIX - GlobalVars.constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value) / _terminationDistance; // segment1Term.TermDistance;
																																			//minDistPDG = Math.Round(minDistPDG + 0.00004999, 4);
																																			//if (appliedPDG < minDistPDG)
					appliedPDG = minDistPDG;
				}

				if (_terminationType == TerminationType.AtHeight)
					for (int i = 0; i < heights.Length; i++)
						if (segment1Term.NomLineAltitude - ptDerPrj.Z < heights[i])
						{
							comboBox206.SelectedIndex = i;
							break;
						}

				//leg = segment1Term.Leg;
				//EPT = Functions.MinFlybyDistByHeightAtFIX(GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value, ptDerPrj.Z, _appliedPDG, segment1Term.PlannedMaxTurnAngle, leg.EndFIX, value);

				//textBox207.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_terminationDistance, eRoundMode.NERAEST).ToString();
				textBox209.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(segment1Term.Leg.EndFIX.ConstructTAS, eRoundMode.NEAREST).ToString();
				textBox210.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_terminationDistance - EPT, eRoundMode.NEAREST).ToString();//leg.EndFIX.EPT
				textBox213.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(segment1Term.Leg.EndFIX.ConstructTurnRadius, eRoundMode.NEAREST).ToString();
				textBox212.Text = GlobalVars.unitConverter.HeightToDisplayUnits(segment1Term.ConstructAltitude, eRoundMode.NEAREST).ToString();
				textBox212.Tag = textBox212.Text;

				double drr = leg.EndFIX.ConstructTurnRadius * Math.Tan(0.5 * segment1Term.PlannedMaxTurnAngle);
				textBox215.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(drr, eRoundMode.NEAREST).ToString();  //leg.EndFIX.EPT


				comboBox201_SelectedIndexChanged(comboBox201, null);
			}
		}

		//double _terminationHeight;
		//private double terminationHeight
		//{
		//	get { return _terminationHeight; }
		//	set
		//	{
		//		RadioButton rbDistance, rbAppliedPDG;
		//		ComboBox cbHeigtAltitude;
		//		TextBox tbHeigtAltitude;
		//		LegDep leg = segment1Term.Leg;

		//		double minHeightLimit = GlobalVars.constants.Pansops[ePANSOPSData.dpGui_Ar1].Value;
		//		double maxHeightLimit;

		//		switch (_terminationType)
		//		{
		//			default:
		//			case TerminationType.AtHeight:
		//				rbDistance = radioButton202;
		//				rbAppliedPDG = radioButton203;
		//				cbHeigtAltitude = comboBox201;
		//				tbHeigtAltitude = textBox201;

		//				if (rbDistance.Checked)
		//				{
		//					maxHeightLimit = GlobalVars.constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value + 1.5 * GlobalVars.constants.Pansops[ePANSOPSData.dpStr_Gui_dist].Value * appliedPDG;
		//				}
		//				else if (rbAppliedPDG.Checked)
		//				{
		//					maxHeightLimit = GlobalVars.constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value + (_terminationDistance - leg.EndFIX.EPT) * GlobalVars.constants.Pansops[ePANSOPSData.dpMaxPosPDG].Value;
		//					//minHeightLimit = GlobalVars.constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value + (_terminationDistance - leg.EndFIX.EPT) * GlobalVars.constants.Pansops[ePANSOPSData.dpPDG_Nom].Value;
		//					minHeightLimit = GlobalVars.constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value + (_terminationDistance - leg.EndFIX.EPT) * segment1Term.MinPDG;
		//				}
		//				else
		//					maxHeightLimit = GlobalVars.constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value + 1.5 * GlobalVars.constants.Pansops[ePANSOPSData.dpStr_Gui_dist].Value * GlobalVars.constants.Pansops[ePANSOPSData.dpMaxPosPDG].Value;

		//				break;

		//			case TerminationType.AtWPT:
		//				rbDistance = radioButton205;
		//				rbAppliedPDG = radioButton206;
		//				cbHeigtAltitude = comboBox202;
		//				tbHeigtAltitude = textBox206;

		//				maxHeightLimit = 3000.0;		//Height At FIX	!
		//				break;
		//		}

		//		if (minHeightLimit < GlobalVars.constants.Pansops[ePANSOPSData.dpGui_Ar1].Value)
		//			minHeightLimit = GlobalVars.constants.Pansops[ePANSOPSData.dpGui_Ar1].Value;

		//		if (value > maxHeightLimit)
		//			value = maxHeightLimit;

		//		if (_terminationType == TerminationType.AtHeight && checkBox201.Checked)
		//		{
		//			double maxDist = GlobalVars.constants.Pansops[ePANSOPSData.dpObsClr].Value / GlobalVars.constants.Pansops[ePANSOPSData.dpMOC].Value;
		//			if (segment1Term.DetObs.X < maxDist)
		//				minHeightLimit = Math.Max(minHeightLimit, segment1Term.DetObs.Height + GlobalVars.constants.Pansops[ePANSOPSData.dpObsClr].Value);
		//		}

		//		if (value < minHeightLimit)
		//			value = minHeightLimit;

		//		if (_terminationHeight != value)
		//		{
		//			_terminationHeight = value;

		//			if (_terminationType == TerminationType.AtHeight)
		//			{
		//				for (int i = 0; i < heights.Length; i++)
		//					if (_terminationHeight < heights[i])
		//					{
		//						comboBox206.SelectedIndex = i;
		//						break;
		//					}
		//			}

		//			if (cbHeigtAltitude.SelectedIndex == 0)
		//				value += ptDerPrj.Z;

		//			tbHeigtAltitude.Text = GlobalVars.unitConverter.HeightToDisplayUnits(value, eRoundMode.NERAEST).ToString();
		//			tbHeigtAltitude.Tag = tbHeigtAltitude.Text;
		//		}

		//		segment1Term.AppliedAltitude = _terminationHeight + ptDerPrj.Z;
		//		// = (height - GlobalVars.constants.Pansops[ePANSOPSData.dpH_abv_DER].Value) / straight.PDG
		//		if (rbDistance.Checked)
		//			terminationDistance = (_terminationHeight - GlobalVars.constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value) / _appliedPDG + _EPTMin;
		//		else if (rbAppliedPDG.Checked)
		//		{
		//			minDistPDG = (_terminationHeight - GlobalVars.constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value) / segment1Term.TermDistance;
		//			minDistPDG = Math.Round(minDistPDG + 0.00004999, 4);

		//			//if (appliedPDG < minDistPDG)
		//			appliedPDG = minDistPDG;
		//		}

		//		textBox209.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(segment1Term.Leg.EndFIX.ConstructTAS, eRoundMode.NERAEST).ToString();
		//		textBox210.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_terminationDistance - _EPTMin, eRoundMode.NERAEST).ToString();//leg.EndFIX.EPT
		//		textBox213.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(segment1Term.Leg.EndFIX.ConstructTurnRadius, eRoundMode.NERAEST).ToString();
		//		textBox212.Text = GlobalVars.unitConverter.HeightToDisplayUnits(segment1Term.ConstructAltitude, eRoundMode.NERAEST).ToString();
		//		textBox212.Tag = textBox212.Text;

		//		comboBox201_SelectedIndexChanged(comboBox201, null);
		//	}
		//}

		double _terminationDistance;
		private double terminationDistance
		{
			get { return _terminationDistance; }    //segment1Term.TermDistance
			set
			{
				RadioButton rbHeigtAltitude, rbAppliedPDG;
				TextBox tbDistance;

				LegDep leg = segment1Term.Leg;
				leg.EndFIX.OutDirection = leg.EndFIX.EntryDirection + segment1Term.PlannedMaxTurnAngle;

				double EPT = 0.0;

				switch (_terminationType)
				{
					default:
					case TerminationType.AtHeight:
						rbHeigtAltitude = radioButton201;
						rbAppliedPDG = radioButton203;
						tbDistance = textBox202;

						if (value > 1.5 * GlobalVars.constants.Pansops[ePANSOPSData.dpStr_Gui_dist].Value)
							value = 1.5 * GlobalVars.constants.Pansops[ePANSOPSData.dpStr_Gui_dist].Value;

						break;

					case TerminationType.AtWPT:
						rbHeigtAltitude = radioButton204;
						rbAppliedPDG = radioButton206;
						tbDistance = textBox207;

						if (value > GlobalVars.RModel)
							value = GlobalVars.RModel;

						if (rbHeigtAltitude.Checked)
							EPT = Functions.MinFlybyDistByHeightAtFIX(GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value, ptDerPrj.Z, _appliedPDG, segment1Term.PlannedMaxTurnAngle, leg.EndFIX, GlobalVars.constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value + _appliedPDG * value);
						else
							EPT = Functions.MinFlybyDistByHeightAtFIX(GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value, ptDerPrj.Z, _appliedPDG, segment1Term.PlannedMaxTurnAngle, leg.EndFIX, _heightAtEndFIX);

						break;
				}

				//if (value < MinTermDist + EPT)		value = MinTermDist + EPT;

				double minDist;
				//minDist = ARANMath.MinFlybyDist(GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value, leg.StartFIX.Altitude, _appliedPDG, segment1Term.PlannedMaxTurnAngle, leg.EndFIX, _heightAtEndFIX);

				if (rbHeigtAltitude.Checked)
					if (appliedPDG < GlobalVars.constants.Pansops[ePANSOPSData.dpPDG_Nom].Value)
						minDist = (_heightAtEndFIX - GlobalVars.constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value) / GlobalVars.constants.Pansops[ePANSOPSData.dpPDG_Nom].Value;
					else
						minDist = (GlobalVars.constants.Pansops[ePANSOPSData.dpGui_Ar1].Value - GlobalVars.constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value) / appliedPDG +
							Functions.MinFlybyDistByHeightAtKKLine(GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value, ptDerPrj.Z, _appliedPDG, segment1Term.PlannedMaxTurnAngle, leg.EndFIX, GlobalVars.constants.Pansops[ePANSOPSData.dpGui_Ar1].Value);
				else if (rbAppliedPDG.Checked)
				{
					minDist = (_heightAtEndFIX - GlobalVars.constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value) / GlobalVars.constants.Pansops[ePANSOPSData.dpMaxPosPDG].Value;
					double maxDist = (_heightAtEndFIX - GlobalVars.constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value) / segment1Term.MinPDG;

					if (value > maxDist)
						value = maxDist;
				}
				else            //apliedDistance
					minDist = (_heightAtEndFIX - GlobalVars.constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value) / GlobalVars.constants.Pansops[ePANSOPSData.dpMaxPosPDG].Value;

				if (value < minDist)
					value = minDist;

				if (_terminationDistance != value)
				{
					_terminationDistance = value;
					tbDistance.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(value, eRoundMode.NEAREST).ToString();
					tbDistance.Tag = tbDistance.Text;
				}

				segment1Term.TermDistance = _terminationDistance;
				if (_terminationType != TerminationType.AtHeight)
				{
					segment1Term.ConstructAltitude = ptDerPrj.Z + GlobalVars.constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value + _terminationDistance * segment1Term.ConstructionPDG;
					segment1Term.NomLineAltitude = ptDerPrj.Z + GlobalVars.constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value + _terminationDistance * segment1Term.NomLinePDG;

					for (int i = 0; i < heights.Length; i++)
						if (segment1Term.NomLineAltitude - ptDerPrj.Z < heights[i])
						{
							comboBox206.SelectedIndex = i;
							break;
						}
				}

				//_terminationDistance = segment1Term.TermDistance;	//????
				//double tdt = segment1Term.ConstructionPDG;

				leg.EndFIX.CalcTurnRangePoints();

				if (rbHeigtAltitude.Checked)
				{
					double newHeight = GlobalVars.constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value + _terminationDistance * _appliedPDG;
					HeightAtEndFIX = newHeight;

					if (Math.Abs(HeightAtEndFIX - newHeight) > ARANMath.EpsilonDistance)
					{
						_terminationDistance = (_heightAtEndFIX - GlobalVars.constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value) / _appliedPDG;
						segment1Term.TermDistance = _terminationDistance;

						tbDistance.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_terminationDistance, eRoundMode.NEAREST).ToString();
						tbDistance.Tag = tbDistance.Text;
					}
				}
				else if (rbAppliedPDG.Checked)
				{
					minDistPDG = (_heightAtEndFIX - GlobalVars.constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value) / _terminationDistance;

					double fTmp = minDistPDG;
					//if (fTmp < minDistPDG)
					//fTmp = minDistPDG;

					if (fTmp < segment1Term.MinPDG)
						fTmp = segment1Term.MinPDG;

					appliedPDG = fTmp;
				}

				if (segment1Term.UpdateEnabled)
				{
					ReportsFrm.FillPage2(segment1Term.InnerObstacleList, segment1Term.DetObs);
					Report = true;

					if (ReportBtn.Checked && !ReportsFrm.Visible)
						ReportsFrm.Show(GlobalVars.Win32Window);
				}

				if (_terminationType == TerminationType.AtWPT)
					EPT = Functions.MinFlybyDistByHeightAtFIX(GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value, ptDerPrj.Z, _appliedPDG, segment1Term.PlannedMaxTurnAngle, leg.EndFIX, _heightAtEndFIX);

				textBox209.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(segment1Term.Leg.EndFIX.ConstructTAS, eRoundMode.NEAREST).ToString();
				textBox210.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_terminationDistance - EPT, eRoundMode.NEAREST).ToString();   //leg.EndFIX.EPT
				textBox212.Text = GlobalVars.unitConverter.HeightToDisplayUnits(segment1Term.ConstructAltitude, eRoundMode.NEAREST).ToString();
				textBox212.Tag = textBox212.Text;

				double drr = leg.EndFIX.ConstructTurnRadius * Math.Tan(0.5 * segment1Term.PlannedMaxTurnAngle);
				textBox215.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(drr, eRoundMode.NEAREST).ToString();  //leg.EndFIX.EPT

				comboBox201_SelectedIndexChanged(comboBox201, null);
			}
		}

		double _RecalceAppliedPDG;
		double _appliedPDG;
		private double appliedPDG
		{
			get { return _appliedPDG; }

			set
			{
				RadioButton rbHeigtAltitude, rbDistance;
				TextBox tbAppliedPDG;

				switch (_terminationType)
				{
					default:
					case TerminationType.AtHeight:
						rbHeigtAltitude = radioButton201;
						rbDistance = radioButton202;
						tbAppliedPDG = textBox203;
						break;

					case TerminationType.AtWPT:
						rbHeigtAltitude = radioButton204;
						rbDistance = radioButton205;
						tbAppliedPDG = textBox208;
						break;
				}

				double minPDG = Math.Max(segment1Term.MinPDG, GlobalVars.constants.Pansops[ePANSOPSData.dpPDG_Nom].Value);
				double maxPDG = GlobalVars.constants.Pansops[ePANSOPSData.dpMaxPosPDG].Value;

				//value = Math.Round(value + 0.00004999, 4);

				if (value < minPDG)
					value = minPDG;

				if (value > maxPDG)
					value = maxPDG;

				LegDep leg = segment1Term.Leg;
				leg.EndFIX.OutDirection = leg.EndFIX.EntryDirection + segment1Term.PlannedMaxTurnAngle;

				double newHeight, EPT;

				if (rbHeigtAltitude.Checked)
				{
					newHeight = GlobalVars.constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value + _terminationDistance * value;
					//EPT = Functions.MinFlybyDistByHeightAtFIX(GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value, ptDerPrj.Z, value, segment1Term.PlannedMaxTurnAngle, leg.EndFIX, newHeight);

					EPT = Functions.MinFlybyDistByHeightAtKKLine(GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value, ptDerPrj.Z, maxPDG, segment1Term.PlannedMaxTurnAngle, leg.EndFIX);
					minPDG = (GlobalVars.constants.Pansops[ePANSOPSData.dpGui_Ar1].Value - GlobalVars.constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value) / (_terminationDistance - EPT);      //segment1Term.TermDistance;

					maxPDG = 3000.0 / _terminationDistance;
					//maxPDG = Math.Round(maxPDG - 0.00004999, 4);
					if (value > maxPDG)
						value = maxPDG;
				}
				else
				{
					EPT = Functions.MinFlybyDistByHeightAtFIX(GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value, ptDerPrj.Z, value, segment1Term.PlannedMaxTurnAngle, leg.EndFIX, _heightAtEndFIX);

					if (rbDistance.Checked)
					{
						if (_terminationType == TerminationType.AtHeight)
							minPDG = (_heightAtEndFIX - GlobalVars.constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value) / (1.5 * GlobalVars.constants.Pansops[ePANSOPSData.dpStr_Gui_dist].Value);
						else
							minPDG = (_heightAtEndFIX - GlobalVars.constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value) / GlobalVars.RModel;

						int io = 100;

						maxPDG = (_heightAtEndFIX - GlobalVars.constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value) / EPT;
						double MaxEPT = Functions.MinFlybyDistByHeightAtFIX(GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value, ptDerPrj.Z, maxPDG, segment1Term.PlannedMaxTurnAngle, leg.EndFIX, _heightAtEndFIX);

						double minKKDist = (GlobalVars.constants.Pansops[ePANSOPSData.dpGui_Ar1].Value - GlobalVars.constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value) / maxPDG;
						double minDist = minKKDist + MaxEPT;
						double prevMax;

						while (io > 0)
						{
							prevMax = maxPDG;
							maxPDG = (_heightAtEndFIX - GlobalVars.constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value) / minDist;

							if (Math.Abs(maxPDG - prevMax) < ARANMath.Epsilon_2Distance)
								break;

							MaxEPT = Functions.MinFlybyDistByHeightAtFIX(GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value, ptDerPrj.Z, maxPDG, segment1Term.PlannedMaxTurnAngle, leg.EndFIX, _heightAtEndFIX);

							minKKDist = (GlobalVars.constants.Pansops[ePANSOPSData.dpGui_Ar1].Value - GlobalVars.constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value) / maxPDG;
							minDist = minKKDist + MaxEPT;

							newHeight = minDist * maxPDG + GlobalVars.constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value;

							//maxPDG = Math.Round(maxPDG - 0.00004999, 4);
							io--;
						}

						//maxPDG = Math.Round(maxPDG - 0.00004999, 4);
						if (value > maxPDG)
							value = maxPDG;
					}
				}
				//else
				//	minPDG = (_heightAtEndFIX - GlobalVars.constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value) / (1.5 * GlobalVars.constants.Pansops[ePANSOPSData.dpStr_Gui_dist].Value);

				//minPDG = Math.Round(minPDG + 0.00004999, 4);
				if (value < minPDG)
					value = minPDG;

				//value = Math.Round(value + 0.00004999, 4);

				if (_appliedPDG != value)
				{
					_appliedPDG = value;
					tbAppliedPDG.Text = (Math.Round(100.0 * _appliedPDG, 2)).ToString();
					tbAppliedPDG.Tag = tbAppliedPDG.Text;

					fillComboBox205();
				}

				segment1Term.AppliedPDG = _appliedPDG;

				if (rbHeigtAltitude.Checked)
				{
					newHeight = GlobalVars.constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value + _terminationDistance * _appliedPDG;    //GlobalVars.constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value +
					HeightAtEndFIX = newHeight;

					if (Math.Abs(_heightAtEndFIX - newHeight) > ARANMath.EpsilonDistance)
						appliedPDG = (_heightAtEndFIX - GlobalVars.constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value) / _terminationDistance;
				}
				else if (rbDistance.Checked)
				{
					double newDistance = (_heightAtEndFIX - GlobalVars.constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value) / _appliedPDG;
					terminationDistance = newDistance;

					if (Math.Abs(_terminationDistance - newDistance) > ARANMath.EpsilonDistance)
						appliedPDG = (_heightAtEndFIX - GlobalVars.constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value) / _terminationDistance;
				}

				//if (_terminationType == TerminationType.AtWPT)
				//EPT = Functions.MinFlybyDistByHeightAtFIX(GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value, ptDerPrj.Z, _appliedPDG, segment1Term.PlannedMaxTurnAngle, leg.EndFIX, _heightAtEndFIX);
				textBox210.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_terminationDistance - EPT, eRoundMode.NEAREST).ToString();   //leg.EndFIX.EPT
				double drr = leg.EndFIX.ConstructTurnRadius * Math.Tan(0.5 * segment1Term.PlannedMaxTurnAngle);
				textBox215.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(drr, eRoundMode.NEAREST).ToString();  //leg.EndFIX.EPT

				comboBox201_SelectedIndexChanged(comboBox201, null);
			}
		}
		#endregion

		#region Form

		public DepartureForm()
		{
			InitializeComponent();
			Label1 = new Label[] { Label01, Label02, Label03, Label04, Label05 };
			screenCapture = GlobalVars.gAranEnv.GetScreenCapture(Aim.FeatureType.StandardInstrumentDeparture.ToString());

			int i, n = GlobalVars.RWYList.Length;
			if (n <= 0)
			{
				MessageBox.Show(Resources.str15056, "PANDA", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				Close();
				return;
			}

			this.Report = false;
			this.ReportsFrm = new CReports();
			this.ReportsFrm.Init(this.ReportBtn, GlobalVars.ReportHelpIDRNAVDeparture);

			this.comboBox201.SelectedIndex = 0;
			this.comboBox202.SelectedIndex = 0;
			this.comboBox203.SelectedIndex = 0;
			this.comboBox206.SelectedIndex = 0;
			this.comboBox302.SelectedIndex = 0;
			this.comboBox308.SelectedIndex = 0;
			// ==============================================================
			MinTermDist = (GlobalVars.constants.Pansops[ePANSOPSData.dpGui_Ar1].Value - GlobalVars.constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value) /
				GlobalVars.constants.Pansops[ePANSOPSData.dpMaxPosPDG].Value;

			Label1[0].Text = Resources.str00300;
			Label1[1].Text = Resources.str00301;
			Label1[2].Text = Resources.str00302;
			Label1[3].Text = Resources.str00303;
			Label1[4].Text = Resources.str00304;

			MultiPage1.TabPages[0].Text = Resources.str00300;
			MultiPage1.TabPages[1].Text = Resources.str00301;
			MultiPage1.TabPages[2].Text = Resources.str00302;
			MultiPage1.TabPages[3].Text = Resources.str00303;
			MultiPage1.TabPages[4].Text = Resources.str00304;

			//ToolTip1.SetToolTip(TextBox501, Resources.str15268 + GlobalVars.constants.Pansops[ePANSOPSData.dpGui_Ar1].Value.ToString() + " ?"); // "??????????? ???????? ?? ?????????? PANS-OPS "" ?"
			//ToolTip1.SetToolTip(TextBox502, ToolTip1.GetToolTip(SpinButton501));

			GlobalVars.ButtonControl1State = true;
			GlobalVars.ButtonControl2State = true;
			GlobalVars.ButtonControl3State = true;
			GlobalVars.ButtonControl4State = true;
			GlobalVars.ButtonControl5State = true;
			GlobalVars.ButtonControl6State = true;
			GlobalVars.ButtonControl7State = true;
			GlobalVars.ButtonControl8State = true;

			GlobalVars.FIXElem = -1;
			GlobalVars.CLElem = -1;
			GlobalVars.pCircleElem = -1;
			GlobalVars.StraightAreaFullElem = -1;
			GlobalVars.StraightAreaPrimElem = -1;
			GlobalVars.p120LineElem = -1;
			GlobalVars.TurnAreaElem = -1;

			GlobalVars.PrimElem = -1;
			GlobalVars.SecRElem = -1;
			GlobalVars.SecLElem = -1;
			GlobalVars.StrTrackElem = -1;
			GlobalVars.NomTrackElem = -1;

			GlobalVars.KKElem = -1;
			GlobalVars.K1K1Elem = -1;
			GlobalVars.TerminationFIXElem = -1;

			// ============================================
			this.CurrPage = 0;

			GlobalVars.RModel = 0.0;
			textBox103.Text = (100.0 * GlobalVars.constants.Pansops[ePANSOPSData.dpPDG_Nom].Value).ToString();

			// =============================================================================================
			ComboBox101.Items.Clear();
			foreach (ePBNClass pbn in firstTurnPBNClass)
				ComboBox101.Items.Add(pbn);

			//ComboBox101.Items.Add(Resources.str30201);
			//ComboBox101.Items.Add(Resources.str30202);
			//ComboBox101.Items.Add(Resources.str30203);

			comboBox102.Items.Clear();
			comboBox102.Items.Add(Resources.str30204);
			comboBox102.Items.Add(Resources.str30205);
			//comboBox102.Items.Add(Resources.str30206);

			// ===============================================================
			PrevBtn.Text = Resources.str00002;
			NextBtn.Text = Resources.str00003;
			OkBtn.Text = Resources.str00004;
			CancelBtn.Text = Resources.str00005;
			ReportBtn.Text = Resources.str00006;

			// ==================Page1=======================================
			Label001.Text = Resources.str03014;
			Label002.Text = Resources.str03013;

			Label003.Text = Resources.str03003;
			Label004.Text = Resources.str03004;
			Label005.Text = Resources.str03005;
			Label006.Text = Resources.str03006;
			Label008.Text = Resources.str03008;
			Label018.Text = Resources.str03015;

			Label016.Text = Resources.str03010;

			Label017.Text = GlobalVars.unitConverter.HeightUnit;
			Label009.Text = GlobalVars.unitConverter.DistanceUnit;

			Label012.Text = GlobalVars.unitConverter.DistanceUnit;
			Label015.Text = GlobalVars.unitConverter.HeightUnit;

			//Label011.Text = GlobalVars.unitConverter.HeightUnit;
			//Label019.Text = GlobalVars.unitConverter.HeightUnit;
			comboBox004.Items.Add(GlobalVars.unitConverter.HeightUnitM);
			comboBox004.Items.Add(GlobalVars.unitConverter.HeightUnitFt);

			comboBox005.Items.Add(GlobalVars.unitConverter.HeightUnitM);
			comboBox005.Items.Add(GlobalVars.unitConverter.HeightUnitFt);

			comboBox004.SelectedIndex = 0;
			comboBox005.SelectedIndex = 0;

			dataGridView401.Columns[5].HeaderText += ", " + GlobalVars.unitConverter.SpeedUnit;
			//dataGridView401.Columns[9].HeaderText += ", " + GlobalVars.unitConverter.HeightUnit;
			dataGridView401.Columns[9].HeaderText += ", " + GlobalVars.unitConverter.HeightUnit;
			dataGridView401.Columns[10].HeaderText += ", " + GlobalVars.unitConverter.HeightUnit;
			dataGridView401.Columns[11].HeaderText += ", " + GlobalVars.unitConverter.DistanceUnit;
			dataGridView401.Columns[12].HeaderText += ", " + GlobalVars.unitConverter.DistanceUnit;

			//
			// ==================Page2======================================
			groupBox101.Text = Resources.str03101;
			Label102.Text = Resources.str03102;
			Label104.Text = Resources.str03104;
			Label108.Text = GlobalVars.unitConverter.HeightUnit;
			Label111.Text = Resources.str03107;
			Label113.Text = GlobalVars.unitConverter.SpeedUnit;
			Label114.Text = Resources.str03109;
			Label115.Text = GlobalVars.unitConverter.HeightUnit;
			//
			// ===================Page3======================================
			label202.Text = GlobalVars.unitConverter.HeightUnit;
			label204.Text = GlobalVars.unitConverter.DistanceUnit;

			label212.Text = GlobalVars.unitConverter.HeightUnit;
			label214.Text = GlobalVars.unitConverter.DistanceUnit;
			label220.Text = GlobalVars.unitConverter.DistanceUnit;

			label225.Text = GlobalVars.unitConverter.HeightUnit;
			label218.Text = GlobalVars.unitConverter.SpeedUnit;
			label227.Text = GlobalVars.unitConverter.DistanceUnit;
			label233.Text = GlobalVars.unitConverter.DistanceUnit;

			label203.Text = Resources.str03207;
			label207.Text = Resources.str03211;
			label213.Text = Resources.str03209;

			label205.Text = Resources.str03206;
			label215.Text = Resources.str03206;

			label201.Text = Resources.str03208;
			label211.Text = Resources.str03208;
			label219.Text = Resources.str03210;

			tabPage201_1.Text = Resources.str03202;
			tabPage201_2.Text = Resources.str03203;
			checkBox201.Text = Resources.str03204;
			checkBox202.Text = Resources.str03205;

			// ===================Page4=======================================
			label304.Text = GlobalVars.unitConverter.DistanceUnit;
			label312.Text = GlobalVars.unitConverter.SpeedUnit;
			label320.Text = GlobalVars.unitConverter.HeightUnit;
			label322.Text = GlobalVars.unitConverter.SpeedUnit;
			label326.Text = GlobalVars.unitConverter.HeightUnit;
			label328.Text = GlobalVars.unitConverter.SpeedUnit;

			label330.Text = GlobalVars.unitConverter.HeightUnit;
			//label331.Text = "Flight time";
			//
			// ====================Page5 ======================================
			//label401.Text = Resources.str03103;
			label401.Text = Resources.str03206;
			label403.Text = Resources.str03104;
			label405.Text = Resources.str01501;

			label404.Text = GlobalVars.unitConverter.HeightUnit;
			label406.Text = GlobalVars.unitConverter.HeightUnit;

			// ==================================================================
			// ==================== Page6  ======================================
			// ==================== Page7  ======================================
			// ==================== Page8  ======================================
			// ==================== Page9  ======================================
			// ==================== Page10 ======================================
			// ==================================================================
			ComboBox001.Items.Clear();
			for (i = 0; i < n; i++)
				ComboBox001.Items.Add(GlobalVars.RWYList[i].Name);

			n = EnrouteMOCValues.Length;

			ComboBox003.Items.Clear();
			comboBox309.Items.Clear();
			for (i = 0; i < n; i++)
			{
				ComboBox003.Items.Add(GlobalVars.unitConverter.HeightToDisplayUnits(EnrouteMOCValues[i], eRoundMode.SPECIAL_NEAREST).ToString());
				comboBox309.Items.Add(GlobalVars.unitConverter.HeightToDisplayUnits(EnrouteMOCValues[i], eRoundMode.SPECIAL_NEAREST).ToString());
			}

			comboBox309.SelectedIndex = 0;
			ComboBox001.SelectedIndex = 0;

			_BankAngle = GlobalVars.constants.Pansops[ePANSOPSData.dpT_Bank].Value;
			_appliedPDG = GlobalVars.constants.Pansops[ePANSOPSData.dpPDG_Nom].Value;
			//===============================================

			MultiPage1.Top = -21;
			this.Height = this.Height - 21;
			Frame01.Top = Frame01.Top - 21;

			ShowPanelBtn.Checked = false;
			this.Width = Frame02.Left + 10;

			this.bFormInitialised = true;
		}

		private void DepartureForm_Shown(object sender, EventArgs e)
		{
			//FormFontsResizeService formFontsResizeService = new FormFontsResizeService();
			//formFontsResizeService.ResizeControlFonts(this);
		}

		private void DepartureForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			screenCapture.Rollback();
			DBModule.CloseDB();

			GlobalVars.gAranGraphics.SafeDeleteGraphic(GlobalVars.pCircleElem);
			GlobalVars.pCircleElem = -1;

			GlobalVars.gAranGraphics.SafeDeleteGraphic(GlobalVars.StraightAreaFullElem);
			GlobalVars.StraightAreaFullElem = -1;

			GlobalVars.gAranGraphics.SafeDeleteGraphic(GlobalVars.StraightAreaPrimElem);
			GlobalVars.StraightAreaPrimElem = -1;

			GlobalVars.gAranGraphics.SafeDeleteGraphic(GlobalVars.p120LineElem);
			GlobalVars.p120LineElem = -1;

			GlobalVars.gAranGraphics.SafeDeleteGraphic(GlobalVars.TerminationFIXElem);
			GlobalVars.TerminationFIXElem = -1;

			GlobalVars.gAranGraphics.SafeDeleteGraphic(GlobalVars.TurnAreaElem);
			GlobalVars.TurnAreaElem = -1;

			GlobalVars.gAranGraphics.SafeDeleteGraphic(GlobalVars.PrimElem);
			GlobalVars.PrimElem = -1;

			GlobalVars.gAranGraphics.SafeDeleteGraphic(GlobalVars.SecRElem);
			GlobalVars.SecRElem = -1;

			GlobalVars.gAranGraphics.SafeDeleteGraphic(GlobalVars.SecLElem);
			GlobalVars.SecLElem = -1;

			GlobalVars.gAranGraphics.SafeDeleteGraphic(GlobalVars.NomTrackElem);
			GlobalVars.NomTrackElem = -1;

			GlobalVars.gAranGraphics.SafeDeleteGraphic(GlobalVars.StrTrackElem);
			GlobalVars.StrTrackElem = -1;

			GlobalVars.gAranGraphics.SafeDeleteGraphic(GlobalVars.KKElem);
			GlobalVars.KKElem = -1;

			GlobalVars.gAranGraphics.SafeDeleteGraphic(GlobalVars.K1K1Elem);
			GlobalVars.K1K1Elem = -1;

			GlobalVars.gAranGraphics.SafeDeleteGraphic(GlobalVars.CLElem);
			GlobalVars.CLElem = -1;

			GlobalVars.gAranGraphics.SafeDeleteGraphic(GlobalVars.FIXElem);
			GlobalVars.FIXElem = -1;

			if (ReportsFrm != null)
				ReportsFrm.Close();

			if (straight != null)
				straight.Clean();

			if (segment1Term != null)
				segment1Term.Clean();

			if (transitions != null)
				transitions.Clean();

			GlobalVars.gAranGraphics.Refresh();
			GlobalVars.CurrCmd = -1;

			//Functions.RefreshCommandBar(_mTool, 65535);
			//CloseLog();
		}

		private void DepartureForm_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.F1)
			{
				NativeMethods.HtmlHelp(0, GlobalVars.HelpFile, GlobalVars.HH_HELP_CONTEXT, HelpContextID);
				e.Handled = true;
			}
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			// Get a handle to a copy of this form's system (window) menu
			IntPtr hSysMenu = Functions.GetSystemMenu(this.Handle, false);
			// Add a separator
			Functions.AppendMenu(hSysMenu, GlobalVars.MF_SEPARATOR, 0, string.Empty);
			// Add the About menu item
			Functions.AppendMenu(hSysMenu, GlobalVars.MF_STRING, GlobalVars.SYSMENU_ABOUT_ID, "&About…");
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

		private void TextBoxes_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
				e.IsInputKey = true;
		}

		private void ShowPanelBtn_CheckedChanged(object sender, EventArgs e)
		{
			if (!bFormInitialised)
				return;

			if (ShowPanelBtn.Checked)
			{
				this.Width = Frame02.Left + Frame02.Width + 6;// FontResizeFactorProvider.Scale(3);
				ShowPanelBtn.Image = Resources.HIDE_INFO;
			}
			else
			{
				this.Width = Frame02.Left + 10;
				ShowPanelBtn.Image = Resources.SHOW_INFO;
			}

			if (NextBtn.Enabled)
				NextBtn.Focus();
			else
				PrevBtn.Focus();
		}

		private void PrevBtn_Click(object sender, EventArgs e)
		{
			screenCapture.Delete();
			NativeMethods.ShowPandaBox(this.Handle.ToInt32());

			switch (CurrPage)
			{
				case 1:
					straight.Clean();
					straight = null;

					break;

				case 2:
					segment1Term.Clean();
					segment1Term = null;
					textBox211.Text = "90";
					straight.ReDraw();
					break;

				case 3:
					transitions.Clean();
					transitions = null;
					segment1Term.ReDraw();

					while (ReportsFrm.ObstacleLists > 0)
						ReportsFrm.RemoveLastLegP4();

					break;
				case 4:
					//button303_Click(null, null);
					transitions.Remove(true);
					//transitions.ReDraw();
					ReportsFrm.RemoveLastLegP4();
					break;
			}

			this.CurrPage = CurrPage - 1;
			NativeMethods.HidePandaBox();
		}

		private void NextBtn_Click(object sender, EventArgs e)
		{
			screenCapture.Save(this);
			NativeMethods.ShowPandaBox(this.Handle.ToInt32());

			//this.Width = Frame02.Left;
			switch (CurrPage)
			{
				case 0:
					AddvanceToPageII();
					break;
				case 1:
					AddvanceToPageIII();
					break;
				case 2:
					AddvanceToPageIV();
					break;
				case 3:
					AddvanceToPageV();
					break;
			}

			this.CurrPage = CurrPage + 1;
			NativeMethods.HidePandaBox();
		}

		private void ReportBtn_CheckedChanged(object sender, EventArgs e)
		{
			if (!Report)
				return;

			if (ReportBtn.Checked)
				ReportsFrm.Show(GlobalVars.Win32Window);
			else
				ReportsFrm.Hide();
		}

		private void OkBtn_Click(object sender, EventArgs e)
		{
			string RepFileName, RepFileTitle;
			screenCapture.Save(this);

			if (!Functions.ShowSaveDialog(out RepFileName, out RepFileTitle))
				return;

			//ProcName Must be FIX Name sProcName = TurnWPT.Name + " RWY" + DER.Identifier;
			string sProcName = RepFileTitle;        //textBox306.Text + " 1A";

			ReportHeader pReport;
			pReport.Procedure = sProcName;

			pReport.Database = GlobalVars.gAranEnv.ConnectionInfo.Database;
			pReport.Aerodrome = GlobalVars.CurrADHP.Name;
			pReport.Category = ComboBox103.Text;

			////pReport.RWY = ComboBox001.Text;
			////pReport.Procedure = _Procedure.Name;
			////pReport.EffectiveDate = _Procedure.TimeSlice.ValidTime.BeginPosition;

			SaveAccuracy(RepFileName, RepFileTitle, pReport);
			SaveLog(RepFileName, RepFileTitle, pReport);
			SaveProtocol(RepFileName, RepFileTitle, pReport);

			ReportPoint[] GuidPoints;
			double totalLen = ConvertTracToPoints(out GuidPoints);

			GuidGeomRep = ReportFile.SaveGeometry(RepFileName, RepFileTitle, pReport, SelectedRWY, GuidPoints, totalLen);

			if (!SaveProcedure())
				return;

			saveReportToDB();
			SaveScreenshotToDB();
			this.Close();
		}

		private void saveReportToDB()
		{
			saveReportToDB(GuidLogRep, FeatureReportType.Log);
			saveReportToDB(GuidProtRep, FeatureReportType.Protocol);
			saveReportToDB(GuidGeomRep, FeatureReportType.Geometry);
		}

		private void saveReportToDB(ReportFile rp, FeatureReportType type)
		{
			if (rp.IsFinished)
			{
				FeatureReport report = new FeatureReport();
				report.Identifier = Procedure.Identifier;
				report.ReportType = type;
				report.HtmlZipped = rp.Report;
				DBModule.pObjectDir.SetFeatureReport(report);
			}
		}

		private void SaveScreenshotToDB()
		{
			Screenshot screenshot = new Screenshot();
			screenshot.DateTime = DateTime.Now;
			screenshot.Identifier = Procedure.Identifier;
			screenshot.Images = screenCapture.Commit(Procedure.Identifier);
			DBModule.pObjectDir.SetScreenshot(screenshot);
		}

		private double ConvertTracToPoints(out ReportPoint[] GuidPoints)
		{
			int n = transitions.Legs.Count;
			if (n == 0)
			{
				GuidPoints = new ReportPoint[0];
				return 0.0;
			}

			GuidPoints = new ReportPoint[n + 1];
			double result;
			LegDep leg = transitions.Legs[0];

			GuidPoints[0].Description = Resources.str00512;
			GuidPoints[0].Lat = SelectedRWY.pPtGeo[eRWY.ptEnd].Y;
			GuidPoints[0].Lon = SelectedRWY.pPtGeo[eRWY.ptEnd].X;

			GuidPoints[0].TrueCourse = SelectedRWY.TrueBearing;// .pPtGeo[eRWY.ptEnd].M;
			GuidPoints[0].Height = GlobalVars.constants.Pansops[ePANSOPSData.dpH_abv_DER].Value + SelectedRWY.pPtGeo[eRWY.ptEnd].Z;
			GuidPoints[0].Radius = -1;

			GuidPoints[0].DistToNext = leg.Length;

			result = leg.Length;

			for (int i = 1; i <= n; i++)
			{
				GuidPoints[i].Description = leg.EndFIX.Name;
				GuidPoints[i].Lat = leg.EndFIX.GeoPt.Y;
				GuidPoints[i].Lon = leg.EndFIX.GeoPt.X;

				GuidPoints[i].TrueCourse = ARANFunctions.DirToAzimuth(leg.EndFIX.PrjPt, leg.EndFIX.EntryDirection, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
				GuidPoints[i].Height = leg.Altitude; //-1.0;	// leg.UpperLimit;
				GuidPoints[i].Radius = -1.0;

				if (i < n)
				{
					leg = transitions.Legs[i];
					GuidPoints[i].DistToNext = leg.Length;
					result += leg.NominalTrack.Length;
				}
				else
					GuidPoints[i].DistToNext = -1;
			}

			return result;
		}

		private void SaveProtocol(string RepFileName, string RepFileTitle, ReportHeader pReport)
		{
			GuidProtRep = new ReportFile();

			GuidProtRep.DerPtPrj = SelectedRWY.pPtPrj[eRWY.ptEnd];
			GuidProtRep.ThrPtPrj = SelectedRWY.pPtPrj[eRWY.ptEnd];

			GuidProtRep.OpenFile(RepFileName + "_Protocol", Resources.str40105);

			GuidProtRep.WriteString(Resources.str15479 + " - " + Resources.str40105, true);
			GuidProtRep.WriteString("");
			//GuidProtRep.WriteString(RepFileTitle, true);

			GuidProtRep.WriteHeader(pReport);
			GuidProtRep.WriteString("");
			GuidProtRep.WriteString("");

			//GuidProtRep.WriteTab(ReportsFrm.dataGridView1, ReportsFrm.GetTabPageText(0));
			//GuidProtRep.WriteTab(ReportsFrm.dataGridView2, ReportsFrm.GetTabPageText(1));
			LegDep leg;
			string fixName;
			int n = transitions.Legs.Count;

			leg = transitions.Legs[0];
			ReportsFrm.SavePage4Obstacles(0, "leg " + ReportsFrm.GetTabPageText(1), GuidProtRep);

			for (int i = 1; i < n; i++)
			{
				leg = transitions.Legs[i];

				fixName = leg.StartFIX.Name;
				if (leg.StartFIX.FlyMode == eFlyMode.Atheight)
					fixName = "Altitude";
				ReportsFrm.SavePage4Obstacles(i, "leg " + fixName + "/" + leg.EndFIX.Name, GuidProtRep);
			}

			GuidProtRep.WriteString("");
			GuidProtRep.WriteString("");

			leg = transitions.Legs[0];
			ReportsFrm.SavePage5Obstacles(0, "leg " + ReportsFrm.GetTabPageText(1) + "-clearances", GuidProtRep);

			for (int i = 1; i < n; i++)
			{
				leg = transitions.Legs[i];

				fixName = leg.StartFIX.Name;
				if (leg.StartFIX.FlyMode == eFlyMode.Atheight)
					fixName = "Altitude";
				ReportsFrm.SavePage5Obstacles(i, "leg " + fixName + "/" + leg.EndFIX.Name + "-clearances", GuidProtRep);
			}

			GuidProtRep.CloseFile();
		}

		private void SaveAccuracy(string RepFileName, string RepFileTitle, ReportHeader pReport)
		{
			AccurRep = new ReportFile();
			AccurRep.OpenFile(RepFileName + "_Accuracy", RepFileTitle + ": " + Resources.str00516);
			//AccurRep.H1(My.Resources.str15479 + " - " + RepFileTitle + ": " + My.Resources.str00516);
			AccurRep.WriteString(Resources.str15479 + " - " + RepFileTitle + ": " + Resources.str00516, true);

			//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

			AccurRep.WriteHeader(pReport);
			AccurRep.Param("Distance accuracy", GlobalVars.settings.DistancePrecision.ToString(), GlobalVars.unitConverter.DistanceUnit);
			AccurRep.Param("Angle accuracy", GlobalVars.settings.AnglePrecision.ToString(), "degrees");
			AccurRep.WriteMessage();

			//double horAccuracy
			SelectedRWY.DERHorAccuracy = Functions.CalcDERHorisontalAccuracy(SelectedRWY);
			segment1Term.Leg.StartFIX.HorAccuracy = SelectedRWY.DERHorAccuracy;
			Functions.SaveDerAccurasyInfo(AccurRep, SelectedRWY);
			//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
			AccurRep.WriteMessage("=================================================");
			AccurRep.WriteMessage();


			if (terminationType != TerminationType.AtHeight)
			{
				Functions.SaveFixAccurasyInfo(AccurRep, segment1Term.Leg.StartFIX, segment1Term.Leg.EndFIX, "TP", comboBox205.SelectedIndex == 0);
				if (comboBox205.SelectedIndex == 0)
					segment1Term.Leg.EndFIX.HorAccuracy = Functions.CalcHorisontalAccuracy(segment1Term.Leg.StartFIX, segment1Term.Leg.EndFIX);
			}
			//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
			//WayPoint prev = segment1Term.Leg.EndFIX;

			for (int i = 1; i < transitions.Legs.Count; i++)
			{
				var leg = transitions.Legs[i];

				if (leg.EndFIX.Id == Guid.Empty) //|| leg.EndFIX.HorAccuracy == 0.0)
					Functions.CalcHorisontalAccuracy(leg.StartFIX, leg.EndFIX);

				Functions.SaveFixAccurasyInfo(AccurRep, leg.StartFIX, leg.EndFIX, "TP", leg.EndFIX.Id == Guid.Empty);
			}

			AccurRep.CloseFile();
		}

		private void SaveLog(string RepFileName, string RepFileTitle, ReportHeader pReport)
		{
			GuidLogRep = new ReportFile();
			GuidLogRep.DerPtPrj = SelectedRWY.pPtPrj[eRWY.ptEnd];
			GuidLogRep.ThrPtPrj = SelectedRWY.pPtPrj[eRWY.ptEnd];

			GuidLogRep.OpenFile(RepFileName + "_Log", Resources.str00520);

			GuidLogRep.WriteString(Resources.str15479 + " - " + Resources.str00520, true);
			GuidLogRep.WriteString("");
			//GuidLogRep.WriteString(RepFileTitle, true);

			GuidLogRep.WriteHeader(pReport);

			//     GuidLogRep.WriteParam LoadResString(518), CStr(Date) + " - " + CStr(Time)
			//     If AIRLayerInfo.Initialised Then GuidLogRep.WriteLayerInfo AIRLayerInfo
			//     If RWYLayerInfo.Initialised Then GuidLogRep.WriteLayerInfo RWYLayerInfo
			//     If NAVLayerInfo.Initialised Then GuidLogRep.WriteLayerInfo NAVLayerInfo
			//     If ObsLayerInfo.Initialised Then GuidLogRep.WriteLayerInfo ObsLayerInfo
			//     If FIXLayerInfo.Initialised Then GuidLogRep.WriteLayerInfo FIXLayerInfo
			//     If WARNINGLayerInfo.Initialised Then GuidLogRep.WriteLayerInfo WARNINGLayerInfo

			GuidLogRep.WriteString("");
			GuidLogRep.WriteString("");

			GuidLogRep.ExH2(MultiPage1.TabPages[0].Text);
			GuidLogRep.HTMLString("[ " + MultiPage1.TabPages[0].Text + " ]", true, false);
			GuidLogRep.WriteString("");

			GuidLogRep.Param(Label001.Text, ComboBox001.Text, "");
			GuidLogRep.Param(Label005.Text, ComboBox002.Text, "");

			GuidLogRep.Param(Label006.Text, TextBox004.Text, "");
			GuidLogRep.Param(Label002.Text, TextBox001.Text, "");

			GuidLogRep.Param(Label014.Text, TextBox008.Text, Label015.Text);
			//=======================================================================
			GuidLogRep.Param(Label003.Text, TextBox002.Text, comboBox004.Text);
			GuidLogRep.Param(Label018.Text, TextBox009.Text, comboBox005.Text);

			GuidLogRep.WriteString("");

			GuidLogRep.Param(Label016.Text, ComboBox003.Text, Label017.Text);
			GuidLogRep.Param(Label004.Text, TextBox003.Text, Label009.Text);
			GuidLogRep.Param(Label007.Text, TextBox005.Text, "");
			GuidLogRep.Param(Label008.Text, TextBox006.Text, Label012.Text);
			//=======================================================================

			GuidLogRep.WriteString("");
			GuidLogRep.WriteString("");

			GuidLogRep.ExH2(MultiPage1.TabPages[1].Text);
			GuidLogRep.HTMLString("[ " + MultiPage1.TabPages[1].Text + " ]", true, false);
			GuidLogRep.WriteString("");

			GuidLogRep.Param(Label109.Text, ComboBox101.Text, "");
			GuidLogRep.Param(Label110.Text, comboBox102.Text, "");
			GuidLogRep.Param(Label111.Text, ComboBox103.Text, "");
			GuidLogRep.WriteString("");

			GuidLogRep.WriteString(groupBox101.Text);
			if (radioButton101.Checked)
				GuidLogRep.Param(radioButton101.Text, numericUpDown101.Text, Label105.Text);
			else
				GuidLogRep.Param(radioButton102.Text, comboBox104.Text, label101.Text);
			GuidLogRep.WriteString("");

			GuidLogRep.Param(Label104.Text, textBox104.Text, Label108.Text);

			GuidLogRep.WriteString("");
			GuidLogRep.WriteString("");


			GuidLogRep.ExH2(MultiPage1.TabPages[2].Text);
			GuidLogRep.HTMLString("[ " + MultiPage1.TabPages[2].Text + " ]", true, false);
			GuidLogRep.WriteString("");

			if (tabControl201.SelectedIndex == 0)
			{
				GuidLogRep.WriteString(tabControl201.TabPages[0].Text, true);
				GuidLogRep.WriteString("");
				//tabPage201_2

				GuidLogRep.Param(label221.Text, comboBox203.Text, "");
				GuidLogRep.Param(comboBox202.Text, textBox206.Text, label212.Text);
				GuidLogRep.Param(label224.Text, textBox212.Text, label225.Text);
				GuidLogRep.Param(label228.Text, comboBox205.Text, "");
				GuidLogRep.Param(label213.Text, textBox207.Text, label214.Text);
				GuidLogRep.Param(label215.Text, textBox208.Text, label216.Text);
				GuidLogRep.Param(label217.Text, textBox209.Text, label218.Text);
				GuidLogRep.Param(label219.Text, textBox210.Text, label220.Text);
				GuidLogRep.Param(label226.Text, textBox213.Text, label227.Text);
			}
			else
			{
				GuidLogRep.WriteString(tabControl201.TabPages[1].Text, true);
				GuidLogRep.WriteString("");
				//tabPage201_1
				GuidLogRep.Param(comboBox201.Text, textBox201.Text, label202.Text);
				GuidLogRep.Param(label203.Text, textBox202.Text, label204.Text);

				if (checkBox202.Checked)
					GuidLogRep.WriteString(checkBox202.Text, true);

				GuidLogRep.Param(label205.Text, textBox203.Text, label206.Text);
				//GuidLogRep.Param(label207.Text, textBox204.Text, label208.Text);

				if (checkBox201.Checked)
					GuidLogRep.WriteString(checkBox201.Text, true);
			}
			GuidLogRep.WriteString("");

			GuidLogRep.Param(label229.Text, comboBox206.Text, "°");
			GuidLogRep.Param(label222.Text, textBox211.Text, "°");
			GuidLogRep.Param(label231.Text, textBox214.Text, "");
			GuidLogRep.WriteString("");
			GuidLogRep.WriteString("");

			GuidLogRep.ExH2(MultiPage1.TabPages[3].Text);
			GuidLogRep.HTMLString("[ " + MultiPage1.TabPages[3].Text + " ]", true, false);
			GuidLogRep.WriteString("");
			GuidLogRep.WriteString("- - -");
			GuidLogRep.WriteString("");
			GuidLogRep.WriteString("");

			GuidLogRep.ExH2(MultiPage1.TabPages[4].Text);
			GuidLogRep.HTMLString("[ " + MultiPage1.TabPages[4].Text + " ]", true, false);
			GuidLogRep.WriteString("");

			GuidLogRep.WriteTab(dataGridView401, RepFileTitle);
			GuidLogRep.WriteString("");
			GuidLogRep.Param(label405.Text, textBox403.Text, label406.Text);
			GuidLogRep.WriteString("");

			GuidLogRep.WriteString(groupBox401.Text, true);
			GuidLogRep.Param(label401.Text, textBox401.Text, label402.Text);
			GuidLogRep.Param(label403.Text, textBox402.Text, label404.Text);

			GuidLogRep.CloseFile();
		}

		private void CancelBtn_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		#endregion

		#region Page I

		private void TextBox003_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				TextBox003_Validating(TextBox003, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, TextBox003.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void TextBox003_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (double.TryParse(TextBox003.Text, out fTmp))
			{
				if (TextBox003.Tag != null && TextBox003.Tag.ToString() == TextBox003.Text)
					return;

				double NewR = GlobalVars.unitConverter.DistanceToInternalUnits(fTmp);

				if (NewR < RMin)
					NewR = RMin;

				if (NewR > 350000.0)
					NewR = 350000.0;

				GlobalVars.gAranGraphics.SafeDeleteGraphic(GlobalVars.pCircleElem);
				GlobalVars.gAranGraphics.SafeDeleteGraphic(GlobalVars.CLElem);

				TextBox003.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(NewR, eRoundMode.CEIL).ToString();
				TextBox003.Tag = TextBox003.Text;
				GlobalVars.RModel = NewR;

				Ring pCircleRing = ARANFunctions.CreateCirclePrj(ptCenter, GlobalVars.RModel + additionR);
				Polygon pCirclePolygon = new Polygon();

				pCircle = new MultiPolygon();
				pCirclePolygon.ExteriorRing = pCircleRing;
				pCircle.Add(pCirclePolygon);

				// =====================================================================
				LineSymbol pLineSym = new LineSymbol();
				pLineSym.Color = ARANFunctions.RGB(255, 0, 0);
				pLineSym.Width = 2;

				LineString ls = new LineString();
				for (int i = 0; i < pCircleRing.Count; i++)
					ls.Add(pCircleRing[i]);

				GlobalVars.pCircleElem = GlobalVars.gAranGraphics.DrawLineString(ls, pLineSym);

				// =====================================================================
				GeometryOperators geometryOperators = new GeometryOperators();

				MultiLineString pLine = new MultiLineString();
				LineString pLineStr = new LineString();

				pLineStr.Add(ptDerPrj);
				pLineStr.Add(ARANFunctions.PointAlongPlane(ptDerPrj, DepDir, GlobalVars.RModel));
				pLine.Add(pLineStr);

				pLine = (MultiLineString)geometryOperators.Intersect(pCircle, pLine);

				// =====================================================================

				pLineSym.Color = 0;
				pLineSym.Style = eLineStyle.slsDash;
				pLineSym.Width = 1;

				GlobalVars.CLElem = GlobalVars.gAranGraphics.DrawMultiLineString(pLine, pLineSym, GlobalVars.ButtonControl7State);

				// =====================================================================

				int ObsNum = Functions.GetObstaclesByDistance(out GlobalVars.ObstacleList, ptCenter, NewR, out double MaxDist);

				Label007.Text = Resources.str15261 + " " + GlobalVars.unitConverter.DistanceToDisplayUnits(GlobalVars.RModel, eRoundMode.NEAREST).ToString() + " " + GlobalVars.unitConverter.DistanceUnit + ":";
				TextBox006.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(MaxDist, eRoundMode.FLOOR).ToString();
				TextBox005.Text = ObsNum.ToString();
			}
			else if (double.TryParse((string)TextBox003.Tag, out fTmp))
				TextBox003.Text = (string)TextBox003.Tag;
			else
				TextBox003.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(GlobalVars.RModel, eRoundMode.CEIL).ToString();
		}

		private void ComboBox001_SelectedIndexChanged(object sender, EventArgs e)
		{
			int RWYIndex = ComboBox001.SelectedIndex;
			if (RWYIndex < 0)
				return;

			SelectedRWY = GlobalVars.RWYList[RWYIndex];
			ptDerPrj = SelectedRWY.pPtPrj[eRWY.ptDER];
			ptDerGeo = SelectedRWY.pPtGeo[eRWY.ptDER];

			DepDir = ptDerPrj.M;
			DepAzt = ptDerGeo.M;

			TextBox008.Text = GlobalVars.unitConverter.HeightToDisplayUnits(ptDerPrj.Z, eRoundMode.NEAREST).ToString();

			comboBox004_SelectedIndexChanged(comboBox004, null);
			comboBox005_SelectedIndexChanged(comboBox005, null);

			additionR = SelectedRWY.TODA - GlobalVars.constants.Pansops[ePANSOPSData.dpT_Init].Value;
			ptCenter = ARANFunctions.LocalToPrj(ptDerPrj, DepDir, -additionR, 0);

			if (ComboBox002.SelectedIndex < 0)
				ComboBox002.SelectedIndex = 0;
			else
				ComboBox002_SelectedIndexChanged(ComboBox002, new System.EventArgs());

			if (ComboBox003.SelectedIndex < 0)
				ComboBox003.SelectedIndex = 0;
			else
				ComboBox003_SelectedIndexChanged(ComboBox003, new System.EventArgs());

			TextBox003.Tag = "";
			TextBox003_Validating(TextBox003, new System.ComponentModel.CancelEventArgs(false));
		}

		private void ComboBox002_SelectedIndexChanged(object sender, EventArgs e)
		{
			int index = ComboBox002.SelectedIndex;

			if (index < 0)
				index = 0;

			TextBox001.Text = ARANMath.Degree2String(ptDerGeo.M, (eDegree2StringMode)index);
			double fTmp = NativeMethods.Modulus(ptDerGeo.M - GlobalVars.CurrADHP.MagVar, 360.0);
			TextBox004.Text = ARANMath.Degree2String(fTmp, (eDegree2StringMode)index);
		}

		private void ComboBox003_SelectedIndexChanged(object sender, EventArgs e)
		{
			double fTmp, OldR = -1.0;
			int k = ComboBox003.SelectedIndex;

			if (k < 0)
			{
				ComboBox003.SelectedIndex = 0;
				return;
			}

			RMin = 50.0 * System.Math.Round(0.02 * 600.0 / GlobalVars.constants.Pansops[ePANSOPSData.dpPDG_Nom].Value + 0.4999);

			MOCLimit = EnrouteMOCValues[k];

			double minR = 50.0 * System.Math.Round(0.02 * MOCLimit / GlobalVars.constants.Pansops[ePANSOPSData.dpMOC].Value + 0.4999);
			if (RMin < minR)
				RMin = minR;

			if (double.TryParse(TextBox003.Text, out fTmp))
				OldR = GlobalVars.unitConverter.DistanceToInternalUnits(fTmp);

			if (OldR < RMin)
			{
				TextBox003.Tag = "";
				TextBox003.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(RMin, eRoundMode.CEIL).ToString();
				TextBox003_Validating(TextBox003, new System.ComponentModel.CancelEventArgs());
			}
		}

		private void comboBox004_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBox004.SelectedIndex == 0)
				TextBox002.Text = GlobalVars.unitConverter.HeightToM(SelectedRWY.Length, eRoundMode.NEAREST).ToString();
			else
				TextBox002.Text = GlobalVars.unitConverter.HeightToFt(SelectedRWY.Length, eRoundMode.NEAREST).ToString();
		}

		private void comboBox005_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBox005.SelectedIndex == 0)
				TextBox009.Text = GlobalVars.unitConverter.HeightToM(SelectedRWY.TODA, eRoundMode.NEAREST).ToString();
			else
				TextBox009.Text = GlobalVars.unitConverter.HeightToFt(SelectedRWY.TODA, eRoundMode.NEAREST).ToString();
		}

		private void AddvanceToPageII()
		{
			DBModule.FillNavaidList(out GlobalVars.NavaidList, out GlobalVars.DMEList, GlobalVars.CurrADHP, GlobalVars.MaxNAVDist + GlobalVars.RModel);

			_accelerationAltitude = GlobalVars.unitConverter.HeightToInternalUnits(GlobalVars.unitConverter.HeightToDisplayUnits(GlobalVars.CurrADHP.Elev + 10000.0, eRoundMode.SPECIAL_NEAREST));
			textBox102.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_accelerationAltitude, eRoundMode.NEAREST).ToString();

			straight = new Straight(SelectedRWY, DepDir, MOCLimit, GlobalVars.RModel, false, GlobalVars.CurrADHP, GlobalVars.gAranEnv);

			straight.PDGChanged += StraightPDGChanged;

			double height = GlobalVars.constants.Pansops[ePANSOPSData.dpNGui_Ar1].Value + ptDerPrj.Z;
			textBox104.Text = GlobalVars.unitConverter.HeightToDisplayUnits(height, eRoundMode.NEAREST).ToString();

			straight.NomPDGDistChanged += StraightNomPDGDistChanged;

			double fTmp = ptDerGeo.M;
			SpinButton1Min = (decimal)ARANMath.Modulus(fTmp - 15.0, 360.0);
			SpinButton1Max = (decimal)ARANMath.Modulus(fTmp + 15.0, 360.0);
			numericUpDown101.Value = (decimal)fTmp;

			comboBox104.Items.Clear();

			foreach (WPT_FIXType SigPoint in GlobalVars.WPTList)
			{
				double fDist = ARANFunctions.PointToLineDistance(SigPoint.pPtPrj, SelectedRWY.pPtPrj[eRWY.ptDER], ptDerPrj.M - ARANMath.C_PI_2);

				if (fDist > 1.0 && fDist <= GlobalVars.RModel)
				{
					double dX = SigPoint.pPtPrj.X - SelectedRWY.pPtPrj[eRWY.ptDER].X;
					double dY = SigPoint.pPtPrj.Y - SelectedRWY.pPtPrj[eRWY.ptDER].Y;
					double fAngle = ARANMath.RadToDeg(ARANMath.Modulus(ptDerPrj.M - Math.Atan2(dY, dX), ARANMath.C_2xPI));

					if (fAngle >= 345 || fAngle <= 15)
						comboBox104.Items.Add(SigPoint);
				}
			}

			comboBox104.Enabled = comboBox104.Items.Count > 0;
			if (comboBox104.Items.Count > 0)
				comboBox104.SelectedIndex = 0;
			comboBox104.Enabled = radioButton102.Checked;

			if (ComboBox101.SelectedIndex == 0)
				ComboBox101_SelectedIndexChanged(ComboBox101, new EventArgs());
			else
				ComboBox101.SelectedIndex = 0;

			if (comboBox102.SelectedIndex == 0)
				comboBox102_SelectedIndexChanged(comboBox102, new EventArgs());
			else
				comboBox102.SelectedIndex = 0;

			if (ComboBox103.SelectedIndex == 0)
				ComboBox103_SelectedIndexChanged(ComboBox101, new EventArgs());
			else
				ComboBox103.SelectedIndex = 0;

			straight.UpdateEnabled = true;

			ReportsFrm.FillPage1(straight.InnerObstacleList, straight.DetObs);
			Report = true;

			if (ReportBtn.Checked && !ReportsFrm.Visible)
				ReportsFrm.Show(GlobalVars.Win32Window);
		}

		#endregion

		#region Page II
		private void StraightPDGChanged(object sender, double newPDG)
		{
			textBox103.Text = Math.Round(newPDG * 100.0, 1).ToString();
		}

		private void StraightNomPDGDistChanged(object sender, double newNomPDG)
		{
			textBox104.Text = GlobalVars.unitConverter.HeightToDisplayUnits(straight.NomPDGHeight + ptDerPrj.Z, eRoundMode.NEAREST).ToString();
		}

		private bool PreventRecurseNUD101 = false;

		private void numericUpDown101_ValueChanged(object sender, EventArgs e)
		{
			if (!PreventRecurseNUD101)
			{
				decimal dTmp = numericUpDown101.Value;
				decimal OldVal = dTmp;

				if (dTmp <= numericUpDown101.Minimum)
					dTmp = numericUpDown101.Maximum - 1;
				else if (dTmp >= numericUpDown101.Maximum)
					dTmp = numericUpDown101.Minimum + 1;

				if ((SpinButton1Min >= 330 && (dTmp < SpinButton1Min && dTmp > SpinButton1Max)) || ((SpinButton1Min < 330 && (dTmp < SpinButton1Min || dTmp > SpinButton1Max))))
				{
					decimal d1 = Math.Abs(dTmp - SpinButton1Min);
					decimal d2 = Math.Abs(dTmp - SpinButton1Max);
					if (d1 > d2)
						dTmp = SpinButton1Max;
					else
						dTmp = SpinButton1Min;
				}

				PreventRecurseNUD101 = OldVal != dTmp;
				numericUpDown101.Value = dTmp;
				if (PreventRecurseNUD101)
					return;
			}
			else
				PreventRecurseNUD101 = false;

			DepAzt = (double)numericUpDown101.Value;
			DepDir = ARANFunctions.AztToDirection(ptDerGeo, DepAzt, GlobalVars.pSpRefGeo, GlobalVars.pSpRefPrj);

			straight.InitialTrack = DepDir;

			if (straight.UpdateEnabled)
			{
				ReportsFrm.FillPage1(straight.InnerObstacleList, straight.DetObs);
				Report = true;
				if (ReportBtn.Checked && !ReportsFrm.Visible)
					ReportsFrm.Show(GlobalVars.Win32Window);
			}
		}

		private void numericUpDown101_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				if (numericUpDown101.Value == numericUpDown101.Minimum)
					numericUpDown101.Value = numericUpDown101.Maximum;
				else if (numericUpDown101.Value == numericUpDown101.Maximum)
					numericUpDown101.Value = numericUpDown101.Minimum;
			}
		}

		private void radioButton101_CheckedChanged(object sender, EventArgs e)
		{
			numericUpDown101.ReadOnly = !radioButton101.Checked;

			comboBox104.Enabled = !radioButton101.Checked;

			if (!radioButton101.Checked)
				comboBox104_SelectedIndexChanged(comboBox104, new EventArgs());
		}

		private void comboBox104_SelectedIndexChanged(object sender, EventArgs e)
		{
			label101.Text = string.Empty;
			int k = comboBox104.SelectedIndex;
			if (k < 0)
				return;

			WPT_FIXType sigPoint = (WPT_FIXType)comboBox104.SelectedItem;
			label101.Text = sigPoint.Name;
			if (radioButton101.Checked)
				return;

			double Direction = Math.Atan2(sigPoint.pPtPrj.Y - SelectedRWY.pPtPrj[eRWY.ptDER].Y, sigPoint.pPtPrj.X - SelectedRWY.pPtPrj[eRWY.ptDER].X);
			double newAzt = ARANFunctions.DirToAzimuth(SelectedRWY.pPtPrj[eRWY.ptDER], Direction, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

			numericUpDown101.Value = (decimal)newAzt;
		}

		private void ComboBox101_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (straight != null)
			{
				straight.PBNtype = (ePBNClass)ComboBox101.SelectedItem;

				if (straight.UpdateEnabled)
				{
					ReportsFrm.FillPage1(straight.InnerObstacleList, straight.DetObs);
					Report = true;
					if (ReportBtn.Checked && !ReportsFrm.Visible)
						ReportsFrm.Show(GlobalVars.Win32Window);
				}
			}
		}

		private void comboBox102_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (straight != null)
			{
				straight.sensor = (eSensorType)comboBox102.SelectedIndex;
				if (straight.UpdateEnabled)
				{
					ReportsFrm.FillPage1(straight.InnerObstacleList, straight.DetObs);
					Report = true;
					if (ReportBtn.Checked && !ReportsFrm.Visible)
						ReportsFrm.Show(GlobalVars.Win32Window);
				}
			}
		}

		private void ComboBox103_SelectedIndexChanged(object sender, EventArgs e)
		{
			_aircat = (aircraftCategory)ComboBox103.SelectedIndex;
			if (_aircat == aircraftCategory.acDL)
				_aircat = aircraftCategory.acE;

			speedLimits.Min = 1.1 * GlobalVars.constants.AircraftCategory[aircraftCategoryData.VmaInter].Value[_aircat];
			speedLimits.Max = 1.1 * GlobalVars.constants.AircraftCategory[aircraftCategoryData.VmaFaf].Value[_aircat];

			textBox101.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(speedLimits.Max, eRoundMode.NEAREST).ToString();
			textBox101.Tag = null;
			textBox101_Validating(textBox101, new CancelEventArgs());
		}

		private void textBox101_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox101_Validating(textBox101, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox101.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox101_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(textBox101.Text, out fTmp))
				return;

			if (textBox101.Tag != null && textBox101.Tag.ToString() == textBox101.Text)
				return;

			_IAS = GlobalVars.unitConverter.SpeedToInternalUnits(fTmp);
			fTmp = _IAS;

			if (_IAS < speedLimits.Min)
				_IAS = speedLimits.Min;

			if (_IAS > speedLimits.Max)
				_IAS = speedLimits.Max;

			if (fTmp != _IAS)
				textBox101.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(_IAS, eRoundMode.NEAREST).ToString();

			straight.IAS = _IAS;
			textBox101.Tag = textBox101.Text;
		}

		private void textBox102_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox102_Validating(textBox102, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox102.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox102_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(textBox102.Text, out fTmp))
				return;

			if (textBox102.Tag != null && textBox102.Tag.ToString() == textBox102.Text)
				return;

			fTmp = GlobalVars.unitConverter.HeightToInternalUnits(fTmp);
			if (fTmp > GlobalVars.MaxAltitude)
				fTmp = GlobalVars.MaxAltitude;

			double minimal = GlobalVars.unitConverter.HeightToInternalUnits(GlobalVars.unitConverter.HeightToDisplayUnits(GlobalVars.CurrADHP.Elev + 2000.0, eRoundMode.SPECIAL_NEAREST));

			_accelerationAltitude = fTmp;
			if (fTmp < minimal)
				_accelerationAltitude = minimal;

			textBox102.Text = GlobalVars.unitConverter.HeightToDisplayUnits(_accelerationAltitude, eRoundMode.SPECIAL_NEAREST).ToString();
		}

		private void AddvanceToPageIII()
		{
			straight.Clean();

			_terminationType = (TerminationType)System.Convert.ToInt32(tabControl201.SelectedTab.Tag);

			segment1Term = new Segment1Term(straight);

			segment1Term.FlyMode = (eFlyMode)comboBox203.SelectedIndex;
			segment1Term.terminationType = _terminationType;

			segment1Term.PDGChanged += termPDGChanged;
			segment1Term.NomPDGDistChanged += termNomPDGDistChanged;

			segment1Term.BankAngle = _BankAngle;
			segment1Term.ConstructionPDG = GlobalVars.constants.Pansops[Aran.PANDA.Constants.ePANSOPSData.dpMaxPosPDG].Value;
			segment1Term.NomLinePDG = GlobalVars.NomLineGrd;
			segment1Term.PlannedMaxTurnAngle = ARANMath.DegToRad(90);

			double height = GlobalVars.constants.Pansops[ePANSOPSData.dpNGui_Ar1].Value;
			double EPT = Functions.MinFlybyDistByHeightAtKKLine(GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value, ptDerPrj.Z, straight.PDG, segment1Term.PlannedMaxTurnAngle, segment1Term.Leg.EndFIX, height);

			_terminationDistance = (height - GlobalVars.constants.Pansops[ePANSOPSData.dpH_abv_DER].Value) / straight.PDG + EPT;

			terminationDistance = _terminationDistance;

			textBox202.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(_terminationDistance, eRoundMode.NEAREST).ToString();

			textBox204.Text = GlobalVars.unitConverter.GradientToDisplayUnits(GlobalVars.NomLineGrd, eRoundMode.NEAREST).ToString();
			textBox204.Tag = null;

			textBox203.Text = GlobalVars.unitConverter.GradientToDisplayUnits(straight.PDG, eRoundMode.NEAREST).ToString();
			textBox203.Tag = null;
			textBox203_Validating(textBox203, new CancelEventArgs());

			textBox211.Tag = null;
			textBox211_Validating(textBox211, new CancelEventArgs());

			segment1Term.UpdateEnabled = true;

			terminationDistance = _terminationDistance;
		}

		#endregion

		#region Page III

		private void comboBox205_SelectedIndexChanged(object sender, EventArgs e)
		{
			label231.Visible = terminationType != TerminationType.AtHeight && comboBox205.SelectedIndex == 0;
			textBox214.Visible = terminationType != TerminationType.AtHeight && comboBox205.SelectedIndex == 0;

			if (comboBox205.SelectedIndex == 0)
			{
				textBox207.ReadOnly = radioButton205.Checked;
				textBox202_Validating(textBox207, new CancelEventArgs());
			}
			else
			{
				textBox207.ReadOnly = true;
				WPT_FIXType SigPoint = (WPT_FIXType)comboBox205.SelectedItem;
				segment1Term.Leg.EndFIX.HorAccuracy = SigPoint.HorAccuracy;

				double fDist = ARANFunctions.PointToLineDistance(SigPoint.pPtPrj, SelectedRWY.pPtPrj[eRWY.ptDER], straight.InitialTrack - ARANMath.C_PI_2);

				terminationDistance = fDist;

				textBox207.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(terminationDistance, eRoundMode.NEAREST).ToString();
				textBox207.Tag = textBox202.Text;
			}
		}

		private void tabControl201_SelectedIndexChanged(object sender, EventArgs e)
		{
			terminationType = (TerminationType)System.Convert.ToInt32(tabControl201.SelectedTab.Tag);

			label231.Visible = terminationType != TerminationType.AtHeight && comboBox205.SelectedIndex == 0;
			textBox214.Visible = terminationType != TerminationType.AtHeight && comboBox205.SelectedIndex == 0;
		}

		private void termPDGChanged(object sender, double newPDG)
		{
			if (appliedPDG < segment1Term.MinPDG)
			{
				_appliedPDG = segment1Term.MinPDG;
				switch (_terminationType)
				{
					default:
					case TerminationType.AtHeight:
						textBox203.Text = Math.Round(100.0 * _appliedPDG, 1).ToString();
						textBox203.Tag = textBox203.Text;
						break;

					case TerminationType.AtWPT:
						textBox208.Text = Math.Round(100.0 * _appliedPDG, 1).ToString();
						textBox208.Tag = textBox208.Text;
						break;
				}
			}
		}

		private void termNomPDGDistChanged(object sender, double newNomPDGHeight)
		{
			ReportsFrm.FillPage2(segment1Term.InnerObstacleList, segment1Term.DetObs);
			Report = true;

			if (ReportBtn.Checked && !ReportsFrm.Visible)
				ReportsFrm.Show(GlobalVars.Win32Window);
		}

		private void comboBox201_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (segment1Term == null)
				return;

			double fTmp = _heightAtEndFIX;

			switch (_terminationType)
			{
				default:
				case TerminationType.AtHeight:
					if (comboBox201.SelectedIndex == 0)
						fTmp += ptDerPrj.Z;

					textBox201.Text = GlobalVars.unitConverter.HeightToDisplayUnits(fTmp, eRoundMode.NEAREST).ToString();
					textBox201.Tag = textBox201.Text;
					break;

				case TerminationType.AtWPT:
					if (comboBox202.SelectedIndex == 0)
						fTmp += ptDerPrj.Z;

					textBox206.Text = GlobalVars.unitConverter.HeightToDisplayUnits(fTmp, eRoundMode.NEAREST).ToString();
					textBox206.Tag = textBox206.Text;
					break;
			}
		}

		private void radioButton201_CheckedChanged(object sender, EventArgs e)
		{
			if (sender == radioButton205)
				comboBox205.Enabled = !radioButton205.Checked;

			if (!((RadioButton)sender).Checked)
				return;

			TextBox control1, control2, control3;

			switch (System.Convert.ToInt32(((Control)sender).Tag))
			{
				case 0:
					switch (_terminationType)
					{
						default:
						case TerminationType.AtHeight:
							control1 = textBox201;
							control2 = textBox202;
							control3 = textBox203;
							break;

						case TerminationType.AtWPT:
							control1 = textBox206;
							control2 = textBox207;
							control3 = textBox208;
							break;
					}

					break;
				case 1:
					switch (_terminationType)
					{
						default:
						case TerminationType.AtHeight:
							control1 = textBox202;
							control2 = textBox201;
							control3 = textBox203;

							break;

						case TerminationType.AtWPT:
							control1 = textBox207;
							control2 = textBox206;
							control3 = textBox208;

							break;
					}
					break;
				default:
					switch (_terminationType)
					{
						default:
						case TerminationType.AtHeight:
							control1 = textBox203;
							control2 = textBox202;
							control3 = textBox201;

							break;

						case TerminationType.AtWPT:
							control1 = textBox208;
							control2 = textBox207;
							control3 = textBox206;

							break;
					}
					break;
			}

			control1.ReadOnly = true;
			control2.ReadOnly = false;
			control3.ReadOnly = false;

			if (_terminationType == TerminationType.AtWPT && !radioButton205.Checked)
				textBox207.ReadOnly = comboBox205.SelectedIndex > 0;
		}

		private void textBox201_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;

			if (eventChar == 13)
				textBox201_Validating(sender, new System.ComponentModel.CancelEventArgs());
			else switch (_terminationType)
				{
					default:
					case TerminationType.AtHeight:
						Functions.TextBoxFloat(ref eventChar, textBox201.Text);
						break;

					case TerminationType.AtWPT:
						Functions.TextBoxFloat(ref eventChar, textBox206.Text);
						break;
				}

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox201_Validating(object sender, CancelEventArgs e)    //height
		{
			ComboBox cbox;
			RadioButton rbtn;
			TextBox tsender = (TextBox)sender;

			switch (_terminationType)
			{
				default:
				case TerminationType.AtHeight:
					rbtn = radioButton201;
					cbox = comboBox201;
					break;

				case TerminationType.AtWPT:
					rbtn = radioButton204;
					cbox = comboBox202;
					break;
			}

			if (rbtn.Checked)
				return;

			double fTmp;

			if (!double.TryParse(tsender.Text, out fTmp))
				return;

			if (tsender.Tag != null && tsender.Tag.ToString() == tsender.Text)
				return;

			double NewH = GlobalVars.unitConverter.HeightToInternalUnits(fTmp);

			if (cbox.SelectedIndex == 0)
				NewH -= ptDerPrj.Z;

			HeightAtEndFIX = NewH;
			if (HeightAtEndFIX != NewH)
			{
				fTmp = HeightAtEndFIX;
				if (cbox.SelectedIndex == 0)
					fTmp += ptDerPrj.Z;

				tsender.Text = GlobalVars.unitConverter.HeightToDisplayUnits(fTmp, eRoundMode.NEAREST).ToString();
			}

			tsender.Tag = tsender.Text;
		}

		private void textBox202_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;

			if (eventChar == 13)
				textBox202_Validating(sender, new System.ComponentModel.CancelEventArgs());
			else switch (_terminationType)
				{
					default:
					case TerminationType.AtHeight:
						Functions.TextBoxFloat(ref eventChar, textBox202.Text);
						break;

					case TerminationType.AtWPT:
						Functions.TextBoxFloat(ref eventChar, textBox207.Text);
						break;
				}

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox202_Validating(object sender, CancelEventArgs e)    //distance
		{
			RadioButton rbtn;
			TextBox tsender = (TextBox)sender;
			if (tsender.ReadOnly)
				return;

			switch (_terminationType)
			{
				default:
				case TerminationType.AtHeight:
					rbtn = radioButton202;
					break;

				case TerminationType.AtWPT:
					rbtn = radioButton205;
					break;
			}

			if (rbtn.Checked)
				return;

			double fTmp;

			if (!double.TryParse(tsender.Text, out fTmp))
				return;

			double NewDist = GlobalVars.unitConverter.DistanceToInternalUnits(fTmp);

			terminationDistance = NewDist;

			if (terminationDistance != NewDist)
				tsender.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(terminationDistance, eRoundMode.NEAREST).ToString();

			tsender.Tag = tsender.Text;
		}


		private void textBox203_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;

			switch (_terminationType)
			{
				default:
				case TerminationType.AtHeight:
					if (eventChar == 13)
						textBox203_Validating(textBox203, new System.ComponentModel.CancelEventArgs());
					else
						Functions.TextBoxFloat(ref eventChar, textBox203.Text);
					break;

				case TerminationType.AtWPT:
					if (eventChar == 13)
						textBox203_Validating(textBox208, new System.ComponentModel.CancelEventArgs());
					else
						Functions.TextBoxFloat(ref eventChar, textBox208.Text);
					break;
			}

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox203_Validating(object sender, CancelEventArgs e)        //applied PDG
		{
			RadioButton rbtn;
			TextBox tsender = (TextBox)sender;

			switch (_terminationType)
			{
				default:
				case TerminationType.AtHeight:
					rbtn = radioButton203;
					break;

				case TerminationType.AtWPT:
					rbtn = radioButton206;
					break;
			}

			if (rbtn.Checked)
				return;

			double fTmp;

			if (!double.TryParse(tsender.Text, out fTmp))
				return;

			double newPDG = GlobalVars.unitConverter.GradientToInternalUnits(fTmp);

			appliedPDG = newPDG;
			//================================================================================================================
			if (appliedPDG != newPDG)
				tsender.Text = GlobalVars.unitConverter.GradientToDisplayUnits(appliedPDG).ToString();

			tsender.Tag = tsender.Text;
		}

		private void textBox204_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;

			if (eventChar == 13)
				textBox204_Validating(sender, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox204.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox204_Validating(object sender, CancelEventArgs e)
		{
			if (_terminationType != TerminationType.AtHeight)
				return;

			double fTmp;

			if (!double.TryParse(textBox204.Text, out fTmp))
				return;

			double newPDG = GlobalVars.unitConverter.GradientToInternalUnits(fTmp);

			segment1Term.NomLinePDG = newPDG;
			//================================================================================================================
			if (segment1Term.NomLinePDG != newPDG)
				textBox204.Text = GlobalVars.unitConverter.GradientToDisplayUnits(segment1Term.NomLinePDG).ToString();

			textBox204.Tag = textBox204.Text;
		}

		private void comboBox206_SelectedIndexChanged(object sender, EventArgs e)
		{
			double bankAngle;

			if (!double.TryParse(comboBox206.Text, out bankAngle))
				return;

			if (comboBox206.Tag != null && comboBox206.Tag.ToString() == comboBox206.Text)
				return;

			_BankAngle = ARANMath.DegToRad(bankAngle);
			comboBox206.Tag = comboBox206.Text;

			if (segment1Term != null)
			{
				LegDep leg = segment1Term.Leg;
				leg.EndFIX.OutDirection = leg.EndFIX.EntryDirection + _BankAngle;

				segment1Term.BankAngle = _BankAngle;

				textBox213.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(segment1Term.Leg.EndFIX.ConstructTurnRadius, eRoundMode.NEAREST).ToString();
			}
		}

		private void textBox205_KeyPress(object sender, KeyPressEventArgs e)
		{
			//char eventChar = e.KeyChar;
			//if (eventChar == 13)
			//	textBox205_Validating(textBox205, new System.ComponentModel.CancelEventArgs());
			//else
			//	Functions.TextBoxFloat(ref eventChar, textBox205.Text);

			//e.KeyChar = eventChar;
			//if (eventChar == 0)
			//	e.Handled = true;
		}

		private void textBox205_Validating(object sender, CancelEventArgs e)
		{
			//double fTmp;

			//if (!double.TryParse(textBox205.Text, out fTmp))
			//	return;

			//if (textBox205.Tag != null && textBox205.Tag.ToString() == textBox205.Text)
			//	return;

			//double bankAngle = fTmp;

			//if (bankAngle < GlobalVars.MinBankAngle)
			//	bankAngle = GlobalVars.MinBankAngle;

			//if (bankAngle > GlobalVars.MaxBankAngle)
			//	bankAngle = GlobalVars.MaxBankAngle;

			//if (fTmp != bankAngle)
			//	textBox205.Text = bankAngle.ToString();

			//_BankAngle = ARANMath.DegToRad(bankAngle);
			//textBox205.Tag = textBox205.Text;

			////if (terminationType != TerminationType.AtHeight && segment1Term != null)
			////{
			////	LegDep leg = segment1Term.Leg;
			////	leg.EndFIX.OutDirection = leg.EndFIX.EntryDirection + ARANMath.DegToRad(fTmp);

			////	segment1Term.BankAngle = _BankAngle;

			////	fillComboBox205();

			////	//double minDist;

			////	//if (radioButton204.Checked)
			////	//	if (appliedPDG < GlobalVars.constants.Pansops[ePANSOPSData.dpPDG_Nom].Value)
			////	//		minDist = (_terminationHeight - GlobalVars.constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value) / GlobalVars.constants.Pansops[ePANSOPSData.dpPDG_Nom].Value;
			////	//	else
			////	//		minDist = (GlobalVars.constants.Pansops[ePANSOPSData.dpGui_Ar1].Value - GlobalVars.constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value) / appliedPDG;
			////	//else
			////	//	minDist = (_terminationHeight - GlobalVars.constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value) / GlobalVars.constants.Pansops[ePANSOPSData.dpMaxPosPDG].Value;

			////	//if (_terminationDistance < minDist + _EPTMin)
			////	//	terminationDistance = minDist + _EPTMin;
			////	terminationDistance = _terminationDistance;
			////	textBox213.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(segment1Term.Leg.EndFIX.ConstructTurnRadius, eRoundMode.NERAEST).ToString();
			////}
		}

		private void checkBox201_CheckedChanged(object sender, EventArgs e)
		{
			//segment1Term.Ensure90m = checkBox201.Checked;
			//double minHeightLimit = GlobalVars.constants.Pansops[ePANSOPSData.dpGui_Ar1].Value;

			//if (checkBox201.Checked)
			//    minHeightLimit = Math.Max(minHeightLimit, segment1Term.DetObs.Height + GlobalVars.constants.Pansops[ePANSOPSData.dpObsClr].Value);
		}

		private void checkBox202_CheckedChanged(object sender, EventArgs e)
		{
			segment1Term.TurnBeforeDer = checkBox202.Checked;

			ReportsFrm.FillPage2(segment1Term.InnerObstacleList, segment1Term.DetObs);
			Report = true;

			if (ReportBtn.Checked && !ReportsFrm.Visible)
				ReportsFrm.Show(GlobalVars.Win32Window);
		}

		private void textBox211_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox211_Validating(textBox211, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox211.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void fillComboBox205()
		{
			WPT_FIXType OldFix = new WPT_FIXType();
			if (comboBox205.SelectedIndex > 0)
				OldFix = (WPT_FIXType)comboBox205.SelectedItem;
			int newIndex = 0;
			double minDist;
			WayPoint EndFIX = segment1Term.Leg.EndFIX;

			double minEPT = Functions.MinFlybyDistByHeightAtKKLine(GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value, ptDerPrj.Z, _appliedPDG, segment1Term.PlannedMaxTurnAngle, EndFIX);

			if (radioButton204.Checked)
				if (appliedPDG > GlobalVars.constants.Pansops[ePANSOPSData.dpPDG_Nom].Value)
					minDist = (GlobalVars.constants.Pansops[ePANSOPSData.dpGui_Ar1].Value - GlobalVars.constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value) / appliedPDG + minEPT;
				else
					minDist = (GlobalVars.constants.Pansops[ePANSOPSData.dpGui_Ar1].Value - GlobalVars.constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value) / GlobalVars.constants.Pansops[ePANSOPSData.dpPDG_Nom].Value + minEPT;
			else
				minDist = MinTermDist + minEPT;

			minDist = Math.Max(MinTermDist + minEPT, minDist);

			comboBox205.Items.Clear();

			comboBox205.Items.Add("new ...");

			foreach (WPT_FIXType SigPoint in GlobalVars.WPTList)
			{
				double fDistDER = ARANFunctions.PointToLineDistance(SigPoint.pPtPrj, SelectedRWY.pPtPrj[eRWY.ptDER], straight.InitialTrack - ARANMath.C_PI_2);
				double fDistCL = ARANFunctions.Point2LineDistancePrj(SigPoint.pPtPrj, SelectedRWY.pPtPrj[eRWY.ptDER], straight.InitialTrack);

				if (fDistCL < 1.2 && fDistDER > minDist && fDistDER <= GlobalVars.RModel)
				{
					if (SigPoint.Name == OldFix.Name && SigPoint.CallSign == OldFix.CallSign)
						newIndex = comboBox205.Items.Count;

					comboBox205.Items.Add(SigPoint);
				}
			}

			if (comboBox205.Items.Count > 0 && terminationType != TerminationType.AtHeight)
				comboBox205.SelectedIndex = newIndex;
		}

		private void textBox211_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(textBox211.Text, out fTmp))
				return;

			if (textBox211.Tag != null && textBox211.Tag.ToString() == textBox211.Text)
				return;

			if (fTmp > maxTurnAngle)
			{
				fTmp = maxTurnAngle;
				textBox211.Text = maxTurnAngle.ToString();
			}

			double PlannedTurn = ARANMath.DegToRad(fTmp);
			segment1Term.PlannedMaxTurnAngle = PlannedTurn;

			LegDep leg = segment1Term.Leg;
			leg.EndFIX.OutDirection = leg.EndFIX.EntryDirection + PlannedTurn;

			fillComboBox205();

			terminationDistance = _terminationDistance;
			textBox211.Tag = textBox211.Text;
		}

		private void textBox212_KeyPress(object sender, KeyPressEventArgs e)
		{
			//char eventChar = e.KeyChar;

			//if (eventChar == 13)
			//	textBox212_Validating(sender, new System.ComponentModel.CancelEventArgs());
			//else
			//	Functions.TextBoxFloat(ref eventChar, textBox212.Text);

			//e.KeyChar = eventChar;
			//if (eventChar == 0)
			//	e.Handled = true;
		}

		private void textBox212_Validating(object sender, CancelEventArgs e)
		{
			//double fTmp;

			//if (!double.TryParse(textBox212.Text, out fTmp))
			//	return;

			//if (textBox212.Tag != null && textBox212.Tag.ToString() == textBox212.Text)
			//	return;

			//double NewH = GlobalVars.unitConverter.HeightToInternalUnits(fTmp);
			////if (comboBox202.SelectedIndex == 0)
			////    NewH -= ptDerPrj.Z;

			//segment1Term.ConstructAltitude = NewH;
			//segment1Term.ConstructionPDG = (NewH - ptDerPrj.Z - GlobalVars.constants.Pansops[ePANSOPSData.dpOIS_abv_DER].Value) / _terminationDistance;

			//textBox212.Tag = textBox212.Text;
			//textBox209.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(segment1Term.Leg.EndFIX.ConstructTAS, eRoundMode.NEAREST).ToString();
			//textBox213.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(segment1Term.Leg.EndFIX.ConstructTurnRadius, eRoundMode.NEAREST).ToString();
		}

		private void comboBox203_SelectedIndexChanged(object sender, EventArgs e)
		{
			label232.Visible = comboBox203.SelectedIndex == 0;
			label233.Visible = comboBox203.SelectedIndex == 0;
			textBox215.Visible = comboBox203.SelectedIndex == 0;

			if (segment1Term == null)
				return;

			segment1Term.FlyMode = (eFlyMode)comboBox203.SelectedIndex;
			terminationDistance = _terminationDistance;
		}

		#endregion

		#region Page IV

		void AddvanceToPageIV()
		{
			LegDep leg = segment1Term.Leg;

			leg.Obstacles = segment1Term.InnerObstacleList;
			leg.DetObstacle_1 = segment1Term.DetObs;
			leg.EndFIX.Name = textBox214.Text;

			if (leg.EndFIX.FlyMode == eFlyMode.Atheight)
			{
				leg.PathAndTermination = CodeSegmentPath.CA;
				leg.MinLegLength = ARANFunctions.ReturnDistanceInMeters(leg.StartFIX.PrjPt, leg.EndFIX.NomLinePrjPt);
			}
			else
			{
				leg.PathAndTermination = CodeSegmentPath.CF;
				if (comboBox205.SelectedIndex != 0)
				{
					WPT_FIXType SigPoint = (WPT_FIXType)comboBox205.SelectedItem;
					leg.EndFIX.Name = SigPoint.Name;
					leg.EndFIX.CallSign = SigPoint.CallSign;
					leg.EndFIX.Id = SigPoint.Identifier;
				}

				Point ptTmp = ARANFunctions.LocalToPrj(leg.EndFIX.PrjPt, leg.EndFIX.EntryDirection, -leg.EndFIX.EPT, 0);
				leg.MinLegLength = ARANFunctions.ReturnDistanceInMeters(leg.StartFIX.NomLinePrjPt, ptTmp);
			}

			leg.Duration = leg.MinLegLength / leg.StartFIX.NomLineTAS;
			textBox310.Text = (leg.Duration / 60.0).ToString("0.00");

			for (int i = 0; i < leg.Obstacles.Parts.Length; i++)
				leg.Obstacles.Parts[i].DistStar = leg.Obstacles.Parts[i].d0 = leg.Obstacles.Parts[i].Dist;

			ReportsFrm.FillPage3(leg.Obstacles, leg.DetObstacle_1);
			ReportsFrm.AddPage4(leg.Obstacles, leg.DetObstacle_1);
			Report = true;

			//=====================================================================================================
			UpDown301Range.Circular = true;
			UpDown301Range.InDegree = true;
			radioButton301.Checked = true;
			radioButton303.Checked = true;

			segment1Term.Clean();

			_SignificantPoints = new List<WayPoint>();
			for (int i = 0; i < GlobalVars.WPTList.Length; i++)
				_SignificantPoints.Add(new WayPoint(eFIXRole.TP_, GlobalVars.WPTList[i], GlobalVars.gAranEnv));

			transitions = new Transitions(_SignificantPoints, segment1Term, SelectedRWY, 300000.0, _aircat, GlobalVars.gAranEnv, InitNewPoint);

			transitions.OnFIXUpdated = FIXUpdated;
			transitions.OnDistanceChanged = OnLegDistanceChanged;
			transitions.OnConstructAltitudeChanged = OnConstructionAltitudeChanged;
			transitions.OnNomlineAltitudeChanged = OnNomLineAltitudeChanged;
			transitions.AccelerationAltitude = _accelerationAltitude;

			transitions.BankAngle = _BankAngle;
			transitions.OnUpdateDirList = OnUpdateDirList;
			transitions.OnUpdateDistList = OnUpdateDistList;

			transitions.MOCLimit = EnrouteMOCValues[comboBox309.SelectedIndex];

			UpDown301Range.Max = ARANFunctions.DirToAzimuth(transitions.CourseOutConvertionPoint, transitions.OutDirRange.Min, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
			UpDown301Range.Min = ARANFunctions.DirToAzimuth(transitions.CourseOutConvertionPoint, transitions.OutDirRange.Max, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

			transitions.ConstructGradient = GlobalVars.constants.Pansops[ePANSOPSData.dpMaxPosPDG].Value;
			transitions.NomLineGradient = GlobalVars.NomLineGrd;
			textBox307.Text = (100.0 * transitions.NomLineGradient).ToString();

			comboBox306.Items.Clear();
			foreach (ePBNClass pbn in firstTurnPBNClass)
				comboBox306.Items.Add(pbn);

			comboBox307.Items.Clear();

			if (segment1Term.terminationType == TerminationType.AtHeight)
				foreach (CodeSegmentPath pt in AtHeightPathAndTerminations)
					comboBox307.Items.Add(pt);
			else if (segment1Term.FlyMode == eFlyMode.Flyover)
				foreach (CodeSegmentPath pt in FlyOverPathAndTerminations)
					comboBox307.Items.Add(pt);
			else
				foreach (CodeSegmentPath pt in FlyByPathAndTerminations)
					comboBox307.Items.Add(pt);

			comboBox302.SelectedIndex = 0;

			if (comboBox305.SelectedIndex != 0)
				comboBox305.SelectedIndex = 0;
			else
				transitions.FlyMode = (eFlyMode)comboBox305.SelectedIndex;

			if (comboBox304.SelectedIndex != 0)
				comboBox304.SelectedIndex = 0;
			else
				transitions.SensorType = (eSensorType)comboBox304.SelectedIndex;

			if (comboBox306.SelectedIndex != 0)
				comboBox306.SelectedIndex = 0;
			else
				transitions.PBNClass = (ePBNClass)comboBox306.SelectedItem;

			if (comboBox307.SelectedIndex != 0)
				comboBox307.SelectedIndex = 0;
			else
				transitions.PathAndTermination = (CodeSegmentPath)comboBox307.SelectedItem;

			numericUpDown302.Value = (decimal)ARANMath.RadToDeg(transitions.PlannedTurnAngle);

			textBox302.Text = transitions.FIXName;
			textBox303.Text = textBox101.Text;
			textBox303.Tag = null;

			double nnAzt = ARANFunctions.DirToAzimuth(transitions.ReferenceFIX.NomLinePrjPt, transitions.EntryDirection, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
			numericUpDown301.Value = (decimal)nnAzt;

			transitions.UpdateEnabled = true;

			textBox301.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(transitions.Distance, eRoundMode.NEAREST).ToString();

			double TAS = ARANMath.IASToTAS(transitions.IAS, transitions.ConstructAltitude, GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value);

			textBox306.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(TAS, eRoundMode.SPECIAL_NEAREST).ToString();
			textBox305.Text = GlobalVars.unitConverter.HeightToDisplayUnits(transitions.ConstructAltitude, eRoundMode.NEAREST).ToString();
			textBox304.Text = GlobalVars.unitConverter.GradientToDisplayUnits(transitions.ConstructGradient, eRoundMode.NEAREST).ToString();

			TAS = ARANMath.IASToTAS(transitions.IAS, transitions.NomLineAltitude, GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value);
			textBox309.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(TAS, eRoundMode.NEAREST).ToString();
			textBox308.Text = GlobalVars.unitConverter.HeightToDisplayUnits(transitions.NomLineAltitude, eRoundMode.NEAREST).ToString();
			textBox307.Text = GlobalVars.unitConverter.GradientToDisplayUnits(transitions.NomLineGradient, eRoundMode.NEAREST).ToString();

			UpDateComboBox308(true);
		}

		#region transitions events

		void InitNewPoint(object sender, WayPoint ReferenceFIX, WayPoint AddedFIX)
		{
			ReferenceFIX.ISAtC = GlobalVars.CurrADHP.ISAtC;
			AddedFIX.ISAtC = GlobalVars.CurrADHP.ISAtC;

			_AddedFIX = AddedFIX;

			if (transitions != null)
			{
				UpDown301Range.Max = ARANFunctions.DirToAzimuth(transitions.CourseOutConvertionPoint, transitions.OutDirRange.Min, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
				UpDown301Range.Min = ARANFunctions.DirToAzimuth(transitions.CourseOutConvertionPoint, transitions.OutDirRange.Max, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

				textBox302.Text = transitions.FIXName;
			}
		}

		void FIXUpdated(object sender, WayPoint CurrFIX)
		{
			CurrFIX.ISAtC = GlobalVars.CurrADHP.ISAtC;

			ReportsFrm.FillPage3(transitions.CurrentObstacleList, transitions.CurrentDetObs);
			Report = true;

			if (ReportBtn.Checked && !ReportsFrm.Visible)
				ReportsFrm.Show(GlobalVars.Win32Window);
		}

		void OnConstructionAltitudeChanged(object sender, double value)
		{
			double TAS = ARANMath.IASToTAS(transitions.IAS, value, GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value);
			textBox306.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(TAS, eRoundMode.SPECIAL_NEAREST).ToString();

			textBox305.Text = GlobalVars.unitConverter.HeightToDisplayUnits(value, eRoundMode.NEAREST).ToString();
			textBox305.Tag = textBox305.Text;
			textBox304.Text = GlobalVars.unitConverter.GradientToDisplayUnits(transitions.ConstructGradient, eRoundMode.NEAREST).ToString();
		}

		void OnNomLineAltitudeChanged(object sender, double value)
		{
			double TAS = ARANMath.IASToTAS(transitions.IAS, transitions.NomLineAltitude, GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value);

			textBox309.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(TAS, eRoundMode.NEAREST).ToString();

			textBox308.Text = GlobalVars.unitConverter.HeightToDisplayUnits(value, eRoundMode.NEAREST).ToString();
			textBox308.Tag = textBox308.Text;
			textBox307.Text = GlobalVars.unitConverter.GradientToDisplayUnits(transitions.NomLineGradient, eRoundMode.NEAREST).ToString();
		}

		void OnLegDistanceChanged(object sender, double value)
		{
			textBox301.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(value, eRoundMode.NEAREST).ToString();
			textBox301.Tag = textBox301.Text;
			textBox304.Text = GlobalVars.unitConverter.GradientToDisplayUnits(transitions.ConstructGradient, eRoundMode.NEAREST).ToString();
		}

		void OnUpdateDirList(object sender, EventArgs e)
		{
			if (transitions == null)
				return;

			transitions.turnDirection = comboBox302.SelectedIndex == 0 ? TurnDirection.CCW : TurnDirection.CW;

			WayPoint OldWpt = null;
			if (comboBox301.SelectedIndex >= 0)
				OldWpt = (WayPoint)comboBox301.SelectedItem;

			int wptIndex = -1;
			comboBox301.Items.Clear();

			foreach (WayPoint wpt in transitions.CourseSgfPoint)
			{
				if (wpt.Equals(OldWpt))
					wptIndex = comboBox301.Items.Count;

				try
				{
					comboBox301.Items.Add(wpt);
				}
				catch
				{
					break;
				}
			}

			radioButton302.Enabled = comboBox301.Items.Count > 0;
			if (radioButton302.Enabled)
			{
				if (wptIndex >= 0)
					comboBox301.SelectedIndex = wptIndex;
				else
					comboBox301.SelectedIndex = 0;
			}
			else
				radioButton301.Checked = true;
		}

		void OnUpdateDistList(object sender, EventArgs e)
		{
			if (transitions == null)
				return;

			WayPoint OldWpt = null;
			int OldIndex = -1;

			if (comboBox303.SelectedIndex >= 0)
				OldWpt = (WayPoint)comboBox303.SelectedItem;

			comboBox303.Items.Clear();

			foreach (WayPoint wpt in transitions.DistanceSgfPoint)
			{
				if (OldWpt != null && wpt.Equals(OldWpt))
					OldIndex = comboBox303.Items.Count;
				comboBox303.Items.Add(wpt);
			}

			radioButton304.Enabled = comboBox303.Items.Count > 0;
			if (radioButton304.Enabled)
			{
				if (OldIndex >= 0)
					comboBox303.SelectedIndex = OldIndex;
				else
					comboBox303.SelectedIndex = 0;
			}
			else
				radioButton303.Checked = true;
		}

		#endregion

		private void button301_Click(object sender, EventArgs e)
		{
			LegsInfoForm.ShowFixInfo(Left + MultiPage1.Left + _MultiPage1_TabPage3.Left + button301.Left, Top + _MultiPage1_TabPage3.Top + button301.Top, transitions.LegPoints);
		}

		private void comboBox304_SelectedIndexChanged(object sender, EventArgs e)
		{
			checkBox301.Enabled = comboBox304.SelectedIndex == 1;
			transitions.SensorType = (eSensorType)comboBox304.SelectedIndex;
		}

		private void comboBox305_SelectedIndexChanged(object sender, EventArgs e)
		{
			transitions.FlyMode = (eFlyMode)comboBox305.SelectedIndex;
		}

		private void comboBox306_SelectedIndexChanged(object sender, EventArgs e)
		{
			transitions.PBNClass = (ePBNClass)comboBox306.SelectedItem;
		}

		private void comboBox307_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBox307.SelectedIndex < 0)
				return;

			transitions.PathAndTermination = (CodeSegmentPath)comboBox307.SelectedItem;

			bool isSame = false;
			if (radioButton302.Checked && radioButton304.Checked && _AddedFIX.FlyMode == eFlyMode.Atheight && transitions.PathAndTermination == CodeSegmentPath.CF)
			{
				WayPoint dirPoint = (WayPoint)comboBox301.SelectedItem;
				WayPoint sigPoint = (WayPoint)comboBox303.SelectedItem;
				isSame = sigPoint.Id == dirPoint.Id;
			}

			numericUpDown301.Enabled = radioButton301.Checked || isSame;// (radioButton304.Checked && _AddedFIX.FlyMode == eFlyMode.Atheight && transitions.PathAndTermination == ePathAndTermination.CF);

			comboBox302.Visible = transitions.PathAndTermination == CodeSegmentPath.DF;
			label303.Visible = comboBox302.Visible;

			double nnAzt = ARANFunctions.DirToAzimuth(transitions.ReferenceFIX.NomLinePrjPt, transitions.EntryDirection, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

			if ((CodeSegmentPath)comboBox307.SelectedItem == CodeSegmentPath.DF)
			{
				transitions.turnDirection = comboBox302.SelectedIndex == 0 ? TurnDirection.CCW : TurnDirection.CW;
				UpDown301Range.Min = ARANMath.Modulus(nnAzt - 270.0, 360.0);
				UpDown301Range.Max = ARANMath.Modulus(nnAzt + 270.0, 360.0);
			}
			else
			{
				UpDown301Range.Min = ARANMath.Modulus(nnAzt - maxTurnAngle, 360.0);
				UpDown301Range.Max = ARANMath.Modulus(nnAzt + maxTurnAngle, 360.0);
			}
		}

		#region direction

		private void radioButton301_CheckedChanged(object sender, EventArgs e)
		{
			if (transitions != null)
				transitions.DirectionIndex = -1;

			bool isSame = false;
			if (radioButton302.Checked && radioButton304.Checked && _AddedFIX.FlyMode == eFlyMode.Atheight && transitions.PathAndTermination == CodeSegmentPath.CF)
			{
				WayPoint dirPoint = (WayPoint)comboBox301.SelectedItem;
				WayPoint sigPoint = (WayPoint)comboBox303.SelectedItem;
				isSame = sigPoint.Id == dirPoint.Id;
			}

			numericUpDown301.Enabled = radioButton301.Checked || isSame;// (radioButton304.Checked && _AddedFIX.FlyMode == eFlyMode.Atheight && transitions.PathAndTermination == ePathAndTermination.CF);

			comboBox301.Enabled = !radioButton301.Checked;

			if (!radioButton301.Checked)
				comboBox301_SelectedIndexChanged(comboBox301, new EventArgs());
		}

		private bool PreventRecurse301 = false;

		private void numericUpDown301_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				if (numericUpDown301.Value == numericUpDown301.Minimum)
					numericUpDown301.Value = numericUpDown301.Maximum;
				else if (numericUpDown301.Value == numericUpDown301.Maximum)
					numericUpDown301.Value = numericUpDown301.Minimum;
			}
		}

		private void numericUpDown301_ValueChanged(object sender, EventArgs e)
		{
			double newAzt = (double)numericUpDown301.Value;
			double Direction;
			Interval OutDirRange = transitions.OutDirRange;

			bool isSame = false;
			if (radioButton302.Checked && radioButton304.Checked && _AddedFIX.FlyMode == eFlyMode.Atheight && transitions.PathAndTermination == CodeSegmentPath.CF)
			{
				WayPoint dirPoint = (WayPoint)comboBox301.SelectedItem;
				WayPoint sigPoint = (WayPoint)comboBox303.SelectedItem;
				isSame = sigPoint.Id == dirPoint.Id;
			}

			if (isSame)
			{
				WayPoint dirSig = (WayPoint)comboBox301.SelectedItem;

				Direction = ARANFunctions.AztToDirection(_AddedFIX.NomLineGeoPt, newAzt, GlobalVars.pSpRefGeo, GlobalVars.pSpRefPrj);

				double OutDir = Math.Atan2(dirSig.PrjPt.Y - _AddedFIX.NomLinePrjPt.Y, dirSig.PrjPt.X - _AddedFIX.NomLinePrjPt.X);

				double legWidth = _AddedFIX.TurnDirection == TurnDirection.CW ? _AddedFIX.ASW_L : _AddedFIX.ASW_R;
				double sinAlpha = legWidth / transitions.Distance;
				if (sinAlpha > 0.5)
					sinAlpha = 0.5;
				double maxTrackOffset = Math.Asin(sinAlpha);

				if (_AddedFIX.TurnDirection == TurnDirection.CCW)
				{
					OutDirRange.Min = OutDir - maxTrackOffset;  //OutDir1;	// 
					OutDirRange.Max = OutDir;
				}
				else
				{
					OutDirRange.Min = OutDir;
					OutDirRange.Max = OutDir + maxTrackOffset;  //OutDir1;	//
				}
			}

			if (!PreventRecurse301)
			{
				if (newAzt <= (double)numericUpDown301.Minimum)
					newAzt = (double)numericUpDown301.Maximum - 1;
				else if (newAzt >= (double)numericUpDown301.Maximum)
					newAzt = (double)numericUpDown301.Minimum + 1;

				UpDown301Range.Max = ARANFunctions.DirToAzimuth(transitions.CourseOutConvertionPoint, OutDirRange.Min, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
				UpDown301Range.Min = ARANFunctions.DirToAzimuth(transitions.CourseOutConvertionPoint, OutDirRange.Max, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

				double OldVal = newAzt;
				double newValue = UpDown301Range.CheckValue(newAzt);

				PreventRecurse301 = numericUpDown301.Value != (decimal)newValue;

				if (PreventRecurse301)
				{
					numericUpDown301.Value = (decimal)newValue;
					return;
				}
			}

			PreventRecurse301 = false;

			if (_AddedFIX != null && _AddedFIX.GeoPt != null)
				Direction = ARANFunctions.AztToDirection(_AddedFIX.NomLineGeoPt, newAzt, GlobalVars.pSpRefGeo, GlobalVars.pSpRefPrj);
			else
				Direction = ARANFunctions.AztToDirection(ptDerGeo, newAzt, GlobalVars.pSpRefGeo, GlobalVars.pSpRefPrj);

			Direction = ARANMath.Modulus(Direction, ARANMath.C_2xPI);       ///????????????????????????????????

			if (isSame)
				Direction = OutDirRange.CheckValue(Direction);

			transitions.OutDirection = Direction;

			textBox301.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(transitions.Distance, eRoundMode.NEAREST).ToString();
			textBox301.Tag = textBox301.Text;

			double TAS = ARANMath.IASToTAS(transitions.IAS, transitions.ConstructAltitude, GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value);
			textBox306.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(TAS, eRoundMode.SPECIAL_NEAREST).ToString();
			textBox305.Text = GlobalVars.unitConverter.HeightToDisplayUnits(transitions.ConstructAltitude, eRoundMode.NEAREST).ToString();
			textBox305.Tag = textBox305.Text;
			textBox304.Text = GlobalVars.unitConverter.GradientToDisplayUnits(transitions.ConstructGradient, eRoundMode.NEAREST).ToString();

			TAS = ARANMath.IASToTAS(transitions.IAS, transitions.NomLineAltitude, GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value);
			textBox309.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(TAS, eRoundMode.NEAREST).ToString();
			textBox308.Text = GlobalVars.unitConverter.HeightToDisplayUnits(transitions.NomLineAltitude, eRoundMode.NEAREST).ToString();
			textBox308.Tag = textBox308.Text;
			textBox307.Text = GlobalVars.unitConverter.GradientToDisplayUnits(transitions.NomLineGradient, eRoundMode.NEAREST).ToString();

			UpDateComboBox308();
		}

		private void comboBox301_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (radioButton301.Checked)
				return;

			if (!comboBox301.Enabled)
				return;

			int k = comboBox301.SelectedIndex;
			if (k < 0)
				return;

			transitions.DirectionIndex = k;

			WayPoint sigPoint = (WayPoint)comboBox301.SelectedItem;
			Point FixPt = _AddedFIX.NomLinePrjPt;

			label302.Text = sigPoint.Role.ToString();               // AIXMTypeNames[sigPoint.AIXMType];

			double newAzt, Direction;

			if (transitions.PathAndTermination == CodeSegmentPath.DF)
			{
				double fTurnDirection = -(int)(_AddedFIX.TurnDirection);
				double r = _AddedFIX.NomLineTurnRadius;
				Point ptCenter = ARANFunctions.LocalToPrj(FixPt, _AddedFIX.EntryDirection, 0, -r * fTurnDirection);

				//GlobalVars.gAranGraphics.DrawPointWithText(ptCenter, "ptCenter-2");
				//LegBase.ProcessMessages();

				double dX = sigPoint.PrjPt.X - ptCenter.X;
				double dY = sigPoint.PrjPt.Y - ptCenter.Y;

				double distDest = ARANMath.Hypot(dX, dY);
				double dirDest = Math.Atan2(dY, dX);

				double TurnAngle = (_AddedFIX.EntryDirection - dirDest) * fTurnDirection + ARANMath.C_PI_2 - Math.Acos(r / distDest);

				Direction = ARANMath.Modulus(_AddedFIX.EntryDirection - TurnAngle * fTurnDirection, ARANMath.C_2xPI);

				//Point ptFrom = ARANFunctions.LocalToPrj(ptCenter, Direction, 0.0, r * fTurnDirection);
				//GlobalVars.gAranGraphics.DrawPointWithText(ptFrom, "ptFrom");
				//LegBase.ProcessMessages();
			}
			else
				Direction = ARANMath.Modulus(Math.Atan2(sigPoint.PrjPt.Y - FixPt.Y, sigPoint.PrjPt.X - FixPt.X), ARANMath.C_2xPI);
			//??????????????????????????
			//else if ((_AddedFIX.FlyMode == eFlyMode.Flyover || _AddedFIX.FlyMode == eFlyMode.Atheight) && transitions.PathAndTermination == ePathAndTermination.CF)
			//{
			//}

			if (FixPt != null)
				newAzt = ARANFunctions.DirToAzimuth(FixPt, Direction, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
			else
				newAzt = ARANFunctions.DirToAzimuth(ptDerPrj, Direction, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

			if (numericUpDown301.Value != (decimal)newAzt)
			{
				numericUpDown301.Value = (decimal)newAzt;
				return;
			}

			transitions.OutDirection = Direction;

			textBox301.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(transitions.Distance, eRoundMode.NEAREST).ToString();
			textBox301.Tag = textBox301.Text;

			double TAS = ARANMath.IASToTAS(transitions.IAS, transitions.ConstructAltitude, GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value);
			textBox306.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(TAS, eRoundMode.SPECIAL_NEAREST).ToString();

			textBox305.Text = GlobalVars.unitConverter.HeightToDisplayUnits(transitions.ConstructAltitude, eRoundMode.NEAREST).ToString();
			textBox305.Tag = textBox305.Text;

			textBox304.Text = GlobalVars.unitConverter.GradientToDisplayUnits(transitions.ConstructGradient, eRoundMode.NEAREST).ToString();

			UpDateComboBox308();
		}

		private void comboBox302_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (transitions == null)
				return;

			double fTmp = transitions.OutDirection;

			transitions.turnDirection = comboBox302.SelectedIndex == 0 ? TurnDirection.CCW : TurnDirection.CW;
			if (fTmp != transitions.OutDirection)
			{
				double newAzt;
				if (_AddedFIX != null && _AddedFIX.NomLinePrjPt != null)
					newAzt = ARANFunctions.DirToAzimuth(_AddedFIX.NomLinePrjPt, transitions.OutDirection, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
				else
					newAzt = ARANFunctions.DirToAzimuth(ptDerPrj, transitions.OutDirection, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

				numericUpDown301.Value = (decimal)newAzt;
			}
		}

		#endregion

		#region distance

		private void UpDateComboBox308(bool decreasingEnabled = false)
		{
			int SelectedIndex = 0;
			for (int i = 0; i < heights.Length; i++)
				if (transitions.NomLineAltitude - ptDerPrj.Z + 30.0 < heights[i])
				{
					SelectedIndex = i;
					break;
				}

			if (decreasingEnabled || SelectedIndex > comboBox308.SelectedIndex)
				comboBox308.SelectedIndex = SelectedIndex;
		}

		private void radioButton303_CheckedChanged(object sender, EventArgs e)
		{
			textBox301.ReadOnly = !radioButton303.Checked;
			comboBox303.Enabled = !radioButton303.Checked;
			textBox302.ReadOnly = !radioButton303.Checked;

			bool isSame = false;
			if (radioButton302.Checked && radioButton304.Checked && _AddedFIX.FlyMode == eFlyMode.Atheight && transitions.PathAndTermination == CodeSegmentPath.CF)
			{
				WayPoint dirPoint = (WayPoint)comboBox301.SelectedItem;
				WayPoint sigPoint = (WayPoint)comboBox303.SelectedItem;
				isSame = sigPoint.Id == dirPoint.Id;
			}

			numericUpDown301.Enabled = radioButton301.Checked || isSame;// (radioButton304.Checked && _AddedFIX.FlyMode == eFlyMode.Atheight && transitions.PathAndTermination == ePathAndTermination.CF);

			if (transitions == null)
				return;

			if (radioButton304.Checked)
				comboBox303_SelectedIndexChanged(comboBox303, new EventArgs());
			else
				transitions.DistanceIndex = -1;

			textBox302.Text = transitions.FIXName;
		}

		private void textBox301_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox301_Validating(textBox301, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox301.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox301_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(textBox301.Text, out fTmp))
				return;

			if (textBox301.Tag != null && textBox301.Tag.ToString() == textBox301.Text)
				return;

			double _Distance = GlobalVars.unitConverter.DistanceToInternalUnits(fTmp);

			transitions.Distance = _Distance;

			textBox301.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(transitions.Distance, eRoundMode.NEAREST).ToString();
			textBox301.Tag = textBox301.Text;

			textBox305.Text = GlobalVars.unitConverter.HeightToDisplayUnits(transitions.ConstructAltitude, eRoundMode.NEAREST).ToString();
			textBox305.Tag = textBox305.Text;

			textBox304.Text = GlobalVars.unitConverter.GradientToDisplayUnits(transitions.ConstructGradient, eRoundMode.NEAREST).ToString();

			UpDateComboBox308(true);
		}

		private void numericUpDown302_ValueChanged(object sender, EventArgs e)
		{
			transitions.PlannedTurnAngle = ARANMath.DegToRad((double)numericUpDown302.Value);

			textBox301.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(transitions.Distance, eRoundMode.NEAREST).ToString();
			textBox301.Tag = textBox301.Text;

			textBox305.Text = GlobalVars.unitConverter.HeightToDisplayUnits(transitions.ConstructAltitude, eRoundMode.NEAREST).ToString();
			textBox305.Tag = textBox305.Text;

			textBox304.Text = GlobalVars.unitConverter.GradientToDisplayUnits(transitions.ConstructGradient, eRoundMode.NEAREST).ToString();

			UpDateComboBox308();
		}

		private void comboBox303_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!radioButton304.Checked)
				return;

			int k = comboBox303.SelectedIndex;
			if (k < 0)
				return;

			transitions.DistanceIndex = k;
			textBox302.Text = transitions.FIXName;

			WayPoint sigPoint = (WayPoint)comboBox303.SelectedItem;
			label305.Text = sigPoint.Role.ToString();

			if (radioButton303.Checked)
				return;

			double dist;

			if (transitions.PathAndTermination == CodeSegmentPath.DF)
			{
				double fTurnDirection = (int)(_AddedFIX.EffectiveTurnDirection);
				double r = _AddedFIX.NomLineTurnRadius;
				Point ptCenter = ARANFunctions.LocalToPrj(_AddedFIX.NomLinePrjPt, _AddedFIX.EntryDirection, 0, r * fTurnDirection);
				Point ptFrom = ARANFunctions.LocalToPrj(ptCenter, _AddedFIX.OutDirection, 0.0, -r * fTurnDirection);

				//GlobalVars.gAranGraphics.DrawPointWithText(ptCenter, "ptCenter");
				//GlobalVars.gAranGraphics.DrawPointWithText(ptFrom, "ptFrom");
				//LegBase.ProcessMessages();

				dist = ARANMath.Hypot(sigPoint.PrjPt.Y - ptFrom.Y, sigPoint.PrjPt.X - ptFrom.X);
			}
			else
				dist = ARANMath.Hypot(sigPoint.PrjPt.Y - _AddedFIX.NomLinePrjPt.Y, sigPoint.PrjPt.X - _AddedFIX.NomLinePrjPt.X);

			bool isSame = false;
			if (radioButton302.Checked && radioButton304.Checked && _AddedFIX.FlyMode == eFlyMode.Atheight && transitions.PathAndTermination == CodeSegmentPath.CF)
			{
				WayPoint dirPoint = (WayPoint)comboBox301.SelectedItem;
				isSame = sigPoint.Id == dirPoint.Id;
			}

			if (isSame)
			{
				numericUpDown301.Enabled = true;
				numericUpDown301_ValueChanged(numericUpDown301, null);
			}
			else
				numericUpDown301.Enabled = radioButton301.Checked;

			transitions.Distance = dist;

			textBox305.Text = GlobalVars.unitConverter.HeightToDisplayUnits(transitions.ConstructAltitude, eRoundMode.NEAREST).ToString();
			textBox305.Tag = textBox305.Text;

			textBox301.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(transitions.Distance, eRoundMode.NEAREST).ToString();
			textBox301.Tag = textBox301.Text;

			UpDateComboBox308(true);
		}

		#endregion

		private void textBox302_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (textBox302.ReadOnly)
				return;

			char eventChar = e.KeyChar;
			if (eventChar == 13)
			{
				textBox302_Validating(textBox302, new System.ComponentModel.CancelEventArgs());
				e.Handled = true;
			}
		}

		private void textBox302_Validating(object sender, CancelEventArgs e)
		{
			if (textBox302.ReadOnly)
				return;

			textBox302.Text = textBox302.Text.ToUpper();
			transitions.FIXName = textBox302.Text;
			textBox302.Tag = textBox302.Text;
		}

		private void textBox310_KeyPress(object sender, KeyPressEventArgs e)
		{
			//char eventChar = e.KeyChar;
			//if (eventChar == 13)
			//	textBox310_Validating(textBox310, new System.ComponentModel.CancelEventArgs());
			//else
			//	Functions.TextBoxFloat(ref eventChar, textBox310.Text);

			//e.KeyChar = eventChar;
			//if (eventChar == 0)
			//	e.Handled = true;
		}

		private void textBox310_Validating(object sender, CancelEventArgs e)
		{
			//double fTmp;

			//if (!double.TryParse(textBox310.Text, out fTmp))
			//	return;

			//if (textBox310.Tag != null && textBox310.Tag.ToString() == textBox310.Text)
			//	return;
		}

		private void textBox303_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox303_Validating(textBox303, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox303.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox303_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(textBox303.Text, out fTmp))
				return;

			if (textBox303.Tag != null && textBox303.Tag.ToString() == textBox303.Text)
				return;

			double fIAS = GlobalVars.unitConverter.SpeedToInternalUnits(fTmp);

			transitions.IAS = fIAS;

			if (transitions.IAS != fIAS)
				textBox303.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(transitions.IAS, eRoundMode.NEAREST).ToString();

			double TAS = ARANMath.IASToTAS(transitions.IAS, transitions.ConstructAltitude, GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value);
			textBox306.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(TAS, eRoundMode.NEAREST).ToString();

			TAS = ARANMath.IASToTAS(transitions.IAS, transitions.NomLineAltitude, GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value);
			textBox309.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(TAS, eRoundMode.NEAREST).ToString();

			textBox303.Tag = textBox303.Text;
		}

		private void textBox304_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox304_Validating(textBox304, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox304.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox304_Validating(object sender, CancelEventArgs e)
		{
			if (!double.TryParse(textBox304.Text, out double fTmp))
				return;

			if (textBox304.Tag != null && textBox304.Tag.ToString() == textBox304.Text)
				return;

			double _PDG = GlobalVars.unitConverter.GradientToInternalUnits(fTmp);
			transitions.ConstructGradient = _PDG;

			if (transitions.ConstructGradient != _PDG)
				textBox304.Text = GlobalVars.unitConverter.GradientToDisplayUnits(transitions.ConstructGradient, eRoundMode.NEAREST).ToString();

			textBox304.Tag = textBox304.Text;

			double TAS = ARANMath.IASToTAS(transitions.IAS, transitions.ConstructAltitude, GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value);
			textBox306.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(TAS, eRoundMode.SPECIAL_NEAREST).ToString();
		}

		private void textBox305_KeyPress(object sender, KeyPressEventArgs e)
		{
			//char eventChar = e.KeyChar;
			//if (eventChar == 13)
			//	textBox305_Validating(textBox305, new System.ComponentModel.CancelEventArgs());
			//else
			//	Functions.TextBoxFloat(ref eventChar, textBox305.Text);

			//e.KeyChar = eventChar;
			//if (eventChar == 0)
			//	e.Handled = true;
		}

		private void textBox305_Validating(object sender, CancelEventArgs e)
		{
			//double fTmp;

			//if (!double.TryParse(textBox305.Text, out fTmp))
			//	return;

			//if (textBox305.Tag != null && textBox305.Tag.ToString() == textBox305.Text)
			//	return;

			//double _Altitude = GlobalVars.unitConverter.HeightToInternalUnits(fTmp);
			//transitions.ConstructAltitude = _Altitude;

			////if (transitions.Altitude != _Altitude)
			//textBox305.Text = GlobalVars.unitConverter.HeightToDisplayUnits(transitions.ConstructAltitude, eRoundMode.NEAREST).ToString();
			//textBox305.Tag = textBox305.Text;

			//double TAS = ARANMath.IASToTAS(transitions.IAS, transitions.ConstructAltitude, GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value);
			//textBox306.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(TAS, eRoundMode.SPECIAL_NEAREST).ToString();

			//textBox304.Text = GlobalVars.unitConverter.GradientToDisplayUnits(transitions.ConstructGradient, eRoundMode.NEAREST).ToString();
		}

		private void textBox307_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox307_Validating(textBox307, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox307.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox307_Validating(object sender, CancelEventArgs e)
		{
			if (!double.TryParse(textBox307.Text, out double fTmp))
				return;

			if (textBox307.Tag != null && textBox307.Tag.ToString() == textBox307.Text)
				return;

			double _PDG = GlobalVars.unitConverter.GradientToInternalUnits(fTmp);
			transitions.NomLineGradient = _PDG;

			if (transitions.NomLineGradient != _PDG)
				textBox307.Text = GlobalVars.unitConverter.GradientToDisplayUnits(transitions.NomLineGradient, eRoundMode.NEAREST).ToString();
			textBox307.Tag = textBox307.Text;

			textBox308.Text = GlobalVars.unitConverter.HeightToDisplayUnits(transitions.NomLineAltitude, eRoundMode.NEAREST).ToString();
			textBox308.Tag = textBox308.Text;

			double TAS = ARANMath.IASToTAS(transitions.IAS, transitions.NomLineAltitude, GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value);
			textBox309.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(TAS, eRoundMode.NEAREST).ToString();

			UpDateComboBox308(true);
		}

		private void textBox308_KeyPress(object sender, KeyPressEventArgs e)
		{
			//char eventChar = e.KeyChar;
			//if (eventChar == 13)
			//	textBox308_Validating(textBox308, new System.ComponentModel.CancelEventArgs());
			//else
			//	Functions.TextBoxFloat(ref eventChar, textBox308.Text);

			//e.KeyChar = eventChar;
			//if (eventChar == 0)
			//	e.Handled = true;
		}

		private void textBox308_Validating(object sender, CancelEventArgs e)
		{
			//return;
		}

		private void textBox3077_KeyPress(object sender, KeyPressEventArgs e)
		{
			//char eventChar = e.KeyChar;
			//if (eventChar == 13)
			//	textBox3077_Validating(textBox3777, new System.ComponentModel.CancelEventArgs());
			//else
			//	Functions.TextBoxFloat(ref eventChar, textBox3777.Text);

			//e.KeyChar = eventChar;
			//if (eventChar == 0)
			//	e.Handled = true;
		}

		private void textBox3077_Validating(object sender, CancelEventArgs e)
		{
			//double fTmp;

			//if (!double.TryParse(textBox3777.Text, out fTmp))
			//	return;

			//if (textBox3777.Tag != null && textBox3777.Tag.ToString() == textBox3777.Text)
			//	return;

			//double bankAngle = fTmp;

			//if (bankAngle < GlobalVars.MinBankAngle)
			//	bankAngle = GlobalVars.MinBankAngle;

			//if (bankAngle > GlobalVars.MaxBankAngle)
			//	bankAngle = GlobalVars.MaxBankAngle;

			//if (fTmp != bankAngle)
			//	textBox3777.Text = bankAngle.ToString();

			//transitions.BankAngle = ARANMath.DegToRad(bankAngle);

			//textBox3777.Tag = textBox3777.Text;
		}

		private void comboBox308_SelectedIndexChanged(object sender, EventArgs e)
		{
			int k = comboBox308.SelectedIndex;
			if (k < 0)
				return;

			if (!bFormInitialised)
				return;

			if (transitions == null)
				return;

			if (transitions.BankAngle >= ARANMath.DegToRad(banks[k]))
				return;

			transitions.BankAngle = ARANMath.DegToRad(banks[k]);
		}

		private void button302_Click(object sender, EventArgs e)
		{
			screenCapture.Save(this);

			ReportsFrm.AddPage4(transitions.CurrentObstacleList, transitions.CurrentDetObs);

			transitions.UpdateEnabled = false;
			double prevDuration = transitions.Legs[transitions.Legs.Count - 1].Duration;

			if (!transitions.Add())
			{
				transitions.UpdateEnabled = true;
				return;
			}

			Report = true;

			LegDep leg = transitions.Legs[transitions.Legs.Count - 1];
			WayPoint EndFIX = (WayPoint)leg.EndFIX;
			WayPoint StartFIX = (WayPoint)leg.StartFIX;

			leg.Duration = prevDuration + leg.NominalTrack.Length / leg.StartFIX.NomLineTAS;
			textBox310.Text = (leg.Duration / 60.0).ToString("0.00");

			UpDown301Range.Max = ARANFunctions.DirToAzimuth(transitions.CourseOutConvertionPoint, transitions.OutDirRange.Min, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
			UpDown301Range.Min = ARANFunctions.DirToAzimuth(transitions.CourseOutConvertionPoint, transitions.OutDirRange.Max, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

			if (transitions.LegPoints.Count == 2)
			{
				comboBox306.Items.Clear();
				foreach (ePBNClass pbn in regularTurnPBNClass)
					comboBox306.Items.Add(pbn);
				comboBox306.SelectedIndex = 0;
			}

			comboBox307.Items.Clear();
			if (EndFIX.FlyMode == eFlyMode.Flyby)
				foreach (CodeSegmentPath pt in FlyByPathAndTerminations)
					comboBox307.Items.Add(pt);
			else
				foreach (CodeSegmentPath pt in FlyOverPathAndTerminations)
					comboBox307.Items.Add(pt);

			comboBox307.SelectedIndex = 0;

			textBox302.Text = transitions.FIXName;
			transitions.FlyMode = (eFlyMode)comboBox305.SelectedIndex;
			transitions.SensorType = (eSensorType)comboBox304.SelectedIndex;
			transitions.PBNClass = (ePBNClass)comboBox306.SelectedItem;
			transitions.PathAndTermination = (CodeSegmentPath)comboBox307.SelectedItem;
			if (radioButton302.Checked)
				comboBox301_SelectedIndexChanged(comboBox301, new EventArgs());

			if (radioButton304.Checked)
				comboBox303_SelectedIndexChanged(comboBox303, new EventArgs());

			button303.Enabled = true;
			Report = true;

			textBox304.Text = GlobalVars.unitConverter.GradientToDisplayUnits(transitions.ConstructGradient, eRoundMode.NEAREST).ToString();
			textBox307.Text = GlobalVars.unitConverter.GradientToDisplayUnits(transitions.NomLineGradient, eRoundMode.NEAREST).ToString();

			transitions.UpdateEnabled = true;
		}

		private void button303_Click(object sender, EventArgs e)
		{
			transitions.UpdateEnabled = false;
			if (!transitions.Remove())
			{
				transitions.UpdateEnabled = true;
				return;
			}

			screenCapture.Delete();

			ReportsFrm.RemoveLastLegP4();

			UpDown301Range.Max = ARANFunctions.DirToAzimuth(transitions.CourseOutConvertionPoint, transitions.OutDirRange.Min, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
			UpDown301Range.Min = ARANFunctions.DirToAzimuth(transitions.CourseOutConvertionPoint, transitions.OutDirRange.Max, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

			_AddedFIX = transitions.LegPoints[transitions.LegPoints.Count - 1];
			int selectedInex, i;

			if (transitions.LegPoints.Count == 1)
			{
				selectedInex = i = 0;
				comboBox306.Items.Clear();

				foreach (ePBNClass pbn in firstTurnPBNClass)
				{
					comboBox306.Items.Add(pbn);
					if (transitions.PBNClass == pbn)
						selectedInex = i;

					i++;
				}

				comboBox306.SelectedIndex = selectedInex;

				button303.Enabled = false;
			}

			selectedInex = i = 0;
			comboBox307.Items.Clear();
			if (_AddedFIX.FlyMode == eFlyMode.Flyby)
				foreach (CodeSegmentPath pt in FlyByPathAndTerminations)
				{
					comboBox307.Items.Add(pt);
					if (transitions.PathAndTermination == pt)
						selectedInex = i;
					i++;
				}
			else
				foreach (CodeSegmentPath pt in FlyOverPathAndTerminations)
				{
					comboBox307.Items.Add(pt);
					if (transitions.PathAndTermination == pt)
						selectedInex = i;
					i++;
				}

			comboBox307.SelectedIndex = selectedInex;

			UpDown301Range.Max = ARANFunctions.DirToAzimuth(transitions.CourseOutConvertionPoint, transitions.OutDirRange.Min, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
			UpDown301Range.Min = ARANFunctions.DirToAzimuth(transitions.CourseOutConvertionPoint, transitions.OutDirRange.Max, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

			double nnAzt = ARANFunctions.DirToAzimuth(transitions.CourseOutConvertionPoint, transitions.OutDirection, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
			numericUpDown301.Value = (decimal)nnAzt;

			comboBox305.SelectedIndex = (int)transitions.FlyMode;
			comboBox304.SelectedIndex = (int)transitions.SensorType;
			comboBox306.SelectedItem = (int)transitions.PBNClass;
			comboBox307.SelectedItem = (int)transitions.PathAndTermination;

			transitions.UpdateEnabled = true;

			textBox310.Text = (transitions.Legs[transitions.Legs.Count - 1].Duration / 60.0).ToString("0.00");

			textBox301.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(transitions.Distance, eRoundMode.NEAREST).ToString();
			double TAS = ARANMath.IASToTAS(transitions.IAS, transitions.ConstructAltitude, GlobalVars.constants.Pansops[ePANSOPSData.ISA].Value);
			textBox306.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(TAS, eRoundMode.SPECIAL_NEAREST).ToString();

			textBox305.Text = GlobalVars.unitConverter.HeightToDisplayUnits(transitions.ConstructAltitude, eRoundMode.NEAREST).ToString();

			textBox304.Text = GlobalVars.unitConverter.GradientToDisplayUnits(transitions.ConstructGradient, eRoundMode.NEAREST).ToString();
			textBox302.Text = transitions.FIXName;

			UpDateComboBox308(true);
		}

		private void checkBox301_CheckedChanged(object sender, EventArgs e)
		{
			transitions.MultiCoverage = checkBox301.Checked;
		}

		private void comboBox309_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (transitions == null)
				return;
			transitions.MOCLimit = EnrouteMOCValues[comboBox309.SelectedIndex];
			ReportsFrm.FillPage3(transitions.CurrentObstacleList, transitions.CurrentDetObs);
			Report = true;
		}

		#endregion

		#region Page V

		ObstacleContainer DetObstacle;
		double XReturnToNomPDG;
		double MinPDG;
		double AppliedhReturn;

		void AddvanceToPageV()
		{
			ReportsFrm.AddPage4(transitions.CurrentObstacleList, transitions.CurrentDetObs);
			transitions.Add(true);

			_RecalceAppliedPDG = _appliedPDG;
			GeometryOperators go = new GeometryOperators();
			int i, j, k = -1, n = transitions.Legs.Count;

			double NomPDG = GlobalVars.constants.Pansops[ePANSOPSData.dpPDG_Nom].Value;

			LegDep leg = transitions.Legs[0];

			DetObstacle.Obstacles = new Obstacle[1];
			DetObstacle.Parts = new ObstacleData[1];

			DetObstacle.Parts[0].PDG = leg.Gradient;
			double legPDG = NomPDG;

			leg.Obstacles = segment1Term.InnerObstacleList;

			if (leg.EndFIX.FlyMode == eFlyMode.Atheight)
				leg.MinLegLength = ARANFunctions.ReturnDistanceInMeters(leg.StartFIX.PrjPt, leg.EndFIX.PrjPt);
			else
			{
				Point ptTmp = ARANFunctions.LocalToPrj(leg.EndFIX.PrjPt, leg.EndFIX.EntryDirection, -leg.EndFIX.EPT, 0);
				leg.MinLegLength = ARANFunctions.ReturnDistanceInMeters(leg.StartFIX.PrjPt, ptTmp);
			}

			for (j = 0; j < transitions.Legs[0].Obstacles.Parts.Length; j++)
			{
				if (transitions.Legs[0].Obstacles.Parts[j].Ignored)
					continue;

				double tmpPDG = transitions.Legs[0].Obstacles.Parts[j].PDG;
				if (tmpPDG > legPDG)
				{
					legPDG = tmpPDG;
					k = j;
				}
			}

			if (_RecalceAppliedPDG < legPDG)
				_RecalceAppliedPDG = legPDG;

			if (k >= 0)
			{
				transitions.Legs[0].DetObstacle_1.Parts[0] = transitions.Legs[0].Obstacles.Parts[k];
				transitions.Legs[0].DetObstacle_1.Obstacles[0] = transitions.Legs[0].Obstacles.Obstacles[transitions.Legs[0].Obstacles.Parts[k].Owner];

				DetObstacle = transitions.Legs[0].DetObstacle_1;
			}

			if (legPDG > transitions.Legs[0].Gradient)
				transitions.Legs[0].Gradient = legPDG;
			else
			{
				DetObstacle.Obstacles[0].ID = -1;
				DetObstacle.Parts[0].PDG = leg.Gradient;
			}

			for (i = 1; i < n; i++)
			{
				k = -1;
				legPDG = NomPDG;
				transitions.Legs[i].Obstacles = ReportsFrm.ObstaclesP4[i];
				leg = transitions.Legs[i];

				if (i == transitions.Legs.Count - 1)
					leg.MinLegLength = go.GetDistance(leg.KKLine, leg.EndFIX.PrjPt) + leg.EndFIX.ATT;
				else
					leg.MinLegLength = go.GetDistance(transitions.Legs[i + 1].KKLine, leg.KKLine);

				for (j = 0; j < transitions.Legs[i].Obstacles.Parts.Length; j++)
				{
					double tmpPDG = transitions.Legs[i].Obstacles.Parts[j].PDG;
					if (tmpPDG > legPDG)
					{
						legPDG = tmpPDG;
						k = j;
					}
				}

				if (_RecalceAppliedPDG < legPDG)
					_RecalceAppliedPDG = legPDG;

				transitions.Legs[i].Gradient = legPDG;
				if (k >= 0)
				{
					transitions.Legs[i].DetObstacle_1.Parts[0] = transitions.Legs[i].Obstacles.Parts[k];
					transitions.Legs[i].DetObstacle_1.Obstacles[0] = transitions.Legs[i].Obstacles.Obstacles[transitions.Legs[i].Obstacles.Parts[k].Owner];

					if (DetObstacle.Parts[0].PDG < transitions.Legs[i].DetObstacle_1.Parts[0].PDG)
					{
						DetObstacle.Parts[0] = transitions.Legs[i].DetObstacle_1.Parts[0];
						DetObstacle.Obstacles[0] = transitions.Legs[i].DetObstacle_1.Obstacles[0];
					}
				}
			}

			/*==============================================================================================================================*/
			MinPDG = _RecalceAppliedPDG;
			textBox401.Text = GlobalVars.unitConverter.GradientToDisplayUnits(MinPDG, eRoundMode.CEIL).ToString();

			double hObovDer = GlobalVars.constants.Pansops[ePANSOPSData.dpH_abv_DER].Value;
			double hReturn;

			if (terminationType == TerminationType.AtHeight)
			{
				hReturn = transitions.Legs[0].MinLegLength * MinPDG + ptDerPrj.Z + hObovDer;        // HeightAtEndFIX + ptDerPrj.Z;	// Math.Max(hReturn , HeightAtEndFIX);
				XReturnToNomPDG = transitions.Legs[0].MinLegLength;
			}
			else
			{
				hReturn = ptDerPrj.Z + GlobalVars.constants.Pansops[ePANSOPSData.dpGui_Ar1].Value;
				XReturnToNomPDG = (hReturn - ptDerPrj.Z - hObovDer) / MinPDG;
			}

			//AppliedhReturn = hReturn;
			//AppliedXReturnToNomPDG = XReturnToNomPDG;
			//if (_RecalceAppliedPDG > _appliedPDG)
			//ObstacleData tmpObst = DetObstacle.Parts[0];

			double AppliedXReturnToNomPDG = XReturnToNomPDG;

			if (MinPDG != NomPDG)
			{
				for (i = 0; i < n; i++)
				{
					leg = transitions.Legs[i];
					int m = leg.Obstacles.Parts.Length;

					for (j = 0; j < m; j++)
					{
						ObstacleData CurrObst = transitions.Legs[i].Obstacles.Parts[j];

						double tmpXReturnToNomPDG = (CurrObst.ReqH - hObovDer - CurrObst.DistStar * NomPDG) / (MinPDG - NomPDG);    /*-ptDerPrj.Z*/

						if (tmpXReturnToNomPDG > XReturnToNomPDG)
							XReturnToNomPDG = tmpXReturnToNomPDG;
					}
				}

				AppliedXReturnToNomPDG = XReturnToNomPDG;
				AppliedhReturn = AppliedXReturnToNomPDG * MinPDG + hObovDer + ptDerPrj.Z;
				textBox402.Text = GlobalVars.unitConverter.HeightToDisplayUnits(AppliedhReturn, eRoundMode.NEAREST).ToString();
			}
			else
			{
				AppliedhReturn = AppliedXReturnToNomPDG * MinPDG + hObovDer + ptDerPrj.Z;
				textBox402.Text = "-";
			}

			/*==============================================================================================================================*/

			//double kkSum = 0;
			double nomSum = 0;
			bool trigger = false, trigger0 = false;

			dataGridView401.RowCount = 0;
			ReportsFrm.ClearPage5Obstacles();

			for (i = 0; i < n; i++)
			{
				leg = transitions.Legs[i];

				int m = leg.Obstacles.Parts.Length;

				for (j = 0; j < m; j++)
				{
					ObstacleData CurrObst = transitions.Legs[i].Obstacles.Parts[j];

					if (CurrObst.DistStar < AppliedXReturnToNomPDG)
						leg.Obstacles.Parts[j].EffectiveHeight = CurrObst.DistStar * MinPDG + hObovDer;
					else
						leg.Obstacles.Parts[j].EffectiveHeight = AppliedXReturnToNomPDG * MinPDG + (CurrObst.DistStar - AppliedXReturnToNomPDG) * NomPDG + hObovDer;
				}

				ReportsFrm.AddPage5(leg.Obstacles, leg.DetObstacle_1);

				DataGridViewRow row = new DataGridViewRow();

				row.Tag = transitions.Legs[i];
				row.ReadOnly = false;

				//kkSum += leg.MinLegLength;
				//double trDist = leg.Length;
				double trDist;
				double Re = leg.EndFIX.NomLineTurnRadius;
				double alpe = 0.5 * leg.EndFIX.TurnAngle;

				if (leg.EndFIX.IsDFTarget || leg.StartFIX.FlyMode == eFlyMode.Atheight)
					trDist = leg.Length + Re * alpe;
				else
				{
					double Rs = leg.StartFIX.NomLineTurnRadius;
					double alps = 0.5 * leg.StartFIX.TurnAngle;

					trDist = leg.Length - Rs * Math.Tan(alps) + Rs * alps;

					if (leg.EndFIX.FlyMode == eFlyMode.Flyby)
						trDist += Re * alpe - Re * Math.Tan(alpe);
				}

				nomSum += trDist;

				double altWPT = nomSum * MinPDG + hObovDer + ptDerPrj.Z;

				DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
				cell.Value = (i + 1).ToString();
				row.Cells.Add(cell);

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[1].Value = leg.EndFIX.Name;

				cell = new DataGridViewTextBoxCell();
				cell.Value = leg.PathAndTermination;
				row.Cells.Add(cell);

				//==================================================//

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[3].Value = leg.EndFIX.FlyMode;

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[4].Value = leg.EndFIX.PBNType;

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[5].Value = GlobalVars.unitConverter.SpeedToDisplayUnits(leg.EndFIX.IAS, eRoundMode.SPECIAL_NEAREST);

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[6].Value = leg.EndFIX.TurnDirection == TurnDirection.CW ? "Right" : "Left";

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[7].Value = Math.Round(GlobalVars.CurrADHP.MagVar, 2).ToString() + "°";

				row.Cells.Add(new DataGridViewTextBoxCell());
				double Val = ARANFunctions.DirToAzimuth(leg.EndFIX.PrjPt, leg.EndFIX.EntryDirection, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);
				double Val1 = ARANMath.Modulus(Val - GlobalVars.CurrADHP.MagVar);
				row.Cells[8].Value = Math.Round(Val, 2).ToString() + "° (" + Math.Round(Val1, 2).ToString() + "°)";

				row.Cells.Add(new DataGridViewTextBoxCell());

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[10].Value = GlobalVars.unitConverter.HeightToDisplayUnits(leg.EndFIX.ConstructAltitude, eRoundMode.NEAREST);

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[11].Value = GlobalVars.unitConverter.DistanceToDisplayUnits(leg.MinLegLength, eRoundMode.NEAREST);

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells[12].Value = GlobalVars.unitConverter.DistanceToDisplayUnits(trDist, eRoundMode.NEAREST);

				//==================================================//
				row.Cells.Add(new DataGridViewTextBoxCell());

				row.Cells.Add(new DataGridViewTextBoxCell());

				//double dDist = leg.Length - leg.MinLegLength;   // -leg.EndFIX.EPT;

				if (altWPT <= AppliedhReturn)
				{
					leg.Altitude = altWPT;
					leg.Gradient = MinPDG;

					row.Cells[9].Value = GlobalVars.unitConverter.HeightToDisplayUnits(altWPT, eRoundMode.NEAREST);

					row.Cells[13].Value = GlobalVars.unitConverter.GradientToDisplayUnits(MinPDG, eRoundMode.SPECIAL_NEAREST);
					row.Cells[14].Value = Math.Round(ARANMath.RadToDeg(Math.Atan(MinPDG)), 2);
				}
				else if (!trigger)
				{
					altWPT = AppliedXReturnToNomPDG * MinPDG + hObovDer + ptDerPrj.Z + (nomSum - AppliedXReturnToNomPDG) * NomPDG;

					leg.Altitude = altWPT;
					leg.Gradient = NomPDG;

					row.Cells[9].Value = GlobalVars.unitConverter.HeightToDisplayUnits(altWPT, eRoundMode.NEAREST);

					row.Cells[13].Value = GlobalVars.unitConverter.GradientToDisplayUnits(MinPDG, eRoundMode.SPECIAL_NEAREST) + "/" + GlobalVars.unitConverter.GradientToDisplayUnits(NomPDG, eRoundMode.SPECIAL_NEAREST);
					row.Cells[14].Value = Math.Round(ARANMath.RadToDeg(Math.Atan(MinPDG)), 2) + "/" + Math.Round(ARANMath.RadToDeg(Math.Atan(NomPDG)), 2);
					trigger = true;
				}
				else
				{
					altWPT = AppliedXReturnToNomPDG * MinPDG + hObovDer + ptDerPrj.Z + (nomSum - AppliedXReturnToNomPDG) * NomPDG;

					if (altWPT <= GlobalVars.MaxAltitude)
					{
						leg.Altitude = altWPT;
						leg.Gradient = NomPDG;

						row.Cells[9].Value = GlobalVars.unitConverter.HeightToDisplayUnits(altWPT, eRoundMode.NEAREST);

						row.Cells[13].Value = GlobalVars.unitConverter.GradientToDisplayUnits(NomPDG, eRoundMode.SPECIAL_NEAREST);
						row.Cells[14].Value = Math.Round(ARANMath.RadToDeg(Math.Atan(NomPDG)), 2);
					}
					else if (!trigger0)
					{
						altWPT = GlobalVars.MaxAltitude;

						leg.Altitude = altWPT;
						leg.Gradient = 0.0;

						row.Cells[9].Value = GlobalVars.unitConverter.HeightToDisplayUnits(GlobalVars.MaxAltitude, eRoundMode.NEAREST);

						row.Cells[13].Value = GlobalVars.unitConverter.GradientToDisplayUnits(NomPDG, eRoundMode.SPECIAL_NEAREST) + "/ 0.0";
						row.Cells[14].Value = Math.Round(ARANMath.RadToDeg(Math.Atan(NomPDG)), 2) + "/ 0.0";
						trigger0 = true;
					}
					else
					{
						altWPT = GlobalVars.MaxAltitude;

						leg.Altitude = altWPT;
						leg.Gradient = 0.0;

						row.Cells[9].Value = GlobalVars.unitConverter.HeightToDisplayUnits(GlobalVars.MaxAltitude, eRoundMode.NEAREST);

						row.Cells[13].Value = "0.0";
						row.Cells[14].Value = "0.0";
					}
				}

				leg.Altitude = altWPT;
				dataGridView401.Rows.Add(row);
			}
		}

		int rowindex = -1;

		private void dataGridView401_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			rowindex = e.RowIndex;
			textBox403.Text = dataGridView401.Rows[e.RowIndex].Cells[9].Value.ToString();
		}

		private void textBox403_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			if (eventChar == 13)
				textBox403_Validating(textBox403, new System.ComponentModel.CancelEventArgs());
			else
				Functions.TextBoxFloat(ref eventChar, textBox403.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void textBox403_Validating(object sender, CancelEventArgs e)
		{
			double fTmp;

			if (!double.TryParse(textBox403.Text, out fTmp))
			{
				if (double.TryParse(textBox403.Tag.ToString(), out fTmp))
					textBox403.Text = textBox403.Tag.ToString();
				return;
			}

			fTmp = GlobalVars.unitConverter.HeightToInternalUnits(fTmp);

			if (fTmp > GlobalVars.MaxAltitude)
			{
				fTmp = GlobalVars.MaxAltitude;
				textBox403.Text = GlobalVars.unitConverter.HeightToDisplayUnits(fTmp, eRoundMode.NEAREST).ToString();
			}

			double NomPDG = GlobalVars.constants.Pansops[ePANSOPSData.dpPDG_Nom].Value;
			double MaxPDG = 2.0 * GlobalVars.constants.Pansops[ePANSOPSData.dpMaxPosPDG].Value;

			double hObovDer = GlobalVars.constants.Pansops[ePANSOPSData.dpH_abv_DER].Value;
			double nomSum = 0, H_min, H_max;

			int i;

			for (i = 0; i <= rowindex; i++)
			{
				LegDep leg = (LegDep)dataGridView401.Rows[i].Tag;

				double trDist;
				double Re = leg.EndFIX.NomLineTurnRadius;
				double alpe = 0.5 * leg.EndFIX.TurnAngle;

				if (leg.EndFIX.IsDFTarget || leg.StartFIX.FlyMode == eFlyMode.Atheight)
					trDist = leg.Length + Re * alpe;
				else
				{
					double Rs = leg.StartFIX.NomLineTurnRadius;
					double alps = 0.5 * leg.StartFIX.TurnAngle;

					trDist = leg.Length - Rs * Math.Tan(alps) + Rs * alps;

					if (leg.EndFIX.FlyMode == eFlyMode.Flyby)
						trDist += Re * alpe - Re * Math.Tan(alpe);
				}

				nomSum += trDist;
			}

			H_max = nomSum * MaxPDG + hObovDer + ptDerPrj.Z;

			if (nomSum <= XReturnToNomPDG)
				H_min = nomSum * MinPDG + hObovDer + ptDerPrj.Z;
			else
				H_min = MinPDG * XReturnToNomPDG + (nomSum - XReturnToNomPDG) * NomPDG + hObovDer + ptDerPrj.Z;

			double newH = fTmp;
			if (fTmp > H_max) fTmp = H_max;
			if (fTmp < H_min) fTmp = H_min;

			if (newH != fTmp)
			{
				newH = fTmp;
				textBox403.Text = GlobalVars.unitConverter.HeightToDisplayUnits(newH, eRoundMode.NEAREST).ToString();
			}

			textBox403.Tag = textBox403.Text;
			int n = transitions.Legs.Count;

			double AppliedPDG = (newH - hObovDer - ptDerPrj.Z) / nomSum;

			/*==================*/
			double AppliedXReturnToNomPDG = nomSum;

			if (AppliedPDG < MinPDG)
			{
				AppliedPDG = MinPDG;

				double X = (newH - nomSum * NomPDG - hObovDer - ptDerPrj.Z) / (AppliedPDG - NomPDG);
				if (X < nomSum)
					AppliedXReturnToNomPDG = X;
			}
			else
				AppliedXReturnToNomPDG = (newH - hObovDer - ptDerPrj.Z) / AppliedPDG;

			if (AppliedXReturnToNomPDG < XReturnToNomPDG)
				AppliedXReturnToNomPDG = XReturnToNomPDG;

			AppliedhReturn = AppliedXReturnToNomPDG * AppliedPDG + hObovDer + ptDerPrj.Z;

			textBox401.Text = GlobalVars.unitConverter.GradientToDisplayUnits(AppliedPDG, eRoundMode.CEIL).ToString();
			textBox402.Text = GlobalVars.unitConverter.HeightToDisplayUnits(AppliedhReturn, eRoundMode.NEAREST).ToString();

			nomSum = 0;
			bool trigger = false, trigger0 = false;
			ReportsFrm.ClearPage5Obstacles();

			for (i = 0; i < n; i++)
			{
				DataGridViewRow row = dataGridView401.Rows[i];
				LegDep leg = (LegDep)row.Tag;

				double trDist;
				double Re = leg.EndFIX.NomLineTurnRadius;
				double alpe = 0.5 * leg.EndFIX.TurnAngle;

				if (leg.EndFIX.IsDFTarget || leg.StartFIX.FlyMode == eFlyMode.Atheight)
					trDist = leg.Length + Re * alpe;
				else
				{
					double Rs = leg.StartFIX.NomLineTurnRadius;
					double alps = 0.5 * leg.StartFIX.TurnAngle;

					trDist = leg.Length - Rs * Math.Tan(alps) + Rs * alps;

					if (leg.EndFIX.FlyMode == eFlyMode.Flyby)
						trDist += Re * alpe - Re * Math.Tan(alpe);
				}

				nomSum += trDist;

				int m = transitions.Legs[i].Obstacles.Parts.Length;

				for (int j = 0; j < m; j++)
				{
					ObstacleData CurrObst = transitions.Legs[i].Obstacles.Parts[j];

					if (CurrObst.DistStar < AppliedXReturnToNomPDG)
						transitions.Legs[i].Obstacles.Parts[j].EffectiveHeight = CurrObst.DistStar * AppliedPDG + hObovDer;
					else
						transitions.Legs[i].Obstacles.Parts[j].EffectiveHeight = AppliedXReturnToNomPDG * AppliedPDG + (CurrObst.DistStar - AppliedXReturnToNomPDG) * NomPDG + hObovDer;
				}

				ReportsFrm.AddPage5(transitions.Legs[i].Obstacles, transitions.Legs[i].DetObstacle_1);

				double altWPT = nomSum * AppliedPDG + hObovDer + ptDerPrj.Z;

				if (altWPT <= AppliedhReturn)
				{
					leg.Altitude = altWPT;
					leg.Gradient = AppliedPDG;


					row.Cells[9].Value = GlobalVars.unitConverter.HeightToDisplayUnits(altWPT, eRoundMode.NEAREST);
					row.Cells[13].Value = GlobalVars.unitConverter.GradientToDisplayUnits(AppliedPDG, eRoundMode.SPECIAL_NEAREST);
					row.Cells[14].Value = Math.Round(ARANMath.RadToDeg(Math.Atan(AppliedPDG)), 2);
				}
				else if (!trigger)
				{
					altWPT = AppliedXReturnToNomPDG * AppliedPDG + hObovDer + ptDerPrj.Z + (nomSum - AppliedXReturnToNomPDG) * NomPDG;

					leg.Altitude = altWPT;
					leg.Gradient = AppliedPDG;// NomPDG;

					row.Cells[9].Value = GlobalVars.unitConverter.HeightToDisplayUnits(altWPT, eRoundMode.NEAREST);

					row.Cells[13].Value = GlobalVars.unitConverter.GradientToDisplayUnits(AppliedPDG, eRoundMode.SPECIAL_NEAREST) + "/" + GlobalVars.unitConverter.GradientToDisplayUnits(NomPDG, eRoundMode.SPECIAL_NEAREST);
					row.Cells[14].Value = Math.Round(ARANMath.RadToDeg(Math.Atan(AppliedPDG)), 2) + "/" + Math.Round(ARANMath.RadToDeg(Math.Atan(NomPDG)), 2);
					trigger = true;
				}
				else
				{
					altWPT = AppliedXReturnToNomPDG * AppliedPDG + hObovDer + ptDerPrj.Z + (nomSum - AppliedXReturnToNomPDG) * NomPDG;

					if (altWPT <= GlobalVars.MaxAltitude)
					{
						leg.Altitude = altWPT;
						leg.Gradient = NomPDG;

						row.Cells[9].Value = GlobalVars.unitConverter.HeightToDisplayUnits(altWPT, eRoundMode.NEAREST);
						row.Cells[13].Value = GlobalVars.unitConverter.GradientToDisplayUnits(NomPDG, eRoundMode.SPECIAL_NEAREST);
						row.Cells[14].Value = Math.Round(ARANMath.RadToDeg(Math.Atan(NomPDG)), 2);
					}
					else if (!trigger0)
					{
						altWPT = GlobalVars.MaxAltitude;

						leg.Altitude = altWPT;
						leg.Gradient = 0.0;

						row.Cells[9].Value = GlobalVars.unitConverter.HeightToDisplayUnits(GlobalVars.MaxAltitude, eRoundMode.NEAREST);
						row.Cells[13].Value = GlobalVars.unitConverter.GradientToDisplayUnits(NomPDG, eRoundMode.SPECIAL_NEAREST) + "/ 0.0";
						row.Cells[14].Value = Math.Round(ARANMath.RadToDeg(Math.Atan(NomPDG)), 2) + "/ 0.0";
						trigger0 = true;
					}
					else
					{
						altWPT = GlobalVars.MaxAltitude;

						leg.Altitude = altWPT;
						leg.Gradient = 0.0;

						row.Cells[9].Value = GlobalVars.unitConverter.HeightToDisplayUnits(GlobalVars.MaxAltitude, eRoundMode.NEAREST);
						row.Cells[13].Value = "0.00";
						row.Cells[14].Value = "0.00";
					}
				}
			}

			if (rowindex >= 0)
				textBox403.Text = dataGridView401.Rows[rowindex].Cells[9].Value.ToString();
		}

		private DepartureLeg CreateDepartureLeg(int num, AircraftCharacteristic IsLimitedTo, ref TerminalSegmentPoint pEndPoint)
		{
			double PriorFixTolerance, PostFixTolerance;

			SegmentLeg pSegmentLeg;
			DepartureLeg pDepartureLeg;
			SegmentPoint pSegmentPoint;

			ValSpeed pSpeed;
			ValDistance pDistance;
			ValDistanceSigned pDistanceSigned;
			ValDistanceVertical pDistanceVertical;

			UomSpeed mUomSpeed;
			UomDistance mUomHDistance;
			UomDistanceVertical mUomVDistance;

			UomSpeed[] uomSpeedTab;
			UomDistance[] uomDistHorTab;
			UomDistanceVertical[] uomDistVerTab;

			uomSpeedTab = new UomSpeed[] { UomSpeed.KM_H, UomSpeed.KT, UomSpeed.M_SEC, UomSpeed.MACH, UomSpeed.M_MIN, UomSpeed.FT_MIN, UomSpeed.FT_SEC };
			uomDistHorTab = new UomDistance[] { UomDistance.KM, UomDistance.NM, UomDistance.M, UomDistance.FT, UomDistance.MI };
			uomDistVerTab = new UomDistanceVertical[] { UomDistanceVertical.M, UomDistanceVertical.FT };

			mUomSpeed = uomSpeedTab[GlobalVars.unitConverter.SpeedUnitIndex];
			mUomVDistance = uomDistVerTab[GlobalVars.unitConverter.HeightUnitIndex];
			mUomHDistance = uomDistHorTab[GlobalVars.unitConverter.DistanceUnitIndex];

			pDepartureLeg = DBModule.pObjectDir.CreateFeature<DepartureLeg>();
			pDepartureLeg.AircraftCategory.Add(IsLimitedTo);
			pSegmentLeg = pDepartureLeg;

			//SegmentLeg
			pSegmentLeg.UpperLimitReference = CodeVerticalReference.MSL;
			pSegmentLeg.LowerLimitReference = CodeVerticalReference.MSL;
			pSegmentLeg.LegPath = CodeTrajectory.STRAIGHT;
			pSegmentLeg.CourseType = CodeCourse.TRUE_TRACK;

			LegDep currLeg = transitions.Legs[num];
			WayPoint StartFix = currLeg.StartFIX;
			WayPoint EndFix = currLeg.EndFIX;

			if (EndFix.IsDFTarget || ARANMath.SubtractAngles(EndFix.EntryDirection, EndFix.OutDirection) < ARANMath.DegToRad(5))
			{
				if (EndFix.TurnDirection == TurnDirection.CW)
					pSegmentLeg.TurnDirection = CodeDirectionTurn.RIGHT;
				else
					pSegmentLeg.TurnDirection = CodeDirectionTurn.LEFT;
			}

			pSegmentLeg.LegTypeARINC = currLeg.PathAndTermination;


			if (EndFix.FlyMode == eFlyMode.Atheight)
			{
				pSegmentLeg.EndConditionDesignator = CodeSegmentTermination.ALTITUDE;
				pSegmentLeg.LegPath = Aran.Aim.Enums.CodeTrajectory.STRAIGHT;
			}
			else if (EndFix.IsDFTarget)
				pSegmentLeg.EndConditionDesignator = CodeSegmentTermination.INTERCEPT;
			else if (num == 0 || StartFix.FlyMode == eFlyMode.Atheight)
				pSegmentLeg.EndConditionDesignator = CodeSegmentTermination.INTERCEPT;
			else
				pSegmentLeg.EndConditionDesignator = CodeSegmentTermination.INTERCEPT;

			pSegmentLeg.ProcedureTurnRequired = false;

			// ====================================================================
			if (!EndFix.IsDFTarget)
				pSegmentLeg.Course = ARANFunctions.DirToAzimuth(EndFix.PrjPt, EndFix.EntryDirection, GlobalVars.pSpRefPrj, GlobalVars.pSpRefGeo);

			// ====================================================================
			if (EndFix.ConstructAltitude < currLeg.Altitude)
				EndFix.ConstructAltitude = currLeg.Altitude;

			pDistanceVertical = new ValDistanceVertical();
			pDistanceVertical.Uom = mUomVDistance;
			pDistanceVertical.Value = GlobalVars.unitConverter.HeightToDisplayUnits(EndFix.ConstructAltitude, eRoundMode.SPECIAL_NEAREST);

			pSegmentLeg.UpperLimitAltitude = pDistanceVertical;
			// ABOVE_LOWER;
			//pSegmentLeg.AltitudeInterpretation = CodeAltitudeUse.BELOW_UPPER;
			// ====================================================================

			pDistanceVertical = new ValDistanceVertical();
			pDistanceVertical.Uom = mUomVDistance;
			pDistanceVertical.Value = GlobalVars.unitConverter.HeightToDisplayUnits(currLeg.Altitude, eRoundMode.SPECIAL_NEAREST);

			pSegmentLeg.LowerLimitAltitude = pDistanceVertical;
			//AT_LOWER;
			pSegmentLeg.AltitudeInterpretation = CodeAltitudeUse.ABOVE_LOWER;
			// ====================================================================

			pDistance = new ValDistance();
			pDistance.Uom = mUomHDistance;

			double trDist = ARANFunctions.ReturnDistanceInMeters(StartFix.PrjPt, EndFix.PrjPt);
			if (EndFix.IsDFTarget || StartFix.FlyMode == eFlyMode.Atheight)
			{
				double Re = EndFix.NomLineTurnRadius;
				double alpe = 0.5 * EndFix.TurnAngle;

				trDist = currLeg.Length + Re * alpe;
			}

			pDistance.Value = GlobalVars.unitConverter.DistanceToDisplayUnits(trDist, eRoundMode.NEAREST);
			pSegmentLeg.Length = pDistance;
			// ====================================================================
			pSegmentLeg.VerticalAngle = ARANMath.RadToDeg(System.Math.Atan(currLeg.Gradient));
			// ====================================================================
			pSpeed = new ValSpeed();
			pSpeed.Uom = mUomSpeed;
			pSpeed.Value = GlobalVars.unitConverter.SpeedToDisplayUnits(currLeg.IAS, eRoundMode.SPECIAL_NEAREST);
			pSegmentLeg.SpeedLimit = pSpeed;
			pSegmentLeg.SpeedReference = CodeSpeedReference.IAS;

			Feature pFixDesignatedPoint;
			SignificantPoint pFIXSignPt;
			PointReference pPointReference;
			Surface pSurface;

			// Start Point ========================================================

			if (pEndPoint == null && StartFix.FlyMode != eFlyMode.Atheight)
			{
				TerminalSegmentPoint pStartPoint = new TerminalSegmentPoint();
				if (StartFix.Role == eFIXRole.TP_)
					pStartPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.TP;
				else if (StartFix.Role == eFIXRole.IAF_GT_56_ || StartFix.Role == eFIXRole.IAF_LE_56_ || StartFix.Role == eFIXRole.PBN_IAF)
					pStartPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.IAF;

				pSegmentPoint = pStartPoint;

				pSegmentPoint.FlyOver = StartFix.FlyMode == eFlyMode.Flyover;
				pSegmentPoint.RadarGuidance = false;

				pSegmentPoint.ReportingATC = CodeATCReporting.NO_REPORT;
				//== ==============================================================

				pFixDesignatedPoint = DBModule.CreateDesignatedPoint(StartFix);//.PrjPt, StartFix.Name, StartFix.OutDirection, StartFix.HorAccuracy);
				pFIXSignPt = new SignificantPoint();

				if (pFixDesignatedPoint is DesignatedPoint)
					pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef();
				else if (pFixDesignatedPoint is Navaid)
					pFIXSignPt.NavaidSystem = pFixDesignatedPoint.GetFeatureRef();
				else if (pFixDesignatedPoint is RunwayCentrelinePoint)
					pFIXSignPt.RunwayPoint = pFixDesignatedPoint.GetFeatureRef();

				if (Functions.PriorPostFixTolerance(StartFix.TolerArea, StartFix.PrjPt, StartFix.EntryDirection, out PriorFixTolerance, out PostFixTolerance))
				{
					pPointReference = new PointReference();

					pDistanceSigned = new ValDistanceSigned();
					pDistanceSigned.Uom = mUomHDistance;
					pDistanceSigned.Value = Math.Abs(GlobalVars.unitConverter.DistanceToDisplayUnits(PriorFixTolerance, eRoundMode.NEAREST));
					pPointReference.PriorFixTolerance = pDistanceSigned;

					pDistanceSigned = new Aran.Aim.DataTypes.ValDistanceSigned();
					pDistanceSigned.Uom = mUomHDistance;
					pDistanceSigned.Value = Math.Abs(GlobalVars.unitConverter.DistanceToDisplayUnits(PostFixTolerance, eRoundMode.NEAREST));
					pPointReference.PostFixTolerance = pDistanceSigned;

					pSurface = new Surface();
					MultiPolygon pl = GlobalVars.pspatialReferenceOperation.ToGeo<MultiPolygon>(StartFix.TolerArea);
					if (!pl.IsEmpty)
					{
						pSurface.Geo.Add(pl);
						pPointReference.FixToleranceArea = pSurface;
					}

					pStartPoint.FacilityMakeup.Add(pPointReference);
				}

				pSegmentPoint.PointChoice = pFIXSignPt;
				pSegmentLeg.StartPoint = pStartPoint;
			}
			else
				pSegmentLeg.StartPoint = pEndPoint;

			//  End Of Start Point ================================================
			//=====================================================================
			//  Start Of End Point ================================================
			pEndPoint = null;
			if (EndFix.FlyMode != eFlyMode.Atheight)
			{
				pEndPoint = new TerminalSegmentPoint();

				if (EndFix.Role == eFIXRole.TP_)
					pEndPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.TP;
				else if (EndFix.Role == eFIXRole.IAF_GT_56_ || EndFix.Role == eFIXRole.IAF_LE_56_ || EndFix.Role == eFIXRole.PBN_IAF)
					pEndPoint.Role = Aran.Aim.Enums.CodeProcedureFixRole.IAF;

				pSegmentPoint = pEndPoint;

				pSegmentPoint.FlyOver = EndFix.FlyMode == eFlyMode.Flyover;
				pSegmentPoint.RadarGuidance = false;
				pSegmentPoint.ReportingATC = Aran.Aim.Enums.CodeATCReporting.NO_REPORT;

				//============================================================

				pFixDesignatedPoint = DBModule.CreateDesignatedPoint(EndFix);//.PrjPt, EndFix.Name, EndFix.EntryDirection);
				pFIXSignPt = new SignificantPoint();

				if (pFixDesignatedPoint is DesignatedPoint)
					pFIXSignPt.FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef();
				else if (pFixDesignatedPoint is Navaid)
					pFIXSignPt.NavaidSystem = pFixDesignatedPoint.GetFeatureRef();
				else if (pFixDesignatedPoint is RunwayCentrelinePoint)
					pFIXSignPt.RunwayPoint = pFixDesignatedPoint.GetFeatureRef();

				//=======================

				if (Functions.PriorPostFixTolerance(EndFix.TolerArea, EndFix.PrjPt, EndFix.EntryDirection, out PriorFixTolerance, out PostFixTolerance))
				{
					pPointReference = new PointReference();

					pDistanceSigned = new ValDistanceSigned();
					pDistanceSigned.Uom = mUomHDistance;
					pDistanceSigned.Value = Math.Abs(GlobalVars.unitConverter.DistanceToDisplayUnits(PriorFixTolerance, eRoundMode.NEAREST));
					pPointReference.PriorFixTolerance = pDistanceSigned;

					pDistanceSigned = new Aran.Aim.DataTypes.ValDistanceSigned();
					pDistanceSigned.Uom = mUomHDistance;
					pDistanceSigned.Value = Math.Abs(GlobalVars.unitConverter.DistanceToDisplayUnits(PostFixTolerance, eRoundMode.NEAREST));
					pPointReference.PostFixTolerance = pDistanceSigned;

					pSurface = new Surface();
					MultiPolygon pl = GlobalVars.pspatialReferenceOperation.ToGeo<MultiPolygon>(EndFix.TolerArea);
					if (!pl.IsEmpty)
					{
						pSurface.Geo.Add(pl);
						pPointReference.FixToleranceArea = pSurface;
					}

					pEndPoint.FacilityMakeup.Add(pPointReference);
				}

				pSegmentPoint.PointChoice = pFIXSignPt;
				pSegmentLeg.EndPoint = pEndPoint;
			}

			// End of EndPoint ========================

			// Trajectory =====================================================

			Curve pCurve = new Curve();

			LineString ls = GlobalVars.pspatialReferenceOperation.ToGeo<LineString>(currLeg.NominalTrack);
			pCurve.Geo.Add(ls);
			pSegmentLeg.Trajectory = pCurve;

			// I protected Area =======================================================
			pSurface = new Surface();
			for (int i = 0; i < currLeg.PrimaryAssesmentArea.Count; i++)
			{
				Polygon pl = GlobalVars.pspatialReferenceOperation.ToGeo<Polygon>(currLeg.PrimaryAssesmentArea[i]);
				if (!pl.IsEmpty)
					pSurface.Geo.Add(pl);
			}

			ObstacleAssessmentArea pPrimProtectedArea = new ObstacleAssessmentArea();

			if (!pSurface.Geo.IsEmpty)
				pPrimProtectedArea.Surface = pSurface;

			pPrimProtectedArea.SectionNumber = 0;
			pPrimProtectedArea.Type = CodeObstacleAssessmentSurface.PRIMARY;
			FillObstacleAssesment(pPrimProtectedArea, currLeg.Obstacles, true);
			pSegmentLeg.DesignSurface.Add(pPrimProtectedArea);

			// II protected Area =======================================================
			GeometryOperators pTopo = new GeometryOperators();
			MultiPolygon pPolygon = (MultiPolygon)pTopo.Difference(currLeg.FullAssesmentArea, currLeg.PrimaryAssesmentArea);

			if (pPolygon != null)
			{
				pSurface = new Surface();

				for (int i = 0; i < pPolygon.Count; i++)
				{
					Polygon pl = GlobalVars.pspatialReferenceOperation.ToGeo<Polygon>(pPolygon[i]);
					if (!pl.IsEmpty)
						pSurface.Geo.Add(pl);
				}

				ObstacleAssessmentArea pSecProtectedArea = new ObstacleAssessmentArea();

				if (!pSurface.Geo.IsEmpty)
					pSecProtectedArea.Surface = pSurface;

				pSecProtectedArea.SectionNumber = 1;
				pSecProtectedArea.Type = CodeObstacleAssessmentSurface.SECONDARY;
				FillObstacleAssesment(pPrimProtectedArea, currLeg.Obstacles, false);

				pSegmentLeg.DesignSurface.Add(pSecProtectedArea);
			}
			//  END =====================================================

			return pDepartureLeg;
		}

		private void FillObstacleAssesment(ObstacleAssessmentArea pPrimProtectedArea, ObstacleContainer obstacles, bool isPrima)
		{
			if (pPrimProtectedArea == null)
				throw new ArgumentNullException();

			if (obstacles.Parts == null)
				return;

			var uomDistVerTab = new UomDistanceVertical[] { UomDistanceVertical.M, UomDistanceVertical.FT };
			var uomDistVer = uomDistVerTab[GlobalVars.unitConverter.DistanceUnitIndex];

			var sortedObstacleList = obstacles.Parts.ToList().
				OrderByDescending(obs => obs.PDG).ToList();

			if (!GlobalVars.settings.AnnexObstalce)
			{
				var closeInObsCount = sortedObstacleList.Count(s => s.PDG >= _RecalceAppliedPDG);
				sortedObstacleList = sortedObstacleList.Take(Math.Max(closeInObsCount, saveFeatureCount)).ToList();
			}

			foreach (var obstacle in sortedObstacleList)
			{
				if (obstacle.Prima != isPrima) return;

				Obstruction obs = new Obstruction
				{
					VerticalStructureObstruction = new FeatureRef(obstacles.Obstacles[obstacle.Owner].Identifier),
					SurfacePenetration = obstacle.PDG >= _RecalceAppliedPDG,
					CloseIn = obstacle.PDG > GlobalVars.constants.Pansops[ePANSOPSData.dpPDG_Nom].Value && obstacle.Ignored
				};

				var minimumAltitude = obstacle.Height + obstacle.MOC;

				var pDistanceVertical = new ValDistanceVertical
				{
					Uom = uomDistVer,
					Value = GlobalVars.unitConverter.HeightToDisplayUnits(minimumAltitude)
				};
				obs.MinimumAltitude = pDistanceVertical;

				//MOC
				var pDistance = new ValDistance
				{
					Uom = GlobalVars.unitConverter.DistanceUnitIndex == 0 ? UomDistance.M : UomDistance.FT,
					Value = GlobalVars.unitConverter.HeightToDisplayUnits(obstacle.MOC)
				};

				obs.RequiredClearance = pDistance;
				pPrimProtectedArea.SignificantObstacle.Add(obs);
			}
		}

		private void HelpBtn_Click(object sender, EventArgs e)
		{
			//
		}

		bool SaveProcedure()
		{
			ProcedureTransition pTransition;
			AircraftCharacteristic IsLimitedTo;
			LandingTakeoffAreaCollection pLandingTakeoffAreaCollection;
			TerminalSegmentPoint pEndPoint = null;
			SegmentLeg pSegmentLeg;
			ProcedureTransitionLeg ptl;

			FeatureRef featureRef;
			FeatureRefObject featureRefObject;
			DBModule.pObjectDir.ClearAllFeatures();

			//  Procedure =================================================================================================
			pLandingTakeoffAreaCollection = new LandingTakeoffAreaCollection();
			pLandingTakeoffAreaCollection.Runway.Add(SelectedRWY.GetFeatureRefObject());

			Procedure = DBModule.pObjectDir.CreateFeature<StandardInstrumentDeparture>();

			Procedure.CodingStandard = CodeProcedureCodingStandard.PANS_OPS;
			Procedure.DesignCriteria = CodeDesignStandard.PANS_OPS;
			Procedure.RNAV = true;
			Procedure.FlightChecked = false;
			Procedure.Takeoff = pLandingTakeoffAreaCollection;

			IsLimitedTo = new AircraftCharacteristic();

			switch (ComboBox103.SelectedIndex)
			{
				case 0:
					IsLimitedTo.AircraftLandingCategory = CodeAircraftCategory.A;
					break;
				case 1:
					IsLimitedTo.AircraftLandingCategory = CodeAircraftCategory.B;
					break;
				case 2:
					IsLimitedTo.AircraftLandingCategory = CodeAircraftCategory.C;
					break;
				case 3:
					IsLimitedTo.AircraftLandingCategory = CodeAircraftCategory.D;
					break;
				case 4:
					IsLimitedTo.AircraftLandingCategory = CodeAircraftCategory.E;
					break;
			}

			Procedure.AircraftCharacteristic.Add(IsLimitedTo);

			featureRefObject = new FeatureRefObject();
			featureRef = new FeatureRef();
			featureRef.Identifier = GlobalVars.CurrADHP.pAirportHeliport.Identifier;
			featureRefObject.Feature = featureRef;
			Procedure.AirportHeliport.Add(featureRefObject);

			// Must be FIX Name sProcName = TurnWPT.Name + " RWY" + DER.Identifier;
			string sProcName = textBox302.Text + " 1A";

			Procedure.Name = sProcName;

			// Transition ==========================================================================
			pTransition = new ProcedureTransition();
			pTransition.DepartureRunwayTransition = pLandingTakeoffAreaCollection;
			pTransition.Type = CodeProcedurePhase.RWY;

			// Legs ======================================================================================================
			uint NO_SEQ = 1;
			for (int i = 0; i < transitions.Legs.Count; i++, NO_SEQ++)
			{
				pSegmentLeg = CreateDepartureLeg(i, IsLimitedTo, ref pEndPoint);

				ptl = new ProcedureTransitionLeg();
				ptl.SeqNumberARINC = NO_SEQ;
				ptl.TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef<AbstractSegmentLegRef>();
				pTransition.TransitionLeg.Add(ptl);
			}

			//=============================================================================
			Procedure.FlightTransition.Add(pTransition);

			try
			{
				DBModule.pObjectDir.SetRootFeatureType(Aim.FeatureType.StandardInstrumentDeparture);

				bool saveRes = DBModule.pObjectDir.Commit(new Aim.FeatureType[]{
						Aim.FeatureType.RunwayCentrelinePoint,
						Aim.FeatureType.DesignatedPoint,
						Aim.FeatureType.AngleIndication,
						Aim.FeatureType.DistanceIndication,
						Aim.FeatureType.StandardInstrumentDeparture,
						Aim.FeatureType.StandardInstrumentArrival,
						Aim.FeatureType.InstrumentApproachProcedure,
						Aim.FeatureType.ArrivalFeederLeg,
						Aim.FeatureType.ArrivalLeg,
						Aim.FeatureType.DepartureLeg,
						Aim.FeatureType.FinalLeg,
						Aim.FeatureType.InitialLeg,
						Aim.FeatureType.IntermediateLeg,
						Aim.FeatureType.MissedApproachLeg});

				GlobalVars.gAranEnv.RefreshAllAimLayers();

				return saveRes;
			}
			catch (Exception ex)
			{
				throw new Exception("Error on commit." + "\r\n" + ex.Message);
			}

			//return false;
		}

		#endregion

	}
}
