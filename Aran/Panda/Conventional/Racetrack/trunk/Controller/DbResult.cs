using System;
using System.Linq;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;
using Aran.Geometries;
using Aran.Aim.Enums;
using Aran.PANDA.Common;
using Aran.Queries;
using System.Collections.Generic;
using Aran.Aim.Data;
using Aran.Aim;

namespace Aran.PANDA.Conventional.Racetrack.Controller
{
	internal class DbResult
	{
		private readonly UomSpeed _speedUom;
		private readonly UomDistanceVertical _heightUom;
		private readonly UomDistance _distanceUom;
		

		public DbResult ( )
		{
			if (GlobalParams.UnitConverter.SpeedUnit == "kt" || GlobalParams.UnitConverter.SpeedUnit == "узел" )
				_speedUom = UomSpeed.KT;
			else if (GlobalParams.UnitConverter.SpeedUnit == "km/h" || GlobalParams.UnitConverter.SpeedUnit == "км/ч")
				_speedUom = UomSpeed.KM_H;

			if ( GlobalParams.UnitConverter.HeightUnit == "m" || GlobalParams.UnitConverter.HeightUnit == "м" )
				_heightUom = UomDistanceVertical.M;
			else if( GlobalParams.UnitConverter.HeightUnit == "ft" || GlobalParams.UnitConverter.HeightUnit == "фт" )
				_heightUom = UomDistanceVertical.FT;
			//heightUom = (UomDistanceVertical)Enum.Parse(typeof(UomDistanceVertical),GlobalParams.UnitConverter.HeightUnit.ToUpper());
			if ( GlobalParams.UnitConverter.DistanceUnit == "km" || GlobalParams.UnitConverter.DistanceUnit == "км" )
				_distanceUom = UomDistance.KM;
			else if (GlobalParams.UnitConverter.DistanceUnit == "NM" || GlobalParams.UnitConverter.DistanceUnit == "мм" )
				_distanceUom = UomDistance.NM;

			//_distanceUom = (UomDistance)Enum.Parse(typeof(UomDistance), GlobalParams.UnitConverter.DistanceUnit.ToUpper());
		}

