using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using System.Collections;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;
using System.Security.Cryptography;
using Aran.Aim.Data.Filters;

namespace Aran.Aim.Data
{
    public class DbUtility
    {
        public DbUtility(DbProvider dbPro)
        {
            _dbPro = dbPro;
        }

        public void SetDefault(TimeSliceInterpretationType interpretationType, DateTime effectiveDate)
        {
            _defaultInterpretationType = interpretationType;
            _defaultEffectiveDate = effectiveDate;
        }

        public void LoadWithRefFeatures(FeatureType featureType, Guid identifier, List<Feature> featureList, List<string> errorList)
        {
            if (IsIdentifierAdded(identifier, featureList))
                return;

            var gr = _dbPro.GetVersionsOf(featureType, _defaultInterpretationType, identifier, true,
                new TimeSliceFilter(_defaultEffectiveDate), null, null);

            if (!gr.IsSucceed) {
                errorList.Add(gr.Message);
                return;
            }

            if (gr.List.Count == 0)
                return;

            var feature = gr.List[0] as Feature;
            featureList.Add(feature);

            FindRefFeatures(feature, featureList, errorList);
        }

        
        public static string GetMd5Hash(string input)
        {
            MD5 md5Hash = MD5.Create();
            // Convert the input string to a byte array and compute the hash. 
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes 
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data  
            // and format each one as a hexadecimal string. 
            for (int i = 0; i < data.Length; i++) {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string. 
            return sBuilder.ToString();
        }


        private void FindRefFeatures(IAimObject aimObj, List<Feature> featureList, List<string> errorList)
        {
            var classInfo = AimMetadata.GetClassInfoByIndex(aimObj);

            foreach (var propInfo in classInfo.Properties) {
                IAimProperty aimProp = aimObj.GetValue(propInfo.Index);
                if (aimProp != null) {

                    #region Object Property

                    if (propInfo.PropType.AimObjectType == AimObjectType.Object) {

                        #region List

                        if (aimProp.PropertyType == AimPropertyType.List) {
                            var list = aimProp as IList;

                            #region AbstractFeatureRef

                            if (propInfo.PropType.SubClassType == AimSubClassType.AbstractFeatureRef) {
                                for (int i = 0; i < list.Count; i++) {
                                    var absFRObj = list[i] as IAimObject;
                                    var pv = absFRObj.GetValue((int)Aran.Aim.PropertyEnum.PropertyAbstractFeatureRefObject.Feature);
                                    var absFR = pv as Aran.Aim.DataTypes.IAbstractFeatureRef;

                                    if (absFR != null) {
                                        LoadWithRefFeatures((FeatureType)absFR.FeatureTypeIndex, absFR.Identifier, featureList, errorList);
                                    }
                                }
                            }

                            #endregion

                            #region Choice

                            else if (propInfo.PropType.SubClassType == AimSubClassType.Choice) {
                                for (int i = 0; i < list.Count; i++) {
                                    var editChoice = list[i] as IEditChoiceClass;

                                    if (editChoice.RefValue != null) {
                                        if (editChoice.RefValue.PropertyType == AimPropertyType.Object) {
                                            var itemObj = aimProp as IAimObject;
                                            FindRefFeatures(itemObj, featureList, errorList);
                                        }
                                        else if (editChoice.RefValue.PropertyType == AimPropertyType.DataType) {
                                            var fr = editChoice.RefValue as FeatureRef;

                                            var propFeatureType = (FeatureType)editChoice.RefType;
                                            var propIdentifier = fr.Identifier;

                                            LoadWithRefFeatures(propFeatureType, propIdentifier, featureList, errorList);
                                        }
                                    }
                                }
                            }

                            #endregion

                            #region Normal Object

                            else {
                                for (int i = 0; i < list.Count; i++) {
                                    var itemObj = list[i] as IAimObject;
                                    FindRefFeatures(itemObj, featureList, errorList);
                                }
                            }

                            #endregion
                        }

                        #endregion

                        #region Not List

                        #region Choice

                        else if (propInfo.PropType.SubClassType == AimSubClassType.Choice) {
                            var editChoice = aimProp as IEditChoiceClass;
                            if (editChoice.RefValue != null) {
                                if (editChoice.RefValue.PropertyType == AimPropertyType.Object) {
                                    var itemObj = aimProp as IAimObject;
                                    FindRefFeatures(itemObj, featureList, errorList);
                                }
                                else if (editChoice.RefValue.PropertyType == AimPropertyType.DataType) {
                                    var fr = editChoice.RefValue as FeatureRef;

                                    var propFeatureType = (FeatureType)editChoice.RefType;
                                    var propIdentifier = fr.Identifier;

                                    LoadWithRefFeatures(propFeatureType, propIdentifier, featureList, errorList);
                                }
                            }
                        }

                        #endregion

                        #region Normal Object

                        else {
                            var itemObj = aimProp as IAimObject;
                            FindRefFeatures(itemObj, featureList, errorList);
                        }

                        #endregion

                        #endregion
                    }

                    #endregion

                    #region Abstract FeatRef Or FeatRef

                    else if (propInfo.PropType.AimObjectType == AimObjectType.DataType) {
                        var absFR = aimProp as IAbstractFeatureRef;
                        if (absFR != null) {
                            LoadWithRefFeatures((FeatureType)absFR.FeatureTypeIndex, absFR.Identifier, featureList, errorList);
                        }
                        else {
                            var fr = aimProp as FeatureRef;
                            if (fr != null) {
                                LoadWithRefFeatures((FeatureType)propInfo.PropType.Index, fr.Identifier, featureList, errorList);
                            }
                        }
                    }

                    #endregion
                }
            }
        }

        private bool IsIdentifierAdded(Guid identifier, List<Feature> featureList)
        {
            foreach (var feat in featureList) {
                if (feat.Identifier == identifier)
                    return true;
            }

            return false;
        }

        private DbProvider _dbPro;
        private TimeSliceInterpretationType _defaultInterpretationType;
        private DateTime _defaultEffectiveDate;
    }
}
