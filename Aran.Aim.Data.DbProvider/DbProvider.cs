using Aran.Aim.Features;
using System.Collections.Generic;
using Aran.Aim.Data.Filters;
using System;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using System.Data;
using System.Linq;

namespace Aran.Aim.Data
{
    public abstract class DbProvider
    {
        #region Fields

        private bool _useChache;
        
        #endregion

        #region Events

        public event EventHandler UseCacheChanged;

        #endregion

        public abstract DbProviderType GetProviderType(ref string otherName);

        public DbProviderType ProviderType {
            get
            {
                var tmp = "";
                return GetProviderType(ref tmp);
            }
        }

        public virtual List<string> GetConnectionStrings()
        {
            return new List<string>();
        }

        public abstract void Open(string connectionString);

        public abstract bool Login(string userName, string md5Password);

        public virtual List<User> GetOriginators()
        {
            return new List<User> {CurrentUser};
        }

        public abstract void Close();

        public virtual ConnectionState State { get; protected set; }

        public abstract int BeginTransaction();


        #region Insert

        public virtual InsertingResult Insert(IEnumerable<Feature> features, int transactionId = -1)
        {
            var isTransCreated = (transactionId == -1);

            if (isTransCreated)
                transactionId = BeginTransaction();

            foreach (var feat in features)
            {
                var notNew = (feat.Id > 0);
                var ir = Insert(feat, transactionId, true, notNew);

                if (!ir.IsSucceed)
                {
                    if (isTransCreated)
                        Rollback(transactionId);

                    return ir;
                }
            }

            if (isTransCreated)
                return Commit(transactionId);

            return new InsertingResult();
        }

        public virtual InsertingResult Insert(Feature feature)
        {
            return Insert(feature, false, false);
        }

        public virtual InsertingResult Insert(Feature feature, bool insertAnyway)
        {
            return Insert(feature, insertAnyway, false);
        }

        public abstract InsertingResult Insert(Feature feature, bool insertAnyway, bool asCorrection);

        public virtual InsertingResult Insert(Feature feature, int transactionId)
        {
            return Insert(feature, transactionId, false, false);
        }

        public virtual InsertingResult Insert(Feature feature, int transactionId, bool insertAnyway)
        {
            return Insert(feature, transactionId, insertAnyway, false);
        }

        public abstract InsertingResult Insert(Feature feature, int transactionId, bool insertAnyway, bool asCorrection);

        #endregion

        public virtual InsertingResult Update(Feature feature)
        {
            return new InsertingResult(false, "Not Implemented.");
        }

        public virtual InsertingResult Update(Feature feature, int transactionId)
        {
            return new InsertingResult(false, "Not Implemented.");
        }

        public virtual InsertingResult Delete(Feature feature)
        {
            return new InsertingResult(false, "Not Implemented.");
        }

        public virtual InsertingResult Commit(int transactionId)
        {
            return new InsertingResult(false, "Not Implemented.");
        }
        
        public virtual InsertingResult Rollback(int transactionId)
        {
            return new InsertingResult(false, "Not Implemented.");
        }

        #region GetVersionsOf

        public virtual GettingResult GetVersionsOf(FeatureType featType,
                              TimeSliceInterpretationType interpretation,
                              Guid identifier = default(Guid),
                              bool loadComplexProps = false,
                              TimeSliceFilter timeSlicefilter = null,
                              List<string> propList = null,
                              Filters.Filter filter = null)
        {
            return GetVersionsOf(featType, interpretation, null, identifier, loadComplexProps, timeSlicefilter, propList, filter);
        }

        public abstract GettingResult GetVersionsOf(FeatureType featType,
                              TimeSliceInterpretationType interpretation,
                              TimePeriod submissionTime,
                              Guid identifier = default(Guid),
                              bool loadComplexProps = false,
                              TimeSliceFilter timeSlicefilter = null,
                              List<string> propList = null,
                              Filters.Filter filter = null);