		internal bool Save(Report report, MainController raceterackModel, out Guid id)
        {
            SegmentPoint segmentPoint = CreateSegmentPoint(raceterackModel.Speed.FlighPhase, raceterackModel.ToleranceArea,
                raceterackModel.SelectedNavPntsPrj, raceterackModel.PostFixTolerance, raceterackModel.PriorFixTolerance,
                raceterackModel.DsgPntSelection, raceterackModel.ProcType, raceterackModel.Faclts, raceterackModel.DsgPntSelection.NominalDistanceInGeo, raceterackModel.EntryDirection);

            DesignatedPoint secondaryPoint = GlobalParams.Database.HoldingQpi.CreateFeature<DesignatedPoint>();
            secondaryPoint.Location = new AixmPoint();
            Point pnt = new Point(raceterackModel.SecondaryPoint.X, raceterackModel.SecondaryPoint.Y);
            Point pntPrj = GlobalParams.SpatialRefOperation.ToGeo<Point>(pnt);

            secondaryPoint.Location.Geo.X = pntPrj.X;
            secondaryPoint.Location.Geo.Y = pntPrj.Y;
            if (raceterackModel.SaveSecondaryPoint)
            {
                GlobalParams.Database.HoldingQpi.SetFeature(secondaryPoint);
            }

            var outboundLegSpan = CreateHoldingPatternLenght(raceterackModel, secondaryPoint, pntPrj);

            HoldingPattern holdingPattern = CreateHoldingPattern(raceterackModel.ProcType, raceterackModel.NominalTrack, raceterackModel.Speed,
    raceterackModel.Side, raceterackModel.LimDist, (double)raceterackModel.DirectionInDeg,
    raceterackModel.Altitude, raceterackModel.LowerLimitHldngPattern);
            holdingPattern.OutboundLegSpan = outboundLegSpan;
            secondaryPoint.Annotation.Add(CreateNote("Secondary point of holding pattern whose Identifier is " +
                                               holdingPattern.Identifier));



            if (!raceterackModel.ProtectionRecipEntry.IsEmpty)
                holdingPattern.Annotation.Add(CreateNote("Angle between RE and RP is " + raceterackModel.AngleRe_Rp + "°"));
            holdingPattern.Type = raceterackModel.Speed.FlighPhase == FlightPhase.Enroute
                ? CodeHoldingUsage.ENR
                : CodeHoldingUsage.TER;
            HoldingAssessment holdingAssessment = CreateHoldingAssesment(report, raceterackModel.LimDist,
                raceterackModel.AssessedAltitude, raceterackModel.Speed, raceterackModel.BasicArea,
                raceterackModel.ProtectionSector1, raceterackModel.ProtectionSector2, raceterackModel.ProtectionRecipEntry,
                raceterackModel.ProtectionInterRadEntryArea, raceterackModel.AreaWithBuffers);
            holdingPattern.HoldingPoint = segmentPoint;
            holdingAssessment.HoldingPoint = segmentPoint;
            holdingAssessment.AssessedHoldingPattern = holdingPattern.GetFeatureRef();
            holdingAssessment.LowerLimit =
                new ValDistanceVertical(raceterackModel.AssessedAltitude, holdingPattern.LowerLimit.Uom);
            holdingAssessment.LowerLimitReference = holdingPattern.LowerLimitReference;
            holdingAssessment.UpperLimit = holdingPattern.UpperLimit;
            holdingAssessment.UpperLimitReference = holdingPattern.UpperLimitReference;

            //InitialLeg initialLeg = CreateApproachLeg ( holdingAssessment, holdingPattern, segmentPoint, raceterackModel.NominalTrack, raceterackModel.Speed, raceterackModel.Side, raceterackModel.Procedure, raceterackModel.LimDist, raceterackModel.InbountTrack, raceterackModel.Faclts, raceterackModel.EntryDirection );


            GlobalParams.Database.HoldingQpi.SetFeature(holdingPattern);
            GlobalParams.Database.HoldingQpi.SetFeature(holdingAssessment);

            id = holdingAssessment.Identifier;
            if (raceterackModel.SaveSecondaryPoint)
            {
                GlobalParams.Database.HoldingQpi.SetRootFeatureType(
                    FeatureType.HoldingAssessment, FeatureType.DesignatedPoint);
                return GlobalParams.Database.HoldingQpi.Commit(new Aim.FeatureType[]
                    {FeatureType.HoldingPattern, FeatureType.HoldingAssessment, Aim.FeatureType.DesignatedPoint,FeatureType.AngleIndication, FeatureType.DistanceIndication}, sort: false);
            }
            else
            {
                GlobalParams.Database.HoldingQpi.SetRootFeatureType(
                    Aim.FeatureType.HoldingAssessment);
                return GlobalParams.Database.HoldingQpi.Commit(new Aim.FeatureType[]
                    {Aim.FeatureType.HoldingPattern, Aim.FeatureType.HoldingAssessment, FeatureType.AngleIndication, FeatureType.DistanceIndication}, sort: false);
            }
        }

        private HoldingPatternLength CreateHoldingPatternLenght(MainController raceterackModel, DesignatedPoint secondaryPoint, Point pntPrj)
        {
            void CreateHoldingPatternDuration(HoldingPatternLength holdingPatternLength1)
            {
                HoldingPatternDuration hpDuration = new HoldingPatternDuration
                {
                    Duration = new ValDuration
                    {
                        Uom = UomDuration.MIN,
                        Value = raceterackModel.LimDist.TimeInMin
                    }
                };
                holdingPatternLength1.EndTime = hpDuration;
            }

            var holdingPatternLength = new HoldingPatternLength();
            if (raceterackModel.ProcType == ProcedureTypeConv.Vordme)
            {
                if (raceterackModel.OutboundDefinitionViaTime)
                {
                    CreateHoldingPatternDuration(holdingPatternLength);
                }
                else
                {
                    SegmentPoint secondarySegmentPoint = new TerminalSegmentPoint();
                    if (raceterackModel.Speed.FlighPhase == FlightPhase.Enroute)
                    {
                        secondarySegmentPoint = new EnRouteSegmentPoint();
                    }

                    var aixmPnt = new AixmPoint();
                    aixmPnt.Geo.X = pntPrj.X;
                    aixmPnt.Geo.Y = pntPrj.Y;
                    var significantPoint = new SignificantPoint
                    {
                        Position = aixmPnt
                    };
                    secondarySegmentPoint.PointChoice = significantPoint;
                    var direction = raceterackModel.CalculateReRpAngle();
                    secondarySegmentPoint.FacilityMakeup.Add(CreatePointReference(null, raceterackModel.ProcType,
                        double.NaN, double.NaN, secondaryPoint,
                        raceterackModel.EntryDirection, direction, double.NaN, raceterackModel.SelectedNavaid,
                        raceterackModel.LimDist.ValueInGeo));
                    holdingPatternLength.EndPoint = secondarySegmentPoint;
                }
            }
            else
            {
                CreateHoldingPatternDuration(holdingPatternLength);
            }
            return holdingPatternLength;
        }

