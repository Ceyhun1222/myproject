using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Aran.Aim.CAWProvider;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;
using Aran.Aim.Data.Filters;
using Aran.Aim.Utilities;
using Aran.Geometries;
using Aran.Aim.CAWProvider.Configuration;
using System.Collections;
using Aran.Package;

namespace Aran.Aim.Data
{
    public class CawDbProvider : DbProvider
    {
        #region Fields
        private CawService _cawService;
        private TimeSliceFilter TimeSliceFilter;
        //private Dictionary<string, GettingResult> _getVersionOfResultCache;
        private DbProvider _cacheDbPro;
        #endregion


        public CawDbProvider()
        {
            _cawService = new CawService();
            State = ConnectionState.Closed;
            //_getVersionOfResultCache = new Dictionary<string, GettingResult>();
        }


        public void Test()
        {
            _cawService.Test();
        }


        public override DbProviderType GetProviderType(ref string otherName)
        {
            return DbProviderType.ComSoft;
        }

        public override void Open(string connectionString)
        {
            string server;
            string port;
            string dbUserName;
            string dbPassword;
            bool useCache;

            ParseConnectionString(connectionString, out server, out port, out dbUserName, out dbPassword, out useCache);

            var serverUri = server;

            if (!server.StartsWith("http"))
                serverUri = string.Format("http://{0}{1}/cadas-aimdb/caw", server, string.IsNullOrWhiteSpace(port) ? "" : ":" + port);

            _cawService.ConnectionInfo = new ConnectionInfo(new Uri(serverUri), dbUserName, dbPassword);
            _cawService.UseCache = useCache;

            State = ConnectionState.Open;
        }

        public override bool Login(string userName, string md5Password)
        {
            CurrentUser = new User();
            CurrentUser.Name = userName;
            CurrentUser.Privilege = Privilige.prReadWrite;

            _cawService.ConnectionInfo = new ConnectionInfo(_cawService.ConnectionInfo.Server, userName, md5Password);

            var ftList = Enum.GetValues(typeof(FeatureType)).Cast<int>();
            CurrentUser.FeatureTypes.AddRange(ftList);
            return true;
        }

        public override void Close()
        {
            State = ConnectionState.Closed;

            if (_cacheDbPro != null)
                _cacheDbPro.Close();
        }

        public override int BeginTransaction()
        {
            return BeginTransaction("PANDA_WorkPackage");
        }

        public int BeginTransaction(string workPackageName)
        {
            if (IncludedWorkPackage != null)
                return IncludedWorkPackage.Id;

            return _cawService.BeginTransaction(workPackageName);
        }

        public override InsertingResult Insert(IEnumerable<Feature> features, int transactionId = -1)
        {
            var result = new InsertingResult(true);

            var isTransCreated = (transactionId == -1);
            if (isTransCreated)
                transactionId = BeginTransaction();

            try
            {
                _cawService.InsertFeature(features, transactionId);

                if (_cacheDbPro != null)
                {
                    try
                    {
                        _cacheDbPro.Insert(features);
                    }
                    catch { }
                }
            }
            catch (CawFaultMessageException cawEx)
            {
                if (isTransCreated)
                    Rollback(transactionId);

                result.IsSucceed = false;
                result.Message = string.Join<string>("\r\n", cawEx.Messages.Select(item => item.ToString()));
                return result;
            }
            catch (Exception ex)
            {
                if (isTransCreated)
                    Rollback(transactionId);

                result.IsSucceed = false;
                result.Message = ex.Message;
                return result;
            }

            if (isTransCreated)
                Commit(transactionId);

            return result;
        }

        public override InsertingResult Insert(Feature feature, bool insertAnyway, bool asCorrection)
        {
            var transactionId = BeginTransaction();
            var ir = Insert(feature, transactionId, insertAnyway, asCorrection);
            if (!ir.IsSucceed) {
                Rollback(transactionId);
                return ir;
            }
            return Commit(transactionId);
        }