        public virtual GettingResult GetVersionsOf(IAbstractFeatureRef absFeatRef,
                                    TimeSliceInterpretationType interpretation,
                                    bool loadComplexProps = false,
                                    TimeSliceFilter timeSliceFilter = null,
                                    List<string> propList = null,
                                    Filter filter = null)
        {

            return GetVersionsOf((FeatureType)absFeatRef.FeatureTypeIndex,
                                                interpretation,
                                                absFeatRef.Identifier,
                                                loadComplexProps,
                                                timeSliceFilter,
                                                propList,
                                                filter);
        }

        public virtual GettingResult GetVersionsOf(AbstractFeatureType absFeatType,
                                                            TimeSliceInterpretationType interpretation,
                                                            bool loadComplexProps = false,
                                                            TimeSliceFilter timeSlicefilter = null,
                                                            List<string> propList = null,
                                                            Filter filter = null)
        {
            if (CurrentUser == null)
                return new GettingResult("Please, Login...");

            var classInfo = AimMetadata.GetClassInfoByIndex((int) absFeatType);
            var descentClassInfoList = Aran.Aim.Utilities.AimMetadataUtility.GetAbstractChilds(classInfo);

            //List<AimClassInfo> descentClassInfoList = AimMetadata.AimClassInfoList.FindAll(
            //                                                    aimClassInfo =>
            //                                                            aimClassInfo.Parent != null &&
            //                                                            aimClassInfo.Parent.Name == absFeatType.ToString());
            
            Feature protoType;
            GettingResult result = new GettingResult(true);
            result.List = AimObjectFactory.CreateList((int)absFeatType);
            GettingResult getResult;
            foreach (AimClassInfo aimClassInfo in descentClassInfoList) {
                protoType = (Feature)AimObjectFactory.Create(aimClassInfo.Index);
                getResult = GetVersionsOf((FeatureType)aimClassInfo.Index,
                                        interpretation,
                                        default(Guid),
                                        loadComplexProps,
                                        timeSlicefilter,
                                        propList,
                                        filter);
                if (!getResult.IsSucceed)
                    return getResult;
                foreach (var feat in getResult.List)
                    result.List.Add(feat);
            }
            return result;
        }

        #endregion

        public virtual List<Feature> GetAllFeatuers(FeatureType featType, Guid identifier = default (Guid))
        {
            var propList = new List<string>();
            propList.Add("<LOAD_ALL_FOR_EXPORT>");

            GettingResult result = GetVersionsOf(
                        featType,
                        TimeSliceInterpretationType.BASELINE,
                        identifier,
                        true,
                        null,
                        propList,
                        null);

            if (result.IsSucceed)
                return result.GetListAs<Feature>();
            else
                throw new Exception(result.Message);
        }

        /// <summary>
        /// Returns all feature types that stored in DB
        /// </summary>
        /// <returns></returns>
        public virtual GettingResult GetAllStoredFeatTypes()
        {
            var list = new List<FeatureType>(Enum.GetValues(typeof(FeatureType)).Cast<FeatureType>());
            return new GettingResult(list);
        }

        /// <summary>
        /// Returns just identifers of features that stored in DB
        /// </summary>
        /// <returns></returns>
        public virtual GettingResult GelAllStoredIdentifiers()
        {
            return new GettingResult(false, "Not Implemented.");
        }

        /// <summary>
        /// Deletes rows only in features table whose has not other part.
        /// </summary>
        /// <param name="notExportedIdentifiers"></param>
        /// <returns> Returns count of rows affected </returns>
        public virtual GettingResult DeleteFeatIdentifiers(List<Guid> identifierList)
        {
            return new GettingResult(false, "Not Implemented.");
        }

        public virtual DateTime DefaultEffectiveDate { get; set; }

        public virtual bool SetEffectiveDateChangedEventHandler(EffectiveDateChangedEventHandler handler)
        {
            return false;
        }

        public virtual User CurrentUser { get; protected set; }

        public virtual string[] GetLastErrors()
        {
            return null;
        }

        public virtual bool IsExists(Guid guid)
        {
            throw new NotImplementedException();
        }

        public virtual bool IsExists(Guid guid, FeatureType type)
        {
            return false;
        }

        public virtual IUserManagement UserManagement { get; protected set; }

