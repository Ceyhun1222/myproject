using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim.DataTypes;
using Aran.Aim.Features;
using Aran.Aim.Objects;
using Aran.Queries.Common;
using Aran.Queries.Viewer;
using Aran.Aim.Metadata.UI;
using Aran.Aim.Data;
using Aran.Aim.Data.Filters;
using System.Reflection;
using Aran.Aim.Enums;
using System.Collections;
using Aran.Aim.DBService;
using System.Xml;
using System.IO;
using Aran.Controls;
using System.Threading;
using Aran.Aim.Utilities;
using Aran.Aim.AixmMessage;
using ASR = Aran.Geometries.SpatialReferences;
using AG = Aran.Geometries;
using Aran.PANDA.Common;

namespace Aran.Aim.InputFormLib
{
    public class InputFormController
    {
        private UIMetadata _metadata;
        private DbProvider _dbProvider;
        private Aran.Aim.FeatureInfo.ROFeatureViewer _featureInfo;
        private object _lockObject;
        private TimeSliceInterpretationType _lastInterpreationType;
        

        public InputFormController ()
        {
            _metadata = UIMetadata.Instance;
            IsFeatureClassified = true;

            foreach (AimClassInfo classInfo in _metadata.ClassInfoList)
            {
                UIClassInfo uiClassInfo = classInfo.UiInfo ();
                if (uiClassInfo.DependsFeature != null &&
                    (uiClassInfo.DependsFeature == "" ||
                    uiClassInfo.DependsFeature == " "))
                {
                    uiClassInfo.DependsFeature = null;
                }
            }

            _lockObject = new object ();

            PolyGeometryForm.SessionGeometriesLoaded = new SessionGeometriesEventHandler (SessionGeometriesLoaded);
            ForExportingList = new Dictionary<FeatureType, List<Feature>> ();
        }


        public FormClosingEventHandler ClosedEventHandler;
        public FeatureEventHandler SavedEventHandler;
        public event FeaturesLoadedEventHandler FeaturesLoaded;


        public DbProvider DbProvider
        {
            get { return _dbProvider; }
            set { _dbProvider = value; }
        }

        public bool IsFeatureClassified { get; set; }

        public List<string> GetFeaturesByDepends(string dependsFeature)
        {
            var list = new List<string>();

            if (IsFeatureClassified)
            {
                foreach (var classInfo in _metadata.ClassInfoList)
                {
                    var uiInfo = classInfo.UiInfo();

                    if (classInfo.AimObjectType == AimObjectType.Feature)
                    {
                        if (dependsFeature == uiInfo.DependsFeature)
                            list.Add(classInfo.Name);
                    }
                }
            }
            else
            {
                return _metadata.ClassInfoList.Where(ci => ci.AimObjectType == AimObjectType.Feature).Select(ci => ci.Name).ToList();
            }

            return list;
        }

        public void GetFeatures (
                        FeatureType featureType,
                        TimeSliceInterpretationType interpretationType,
                        DateTime dateTime,
                        Feature dependsFeature)
        {
            System.Threading.Thread thread = new System.Threading.Thread (
                new System.Threading.ParameterizedThreadStart (GetFeatures));

            thread.Start (new object [] {
                featureType,
                interpretationType,
                dateTime,
                dependsFeature});

        }

        public void FillColumns (FeatureType featureType, DataGridView dgv)
        {
            UIUtilities.FillColumns (_metadata.GetClassInfo ((int) featureType), dgv);
        }

        public void SetRow (DataGridView dgv, Feature feature, int rowIndex = -1)
        {
            UIUtilities.SetRow (dgv, feature, rowIndex);
        }

        public string GetFeatureDescription (Feature feature)
        {
            return UIUtilities.GetFeatureDescription (feature);
        }

        public Control GetPropertiesControl (Feature feature)
        {
            AimClassInfo classInfo = _metadata.ClassInfoList.Where (ci => ci.Index == (int) feature.FeatureType).First ();

            if (feature.Id >= 0)
                feature = LoadFullFeature (feature);

            FeatureControl featureControl = new FeatureControl ();
            featureControl.Closed += ClosedEventHandler;
            featureControl.Saved += SavedEventHandler;
            featureControl.GetFeature += new GetFeatureHandler (FeatureControl_GetFeature);
            featureControl.DataGridColumnsFilled = new FillDataGridColumnsHandler (FillDataGridColumns);
            featureControl.GetFeatListByDepend += new FeatureListByDependEventHandler (GetFeaturesSingleThread);
            featureControl.DataGridRowSetted = new SetDataGridRowHandler (SetRow);

            featureControl.LoadFeature (feature, classInfo);

            return featureControl;
        }

        public string GetDependsFeature (Feature feature)
        {
            AimClassInfo classInfo = AimMetadata.GetClassInfoByIndex ((int) feature.FeatureType);
            UIClassInfo uiClassInfo = classInfo.UiInfo ();
            return uiClassInfo.DependsFeature;
        }