        //private InitialLeg CreateApproachLeg ( HoldingAssessment holdingAssessment, HoldingPattern holdingPattern, SegmentPoint segmentPoint, LineString nominalTrack, Speed speed, SideDirection side, Procedure proc, LimitingDistance limDist, InboundTrack inbountTrack, FixFacilities faclts, EntryDirection entryDirection)
        //{
        //    InitialLeg initialLeg = new InitialLeg ( );
        //    initialLeg.Identifier = Guid.NewGuid();
        //    initialLeg.EndConditionDesignator = CodeSegmentTermination.DURATION;
        //    initialLeg.LegPath = CodeTrajectory.HOLDING;
        //    initialLeg.LegTypeARINC = CodeSegmentPath.HF;
        //    initialLeg.Course = ARANMath.RadToDeg ( inbountTrack.Direction );
        //    initialLeg.CourseType = CodeCourse.TRUE_TRACK;
        //    if ( proc.Type == ProcedureTypeConv.VORDME && entryDirection == EntryDirection.Away )
        //        initialLeg.CourseDirection = CodeDirectionReference.FROM;
        //    else
        //        initialLeg.CourseDirection = CodeDirectionReference.TO;

        //    if ( side == SideDirection.sideLeft )
        //        initialLeg.TurnDirection = CodeDirectionTurn.LEFT;
        //    else
        //        initialLeg.TurnDirection = CodeDirectionTurn.RIGHT;

        //    ValSpeed valSpeed = new ValSpeed ( );
        //    valSpeed.Uom = UomSpeed.M_SEC;
        //    valSpeed.Value = speed.TAS;
        //    initialLeg.SpeedLimit = valSpeed;

        //    initialLeg.SpeedReference = CodeSpeedReference.TAS;
        //    initialLeg.Length = holdingAssessment.LegLengthAway;

        //    ValDuration duration = new ValDuration ( );
        //    duration.Uom = UomDuration.MIN;
        //    duration.Value = limDist.TimeInMin;
        //    initialLeg.Duration = duration;

        //    initialLeg.ProcedureTurnRequired = false;

        //    initialLeg.UpperLimitAltitude = holdingPattern.UpperLimit;
        //    initialLeg.UpperLimitReference = holdingPattern.UpperLimitReference;

        //    initialLeg.LowerLimitAltitude = holdingPattern.LowerLimit;
        //    initialLeg.LowerLimitReference = holdingPattern.LowerLimitReference;
        //    initialLeg.AltitudeInterpretation = CodeAltitudeUse.BETWEEN;

        //    initialLeg.StartPoint = ( TerminalSegmentPoint ) segmentPoint;
        //    initialLeg.EndPoint = ( TerminalSegmentPoint ) segmentPoint;
        //    initialLeg.Trajectory = GeomFunctions.ConvertToCurve ( nominalTrack );

        //    if ( proc.Type == ProcedureTypeConv.VORDME )
        //    {
        //        AngleIndication angle = new AngleIndication ( );
        //        angle.Identifier = new Guid ( );
        //        angle.Angle = Math.Round ( limDist.AngleInDeg );
        //        angle.AngleType = CodeBearing.TRUE;
        //        angle.IndicationDirection = initialLeg.CourseDirection;
        //        angle.TrueAngle = angle.Angle;
        //        angle.PointChoice = new SignificantPoint ( );
        //        angle.PointChoice.NavaidSystem = faclts.SelectedNavaid.GetFeatureRef ( );
        //        initialLeg.Angle = angle.GetFeatureRef ( );
        //        GlobalParams.Database.HoldingQpi.SetFeature ( angle );

