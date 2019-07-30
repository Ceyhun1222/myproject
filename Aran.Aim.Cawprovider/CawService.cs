using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Features;
using Aran.Aim.CAWProvider.Configuration;
using Aran.Aim.AixmMessage;
using System.IO;

namespace Aran.Aim.CAWProvider
{
    internal class CawService : ICawService
    {
        public CawService ()
        {
            _service = new CsCawPortTypeService ();
            _sdpService = new CsCawPortTypeService ();
            _sdpService.UseChache = false;
            _workPackageCacheDict = new Dictionary<int, WorkPackage>();
        }

        public ConnectionInfo ConnectionInfo
        {
            get 
            {
                return _service.ConnectionInfo;
            }
            set 
            {
                _service.ConnectionInfo = value;

                if (_service.ConnectionInfo.Server.OriginalString.ToLower ().EndsWith ("/caw"))
                {
                    string host = _service.ConnectionInfo.Server.OriginalString;
                    host = host.Remove (host.Length - 3) + "sdp";
                    _sdpService.ConnectionInfo = new ConnectionInfo
                    {
                        Server = new Uri (host),
                        UserName = _service.ConnectionInfo.UserName,
                        Password = _service.ConnectionInfo.Password
                    };
                }
            }
        }

        public int BeginTransaction()
        {
            return BeginTransaction("");
        }

        public int BeginTransaction(string workPackageName)
        {
            var cop = new CreateOperationParameter();
            cop.Properties = new WorkPackageProperties();
            cop.Properties.Name = workPackageName;
            cop.Properties.Type = "sdp.type.default";

            var oper = new Operation();
            oper.Name = "sdp.operation.create";
            oper.OperationParameter = cop;

            var resp = _sdpService.GetResponse<OperationResponse>(oper);

            if (resp.Status == ResponseStatus.FAILURE && resp.FaultMessages.Count > 0)
                throw new Exception(resp.FaultMessages[0].DefaultErrorMessage);

            return resp.WorkPackage.Id;
        }

        public void Commit(int workPackageId, bool save)
        {
            var oper = new Operation();
            oper.WorkPackageId = workPackageId;

            OperationResponse resp;

            if (!save) {
                oper.Name = "sdp.operation.discard";
                resp = _sdpService.GetResponse<OperationResponse>(oper);
                return;
            }

            oper.Name = "sdp.operation.lock";
            resp = _sdpService.GetResponse<OperationResponse>(oper);
            if (resp.Status == ResponseStatus.FAILURE)
                throw new CawFaultMessageException(resp.FaultMessages);

            oper.Name = "sdp.operation.validate";
            resp = _sdpService.GetResponse<OperationResponse>(oper);
            if (resp.Status == ResponseStatus.FAILURE)
                throw new CawFaultMessageException(resp.FaultMessages);

            oper.Name = "sdp.operation.commit";
            resp = _sdpService.GetResponse<OperationResponse>(oper);
            if (resp.Status == ResponseStatus.FAILURE)
                throw new CawFaultMessageException(resp.FaultMessages);
        }

        public bool InsertFeature (Feature feature, int? workpackageId)
        {
            return InsertFeature(new List<Feature> { feature }, workpackageId);
        }

        public bool InsertFeature(IEnumerable<Feature> features, int? workpackageId)
        {
            var useCache = _service.UseChache;
            _service.UseChache = false;

            var transaction = new Transaction();
            transaction.Workpackage = workpackageId;
            transaction.Insert = new Insert();

            var iam = new InsertAixmMessage
            {
                AixmBasicMessage = new AixmBasicMessage(MessageReceiverType.Cadas) { WriteExtension = false }
            };

            foreach (var feat in features)
                iam.AixmBasicMessage.HasMember.Add(new AixmFeatureList { feat });

            transaction.Insert.InsertAixmMessages.Add(iam);

	        CsCawPortTypeService.WriteReqRespData = File.Exists(@"C:\write-cadas-set.log");

            var response = _service.Transaction(transaction);

            CheckCawFault(response);

            _service.UseChache = useCache;
            return true;
        }

