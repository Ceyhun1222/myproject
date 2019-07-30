using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Data;
using Aran.Aim.Features;
using Aran.Aim;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Data.Filters;
using System.Xml;
using System.Data;
using Aran.Aim.Utilities;
using Aran.Aim.AixmMessage;

namespace Aran.Aim.Data.XmlProvider
{
    public class XmlDbProvider : DbProvider
    {
        #region Fields

        private List<Feature> _allFeatures;
        private string _fileName;
        private bool _saveOnClose;

        #endregion


        public XmlDbProvider()
        {
            _allFeatures = new List<Feature>();
            _saveOnClose = false;
            State = ConnectionState.Closed;
        }

        public override void Open(string connectionString)
        {
            _fileName = connectionString;
            _allFeatures.Clear();

            if (System.IO.File.Exists(_fileName)) {
                var xmlReader = XmlReader.Create(_fileName);

                var absFeatRefList = new List<AbstractFeatureRefBase>();
                Other.AbstractFeatureRefTypeReading absFeatRefReading = (absFeat) => { absFeatRefList.Add(absFeat); };
                Other.AbstractFeatureRefTypeReadingHandle.Handle = new Other.AbstractFeatureRefTypeReading(absFeatRefReading);

                var aixmBasicMess = new AixmBasicMessage(MessageReceiverType.Panda);
                aixmBasicMess.ReadXml(xmlReader);

                Other.AbstractFeatureRefTypeReadingHandle.Handle = null;

                CurrentUser = new User();
                CurrentUser.Privilege = Privilige.prReadOnly;

                var featTypeDict = new Dictionary<Guid, FeatureType>();

                foreach (var afl in aixmBasicMess.HasMember) {
                    foreach (var feat in afl) {
                        CurrentUser.AddFeatType(feat.FeatureType.ToString());
                        _allFeatures.Add(feat);

                        if (!featTypeDict.ContainsKey(feat.Identifier))
                            featTypeDict.Add(feat.Identifier, feat.FeatureType);
                    }
                }

                foreach (IAbstractFeatureRef afr in absFeatRefList) {
                    FeatureType featType;
                    if (featTypeDict.TryGetValue(afr.Identifier, out featType))
                        afr.FeatureTypeIndex = (int)featType;
                }
            }

            State = ConnectionState.Open;
        }

        public override bool Login(string userName, string md5Password)
        {
            CurrentUser.Name = userName;
            CurrentUser.Password = md5Password;
            return true;
        }

        public override void Close()
        {
            if (!_saveOnClose)
                return;

            _allFeatures.Sort(CompareFeature);

            AixmBasicMessage aixmBasicMess = new AixmBasicMessage(MessageReceiverType.Panda);

            foreach (Feature feat in _allFeatures) {
                AixmFeatureList afl = new AixmFeatureList();
                afl.Add(feat);
                aixmBasicMess.HasMember.Add(afl);
            }

            XmlWriter writer = XmlWriter.Create(_fileName);
            aixmBasicMess.WriteXml(writer);
            writer.Close();

            State = ConnectionState.Closed;
        }

        public override int BeginTransaction()
        {
            return -1;
        }

        public override InsertingResult Insert(Feature feature, bool insertAnyway, bool asCorrection)
        {
            _saveOnClose = true;

            for (int i = 0; i < _allFeatures.Count; i++) {
                if (_allFeatures[i].Identifier == feature.Identifier) {
                    _allFeatures[i] = feature;
                    return new InsertingResult(true);
                }
            }

            _allFeatures.Add(feature);
            return new InsertingResult(true);
        }

        public override InsertingResult Insert(Feature feature, int transactionId, bool insertAnyway, bool asCorrection)
        {
            _saveOnClose = true;

            for (int i = 0; i < _allFeatures.Count; i++) {
                if (_allFeatures[i].Identifier == feature.Identifier) {
                    _allFeatures[i] = feature;
                    return new InsertingResult(true);
                }
            }

            _allFeatures.Add(feature);
            return new InsertingResult(true);
        }

        public override GettingResult GetVersionsOf(FeatureType featType, 
            TimeSliceInterpretationType interpretation, 
            TimePeriod submissionTime, 
            Guid identifier = default(Guid),
            bool loadComplexProps = false,
            TimeSliceFilter timeSlicefilter = null,
            List<string> propList = null,
            Filters.Filter filter = null)
        {
            var result = new GettingResult(false);
            result.List = new List<Feature>();

            if (identifier == Guid.Empty) {
                foreach (Feature feat in _allFeatures) {
                    if (feat.FeatureType == featType &&
                        CheckFilter(feat, filter)) {
                        result.List.Add(feat);
                    }
                }
            }
            else if (identifier != Guid.Empty) {
                foreach (Feature feat in _allFeatures) {
                    if ((featType == 0 || feat.FeatureType == featType) &&
                        feat.Identifier == identifier) {
                        result.List.Add(feat);
                    }
                }
            }

            result.IsSucceed = true;
            return result;
        }

        private int CompareFeature(Feature feat1, Feature feat2)
        {
            return string.Compare(feat1.FeatureType.ToString(), feat2.FeatureType.ToString());
        }

        private bool CheckFilter(Feature feature, Aran.Aim.Data.Filters.Filter filter)
        {
            if (filter == null) return true;

            if (filter.Operation.Choice == Filters.OperationChoiceType.Comparison) {
                var propName = filter.Operation.ComparisonOps.PropertyName;
                var innerPropInfos = AimMetadataUtility.GetInnerProps((int)feature.FeatureType, propName);
                var listVals = AimMetadataUtility.GetInnerPropertyValue(feature, innerPropInfos);
                if (listVals != null && listVals.Count > 0) {
                    FeatureRef featRef = listVals[0] as FeatureRef;
                    if (featRef != null) {
                        return featRef.Identifier == (Guid)filter.Operation.ComparisonOps.Value;
                    }
                }
            }

            //Others not implemented
            return true;
        }

        public override DbProviderType GetProviderType(ref string otherName)
        {
            return DbProviderType.XmlFile;
        }
    }
}