        //        #region Distance
        //        DistanceIndication distance = new DistanceIndication ( );
        //        distance.Identifier = new Guid ( );
        //        distance.Distance = new ValDistance ( );
        //        distance.Distance.Uom = UomDistance.M;
        //        distance.Distance.Value = Math.Round ( limDist.ValueInGeo );

        //        distance.Type = CodeDistanceIndication.DME;
        //        distance.PointChoice = new SignificantPoint ( );
        //        distance.PointChoice.NavaidSystem = faclts.SelectedNavaid.GetFeatureRef ( );
        //        GlobalParams.Database.HoldingQpi.SetFeature ( distance );
        //        initialLeg.Distance = distance.GetFeatureRef ( );
        //        #endregion
        //    }
        //    initialLeg.AircraftCategory.AddRange ( holdingAssessment.ObstacleAssessment [ 0 ].AircraftCategory );
        //    HoldingUse holdingUse = new HoldingUse ( );
        //    holdingUse.TheHoldingPattern = holdingPattern.GetFeatureRef ( );
        //    initialLeg.Holding = holdingUse;

        //    initialLeg.DesignSurface.AddRange ( holdingAssessment.ObstacleAssessment );
        //    return initialLeg;
        //}

        private HoldingAssessment CreateHoldingAssesment ( Report report, LimitingDistance limDist, double assessedAltitude, Speed speed, Polygon basicArea, MultiPolygon protectSect1, MultiPolygon protectSect2, MultiPolygon protectRecipDir, MultiPolygon protectIntersectRadEntry, List<Polygon> areaWithBuffers )
		{
			HoldingAssessment holdingAssessment = GlobalParams.Database.HoldingQpi.CreateFeature<HoldingAssessment> ( );

			ValDistance legLength = new ValDistance
			{
				Uom = _distanceUom,
				Value = GlobalParams.UnitConverter.DistanceToDisplayUnits(limDist.LegLength, eRoundMode.NEAREST)
			};

			holdingAssessment.LegLengthAway = legLength;
			holdingAssessment.LegLengthToward = legLength;
			holdingAssessment.ObstacleAssessment.AddRange ( CreateObstacleAssesmentAreas ( speed, report, assessedAltitude, basicArea, protectSect1, protectSect2, protectRecipDir, protectIntersectRadEntry, areaWithBuffers ) );
			return holdingAssessment;
		}

