using System;
using System.Collections.Generic;

namespace Aran.Aim.Data.Filters
{
    internal class PropInfoParser
    {
        public PropInfoParser ( )
        {
            ReplacableString = "<replacable>";
        }

        public Dictionary<AimPropInfo, List<int>> AbstractPropDescents
        {
            get;
            set;
        }

        public AimPropInfoList GetPropInfoListFromPropName ( int dbEntityTypeIndex, string PropertyName )
        {
            AbstractPropDescents = new Dictionary<AimPropInfo, List<int>> ();
            string[] propNames = PropertyName.Split ( '.' );
            AimPropInfoList propInfoList = new AimPropInfoList ();
            for ( int i = 0; i < propNames.Length; i++ )
            {
                AimPropInfo[] allPropInfo;
                AimPropInfo propInfo;
                if ( AimMetadata.IsAbstract ( dbEntityTypeIndex ) )
                {
                    // Walk through all descendent AimObject to find appropriate property
                    // For test dbEntityTypeIndex is 1094 (TerminalSegmentPoint)                    
                    List<int> descendantDbEntityIndices = GetDescendants ( dbEntityTypeIndex );
                    propInfo = null;

                    // If this propInfo is in abstract object then it should walk thgrough all descendant AimObjects
                    allPropInfo = AimMetadata.GetAimPropInfos ( dbEntityTypeIndex );
                    propInfo = GetPropInfo ( allPropInfo, propNames [i].ToLower () );
                    if ( propInfo == null )
                    {
                        foreach ( int descendanDbEntTypeIndex in descendantDbEntityIndices )
                        {
                            allPropInfo = AimMetadata.GetAimPropInfos ( descendanDbEntTypeIndex );
                            propInfo = GetPropInfo ( allPropInfo, propNames [i].ToLower () );
                            if ( propInfo != null )
                            {
                                propInfoList.Add ( propInfo );
                                descendantDbEntityIndices.Clear ();
                                descendantDbEntityIndices.Add ( descendanDbEntTypeIndex );
                                AbstractPropDescents.Add ( propInfo, descendantDbEntityIndices );
                                dbEntityTypeIndex = propInfo.TypeIndex;
                                break;
                            }
                        }
                        if ( propInfo == null )
                            throw new Exception ( "Not found property" );
                        continue;
                    }
                    else
                    {
                        propInfoList.Add ( propInfo );
                        //propInfo.Tag = descendantDbEntityIndices;
                        AbstractPropDescents.Add ( propInfo, descendantDbEntityIndices );
                        dbEntityTypeIndex = propInfo.TypeIndex;
                        continue;
                    }
                }

                if ( AimMetadata.IsChoice ( dbEntityTypeIndex ) )
                {
                    if ( propNames [i].ToLower () == "choice" )
                        continue;
                }
                allPropInfo = AimMetadata.GetAimPropInfos ( dbEntityTypeIndex );
                propInfo = GetPropInfo ( allPropInfo, propNames [i].ToLower () );
                if ( propInfo != null )
                    propInfoList.Add ( propInfo );
                else
                    throw new Exception ( "Not found property" );
                dbEntityTypeIndex = propInfo.TypeIndex;
            }
            return propInfoList;
        }

