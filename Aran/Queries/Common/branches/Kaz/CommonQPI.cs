//#define DONT_USE_TRANSACTION

using System;
using System.Linq;
using System.Collections.Generic;
using Aran.Aim.Features;
using Aran.Aim;
using System.Windows.Forms;
using Aran.Queries.Viewer;
using Aran.Aim.DataTypes;
using Aran.Queries.Common;
using Aran.Aim.DB;
using Aran.Aim.Data.Filters;
using Aran.Aim.Data;
using Aran.Aim.Enums;


namespace Aran.Queries
{
    public class CommonQPI : ICommonQPI
    {
        public CommonQPI()
        {
            _createdFeatureList = new Dictionary<Guid, Feature>();
            _timeSliceFilter = new TimeSliceFilter(DateTime.Now);
            _interpretation = TimeSliceInterpretationType.BASELINE;
        }

        public void Open(string connectionString)
        {
            DbProvider = PgProviderFactory.Create();
			string errorString;
			DbProvider.Open ( connectionString, out errorString );
			//DbProvider.TimeSliceFilter = _timeSliceFilter;
        }

        public void Open(IDbProvider dbProvider)
        {
            DbProvider = dbProvider;
            
        }

        public void Close()
        {
            DbProvider.Close();
        }

        #region ICommonQPI Members

        public TimeSliceInterpretationType Interpretation
        {
            get { return _interpretation; }
        }

        public void Commit ()
        {
            Commit (null);
        }

        /*
        public void Commit (FeatureType [] featureTypes)
        {
            List<Feature> featureList = _createdFeatureList.Values.ToList ();

            if (featureTypes != null)
            {
                SortFeatures (featureList, featureTypes);
            }

            Procedure proc = null;

            foreach (Feature feat in featureList)
            {
                if (feat.FeatureType == FeatureType.InstrumentApproachProcedure ||
                    feat.FeatureType == FeatureType.StandardInstrumentArrival ||
                    feat.FeatureType == FeatureType.StandardInstrumentDeparture)
                {
                    proc = feat as Procedure;
                    break;
                }
            }

            if (proc == null)
            {
                throw new Exception ("Procedure not exists in insterted features list");
            }

            #region Check Existing DesignatedPoint coordinate.

            List<Guid []> dsgPointIdenList = new List<Guid []> ();

            foreach (Feature feat in featureList)
            {
                if (feat.FeatureType == FeatureType.DesignatedPoint)
                {
                    DesignatedPoint dp = GetDesignatedPointByCoord ((feat as DesignatedPoint).Location.Geo);
                    if (dp != null)
                    {
                        dsgPointIdenList.Add (new Guid [] { feat.Identifier, dp.Identifier });
                    }
                }
            }

            foreach (Guid [] oldNewGuids in dsgPointIdenList)
            {
                foreach (Feature feat in featureList)
                {
                    if (feat is SegmentLeg)
                    {
                        SegmentLeg sg = (SegmentLeg) feat;
                        if (IsRefTerminalSegmentPoint (sg.StartPoint, oldNewGuids [0]))
                        {
                            sg.StartPoint.PointChoice.FixDesignatedPoint.Identifier = oldNewGuids [1];
                        }
                        if (IsRefTerminalSegmentPoint (sg.EndPoint, oldNewGuids [0]))
                        {
                            sg.EndPoint.PointChoice.FixDesignatedPoint.Identifier = oldNewGuids [1];
                        }
                    }
                }
            }

            #endregion

            FeatureViewerForm viewer = new FeatureViewerForm ();
            viewer.GetFeature += new GetFeatureHandler (viewer_GetFeature);
            viewer.SetFeature (proc);

            if (viewer.ShowDialog () != DialogResult.OK)
                return;

            try
            {
                string insertError = "";
                try
                {
                    foreach (Feature feature in featureList)
                    {
                        if (feature.FeatureType == FeatureType.InstrumentApproachProcedure)
                        {
                        }
                        InsertingResult insertResult = DbProvider.Insert (feature, true, false);
                        if (!insertResult.IsSucceed)
                            insertError = insertResult.Message;
                        //_cawService.InsertFeature(feature, workPackageId);


                    }
                }
                catch (Exception ex)
                {
                    insertError = ex.Message;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
         */