        public Feature[] GetFeature(AbstractRequest query, int? workPackageId)
        {
	        CsCawPortTypeService.WriteReqRespData = File.Exists(@"C:\write-cadas-get.log");

            GetFeature getFeature = new GetFeature();
            getFeature.Workpackage = workPackageId;
            getFeature.QueryList.Add(query);

            GetFeatureResponse gfResponse = _service.GetFeature(getFeature);

            if (gfResponse.FaultMessages.Count > 0)
            {
                throw new CawFaultMessageException(gfResponse.FaultMessages);
            }

            if (gfResponse.QueryResults.Count == 0)
            {
                throw new CawException("Response format is invalid. QueryResult not exists.");
            }

            QueryResult queryResult = gfResponse.QueryResults[0];

            if (queryResult.FaultMessages != null &&
                queryResult.FaultMessages.Count > 0)
            {
                throw new CawFaultMessageException(queryResult.FaultMessages);
            }

            if (queryResult.ChoiceValues == null ||
                queryResult.ChoiceValues.Length == 0)
            {
                return new Feature[0];
            }

            QueryResultChoice choice = queryResult.ChoiceValues[0];

            if (choice.Choice != QueryResultChoiceType.AixmBasicMessage)
            {
                throw new CawException("QueryResult Choice not implemented.");
            }

            AixmBasicMessage aixmBasicMessage = choice.AixmBasicMessage;

            if (aixmBasicMessage.HasMember.Count == 0)
                return null;

            List<Feature> featureList = new List<Feature>();

            int startIndex = 0;
            if (query is LinkQuery && (query as LinkQuery).RetrieveSourceFeature)
                startIndex = 1;

            for (int i = startIndex; i < aixmBasicMessage.HasMember.Count; i++)
            {
                foreach (Feature feature in aixmBasicMessage.HasMember[i])
                {
                    featureList.Add(feature);
                }
            }

            return featureList.ToArray();
        }

        public List<TFeature> GetFeature<TFeature> (AbstractRequest query) where TFeature : Feature
        {
            Feature [] featArr = GetFeature (query, null);
            List<TFeature> featureList = new List<TFeature> ();
            foreach (Feature feat in featArr)
                featureList.Add ((TFeature) feat);
            return featureList;
        }

        public List<WorkPackageOutline> GetWorkPackages(IEnumerable<WorksPackageStatusType> statuses)
        {
	        CsCawPortTypeService.WriteReqRespData = File.Exists(@"C:\write-cadas-other.log");

            var outline = new Outline();
            outline.RetrieveWorkPackages = true;
            outline.RetrieveTimeSlices = true;
            outline.Filter = new OutlineFilter();

            foreach (var item in statuses) {
                var status = "";
                switch (item) {
                    case WorksPackageStatusType.Open:
                        status = "sdp.state.open";
                        break;
                    case WorksPackageStatusType.Close:
                        status = "sdp.state.close";
                        break;
                }
                outline.Filter.WorkPackageStatusList.Add(status);
            }

            var outlineResp = _sdpService.GetResponse<OutlineResponse>(outline);
            return outlineResp.WorkPackages;
        }

        public WorkPackage GetWorksPackage(int id)
        {
            if (_workPackageCacheDict.ContainsKey(id))
                return _workPackageCacheDict[id];

            var getWP = new GetWorkPackage();
            getWP.WorkPackageIdList.Add(id);

            var wpr = _sdpService.GetResponse<GetWorkPackageResponse>(getWP);

            if (wpr.FaultMessages.Count > 0) {
                var ex = new CawException(wpr.FaultMessages[0].DefaultErrorMessage);
                ex.FaultMessages.AddRange(wpr.FaultMessages);
                throw ex;
            }

            if (wpr.WorkPackages.Count > 0) {
                var wp = wpr.WorkPackages[0];
                _workPackageCacheDict.Add(id, wp);
                return wp;
            }

            return null;
        }
        

        internal bool UseCache
        {
            get { return _service.UseChache; }
            set { _service.UseChache = value; }
        }

