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
using Aran.Geometries;
using System.Collections;
using Aran.Metadata.Forms.Views;
using Aran.Metadata.Forms.Classes;
using Aran.Metadata.Utils;

namespace Aran.Queries
{
    public class CommonQPI : ICommonQPI
    {
        public CommonQPI()
        {
            _createdFeatureList = new Dictionary<Guid, Feature>();
            _interpretationTypeList = new Dictionary<Guid, bool>();
            _rootFeatureTypes = new List<FeatureType>();
        }

        public void Open(DbProvider dbProvider)
        {
            DbProvider = dbProvider;
        }

        public void Close()
        {
            DbProvider.Close();
        }

        #region ICommonQPI Members

        public event TerrainDataReaderEventHandler TerrainDataReader;

        public TimeSliceInterpretationType Interpretation
        {
            get { return TimeSliceInterpretationType.BASELINE; }
        }

        //public void SetRootFeatureType (FeatureType rootFeatureType)
        //{
        //    _rootFeatureTypes.Clear();
        //    _rootFeatureTypes.Add(rootFeatureType);
        //}

        public void SetRootFeatureType(params FeatureType[] rootFeatureTypes)
        {
            _rootFeatureTypes.Clear();
            foreach (var featType in rootFeatureTypes)
            {
                _rootFeatureTypes.Add(featType);
            }
        }

        public bool Commit (bool showTimePanel)
        {
			return Commit ( null,showTimePanel );
        }

