using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Aran.Aim;
using Aran.Aim.Data;
using Aran.Aim.Data.Filters;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.AranEnvironment;
using Aran.Geometries;
using Aran.PANDA.Common;
using Aran.PANDA.LegCreator.Model;
using Aran.Queries;
using GalaSoft.MvvmLight;
using NLog;

namespace Aran.PANDA.LegCreator.ViewModel
{
	/// <summary>
	/// This class contains properties that the main View can data bind to.
	/// <para>
	/// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
	/// </para>
	/// <para>
	/// You can also use Blend to data bind with the tool's support.
	/// </para>
	/// <para>
	/// See http://www.galasoft.ch/mvvm
	/// </para>
	/// </summary>
	public class MainViewModel : ViewModelBase
	{
		private string _selectedEffectiveDate, _procCount, _transitionId, _legCount, _transitionCount;
		private bool _canFinish, _canSelectPage3, _reverseGeometry, _canSelectPage2, _addToExistProc, _canSelectPage4, _createNewTransition;
		private FeatureType _selectedSaveProcType;
		private Proc _selectedSaveProc;
		private readonly List<int> _drawedItemIndexList;
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
		private Leg _selectedLeg;
		private DateTime _effectiveDate;
		private FeatureType _selectedProcType;
		private FeatureType _newFeatType;
		private Proc _selectedProc;
		private readonly DbProvider _dbProvider;
		private ObservableCollection<CodeProcedurePhase> _transitionTypes;
		private CodeProcedurePhase _selectedTransitionType;
		private Airport _selectedAirport;
		private Transition _selectedTransition;
		private readonly IScreenCapture _screenCapture;

		public MainViewModel()
		{
			Logger.Info("Beginning of Construct method");
			_dbProvider = (DbProvider) GlobalParams.AranEnvironment.DbProvider;
			_drawedItemIndexList = new List<int>();
			Procs = new ObservableCollection<Proc>();
			ResultProcs = new ObservableCollection<Proc>();
			Transitions = new ObservableCollection<Transition>();
			//CreateNewTransition = true;
			EffectiveDate = _dbProvider.DefaultEffectiveDate.Date;
			LoadAirports();
			LoadProcsTypes();
			LoadResultFeatTypes();
			Legs = new ObservableCollection<Leg>();
			Add2ExistProc = true;
			Logger.Info("End of Construct method");
			_screenCapture = GlobalParams.AranEnvironment.GetScreenCapture("LegCreator");
			////if (IsInDesignMode)
			////{
			////    // Code runs in Blend --> create design time data.
			////}
			////else
			////{
			////    // Code runs "for real"
			////}
		}

		internal void Closing()
		{
		    ClearGraphics();
			_screenCapture.Rollback();
		}