        public virtual bool UseCache
        {
            get { return _useChache; }
            set
            {
                if (_useChache == value)
                    return;

                _useChache = value;
                if (UseCacheChanged != null)
                    UseCacheChanged(this, new EventArgs());
            }
        }

        public virtual bool IsSlotActive { get; set; }

        public virtual bool IsDbLockWrite { get; set; }

        public virtual bool IsDbLockRead { get; set; }

        /// <summary>
        /// Return value format: "$serverName;$db;$userName;$effectiveDate"
        /// serverName format: $serverName:$port
        /// effectiveDate format: "yyyy-MM-hh";
        /// </summary>
        public virtual string GetConnectionInfo
        {
            get { return string.Empty; }
        }

        public virtual List<string> GetAllDBList(string host, int port)
        {
            return null;
        }

        public List<TFeat> GetBLFeatures<TFeat>() where TFeat : Feature, new()
        {
            var tmpFeat = new TFeat();

            var gr = GetVersionsOf(tmpFeat.FeatureType, TimeSliceInterpretationType.BASELINE,
                default(Guid), false, null, null, null);

            if (!gr.IsSucceed)
                throw new Exception(gr.Message);

            return gr.GetListAs<TFeat>();
        }

        public List<Feature> GetBLFeatures(FeatureType featType)
        {
            var gr = GetVersionsOf(featType, TimeSliceInterpretationType.BASELINE,
                default(Guid), false, null, null, null);

            if (!gr.IsSucceed)
                throw new Exception(gr.Message);

            return gr.GetListAs<Feature>();
        }

        public TFeat GetBLFeature<TFeat>(Guid identifier) where TFeat : Feature, new()
        {
            var tmpFeat = new TFeat();

            var gr = GetVersionsOf(tmpFeat.FeatureType, TimeSliceInterpretationType.BASELINE, identifier,
                false, null, null, null);

            if (!gr.IsSucceed)
                throw new Exception(gr.Message);

            if (gr.List == null || gr.List.Count == 0)
                return null;

            var feat = gr.List[0] as TFeat;
            return feat;
        }

        public Feature GetBLFeature(FeatureType featType, Guid identifier)
        {
            var gr = GetVersionsOf(featType, TimeSliceInterpretationType.BASELINE, identifier,
                false, null, null, null);

            if (!gr.IsSucceed)
                throw new Exception(gr.Message);

            if (gr.List == null || gr.List.Count == 0)
                return null;

            var feat = gr.List[0] as Feature;
            return feat;
        }

        public virtual void SetParameter(string key, object value)
        {
        }

        public virtual object GetParameter(string key)
        {
            return null;
        }

        public virtual List<FeatureReport> GetFeatureReport(Guid identifier)
        {
            return new List<FeatureReport>();
        }

        public virtual void SetFeatureReport(FeatureReport report)
        {
        }

        public virtual List<Screenshot> GetFeatureScreenshot(Guid identifier)
        {
            return new List<Screenshot>();
        }

        public virtual void SetFeatureScreenshot(Screenshot screenshot)
        {
        }

        public virtual void CallSpecialMethod(string methodName, object arg)
        {

        }

    }

    #region Delegates

    public delegate void EffectiveDateChangedEventHandler(object sender, EffectiveDateChangedEventArgs e);

    #endregion

    #region EventArgs

    public class EffectiveDateChangedEventArgs : EventArgs
    {
        public bool Ignore { get; set; }
    }

    #endregion

    public enum DbProviderType
    {
        Aran,
        ComSoft,
        XmlFile,
        TDB,
        AimLocal,
        Other
    }

    public class FeatureReport
    {
        public Guid Identifier { get; set; }

        public FeatureReportType ReportType { get; set; }
        public DateTime DateTime { get; set; }

        public byte[] HtmlZipped { get; set; }
    }

    public enum FeatureReportType { Mixed, Obstacle, Geometry, Log, Protocol, AIXM51 }

    public class Screenshot
    {
        public Guid Identifier { get; set; }
        public DateTime DateTime { get; set; }
        public byte[] Images { get; set; }
    }
}