        public bool Commit(FeatureType[] featureTypes, bool showTimePanel, bool sort = true)
        {
            try
            {
                List<Feature> featureList = _createdFeatureList.Values.ToList();

                if (featureTypes != null && sort)
                    SortFeatures(featureList, featureTypes);

                var rootFeatures = GetRootFeatures();

                if (rootFeatures.Count == 0)
                    throw new Exception("Root Feature(s) is(are) not defined!");

                ////featureList.Remove (rootFeature);
                ////featureList.Insert (0, rootFeature);

                ////////Aran.Aim.FeatureInfo.ROFeatureViewer rofv = new Aran.Aim.FeatureInfo.ROFeatureViewer ();
                //////////rofv.SetOwner (Globals.MainForm);
                ////////rofv.GettedFeature += viewer_GetFeature;
                ////////if (!rofv.ShowFeaturesForm (featureList, true, true))
                ////////    return false;

                var viewer = new FeatureViewerForm();
                viewer.DefaultEffectiveDate = DbProvider.DefaultEffectiveDate;
                viewer.GetFeature += new GetFeatureHandler(viewer_GetFeature);
                //viewer.StartOfValidChanged += Viewer_StartOfValidChanged;
                if (showTimePanel)
                {
                    viewer.ValidTimeSelected += Viewer_ValidTimeSelected;
                    viewer.ShowValidTimePanel();
                    viewer.SelectedValidTime =
                        Aran.Controls.Airac.AiracCycle.CreateAiracDateTime(DbProvider.DefaultEffectiveDate);
                }
                viewer.SetFeature(rootFeatures.ToArray());

                if (viewer.ShowDialog() == DialogResult.OK)
                {
                    var ir = DbProvider.Insert(featureList);
                    if (!ir.IsSucceed)
                        throw new Exception(ir.Message);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }

        public virtual bool  Commit (FeatureType [] featureTypes,bool showTimePanel)
        {
            return Commit(featureTypes, showTimePanel, true);
        }

        protected List<Feature> GetRootFeatures()
        {
            List<Feature> rootFeatures = new List<Feature>();

            foreach (Feature feat in _createdFeatureList.Values)
            {
                if (_rootFeatureTypes.Contains(feat.FeatureType))
                    rootFeatures.Add(feat);
            }
            return rootFeatures;
        }

        public bool CommitWithoutViewer(FeatureType[] featureTypes)
        {
            try
            {
                List<Feature> featureList = _createdFeatureList.Values.ToList();

                if (featureTypes != null)
                {
                    SortFeatures(featureList, featureTypes);
                }

                int transId = DbProvider.BeginTransaction();
                
                foreach (var feat in featureList)
                {
                    bool isCorrection = false;
                    if (_interpretationTypeList.ContainsKey(feat.Identifier))
                    {
                        //feat.TimeSlice.CorrectionNumber++;
                        isCorrection = _interpretationTypeList[feat.Identifier];
                    }

                    var insRes = DbProvider.Insert(feat, transId, true, isCorrection);
                    if (!insRes.IsSucceed)
                    {
                        DbProvider.Rollback(transId);
                        throw new Exception(insRes.Message);
                    }
                }

                var insResult = DbProvider.Commit(transId);
                if (!insResult.IsSucceed)
                {
                    throw new Exception(insResult.Message);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;

        }

        public bool CommitWithMetadataViewer(string coordinateSystemReferenceName, List<GeoNumericalDataModel> geoNumericalData, bool showTimePanel)
        {
            return CommitWithMetadataViewer(coordinateSystemReferenceName, null, null, geoNumericalData, showTimePanel);
        }

        public bool CommitWithMetadataViewer(string coordinateSystemReferenceName, FeatureType[] featureTypes, FeatureType[] featureTypesForMetadata, List<GeoNumericalDataModel> geoNumericalData, bool showTimePanel)
        {
            List<Feature> featureList;
            
            if (featureTypesForMetadata != null  && featureTypesForMetadata.Any())
                featureList = _createdFeatureList.Values.Where(feature => featureTypesForMetadata.Contains(feature.FeatureType)).ToList();
            else
                featureList = _createdFeatureList.Values.ToList();

            var metadataBuilder = new MetadataBuilder(featureList, DbProvider.CurrentUser, DbProvider.GetOriginators(), coordinateSystemReferenceName, geoNumericalData);

            metadataBuilder.Build();
            
            return Commit(featureTypes, showTimePanel);
        }

        public void SetFeature(Feature feature,bool asCorrection)
        {
            if (!_createdFeatureList.ContainsKey(feature.Identifier))
                _createdFeatureList.Add(feature.Identifier, feature);

            if (!_interpretationTypeList.ContainsKey(feature.Identifier))
                _interpretationTypeList.Add(feature.Identifier, asCorrection);
        }

        public TFeature CreateFeature<TFeature> () where TFeature : Feature, new ()
        {
            TFeature feature = new TFeature ();
            feature.Id = -1;
            feature.Identifier = Guid.NewGuid ();
            feature.TimeSlice = new Aim.DataTypes.TimeSlice ();
            feature.TimeSlice.Interpretation = Aim.Enums.TimeSliceInterpretationType.PERMDELTA;
            feature.TimeSlice.ValidTime = new Aim.DataTypes.TimePeriod (DbProvider.DefaultEffectiveDate);
            feature.TimeSlice.FeatureLifetime = feature.TimeSlice.ValidTime;
            feature.TimeSlice.SequenceNumber = 1;
            feature.TimeSlice.CorrectionNumber = 0;
			
            _createdFeatureList.Add (feature.Identifier, feature);

            return feature;
        }

        public void ExcludeFeature (Guid identifier)
        {
            if (_createdFeatureList.ContainsKey(identifier))
                _createdFeatureList.Remove(identifier);

            if (_interpretationTypeList.ContainsKey(identifier))
                _interpretationTypeList.Remove(identifier);
        }

        public void SetFeature (Feature feature)
        {
            if (!_createdFeatureList.ContainsKey(feature.Identifier))
                _createdFeatureList.Add(feature.Identifier, feature);
        }

        public virtual IList GetFeatureList(FeatureType featureType,Filter filter = null)
        {
            GettingResult gettingResult = DbProvider.GetVersionsOf(featureType, TimeSliceInterpretationType.BASELINE, default(Guid), true, null, null, filter);
            if (gettingResult.IsSucceed && gettingResult.List.Count > 0)
            {
                return gettingResult.List;
            }
            return null;
        }

        public virtual Feature GetFeature(FeatureType featureType, Guid identifier)
        {
            
            GettingResult gettingResult = DbProvider.GetVersionsOf(featureType, Interpretation, identifier, true, null, null, null);
            if (gettingResult.IsSucceed && gettingResult.List.Count > 0)
            {
                return gettingResult.List[0] as Feature;
            }
            return null;
        }

        public virtual Feature GetAbstractFeature(IAbstractFeatureRef abstractFeatureRef)
        {
            GettingResult gettingResult = DbProvider.GetVersionsOf(
                abstractFeatureRef, 
                Interpretation,
                true, null, null, null);
            
            if (gettingResult.IsSucceed && gettingResult.List.Count > 0)
            {
                return gettingResult.List[0] as Feature;
            }
            
            return null;
        }

        public TimeSliceFilter TimeSlice
        {
            get
            {
                return new TimeSliceFilter((DbProvider != null ?
                    DbProvider.DefaultEffectiveDate : DateTime.Now));
            }
        }

        public void ClearAllFeatures()
        {
            _createdFeatureList.Clear();
            _interpretationTypeList.Clear();
        }

        #endregion

        public List<TFeature> GetFeatureList<TFeature>(Filter filter) where TFeature : Feature, new()
        {
            var tmpFeat = new TFeature();

            var gr = DbProvider.GetVersionsOf(tmpFeat.FeatureType, Interpretation, Guid.Empty, true, null, null, filter);

            if (!gr.IsSucceed)
                throw new Exception(gr.Message);

            return gr.GetListAs<TFeature>();
        }

        public TFeature GetFeature<TFeature>(Guid identifier) where TFeature : Feature, new()
        {
            var tmpFeat = new TFeature();

            var gr = DbProvider.GetVersionsOf(tmpFeat.FeatureType, Interpretation, identifier);

            if (!gr.IsSucceed)
                throw new Exception(gr.Message);

            if (gr.List.Count == 0)
                return null;

            return gr.List[0] as TFeature;
        }

        protected DbProvider DbProvider { get; private set; }

        protected List<VerticalStructure> DoTerrainDataReader(MultiPolygon polygon)
        {
            if (TerrainDataReader == null)
                return null;

            var tdrEventArgs = new TerrainDataReaderEventArgs(polygon);
            TerrainDataReader(this, tdrEventArgs);
            return tdrEventArgs.Result;
        }

        protected List<VerticalStructure> DoTerrainDataReader(Aran.Geometries.Point ptCenter, double distance)
        {
            if (TerrainDataReader == null)
                return null;

            var tdrEventArgs = new TerrainDataReaderEventArgs(ptCenter, distance);
            TerrainDataReader(this, tdrEventArgs);
            return tdrEventArgs.Result;
        }

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

		private void Viewer_StartOfValidChanged(object sender, FeatureEventArgs e)
		{
			if (_rootFeatureTypes.Contains(e.Feature.FeatureType))
			{
				foreach (Feature feat in _createdFeatureList.Values)
				{
					if (feat.TimeSlice != null && feat.TimeSlice.ValidTime != null)
						feat.TimeSlice.ValidTime.BeginPosition = e.Feature.TimeSlice.ValidTime.BeginPosition;
				}
			}
		}

        private void Viewer_ValidTimeSelected(object sender, EventArgs e)
        {
            var viewer = sender as FeatureViewerForm;
            var dt = viewer.SelectedValidTime.Value;

            foreach (Feature feat in _createdFeatureList.Values) {
                feat.TimeSlice.ValidTime.BeginPosition = dt;
                feat.TimeSlice.FeatureLifetime.BeginPosition = dt;
            }
        }

        public List<FeatureReport> GetFeatureReport(Guid identifier)
        {
            return DbProvider.GetFeatureReport(identifier);
        }

        public void SetFeatureReport(FeatureReport report)
        {
            DbProvider.SetFeatureReport(report);
        }

        public List<Screenshot> GetScreenshots(Guid identifier)
        {
            return DbProvider.GetFeatureScreenshot(identifier);
        }

        public void SetScreenshot(Screenshot screenshot)
        {
            DbProvider.SetFeatureScreenshot(screenshot);
        }

        protected Dictionary<Guid, Feature> _createdFeatureList;
        protected Dictionary<Guid, bool> _interpretationTypeList;
        private List<FeatureType> _rootFeatureTypes;        
    }
}