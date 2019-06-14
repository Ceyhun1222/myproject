using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using Aran;
using System.Collections;
using Aran.Aim.CAWProvider;
using System.Xml;
using System.IO;
using Aran.Aim.Features;
using Aran.Aim.Data;

namespace Aran.Aim.DBService
{
    public class DbService : IDbService
    {
        public DbService ()
        {
            _dbProvider = DbProviderFactory.Create ("Aran.Aim.Data.PgDbProvider");
            _dbProvider.Open ("Server=172.30.31.18;Port=5432;User Id=aran;Password=aran;Database=AIM");
        }

        public string SetFeatures (string aixmBasicMessageText)
        {
            OnRequested ("SetFeatures (" + aixmBasicMessageText + ")");

            string result = "";

            try
            {
                byte [] buffer = Encoding.UTF8.GetBytes (aixmBasicMessageText);
                MemoryStream ms = new MemoryStream (buffer);
                XmlReader reader = XmlReader.Create (ms);

                AixmBasicMessage abm = new AixmBasicMessage ();
                abm.ReadXml (reader);
                List<Feature> featList = new List<Feature> ();
                foreach (var hasMember in abm.HasMember)
                {
                    foreach (Feature feature in hasMember)
                    {
                        featList.Add ( feature );
                        //_service.InsertFeature (feature, null);
                    }
                }

                Dictionary<Feature,ReasonNotInserted> notInsertedFeatList = new Dictionary<Feature, ReasonNotInserted> ();
                Dictionary<Feature, string> reasonError = InsertFeatures ( featList, notInsertedFeatList );
                if ( reasonError.Count > 0 )
                {
                    StringBuilder stringBuilder = new StringBuilder ();
                    foreach ( Feature feat in reasonError.Keys.ToList<Feature> () )
                    {
                        // StringBuilder is effective than string in these situation :)
                        // While when concenate 2 strings it creates new string.So string is immutable :(
                        stringBuilder.Append ( feat.FeatureType ).
                                                  Append ( "(" ).
                                                  Append ( feat.Identifier.ToString () ).
                                                  Append ( ") could not be imported to DB:" ).
                                                  AppendLine ( reasonError [feat] );
                    }
                    result = stringBuilder.ToString ();
                }
                ms.Close ();
                ms.Dispose ();
            }
            catch (Exception ex)
            {
                result = "error: " + ex.Message;
            }

            OnRespensed (result);

            return result;
        }

        private Dictionary<Feature, string> InsertFeatures ( List<Feature> allFeatList, Dictionary<Feature, ReasonNotInserted> notInsertedFeatList )
        {
            InsertingResult insertResult;
            Feature feat;
            Dictionary<Feature, string> couldntInsertedList = new Dictionary<Feature, string> ();
            bool isInsertedReasonFeat = false;
            int countOfInsertedFeat = 0;
            for ( int i = 0; i <= allFeatList.Count - 1; i++ )
            {
                feat = allFeatList [i];
                insertResult = _dbProvider.Insert ( feat, true );
                if ( !insertResult.IsSucceed )
                {
                    if ( insertResult.Message.StartsWith ( "ERROR: 23503:" ) && 
                            insertResult.Message.Contains ( "violates foreign key constraint" ) && 
                            insertResult.Message.Contains ( "is not present in table \"features\"" ) )

                        // Reference feature has to be inserted first
                        notInsertedFeatList.Add ( feat, CreatReasonNotInserted ( feat, insertResult.Message ) );
                    else
                        couldntInsertedList.Add ( feat, insertResult.Message );
                }
                else
                {
                    List<Feature> featList = notInsertedFeatList.Keys.ToList<Feature> ();
                    foreach ( Feature notInsertedFeat in featList )
                    {
                        if ( notInsertedFeatList [notInsertedFeat].FeatType == feat.FeatureType && 
                                notInsertedFeatList [notInsertedFeat].Identifier == feat.Identifier )
                        {
                            notInsertedFeatList [notInsertedFeat].IsReasonFeatInserted = true;
                            countOfInsertedFeat++;
                            isInsertedReasonFeat = true;
                        }
                    }
                }
            }
            if ( isInsertedReasonFeat )
            {
                List<Feature> allFeatListHaveToBeInserted = SeparateFeatList ( notInsertedFeatList );
                Dictionary<Feature, string> result = InsertFeatures ( allFeatListHaveToBeInserted, notInsertedFeatList );
                foreach ( Feature featInResult in result.Keys.ToList<Feature> () )
                {
                    couldntInsertedList.Add ( featInResult, result [featInResult] );
                }
            }
            return couldntInsertedList;
        }