        public void Commit (FeatureType [] featureTypes)
        {
            try
            {
                List<Feature> featureList = _createdFeatureList.Values.ToList();

                if (featureTypes != null)
                {
                    SortFeatures(featureList, featureTypes);
                }

                HoldingAssessment hAssesment = null;

                foreach (Feature feat in _createdFeatureList.Values)
                {
                    if (feat.FeatureType == FeatureType.HoldingAssessment)
                        hAssesment = feat as HoldingAssessment;
                }

                FeatureViewerForm viewer = new FeatureViewerForm();
                viewer.GetFeature += new GetFeatureHandler(viewer_GetFeature);
                viewer.SetFeature(hAssesment);

                if (viewer.ShowDialog() != DialogResult.OK)
                    return;
                else
                {
                    foreach(Feature feat in _createdFeatureList.Values)
                    {
                        if (feat != null)
                            //DbProvider.Insert(feat, true, false);
							DbProvider.Insert ( feat);
                    }

                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
              
        }

        public TFeature CreateFeature<TFeature> () where TFeature : Feature, new ()
        {
            TFeature feature = new TFeature ();
            feature.Id = -1;
            feature.Identifier = Guid.NewGuid ();
            feature.TimeSlice = new Aim.DataTypes.TimeSlice ();
            feature.TimeSlice.Interpretation = Aim.Enums.TimeSliceInterpretationType.PERMDELTA;
            feature.TimeSlice.ValidTime = new Aim.DataTypes.TimePeriod (DateTime.Now);
            feature.TimeSlice.FeatureLifetime = feature.TimeSlice.ValidTime;
            feature.TimeSlice.SequenceNumber = 1;
            feature.TimeSlice.CorrectionNumber = 0;

            _createdFeatureList.Add (feature.Identifier, feature);

            return feature;
        }

        public void ExcludeFeature (Guid identifier)
        {
            if (_createdFeatureList.ContainsKey (identifier))
                _createdFeatureList.Remove (identifier);
        }

        public void SetFeature (Feature feature)
        {
            _createdFeatureList.Add (feature.Identifier, feature);
        }

        public System.Collections.IList GetFeatureList(FeatureType featureType,Filter filter = null)
        {
            //GettingResult gettingResult = DbProvider.GetVersionsOf(featureType, TimeSliceInterpretationType.BASELINE, default(Guid), true, null, null, null);
			GettingResult gettingResult = DbProvider.GetFeat ( featureType, default ( Guid ), true, null, null );
            if (gettingResult.IsSucceed && gettingResult.List.Count > 0)
            {
                return gettingResult.List;
            }
            return null;
        }

        public Feature GetFeature(FeatureType featureType, Guid identifier)
        {
            //GettingResult gettingResult = DbProvider.GetVersionsOf(featureType, _interpretation, identifier, true, null, null, null);
			GettingResult gettingResult = DbProvider.GetFeat( featureType, identifier, true, null, null );
            if (gettingResult.IsSucceed && gettingResult.List.Count > 0)
            {
                return gettingResult.List[0] as Feature;
            }
            return null;
        }

        public Feature GetAbstractFeature(IAbstractFeatureRef abstractFeatureRef)
        {
			//GettingResult gettingResult = DbProvider.GetVersionsOf(
			//    abstractFeatureRef, 
			//    _interpretation,
			//    true, null, null, null);
			GettingResult gettingResult = DbProvider.GetFeat (
				abstractFeatureRef,
				true, null, null );
            

            if (gettingResult.IsSucceed && gettingResult.List.Count > 0)
            {
                return gettingResult.List[0] as Feature;
            }
            
            return null;
        }

        public TimeSliceFilter TimeSlice
        {
            get { return _timeSliceFilter; }
        }

        #endregion

        protected IDbProvider DbProvider { get; private set; }

        private DesignatedPoint GetDesignatedPointByCoord (Geometries.Point point)
        {
            //Geometries.Point leftLowerCorner = null;
            //Geometries.Point rightUpperCorner = null;

            //BBox box = new BBox ();
            //box.PropertyName = "location";
            //box.Envelope = new Geometries.Box ();
            //box.Envelope [0] = leftLowerCorner;
            //box.Envelope [0] = rightUpperCorner;

            //Filter filter = new Filter ();
            //filter.Operation = new OperationChoice (box);

            //return null;


            //List<DesignatedPoint> dpList = new List<DesignatedPoint>(
            //      new ListAdapter<DesignatedPoint>(GetFeatureList(FeatureType.DesignatedPoint,null)));
            
            //foreach (var dp in dpList)
            //{
            //    if (dp.Location != null)
            //    {
            //        var dpPoint = dp.Location.Geo;
            //        double d = CalculateDesimalDegreeDistance(dpPoint.Y, dpPoint.X, point.Y, point.X);
            //        if (d < 5)
            //        {
            //            return dp;
            //        }
            //    }
            //}

            //return null;

            System.Collections.IList dpList = GetFeatureList (FeatureType.DesignatedPoint, null);

            foreach (DesignatedPoint dp in dpList)
            {
                if (dp.Location != null)
                {
                    var dpPoint = dp.Location.Geo;
                    double d = CalculateDesimalDegreeDistance (dpPoint.Y, dpPoint.X, point.Y, point.X);
                    if (d < 5)
                    {
                        return dp;
                    }
                }
            }

            return null;
        }

        private static double CalculateDesimalDegreeDistance (double lat1, double lon1, double lat2, double lon2)
        {
            double r = 6371100;  // M

            return
                (r * Math.Acos (Math.Sin (lat1 / 57.2958) * Math.Sin (lat2 / 57.2958) +
                Math.Cos (lat1 / 57.2958) * Math.Cos (lat2 / 57.2958) * Math.Cos (lon2 / 57.2958 - lon1 / 57.2958)));
        }

        private bool IsRefTerminalSegmentPoint (TerminalSegmentPoint terminalSegmentPoint, Guid guid)
        {
            if (terminalSegmentPoint == null)
                return false;
            if (terminalSegmentPoint.PointChoice == null)
                return false;
            if (terminalSegmentPoint.PointChoice.Choice != SignificantPointChoice.DesignatedPoint)
                return false;
            if (terminalSegmentPoint.PointChoice.FixDesignatedPoint.Identifier != guid)
                return false;

            return true;
        }

        private Feature viewer_GetFeature (FeatureType featureType, Guid identifier)
        {
            foreach (Feature feat in _createdFeatureList.Values)
            {
                if (feat.FeatureType == featureType && feat.Identifier == identifier)
                    return feat;
            }

            return GetFeature (featureType, identifier);
        }

        private void SortFeatures (List<Feature> featureList, FeatureType [] featureTypes)
        {
            List<FeatureType> ftList = new List<FeatureType> (featureTypes);

            featureList.Sort (delegate (Feature feat1, Feature feat2)
            {
                return ftList.IndexOf (feat1.FeatureType) - ftList.IndexOf (feat2.FeatureType);
            });
        }

        protected Dictionary<Guid, Feature> _createdFeatureList;
        private TimeSliceFilter _timeSliceFilter;
        private TimeSliceInterpretationType _interpretation;
    }
}