		internal string Finish(IntPtr ptr)
		{
			_screenCapture.Save(ptr);
			Logger.Info("Beginning of Finish method");
			try
			{
				if (SaveProcedure())
				{
				    Screenshot screenshot;
                    if (ReplaceSourceLeg)
                    {
                        screenshot = new Screenshot
                        {
                            DateTime = DateTime.Now,
                            Identifier = SelectedProc.Id,
                            Images = _screenCapture.Commit(SelectedProc.Id)
                        };
                        _dbProvider.SetFeatureScreenshot(screenshot);
                    }
                    else if (Add2ExistProc)
					{
						screenshot = new Screenshot
						{
							DateTime = DateTime.Now,
							Identifier = SelectedResultProc.Id,
							Images = _screenCapture.Commit(SelectedResultProc.Id)
						};
						_dbProvider.SetFeatureScreenshot(screenshot);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Error inside Finish method");
			    return ex.Message;
			}
			finally
			{
				_screenCapture.Rollback();
			}
			Logger.Info("End of Finish method");
            return string.Empty;
		}

		private bool SaveProcedure()
		{
			SegmentLeg newFeature = (SegmentLeg) AimObjectFactory.CreateFeature(SelectedResultFeatType);
			newFeature.Id = -1;
			newFeature.Identifier = Guid.NewGuid();
			newFeature.TimeSlice = new Aran.Aim.DataTypes.TimeSlice
			{
				Interpretation = Aran.Aim.Enums.TimeSliceInterpretationType.PERMDELTA,
				SequenceNumber = 1,
				CorrectionNumber = 0,
                ValidTime = new TimePeriod (_dbProvider.DefaultEffectiveDate),
                FeatureLifetime = new TimePeriod (_dbProvider.DefaultEffectiveDate)
			};

			SegmentLeg source = SelectedLeg.GetSourceSegmentLeg();

			newFeature.EndConditionDesignator = source.EndConditionDesignator;
			newFeature.LegPath = source.LegPath;
			newFeature.LegTypeARINC = source.LegTypeARINC;
			newFeature.Course = source.Course;
			newFeature.CourseType = source.CourseType;
			newFeature.CourseDirection = source.CourseDirection;
			newFeature.TurnDirection = source.TurnDirection;
			newFeature.SpeedLimit = source.SpeedLimit;
			newFeature.SpeedReference = source.SpeedReference;
			newFeature.SpeedInterpretation = source.SpeedInterpretation;
			newFeature.BankAngle = source.BankAngle;
			newFeature.Length = source.Length;
			newFeature.Duration = source.Duration;
			newFeature.ProcedureTurnRequired = source.ProcedureTurnRequired;
			newFeature.UpperLimitAltitude = source.UpperLimitAltitude;
			newFeature.UpperLimitReference = source.UpperLimitReference;
			newFeature.LowerLimitAltitude = source.LowerLimitAltitude;
			newFeature.LowerLimitReference = source.LowerLimitReference;
			newFeature.AltitudeInterpretation = source.AltitudeInterpretation;
			newFeature.AltitudeOverrideATC = source.AltitudeOverrideATC;
			newFeature.AltitudeOverrideReference = source.AltitudeOverrideReference;
			newFeature.VerticalAngle = source.VerticalAngle;
			newFeature.StartPoint = source.StartPoint;
			if (newFeature.StartPoint != null)
			{
				newFeature.StartPoint.Id = -1;
				newFeature.StartPoint.FacilityMakeup.ForEach(it => it.Id = -1);
				newFeature.StartPoint.FacilityMakeup.ForEach(it => it.FacilityAngle.ForEach(t => t.Id = -1));
			}

			newFeature.EndPoint = source.EndPoint;
			if (newFeature.EndPoint != null)
			{
				newFeature.EndPoint.Id = -1;
				newFeature.EndPoint.FacilityMakeup.ForEach(it => it.Id = -1);
				newFeature.EndPoint.FacilityMakeup.ForEach(it => it.FacilityAngle.ForEach(t => t.Id = -1));
			}

			newFeature.Trajectory = ReverseGeomOrder(source.Trajectory);

			newFeature.ArcCentre = source.ArcCentre;
			if (newFeature.ArcCentre != null)
			{
				newFeature.ArcCentre.Id = -1;
				newFeature.ArcCentre.FacilityMakeup.ForEach(it => it.Id = -1);
				newFeature.ArcCentre.FacilityMakeup.ForEach(t => t.FacilityAngle.ForEach(k => k.Id = -1));
			}

			newFeature.Angle = source.Angle;
			newFeature.Distance = source.Distance;
			newFeature.AircraftCategory.AddRange(source.AircraftCategory);
			newFeature.AircraftCategory.ForEach(it => it.Id = -1);

			newFeature.Holding = source.Holding;
			if (newFeature.Holding != null)
				newFeature.Holding.Id = -1;

			newFeature.DesignSurface.AddRange(source.DesignSurface);
			newFeature.DesignSurface.ForEach(it => it.Id = -1);
			newFeature.DesignSurface.ForEach(it => it.AircraftCategory.ForEach(t => t.Id = -1));
			newFeature.DesignSurface.ForEach(it => it.SignificantObstacle.ForEach(k => k.Id = -1));
			newFeature.DesignSurface.ForEach(it => it.SignificantObstacle.ForEach(k => k.Adjustment.ForEach(m => m.Id = -1)));
			newFeature.DesignSurface.ForEach(
				it => it.SignificantObstacle.ForEach(k => k.ObstaclePlacement.ForEach(m => m.Id = -1)));


			newFeature.Annotation.AddRange(source.Annotation);
			newFeature.Annotation.ForEach(it => it.Id = -1);
			newFeature.Annotation.ForEach(it => it.TranslatedNote.ForEach(k => k.Id = -1));


			ICommonQPI commonQpi = new CommonQPI();
			commonQpi.Open(_dbProvider);

		    if (ReplaceSourceLeg)
		    {
		        var aixmProc = SelectedProc.GetSourceProcedure();
		        aixmProc.TimeSlice.CorrectionNumber++;
		        var transition = aixmProc.FlightTransition.Find(t => t.Equals(SelectedLeg.GetTransition()));
		        var procTransLeg = transition.TransitionLeg.Find(lg => lg.SeqNumberARINC == SelectedLeg.SequenceNumber);
		        procTransLeg.TheSegmentLeg = newFeature.GetAbstractFeatureRef<AbstractSegmentLegRef>();

		        SelectedLeg.GetSourceSegmentLeg().TimeSlice.CorrectionNumber++;
                SelectedLeg.GetSourceSegmentLeg().TimeSlice.FeatureLifetime.EndPosition =
		            SelectedLeg.GetSourceSegmentLeg().TimeSlice.FeatureLifetime.BeginPosition;
		        SelectedLeg.GetSourceSegmentLeg().TimeSlice.ValidTime.EndPosition =
		            SelectedLeg.GetSourceSegmentLeg().TimeSlice.ValidTime.BeginPosition;

		        commonQpi.SetFeature(SelectedLeg.GetSourceSegmentLeg());
		        commonQpi.SetFeature(newFeature);
		        commonQpi.SetFeature(aixmProc);
		        FeatureType sourceLegType = (FeatureType) Enum.Parse(typeof(FeatureType), SelectedLeg.Type);
                commonQpi.SetRootFeatureType(new FeatureType[] {SelectedProcType, SelectedResultFeatType, sourceLegType});
		        return commonQpi.Commit();

		    }
            else if (!Add2ExistProc)
			{
				commonQpi.SetFeature(newFeature);
				commonQpi.SetRootFeatureType(SelectedResultFeatType);
				return commonQpi.Commit();
			}
			else
			{
				var aixmProc = SelectedResultProc.GetSourceProcedure();
                if(_dbProvider.ProviderType != DbProviderType.TDB)
                {
                    aixmProc.TimeSlice.CorrectionNumber++;
                }                
				ProcedureTransitionLeg transLeg = new ProcedureTransitionLeg();
				if (CreateNewTransition)
				{
					aixmProc.FlightTransition.Add(new ProcedureTransition()
					{
						TransitionId = TransitionId,
						Type = SelectedTransitionType
					});
					transLeg.SeqNumberARINC = 1;
					transLeg.TheSegmentLeg = newFeature.GetAbstractFeatureRef<AbstractSegmentLegRef>();
					aixmProc.FlightTransition[aixmProc.FlightTransition.Count - 1].TransitionLeg.Add(transLeg);
				}
				else
				{
					var trans = aixmProc.FlightTransition.Find(t => t.Equals(SelectedTransition.GetSourceTransition()));
					uint seqNumber = 1;
					trans.TransitionLeg.ForEach(t =>
					{
						if (t.SeqNumberARINC.HasValue && t.SeqNumberARINC.Value > seqNumber)
							seqNumber = t.SeqNumberARINC.Value;
					});
					seqNumber++;
					transLeg.SeqNumberARINC = seqNumber;
					transLeg.TheSegmentLeg = newFeature.GetAbstractFeatureRef<AbstractSegmentLegRef>();
					trans.TransitionLeg.Add(transLeg);
				}
				commonQpi.SetFeature(aixmProc);
				commonQpi.SetFeature(newFeature);
				commonQpi.SetRootFeatureType(SelectedResultProcType);
				return commonQpi.Commit(new FeatureType[] {SelectedResultProcType, SelectedResultFeatType});
			}
		}

		private Curve ReverseGeomOrder(Curve sourceCurve)
		{
            if (sourceCurve == null)
                return null;
			Curve result = new Curve();
			result.Assign(sourceCurve);
			result.Id = -1;
			if (!_reverseGeometry) return result;

			var geo = new MultiLineString();
			for (var i = sourceCurve.Geo.Count - 1;i >= 0;i--)
			{
				var lnString = new LineString();
				lnString.AddReverse( sourceCurve.Geo[ i ] );
				geo.Add(lnString);
			}
			result.Geo.Assign(geo);
			return result;
		}

		public bool CanFinish
		{
			get { return _canFinish; }
			set { Set(() => CanFinish, ref _canFinish, value); }
		}

		private void DrawMultiLineString(MultiLineString multiLineString)
		{
			_drawedItemIndexList.Add(GlobalParams.AranEnvironment.Graphics.DrawMultiLineString(multiLineString, 3, 255, true, true));
			GlobalParams.AranEnvironment.Graphics.Refresh();
		}

		private void ClearGraphics()
		{
			_drawedItemIndexList.ForEach(t => { GlobalParams.AranEnvironment.Graphics.SafeDeleteGraphic(t); });
			_drawedItemIndexList.Clear();
		}

		internal void Next(IntPtr ptr)
		{
			_screenCapture.Save(ptr);
		}

		#region 1st Page

		private void LoadAirports()
		{
			Logger.Info("Beginning of LoadAirports method");
			try
			{
				var res = _dbProvider.GetVersionsOf(FeatureType.AirportHeliport, Aim.Enums.TimeSliceInterpretationType.BASELINE,
					default(Guid), true, new Aim.Data.Filters.TimeSliceFilter(EffectiveDate), null);
				if (res.IsSucceed)
				{
					Airports = new ObservableCollection<Airport>();
					res.GetListAs<AirportHeliport>().ForEach(t => { Airports.Add(new Airport() {Name = t.Name, Id = t.Identifier}); });
					Settings settings = new Settings();
					settings.Load(GlobalParams.AranEnvironment);
					foreach (var arp in Airports)
					{
						if (arp.Id == settings.Aeroport)
						{
							SelectedAirport = arp;
							break;
						}
					}
				}
				else
					Logger.Error("Error in LoadAirports when read airport from db.\r\nError : " + res.Message);
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Error inside LoadAirports");
			}
			Logger.Info("End of LoadAirports method");
		}

		public Airport SelectedAirport
		{
			get { return _selectedAirport; }
			set
			{
				Set(() => SelectedAirport, ref _selectedAirport, value);
				LoadProcs(Procs, SelectedProcType);
			}
		}

		public ObservableCollection<FeatureType> ProcTypes { get; private set; }

		public FeatureType SelectedProcType
		{
			get { return _selectedProcType; }
			set
			{
				Set(() => SelectedProcType, ref _selectedProcType, value);
				LoadProcs(Procs, SelectedProcType);
			}
		}

		private void LoadProcs(ObservableCollection<Proc> procList, FeatureType procType)
		{
			if (SelectedAirport == null || (int) procType == 0)
				return;
			Logger.Info(
				"Beginning of LoadProcs method. \r\nParameters => SelectedAirport : {0}; SelectedProcType : {1}; SelectedEffectiveDate : {2}",
				SelectedAirport.Name, procType, SelectedEffectiveDate);
			try
			{
				procList.Clear();
				ComparisonOps compOper = new ComparisonOps(ComparisonOpType.EqualTo, "airportHeliport", SelectedAirport.Id);
				OperationChoice operChoice = new OperationChoice(compOper);
				Filter filter = new Filter(operChoice);

				var res = _dbProvider.GetVersionsOf(procType, Aim.Enums.TimeSliceInterpretationType.BASELINE, default(Guid), false,
					new Aim.Data.Filters.TimeSliceFilter(EffectiveDate), null, filter);
				if (res.IsSucceed)
				{
					var procedureList = res.GetListAs<Procedure>();
					procedureList.ForEach(proc =>
					{
						var tmp = new Proc
						{
							Id = proc.Identifier,
							TimeSlice = proc.TimeSlice.SequenceNumber + "." + proc.TimeSlice.CorrectionNumber + " from " +
							            proc.TimeSlice.ValidTime.BeginPosition.ToString("dd MMM yyyy")
						};

						if (proc.TimeSlice.ValidTime.EndPosition.HasValue)
							tmp.TimeSlice += " to " + proc.TimeSlice.ValidTime.EndPosition.Value.ToString("dd MMM yyyy");
						if (procType == FeatureType.StandardInstrumentArrival)
							tmp.Designator = (proc as StandardInstrumentArrival)?.Designator;
						else if (procType == FeatureType.StandardInstrumentDeparture)
							tmp.Designator = (proc as StandardInstrumentDeparture)?.Designator;
						tmp.Name = proc.Name;
						if (proc.CodingStandard.HasValue)
							tmp.CodingStandard = proc.CodingStandard.Value.ToString();
						if (proc.RNAV.HasValue)
							tmp.IsRnav = proc.RNAV.Value;
						if (proc.DesignCriteria.HasValue)
							tmp.DesignCriteria = proc.DesignCriteria.Value.ToString();
						tmp.SetSourceProcedure(proc);
						//tmp.SetLegs ( LoadLegs ( proc ) );
						procList.Add(tmp);
					});
				}
				ProcCount = "Count: " + procList.Count.ToString();
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Error inside of LoadProcs method");
			}
			Logger.Info("End of LoadProcs method");
		}

		public Proc SelectedProc
		{
			get { return _selectedProc; }
			set
			{
				Set(() => SelectedProc, ref _selectedProc, value);
				ClearGraphics();
				Legs.Clear();
				CanSelectPage2 = (_selectedProc != null);
				if (_selectedProc == null)
					return;
				if (_selectedProc.GetLegs() == null)
					_selectedProc.SetLegs(LoadLegs(_selectedProc.GetSourceProcedure().FlightTransition));
				foreach (var item in _selectedProc.GetLegs())
					Legs.Add(item);
				LegCount = "Count: " + Legs.Count.ToString();
				foreach (var leg in Legs)
				{
					var geo = leg.GetProjGeo();
					if (geo == null)
						continue;
					DrawMultiLineString(geo);
				}
			}
		}

		public DateTime EffectiveDate
		{
			get { return _effectiveDate; }
			set
			{
				Set(() => EffectiveDate, ref _effectiveDate, value.Date);
				SelectedEffectiveDate = EffectiveDate.ToString("dd MMM yyyy");
				//BeginLifeTime = EffectiveDate;
				LoadProcs(Procs, SelectedProcType);
			}
		}

		public ObservableCollection<Airport> Airports { get; private set; }

		public ObservableCollection<Proc> Procs { get; }

		public bool CanSelectPage2
		{
			get { return _canSelectPage2; }
			set { Set(() => CanSelectPage2, ref _canSelectPage2, value); }
		}

		public string ProcCount
		{
			get { return _procCount; }
			private set { Set(() => ProcCount, ref _procCount, value); }
		}

		private void LoadProcsTypes()
		{
			if (ProcTypes == null)
			{
				ProcTypes = new ObservableCollection<FeatureType>
				{
					FeatureType.StandardInstrumentDeparture,
					FeatureType.StandardInstrumentArrival,
					FeatureType.InstrumentApproachProcedure
				};
			}
		}

		#endregion

		#region 2nd Page

		public ObservableCollection<Leg> Legs { get; set; }

		public Leg SelectedLeg
		{
			get { return _selectedLeg; }
			set
			{
				Set(() => SelectedLeg, ref _selectedLeg, value);
				CanSelectPage3 = (_selectedLeg != null);
				ClearGraphics();
				var geo = _selectedLeg?.GetProjGeo();
				if (geo == null)
					return;
				DrawMultiLineString(geo);
			}
		}

		private void LoadResultFeatTypes()
		{
			if (ResultFeatTypes == null)
			{
				ResultFeatTypes = new ObservableCollection<FeatureType>
				{
					FeatureType.ArrivalLeg,
					FeatureType.DepartureLeg,
					FeatureType.ArrivalFeederLeg,
					FeatureType.InitialLeg,
					FeatureType.IntermediateLeg,
					FeatureType.MissedApproachLeg,
					FeatureType.FinalLeg
				};
			}
		}

		public string LegCount
		{
			get { return _legCount; }
			private set { Set(() => LegCount, ref _legCount, value); }
		}

		private ObservableCollection<Leg> LoadLegs(List<ProcedureTransition> transitionList)
		{
			Logger.Info("Beginning of LoadLegs method");
			ObservableCollection<Leg> legs = new ObservableCollection<Leg>();
			try
			{
			    transitionList.ForEach(flight => flight.TransitionLeg.ForEach(leg =>
				{
					FeatureType featType;
					if (leg.TheSegmentLeg.FeatureType.HasValue)
						featType = leg.TheSegmentLeg.FeatureType.Value;
					else
						featType = (FeatureType) (leg.TheSegmentLeg as IAbstractFeatureRef).FeatureTypeIndex;
					GettingResult res = _dbProvider.GetVersionsOf(featType, Aim.Enums.TimeSliceInterpretationType.BASELINE,
						leg.TheSegmentLeg.Identifier, false, new Aim.Data.Filters.TimeSliceFilter(EffectiveDate));
					if (res.IsSucceed)
					{
						var segLeg = res.List[0] as SegmentLeg;
						Leg myLeg = new Leg
						{
							Id = segLeg.Identifier,
							TimeSlice = segLeg.TimeSlice.SequenceNumber + "." + segLeg.TimeSlice.CorrectionNumber + " from " +
							            segLeg.TimeSlice.ValidTime.BeginPosition.ToString("dd MMM yyyy")
						};

					    myLeg.SetTransition(flight);

						if (segLeg.TimeSlice.ValidTime.EndPosition.HasValue)
							myLeg.TimeSlice += " to " + segLeg.TimeSlice.ValidTime.EndPosition.Value.ToString("dd MMM yyyy");
						myLeg.Type = segLeg.FeatureType.ToString();

						if (segLeg.LegTypeARINC.HasValue)
							myLeg.LegTypeArinc = segLeg.LegTypeARINC.Value.ToString();
						if (segLeg.EndConditionDesignator.HasValue)
							myLeg.EndConditionDesignator = segLeg.EndConditionDesignator.Value.ToString();
						if (segLeg.LegPath.HasValue)
							myLeg.LegPath = segLeg.LegPath.Value.ToString();
						if (segLeg.Course.HasValue)
							myLeg.Course = segLeg.Course.Value;
						if (segLeg.CourseType.HasValue)
							myLeg.CourseType = segLeg.CourseType.Value.ToString();
						if (segLeg.CourseDirection.HasValue)
							myLeg.CourseDirection = segLeg.CourseDirection.ToString();
						myLeg.SetSourceSegmentLeg(segLeg);
						if (flight.Type.HasValue)
							myLeg.TransitionType = flight.Type.Value.ToString();
						if (leg.SeqNumberARINC.HasValue)
							myLeg.SequenceNumber = leg.SeqNumberARINC.Value;
						if (segLeg.Trajectory != null)
							myLeg.SetProjGeo(GlobalParams.SpatialRefOperation.ToPrj<MultiLineString>(segLeg.Trajectory.Geo));
						legs.Add(myLeg);
					}
				}));
				//LegCount = "Count: " + legs.Count.ToString ( );
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Error inside of LoadLegs method");
			}
			Logger.Info("End of LoadLegs method");
			return legs;
		}

		public bool CanSelectPage3
		{
			get { return _canSelectPage3; }
			set { Set(() => CanSelectPage3, ref _canSelectPage3, value); }
		}

		public string SelectedEffectiveDate
		{
			get { return _selectedEffectiveDate; }
			set { Set(() => SelectedEffectiveDate, ref _selectedEffectiveDate, value); }
		}

		#endregion

		#region 3rd Page

		public bool Add2ExistProc
		{
			get { return _addToExistProc; }
			set
			{
				Set(() => Add2ExistProc, ref _addToExistProc, value);
				if (_addToExistProc)
					LoadResultProcTypes();
				//CanSelect4thPage = !_addToExistProc;
				//if ( _addToExistProc )
				CanSelectPage4 = false;
				//else
				//CanSelect4thPage = false;
				CanFinish = !_addToExistProc;
			    SetGridVisibility();
			}
		}

        private bool _replaceSourceLeg;

	    public bool ReplaceSourceLeg
	    {
	        get => _replaceSourceLeg;
	        set
	        {
	            Set(() => ReplaceSourceLeg, ref _replaceSourceLeg, value);
                SetGridVisibility();
	        }
	    }

	    private void SetGridVisibility()
	    {
	        if (ReplaceSourceLeg)
	        {
	            GridVisible = false;
	            CanFinish = true;
	            CanSelectPage4 = false;
	        }
	        else if (Add2ExistProc)
	        {
	            GridVisible = true;
	            CanFinish = (SelectedResultProc != null);
	        }
	        else
                GridVisible = false;
	    }

	    private void LoadResultProcTypes()
		{
			if (ResultProcTypes == null)
			{
				ResultProcTypes = new ObservableCollection<FeatureType>();
				foreach (var procType in ProcTypes)
				{
					ResultProcTypes.Add(procType);
				}
			}
		}

		public Proc SelectedResultProc
		{
			get { return _selectedSaveProc; }
			set
			{
				Set(() => SelectedResultProc, ref _selectedSaveProc, value);
				//CreateNewTransition = true;
				//CanFinish = false;
				LoadTransitions();
				CanSelectPage4 = (_selectedSaveProc != null);
				ClearGraphics();
				if (_selectedSaveProc == null)
					return;
				if( _selectedSaveProc.GetLegs() == null)
					_selectedSaveProc.SetLegs(LoadLegs(_selectedSaveProc.GetSourceProcedure().FlightTransition));
				foreach (var leg in _selectedSaveProc.GetLegs())
				{
					var geo = leg.GetProjGeo();
					if (geo == null)
						continue;
					DrawMultiLineString(geo);
				}
			}
		}

		private void LoadTransitions()
		{
			if (SelectedResultProc == null)
				return;

			Logger.Info("Beginning of LoadTransitions method");
			try
			{
				var aixmProc = SelectedResultProc.GetSourceProcedure();
				Transitions.Clear();
				aixmProc.FlightTransition.ForEach(t =>
				{
					Transition trans = new Transition {TransitionId = t.TransitionId};
					if (t.Type.HasValue)
						trans.Type = t.Type.Value;
					trans.Instruction = t.Instruction;
					if (t.DepartureRunwayTransition != null)
					{
						trans.DepartureRunwayDirection = "";
						GettingResult getResult;
						t.DepartureRunwayTransition.Runway.ForEach(rwy =>
							{
								getResult = _dbProvider.GetVersionsOf(FeatureType.RunwayDirection, TimeSliceInterpretationType.BASELINE,
									rwy.Feature.Identifier, true, new Aim.Data.Filters.TimeSliceFilter(EffectiveDate), null);
								if (getResult.IsSucceed)
								{
									trans.DepartureRunwayDirection += getResult.GetListAs<RunwayDirection>()[0].Designator + ", ";
								}
							}
						);
						if (!string.IsNullOrEmpty(trans.DepartureRunwayDirection))
							trans.DepartureRunwayDirection = trans.DepartureRunwayDirection.Substring(0,
								trans.DepartureRunwayDirection.Length - 2);
					}
					trans.SetSourceTransition(t);
					Transitions.Add(trans);
				});
				TransitionCount = "Count: " + Transitions.Count;
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Error inside of LoadTransitions method");
			}
			Logger.Info("End of LoadTransitions method");
		}

		public ObservableCollection<Proc> ResultProcs { get; }

		public ObservableCollection<FeatureType> ResultProcTypes { get; private set; }

		public FeatureType SelectedResultProcType
		{
			get { return _selectedSaveProcType; }
			set
			{
				Set(() => SelectedResultProcType, ref _selectedSaveProcType, value);
				LoadProcs(ResultProcs, SelectedResultProcType);
			}
		}

		public bool CanSelectPage4
		{
			get { return _canSelectPage4; }
			set { Set(() => CanSelectPage4, ref _canSelectPage4, value); }
		}

		public ObservableCollection<FeatureType> ResultFeatTypes { get; set; }

		public FeatureType SelectedResultFeatType
		{
			get { return _newFeatType; }
			set { Set(() => SelectedResultFeatType, ref _newFeatType, value); }
		}

        private bool _gridVisible;

        public bool GridVisible
        {
            get { return _gridVisible; }
            set { Set(() => GridVisible, ref _gridVisible, value);  }
        }


        #endregion

        #region 4th Page

        public ObservableCollection<CodeProcedurePhase> TransitionTypes
		{
			get
			{
				if (_transitionTypes == null)
				{
					_transitionTypes = new ObservableCollection<CodeProcedurePhase>();
					foreach (var trType in Enum.GetValues(typeof(CodeProcedurePhase)))
					{
						_transitionTypes.Add((CodeProcedurePhase) trType);
					}
				}
				return _transitionTypes;
			}
			set { Set(() => TransitionTypes, ref _transitionTypes, value); }
		}

		public CodeProcedurePhase SelectedTransitionType
		{
			get { return _selectedTransitionType; }
			set { Set(() => SelectedTransitionType, ref _selectedTransitionType, value); }
		}

		public bool CreateNewTransition
		{
			get { return _createNewTransition; }
			set
			{
				Set(() => CreateNewTransition, ref _createNewTransition, value);
				ClearGraphics();
				if (_createNewTransition)
					CanFinish = true;
				else
					CanFinish = SelectedTransition != null;
			}
		}

		public bool ReverseGeometry
		{
			get
			{
				return _reverseGeometry;
			}
			set { Set(() => ReverseGeometry, ref _reverseGeometry, value); }
		}

		public string TransitionId
		{
			get { return _transitionId; }
			set { Set(() => TransitionId, ref _transitionId, value); }
		}

		public ObservableCollection<Transition> Transitions { get; }

		public Transition SelectedTransition
		{
			get { return _selectedTransition; }
			set
			{
				Set(() => SelectedTransition, ref _selectedTransition, value);
				ClearGraphics();

			    //Legs.Clear();
			    var procTransList = new List<ProcedureTransition>();
			    procTransList.Add(_selectedTransition.GetSourceTransition());
			    var transitionLegs = LoadLegs(procTransList);
			    LegCount = "Count: " + transitionLegs.Count.ToString();
                foreach (var leg in transitionLegs)
                {
                    var geo = leg.GetProjGeo();
                    if (geo == null)
                        continue;
                    DrawMultiLineString(geo);
                }
                //         foreach (var item in LoadLegs(procTransList))
                //    Legs.Add(item);
                //LegCount = "Count: " + Legs.Count.ToString();
                //foreach (var leg in Legs)
                //{
                //    var geo = leg.GetProjGeo();
                //    if (geo == null)
                //        continue;
                //    DrawMultiLineString(geo);
                //}
                CanFinish = (_selectedTransition != null);
			}
		}

		public string TransitionCount
		{
			get { return _transitionCount; }
			set { Set(() => TransitionCount, ref _transitionCount, value); }
		}

		#endregion
	}
}