		private List<ObstacleAssessmentArea> CreateObstacleAssesmentAreas ( Speed speed, Report report, double assessedAltitude, Polygon basicArea, MultiPolygon protectSect1, MultiPolygon protectSect2, MultiPolygon protectRecipDir, MultiPolygon protectIntersectRadEntry, List<Polygon> areaWithBuffes )
		{
			List<ObstacleAssessmentArea> result = new List<ObstacleAssessmentArea> ( );

			ValDistanceVertical valAssessedAltitude = new ValDistanceVertical
			{
				Value = assessedAltitude,
				Uom = _heightUom
			};
			//GlobalParams.UnitConverter.HeightToDisplayUnits(assessedAltitude, eRoundMode.NERAEST);

			MultiPolygon tmpMltPolygon = new MultiPolygon {basicArea};
			List<CodeAircraftCategory> airCraftCategoryList = new List<CodeAircraftCategory>();
			if (speed.SelectedAircraftCategoryIndex == 0)
			{
				airCraftCategoryList.Add(CodeAircraftCategory.A);
				airCraftCategoryList.Add(CodeAircraftCategory.B);
			}
			else if (speed.SelectedAircraftCategoryIndex == 1)
			{
				airCraftCategoryList.Add(CodeAircraftCategory.C);
				airCraftCategoryList.Add(CodeAircraftCategory.D);
				airCraftCategoryList.Add(CodeAircraftCategory.E);
			}
			ObstacleAssessmentArea basicObsAssArea = CreateObstacleAssessmentArea ( airCraftCategoryList, report, tmpMltPolygon,
							CodeObstacleAssessmentSurface.PRIMARY, ObstactleReportType.BasicArea, valAssessedAltitude );
			result.Add ( basicObsAssArea );

			if ( report.ObstacleReport.Count > 2 )
			{
				//UserReport assessedItem = null;

				var assessedItem = report.ObstacleReport.Aggregate ( ( rep1, rep2 ) => rep1.Penetrate > rep2.Penetrate ? rep1 : rep2 );

				int i = 0;
				double areaElevation = 0;
				foreach ( var item in areaWithBuffes )
				{
					tmpMltPolygon.Clear ( );
					MultiPoint mltPnt = item.ToMultiPoint ( );
					var tmpReport = report.ObstacleReport.FirstOrDefault ( rep => rep.AreaNumber == i + 1 );
					if ( tmpReport != null )
						areaElevation = tmpReport.Moc + assessedItem.Elevation;
					else
						areaElevation = assessedItem.Elevation;

					//foreach ( Point pnt in mltPnt )
					//{
					//}
					tmpMltPolygon.Add ( item );

					ObstacleAssessmentArea bufferObsAssArea = CreateObstacleAssessmentArea ( airCraftCategoryList, report, tmpMltPolygon, CodeObstacleAssessmentSurface.SECONDARY, ObstactleReportType.Buffer, valAssessedAltitude );
					result.Add ( bufferObsAssArea );
					i++;
				}
			}
			

	
			if ( !protectSect1.IsEmpty )
			{
				// Protection Sector 1
				ObstacleAssessmentArea protectSect1ObstAssessment = CreateObstacleAssessmentArea(airCraftCategoryList, report, protectSect1, CodeObstacleAssessmentSurface.PT_ENTRY_AREA, ObstactleReportType.ProtectSect1, valAssessedAltitude);
				protectSect1ObstAssessment.Annotation.Add ( CreateNote ( "Protection sector 1" ) );
				result.Add ( protectSect1ObstAssessment );
			}

			if ( !protectSect2.IsEmpty )
			{
				// Protection Sector 2
				ObstacleAssessmentArea protectSect2ObstAssessment = CreateObstacleAssessmentArea(airCraftCategoryList, report, protectSect2, CodeObstacleAssessmentSurface.PT_ENTRY_AREA, ObstactleReportType.ProtectSect2, valAssessedAltitude);
				protectSect2ObstAssessment.Annotation.Add ( CreateNote ( "Protection sector 2" ) );
				result.Add ( protectSect2ObstAssessment );
			}

			if ( !protectRecipDir.IsEmpty )
			{
				ObstacleAssessmentArea protectRecipAreaObstAssessment = CreateObstacleAssessmentArea(airCraftCategoryList, report, protectRecipDir, CodeObstacleAssessmentSurface.PT_ENTRY_AREA, ObstactleReportType.ProtectRecipDir, valAssessedAltitude);
				protectRecipAreaObstAssessment.Annotation.Add ( CreateNote ( "Protection reciprocal direction entry area" ) );
				result.Add ( protectRecipAreaObstAssessment );
			}

			if ( !protectIntersectRadEntry.IsEmpty )
			{
				ObstacleAssessmentArea protectIntersectRadEntryAreaObstAssessment = CreateObstacleAssessmentArea(airCraftCategoryList, report, protectSect2, CodeObstacleAssessmentSurface.PT_ENTRY_AREA, ObstactleReportType.ProtectIntersectRecipEntry, valAssessedAltitude);
				protectIntersectRadEntryAreaObstAssessment.Annotation.Add ( CreateNote ("Area assuming entries along the homing and intersecting radial in the case of a procedure based on the intersection of VOR radials") );
				result.Add ( protectIntersectRadEntryAreaObstAssessment );
			}
			return result;
		}

		private ObstacleAssessmentArea CreateObstacleAssessmentArea ( List<CodeAircraftCategory> aircraftCategoryList, Report report, MultiPolygon polygon, 
																			CodeObstacleAssessmentSurface codeAssSurface, ObstactleReportType obsReportType, ValDistanceVertical valAssessedAltitude )
		{
			ObstacleAssessmentArea result = new ObstacleAssessmentArea
			{
				Type = codeAssSurface,
				AssessedAltitude = valAssessedAltitude
			};

			foreach (var item in aircraftCategoryList)
			{
				AircraftCharacteristic aircraftCharecteristic = new AircraftCharacteristic
				{
					AircraftLandingCategory = item
				};
				result.AircraftCategory.Add(aircraftCharecteristic);
			}
			

			MultiPolygon geoPolygon = GlobalParams.SpatialRefOperation.ToGeo<MultiPolygon> ( polygon );
			result.Surface = GeomFunctions.ConvertMultiPolygonToSurface ( geoPolygon );

			foreach ( var item in report.VerticalStructureList )
			{
				if ( item.Value == obsReportType)
				{
					result.SignificantObstacle.Add ( new Obstruction
					{
						VerticalStructureObstruction = item.Key.GetFeatureRef ( )
					} );
				}
			}
			return result;
		}

