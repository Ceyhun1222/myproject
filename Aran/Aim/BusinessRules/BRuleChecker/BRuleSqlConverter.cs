using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Aran.Aim.Utilities;

namespace Aran.Aim.BusinessRules
{
    public static class BRuleSqlConverter
    {
        public static List<CommandInfo> ToSqlCommand(BRule brule)
        {
            var result = new List<CommandInfo>();

            var typeIndexList = NounToAimTypeIndex(brule.Noun);

            foreach (var typeIndex in typeIndexList)
            {
                var cInfo = AimMetadata.GetClassInfoByIndex(typeIndex);

                List<Tuple<FeatureType, string>> featTypePrefixList = null;

                if (cInfo.AimObjectType == AimObjectType.Feature)
                {
                    featTypePrefixList = new List<Tuple<FeatureType, string>>
                    {
                        new Tuple<FeatureType, string>((FeatureType)cInfo.Index, null)
                    };
                }
                else if (cInfo.AimObjectType == AimObjectType.Object)
                {
                    featTypePrefixList = AimMetadataUtility.GetUsedByFeatureTypeList((ObjectType)cInfo.Index, ".", true, true);
                }

                foreach(var featTypePrefix in featTypePrefixList)
                {
                    var cmdInfo = new CommandInfo { FeatureType = featTypePrefix.Item1 };

                    cmdInfo.Command = ToSqlCommand(featTypePrefix.Item2, brule.Type, cmdInfo.FeatureType, brule.Operation, cmdInfo.CommandValues);

                    if (!string.IsNullOrEmpty(cmdInfo.Command))
                        result.Add(cmdInfo);
                }
            }

            return result;
        }


        private static string ToSqlCommand(string featPropPrefix, RuleType rootRoleType, FeatureType featType, AbstractOperation oper, List<object> commandValues)
        {
            var sb = new StringBuilder();

            if (oper != null)
            {
                var isOk = AddAbsOperToSqlCommand(featPropPrefix, sb, rootRoleType, oper, commandValues);

                if (!isOk)
                    return null;
            }

            var cmd =
                "SELECT identifier_p1, identifier_p2 " +
                "FROM features " +
                "WHERE feat_type = :featType";

            if (sb.Length > 0)
            {
                if (!string.IsNullOrEmpty(featPropPrefix))
                    cmd += " AND ($AimIsAssigned$('" + featPropPrefix + "') AND (" + sb + "))";
                else
                    cmd += " AND (" + sb + ")";
            }

            return cmd;
        }

        private static bool AddAbsOperToSqlCommand(string featPropPrefix, StringBuilder sqlCommand, RuleType rootRoleType, AbstractOperation oper, List<object> commandValues)
        {
            if (oper.OperType == AbstractOperationType.Operation)
            {
                return AddOperSqlCommand(featPropPrefix, sqlCommand, rootRoleType, oper as Operation, commandValues);
            }
            else
            {
                var operGroup = oper as OperationGroup;
                for (var i = 0; i < operGroup.Items.Count; i++)
                {
                    var isOk = AddAbsOperToSqlCommand(featPropPrefix, sqlCommand, rootRoleType, operGroup.Items[i], commandValues);
                    if (!isOk)
                        return false;
                }
            }

            return true;
        }

        private static bool AddOperSqlCommand(string featPropPrefix, StringBuilder sqlCommand, RuleType rootRoleType, Operation oper, List<object> commandValues)
        {
            string cmd = null;

            switch (oper.Type)
            {
                case OperationType.Assigned:
                    cmd = GetAssignedCommand(featPropPrefix, oper);
                    break;
                case OperationType.Equal:
                    cmd = GetEqualCommand(featPropPrefix, oper, commandValues);
                    break;
                case OperationType.ExactlyOne:
                    cmd = GetQuantityCommand(featPropPrefix, oper, commandValues, "=", 1);
                    break;
                case OperationType.Higher:
                case OperationType.HigherEqual:
                case OperationType.Lower:
                case OperationType.LowerEqual:
                    cmd = GetHigherLowerEqualCommand(featPropPrefix, oper, commandValues);
                    break;
                case OperationType.ResolvedInto:
                    cmd = GetResolvedIntoCommand(featPropPrefix, oper, commandValues);
                    break;
                case OperationType.MoreThanOne:
                    cmd = GetQuantityCommand(featPropPrefix, oper, commandValues, ">", 1);
                    break;
                case OperationType.AtLeast:
                    cmd = GetQuantityCommand(featPropPrefix, oper, commandValues, ">", null);
                    break;
                case OperationType.AtMost:
                    cmd = GetQuantityCommand(featPropPrefix, oper, commandValues, "<", null);
                    break;
                case OperationType.OtherThan:
                    cmd = "NOT " + GetEqualCommand(featPropPrefix, oper, commandValues);
                    break;
                case OperationType.Expressed:
                    cmd = GetExpressedCommand(featPropPrefix, oper, commandValues);
                    break;
            }

            if (!string.IsNullOrEmpty(cmd))
            {
                if (sqlCommand.Length > 0)
                    sqlCommand.Append(" AND ");
                
                if (oper.IsWith)
                {
                    if (oper.IsNot)
                        sqlCommand.Append("NOT ");
                }
                else
                {
                    if (!(oper.IsNot ^ (rootRoleType == RuleType.Prohibited)))
                        sqlCommand.Append("NOT ");
                }

                
                sqlCommand.Append("(");
                sqlCommand.Append(cmd);
                sqlCommand.Append(")");

                return true;
            }

            return false;
        }


