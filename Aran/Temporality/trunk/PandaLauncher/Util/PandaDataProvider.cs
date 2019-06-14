using System;
using System.Collections.Generic;
using System.Linq;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.Converters;
using Aran.Geometries;
using Aran.Temporality.Common.ArcGis;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Id;
using Aran.Temporality.CommonUtil.Context;

namespace PandaLauncher.Util
{
    public class PandaDataProvider : AbstractPandaDataProvider
    {
        #region Overrides of AbstractPandaDataProvider

        public override DateTime GetActualDate()
        {
            if (CurrentDataContext.CurrentUser == null) return default(DateTime);
            if (CurrentDataContext.CurrentUser.ActivePrivateSlot == null) return default(DateTime);
            return CurrentDataContext.CurrentUser.ActivePrivateSlot.PublicSlot.EffectiveDate;
        }

        public override bool CommitWithoutViewer(FeatureType[] featureTypes)
        {
            throw new NotImplementedException();
        }

        public override bool Commit()
        {
            throw new NotImplementedException();
        }

        public override bool Commit(FeatureType[] featureTypes)
        {
            throw new NotImplementedException();
        }

        public override Feature GetFeature(FeatureType featureType, Guid identifier)
        {
            var aimFeature=CurrentDataContext.CurrentService.GetActualDataByDate(Interpretation.BaseLine,
                                                                 new FeatureId
                                                                 {
                                                                     FeatureTypeId = (int)featureType,
                                                                     Guid = identifier
                                                                 }, false, GetActualDate()).FirstOrDefault();
            if (aimFeature == null) return null;
            return aimFeature.Data.Feature;
        }

        public override List<Feature> GetList(FeatureType featureType)
        {
            return CurrentDataContext.CurrentService.GetActualDataByDate(
                Interpretation.BaseLine,
                new FeatureId
                    {
                        FeatureTypeId = (int) featureType
                    }, false, GetActualDate()).
                Select(t => t.Data.Feature).ToList();
        }

        public override List<Feature> GetList(FeatureType featureType, MultiPolygon polygon)
        {
            var states = CurrentDataContext.CurrentService.GetActualDataByDate(
                     Interpretation.BaseLine,
                     new FeatureId { FeatureTypeId = (int)featureType },
                     false,
                     GetActualDate()
                     ).Select(t=>t.Data);

            var esriGeometry = ConvertToEsriGeom.FromMultiPolygon(polygon);

            return (from feature2 in states where GeometryFormatter.HasIntersection(esriGeometry, feature2) 
                    select feature2.Feature).ToList();
        }

        #endregion
    }
}