		private Note CreateNote ( string text)
		{
			Note note = new Note {Purpose = CodeNotePurpose.DESCRIPTION};
			LinguisticNote lingNote = new LinguisticNote ( );
			TextNote txtNote = new TextNote
			{
				Lang = language.ENG,
				Value = text
			};
			lingNote.Note = txtNote;
			note.TranslatedNote.Add ( lingNote );
			return note;
		}

		private HoldingPattern CreateHoldingPattern (ProcedureTypeConv procType, LineString nominalTrack, Speed speed, 
            SideDirection side, LimitingDistance limDist, double direction, double altitude, double assessedAltitude)
		{
			HoldingPattern holdingPattern = GlobalParams.Database.HoldingQpi.CreateFeature<HoldingPattern> ( );            
			holdingPattern.InboundCourse = ARANMath.Modulus (direction - 180, 360);
			holdingPattern.OutboundCourse = direction;
			if ( side == SideDirection.sideLeft )
			{
				holdingPattern.TurnDirection = CodeDirectionTurn.LEFT;
				holdingPattern.NonStandardHolding = false;
			}
			else
			{
				holdingPattern.TurnDirection = CodeDirectionTurn.RIGHT;
				holdingPattern.NonStandardHolding = true;
			}

			holdingPattern.LowerLimit = new ValDistanceVertical
			{
				Uom = _heightUom,
				Value = assessedAltitude
			};
			// GlobalParams.UnitConverter.HeightToDisplayUnits (assessedAltitude, eRoundMode.NERAEST);
			holdingPattern.LowerLimitReference = CodeVerticalReference.MSL;

			holdingPattern.UpperLimit = new ValDistanceVertical
			{
				Uom = holdingPattern.LowerLimit.Uom,
				Value = GlobalParams.UnitConverter.HeightToDisplayUnits(altitude, eRoundMode.NEAREST)
			};
			holdingPattern.UpperLimitReference = CodeVerticalReference.MSL;

			holdingPattern.SpeedLimit = new ValSpeed
			{
				Uom = _speedUom,
				Value = GlobalParams.UnitConverter.SpeedToDisplayUnits(speed.Ias, eRoundMode.NEAREST)
			};

			holdingPattern.OutboundCourseType = CodeCourse.MAG_BRG;


			holdingPattern.Extent = new Curve ( );
			LineString lnString = GlobalParams.SpatialRefOperation.ToGeo<LineString> ( nominalTrack );
			holdingPattern.Extent.Geo.Add ( lnString );
			//LineString lnString = new LineString ( );
			//foreach ( Point pnt in nominalTrack )
			//{
			//    lnString.Add ( GlobalParams.SpatialRefOperation.ToGeo<Point> ( pnt ) );
			//}
			//holdingPattern.Extent.Geo.Add ( lnString );
			// HPattern.Instruction = "RNAV," + bLogic.ProcedureType.PropType + "," + bLogic.ModelPBN.CurReciever.RecieverName + "," +
			//bLogic.ModelPBN.CurPBN.PBNName;
			return holdingPattern;
		}


