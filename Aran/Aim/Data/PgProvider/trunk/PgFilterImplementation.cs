using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Data.Filters;
using Aran.Geometries.IO;
using Aran.Converters;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Data
{
    public class PgFilterImplementation
    {
        public PgFilterImplementation ()
        {            
        }

        public string GetSqlString ( OperationChoice operChoice, int dbEntityTypeIndex, ref byte [] geomByteA )
        {
            switch ( operChoice.Choice )
            {
                case OperationChoiceType.Spatial:
                    return GetSqlStringForSpatialOp ( operChoice.SpatialOps, dbEntityTypeIndex, ref geomByteA );                
                case OperationChoiceType.Comparison:
                    return GetSqlStringForCompOp ( operChoice.ComparisonOps, dbEntityTypeIndex );
                case OperationChoiceType.Logic:
                    // Recycle it until choice is not logic(Binary Logic Operation)
                    BinaryLogicOp binLogOper = operChoice.LogicOps as BinaryLogicOp;
                    string result = "(";
                    int oprCount = binLogOper.OperationList.Count;
                    for ( int i = 0; i < oprCount - 1; i++ )
                    {
                        result += " " + GetSqlString ( binLogOper.OperationList [i], dbEntityTypeIndex, ref geomByteA ) + " " + binLogOper.Type.ToString ();
                    }
                    result += GetSqlString ( binLogOper.OperationList [oprCount-1], dbEntityTypeIndex, ref geomByteA );
                    result += ")";
                    return result;
                default:
                    throw new Exception ( "Not Implemented Operation Type !" );
            }
        }

        #region Spatial Operation

        private string GetSqlStringForSpatialOp ( SpatialOps spatOp, int dbEntityTypeIndex, ref byte[] geomByteA  )
        {
            if ( spatOp is DWithin )
            {
                DWithin dWithin = spatOp as DWithin;
                GeometryWKBWriter geomWkbWriter = new GeometryWKBWriter ();
                geomWkbWriter.Write ( dWithin.Geometry, ByteOrder.LittleEndian );
                geomByteA = geomWkbWriter.GetByteArray ();

                PropInfoParser propInfoParser = new PropInfoParser ();
                AimPropInfoList propInfoList = propInfoParser.GetPropInfoListFromPropName ( dbEntityTypeIndex, dWithin.PropertyName );
                string tableName = propInfoParser.GetTableName ( dbEntityTypeIndex );
                string result = "";
                double valueInM = ConverterToSI.Convert ( dWithin.Distance, double.NaN );
                if ( propInfoList.Count > 1 )
                    result = propInfoParser.AddPreviousWhereString ( ref tableName, propInfoList );
                string geogColName = propInfoList [propInfoList.Count - 1].Name;
                result = result.Replace ( PropInfoParser.ReplacableString,
                            string.Format ( "St_DWithin(\"{0}\",ST_GeomFromWKB(:geomByteA, 4326),{1})", geogColName, valueInM ) );
                return result;
            }
            else if ( spatOp is Within )
            {
                Within within = spatOp as Within;
                GeometryWKBWriter geomWkbWriter = new GeometryWKBWriter ();
                geomWkbWriter.Write ( within.Geometry, ByteOrder.LittleEndian );
                geomByteA = geomWkbWriter.GetByteArray ();

                PropInfoParser propInfoParser = new PropInfoParser ();
                AimPropInfoList propInfoList = propInfoParser.GetPropInfoListFromPropName ( dbEntityTypeIndex, within.PropertyName );
                string tableName = propInfoParser.GetTableName ( dbEntityTypeIndex );
                string result = "";
                if ( propInfoList.Count > 1 )
                    result = propInfoParser.AddPreviousWhereString ( ref tableName, propInfoList );
                string geogColName = propInfoList [propInfoList.Count - 1].Name;
                // It uses ST_Distance to check whether geometry is in other geometry
                // If ST_Distance = 0 then it is inside of geometry
                result = result.Replace ( PropInfoParser.ReplacableString,
                            string.Format ( "St_Distance(\"{0}\", ST_GeomFromWKB(:geomByteA, 4326), TRUE) = 0", geogColName ) );
                return result;
            }
            else
                throw new Exception ( "Not found SpatialOperation type !" );
        }

        #endregion

        #region Comparison Operation
        private string GetSqlStringForCompOp ( ComparisonOps compOp, int dbEntityTypeIndex )
        {
            PropInfoParser propInfoParser = new PropInfoParser ();
            AimPropInfoList propInfoList = propInfoParser.GetPropInfoListFromPropName ( dbEntityTypeIndex, compOp.PropertyName );
            ValueType valType = GetValueType ( compOp.Value );
            if ( valType == ValueType.VT_Simple )
            {
                return ( SqlStringForSimpleValueType ( compOp, valType, dbEntityTypeIndex, propInfoList ) );
            }
            else if ( valType == ValueType.VT_ValClass )
            {
                return SqlStringForValClassValType ( compOp, dbEntityTypeIndex, propInfoList );
            }
            else if ( valType == ValueType.VT_TextNote )
            {
                return SqlStringForTextNoteValueType ( compOp, valType, dbEntityTypeIndex, propInfoList );
            }
            else
                throw new Exception ( "Not found Value Type" );
        }

        private string SqlStringForSimpleValueType ( ComparisonOps compOp, ValueType valType, int dbEntityTypeIndex, AimPropInfoList propInfoList )
        {
            PropInfoParser propInfoParser = new PropInfoParser ();
            string tableName;
            string selectTbls = PropInfoParser.ReplacableString;
            if ( propInfoList.Count > 1 )
            {
                tableName = propInfoParser.GetTableName ( dbEntityTypeIndex );
                selectTbls = propInfoParser.AddPreviousWhereString ( ref tableName, propInfoList );
            }
            else
                tableName = propInfoParser.GetTableName ( dbEntityTypeIndex );
            AimPropInfo latestAimPropInfo = propInfoList [propInfoList.Count - 1];
            string latestPropName = latestAimPropInfo.Name;
            if ( latestAimPropInfo.IsFeatureReference )
            {
                if ( AimMetadata.IsChoice ( latestAimPropInfo.TypeIndex ) )
                    latestPropName = "target_guid";
                else if ( latestAimPropInfo.IsList )
                {
                    if ( ( latestAimPropInfo.PropType == null || !latestAimPropInfo.PropType.IsAbstract ) && !AimMetadata.IsChoice ( latestAimPropInfo.TypeIndex ) )
                    {
                        latestPropName = "target_guid";
                        if ( propInfoList.Count > 1 && propInfoList [propInfoList.Count - 2].IsList )
                            selectTbls = selectTbls.Replace ( PropInfoParser.ReplacableString, string.Format ( "SELECT \"{0}_id\" FROM \"{0}_link\" WHERE (prop_index = {1} AND {2} )",
                                                                                                 tableName, latestAimPropInfo.Index, PropInfoParser.ReplacableString ) );
                        else
                            selectTbls = selectTbls.Replace ( PropInfoParser.ReplacableString, string.Format ( "\"{0}\".\"Id\" IN (SELECT \"{0}_id\" FROM \"{0}_link\" WHERE (prop_index = {1} AND {2} ))",
                                                                                             tableName, latestAimPropInfo.Index, PropInfoParser.ReplacableString ) );
                    }
                }
            }
            string whereStr;
            switch ( compOp.OperationType )
            {
                case ComparisonOpType.EqualTo:
                    whereStr = string.Format ( "\"{0}\" = {1}", latestPropName, ValueAsString (valType, compOp.Value) [0] );
                    break;

                case ComparisonOpType.NotEqualTo:
                    whereStr = string.Format ( "\"{0}\" != {1}", latestPropName, ValueAsString ( valType, compOp.Value ) [0] );
                    break;

                case ComparisonOpType.LessThan:
                    whereStr = string.Format ( "\"{0}\" < {1}", latestPropName, ValueAsString ( valType, compOp.Value ) [0] );
                    break;

                case ComparisonOpType.GreaterThan:
                    whereStr = string.Format ( "\"{0}\" > {1}", latestPropName, ValueAsString ( valType, compOp.Value ) [0] );
                    break;

                case ComparisonOpType.LessThanOrEqualTo:
                    whereStr = string.Format ( "\"{0}\" <= {1}", latestPropName, ValueAsString ( valType, compOp.Value ) [0] );
                    break;

                case ComparisonOpType.GreaterThanOrEqualTo:
                    whereStr = string.Format ( "\"{0}\" >= {1}", latestPropName, ValueAsString ( valType, compOp.Value ) [0] );
                    break;

                case ComparisonOpType.Null:
                    if ( propInfoList.Count > 1 && AimMetadata.IsChoice ( propInfoList [propInfoList.Count - 2].TypeIndex ) )
                    {
                        whereStr = GetChoiceSql ( compOp, latestAimPropInfo );
                    }
                    else
                        whereStr = string.Format ( "\"{0}\" IS NUll", latestPropName );
                    break;

                case ComparisonOpType.NotNull:
                    if ( propInfoList.Count > 1 && AimMetadata.IsChoice ( propInfoList [propInfoList.Count - 2].TypeIndex ) )
                    {
                        whereStr = GetChoiceSql ( compOp, latestAimPropInfo );
                    }
                    else
                        whereStr = string.Format ( "\"{0}\" IS NOT NUll", latestPropName );
                    break;

                case ComparisonOpType.Like:
					whereStr = string.Format ( "\"{0}\" LIKE '%{1}%'", latestPropName, compOp.Value );
                    break;

                case ComparisonOpType.NotLike:
                    whereStr = string.Format ( "\"{0}\" NOT LIKE '{1}%'", latestPropName, compOp.Value );
                    break;

                case ComparisonOpType.Is:
                    if ( AimMetadata.IsChoice ( latestAimPropInfo.TypeIndex ) )
                    {
                        string[] propNames = compOp.PropertyName.Split ( '.' );
                        if ( propNames [propNames.Length - 1].ToLower () != "choice" )
                        {
                            throw new Exception ( compOp.PropertyName + ".Choice has to be written instead of " + compOp.PropertyName );
                        }
                        string choiceTableName = AimMetadata.GetAimTypeName ( latestAimPropInfo.TypeIndex );
                        whereStr = string.Format ( "prop_type = {0} OR choice_type = {0}", ( ( int ) compOp.Value ).ToString () );
                        if ( latestAimPropInfo.IsList )
                        {
                            selectTbls = selectTbls.Replace ( PropInfoParser.ReplacableString,
                                                                    string.Format ( "\"{0}\".\"Id\" IN " + 
                                                                                        "(SELECT \"{0}_id\"  from \"{0}_link\" " +
                                                                                                "where prop_index = {1} AND target_id In " +
                                                                                                        "(SELECT \"Id\" FROM \"{2}\" " + 
                                                                                                            "WHERE {3} " +
                                                                                                         ")" + 
                                                                                         ")",
                                                                         tableName, latestAimPropInfo.Index,
                                                                         choiceTableName, PropInfoParser.ReplacableString ) );

                        }
                        else
                        {
                            selectTbls = selectTbls.Replace ( PropInfoParser.ReplacableString,
                                                              string.Format ( "\"{0}\" IN (SELECT \"Id\" FROM \"{1}\" WHERE {2})",
                                                                latestPropName, choiceTableName, PropInfoParser.ReplacableString ) );
                        }
                    }
                    else
                    {
                        if ( latestAimPropInfo.IsList && latestAimPropInfo.PropType != null && AimMetadata.IsAbstractFeatureRefObject ( latestAimPropInfo.TypeIndex ) )
                        {
                            whereStr = string.Format ( "targettableindex = {0}", ( ( int ) compOp.Value ) );
                            selectTbls = selectTbls.Replace ( PropInfoParser.ReplacableString, string.Format ( "\"{0}\".\"Id\" IN " + 
                                                                                                "(SELECT \"{0}_id\"  from \"{0}_link\" " +
                                                                                                    "where prop_index = {1} AND {2})",
                                                                                                    tableName, latestAimPropInfo.Index, PropInfoParser.ReplacableString ) );
                        }
                        else
                            whereStr = string.Format ( "\"ref_{0}\" = {1}", latestPropName, ( ( int ) compOp.Value ).ToString () );
                    }
                    break;

                default:
                    throw new Exception ( "Not found Operation Sql String !" );
            }
            selectTbls = selectTbls.Replace ( PropInfoParser.ReplacableString, whereStr );
            return selectTbls;
        }

        private string GetChoiceSql ( ComparisonOps compOp, AimPropInfo latestAimPropInfo )
        {
            string result;
            if ( latestAimPropInfo.IsFeatureReference )
            {
                result = string.Format ( "(prop_type = {0} OR choice_type = {0}) AND target_guid", ( int ) latestAimPropInfo.ReferenceFeature );
            }
            else
            {
                result = string.Format ( "(prop_type = {0} OR choice_type = {0}) AND target_id ", latestAimPropInfo.TypeIndex );
            }
            if ( compOp.OperationType == ComparisonOpType.EqualTo )
                result += " = '" + compOp.Value.ToString () + "'";
            else if ( compOp.OperationType == ComparisonOpType.NotEqualTo )
                result += " != '" + compOp.Value.ToString () + "'";
            else if ( compOp.OperationType == ComparisonOpType.Null )
                result += " IS NULL";
            else if ( compOp.OperationType == ComparisonOpType.NotNull )
                result += " IS NOT NULL";
            return result;
        }

        private string SqlStringForValClassValType ( ComparisonOps compOp, int dbEntityTypeIndex, AimPropInfoList propInfoList )
        {
            PropInfoParser propInfoParser = new PropInfoParser ();
            string tableName;
            string selectTbls = PropInfoParser.ReplacableString;
            if ( propInfoList.Count > 1 )
            {
                tableName =  propInfoParser.GetTableName ( dbEntityTypeIndex );
                selectTbls = propInfoParser.AddPreviousWhereString ( ref tableName, propInfoList );
            }
            else
                tableName = propInfoParser.GetTableName ( dbEntityTypeIndex );
            string latestPropName = propInfoList [propInfoList.Count - 1].Name;
            double valueInSI =  ConverterToSI.Convert ( compOp.Value, double.NaN );
            if ( double.IsNaN ( valueInSI ) )
                throw new Exception ( "Cann't convert " + latestPropName + " !" );
            string whereStr;
            switch ( compOp.OperationType )
            {
                case ComparisonOpType.EqualTo:
                    whereStr = string.Format ( "\"{0}_SIValue\" = {1}", latestPropName, valueInSI );
                    break;

                case ComparisonOpType.NotEqualTo:
                    whereStr = string.Format ( "\"{0}_SIValue\" != {1}", latestPropName, valueInSI );
                    break;

                case ComparisonOpType.LessThan:
                    whereStr = string.Format ( "\"{0}_SIValue\" < {1}", latestPropName, valueInSI );
                    break;

                case ComparisonOpType.GreaterThan:
                    whereStr = string.Format ( "\"{0}_SIValue\" > {1}", latestPropName, valueInSI );
                    break;

                case ComparisonOpType.LessThanOrEqualTo:
                    whereStr = string.Format ( "\"{0}_SIValue\" <= {1}", latestPropName, valueInSI );
                    break;

                case ComparisonOpType.GreaterThanOrEqualTo:
                    whereStr = string.Format ( "\"{0}_SIValue\" >= {1}", latestPropName, valueInSI );
                    break;

                case ComparisonOpType.Null:
                    whereStr = string.Format ( "\"{0}_SIValue\" IS NUll ", latestPropName );
                    break;

                case ComparisonOpType.NotNull:
                    whereStr = string.Format ( "\"{0}_SIValue\" IS NOT NUll ", latestPropName );
                    break;

                default:
                    throw new Exception ( "Not found Operation Sql String !" );
            }
            selectTbls = selectTbls.Replace ( PropInfoParser.ReplacableString, whereStr );
            return selectTbls;
        }

        private string SqlStringForTextNoteValueType ( ComparisonOps compOp, ValueType valType, int dbEntityTypeIndex, AimPropInfoList propInfoList )
        {
            PropInfoParser propInfoParser = new PropInfoParser ();
            string tableName;
            string selectTbls = PropInfoParser.ReplacableString;
            if ( propInfoList.Count > 1 )
            {
                tableName = propInfoParser.GetTableName ( dbEntityTypeIndex );
                selectTbls = propInfoParser.AddPreviousWhereString ( ref tableName, propInfoList );
            }
            else
                tableName = propInfoParser.GetTableName ( dbEntityTypeIndex );
            string latestPropName = propInfoList [propInfoList.Count - 1].Name;
            List<string> textNote = ValueAsString ( valType, compOp.Value );
            switch ( compOp.OperationType )
            {
                case ComparisonOpType.EqualTo:
                    selectTbls += string.Format ( "(\"{0}_Value\" = \"{1}\" AND \"{0}_Lang\" = {2})", latestPropName, textNote [0], textNote [1] );
                    break;

                case ComparisonOpType.NotEqualTo:
                    selectTbls += string.Format ( "(\"{0}_Value\" != \"{1}\" OR \"{0}\".\"{0}_Lang\" != {2})", latestPropName, textNote [0], textNote [1] );
                    break;

                case ComparisonOpType.Null:
                    selectTbls += string.Format ( "(\"{0}_Value\" IS NULL OR \"{0}_Lang\" IS NULL)", latestPropName );
                    break;

                case ComparisonOpType.NotNull:
                    selectTbls += string.Format ( "(\"{0}_Value\" IS NOT NULL AND \"{0}_Lang\" IS NOT NULL)", latestPropName );
                    break;

                case ComparisonOpType.Like:
                    selectTbls += string.Format ( "(\"{0}_Value\" LIKE \"{1}\" AND \"{0}_Lang\" = {2})", latestPropName, textNote [0], textNote [1] );
                    break;

                case ComparisonOpType.NotLike:
                    selectTbls += string.Format ( "(\"{0}_Value\" NOT LIKE \"{1}\" AND \"{0}_Lang\" = {2})", latestPropName, textNote [0], textNote [1] );
                    break;

                default:
                    throw new Exception ( "Not found Operation Sql String !" );
            }
            return selectTbls;
        }

        private ValueType GetValueType ( object _value)
        {
            if ( _value is ADataType )
            {
                if ( _value is IEditValClass )
                    return ValueType.VT_ValClass;
                else
                    return ValueType.VT_TextNote;
            }
            else
                return ValueType.VT_Simple;
        }

        private List<string> ValueAsString ( ValueType ValType, object Value )
        {
            List<string> result = new List<string> ();
            if ( ValType == ValueType.VT_Simple )
            {
                if ( ( Value is DateTime ) || ( Value is string ) || ( Value is Guid ) )
                {
                    result.Add ( "'" + Value.ToString () + "'" );
                }
                else if ( Value is Enum )
                {
                    result.Add ( ( ( int ) Value ).ToString () );
                }
                else
                    result.Add ( Value.ToString () );
                return result;
            }
            else if ( ValType == ValueType.VT_TextNote )
            {
                string value = ( Value as TextNote ).Value;
                int uom = ( int ) ( Value as TextNote ).Lang;
                result.Add ( value.ToString () );
                result.Add ( uom.ToString () );
                return result;
            }
            throw new Exception ( "Not found Value Type" );
        }

        private enum ValueType
        {
            VT_Simple,
            VT_ValClass,
            VT_TextNote,
        };

        #endregion


    }
}
