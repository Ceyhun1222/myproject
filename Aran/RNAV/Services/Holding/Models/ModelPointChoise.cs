using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Aran.Aim;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.AranEnvironment;
using Aran.AranEnvironment.Symbols;
using Aran.Panda.Rnav.Holding.Helper;
using Aran.Panda.Rnav.Holding.Properties;
using Aran.PANDA.Common;
using Aran.PANDA.Rnav.Holding.Properties;

namespace Holding.Models
{
	public class ModelPointChoise : Changed, INotifyPropertyChanged
	{
		#region :>Fields
		private WayPointChoice _curSignificantType;
		private AirportHeliport _curAdhp;
		private DesignatedPoint _curSigPoint;
		private Navaid _curNavaid;
		private DBModule _dbModule;
		private SpatialReferenceOperation _spatialOperation;
		private int _bigCircleHandle, _littleCircleHandle, _ptHandle;
		private double _applyAtt, _applyXtt;
		private Aran.Geometries.Point _applyPoint;

		private int _changedCount;
		private bool _holdingAreaIsEnabled = true;
		private IAranGraphics _ui;
		//private double distanceOld;
		#endregion

		#region :>Constructor

		public ModelPointChoise(double distance, double minDistance, double maxDistance)
		{
			_ui = GlobalParams.UI;
			_spatialOperation = GlobalParams.SpatialRefOperation;
			_dbModule = GlobalParams.Database;
			_distance = distance;
			_minDistance = minDistance;
			_maxDistance = maxDistance;
			SignificantList = new List<WayPointChoice>
			{
			  new WayPointChoice(SignificantPointChoice.DesignatedPoint,"DesignatedPoint"),
			  new WayPointChoice(SignificantPointChoice.Navaid, "Navaid"),
			  new WayPointChoice(SignificantPointChoice.AixmPoint,"Point")
			};
			_curSignificantType = SignificantList[0];
			CurAdhpChange(InitHolding.CurAdhp);
			_changedCount = 0;
			DesignatedPointListIsActive = true;

		}

		#endregion

		#region :>Property

		#region Lists
		public List<DesignatedPoint> PointList { get; set; }

		public List<Navaid> NavaidList { get; set; }

		public List<WayPointChoice> SignificantList { get; set; }
		#endregion

		#region CurValue


		public DesignatedPoint CurSigPoint
		{
			get { return _curSigPoint; }
			set
			{
				if (Equals(_curSigPoint, value))
					return;
				if (value == null)
				{
					DeletePoint();
					_curPoint = null;
					_curSigPoint = value;

					ChangeCentralMeridian(GeomFunctions.Assign(_curAdhp.ARP));

				}
				else
					CurSigPointChange(value);

				if (CurPointChanged != null)
					CurPointChanged(this, new EventArgs());

				//if (PropertyChanged != null)
				//    PropertyChanged(this, new PropertyChangedEventArgs("CurSigPoint"));
			}
		}

		public Navaid CurNavaid
		{
			get { return _curNavaid; }
			set
			{
				if (Equals(_curNavaid, value))
					return;

				if (value == null)
				{
					DeletePoint();
					_curPoint = null;
					_curNavaid = value;

					// ChangeCentralMeridian(GeomFunctions.Assign(_curAdhp.ARP));
				}
				else
					CurNavaidChange(value);

				if (CurPointChanged != null)
					CurPointChanged(this, new EventArgs());

				//if (PropertyChanged != null)
				//    PropertyChanged(this, new PropertyChangedEventArgs("CurNavaid"));
			}
		}

		private Aran.Geometries.Point _curPoint;

		public Aran.Geometries.Point CurPoint
		{
			get { return _curPoint; }
			set
			{
				if (Equals(_curPoint == value))
					return;
				ChangeModelChanged(_curPoint, value, _applyPoint);
				_curPoint = value;
				ChangeCentralMeridian(_curPoint);

				if (CurPointChanged != null)
					CurPointChanged(this, new EventArgs());
			}
		}