		private SegmentPoint CreateSegmentPoint (FlightPhase flightPhase, Ring toleranceArea, List<NavaidPntPrj> selectedNavPntsPrj, 
            double postFixTolerance, double priorFixtolerance, DesignatedPntSelection dsgPntSelection, 
            ProcedureTypeConv procType, FixFacilities faclts, double distanceInGeo, EntryDirection entryDirection)
		{
            SegmentPoint segmentPoint = new TerminalSegmentPoint();
            if (flightPhase == FlightPhase.Enroute)
                segmentPoint = new EnRouteSegmentPoint();
			DesignatedPoint wayPoint = null;
			if (procType == ProcedureTypeConv.VorNdb)
			{
				segmentPoint.PointChoice = new SignificantPoint {NavaidSystem = faclts.SelectedNavaid.GetFeatureRef()};
			}
			else
			{
				if (dsgPntSelection.ChosenPntType == PointChoice.None || dsgPntSelection.SelectedDesignatedPoint == null)
					return null;				
				segmentPoint.PointChoice = new SignificantPoint ();

				if (dsgPntSelection.ChosenPntType == PointChoice.Select)
				{
					wayPoint = dsgPntSelection.SelectedDesignatedPoint;
				}
				else if (dsgPntSelection.ChosenPntType == PointChoice.Create)
				{
					wayPoint = GlobalParams.Database.HoldingQpi.CreateFeature<DesignatedPoint> ();
					wayPoint.Location = new AixmPoint ();
					Point pnt = new Point (dsgPntSelection.SelectedDsgPntPrj.X, dsgPntSelection.SelectedDsgPntPrj.Y);
					Point pntPrj = GlobalParams.SpatialRefOperation.ToGeo<Point> (pnt);
					wayPoint.Location.Geo.X = pntPrj.X;
					wayPoint.Location.Geo.Y = pntPrj.Y;
					GlobalParams.Database.HoldingQpi.SetFeature (wayPoint);
				}
				segmentPoint.PointChoice.FixDesignatedPoint = wayPoint.GetFeatureRef ();

			}
            var degree = procType == ProcedureTypeConv.VorNdb ? 0 : ARANMath.DegToRad(dsgPntSelection.DirectionInAzimuth);
            segmentPoint.FacilityMakeup.Add(CreatePointReference(toleranceArea, procType, postFixTolerance, priorFixtolerance,wayPoint, 
                entryDirection, degree, dsgPntSelection.IntersectionDirection, faclts.SelectedNavaid, distanceInGeo));
			//foreach (PointReference pntReference in pntReferences)
            //{
			//	if (pntReference != null)
			//	{
			//		if (wayPoint != null)
			//			pntReference.Point = wayPoint.GetFeatureRef();
			//		segmentPoint.FacilityMakeup.Add(pntReference);
			//	}
			//}
			segmentPoint.FlyOver = true;
			segmentPoint.RadarGuidance = true;
			segmentPoint.ReportingATC = CodeATCReporting.COMPULSORY;
			segmentPoint.Waypoint = false;
			return segmentPoint;
		}

        //private PointReference[] CreatePointReferences(Ring toleranceArea, List<NavaidPntPrj> selectedNavPntsPrj, ProcedureTypeConv procType, double postFixTolerance, double priorFixtolerance, FixFacilities faclts)
        //{
        //	PointReference[] result = new PointReference[2];
        //	Surface surface = CreateToleranceArea(toleranceArea);
        //	if (procType == ProcedureTypeConv.Vordme)
        //	{
        //		result[0] = CreatePointReference(surface, faclts.Vor.GetFeatureRef(), postFixTolerance, priorFixtolerance);
        //		result[0].Role = CodeReferenceRole.RAD_DME;

        //		result[1] = CreatePointReference(surface, faclts.Dme.GetFeatureRef(), postFixTolerance, priorFixtolerance);
        //		result[1].Role = CodeReferenceRole.RAD_DME;
        //		return result;
        //	}
        //	else if (procType == ProcedureTypeConv.VorNdb)
        //	{
        //		if (selectedNavPntsPrj[0].Type == NavType.Vor)
        //			result[0] = CreatePointReference(surface, faclts.Vor.GetFeatureRef(), postFixTolerance, priorFixtolerance);
        //		else
        //			result[0] = CreatePointReference(surface, faclts.Ndb.GetFeatureRef(), postFixTolerance, priorFixtolerance);
        //		result[0].Role = CodeReferenceRole.OTHER_OVERHEAD;
        //		return result;
        //	}
        //	else
        //	{
        //		result[0] = CreatePointReference(surface, faclts.Vor.GetFeatureRef(), postFixTolerance, priorFixtolerance);
        //		result[0].Role = CodeReferenceRole.INTERSECTION;

        //		result[1] = CreatePointReference(surface, faclts.IntersectingVor.GetFeatureRef(), postFixTolerance, priorFixtolerance);
        //		result[1].Role = CodeReferenceRole.INTERSECTION;
        //		return result;
        //	}
        //}

