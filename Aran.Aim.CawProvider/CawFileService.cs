using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Features;
using System.IO;
using Aran.Package;
using Aran.Aim.Package;
using Aran.Aim.DataTypes;
using System.Collections;
using Aran.Aim.Objects;

namespace Aran.Aim.CAWProvider
{
    internal class CawFileService : ICawService
    {
        public CawFileService ()
        {
            _transactionFeaturesDic = new Dictionary<int, List<Feature>> ();
            _lastWorkPackageId = 1;
        }

        public ConnectionInfo ConnectionInfo
        {
            get { return _connectionInfo; }
            set 
            {
                if (!value.Server.IsAbsoluteUri)
                    throw new Exception ("URI is not file path");

                _connectionInfo = value;
                _folderName = _connectionInfo.Server.LocalPath;
            }
        }

        public int BeginTransaction ()
        {
            _transactionFeaturesDic.Add (_lastWorkPackageId, new List<Feature> ());
            int rv = _lastWorkPackageId;
            _lastWorkPackageId++;
            return rv;
        }

        public void Commit (int workPackageId, bool save)
        {
            List<Feature> featureList = _transactionFeaturesDic [workPackageId];
            _transactionFeaturesDic.Remove (workPackageId);

            if (!save)
            {
                foreach (var item in featureList)
                {
                    DeleteFeature (item);
                }
            }
        }

        public bool InsertFeature (Feature feature, int? workpackageId)
        {
            if (workpackageId != null)
            {
                List<Feature> featureList = _transactionFeaturesDic [workpackageId.Value];
                featureList.Add (feature);
            }

            return SaveFeature (feature);
        }

        public Feature [] GetFeature (AbstractRequest query, int? workPackageId)
        {
            SimpleQuery sq = null;
            if (query is SimpleQuery)
            {
                sq = (SimpleQuery) query;

                if (sq.FeatureType == 0 && sq.Filter != null &&
                    sq.Filter.Operation.Choice == OperationChoiceType.Comparison &&
                    sq.Filter.Operation.ComparisonOps.PropertyName == "createTime")
                {
                    return GetAllFeatureByCreateDate ((DateTime) sq.Filter.Operation.ComparisonOps.Value);
                }

                return GetFeature (sq.FeatureType, sq.IdentifierList);
            }
            else if (query is LinkQuery)
            {
                LinkQuery lq = (LinkQuery) query;

                return GetLinkFeature (
                    lq.FeatureTypeList [0], 
                    lq.SimpleQuery.IdentifierList [0],
                    lq.TraverseTimeslicePropertyName);
            }
            
            return null;
        }

        public List<TFeature> GetFeature<TFeature> (AbstractRequest query) where TFeature : Feature
        {
            List<TFeature> list = new List<TFeature> ();
            Feature [] featureArr = GetFeature (query, null);
            foreach (var item in featureArr)
            {
                list.Add ((TFeature) item);
            }
            return list;
        }


        private Feature [] GetFeature (FeatureType featureType, List<Guid> identifierList)
        {
            List<Feature> featureList = new List<Feature> ();
            string featureFolder = _folderName + "\\" + featureType;
            
            if (Directory.Exists (featureFolder))
            {
                List<string> fileNameList = new List<string> ();
                
                if (identifierList.Count > 0)
                {
                    foreach (Guid identifier in identifierList)
                    {
                        string fn = featureFolder + "\\" + identifier + ".dat";
                        if (File.Exists (fn))
                            fileNameList.Add (fn);
                    }
                }
                else
                {
                    string [] fileNames = Directory.GetFiles (featureFolder);
                    fileNameList.AddRange (fileNames);
                }

                foreach (string fileName in fileNameList)
                {
                    Feature feature = AimObjectFactory.CreateFeature (featureType);
                    if (LoadFromFile (feature, fileName))
                        featureList.Add (feature);
                }
            }

            return featureList.ToArray ();
        }