        public override InsertingResult Insert(Feature feature, int transactionId, bool insertAnyway, bool asCorrection)
        {
            var result = new InsertingResult(true);

            feature.TimeSlice.Interpretation = TimeSliceInterpretationType.PERMDELTA;

            try {
                if (IncludedWorkPackage != null) {
                    var ed = IncludedWorkPackage.EffectiveDate;
                    if (feature.TimeSlice.ValidTime.BeginPosition < ed)
                        feature.TimeSlice.ValidTime.BeginPosition = new DateTime(ed.Year, ed.Month, ed.Day);
                }

                if (asCorrection)
                {
                    if (feature.TimeSlice.SequenceNumber > 1)
                        feature.TimeSlice.FeatureLifetime = null;

                    feature.TimeSlice.CorrectionNumber++;
                }

                _cawService.InsertFeature(feature, transactionId);

                if (_cacheDbPro != null)
                {
                    try
                    {
                        _cacheDbPro.Insert(feature);
                    }
                    catch { }
                }
            }
            catch (CawFaultMessageException cawEx){
                result.IsSucceed = false;
                result.Message = string.Join<string>("\r\n", cawEx.Messages.Select(item => item.ToString()));
            }
            catch (Exception ex) {
                result.IsSucceed = false;
                result.Message = ex.Message;
            }
            return result;
        }

        public override InsertingResult Commit(int transactionId)
        {
            var result = new InsertingResult(true);

            if (IncludedWorkPackage != null)
                return result;

            try {
                _cawService.Commit(transactionId, true);
            }
            catch (Exception ex) {
                result.IsSucceed = false;
                result.Message = ex.Message;
            }
            
            return result;
        }

        public override InsertingResult Rollback(int transactionId)
        {
            var result = new InsertingResult(true);

            if (IncludedWorkPackage != null)
                return result;

            try {
                _cawService.Commit(transactionId, false);
            }
            catch (Exception ex) {
                result.IsSucceed = false;
                result.Message = ex.Message;
            }
            
            return result;
        }

        public WorkPackageName IncludedWorkPackage { get; set; }

        public void CloseCurrentWorkPackage(bool save)
        {
            if (IncludedWorkPackage == null)
                return;

            _cawService.Commit(IncludedWorkPackage.Id, save);
        }