        private PointReference CreatePointReference(Ring toleranceArea, ProcedureTypeConv procType, double postFixTolerance, 
            double priorFixtolerance, DesignatedPoint designatedPoint, 
            EntryDirection entryDirection, double direction, double intersectingDirection, 
            Navaid selectedNavaid, double distanceInGeo)
		{
            PointReference result = new PointReference();
            if (toleranceArea != null)
            {
                Surface surface = new Surface();
                surface.Geo.Add(new Polygon { ExteriorRing = GlobalParams.SpatialRefOperation.ToGeo<Ring>(toleranceArea) });
                result.FixToleranceArea = surface;
            }
            if (!double.IsNaN(postFixTolerance) && !double.IsNaN(priorFixtolerance))
            {
                result.PostFixTolerance = new ValDistanceSigned
                {
                    Uom = _distanceUom,
                    Value = GlobalParams.UnitConverter.DistanceToDisplayUnits(postFixTolerance, eRoundMode.NEAREST)
                };
                result.PriorFixTolerance = new ValDistanceSigned
                {
                    Uom = _distanceUom,
                    Value = GlobalParams.UnitConverter.DistanceToDisplayUnits(priorFixtolerance, eRoundMode.NEAREST)
                };
            }

			if (procType == ProcedureTypeConv.VorNdb)
			{
				result.Role = CodeReferenceRole.OTHER_OVERHEAD;
			}
			else
			{
                var angleIndication = GlobalParams.Database.HoldingQpi.CreateFeature<AngleIndication>();
                angleIndication.Angle = ARANMath.RadToDeg(direction);
                angleIndication.AngleType = CodeBearing.TRUE;
                angleIndication.IndicationDirection = CodeDirectionReference.FROM;
                angleIndication.Fix = designatedPoint.GetFeatureRef();
                angleIndication.PointChoice = new SignificantPoint() { NavaidSystem = selectedNavaid.GetFeatureRef() };
                GlobalParams.Database.HoldingQpi.SetFeature(angleIndication);
                result.FacilityAngle.Add(new AngleUse()
                {
                    TheAngleIndication = angleIndication.GetFeatureRef(),
                    AlongCourseGuidance = true                   
                });
                if (procType == ProcedureTypeConv.Vordme)
				{
                    if(entryDirection == EntryDirection.Toward)   
                        angleIndication.IndicationDirection = CodeDirectionReference.TO;

                    var distanceIndication = GlobalParams.Database.HoldingQpi.CreateFeature<DistanceIndication>();
                    distanceIndication.Distance = new ValDistance(GlobalParams.UnitConverter.DistanceToDisplayUnits(distanceInGeo, eRoundMode.NEAREST), _distanceUom);
                    distanceIndication.Type = CodeDistanceIndication.DME;
                    distanceIndication.PointChoice = new SignificantPoint() { NavaidSystem = selectedNavaid.GetFeatureRef() };
                    distanceIndication.Fix = designatedPoint.GetFeatureRef();
                    GlobalParams.Database.HoldingQpi.SetFeature(distanceIndication);
                    result.FacilityDistance.Add(distanceIndication.GetFeatureRefObject());
                    result.Role = CodeReferenceRole.RAD_DME;
				}
				else 
				{
                    var intersectAngleIndication = new AngleIndication();
                    intersectAngleIndication.Angle = intersectingDirection;
                    intersectAngleIndication.AngleType = CodeBearing.TRUE;
                    intersectAngleIndication.IndicationDirection = CodeDirectionReference.FROM;
                    intersectAngleIndication.Fix = designatedPoint.GetFeatureRef();
                    intersectAngleIndication.PointChoice = new SignificantPoint() { NavaidSystem = selectedNavaid.GetFeatureRef() };
                    GlobalParams.Database.HoldingQpi.SetFeature(intersectAngleIndication);
                    result.FacilityAngle.Add(new AngleUse()
                    {
                        TheAngleIndication = intersectAngleIndication.GetFeatureRef(),
                        AlongCourseGuidance = true
                    });
                    result.Role = CodeReferenceRole.INTERSECTION;
				}
			}
			return result;
		}

		//private Surface CreateToleranceArea ( Ring toleranceArea )
		//{
		//	if ( toleranceArea == null )
		//		return null;
		//	Surface surface = new Surface ( );
		//	Polygon poly = new Polygon {ExteriorRing = GlobalParams.SpatialRefOperation.ToGeo<Ring>(toleranceArea)};
		//	//foreach ( Point pnt in toleranceArea )
		//	//{
		//	//    poly.ExteriorRing.Add ( GlobalParams.SpatialRefOperation.ToGeo<Point> ( pnt ) );
		//	//}
		//	surface.Geo.Add ( poly );
		//	return surface;
		//}
	}
}