        public string AddPreviousWhereString ( ref string tableName, AimPropInfoList propInfoList )
        {
            string propName = propInfoList [0].Name;
            string result = ReplacableString;
            int i = 0;
            bool isPropNameTargetId = false;
            while ( i<= propInfoList.Count-2 )
            {
                propName = propInfoList [i].Name;                
                List<int> descentTypeindices;
                if ( AimMetadata.IsAbstract ( propInfoList [i].TypeIndex ) )
                {
                    tableName = GetTableName ( propInfoList [i].TypeIndex );
                    AbstractPropDescents.TryGetValue ( propInfoList [i+1], out descentTypeindices );
                    i++;
                    // This property is in abstract object that is why 
                    // it should walk through all descendant objects
                    string absStr = "(";
                    foreach ( int index in descentTypeindices )
                    {
                        tableName = GetTableName ( index );
                        if ( !isPropNameTargetId )
                            absStr += string.Format ( "(\"ref_{0}\" = {1} AND \"{0}\" IN (SELECT \"Id\" FROM \"{2}\" WHERE {3})) OR ", propName, index, tableName, ReplacableString );
                        else
                            absStr += string.Format ( "SELECT \"Id\" FROM \"{0}\" WHERE {1}    ", tableName, ReplacableString );
                    }
                    absStr = absStr.Remove ( absStr.Length - 3 ) + ")";
                    result = result.Replace ( ReplacableString, absStr );
                }
                else if ( AimMetadata.IsChoice ( propInfoList [i].TypeIndex ) )
                {
                    if ( propInfoList [i].IsList )
                    {
                        if ( propInfoList [i+1].IsFeatureReference )
                        {
                            result = result.Replace ( ReplacableString, string.Format ( "\"{0}\".\"Id\" IN (SELECT \"{0}_id\" FROM \"{0}_link\" " + 
                                "WHERE prop_index = {1} AND targettableindex = {2} " +
                                "AND target_id IN (SELECT \"Id\" FROM \"{3}\" WHERE (prop_type = {4} OR choice_type = {4}) AND {5}))",
                                tableName, propInfoList [i].Index, propInfoList [i].TypeIndex,
                                GetTableName ( propInfoList [i].TypeIndex ), ( int ) propInfoList [i+1].ReferenceFeature, ReplacableString) );
                        }
                        else
                            result = result.Replace ( ReplacableString, string.Format ( "\"{0}\".\"Id\" IN (SELECT \"{0}_id\" FROM \"{0}_link\" " + 
                                "WHERE prop_index = {1} AND targettableindex = {2} " +
                                "AND target_id IN (SELECT \"Id\" FROM \"{3}\" WHERE (prop_type = {4} OR choice_type = {4}) AND " + 
                                "target_id IN (SELECE \"Id\" from \"{5}\" WHERE {6})))",
                                tableName, 
                                propInfoList [i].Index, 
                                propInfoList [i].TypeIndex,
                                GetTableName ( propInfoList [i].TypeIndex ), 
                                GetTableName ( propInfoList [i+1].TypeIndex ),
                                ReplacableString ) );                                
                    }
                    else
                    {
                        tableName = GetTableName ( propInfoList [i].TypeIndex );
                        if ( propInfoList [i+1].IsFeatureReference )
                        {
                            result = result.Replace ( ReplacableString, string.Format ( "\"{0}\" IN (SELECT \"Id\" FROM \"{1}\" WHERE ( (prop_type = {2} OR choice_type = {2}) AND {3} )) ",
                                propName, tableName, ( int ) propInfoList [i+1].ReferenceFeature, ReplacableString ) );
                        }
                        else
                        {
                            isPropNameTargetId = true;
                            if ( AimMetadata.IsAbstract ( propInfoList [i+1].TypeIndex ) )
                            {
                                AbstractPropDescents.TryGetValue ( propInfoList [i+2], out descentTypeindices );
                                result = result.Replace ( ReplacableString, string.Format ( "\"{0}\" IN (SELECT \"Id\" FROM \"{1}\" WHERE ({2}))", propName, tableName, ReplacableString ) );
                                string absStr = "";
                                foreach ( int index in descentTypeindices )
                                {
                                    tableName = GetTableName ( index );
                                    absStr += string.Format ( "( choice_type = {0} AND target_id IN (SELECT \"Id\" FROM \"{1}\" WHERE {2})) OR ", index, tableName, ReplacableString );
                                }
                                i++;
                                absStr = absStr.Remove ( absStr.Length - 3 );
                                result = result.Replace ( ReplacableString, absStr );
                            }
                            else
                                result = result.Replace ( ReplacableString, string.Format ( "\"{0}\" IN (SELECT \"Id\" FROM \"{1}\" WHERE ( (prop_type = {2} OR choice_type = {2}) AND target_id IN ({3}) )) ",
                                    propName, tableName, ( int ) propInfoList [i+1].TypeIndex, ReplacableString ) );
                        }
                    }
                }
                else  if ( propInfoList [i].IsList )
                {
                    if ( !propInfoList [i].IsFeatureReference )
                    {
                        if ( !propInfoList [i+1].IsList )
                        {
                            result = result.Replace ( ReplacableString, string.Format ( "\"{0}\".\"Id\" IN (SELECT \"{0}_id\" FROM \"{0}_link\" WHERE (prop_index = {1} AND " + 
                                                                        "targettableindex = {2} and target_id IN (SELECT \"Id\" FROM \"{3}\" WHERE {4})))",
                                                                            tableName, propInfoList [i].Index, propInfoList [i].TypeIndex,
                                                                            GetTableName ( propInfoList [i].TypeIndex ), ReplacableString ) );
                        }
                        else
                        {
                            result = result.Replace ( ReplacableString, string.Format ( "\"{0}\".\"Id\" IN (SELECT \"{0}_id\" FROM \"{0}_link\" WHERE (prop_index = {1} AND " + 
                                                                                        "target_id IN (\"{2}\"))",
                                                                                            tableName, propInfoList [i].Index, ReplacableString ) );
                        }

                    }
                    else
                        result = result.Replace ( ReplacableString, string.Format ( "\"{0}\".\"Id\" IN (SELECT \"{0}_id\" FROM \"{0}_link\" WHERE (prop_index = {1} AND " + 
                            "targettableindex = {2} and {3} ))",
                            tableName, propInfoList [i].Index,
                            propInfoList [i].TypeIndex, ReplacableString ) );
                    tableName = GetTableName ( propInfoList [i].TypeIndex );
                }
                else
                {
                    tableName = GetTableName ( propInfoList [i].TypeIndex );
                    if ( !isPropNameTargetId )
                        result = result.Replace ( ReplacableString, string.Format ( "\"{0}\" in (SELECT \"Id\" FROM \"{1}\" WHERE {2})", propName, tableName, ReplacableString ) );
                    else
                        result = result.Replace ( ReplacableString, string.Format ( "SELECT \"Id\" FROM \"{1}\" WHERE {2}", propName, tableName, ReplacableString ) );
                }                        
                i++;
            }
            return result;
        }

