using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Features;
using Aran.Converters;
using Aran.Geometries;
using Aran.PANDA.Common;
using Aran.Aim.Enums;
using Aran.PANDA.Constants;

namespace Aran.Omega.Models
{
    public class RwyDirClass
    {
        private Runway _rwy;
        public RwyDirClass(RunwayDirection rwyDir,Runway rwy)
        {
            NativeMethods.InitAll();
            _rwy = rwy;
            RwyDir = rwyDir;
            Validation = new RwyDirValidation();
            Calculate();
            var startPt = GlobalParams.SpatialRefOperation.ToPrj(StartCntlPt.Location.Geo);
            var endPt = GlobalParams.SpatialRefOperation.ToPrj(EndCntlPt.Location.Geo);
            Length = ARANFunctions.ReturnDistanceInMeters(startPt, endPt);
            double inverseAzimuth,directionAzimuth;
            ARANFunctions.ReturnGeodesicAzimuth(StartCntlPt.Location.Geo, EndCntlPt.Location.Geo, out directionAzimuth,
                out inverseAzimuth);
             Aziumuth = directionAzimuth;
        }

        public RunwayDirection RwyDir { get; private set; }

        public string Name { get { return RwyDir.Designator; } }

        public List<RunwayCentrelinePoint> RwyCntrPtList { get; set; }

        public List<RwyCenterlineClass> CenterLineClassList { get; private set; }

        public RunwayCentrelinePoint StartCntlPt { get; set; }

        public RunwayCentrelinePoint EndCntlPt { get; set; }

        public double ClearWay { get; private set; }

        public double StopWay { get; set; }

        public double Length { get; private set; }

        public double TDZElevation { get; set; }

        public double CalculationLength { get;private set; }

        public RwyDirValidation Validation { get; set; }

        public double Aziumuth { get; set; }

        public double Direction { get; set; }

        public RunwayCentrelinePoint ThresHold { get; set; }

        public double Tora { get; set; }
        public double Toda { get; set; }
        public double Asda { get; set; }
        public double LDA { get; set; }

        public RunwayClassificationType SelectedClassification { get; set; }
        public CategoryNumber SelectedCategory { get; set; }
        public int SelectedCodeNumber { get; set; }