        public Feature CreateNewFeature (FeatureType featureType, Feature dependsFeature)
        {
            Feature newFeature = AimObjectFactory.CreateFeature (featureType);
            newFeature.Id = -1;
            newFeature.Identifier = Guid.NewGuid ();
            newFeature.TimeSlice = new Aran.Aim.DataTypes.TimeSlice ();
            newFeature.TimeSlice.Interpretation = Aran.Aim.Enums.TimeSliceInterpretationType.PERMDELTA;
            newFeature.TimeSlice.ValidTime = new Aran.Aim.DataTypes.TimePeriod (DateTime.Now);
            newFeature.TimeSlice.SequenceNumber = 1;
            newFeature.TimeSlice.CorrectionNumber = 0;
            newFeature.TimeSlice.FeatureLifetime = newFeature.TimeSlice.ValidTime;


            #region Set depends value
            if (dependsFeature != null)
            {
                AimClassInfo classInfo = AimMetadata.GetClassInfoByIndex ((int) featureType);
                AimPropInfo propInfo = classInfo.Properties.Where (
					pi => pi.ReferenceFeature == dependsFeature.FeatureType).FirstOrDefault ();
				
                if (propInfo != null)
                {
                    if (propInfo.IsList)
                    {
						var frObj = (FeatureRefObject) AimObjectFactory.Create (propInfo.TypeIndex);
						frObj.Feature = new FeatureRef (dependsFeature.Identifier);

                        var listPropVal = ((IAimObject) newFeature).GetValue (propInfo.Index);

						if (listPropVal == null)
						{
							listPropVal = AimObjectFactory.CreateList (propInfo.TypeIndex) as IAimProperty;
							((IAimObject) newFeature).SetValue (propInfo.Index, listPropVal);
						}
						(listPropVal as IList).Add (frObj);
                    }
                    else
                    {
						var featureRef = (FeatureRef) AimObjectFactory.Create (propInfo.TypeIndex);
						featureRef.Identifier = dependsFeature.Identifier;

                        ((IAimObject) newFeature).SetValue (propInfo.Index, featureRef);
                    }
                }
            }
            #endregion

            return newFeature;
        }

        public void ViewFeature (Feature feature, IWin32Window owner)
        {
            feature = LoadFullFeature (feature);

            var featList = new List<Feature> ();
            featList.Add (feature);

            if (_featureInfo == null)
            {
                _featureInfo = new Aran.Aim.FeatureInfo.ROFeatureViewer ();
                _featureInfo.GettedFeature = FeatureControl_GetFeature;
                _featureInfo.SetOwner (owner);
            }

            _featureInfo.ShowFeaturesForm (featList, true);
        }

		public void ViewFeatureHistory (Feature feature, IWin32Window owner)
		{
			var ahList = _dbProvider.GetAllFeatuers (feature.FeatureType, feature.Identifier);

			var fh = new Aran.Aim.FeatureInfo.FeatureHistoryForm ();
			fh.GettedFeature = FeatureControl_GetFeature;
			fh.Open (ahList);
			fh.Show (owner);
		}

        public string SaveFeature (Feature newFeature)
        {
            int corr = newFeature.TimeSlice.CorrectionNumber;
            if (newFeature.Id != -1)
            {
                if (InsertAsCorrection)
                {
                    newFeature.TimeSlice.CorrectionNumber++;
                }
                else
                {
                    {
                        newFeature.TimeSlice.SequenceNumber++;
                        newFeature.TimeSlice.CorrectionNumber = 0;
                    }
                }
            }

            if (newFeature is RouteSegment)
                SetCurveExtenByStartAndEndPoints(newFeature as RouteSegment);

//#warning ForDebug - Insert in workpackage ...

//            //--- 1. del
//            var transId = _dbProvider.BeginTransaction ();

            //--- 2. restore
            InsertingResult result = _dbProvider.Insert (newFeature, true, InsertAsCorrection);

            //--- 3. del
            //InsertingResult result = _dbProvider.Insert (newFeature, transId, false, InsertAsCorrection);

            if (!result.IsSucceed)
            {
                // It means that you have not latest version.
                // So first you have to update
                if (InsertAsCorrection)
                {
                    newFeature.TimeSlice.CorrectionNumber--;
                }
                else
                {
                    newFeature.TimeSlice.SequenceNumber--;
                    newFeature.TimeSlice.CorrectionNumber = corr;
                }
            }
            return result.Message;
        }

        /// <summary>
        /// Just sets value to LifeTime/EndPosition property
        /// </summary>
        /// <param name="feature"></param>
        public void DeleteFeature (Feature feature, DateTime endTime)
        {
            feature.TimeSlice.FeatureLifetime.EndPosition = endTime;
            _dbProvider.Insert(feature, true, false);
        }