        private Feature [] GetLinkFeature (FeatureType featureType, Guid linkIdentifier, string linkPropName)
        {
            //--- Add Exception Case. Association relation not implemented yet.
            if (featureType == FeatureType.AirportHeliport && linkPropName == "responsibleOrganisation")
            {
                return GetAirportHeliportByOrganisation (linkIdentifier);
            }

            AimPropInfo linkPropInfo = GetPropTypeIndex ((int) featureType, linkPropName);
            if (linkPropInfo == null)
                throw new Exception ("Property name '" + linkPropName + "' does not exists");

            Feature [] featureArr = GetFeature (featureType, new List<Guid> ());
            List<Feature> featureList = new List<Feature> ();

            foreach (Feature feat in featureArr)
            {
                IAimProperty aimProp = (feat as IAimObject).GetValue (linkPropInfo.Index);
                if (aimProp != null)
                {
                    if (aimProp.PropertyType == AimPropertyType.DataType)
                    {
                        if (aimProp is FeatureRef)
                        {
                            if (((FeatureRef) aimProp).Identifier == linkIdentifier)
                                featureList.Add (feat);
                        }
                    }
                    else if (aimProp.PropertyType == AimPropertyType.List)
                    {
                        IList list = aimProp as IList;
                        foreach (AObject featureItem in list)
                        {
                            if (featureItem.ObjectType == ObjectType.FeatureRefObject)
                            {
                                FeatureRefObject fro = (FeatureRefObject) featureItem;
                                if (fro.Feature != null && fro.Feature.Identifier == linkIdentifier)
                                {
                                    featureList.Add (feat);
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return featureList.ToArray ();
        }

        private Feature [] GetAirportHeliportByOrganisation (Guid orgIdentifier)
        {
            Feature [] featureArr = GetFeature (FeatureType.AirportHeliport, new List<Guid> ());
            List<Feature> featureList = new List<Feature> ();

            foreach (AirportHeliport ah in featureArr)
            {
                if (ah.ResponsibleOrganisation != null &&
                    ah.ResponsibleOrganisation.TheOrganisationAuthority != null &&
                    ah.ResponsibleOrganisation.TheOrganisationAuthority.Identifier == orgIdentifier)
                {
                    featureList.Add (ah);
                }
            }

            return featureList.ToArray ();
        }

        private bool SaveFeature (Feature feature)
        {
            if (feature.Id == -1)
                feature.Id = 0;

            string dirName = _folderName + "\\" + feature.FeatureType;
            string fileName = dirName + "\\" + feature.Identifier + ".dat";

            if (File.Exists (fileName))
                File.Delete (fileName);
            else if (!Directory.Exists (dirName))
                Directory.CreateDirectory (dirName);

            FileStream fs = new FileStream (fileName, FileMode.Create);
            PackageWriter pw = new AranPackageWriter (fs);
            (feature as IPackable).Pack (pw);
            fs.Close ();

            return true;
        }

        private void DeleteFeature (Feature feature)
        {
            
        }

        private bool LoadFromFile (Feature feature, string fileName)
        {
            FileStream fs = new FileStream (fileName, FileMode.Open);

            //try
            //{
                byte [] ba = new byte [fs.Length];
                fs.Read (ba, 0, ba.Length);
                PackageReader pr = new AranPackageReader (ba);
                (feature as IPackable).Unpack (pr);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine ("Error in LoadFromFile function: " + ex.Message);
            //    throw ex;
            //}
            //finally
            //{
                fs.Close ();
            //}
            return true;
        }

        private AimPropInfo GetPropTypeIndex (int aimTypeIndex, string propName)
        {
            AimPropInfo [] propInfos = AimMetadata.GetAimPropInfos (aimTypeIndex);

            foreach (var item in propInfos)
            {
                if (item.Name.Equals (propName, StringComparison.CurrentCultureIgnoreCase))
                {
                    return item;
                }
            }

            return null;
        }

        private Feature [] GetAllFeatureByCreateDate (DateTime dateTime)
        {
            List<string> allFilesList = new List<string> ();
            GetFiles (_folderName, allFilesList);

            List<Feature> featureList = new List<Feature> ();

            foreach (string fileName in allFilesList)
            {
                string dirName = Path.GetDirectoryName (fileName);
                string featureName = dirName.Substring (dirName.LastIndexOf ('\\') + 1);

                FeatureType featureType = (FeatureType) Enum.Parse (typeof (FeatureType), featureName);
                Feature feature = AimObjectFactory.CreateFeature (featureType);

                DateTime creationTime = File.GetCreationTime (fileName);
                if (creationTime < dateTime)
                    continue;

                FileStream fs = File.Open (fileName, FileMode.Open);
                AranPackageReader pr = new AranPackageReader (fs);
                (feature as IPackable).Unpack (pr);
                fs.Close ();

                featureList.Add (feature);
            }

            return featureList.ToArray ();
        }

        private void GetFiles (string dir, List<string> allFilesList)
        {
            string [] dirArr = Directory.GetDirectories (dir);
            foreach (string subDir in dirArr)
            {
                GetFiles (subDir, allFilesList);
            }

            string [] files = Directory.GetFiles (dir);
            allFilesList.AddRange (files);
        }

        private ConnectionInfo _connectionInfo;
        private Dictionary<int, List<Feature>> _transactionFeaturesDic;
        private int _lastWorkPackageId;
        private string _folderName;
    }
}