		public WayPointChoice PointChoise
		{
			get { return _curSignificantType; }
			set
			{
				if (Equals(_curSignificantType, value))
					return;

				_curSignificantType = value;

				DeletePoint();
				var circle = FeatureConvert.CreateCircle(_curAdhp.ARP.Geo, _minDistance, _distance);

				_curSigPoint = null;
				_curNavaid = null;

				NavaidList = null;
				PointList = null;

				DesignatedPointListIsActive = false;
				NavaidListIsActive = false;
				PointPickerIsActive = false;

				if (_curSignificantType == null)
					return;

				if (_curSignificantType.Choice == SignificantPointChoice.DesignatedPoint)
				{
					DesignatedPointListIsActive = true;

					PointList = _dbModule.HoldingQpi.GetDesignatedPointList(circle[0]);
					if (PointList != null && PointList.Count > 0)
					{
						PointList = PointList.Where(dPoint => FeatureConvert.CheckPtInLIcenseArea(GeomFunctions.Assign(dPoint.Location)))
							.OrderBy(dpoint => dpoint.Designator)
						   .ToList<DesignatedPoint>();

						if (PointList != null && PointList.Count > 0)
							CurSigPoint = PointList[0];
					}
					else
						CurPoint = null;
				}

				else if (_curSignificantType.Choice == SignificantPointChoice.Navaid)
				{
					NavaidListIsActive = true;

                    var circlePrj = GlobalParams.SpatialRefOperation.ToPrj(circle);

					NavaidList = _dbModule.HoldingQpi.GetNavaidListByTypes(circlePrj[0], GlobalParams.SpatialRefOperation,
						new CodeNavaidService[]
						{
							CodeNavaidService.VOR, Aran.Aim.Enums.CodeNavaidService.VOR_DME,
							CodeNavaidService.NDB, Aran.Aim.Enums.CodeNavaidService.NDB_DME,
							CodeNavaidService.TACAN,
                            CodeNavaidService.ILS_DME,
                            CodeNavaidService.DME,
                            CodeNavaidService.ILS,
                            CodeNavaidService.LOC,
                        });

                    if (NavaidList != null && NavaidList.Count > 0)
                        CurNavaid = NavaidList[0];
                    else
                        CurPoint = null;

				}
				else
				{
					PointPickerIsActive = true;
					CurPoint = null;
				}


				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("PointChoise"));
			}

		}


		private double _att;