        private List<Feature> SeparateFeatList ( Dictionary<Feature, ReasonNotInserted> notInsertedFeatList )
        {
            List<Feature> result = new List<Feature> ();
            foreach ( Feature feat in notInsertedFeatList.Keys.ToList<Feature> () )
            {
                if ( notInsertedFeatList [feat].IsReasonFeatInserted == true )
                {
                    result.Add ( feat );
                }
            }
            foreach ( Feature feat in result )
            {
                notInsertedFeatList.Remove ( feat );
            }
            return result;
        }

        private ReasonNotInserted CreatReasonNotInserted ( Feature feat, string message )
        {
            ReasonNotInserted result = new ReasonNotInserted ();
            int indexOfKey = message.IndexOf ( "Key (" );
            string propName = message.Substring ( indexOfKey + 5, message.IndexOf ( ")", indexOfKey ) - indexOfKey - 5 );
            indexOfKey = message.IndexOf ( "=(" );
            int i = message.IndexOf ( ") is not present in table \"features\"" );
            string identifier = message.Substring ( indexOfKey + 2, i - indexOfKey - 2 );
            AimPropInfo aimPropInfo = null;
            AimPropInfoList insidePropList = new AimPropInfoList ();

            aimPropInfo = GetPropInfo ( ( int ) feat.FeatureType, propName, insidePropList );
            result.FeatType = aimPropInfo.ReferenceFeature;
            result.Identifier = new Guid ( identifier );
            result.IsReasonFeatInserted = false;
            return result;
        }

        private AimPropInfo GetPropInfo ( int typeIndex, string propName, AimPropInfoList insidePropList )
        {
            AimPropInfo[] propInfos = AimMetadata.GetAimPropInfos ( typeIndex );
            AimPropInfo result = FindPropInfo ( propInfos, propName, insidePropList );
            if ( result == null )
            {
                foreach ( AimPropInfo propInfo in propInfos )
                {
                    if ( propInfo.PropType != null && propInfo.PropType.AimObjectType != AimObjectType.Field && propInfo.Name.ToLower () != "timeslice" )
                    {
                        result = GetPropInfo ( propInfo.TypeIndex, propName, insidePropList );
                        if ( result != null )
                        {
                            insidePropList.Add ( propInfo );
                            return result;
                        }
                    }
                }
            }
            return result;
        }

        private AimPropInfo FindPropInfo ( AimPropInfo [] propInfos, string propName, AimPropInfoList insidePropList )
        {
            foreach ( AimPropInfo propInfo in propInfos )
            {
                if ( propInfo.Name == propName )
                {
                    insidePropList.Add ( propInfo );
                    return propInfo;
                }
            }
            return null;
        }

        public string GetFeatures ( DateTime creationTime )
        {
            #region File Base

            //OnRequested ("GetFeatures (" + creationTime + ")");

            //string result = "";

            //try
            //{
            //    SimpleQuery sq = new SimpleQuery ();
            //    sq.Filter = new Filter ();
            //    ComparisonOps compOps = new ComparisonOps (ComparisonOpType.IsGreaterThanOrEqualTo,
            //        "createTime", creationTime);
            //    sq.Filter.Operation = new OperationChoice (compOps);

            //    Feature [] featArr = _service.GetFeature ( sq );

            //    AixmBasicMessage amb = new AixmBasicMessage ();

            //    foreach (Feature feature in featArr)
            //    {
            //        AixmFeatureList afl = new AixmFeatureList ();
            //        afl.Add (feature);
            //        amb.HasMember.Add (afl);
            //    }

            //    MainForm.This.DBService_Responsed ("Feature count: " + featArr.Length, null);

            //    StringBuilder strBuilder = new StringBuilder ();

            //    using (XmlWriter writer = XmlWriter.Create (strBuilder))
            //    {
            //        amb.WriteXml (writer);
            //    }

            //    result = strBuilder.ToString ();
            //}
            //catch (Exception ex)
            //{
            //    result = "error: " + ex.Message;
            //}

            //OnRespensed (result);
            //return result;
            #endregion
            return "";
        }

        private void OnRequested (string message)
        {
            if (MainForm.This != null)
                MainForm.This.DBService_Requested (message, null);

            //if (Requested != null)
            //    Requested (message, null);
        }

        private void OnRespensed (string message)
        {
            if (MainForm.This != null)
                MainForm.This.DBService_Responsed (message, null);
            //if (Responsed != null)
            //    Responsed (message, null);
        }
        
        //private ICawService _service;
        private IDbProvider _dbProvider;
        public event EventHandler Requested;
        public event EventHandler Responsed;
    }

}