        private static string GetAssignedCommand(string featPropPrefix, Operation oper)
        {
            if (string.IsNullOrWhiteSpace(oper.Noun))
                return null;

            return string.Format("$AimIsAssigned$('{0}')", ReplaceNoun(featPropPrefix, oper.Noun));
        }

        private static string GetEqualCommand(string featPropPrefix, Operation oper, List<object> commandValues)
        {
            if (oper.Value == null)
                return null;

            if (oper.Value.StartsWith("$prop$"))
            {
                return string.Format("$AimEqualToProp$('{0}', '{1}')",
                    ReplaceNoun(featPropPrefix, oper.Noun), 
                    ReplaceNoun(featPropPrefix, oper.Value.Substring(6)));
            }

            var value = ParseEqualValue(oper.Value);
            commandValues.Add(value);
            return string.Format("$AimEqualTo$('{0}', :v{1})", ReplaceNoun(featPropPrefix, oper.Noun), commandValues.Count - 1);
        }

        private static string GetResolvedIntoCommand(string featPropPrefix, Operation oper, List<object> commandValues)
        {
            if (oper.Association == null)
                return null;

            var refCount = 0;

            if (oper.Value == "exactly one")
                refCount = 1;
            else
                return null;
            
            var assocTypes = NounToAimTypeIndex(oper.Association.Noun);
            if (assocTypes.Count == 0)
                return null;

            string str;

            if (assocTypes.Count == 1)
            {
                str = string.Format("$AimRefCount$('{0}', :v{1}, {2})", 
                    ReplaceNoun(featPropPrefix, oper.Noun), commandValues.Count, refCount);
                commandValues.Add(assocTypes[0]);
            }
            else
            {
                str = string.Format("$AimRefCountByTypes$('{0}', :v{1}, {2})", 
                    ReplaceNoun(featPropPrefix, oper.Noun), commandValues.Count, refCount);

                var objArr = new object[assocTypes.Count];
                for (var i = 0; i < assocTypes.Count; i++)
                    objArr[i] = assocTypes[i];
                commandValues.Add(objArr);
            }

            //foreach (var item in assocTypes)
            //{
            //    if (str.Length > 0)
            //        str += " OR ";
            //    str += string.Format("$AimRefCount$('{0}', :v{1}, {2})", ReplaceNoun(featPropPrefix, oper.Noun), commandValues.Count, refCount);
            //    commandValues.Add(item);
            //}

            return str;
        }

        private static string GetQuantityCommand(string featPropPrefix, Operation oper, List<object> commandValues, string refOper, int? refCount)
        {
            if (refCount == null)
            {
                //*** TODO Other than ONE quantity keywords must implement...
                return null;
            }

            string str = null;

            if (oper.InnerOper == null)
            {
                str = string.Format("$AimPropCount$('{0}', null, null) {1} {2}", ReplaceNoun(featPropPrefix, oper.Noun), refOper, refCount);
            }
            else
            {
                str = string.Format("$AimPropCount$('{0}', :v{1}, :v{2}) {3} {4}", ReplaceNoun(featPropPrefix, oper.Noun), commandValues.Count, commandValues.Count + 1, refOper, refCount);
                commandValues.Add((int)(PropConditionType)oper.InnerOper.Type);
                var value = ParseEqualValue(oper.InnerOper.Value);
                if (value != null)
                    commandValues.Add(value);
            }

            return str;
        }