        public List<Exception> ExportToXML (string xmlFileName, bool writeExtension, bool loadFeatureAllVersion, bool write3DifExists, SrsNameType srsType)
        {
            List<Feature> featureList = new List<Feature> ();

            Data.GettingResult featTypesResult = _dbProvider.GetAllStoredFeatTypes ();

            StringBuilder errorString  = new StringBuilder ();

            var excList = new List<Exception> ();

            if (featTypesResult.IsSucceed)
            {
                var featTypes = featTypesResult.List as List<FeatureType>;

                foreach (FeatureType featType in featTypes)
                {
                    try
                    {
						if (loadFeatureAllVersion)
						{
							var fl = _dbProvider.GetAllFeatuers (featType);
							featureList.AddRange (fl);
						}
						else
						{
							var gr = _dbProvider.GetVersionsOf (featType, _lastInterpreationType);
							if (!gr.IsSucceed)
								throw new Exception (gr.Message);
							featureList.AddRange (gr.GetListAs<Feature> ());
						}
                    }
                    catch (Exception ex)
                    {
                        excList.Add (ex);
                    }
                }
            }

            try
            {
                WriteAllFeatureToXML(featureList, xmlFileName, writeExtension, write3DifExists, loadFeatureAllVersion ? (DateTime?)null :_dbProvider.DefaultEffectiveDate, srsType);
            }
            catch (Exception ex)
            {
                excList.Insert (0, ex);
            }

            return excList;
        }

        public List<Exception> ExportSelectedToXml(string xmlFileName, bool serializableExtension, bool includeRefFeatures, bool write3DifExists, SrsNameType srsType)
        {
            try {
                var dbUtil = new DbUtility(_dbProvider);
                var defaultInterpretationType = TimeSliceInterpretationType.BASELINE;
                dbUtil.SetDefault(defaultInterpretationType, DefaultEffectiveDate);

                var featList = new List<Feature>();
                var errorList = new List<string>();

                foreach (var featType in ForExportingList.Keys) {
                    var selFeatList = ForExportingList[featType];

                    foreach (var feat in selFeatList) {
                        if (!includeRefFeatures) {
                            var gr = _dbProvider.GetVersionsOf(featType, defaultInterpretationType, feat.Identifier, true,
                                    new TimeSliceFilter(DefaultEffectiveDate), null, null);

                            if (gr.IsSucceed)
                                featList.AddRange(gr.GetListAs<Feature>());
                        }
                        else
                            dbUtil.LoadWithRefFeatures(featType, feat.Identifier, featList, errorList);
                    }
                }

                WriteAllFeatureToXML(featList, xmlFileName, serializableExtension, write3DifExists, DefaultEffectiveDate, srsType);
                return null;
            }
            catch (Exception ex) {
                var list = new List<Exception>();
                list.Add(ex);
                return list;
            }
        }

        public bool ExportAsSeperate(string selectedPath, bool isWriteExtension, bool includeRefFeatures, bool isWrite3DisExists, SrsNameType srsType, out string resultMessage)
        {
            resultMessage = null;

            var dbUtil = new DbUtility(_dbProvider);
            dbUtil.SetDefault(TimeSliceInterpretationType.BASELINE, DefaultEffectiveDate);

            var dirName = selectedPath + "\\" + InputFormController.DefaultEffectiveDate.ToString("yyyy-MM-dd");
            var deltaStr = "";
            var n = 1;

            while (Directory.Exists(dirName + deltaStr)) {
                deltaStr = " (" + n + ")";
                n++;

                if (n > 20)
                    return false;
            }
            dirName = dirName + deltaStr;
            Directory.CreateDirectory(dirName);
            var exList = new List<Exception>();

            foreach (var featType in ForExportingList.Keys) {
                var subDirName = dirName + "\\" + featType;
                Directory.CreateDirectory(subDirName);
                var featList = ForExportingList[featType];

                foreach (var feat in featList) {
                    bool hasDesc;
                    var str = Aran.Aim.Metadata.UI.UIUtilities.GetFeatureDescription(feat, out hasDesc);
                    var fileName = (hasDesc ? str : feat.Identifier.ToString());

                    var fl = new List<Feature>();

                    if (!includeRefFeatures) {
                        var gr = _dbProvider.GetVersionsOf(featType, TimeSliceInterpretationType.BASELINE, feat.Identifier, true,
                                        new TimeSliceFilter(DefaultEffectiveDate), null, null);

                        if (gr.IsSucceed)
                            fl.AddRange(gr.GetListAs<Feature>());
                    }
                    else {
                        var errorList = new List<string>();
                        dbUtil.LoadWithRefFeatures(featType, feat.Identifier, fl, errorList);
                    }

                    try {
                        InputFormController.WriteAllFeatureToXML(fl, subDirName + "\\" + fileName + ".xml",
                            isWriteExtension, isWrite3DisExists, DefaultEffectiveDate, srsType);
                    }
                    catch (Exception ex) {
                        exList.Add(ex);
                    }
                }
            }

            if (exList.Count == 0) {
                resultMessage = "All selected features exproted successfully!";
                return true;
            }
            else {
                var s = string.Join<string>("\n", exList.Select(ex => ex.Message));
                if (s.Length > 1000)
                    s = s.Substring(1000) + " ... ";

                resultMessage = "Some of features does not exproted!\n\n";
                return false;
            }
        }