        public override GettingResult GetVersionsOf(FeatureType featType,
            TimeSliceInterpretationType interpretation,
            Aran.Aim.DataTypes.TimePeriod submissionTime,
            Guid identifier = default(Guid),
            bool loadComplexProps = false,
            TimeSliceFilter timeSlicefilter = null,
            List<string> propList = null,
            Filters.Filter filter = null)
        {
            if (_cacheDbPro != null)
            {
                return _cacheDbPro.GetVersionsOf(
                    featType, interpretation, submissionTime, identifier, 
                    loadComplexProps, timeSlicefilter, propList, filter);
            }

            GettingResult result;
            TemporalTimeslice tempTS = null;

            if (timeSlicefilter == null)
            {
                if (TimeSliceFilter == null)
                    throw new Exception("TimeSliceFilter is undefined");

                tempTS = ToTemporalTimeslice(TimeSliceFilter);
            }
            else
            {
                tempTS = ToTemporalTimeslice(timeSlicefilter);
            }


            //var argKey = GetArgumentKey(featType, identifier, tempTS, filter);
            //if (_getVersionOfResultCache.TryGetValue(argKey, out result))
            //    return result;


            AbstractRequest absReq;
            bool manualFilter = false;

            int linkQueryOperIndex = -1;
            LinkQuery lq;

            if ((lq = IsLinkQuery(featType, filter, out linkQueryOperIndex)) != null)
            {
                lq.SimpleQuery.TemproalTimeslice = tempTS;
                lq.SimpleQuery.Interpretation = ToCawInterpretationType(interpretation);

                absReq = lq;
            }
            else
            {
                var sq = new SimpleQuery();
                sq.FeatureType = featType;
                if (identifier != Guid.Empty)
                {
                    sq.IdentifierList.Add(identifier);
                }

                sq.Interpretation = ToCawInterpretationType(interpretation);
                sq.TemproalTimeslice = tempTS;

                if (submissionTime != null) {
#warning EndPosition have to in the past (Comsoft error)
                    if (submissionTime.EndPosition != DateTime.Now)
                        sq.InsertionPeriod = submissionTime;
                }

                var identifierList = false;

                if (filter != null)
                {
                    var co = filter.Operation.ComparisonOps;

                    if (co != null)
                    {

                        if ((co.OperationType == Filters.ComparisonOpType.In || co.OperationType == Filters.ComparisonOpType.EqualTo) &&
                        co.PropertyName.ToLower() == "identifier" &&
                        (co.Value is IEnumerable<Guid> || co.Value is Guid))
                        {

                            if (co.Value is Guid)
                            {
                                sq.IdentifierList.Add((Guid)co.Value);
                            }
                            else
                            {
                                foreach (Guid iden in (co.Value as IEnumerable<Guid>))
                                    sq.IdentifierList.Add(iden);
                            }
                            identifierList = true;
                        }
                    }
                    else
                    {
                        sq.Filter = ToCawFilter(filter);
                    }
                }

                if (propList != null && propList.Count > 0)
                    sq.SetPropertyNames(propList);

                absReq = sq;

                manualFilter = (filter != null && sq.Filter == null && !identifierList);
            }

            result = new GettingResult();

            try
            {
                Feature[] featArr = _cawService.GetFeature(absReq, IncludedWorkPackage == null ? null : (int?)IncludedWorkPackage.Id);
                result.List = featArr;
                result.IsSucceed = true;

                #region Set is inside workpackage

                if (IncludedWorkPackage != null)
                {
                    var wp = _cawService.GetWorksPackage(IncludedWorkPackage.Id);
                    if (wp != null && wp.TimeSlices.Count > 0)
                    {
                        foreach (var tsb in wp.TimeSlices)
                        {
                            foreach (var feat in featArr)
                            {
                                if (feat.Identifier == tsb.Identifier)
                                {
                                    feat.WorksPackageId = wp.Id;
                                    break;
                                }
                            }
                        }
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                result.IsSucceed = false;
                result.Message = ex.Message;
            }

            #region ManualFilter

            if (result.List != null && result.List.Count > 0 &&
                (manualFilter || linkQueryOperIndex != -1))
            {
                AimClassInfo classInfo = AimMetadata.GetClassInfoByIndex(result.List[0] as IAimObject);

                List<Feature> featureList = new List<Feature>();
                foreach (Feature feat in result.List)
                {
                    if ((int)feat.FeatureType != classInfo.Index)
                    {
                        classInfo = AimMetadata.GetClassInfoByIndex(feat as IAimObject);
                    }

                    try
                    {
                        if (ManualFilter(classInfo, feat, filter.Operation, linkQueryOperIndex))
                        {
                            featureList.Add(feat);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Filter not correct\n" + ex.Message);
                    }
                }

                result.List = featureList;
            }

            #endregion

            //_getVersionOfResultCache.Add(argKey, result);

            return result;
        }

        private string GetArgumentKey(FeatureType featType, Guid identifier, TemporalTimeslice tempTS, Filters.Filter filter)
        {
            using (var ms = new System.IO.MemoryStream()) {
                using (var bpw = new Aran.Package.BinaryPackageWriter(ms)) {
                    bpw.PutEnum<FeatureType>(featType);
                    bpw.PutString(identifier.ToString());

                    tempTS.Pack(bpw);

                    bpw.PutBool(filter != null);
                    if (filter != null)
                        filter.Operation.Pack(bpw);

                    bpw.PutBool(IncludedWorkPackage != null);
                    if (IncludedWorkPackage != null)
                        bpw.PutInt32(IncludedWorkPackage.Id);
                    
                    var ba = ms.ToArray();
                    return Encoding.UTF8.GetString(ba);
                }
            }
        }

        public override DateTime DefaultEffectiveDate
        {
            get
            {
                if (TimeSliceFilter == null)
                    return default(DateTime);
                return TimeSliceFilter.EffectiveDate;
            }
            set
            {
                TimeSliceFilter = new TimeSliceFilter(value);
            }
        }

        public List<WorkPackageOutline> GetWorkPackages(params WorksPackageStatusType[] statuses)
        {
            return _cawService.GetWorkPackages(statuses);
        }

        public WorkPackage GetWorkPackage(int workPackageId)
        {
            return _cawService.GetWorksPackage(workPackageId);
        }

        public void ClearRuntimeCache()
        {
            //_getVersionOfResultCache.Clear();
        }

        public DbProvider CacheDbProvider
        {
            get { return _cacheDbPro; }
            set { _cacheDbPro = value; }
        }

        public override string GetConnectionInfo
        {
            get
            {
                return string.Format("{0};{1}",
                    _cawService.ConnectionInfo.Server,
                    "---");
            }
        }

        #region HelperFunctions

        private void ParseConnectionString(string connectionString,
                        out string server,
                        out string port,
                        out string userName,
                        out string password,
                        out bool useCache)
        {
            //Server=abc\nPort=8071\nUserName=bcd\nPassword=cde\nUseCache=TRUE

            useCache = false;

            var errorText = "Connection String is not valid.";

            var sa = connectionString.Split("\n;".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (sa.Length < 4)
                throw new Exception(errorText);

            var resultSA = new string[5];

            var ind = 0;
            foreach (string s in sa) {
                string ls = s.Trim().ToLower();

                if (ls.StartsWith("server"))
                    ind = 0;
                else if (ls.StartsWith("port"))
                    ind = 1;
                else if (ls.StartsWith("username"))
                    ind = 2;
                else if (ls.StartsWith("password"))
                    ind = 3;
                else if (ls.StartsWith("usecache"))
                    ind = 4;

                int charIndex = s.IndexOf('=');
                if (charIndex == -1)
                    throw new Exception(errorText);

                resultSA[ind] = s.Substring(charIndex + 1);
            }

            server = resultSA[0];
            port = resultSA[1];
            userName = resultSA[2];
            password = resultSA[3];
            if (resultSA.Last () != null)
                useCache = bool.Parse(resultSA[4]);
        }

        private TemporalTimeslice ToTemporalTimeslice(TimeSliceFilter tsFilter)
        {
            switch (tsFilter.QueryType) {
                case QueryType.ByEffectiveDate:
                    return new TemporalTimeslice(tsFilter.EffectiveDate);
                case QueryType.BySequenceNumber:
                    return new TemporalTimeslice((uint)tsFilter.SequenceNumber);
                default:
                    return new TemporalTimeslice(tsFilter.ValidTime);
            }
        }

        private bool ManualFilter(AimClassInfo classInfo, Feature feature, Filters.OperationChoice operChoice, int linkQueryOperIndex = -1)
        {
            switch (operChoice.Choice) {
                case Filters.OperationChoiceType.Comparison:
                    return ManualFilter(classInfo, feature, operChoice.ComparisonOps);
                case Filters.OperationChoiceType.Spatial:
                    return ManualFilter(classInfo, feature, operChoice.SpatialOps);
                default:
                    return ManualFilter(classInfo, feature, operChoice.LogicOps, linkQueryOperIndex);
            }
        }

        private bool ManualFilter(AimClassInfo classInfo, Feature feature, Filters.ComparisonOps compOps)
        {
            IAimProperty aimPropValue = null;

            if (compOps.PropertyName.IndexOf('.') > 0) {
                string propName = compOps.PropertyName.Replace('.', '/');

                AimPropInfo[] propInfoArr = AimMetadataUtility.GetInnerProps(classInfo.Index, compOps.PropertyName);
                List<IAimProperty> propValList =
                    AimMetadataUtility.GetInnerPropertyValue(feature, propInfoArr, false);

                if (propValList.Count > 0)
                    aimPropValue = propValList[propValList.Count - 1];
            }
            else {
                AimPropInfo propInfo = classInfo.Properties[compOps.PropertyName];
                if (propInfo != null)
                    aimPropValue = (feature as IAimObject).GetValue(propInfo.Index);
            }

            if (aimPropValue != null && aimPropValue.PropertyType == AimPropertyType.AranField) {
                IEditAimField editAimField = aimPropValue as IEditAimField;
                return compOps.Value.Equals(editAimField.FieldValue);
            }

            return false;
        }

        private bool ManualFilter(AimClassInfo classInfo, Feature feature, Filters.SpatialOps spatialOps)
        {
            if (spatialOps is Filters.Within) {
                Filters.Within within = (Filters.Within)spatialOps;

                Aran.Geometries.Point point = null;

                string propName = within.PropertyName.Replace('.', '/');

                AimPropInfo[] propInfoArr = AimMetadataUtility.GetInnerProps(classInfo.Index, propName);

                if (propInfoArr.Length > 0 && propInfoArr[propInfoArr.Length - 1].PropType.Index == (int)AimFieldType.GeoPoint) {
                    List<IAimProperty> aimPropValList = AimMetadataUtility.GetInnerPropertyValue(feature, propInfoArr);
                    if (aimPropValList.Count > 0 && aimPropValList[aimPropValList.Count - 1] != null) {
                        IEditAimField editField = (aimPropValList[aimPropValList.Count - 1] as IEditAimField);
                        point = editField.FieldValue as Aran.Geometries.Point;

                        if (within.Geometry.Type == GeometryType.MultiPolygon) {
                            MultiPolygon multiPolygon = within.Geometry as MultiPolygon;
                            return multiPolygon.IsPointInside(point);
                        }
                    }
                }
            }
            return false;
        }

        private bool ManualFilter(AimClassInfo classInfo, Feature feature, Filters.LogicOps logicOps, int linkQueryOperIndex = -1)
        {
            Filters.BinaryLogicOp binLogOp = logicOps as Filters.BinaryLogicOp;

            for (int i = 0; i < binLogOp.OperationList.Count; i++) {
                if (i == linkQueryOperIndex)
                    continue;

                bool resultBool = ManualFilter(classInfo, feature, binLogOp.OperationList[i]);

                if (binLogOp.Type == Filters.BinaryLogicOpType.And) {
                    if (!resultBool)
                        return false;
                    else {
                        if (i == binLogOp.OperationList.Count - 1 ||
                            (i == binLogOp.OperationList.Count - 2 && i == linkQueryOperIndex - 1)) {
                            return true;
                        }
                    }
                }
                else {
                    if (resultBool)
                        return true;
                }
            }

            return false;
        }

        private CAWProvider.Filter ToCawFilter(Filters.Filter aranFilter)
        {
            var cawOperChoice = ToCawOperationChoice(aranFilter.Operation);

            if (cawOperChoice == null)
                return null;

            var cawFilter = new CAWProvider.Filter();
            cawFilter.Operation = cawOperChoice;
            return cawFilter;
        }

        private CAWProvider.OperationChoice ToCawOperationChoice(Filters.OperationChoice aranOperChoice)
        {
            if (aranOperChoice.Choice == Filters.OperationChoiceType.Comparison) {
                var aranCompOp = aranOperChoice.ComparisonOps;

                string propName = aranCompOp.PropertyName;

                #region Remove AssociationObect propName like that: ('ResponsibleOrganisation/TheOrganisationAuthority' => 'ResponsibleOrganisation')

                var ind = propName.LastIndexOf('/');
                if (ind != -1) {
                    var s = propName.Substring(ind + 1);
                    if (s.ToLower().StartsWith("the")) {
                        s = s.Substring(3);
                        Aran.Aim.FeatureType ft;
                        if (Enum.TryParse<Aran.Aim.FeatureType>(s, out ft)) {
                            propName = propName.Substring(0, ind);
                        }
                    }
                }

                #endregion

                if (propName.IndexOf('.') == -1) {
                    CAWProvider.ComparisonOps cawCompOps = new CAWProvider.ComparisonOps();
                    cawCompOps.PropertyName = propName;
                    cawCompOps.Value = aranCompOp.Value;

                    try {
                        cawCompOps.OperationType = ToCAWComparisonOpType(aranCompOp.OperationType);
                    }
                    catch {
                        return null;
                    }

                    return new CAWProvider.OperationChoice(cawCompOps);
                }
                else {
                }
            }
            else if (aranOperChoice.Choice == Filters.OperationChoiceType.Spatial) {
                if (aranOperChoice.SpatialOps is Filters.Within) {

                    var aranWithin = aranOperChoice.SpatialOps as Filters.Within;
                    
                    var cawWithin = new CAWProvider.Within();

                    string propName = aranWithin.PropertyName;
                    if (propName.ToLower().EndsWith(".geo"))
                        propName = propName.Remove(propName.Length - 4);

                    cawWithin.PropertyName = propName;
                    cawWithin.Geometry = aranWithin.Geometry;

                    return new CAWProvider.OperationChoice(cawWithin);
                }
                else if (aranOperChoice.SpatialOps is Filters.DWithin) {
                    var aranDWithin = aranOperChoice.SpatialOps as Filters.DWithin;

                    var cawDWithin = new CAWProvider.DWithin();
                    string propName = aranDWithin.PropertyName;
                    if (propName.ToLower().EndsWith(".geo"))
                        propName = propName.Remove(propName.Length - 4);

                    cawDWithin.PropertyName = propName;
                    cawDWithin.Geometry = aranDWithin.Point;
                    cawDWithin.Distance = aranDWithin.Distance;

                    return new CAWProvider.OperationChoice(cawDWithin);
                }
            }
            else if (aranOperChoice.Choice == Filters.OperationChoiceType.Logic) {
                var aranBlo = aranOperChoice.LogicOps as Filters.BinaryLogicOp;

                var cawBlo = new CAWProvider.BinaryLogicOp();
                cawBlo.Type = (CAWProvider.BinaryLogicOpType)(int)aranBlo.Type;

                foreach (var aranOperChoiceItem in aranBlo.OperationList) {
                    var cawOperChoice = ToCawOperationChoice(aranOperChoiceItem);
                    if (cawOperChoice != null)
                        cawBlo.OperationList.Add(cawOperChoice);
                }
                
                return new CAWProvider.OperationChoice(cawBlo);
            }

            return null;
        }

        private CAWProvider.ComparisonOpType ToCAWComparisonOpType(Filters.ComparisonOpType comparisonOpType)
        {
            if (comparisonOpType == Filters.ComparisonOpType.NotNull)
                return CAWProvider.ComparisonOpType.ContainsProperty;

            string enumItem = "Is" + comparisonOpType.ToString();

            try {
                CAWProvider.ComparisonOpType result = CAWProvider.ComparisonOpType.IsEqualTo;
                if (!Enum.TryParse<CAWProvider.ComparisonOpType>(enumItem, out result))
                    throw new Exception(string.Format("Camparison OperationType is not supported! '{0}'", comparisonOpType.ToString()));
                return result;
            }
            catch {

            }

            throw new Exception("Could not convert from ARAN.ComparisonOpType to ComSoft.ComparisonOpType");
        }

        private LinkQuery IsLinkQuery(FeatureType featureType, Filters.Filter filter, out int linkQueryOperIndex)
        {
            linkQueryOperIndex = -1;

            if (filter == null)
                return null;

            if (filter.Operation.Choice == Filters.OperationChoiceType.Comparison) {
                return ComparisonOpsToLinkQuery(featureType, filter.Operation.ComparisonOps);
            }
            else if (filter.Operation.Choice == Filters.OperationChoiceType.Logic) {
                Filters.BinaryLogicOp binLogOp = (Filters.BinaryLogicOp)filter.Operation.LogicOps;

                for (int i = 0; i < binLogOp.OperationList.Count; i++) {
                    Filters.OperationChoice operChoice = binLogOp.OperationList[i];

                    if (operChoice.Choice == Filters.OperationChoiceType.Comparison) {
                        LinkQuery tmpLq = ComparisonOpsToLinkQuery(featureType, operChoice.ComparisonOps);
                        if (tmpLq != null) {
                            linkQueryOperIndex = i;
                            return tmpLq;
                        }
                    }
                }
            }

            return null;
        }

        private LinkQuery ComparisonOpsToLinkQuery(FeatureType featureType, Filters.ComparisonOps compOps)
        {
            LinkQuery lq = null;

            if (compOps.Value is Guid && !compOps.PropertyName.Equals("identifier", StringComparison.InvariantCultureIgnoreCase)) {
                lq = new LinkQuery();
                lq.FeatureTypeList.Add(featureType);

                lq.SimpleQuery = new SimpleQuery();
                lq.SimpleQuery.IdentifierList.Add((Guid)compOps.Value);

                string propName = compOps.PropertyName;

                if (propName != null && 
                    propName.Equals("ResponsibleOrganisation/TheOrganisationAuthority", StringComparison.InvariantCultureIgnoreCase) ||
                    propName.Equals("ResponsibleOrganisation.TheOrganisationAuthority", StringComparison.InvariantCultureIgnoreCase)) {
                    lq.TraverseTimeslicePropertyName = "responsibleOrganisation";
                    lq.SimpleQuery.FeatureType = FeatureType.OrganisationAuthority;
                    return lq;
                }

                AimClassInfo classInfo = AimMetadata.GetClassInfoByIndex((int)featureType);
                AimPropInfo propInfo = classInfo.Properties[propName];

                lq.TraverseTimeslicePropertyName = propInfo.AixmName;
                lq.SimpleQuery.FeatureType = (FeatureType)propInfo.ReferenceFeature;
            }

            return lq;
        }

        private InterpretationType ToCawInterpretationType(TimeSliceInterpretationType interpretation)
        {
            return (InterpretationType)(int)interpretation;
        }

        #endregion

        public override void SetParameter(string key, object value)
        {
            if (_cacheDbPro != null && key == "central-meridian")
                _cacheDbPro.SetParameter(key, value);
        }

        public override object GetParameter(string key)
        {
            if (_cacheDbPro != null && key == "central-meridian")
                return _cacheDbPro.GetParameter(key);

            return base.GetParameter(key);
        }

        public override void CallSpecialMethod(string methodName, object arg)
        {
            if (methodName == "SetCacheDbProvider")
            {
                CacheDbProvider = arg as DbProvider;
            }
        }
    }

    public class WorkPackageName : IPackable
    {
        public WorkPackageName()
        {
        }

        public WorkPackageName(int id, string name, DateTime effectiveDate)
        {
            Id = id;
            Name = name;
            EffectiveDate = effectiveDate;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime EffectiveDate { get; set; }

        public void Pack(PackageWriter writer)
        {
            writer.PutInt32(Id);
            writer.PutString(Name);
            writer.PutDateTime(EffectiveDate);
        }

        public void Unpack(PackageReader reader)
        {
            Id = reader.GetInt32();
            Name = reader.GetString();
            EffectiveDate = reader.GetDateTime();
        }
    }
}