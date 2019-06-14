using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Aran.Aim.Features;
using Aran.Queries;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
namespace Holding.HoldingSave
{
   public class SegmentPointModel:INotifyPropertyChanged
   {
       #region :>Fields
       private ModelPBN _modelPBN;
       private ModelPointChoise _modelPointChoise;
       private double _priorFixTolerance;

       #endregion

       #region :>Ctor
       public SegmentPointModel(ModelPBN modelPBN,ModelPointChoise modelPointChoise,ARAN.GeometryClasses.Ring toleranceArea)
       {
           _modelPBN = modelPBN;
           _modelPointChoise = modelPointChoise;
           
           _priorFixTolerance = _modelPointChoise.ATT;

           WayPointModel DesignatedPointModel = new WayPointModel(modelPBN, modelPointChoise);
           if (modelPBN.CurFlightPhase.FlightPhaseName == InitHolding.FlightPhaseValue[flightPhase.Enroute])
               SegmentPt = new EnRouteSegmentPoint();
           else
               SegmentPt = new TerminalSegmentPoint();

           SegmentPt.PointChoice = DesignatedPointModel.WayPoint;
           
          // SegmentPt.FacilityMakeup
           PointReference ptReference   = new PointReference();
           ptReference.FixToleranceArea = CreateToleranceArea(toleranceArea);
           ptReference.Point = DesignatedPointModel.WayPoint.GetFeatureRef();
           ptReference.PostFixTolerance = new ValDistanceSigned();
           if (InitHolding.DistanceUnit == ARAN.Contracts.Settings.HorisontalDistanceUnit.hduKM)
           {
               ptReference.PostFixTolerance.Uom = UomDistance.KM;
               ptReference.PostFixTolerance.Value = (modelPointChoise.ATT * 0.001);
           }
           else
               if (InitHolding.DistanceUnit == ARAN.Contracts.Settings.HorisontalDistanceUnit.hduMeter)
               {
                   ptReference.PostFixTolerance.Uom = UomDistance.M;
                   ptReference.PostFixTolerance.Value = modelPointChoise.ATT;
               }
               else
                   if (InitHolding.DistanceUnit == ARAN.Contracts.Settings.HorisontalDistanceUnit.hduNM)
                   {
                       ptReference.PostFixTolerance.Uom = UomDistance.NM;
                       ptReference.PostFixTolerance.Value = (modelPointChoise.ATT * (1.0 / 1852.0)); 
                   }

           ptReference.PriorFixTolerance = ptReference.PostFixTolerance;
           SegmentPt.FacilityMakeup.Add(ptReference);

           SegmentPt.FlyOver = true;
           SegmentPt.RadarGuidance = false;
           SegmentPt.ReportingATC = CodeATCReporting.COMPULSORY;
           SegmentPt.Waypoint = true;
       }
       #endregion

       #region :>Propertry

       public WayPointModel DesignatedPointModel { get; set; }
       
       public Delib.Classes.Codes.YesNoType? CurRadarGuidance 
       {
           get { return SegmentPt.radarGuidance; }
           set
           {
               SegmentPt.radarGuidance = value;
               if (PropertyChanged != null)
                   PropertyChanged(this, new PropertyChangedEventArgs("WayPoint"));
           }
       }


       public Delib.Classes.Codes.ReferenceRoleType? RoleType
       {
           get { return SegmentPt.facilityMakeUp.role; }
           set
           {  
               SegmentPt.facilityMakeUp.role = value;
               if (PropertyChanged != null)
                   PropertyChanged(this, new PropertyChangedEventArgs("WayPoint"));
           }
       }

       public Delib.Classes.Codes.YesNoType? WayPoint
       {
           get { return SegmentPt.waypoint; }
           set
           {
               SegmentPt.waypoint = value;
               if (PropertyChanged != null)
                   PropertyChanged(this, new PropertyChangedEventArgs("WayPoint"));
           }
       }


       public Delib.Classes.Codes.ATCReportingType? CurATCReportingType
       {
           get { return SegmentPt.reportingATC; }
           set
           {
               SegmentPt.reportingATC = value;
               if (PropertyChanged != null)
                   PropertyChanged(this, new PropertyChangedEventArgs("WayPoint"));
           }
       }

       public Delib.Classes.Codes.YesNoType? CurFlyOverType
       {
           get { return SegmentPt.flyOver; }
           set
           {
               SegmentPt.flyOver = value;
               if (PropertyChanged != null)
                   PropertyChanged(this, new PropertyChangedEventArgs("WayPoint"));
           }
       }

       public double PriorFixTolerance 
       {
           get { return Math.Round(Common.ConvertDistance(_priorFixTolerance, roundType.toNearest),2); }
           set
           {
               _priorFixTolerance = Common.DeConvertDistance(value);
           }
       }
       public SegmentPoint SegmentPt { get; set; }


#endregion

       private Surface CreateToleranceArea(ARAN.GeometryClasses.Ring toleranceArea)
       {
           if (toleranceArea == null)
               return null;
           
           LinearRing linearRing = new LinearRing();


           foreach (ARAN.GeometryClasses.Point pt in toleranceArea)
           {
               ARAN.GeometryClasses.Point ptGeo = GlobalParams.SpatialRefOperation.PrjToGeo(pt);
               linearRing.pointList.Add(new Delib.Classes.GeomObjects.Point(ptGeo.X, ptGeo.Y, ptGeo.Z));
           }

           SurfacePatch surfacePatch = new PolygonPatch(linearRing);

           Surface fixtoleranceArea = new Surface();
           fixtoleranceArea.surfacePatchList.Add(surfacePatch);
           return fixtoleranceArea;
       }




       public event PropertyChangedEventHandler PropertyChanged;
   }
}
