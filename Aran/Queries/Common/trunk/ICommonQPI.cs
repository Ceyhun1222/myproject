using System;
using System.Collections.Generic;
using Aran.Aim.DB;
using Aran.Aim.Data.Filters;
using Aran.Aim.Data;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Aim;
using System.Collections;
using Aran.Aim.DataTypes;
using Aran.Geometries;
using Aran.Metadata.Utils;

namespace Aran.Queries
{
    public interface ICommonQPI
    {
        void Close();
        void Open(DbProvider dbProvider);

        event TerrainDataReaderEventHandler TerrainDataReader;

        TimeSliceFilter TimeSlice { get; }
        TimeSliceInterpretationType Interpretation { get; }

        //void SetRootFeatureType (FeatureType rootFeatureType);
        void SetRootFeatureType(params FeatureType[] rootFeatureType);

        bool Commit(FeatureType[] featureTypes, bool showFetureForm = false, bool sort = true);
        bool Commit(bool showTimePanel = false);

        bool CommitWithMetadataViewer(string coordinateSystemReferenceName,
            List<GeoNumericalDataModel> geoNumericalData, bool showTimePanel = false);

        bool CommitWithMetadataViewer(string coordinateSystemReferenceName, FeatureType[] featureTypes, FeatureType[] featureTypesForMetadata,
            List<GeoNumericalDataModel> geoNumericalData, bool showTimePanel = false);

        bool CommitWithoutViewer(FeatureType[] featureTypes);

        void ExcludeFeature(Guid identifier);
        void SetFeature(Feature feature);
        void SetFeature(Feature feature, bool asCorrection);
        TFeature CreateFeature<TFeature>() where TFeature : Feature, new();

        IList GetFeatureList(FeatureType featureType, Filter filter = null);
        Feature GetFeature(FeatureType featureType, Guid identifier);
        Feature GetAbstractFeature(IAbstractFeatureRef abstractFeatureRef);

        List<FeatureReport> GetFeatureReport(Guid identifier);

        void SetFeatureReport(FeatureReport report);

        List<Screenshot> GetScreenshots(Guid identifier);

        void SetScreenshot(Screenshot screenshot);

        void ClearAllFeatures();
    }

    public delegate void TerrainDataReaderEventHandler(object sender, TerrainDataReaderEventArgs e);

    public class TerrainDataReaderEventArgs : EventArgs
    {
        public TerrainDataReaderEventArgs(MultiPolygon filterPolygon)
            : this()
        {
            FilterPolygon = filterPolygon;
        }

        public TerrainDataReaderEventArgs(Point centrePoint, double distance)
            : this()
        {
            CentrePoint = centrePoint;
            Distance = distance;
        }

        private TerrainDataReaderEventArgs()
        {
            Result = new List<VerticalStructure>();
            Errors = new List<string>();
        }

        public MultiPolygon FilterPolygon { get; private set; }

        public Point CentrePoint { get; private set; }

        public double Distance { get; private set; }

        public List<VerticalStructure> Result { get; private set; }

        public List<string> Errors { get; private set; }
    }
}
