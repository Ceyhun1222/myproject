//#define DONT_USE_TRANSACTION

using System;
using System.Linq;
using System.Collections.Generic;
using Aran.Aim.CAWProvider;
using Aran.Aim.Features;
using Aran.Aim;
using System.Windows.Forms;
using Aran.Queries.Viewer;
using Aran.Aim.DataTypes;
using Aran.Queries.Common;

namespace Aran.Queries
{
    public class CommonQPI : ICommonQPI
    {
        protected Dictionary<Guid, Feature> _createdFeatureList;
        protected TemporalTimeslice _timeSlice;
        protected InterpretationType _interpretation;
        protected ICawService _cawService;

        public CommonQPI ()
        {
            _createdFeatureList = new Dictionary<Guid, Feature> ();
        }

        #region ICommonQPI Members

        public ConnectionInfo ConnectionInfo
        {
            get { return _cawService.ConnectionInfo; }
            set
            {
                CawProviderType providerType = CawProviderType.ComSoft;
                
                if (value.Server.IsFile)
                    providerType = CawProviderType.FileBase;

                _cawService = CawProviderFactory.CreateService (providerType);
                _cawService.ConnectionInfo = value;
            }
        }

        public void Close ()
        {
        }

        public void Open (TemporalTimeslice timeSlice, InterpretationType interpretation)
        {
            _timeSlice = timeSlice;
            _interpretation = interpretation;
        }

        public TemporalTimeslice TimeSlice
        {
            get { return _timeSlice; }
        }

        public InterpretationType Interpretation
        {
            get { return _interpretation; }
        }

        public void Commit ()
        {
            Commit (null);
        }

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
#if !DONT_USE_TRANSACTION
                int workPackageId = _cawService.BeginTransaction ();
#endif
                bool save = true;
                Exception insertError = null;
                Exception commitError = null;

                try
                {
                    foreach (Feature feature in featureList)
                    {
						if (feature.FeatureType == FeatureType.InstrumentApproachProcedure)
						{
						}
#if DONT_USE_TRANSACTION
						_cawService.InsertFeature(feature, null);
#else
                        _cawService.InsertFeature (feature, workPackageId);
#endif

                    }
                }
                catch (Exception ex)
                {
                    insertError = ex;
                    save = false;
                }

#if !DONT_USE_TRANSACTION
                try
                {
                    _cawService.Commit (workPackageId, save);
                }
                catch (Exception ex)
                {
                    commitError = ex;
                }
#endif

                if (insertError != null)
                    throw insertError;
                else if (commitError != null)
                    throw commitError;
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

        public List<TFeature> GetFeatureList<TFeature> (Filter filter = null) where TFeature : Feature, new ()
        {
            TFeature tmpFeat = new TFeature ();

            SimpleQuery simpleQuery = new SimpleQuery ();
            simpleQuery.TemproalTimeslice = _timeSlice;
            simpleQuery.FeatureType = tmpFeat.FeatureType;
            simpleQuery.Interpretation = _interpretation;
            simpleQuery.Filter = filter;

            return _cawService.GetFeature<TFeature> (simpleQuery);
        }

        public TFeature GetFeature<TFeature> (Guid identifier) where TFeature : Feature
        {
            FeatureType featureType = (FeatureType) Enum.Parse (typeof (FeatureType), typeof (TFeature).Name);

            SimpleQuery simpleQuery = new SimpleQuery ();
            simpleQuery.TemproalTimeslice = _timeSlice;
            simpleQuery.FeatureType = featureType;
            simpleQuery.Interpretation = _interpretation;

            simpleQuery.IdentifierList.Add (identifier);

            List<TFeature> featureList = _cawService.GetFeature<TFeature> (simpleQuery);

            if (featureList.Count > 0)
                return featureList [0];

            return null;
        }

        public List<TFeature> GetLinkedFeatures<TFeature> (FeatureType linkedType,
            Guid linkIdentifier, string linkPropertyName) where TFeature : Feature, new ()
        {
            FeatureType returnFeatureType = (FeatureType) Enum.Parse (typeof (FeatureType), typeof (TFeature).Name);

            Feature [] featureArr = GetLinkedFeatures (returnFeatureType, linkedType, linkIdentifier, linkPropertyName);

            List<TFeature> list = new List<TFeature> ();
            foreach (Feature feat in featureArr)
            {
                list.Add ((TFeature) feat);
            }
            return list;
        }

        public Feature [] GetLinkedFeatures (FeatureType returnFeatureType, FeatureType linkedType, 
            Guid linkIdentifier, string linkPropertyName)
        {
            LinkQuery lq = new LinkQuery ();
            lq.FeatureTypeList.Add (returnFeatureType);
            lq.TraverseTimeslicePropertyName = linkPropertyName;

            lq.SimpleQuery = new SimpleQuery ();
            lq.SimpleQuery.FeatureType = linkedType;
            lq.SimpleQuery.IdentifierList.Add (linkIdentifier);
            lq.SimpleQuery.TemproalTimeslice = _timeSlice;

            Feature [] featureArr = _cawService.GetFeature (lq);
            return featureArr;
        }

        public Feature GetFeature (FeatureType featureType, Guid identifier)
        {
            SimpleQuery simpleQuery = new SimpleQuery ();
            simpleQuery.TemproalTimeslice = _timeSlice;
            simpleQuery.FeatureType = featureType;
            simpleQuery.Interpretation = _interpretation;

            simpleQuery.IdentifierList.Add (identifier);

            Feature [] featureArr = _cawService.GetFeature (simpleQuery);

            if (featureArr.Length > 0)
                return featureArr [0];

            return null;
        }

        #endregion

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


            List<DesignatedPoint> dpList = GetFeatureList<DesignatedPoint> ();
            
            foreach (var dp in dpList)
            {
                if (dp.Location != null)
                {
                    var dpPoint = dp.Location.Geo;
                    double d = CalculateDesimalDegreeDistance(dpPoint.Y, dpPoint.X, point.Y, point.X);
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
    }
}