        private void Calculate()
        {
            List<RunwayCentrelinePoint> tmpCntlList = GlobalParams.Database.GetRunwayCenterLinePoints(RwyDir.Identifier);
            foreach (RunwayCentrelinePoint cntlnPt in tmpCntlList)
            {
                if (cntlnPt.Role == CodeRunwayPointRole.DISTHR)
                {
                    StartCntlPt = cntlnPt;
                    break;
                }
                else if (cntlnPt.Role == CodeRunwayPointRole.START && StartCntlPt == null)
                    StartCntlPt = cntlnPt;
                else if (cntlnPt.Role == CodeRunwayPointRole.THR)
                    ThresHold = cntlnPt;
                else if (cntlnPt.Role == CodeRunwayPointRole.END)
                    EndCntlPt = cntlnPt;
            }

            if (StartCntlPt == null && ThresHold != null)
                StartCntlPt = ThresHold;
            else if (StartCntlPt != null && StartCntlPt.Role == CodeRunwayPointRole.START)
            {
                if (ThresHold != null)
                {
                    double dx = StartCntlPt.Location.Geo.X - ThresHold.Location.Geo.X;
                    double dy = ThresHold.Location.Geo.Y - ThresHold.Location.Geo.Y;
                    double dis = Math.Sqrt(dx*dx + dy*dy);
                    if (dis*3600 > 0.01)
                        StartCntlPt = ThresHold;
                }
                else
                {
                    Validation.ThresholdNotAvailable = true;
                    Logs += "Threshold not available/r/n";
                    // throw new OmegaException(ExceptionType.Critical, ExceptionMessageType.ThresholdNotAvailable);
                }
            }

            CalculateClearAndStopWay();
            if (StartCntlPt == null) return;
            CalculationLength =Common.ConvertDistance(ARANFunctions.ReturnGeodesicDistance(StartCntlPt.Location.Geo, EndCntlPt.Location.Geo),Enums.RoundType.ToNearest);

            RwyCntrPtList = CheckCntlnPt(tmpCntlList);

            var startPrj = GlobalParams.SpatialRefOperation.ToPrj(StartCntlPt.Location.Geo);
            var endPrj = GlobalParams.SpatialRefOperation.ToPrj(EndCntlPt.Location.Geo);
            Direction = ARANFunctions.ReturnAngleInRadians(startPrj, endPrj);

            CenterLineClassList = new List<RwyCenterlineClass>();
            int i = 0;
            foreach (var rwyCntrPt in RwyCntrPtList)
            {
                //Calculate shift
                var rwyCntrPrj = GlobalParams.SpatialRefOperation.ToPrj(rwyCntrPt.Location.Geo);
                var shiftPt =ARANFunctions.LineLineIntersect(startPrj, Direction, rwyCntrPrj, Direction + Math.PI/2) as Point;
                double shift = ARANFunctions.ReturnDistanceInMeters(shiftPt, rwyCntrPrj);
                
                //it must be 0.25 m-Ilyas m said 28.11.2013
                if (shift > 0.25)
                {
                    Validation.ShiftLogs += rwyCntrPt.Role + " " + rwyCntrPt.Designator + " : "
                                + (int)ARANMath.SideDef(startPrj, Direction, rwyCntrPrj) * Math.Round(Common.ConvertDistance(shift, Enums.RoundType.RealValue), 3) + " " + InitOmega.DistanceConverter.Unit + "<br />";
                }
                //end calculating

                var centerLineClass = new RwyCenterlineClass();
                centerLineClass.PtGeo = rwyCntrPt.Location.Geo;
                centerLineClass.PtPrj = shiftPt;
                centerLineClass.Role = rwyCntrPt.Role;
                centerLineClass.Elevation = ConverterToSI.Convert(rwyCntrPt.Location.Elevation, 0);
                centerLineClass.DistFromThreshold = rwyCntrPt.GetDistance();
               
                if (rwyCntrPt.Role == CodeRunwayPointRole.THR)
                    centerLineClass.Id = "THR" + RwyDir.Designator;
                else if (rwyCntrPt.Role== CodeRunwayPointRole.END)
                    centerLineClass.Id = "End" + RwyDir.Designator;
                else
                {
                    centerLineClass.Id = i.ToString();
                    i++;
                }
                CenterLineClassList.Add(centerLineClass);
            }
        }

        //Check centerlinepoint eyni duzz xetdedi ya yox
        private List<RunwayCentrelinePoint> CheckCntlnPt(IEnumerable<RunwayCentrelinePoint> rwyCntlnPtList)
        {
            try
            {
                var cntlPtDict = new Dictionary<RunwayCentrelinePoint, double>();

                double directAzimuth;
                double inverseAzimuth;
                var startPt = StartCntlPt.Location.Geo;
                var endPt = EndCntlPt.Location.Geo;
                ARANFunctions.ReturnGeodesicAzimuth(startPt, endPt, out directAzimuth, out inverseAzimuth);

                var runwayLength = ARANFunctions.ReturnGeodesicDistance(startPt, endPt);
                foreach (var item in rwyCntlnPtList)
                {
                    if (item.Role == CodeRunwayPointRole.START)
                        continue;
                    if (item.Location == null) continue;
                    double distance = ARANFunctions.ReturnGeodesicDistance(startPt, item.Location.Geo);
                    if (distance <= runwayLength)
                    {
                        double tmpDistanceAzimuth;
                        ARANFunctions.ReturnGeodesicAzimuth(startPt, item.Location.Geo, out tmpDistanceAzimuth, out inverseAzimuth);
                        if ((startPt.Equals2D(item.Location.Geo) ||
                             (Math.Abs(tmpDistanceAzimuth - directAzimuth) < 0.1 ||
                              Math.Abs(tmpDistanceAzimuth) < 0.0001)))
                        {
                            cntlPtDict.Add(item, distance);
                            item.SetDistance(distance);
                        }
                    }
                }

                double tdzLength = 900;
                if (CalculationLength > 2700)
                    tdzLength = CalculationLength / 3;

                var tdzCntln = cntlPtDict.Where(cntln => cntln.Value <= tdzLength).
                                            OrderByDescending(cntln => cntln.Value).FirstOrDefault();

                if (tdzCntln.Key != null)
                    TDZElevation = ConverterToSI.Convert(tdzCntln.Key.Location.Elevation, 0);
                else
                {
                    var tdzad = cntlPtDict.OrderByDescending(cntln=>cntln.Value).Select(a=>a.Key).ToList<RunwayCentrelinePoint>();
                    TDZElevation = ConverterToSI.Convert(tdzad[0].Location.Elevation, 0);
                }

                //sort the list
                var result = (from entry in cntlPtDict orderby entry.Value ascending select entry.Key).
                    ToList<RunwayCentrelinePoint>();

                return result;
            }
            catch (Exception)
            {
                throw new Exception("Error validating RunwayDirection : "+this.Name);
                
            }
        }