        public string GetTableName ( int dbEntityTypeIndex )
        {
            AimObjectType aimObjType = AimMetadata.GetAimObjectType ( dbEntityTypeIndex );
            string result = AimMetadata.GetAimTypeName ( dbEntityTypeIndex );
            if ( aimObjType == AimObjectType.Feature )
                return "bl_" + result;
            if ( aimObjType == AimObjectType.Object )
            {
                if ( AimMetadata.IsChoice ( dbEntityTypeIndex ) )
                    return result;
                return "obj_" + result;
            }
            return result;
        }

        public static string ReplacableString
        {
            get;
            private set;
        }
        
        private List<int> GetDescendants ( int dbEntityTypeIndex )
        {
            List<AimClassInfo> aimClassInfoList = AimMetadata.AimClassInfoList.FindAll ( classInfo => classInfo.Parent != null && classInfo.Parent.Index == dbEntityTypeIndex );
            List<int> result = new List<int> ();
            foreach ( AimClassInfo classInfo in aimClassInfoList )
                result.Add ( classInfo.Index );
            return result;
        }

        private AimPropInfo GetPropInfo ( AimPropInfo [] allPropInfo, string nameInLowerCase )
        {
            for ( int i = 0; i < allPropInfo.Length; i++ )
            {
                if ( allPropInfo [i].Name.ToLower () == nameInLowerCase )
                    return allPropInfo [i];
            }
            return null;
        }
    }
}