        private void CheckCawFault(TransactionResponse response)
        {
            if (response.TransactionSummary != null)
            {
                var ts = response.TransactionSummary;
                if (ts.Successfully == 0 &&
                    ts.FaultMessages.Count(fm => fm.Category == LogMessageCategoryType.ERROR) > 0)
                {
                    throw new CawFaultMessageException(ts.FaultMessages);
                }
            }

            if (response.FaultMessageList.Count > 0)
                throw new CawFaultMessageException(response.FaultMessageList);

            if (response.InsertResultsList.Count == 0)
                throw new CawException("[InsertResult] does not exists.");

            if (response.InsertResultsList[0].InsertedFeatureList.Count == 0)
                throw new CawException("[InsertedFeature] does not exists.");

            if (response.InsertResultsList[0].InsertedFeatureList[0].FaultMessageList.Count > 0)
                throw new CawFaultMessageException(response.InsertResultsList[0].InsertedFeatureList[0].FaultMessageList);

        }

        private CsCawPortTypeService _service;
        private CsCawPortTypeService _sdpService;
        private Dictionary<int, WorkPackage> _workPackageCacheDict;

        internal void Test()
        {
            var outline = new Outline();
            outline.RetrieveWorkPackages = true;
            outline.RetrieveTimeSlices = true;
            outline.Filter = new OutlineFilter();
            outline.Filter.WorkPackageStatusList.Add("sdp.state.open");
            
            var outlineResp = _sdpService.GetResponse<OutlineResponse>(outline);

            var list = new List<Feature>();

            foreach (var wpOutline in outlineResp.WorkPackages)
            {
                if (wpOutline.Content != null && wpOutline.Content.Name == "test-wp-1")
                {
                    if (wpOutline.TimeSlices.Count > 0)
                    {
                        foreach (var tsOutline in wpOutline.TimeSlices)
                        {
                            var content = tsOutline.Content;

                            if (content != null &&
                                content.FeatureType != null)
                            {
                                var featArr = Test_GetFeatureInWP(content.FeatureType.Value, content.Identifier,
                                    wpOutline.Content.Id, wpOutline.Content.EffectiveDate);

                                list.AddRange(featArr);
                            }
                        }
                    }
                }
            }
            
            //var count = resp.WorkPackages.Count;

            var count = list.Count;
        }

        internal Feature[] Test_GetFeatureInWP(FeatureType featType, Guid identifier, int workPackage, DateTime effectiveDate)
        {
            var sq = new SimpleQuery();
            sq.FeatureType = featType;
            sq.IdentifierList.Add(identifier);
            sq.Interpretation = InterpretationType.BASELINE;
            sq.TemproalTimeslice = new TemporalTimeslice(effectiveDate);

            GetFeature getFeature = new GetFeature();
            getFeature.Workpackage = workPackage;
            getFeature.QueryList.Add(sq);

            GetFeatureResponse gfResponse = _service.GetFeature(getFeature);

            if (gfResponse.FaultMessages.Count > 0) {
                throw new CawFaultMessageException(gfResponse.FaultMessages);
            }

            if (gfResponse.QueryResults.Count == 0) {
                throw new CawException("Response format is invalid. QueryResult not exists.");
            }

            QueryResult queryResult = gfResponse.QueryResults[0];

            if (queryResult.FaultMessages != null &&
                queryResult.FaultMessages.Count > 0) {
                throw new CawFaultMessageException(queryResult.FaultMessages);
            }

            if (queryResult.ChoiceValues == null ||
                queryResult.ChoiceValues.Length == 0) {
                return new Feature[0];
            }

            QueryResultChoice choice = queryResult.ChoiceValues[0];

            if (choice.Choice != QueryResultChoiceType.AixmBasicMessage) {
                throw new CawException("QueryResult Choice not implemented.");
            }

            AixmBasicMessage aixmBasicMessage = choice.AixmBasicMessage;

            if (aixmBasicMessage.HasMember.Count == 0)
                return null;

            var featureList = new List<Feature>();

            for (int i = 0; i < aixmBasicMessage.HasMember.Count; i++) {
                foreach (Feature feature in aixmBasicMessage.HasMember[i]) {
                    featureList.Add(feature);
                }
            }

            return featureList.ToArray();
        }
    }

    public enum WorksPackageStatusType { Open, Close }
}