		public double ATT
		{
			get { return _att; }
			set
			{
				if (_att == value)
					return;
				_att = value;
				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("ATTXTT"));
			}
		}

		private double _xtt;
		public double XTT
		{
			get { return _xtt; }
			set
			{
				if (_xtt == value)
					return;
				ChangeModelChanged(_xtt, value, _applyXtt);
				_xtt = value;
				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("ATTXTT"));
			}

		}

		#endregion

		#region distance
		private double _minDistance;
		public double MinDistance
		{
			get { return Common.ConvertDistance(_minDistance, roundType.toUp); }
			set
			{
				_minDistance = Common.DeConvertDistance(value);

			}

		}

		private double _maxDistance;
		public double MaxDistance
		{
			get { return Common.ConvertDistance(_maxDistance, roundType.toDown); }
			set
			{
				_maxDistance = Common.DeConvertDistance(value);
			}
		}

		private double _distance;
		public double Distance
		{
			get { return Common.ConvertDistance(_distance, roundType.toNearest); }
			set { DistanceCondition(this, "Distance", Common.DeConvertDistance(value), PropertyChanged); }
		}

		private int _tag;
		public int TAG
		{
			get
			{

				return _tag;
			}
			set
			{
				_tag = value;

				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("TAG"));
			}
		}
		#endregion

		#region ValueActive


		public bool PointPickerIsActive { get; set; }

		public bool DesignatedPointListIsActive { get; set; }

		public bool NavaidListIsActive { get; set; }

		#endregion

		public bool HoldingAreaIsEnabled
		{
			get { return ((!IsApply && _holdingAreaIsEnabled) || (ChangedCount != 0 && _holdingAreaIsEnabled)) && (WizardNumber == 1); }
			set
			{
				if (_holdingAreaIsEnabled == value)
					return;

				_holdingAreaIsEnabled = value;
				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("HoldingAreaIsEnabled"));
			}
		}

		public int ChangedCount
		{
			get { return _changedCount; }
			set
			{
				_changedCount = value;
				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("HoldingAreaIsEnabled"));
			}

		}

		public bool ReportIsEnabled
		{
			get { return !HoldingAreaIsEnabled && _holdingAreaIsEnabled && WizardNumber == 1; }
		}

		private bool _saveIsEnabled;
		public bool SaveIsEnabled
		{
			get { return !HoldingAreaIsEnabled && _holdingAreaIsEnabled && _saveIsEnabled && WizardNumber == 1; }
			set
			{
				_saveIsEnabled = value;
				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("ReportIsEnabled"));

			}
		}

		private int _wizardNumber;
		public int WizardNumber
		{
			get { return _wizardNumber; }
			set
			{
				_wizardNumber = value;
				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("WizardNumber"));
			}
		}

		public string SaveAvailableHint { get; set; }

		#endregion

		#region :>Methods
		public void SetMinMaxDistance(double minValue, double maxValue)
		{
			_minDistance = minValue;
			_maxDistance = maxValue;
			double tmpDouble = _distance;
			if (_distance < _minDistance)
				tmpDouble = 2 * _minDistance;
			else
				if (_distance > _maxDistance)
				tmpDouble = _maxDistance;
			//if (tmpDouble != 0)
			DistanceCondition(this, "Distance", maxValue, PropertyChanged);
		}

		public Boolean PointDistanceControl(Aran.Geometries.Point ptGeo)
		{
			if ((ptGeo == null) || (_curAdhp == null))
				return false;

			Aran.Geometries.Point ptPrj = _spatialOperation.ToPrj(ptGeo);
			Aran.Geometries.Point ptPrjAirpot = _spatialOperation.ToPrj(GeomFunctions.Assign(_curAdhp.ARP));
			double ptToArpDistance = ARANFunctions.ReturnDistanceInMeters(ptPrj, ptPrjAirpot);
			if (ptToArpDistance > _distance || ptToArpDistance < _minDistance || !FeatureConvert.CheckPtInLIcenseArea(ptGeo))
			{
				System.Windows.Forms.MessageBox.Show("You must select point only inside the circle", Resources.Holding_Caption);
				return false;
			}
			else
				return true;

		}

		public void SetATTXTTValue(double att, double xtt)
		{
			if ((_att == att) && (_xtt == xtt))
				return;
			ChangeModelChanged(_att, att, _applyAtt);
			ChangeModelChanged(_xtt, xtt, _applyXtt);
			_att = att;
			_xtt = xtt;
			// if (PropertyChanged != null)
			//   PropertyChanged(this, new PropertyChangedEventArgs("ATTXTT"));

		}

		public void Dispose()
		{
			_ui.SafeDeleteGraphic(_ptHandle);
			_ui.SafeDeleteGraphic(_bigCircleHandle);
			_ui.SafeDeleteGraphic(_littleCircleHandle);
		}

		public void DrawPoint(Aran.Geometries.Point ptPrj)
		{
			DeletePoint();
			_ptHandle = _ui.DrawPoint(ptPrj, 1);
		}

		public void DeletePoint()
		{
			_ui.SafeDeleteGraphic(_ptHandle);
		}

		private void DistanceCondition(object owner, string propName, double newValue, PropertyChangedEventHandler eventHandler)
		{
			if (owner.GetType().GetProperty(propName) == null)
			{
				throw new ArgumentException("No property named " + propName + " on " + owner.GetType().FullName);
			}

			if (!Equals(_distance, Common.DeConvertDistance(newValue)))
			{
				_distance = Common.AdaptToInterval(newValue, _minDistance, _maxDistance, 1);
				var filterArea = FeatureConvert.CreateCircle(_curAdhp.ARP.Geo, _minDistance, _distance);

                if (_curSignificantType.Choice == SignificantPointChoice.DesignatedPoint)
				{
					PointList = _dbModule.HoldingQpi.GetDesignatedPointList(filterArea[0]);
					if (PointList != null)
						PointList = PointList.Where(dPoint => FeatureConvert.CheckPtInLIcenseArea(GeomFunctions.Assign(dPoint.Location)))
							.OrderBy(dpoint => dpoint.Designator).ToList<DesignatedPoint>();

				}
				else if (_curSignificantType.Choice == SignificantPointChoice.Navaid)
				{
                    var filterAreaInPrj = GlobalParams.SpatialRefOperation.ToPrj(filterArea);
					NavaidList = _dbModule.HoldingQpi.GetNavaidListByTypes(filterAreaInPrj[0],
                        GlobalParams.SpatialRefOperation, 
                        new CodeNavaidService[] {
                            CodeNavaidService.VOR, Aran.Aim.Enums.CodeNavaidService.VOR_DME,
                            CodeNavaidService.NDB, Aran.Aim.Enums.CodeNavaidService.NDB_DME,
                            CodeNavaidService.TACAN,
                            CodeNavaidService.ILS_DME,
                            CodeNavaidService.DME,
                            CodeNavaidService.ILS,
                            CodeNavaidService.LOC,});

					//if (NavaidList != null)
					//	NavaidList = NavaidList.Where(nPoint => FeatureConvert.CheckPtInLIcenseArea(GeomFunctions.Assign(nPoint.Location)))
					//	   .ToList<Navaid>();

				}

				DrawPointArea(GeomFunctions.Assign(_curAdhp.ARP), _distance, _minDistance);

				if (eventHandler != null)
					eventHandler(owner, new PropertyChangedEventArgs(propName));
			}

		}

		//private bool CheckPtInLIcenseArea(Aran.Geometries.Point ptGeo)
		//{
		//    return ARANFunctions.IsPointInPoly(_spatialOperation.GeoToPrj(ptGeo), InitHolding.LicenseRectPrj);

		//}

		private void DrawCircle(Aran.Geometries.Polygon circle)
		{
			if (circle == null)
				return;
			for (int i = 0; i < circle.ExteriorRing.Count; i++)
			{
				GlobalParams.UI.DrawPoint(GeomFunctions.AimToAranPointPrj(circle.ExteriorRing[i]), ePointStyle.smsCircle, 243);
			}
			for (int i = 0; i < circle.InteriorRingList.Count; i++)
			{
				for (int j = 0; j < circle.InteriorRingList[i].Count; j++)
				{
					GlobalParams.UI.DrawPoint(GeomFunctions.AimToAranPointPrj(circle.InteriorRingList[i][j]), ePointStyle.smsSquare, 100);
				}
			}
		}

		private void CurAdhpChange(AirportHeliport value)
		{

			if (_curAdhp != value && value != null)
			{
				_curAdhp = value;
				if (_curSignificantType.Choice == SignificantPointChoice.DesignatedPoint)
				{
					var filterArea = FeatureConvert.CreateCircle(_curAdhp.ARP.Geo, _minDistance, _distance);

                    PointList = _dbModule.HoldingQpi.GetDesignatedPointList(filterArea[0]);
					if (PointList != null)
						PointList = PointList.Where(dPoint => FeatureConvert.CheckPtInLIcenseArea(GeomFunctions.Assign(dPoint.Location)))
								.OrderBy(dpoint => dpoint.Designator)
								.ToList<DesignatedPoint>();

					if (PointList != null && PointList.Count > 0)
						CurSigPoint = PointList[0];
					else
					{
						if (CurSigPoint == null)
							ChangeCentralMeridian(GeomFunctions.Assign(_curAdhp.ARP));
						else
							CurSigPoint = null;
					}
				}

				DrawPointArea(GeomFunctions.Assign(_curAdhp.ARP), _distance, _minDistance);

				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("CurAdhp"));

			}
			else if (_curAdhp != value && value == null)
			{
				PointList = null;
				DeletePointArea();

				_curAdhp = value;
				CurPoint = null;
				DeletePoint();

				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("CurAdhp"));
			}

		}

		private void CurSigPointChange(DesignatedPoint value)
		{
			if (_curSigPoint != value)
			{

				Aran.Geometries.Point tmpPt = GeomFunctions.Assign(value.Location);
				ChangeModelChanged(_curPoint, tmpPt, _applyPoint);
				_curPoint = tmpPt;
				ChangeCentralMeridian(_curPoint);

				Aran.Geometries.Point ptPrj = _spatialOperation.ToPrj(_curPoint);

				_ui.SafeDeleteGraphic(_ptHandle);

				_ptHandle = _ui.DrawPointWithText(ptPrj, value.Designator, 1);

				_curSigPoint = value;

			}

		}

		private void CurNavaidChange(Navaid selectedNavaid)
		{
			if (_curNavaid != selectedNavaid)
			{
                var selectedNavaidPt = selectedNavaid.ComponentGeometry();
                if (selectedNavaidPt == null)
                    throw new NullReferenceException(selectedNavaid.Name + " Navaid's geometry is null");

				ChangeModelChanged(_curPoint, selectedNavaidPt, _applyPoint);
				_curPoint = selectedNavaidPt;
				ChangeCentralMeridian(_curPoint);

				Aran.Geometries.Point ptPrj = _spatialOperation.ToPrj(_curPoint);
				_ui.SafeDeleteGraphic(_ptHandle);

				_ptHandle = _ui.DrawPointWithText(ptPrj, selectedNavaid.Designator, 1);

				_curNavaid = selectedNavaid;

			}

		}

		private void DrawSignificantPoint(DesignatedPoint significantPoint)
		{
			_ui.SafeDeleteGraphic(_ptHandle);
			_curPoint = GeomFunctions.Assign(significantPoint.Location);
			_ptHandle = _ui.DrawPointWithText(_spatialOperation.ToPrj(_curPoint), significantPoint.Designator, 1);
		}

		private void DrawPointArea(Aran.Geometries.Point centerPt, double maxDistance, double minDistance)
		{
			DeletePointArea();
			Aran.Geometries.LineString bigCircle = ARANFunctions.CreateCircleAsPartPrj(_spatialOperation.ToPrj(centerPt), maxDistance);
			_bigCircleHandle = _ui.DrawLineString(bigCircle, 1, 1);
			if (minDistance > 1)
			{
				Aran.Geometries.LineString littleCircle = ARANFunctions.CreateCircleAsPartPrj(_spatialOperation.ToPrj(centerPt), minDistance);
				_littleCircleHandle = _ui.DrawLineString(littleCircle, 1, 1);
			}
		}

		private void DeletePointArea()
		{
			_ui.SafeDeleteGraphic(_bigCircleHandle);
			_ui.SafeDeleteGraphic(_littleCircleHandle);
		}

		private void ChangeCentralMeridian(Aran.Geometries.Point pt)
		{
			//if (pt == null)
			//    return;
			//if (Math.Abs(pt.X - _spatialOperation.CentralMeridian) > 3)
			//{
			//    GlobalParams.SpatialRefOperation.ChangeCentralMeridian(pt);
			//}

		}

		#endregion

		#region :>SpecialView

		public string ATTXTT
		{
			get
			{
				if (_curPoint != null)
					return Math.Round(ATT, 2) + " / " + Math.Round(XTT, 2);
				else
					return "";
			}
		}
		#endregion

		#region :>Event
		public event PropertyChangedEventHandler PropertyChanged;
		public event EventHandler CurPointChanged;
		#endregion

		public override void SetApplyParams()
		{
			_applyAtt = _att;
			_applyXtt = _xtt;
			_applyPoint = _curPoint;
			SaveIsEnabled = false;
		}
	}
}