        public void Import(string xmlFileName)
        {
            var parseXml = new ParseXmlFile();
            parseXml.Parse(xmlFileName);

            #region NON_GUID_IDENTIFIER

            if (Aran.Aim.DataTypes.FeatureRef.NonGuidIdentifier)
            {
                var dict = parseXml.Features.ToDictionary(f => f.Identifier, f => f);
                foreach (var pair in Aran.Aim.DataTypes.FeatureRef.GuidWaitingList)
                {
                    foreach (var featRef in pair.Value)
                    {
                        if (featRef is IAbstractFeatureRef)
                        {
                            Feature feat;
                            if (dict.TryGetValue(featRef.Identifier, out feat))
                                (featRef as IAbstractFeatureRef).FeatureTypeIndex = (int)feat.FeatureType;
                        }
                    }
                }
                foreach (var pair in Aran.Aim.DataTypes.FeatureRef.GuidAssociteList)
                {
                    Feature feat;
                    if (dict.TryGetValue(pair.Value, out feat))
                    {
                        var note = new Note();
                        note.Purpose = CodeNotePurpose.DISCLAIMER;
                        note.TranslatedNote.Add(new LinguisticNote() { Note = new TextNote { Lang = language.ENG, Value = pair.Key } });
                        feat.Annotation.Add(note);
                    }
                }
            }

            #endregion

            var errorInfoList = new List<DeserializedErrorInfo>(parseXml.ErrorInfoList);

            errorInfoList.ForEach(ei =>
            {
                if (string.IsNullOrEmpty(ei.Action))
                    ei.Action = "Property Ignored";
            });

            
            #region Check Non Existing Feature Reference

            var nonExistingRefFeaturePropList = CheckNonExistingFeatureRef(parseXml.Features);

            if (nonExistingRefFeaturePropList.Count > 0)
            {
                var sss = string.Empty;

                foreach (var item in nonExistingRefFeaturePropList)
                {
                    Utilities.AimMetadataUtility.SetValue(item.Feature, item.PropInfoList.ToArray(), item.ListPropIndexes, null);

                    var propPath = string.Empty;

                    if (item.PropInfoList.Count > 0)
                    {
                        item.PropInfoList.ForEach(pi => propPath += pi.AixmName + "/");
                        propPath = propPath.Remove(propPath.Length - 1);
                    }

                    errorInfoList.Add(new DeserializedErrorInfo
                    {
                        FeatureType = item.Feature.FeatureType,
                        Identifier = item.Feature.Identifier,
                        PropertyName = propPath,
                        ErrorMessage = string.Format("Non existing reference detected: ({0}: {1})",
                            (item.PropInfoList.Count == 0 ? "" : item.PropInfoList.Last().ReferenceFeature.ToString()),
                            item.RefIdentifier),
                        Action = "Property Cleared"
                    });

                    #region NON_GUID_IDENTIFIER

                    if (Aran.Aim.DataTypes.FeatureRef.NonGuidIdentifier)
                    {
                        foreach (var pair in FeatureRef.GuidAssociteList)
                        {
                            if (pair.Value == item.RefIdentifier)
                            {
                                sss += pair.Key + "\r\n";
                                break;
                            }
                        }
                    }

                    #endregion
                }
            }

            #endregion

            if (errorInfoList.Count > 0)
            {
                //*** Show Error Info.

                var errRepForm = new ParserErrorReportForm();
                errRepForm.ErrorInfoList.AddRange(errorInfoList);
                if (errRepForm.ShowDialog() != DialogResult.OK)
                    return;
            }


            var airacSelForm = new AiracSelectorForm();
            airacSelForm.SetFeatureCount(parseXml.Features.Count);
            if (airacSelForm.ShowDialog() != DialogResult.OK)
                return;

            var beginPos = airacSelForm.BeginValidTimeDateTime;

            foreach (var feat in parseXml.Features)
            {
                feat.TimeSlice.Interpretation = TimeSliceInterpretationType.BASELINE;
                feat.TimeSlice.ValidTime = new TimePeriod(beginPos);
                feat.TimeSlice.SequenceNumber = 1;
                feat.TimeSlice.CorrectionNumber = 0;
                if (feat.TimeSlice.FeatureLifetime == null)
                    feat.TimeSlice.FeatureLifetime = new TimePeriod();
                feat.TimeSlice.FeatureLifetime.BeginPosition = feat.TimeSlice.ValidTime.BeginPosition;
                feat.TimeSlice.FeatureLifetime.EndPosition = null;
            }

            string errorMessage = InsertFeatures(parseXml.Features); //.Where(f => f.FeatureType == FeatureType.VerticalStructure).ToList());

            if (errorMessage != string.Empty)
                MessageBox.Show(errorMessage, "Couldn't finish importing", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                MessageBox.Show("Finished to import");
        }

        public void SessionGeometriesLoaded (object sender, SessionGeometriesEventArgs e)
        {
            if (GeomCreatorForm.GeometryDictionary == null)
                return;

            foreach (string key in GeomCreatorForm.GeometryDictionary.Keys)
            {
                e.GeometriesDict.Add (key, GeomCreatorForm.GeometryDictionary [key]);
            }
        }

        public bool InsertAsCorrection
        {
            get;
            set;
        }

        public void SaveUIMetadata ()
        {
            UIMetadata.Instance.Save ();
        }

        public Feature FeatureControl_GetFeature (FeatureType featureType, Guid identifier)
        {
			GettingResult getResult = _dbProvider.GetVersionsOf ( featureType, TimeSliceInterpretationType.BASELINE, identifier, false, new TimeSliceFilter ( DefaultEffectiveDate ) );
            if (getResult.IsSucceed && getResult.List.Count > 0)
            {
                return (Feature) getResult.List [0];
            }
            return null;
        }

        public Dictionary<FeatureType, List<Feature>> ForExportingList { get; private set; }

        public void AddForExporting (FeatureType featureType, Feature feat)
        {
            List<Feature> featList;
            if (ForExportingList.TryGetValue (featureType, out featList))
            {
                featList = ForExportingList [featureType];

                if (!featList.Exists(ft => ft.Identifier == feat.Identifier))
                    featList.Add (feat);
            }
            else
            {
                featList = new List<Feature> ();
                featList.Add (feat);
                ForExportingList.Add (featureType, featList);
            }
        }

        public void FillDataGridColumns (AimClassInfo classInfo, DataGridView dgv)
        {
            dgv.Rows.Clear ();
            dgv.Columns.Clear ();

            foreach (AimPropInfo propInfo in classInfo.Properties)
            {
                UIPropInfo uiPropInfo = propInfo.UiPropInfo ();
                if (uiPropInfo.ShowGridView)
                {
                    DataGridViewColumn dgvColumn = ToColumn (propInfo);
                    if (dgvColumn != null)
                    {
                        dgv.Columns.Add (dgvColumn);
                    }
                }
            }
        }

        public Feature LoadFullFeature (Feature feature)
        {
            var gr = _dbProvider.GetVersionsOf (
                feature.FeatureType,
                _lastInterpreationType,
                feature.Identifier,
                true,
                new TimeSliceFilter(DefaultEffectiveDate),
                null, null);

            if (!gr.IsSucceed)
                throw new Exception (gr.Message);

            if (gr.List.Count == 0)
                throw new Exception ("Feature Not Found, Identifier: " + feature.Identifier);

            return gr.List [0] as Feature;
        }

        public void GetFeaturesSingleThread (object sender, FeatureListByDependEventArgs e)
        {
            var featLoadedArgs = GetFeaturesEventArgs (new object [] {
                                    e.FeatureType,
                                    e.InterpretationType,
                                    e.EffectiveDate,
                                    e.DependFeature});

            if (featLoadedArgs.Exception != null)
            {
                throw featLoadedArgs.Exception;
            }

            e.FeatureList.AddRange (featLoadedArgs.FeatureList);
        }



        private void GetFeatures (object arg)
        {
            if (FeaturesLoaded == null)
                return;

            var featLoadedArgs = GetFeaturesEventArgs (arg);
            FeaturesLoaded (this, featLoadedArgs);
        }

        private FeatureListLoadedEventArgs GetFeaturesEventArgs (object arg)
        {
            var argArr = arg as object [];
            var featureType = (FeatureType) argArr [0];
            var interpretationType = (TimeSliceInterpretationType) argArr [1];
            var effectiveDate = (DateTime) argArr [2];
            var dependsFeature = (Feature) argArr [3];

            var featLoadedArgs = new FeatureListLoadedEventArgs (featureType);

            try
            {
                lock (_lockObject)
                {
                    _lastInterpreationType = interpretationType;

                    Aran.Aim.Data.Filters.Filter filter = null;

                    if (dependsFeature != null)
                    {
                        string propertyName = GetFeatureRelationPropName (featureType, dependsFeature.FeatureType);
                        if (propertyName != null)
                        {
                            object identifier = dependsFeature.Identifier;
                            Aran.Aim.Data.Filters.ComparisonOps compOp = new Data.Filters.ComparisonOps (Data.Filters.ComparisonOpType.EqualTo, propertyName, identifier);
                            filter = new Data.Filters.Filter (new Data.Filters.OperationChoice (compOp));
                        }
                    }

                    var propList = GetVisiblePropList (featureType);

                    if (propList.Count == 0 || (propList.Count == 1 && propList [0] == "Identifier"))
                        propList = null;

                    var getResult = _dbProvider.GetVersionsOf (
                        featureType,
                        interpretationType,
                        default (Guid),
                        false,
                        new TimeSliceFilter(effectiveDate),
                        propList,
                        filter);

                    if (!getResult.IsSucceed)
                        throw new Exception (getResult.Message);

                    for (int i = 0; i < getResult.List.Count; i++)
                        featLoadedArgs.FeatureList.Add (getResult.List [i] as Feature);

                }
            }
            catch (Exception ex)
            {
                featLoadedArgs.Exception = ex;
            }

            return featLoadedArgs;
        }

        private Feature [] GetFeaturesSingleThread (
                        FeatureType featureType,
                        TimeSliceInterpretationType interpretationType,
                        DateTime dateTime,
                        Feature dependsFeature)
        {
            var featLoadedArgs = GetFeaturesEventArgs (new object [] {
                featureType,
                interpretationType,
                dateTime,
                dependsFeature});

            if (featLoadedArgs.Exception != null)
            {
                throw featLoadedArgs.Exception;
            }

            return featLoadedArgs.FeatureList.ToArray ();
        }

        private string InsertFeatures (List<Feature> featureList)
        {
            int transactionId = _dbProvider.BeginTransaction ();

            try
            {
                InsertingResult insertResult;
                foreach (var feat in featureList)
                {
                    Feature newFeat = AimObjectFactory.CreateFeature (feat.FeatureType);
                    newFeat.Identifier = feat.Identifier;
                    newFeat.TimeSlice = feat.TimeSlice;
                    
                    insertResult = _dbProvider.Insert (newFeat, transactionId);

                    if (!insertResult.IsSucceed)
                    {
                        _dbProvider.Rollback (transactionId);
                        return $"{feat.FeatureType} ({feat.Identifier})\r\n{insertResult.Message}";
                    }
                }

                foreach (var feat in featureList)
                {
                    insertResult = _dbProvider.Update ( feat, transactionId );

					if ( !insertResult.IsSucceed )                    
					{
                        _dbProvider.Rollback (transactionId);
					    return $"{feat.FeatureType} ({feat.Identifier})\r\n{insertResult.Message}";
                    }
                }
            }
            catch (Exception ex)
            {
                _dbProvider.Rollback (transactionId);
                throw ex;
            }

            _dbProvider.Commit (transactionId);
            return string.Empty;
        }

        private Dictionary<Feature, string> InsertFeatures (List<Feature> allFeatList,
            Dictionary<Feature, ReasonNotInserted> notInsertedFeatList, List<Feature> insertedFeatList, ref int transId)
        {
            bool transCreated = (transId < 0);
            if (transCreated)
                transId = _dbProvider.BeginTransaction ();

            Dictionary<Feature, string> couldntInsertedList = null;

            //try
            {
                InsertingResult insertResult;
                Feature feat;
                couldntInsertedList = new Dictionary<Feature, string> ();
                bool isInsertedReasonFeat = false;
                int countOfInsertedFeat = 0;
                //int transactionId;
                ReasonNotInserted reasonNotInserted;
                for (int i = 0; i <= allFeatList.Count - 1; i++)
                {
                    feat = allFeatList [i];

                    //transactionId = _dbProvider.BeginTransaction ( );
                    insertResult = _dbProvider.Insert (feat, transId, true);
                    //_dbProvider.Roollback ( transactionId );

                    if (!insertResult.IsSucceed)
                    {
                        if (insertResult.Message.StartsWith ("ERROR: 23503:") &&
                                insertResult.Message.Contains ("violates foreign key constraint") &&
                                insertResult.Message.Contains ("is not present in table \"features\""))

                        // Reference feature has to be inserted first
                        {
                            reasonNotInserted = CreateReasonNotInserted (feat, insertResult.Message);

                            //notInsertedFeatList.Add ( feat,  );
                        }
                        else
                            couldntInsertedList.Add (feat, insertResult.Message);
                    }
                    else
                    {
                        insertedFeatList.Add (feat);

                        List<Feature> featList = notInsertedFeatList.Keys.ToList<Feature> ();
                        foreach (Feature notInsertedFeat in featList)
                        {
                            if (notInsertedFeatList [notInsertedFeat].FeatType == feat.FeatureType &&
                                    notInsertedFeatList [notInsertedFeat].Identifier == feat.Identifier)
                            {
                                notInsertedFeatList [notInsertedFeat].IsReasonFeatInserted = true;
                                countOfInsertedFeat++;
                                isInsertedReasonFeat = true;
                            }
                        }
                    }
                }
                if (isInsertedReasonFeat)
                {
                    List<Feature> allFeatListHaveToBeInserted = SeparateFeatList (notInsertedFeatList);
                    Dictionary<Feature, string> result = InsertFeatures (
                        allFeatListHaveToBeInserted, notInsertedFeatList, insertedFeatList, ref transId);
                    foreach (Feature featInResult in result.Keys.ToList<Feature> ())
                    {
                        couldntInsertedList.Add (featInResult, result [featInResult]);
                    }
                }

                //if ( transCreated )
                //    _dbProvider.Commit ( transId );

            }
            //catch (Exception ex)
            //{
            if (transCreated && couldntInsertedList.Count > 0)
                _dbProvider.Rollback (transId);
            //    throw ex;
            //}

            return couldntInsertedList;
        }

        private List<Feature> SeparateFeatList (Dictionary<Feature, ReasonNotInserted> notInsertedFeatList)
        {
            List<Feature> result = new List<Feature> ();
            foreach (Feature feat in notInsertedFeatList.Keys.ToList<Feature> ())
            {
                if (notInsertedFeatList [feat].IsReasonFeatInserted == true)
                {
                    result.Add (feat);
                }
            }

            foreach (Feature feat in result)
            {
                notInsertedFeatList.Remove (feat);
            }
            //result.Reverse ( );
            return result;
        }

        private ReasonNotInserted CreateReasonNotInserted (Feature feat, string message)
        {
            ReasonNotInserted result = new ReasonNotInserted ();
            int indexOfKey = message.IndexOf ("Key (");
            string propName = message.Substring (indexOfKey + 5, message.IndexOf (")", indexOfKey) - indexOfKey - 5);
            indexOfKey = message.IndexOf ("=(");
            int i = message.IndexOf (") is not present in table \"features\"");
            string identifier = message.Substring (indexOfKey + 2, i - indexOfKey - 2);
            AimPropInfo aimPropInfo = null;
            AimPropInfoList insidePropList = new AimPropInfoList ();

            aimPropInfo = GetPropInfo ((int) feat.FeatureType, propName, insidePropList);
            result.FeatType = aimPropInfo.ReferenceFeature;
            result.Identifier = new Guid (identifier);
            result.IsReasonFeatInserted = false;
            return result;


            //object prop = feat;
            //for ( int i = insidePropList.Count - 1; i >= 0; i-- )
            //{
            //    propName = insidePropList [i].Name;
            //    if ( insidePropList [i].IsList )
            //    {

            //    }
            //    PropertyInfo netPropInfo = prop.GetType ().GetProperty ( propName );
            //    prop = netPropInfo.GetValue ( prop, null );
            //}
            //result.Identifier = ( prop as FeatureRef ).Identifier;
            //result.IsReasonFeatInserted = false;
            //return result;
        }

        private AimPropInfo GetPropInfo (int typeIndex, string propName, AimPropInfoList insidePropList)
        {
            AimPropInfo [] propInfos = AimMetadata.GetAimPropInfos (typeIndex);
            AimPropInfo result = FindPropInfo (propInfos, propName, insidePropList);
            if (result == null)
            {
                foreach (AimPropInfo propInfo in propInfos)
                {
                    if (propInfo.PropType != null && propInfo.PropType.AimObjectType != AimObjectType.Field && propInfo.Name.ToLower () != "timeslice")
                    {
                        result = GetPropInfo (propInfo.TypeIndex, propName, insidePropList);
                        if (result != null)
                        {
                            insidePropList.Add (propInfo);
                            return result;
                        }
                    }
                }
            }
            return result;
        }

        private AimPropInfo FindPropInfo (AimPropInfo [] propInfos, string propName, AimPropInfoList insidePropList)
        {
            foreach (AimPropInfo propInfo in propInfos)
            {
                if (propInfo.Name == propName)
                {
                    insidePropList.Add (propInfo);
                    return propInfo;
                }
            }
            return null;
        }

        private DataGridViewColumn ToColumn (AimPropInfo propInfo)
        {
            DataGridViewColumn dgvColumn = null;

            AimClassInfo propTypeClassInfo = propInfo.PropType;

            bool b = AimMetadata.IsEnum (propTypeClassInfo.Index);

            if (propTypeClassInfo.AimObjectType == AimObjectType.Field)
            {
                AimFieldType aimFieldType = (AimFieldType) propTypeClassInfo.Index;

                switch (aimFieldType)
                {
                    case AimFieldType.SysString:
                    case AimFieldType.SysGuid:
                        {
                            DataGridViewTextBoxColumn tbCol = new DataGridViewTextBoxColumn ();
                            dgvColumn = tbCol;
                        }
                        break;
                    case AimFieldType.SysBool:
                        DataGridViewCheckBoxColumn chbCol = new DataGridViewCheckBoxColumn ();
                        dgvColumn = chbCol;
                        break;
                    default:
                        {
                            if (propTypeClassInfo.SubClassType == AimSubClassType.Enum)
                            {
                                DataGridViewTextBoxColumn tbCol = new DataGridViewTextBoxColumn ();
                                dgvColumn = tbCol;
                            }
                        }
                        break;
                }
            }
            else if (propTypeClassInfo.AimObjectType == AimObjectType.DataType)
            {
                if (propTypeClassInfo.SubClassType == AimSubClassType.ValClass)
                {
                    DataGridViewTextBoxColumn tbCol = new DataGridViewTextBoxColumn ();
                    dgvColumn = tbCol;
                }
            }

            if (dgvColumn != null)
            {
                dgvColumn.Name = propInfo.Name;
                dgvColumn.Tag = propInfo;
            }

            return dgvColumn;
        }

        private List<Utilities.RefFeatureProp> CheckNonExistingFeatureRef (List<Feature> featureList)
        {
            var guidList = new List<Guid> ();
            foreach (var feat in featureList)
                guidList.Add (feat.Identifier);

            var nonExistingRefFeaturePropList = new List<Utilities.RefFeatureProp> ();

            foreach (var feat in featureList)
            {
                var refFeaturePropList = new List<Utilities.RefFeatureProp> ();
                Utilities.AimMetadataUtility.GetReferencesFeatures (feat, refFeaturePropList);

                foreach (var refFeatureProp in refFeaturePropList)
                {
                    if (!guidList.Contains (refFeatureProp.RefIdentifier))
                    {
                        if (!_dbProvider.IsExists (refFeatureProp.RefIdentifier) 
                            && !_dbProvider.IsExists(refFeatureProp.RefIdentifier, refFeatureProp.FeatureType))
                        {
                            nonExistingRefFeaturePropList.Add (refFeatureProp);
                            refFeatureProp.Feature = feat;
                        }
                    }
                }
            }

            return nonExistingRefFeaturePropList;
        }

        public static void WriteAllFeatureToXML(List<Feature> featureList, string xmlFileName, bool writeExtension, bool write3DifExists, DateTime? effectiveDate, SrsNameType srsType)
        {
            var writerSettings = new XmlWriterSettings ();
            writerSettings.NewLineOnAttributes = false;
            writerSettings.Indent = true;
            writerSettings.NamespaceHandling = NamespaceHandling.OmitDuplicates;

            var xmlWriter = XmlWriter.Create (xmlFileName, writerSettings);

            var aixmBasicMessage = new AixmBasicMessage(MessageReceiverType.Panda, srsType);
            aixmBasicMessage.Write3DCoordinateIfExists = write3DifExists;
            aixmBasicMessage.WriteExtension = writeExtension;
            aixmBasicMessage.EffectiveDate = effectiveDate;

            foreach (Feature feature in featureList)
            {
                var afl = new AixmFeatureList ();
                afl.Add (feature);
                aixmBasicMessage.HasMember.Add (afl);
            }

            featureList.Clear();

            aixmBasicMessage.WriteXml (xmlWriter);
            xmlWriter.Close ();
        }


        private static List<string> GetVisiblePropList (FeatureType featureType)
        {
            var classInfo = AimMetadata.GetClassInfoByIndex ((int) featureType);
            var propList = new List<string> ();

            foreach (var propInfo in classInfo.Properties)
            {
                var uiPropInfo = propInfo.UiPropInfo ();
                if (uiPropInfo.ShowGridView)
                    propList.Add (propInfo.Name);
            }

            return propList;
        }

        private static string GetFeatureRelationPropName (FeatureType featureType, FeatureType propertyFeatureType)
        {
            AimClassInfo classInfo = AimMetadata.GetClassInfoByIndex ((int) featureType);
            AimPropInfo propInfo = classInfo.Properties.Where (pi => pi.ReferenceFeature == propertyFeatureType).FirstOrDefault ();
            if (propInfo != null)
                return propInfo.AixmName;

            return null;
        }

        public static DateTime DefaultEffectiveDate { get; set; }

        private bool SetCurveExtenByStartAndEndPoints(RouteSegment routeSegment)
        {
            if (routeSegment.CurveExtent != null ||

                routeSegment.Start == null ||
                routeSegment.Start.PointChoice == null ||
                routeSegment.Start.PointChoice.FixDesignatedPoint == null ||

                routeSegment.End == null ||
                routeSegment.End.PointChoice == null ||
                routeSegment.End.PointChoice.FixDesignatedPoint == null)

                return false;

            DesignatedPoint startDP = null;
            DesignatedPoint endDP = null;

            var gr = _dbProvider.GetVersionsOf(FeatureType.DesignatedPoint,
                TimeSliceInterpretationType.BASELINE,
                routeSegment.Start.PointChoice.FixDesignatedPoint.Identifier);

            if (!gr.IsSucceed || gr.List.Count == 0)
                return false;

            startDP = gr.List[0] as DesignatedPoint;

            if (startDP.Location == null)
                return false;

            gr = _dbProvider.GetVersionsOf(FeatureType.DesignatedPoint,
                TimeSliceInterpretationType.BASELINE,
                routeSegment.End.PointChoice.FixDesignatedPoint.Identifier);

            if (!gr.IsSucceed || gr.List.Count == 0)
                return false;

            endDP = gr.List[0] as DesignatedPoint;

            if (endDP.Location == null)
                return false;

            var ls = new Aran.Geometries.LineString();
            ls.Add(startDP.Location.Geo);
            ls.Add(endDP.Location.Geo);

            routeSegment.CurveExtent = new Curve();
            routeSegment.CurveExtent.Geo.Add(ls);

            return true;
        }
	}

    public delegate void FeaturesLoadedEventHandler (object sender, FeatureListLoadedEventArgs e);

    public class FeatureListLoadedEventArgs : EventArgs
    {
        public FeatureListLoadedEventArgs (FeatureType featureType)
        {
            FeatureList = new List<Feature> ();
            FeatureType = featureType;
        }

        public List<Feature> FeatureList { get; private set; }

        public FeatureType FeatureType { get; private set; }

        public Exception Exception { get; set; }
    }
}