        private void CalculateClearAndStopWay()
        {
            RunwayDeclaredDistance toda = null, tora = null, asda = null,lda=null;
            //must change

            ClearWay = 0;
            StopWay = 0;
            //-----------------

            if (StartCntlPt != null)
            {
                foreach (RunwayDeclaredDistance declaredDistance in StartCntlPt.AssociatedDeclaredDistance)
                {
                    if (declaredDistance.Type == CodeDeclaredDistance.TODA)
                        toda = declaredDistance;
                    else if (declaredDistance.Type == CodeDeclaredDistance.TORA)
                        tora = declaredDistance;
                    else if (declaredDistance.Type == CodeDeclaredDistance.ASDA)
                        asda = declaredDistance;
                    else if (declaredDistance.Type == CodeDeclaredDistance.LDA)
                        lda = declaredDistance;
                }
                if (tora == null || tora.DeclaredValue.Count==0)
                {
                    Validation.DeclaredDistanceNotAvailable = true;
                    Logs +="Declared distance not available\r\n";
                    return;
                }


                foreach (var toraDeclareValue in tora.DeclaredValue)
                {
                    if (toraDeclareValue != null)
                    {
                        if (toda != null)
                        {
                            foreach (RunwayDeclaredDistanceValue todaDeclareValue in toda.DeclaredValue)
                            {
                                if (todaDeclareValue != null)
                                {
                                    Toda =
                                        Aran.Converters.ConverterToSI.Convert(todaDeclareValue.Distance, 0);
                                    Tora =
                                        Aran.Converters.ConverterToSI.Convert(toraDeclareValue.Distance, 0);
                                    ClearWay = Toda - Tora;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            Validation.ClearWay = true;
                            //Logs += " Clearway not available\r\n";
                        }
                        if (asda != null)
                        {
                            foreach (RunwayDeclaredDistanceValue asdaDeclaredValue in asda.DeclaredValue)
                            {
                                if (asdaDeclaredValue != null)
                                {
                                    Asda =
                                        Aran.Converters.ConverterToSI.Convert(asdaDeclaredValue.Distance, 0);
                                    StopWay = Asda - Tora;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            Validation.StopWay = true;
                            Logs += " Stopway not available\r\n";
                        }

                        if (lda != null) 
                        {
                            foreach (RunwayDeclaredDistanceValue ldaDeclaredValue in lda.DeclaredValue)
                            {
                                if (ldaDeclaredValue != null)
                                {
                                    LDA =
                                        Aran.Converters.ConverterToSI.Convert(ldaDeclaredValue.Distance, 0);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
      
        public string Logs { get; private set; }

        
    }

    public class RwyDirValidation
    {
        public bool ThresholdNotAvailable { get; set; }
        public bool DeclaredDistanceNotAvailable { get; set; }
        public bool ClearWay { get; set; }
        public bool StopWay { get; set; }
        public string ShiftLogs { get; set; }
    }
}
