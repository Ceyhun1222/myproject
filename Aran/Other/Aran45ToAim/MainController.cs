using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Features;
using Aran.Aim;

namespace Aran45ToAixm
{
    public class MainController
    {
        public event EventHandler FeatureInserted;

        
        public bool ContinureIfError { get; set; }

        public List<string> InsertFeatures (List<Aran.Aim.Features.Feature> featureList, out int insertedCount)
        {
            var errorList = new List<string> ();
            insertedCount = 0;

            int oldCount = featureList.Count;

            featureList = CheckFeatures (featureList);

            int newCount = featureList.Count;

            if (newCount < oldCount)
            {
                errorList.Add (string.Format ("{0} Feature(s) is (are) same.", oldCount - newCount));
            }


            if (featureList.Count == 0)
                return errorList;

            int transId = -1;

            if (!ContinureIfError)
                transId = Global.DbProvider.BeginTransaction ();

            try
            {
                foreach (var feature in featureList)
                {
                    if (ContinureIfError)
                        transId = Global.DbProvider.BeginTransaction ();

                    if (feature.FeatureType == FeatureType.RouteSegment)
                    {
                        var rs = feature as RouteSegment;
                        if (rs.TrueTrack == 6.1)
                        {
                        }
                    }

                    var res = Global.DbProvider.Insert (feature, transId);

                    if (FeatureInserted != null)
                        FeatureInserted (this, null);
                    
                    if (res.IsSucceed)
                    {
                        insertedCount++;
                        
                        if (ContinureIfError)
                            Global.DbProvider.Commit (transId);
                    }
                    else
                    {
                        if (ContinureIfError)
                            Global.DbProvider.Rollback (transId);

                        if (res.Message.Length > 1000)
                        {
                            var ind = res.Message.IndexOfAny (new char [] {'\n', '\r'} );
                            if (ind > 0)
                                errorList.Add (res.Message.Substring (0, ind));
                        }
                        else
                            errorList.Add (res.Message);
                    }
                }

                if (!ContinureIfError)
                    Global.DbProvider.Commit (transId);
            }
            catch (Exception ex)
            {
                if (!ContinureIfError)
                    Global.DbProvider.Rollback (transId);
                throw ex;
            }

            return errorList;
        }

        public List<Feature> CheckFeatures (List<Feature> featureList)
        {
            if (featureList.Count == 0)
                return new List<Feature> ();

            List<Feature> dbList = Global.DbProvider.GetAllFeatuers (featureList [0].FeatureType);
            return CheckFeatures (featureList, dbList);
        }

        
        private List<Feature> CheckFeatures (List<Feature> feature1List, List<Feature> feature2List)
        {
            var list = new List<Feature> ();

            foreach (var feat1 in feature1List)
            {
                bool isSame = false;
                foreach (var feat2 in feature2List)
                {
                    if (IsFeatureSame (feat1, feat2))
                    {
                        isSame = true;
                        break;
                    }
                }
                if (!isSame)
                    list.Add (feat1);
            }

            return list;
        }

        private bool IsFeatureSame (Feature feat1, Feature feat2)
        {
            if (feat1.FeatureType == Aran.Aim.FeatureType.Airspace)
                return IsAirspaceSame (feat1 as Airspace, feat2 as Airspace);
            return false;
        }

        private bool IsAirspaceSame (Airspace airsp1, Airspace airsp2)
        {
            if (airsp1.Designator == airsp2.Designator)
                return true;

            return false;
        }

        private bool IsDesignatedPointSame (DesignatedPoint dp1, DesignatedPoint dp2)
        {
            if (dp1.Designator == dp2.Designator)
                return true;

            return false;
        }
    }
}