        private static string GetHigherLowerEqualCommand(string featPropPrefix, Operation oper, List<object> commandValues)
        {
            if (oper.Value == null)
                return null;

            string op;

            switch(oper.Type)
            {
                case OperationType.Higher:
                    op = ">";
                    break;
                case OperationType.HigherEqual:
                    op = ">=";
                    break;
                case OperationType.LowerEqual:
                    op = "<=";
                    break;
                default:
                    op = "<";
                    break;
            }

            if (oper.Value.StartsWith("$prop$"))
                return string.Format("$AimHigherLowerEqualProp$('{0}', '{1}', '{2}')", ReplaceNoun(featPropPrefix, oper.Noun), op, oper.Value.Substring(6));

            var value = ParseEqualValue(oper.Value);
            commandValues.Add(value);
            return string.Format("$AimHigherLowerEqual$('{0}', '{1}', :v{2})", ReplaceNoun(featPropPrefix, oper.Noun), op, commandValues.Count - 1);
        }

        private static string GetExpressedCommand(string featPropPrefix, Operation oper, List<object> commandValues)
        {
            if (oper.Value == null || oper.ExpressedType == ExpressedType.None)
                return null;
            
            ///*** oper value format: "<number>;<exp-type>"
            var sa = oper.Value.Split(';');
            var expValueType = sa[1];

            if (expValueType == "letters")
            {
                var expTypeText = 
                    (oper.ExpressedType == ExpressedType.Less ? "<" :
                    (oper.ExpressedType == ExpressedType.Exactly ? "=" : 
                    (oper.ExpressedType == ExpressedType.More ? ">" : "")));

                commandValues.Add(Convert.ToInt32(sa[0]));

                return string.Format("length($GetConcatPropValue$('{0}', NULL)){1}:v{2}", 
                    ReplaceNoun(featPropPrefix, oper.Noun),
                    expTypeText, 
                    commandValues.Count - 1);
            }
            else if (expValueType == "digits")
            {
                var expTypeText = 
                    (oper.ExpressedType == ExpressedType.Less ? "<" :
                    (oper.ExpressedType == ExpressedType.Exactly ? "=" : 
                    (oper.ExpressedType == ExpressedType.More ? ">" : "")));

                int num = Convert.ToInt32(sa[0]);
                commandValues.Add(Math.Pow(10, num) - 1);

                return string.Format("CAST($GetConcatPropValue$('{0}', NULL) AS INT){1}:v{2}", 
                    ReplaceNoun(featPropPrefix, oper.Noun),
                    expTypeText, 
                    commandValues.Count - 1);
            }

            return null;
        }


        private static object[] ParseEqualValue(string value)
        {
            //('FLOOR', 'CEILING', 'GND', 'UNL')

            if (value == null)
                return null;

            string[] sa = null;

            if (value[0] == '(' && value[value.Length - 1] == ')')
            {
                value = value.Substring(1, value.Length - 2);
                sa = value.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                sa = new string[] { value };
            }

            var res = new List<object>();

            foreach(var item in sa)
            {
                if (string.IsNullOrEmpty(item))
                    continue;

                var newItem = item.Trim();

                if (newItem[0] == '\'' && newItem[newItem.Length - 1] == '\'')
                {
                    res.Add(newItem.Substring(1, newItem.Length - 2));
                }
                else
                {
                    string s = null;
                    if (newItem.EndsWith("M"))
                        s = "M";
                    else if (newItem.EndsWith("FT"))
                        s = "FT";

                    if (s != null)
                        newItem = newItem.Substring(0, newItem.Length - s.Length);

                    if (double.TryParse(newItem, out double dVal))
                    {
                        res.Add(dVal);
                        if (s != null)
                            res.Add(s);
                    }
                }

            }

            return res.ToArray();
        }

        private static List<int> NounToAimTypeIndex(Noun noun)
        {
            var list = new List<int>();

            if (noun.Childs.Count == 0)
            {
                var classInfo = AimMetadata.GetClassInfoByName(noun.Name);
                if (classInfo != null)
                    list.Add(classInfo.Index);
            }
            else
            {
                foreach(var child in noun.Childs)
                    list.AddRange(NounToAimTypeIndex(child));
            }

            return list;
        }

        private static string ReplaceNoun(string featPropPrefix, string noun)
        {
            if (noun.EndsWith("Point.pos"))
                noun = noun.Substring(0, noun.Length - 9) + "Point.geo";

            if (featPropPrefix == null || featPropPrefix.Length == 0)
                return noun;

            return featPropPrefix + "." + noun;
        }
    }
}
