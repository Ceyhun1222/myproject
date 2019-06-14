using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Aim.BusinessRules
{
    static class SqliteProviderStack
    {
        private static Dictionary<int, IFeatureProvider> _stack;
        private static int _maxId = 1;

        static SqliteProviderStack()
        {
            _stack = new Dictionary<int, IFeatureProvider>();
        }

        internal static void RegisterFunctions()
        {
            var currType = typeof(SqliteProviderStack);
            var allTypes = currType.Assembly.GetTypes();
            foreach(var type in allTypes)
            {
                if (type.Namespace == currType.Namespace)
                {
                    if (type.Name.StartsWith("Aim") && type.Name.EndsWith("SqlFunc"))
                        SQLiteFunction.RegisterFunction(type);
                }
            }
        }

        internal static int AddProvider(IFeatureProvider pro)
        {
            _stack[_maxId] = pro;
            return _maxId++;
        }

        internal static IFeatureProvider GetProvider(int key)
        {
            _stack.TryGetValue(key, out IFeatureProvider pro);
            return pro;
        }

        internal static void RemoveProvider(int key)
        {
            _stack.Remove(key);
        }
    }

    static class SqliteQueryValueStack
    {
        private static Dictionary<int, object> _stack;
        private static int _maxId = 1;

        static SqliteQueryValueStack()
        {
            _stack = new Dictionary<int, object>();
        }

        internal static int AddValue(object value)
        {
            _stack[_maxId] = value;
            return _maxId++;
        }

        internal static object GetValue(int key)
        {
            _stack.TryGetValue(key, out object value);
            return value;
        }

        internal static void RemoveValue(int key)
        {
            _stack.Remove(key);
        }

        internal static void RemoveValues(IEnumerable<int> keys)
        {
            foreach(var item in keys)
                _stack.Remove(item);
        }
    }

    internal static class AimProp
    {
        internal static IEnumerable<object> GetProperty(object[] args)
        {
            var providerKey = Convert.ToInt32(args[0]);
            var dbPro = SqliteProviderStack.GetProvider(providerKey);
            if (dbPro == null)
                return null;

            var featType = (FeatureType)Convert.ToInt32(args[1]);
            var idenP1 = (long)args[2];
            var idenP2 = (long)args[3];
            var propName = (string)args[4];

            var identifier = GuidConverter.ToGuid(idenP1, idenP2);

            var feat = dbPro.GetFeature(featType, identifier);

            return AimPropertyGetter.GetPropValue(feat, propName);
        }
    }

    [SQLiteFunction(Name = "AimIsAssigned", Arguments = 5, FuncType = FunctionType.Scalar)]
    public class AimIsAssignedSqlFunc : SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            var providerKey = Convert.ToInt32(args[0]);
            var dbPro = SqliteProviderStack.GetProvider(providerKey);
            if (dbPro == null)
                return null;

            var featType = (FeatureType)Convert.ToInt32(args[1]);
            var idenP1 = (long)args[2];
            var idenP2 = (long)args[3];
            var propName = (string)args[4];

            var identifier = GuidConverter.ToGuid(idenP1, idenP2);
            var feat = dbPro.GetFeature(featType, identifier);
            var propVars = AimPropertyGetter.GetPropValue(feat, propName);
            var enmr = propVars.GetEnumerator();
            var res = (enmr.MoveNext() && enmr.Current != null);
            return res;
        }
    }

    [SQLiteFunction(Name = "AimRefCount", Arguments = 7, FuncType = FunctionType.Scalar)]
    public class AimRefCountSqlFunc : SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            var providerKey = Convert.ToInt32(args[0]);
            var dbPro = SqliteProviderStack.GetProvider(providerKey);
            if (dbPro == null)
                return null;

            var featType = (FeatureType)Convert.ToInt32(args[1]);
            var idenP1 = (long)args[2];
            var idenP2 = (long)args[3];
            var propName = (string)args[4];

            var refFeatType = (FeatureType)Convert.ToInt32(args[5]);
            var refCount = Convert.ToInt32(args[6]);

            var identifier = GuidConverter.ToGuid(idenP1, idenP2);
            var feat = dbPro.GetFeature(featType, identifier);
            var propVars = AimPropertyGetter.GetPropValue(feat, propName);

            foreach (var item in propVars)
            {
                if (item is Guid itemIdentifier)
                {
                    var realRefCount = dbPro.GetFeatureCount(refFeatType, itemIdentifier);
                    if (refCount != realRefCount)
                        return false;
                }
            }

            return true;
        }
    }


    [SQLiteFunction(Name = "AimRefCountByTypes", Arguments = 7, FuncType = FunctionType.Scalar)]
    public class AimRefCountByTypesSqlFunc : SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            var providerKey = Convert.ToInt32(args[0]);
            var dbPro = SqliteProviderStack.GetProvider(providerKey);
            if (dbPro == null)
                return null;

            var featType = (FeatureType)Convert.ToInt32(args[1]);
            var idenP1 = (long)args[2];
            var idenP2 = (long)args[3];
            var propName = (string)args[4];

            var refFeatTypesKey = Convert.ToInt32(args[5]);
            var refFeatTypes = SqliteQueryValueStack.GetValue(refFeatTypesKey) as object[];
            var refCount = Convert.ToInt32(args[6]);

            var identifier = GuidConverter.ToGuid(idenP1, idenP2);
            var feat = dbPro.GetFeature(featType, identifier);
            var propVars = AimPropertyGetter.GetPropValue(feat, propName);

            foreach (var item in propVars)
            {
                if (item is Guid itemIdentifier)
                {
                    var realRefCount = 0;
                    foreach(var refFeatType in refFeatTypes)
                    {
                        var x = dbPro.GetFeatureCount((FeatureType)refFeatType, itemIdentifier);
                        realRefCount += x;
                    }

                    if (refCount != realRefCount)
                        return false;
                }
            }

            return true;
        }
    }

    [SQLiteFunction(Name = "AimPropCount", Arguments = 7, FuncType = FunctionType.Scalar)]
    public class AimPropCountSqlFunc : SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            var providerKey = Convert.ToInt32(args[0]);
            var dbPro = SqliteProviderStack.GetProvider(providerKey);
            if (dbPro == null)
                return null;

            var featType = (FeatureType)Convert.ToInt32(args[1]);
            var idenP1 = (long)args[2];
            var idenP2 = (long)args[3];
            var propName = (string)args[4];

            var identifier = GuidConverter.ToGuid(idenP1, idenP2);
            var feat = dbPro.GetFeature(featType, identifier);
            var propVars = AimPropertyGetter.GetPropValue(feat, propName);

            int propCount = 0;

            var conditionType = PropConditionType.None;
            if (args[5] != null && args[5] != DBNull.Value)
                conditionType = (PropConditionType)Convert.ToInt32(args[5]);

            if (conditionType == PropConditionType.None)
            {
                foreach (var item in propVars)
                {
                    if (item != null)
                        propCount++;
                }
            }
            else
            {
                var rightItemsKey = Convert.ToInt32(args[6]);
                var rightItems = SqliteQueryValueStack.GetValue(rightItemsKey) as object[];
                if (rightItems == null || rightItems.Length == 0)
                    return false;

                foreach (var left in propVars)
                {
                    var inResult = false;
                    foreach (var right in rightItems)
                    {
                        if (left != null && right != null && 
                            PropConditionChecker.Check(left, conditionType, right))
                        {
                            inResult = true;
                            break;
                        }
                    }

                    if (inResult)
                        propCount++;
                }
            }

            return propCount;
        }
    }

    [SQLiteFunction(Name = "AimEqualTo", Arguments = 6, FuncType = FunctionType.Scalar)]
    public class AimEqualToSqlFunc : SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            var rightItemsKey = Convert.ToInt32(args[5]);
            var rightItems = SqliteQueryValueStack.GetValue(rightItemsKey) as object[];
            if (rightItems == null || rightItems.Length == 0)
                return false;

            var leftItems = AimProp.GetProperty(args);
            if (leftItems == null)
                return false;

            var result = false;

            foreach(var left in leftItems)
            {
                var inResult = false;
                foreach(var right in rightItems)
                {
                    if (left != null && right != null)
                    {
                        if (left is bool && right is string)
                        {
                            if (((string)right).Equals("yes", StringComparison.InvariantCultureIgnoreCase))
                                inResult = (bool)left;
                            else if (((string)right).Equals("no", StringComparison.InvariantCultureIgnoreCase))
                                inResult = !(bool)left;
                        }
                        else
                        {
                            inResult = left.Equals(right);
                        }

                        if (inResult)
                            break;
                    }
                }

                if (!inResult)
                    return false;
                else
                    result = true;
            }

            return result;
        }
    }

    [SQLiteFunction(Name = "AimEqualToProp", Arguments = 6, FuncType = FunctionType.Scalar)]
    public class AimEqualToPropSqlFunc : SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            var providerKey = Convert.ToInt32(args[0]);
            var dbPro = SqliteProviderStack.GetProvider(providerKey);
            if (dbPro == null)
                return null;

            var featType = (FeatureType)Convert.ToInt32(args[1]);
            var idenP1 = (long)args[2];
            var idenP2 = (long)args[3];
            var propName = (string)args[4];

            var identifier = GuidConverter.ToGuid(idenP1, idenP2);

            var feat = dbPro.GetFeature(featType, identifier);

            var leftItems = AimPropertyGetter.GetPropValue(feat, propName);

            var rightPropName = (args[5] as string);
            
            var result = false;

            foreach (var left in leftItems)
            {
                var inResult = false;

                var rightItems = AimPropertyGetter.GetPropValue(feat, rightPropName);

                foreach (var right in rightItems)
                {
                    if (left != null && right != null &&
                        string.Equals(left.ToString(), right.ToString(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        inResult = true;
                        break;
                    }
                }

                if (!inResult)
                    return false;
                else
                    result = true;
            }

            return result;
        }
    }

    [SQLiteFunction(Name = "AimHigherLowerEqualProp", Arguments = 7, FuncType = FunctionType.Scalar)]
    public class AimHigherLowerEqualPropSqlFunc : SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            var providerKey = Convert.ToInt32(args[0]);
            var dbPro = SqliteProviderStack.GetProvider(providerKey);
            if (dbPro == null)
                return null;

            var featType = (FeatureType)Convert.ToInt32(args[1]);
            var idenP1 = (long)args[2];
            var idenP2 = (long)args[3];
            var propName = (string)args[4];

            var identifier = GuidConverter.ToGuid(idenP1, idenP2);
            var feat = dbPro.GetFeature(featType, identifier);
            var leftItems = AimPropertyGetter.GetPropValue(feat, propName);
            var rightPropName = args[5] as string;
            var opr = args[6] as string;

            var result = false;

            foreach (var left in leftItems)
            {
                var inResult = false;

                var rightItems = AimPropertyGetter.GetPropValue(feat, rightPropName);

                foreach (var right in rightItems)
                {
                    if (left != null && right != null)
                    {
                        try
                        {
                            var leftD = Convert.ToDouble(left);
                            var rightD = Convert.ToDouble(right);

                            if (opr == ">")
                                inResult = (leftD > rightD);
                            else if (opr == ">=")
                                inResult = (leftD >= rightD);
                            else if (opr == "<")
                                inResult = (leftD < rightD);
                            else if (opr == "<=")
                                inResult = (leftD <= rightD);
                        }
                        catch { }
                    }
                }

                if (!inResult)
                    return false;
                else
                    result = true;
            }

            return result;
        }
    }

    [SQLiteFunction(Name = "AimHigherLowerEqual", Arguments = 7, FuncType = FunctionType.Scalar)]
    public class AimHigherLowerEqualSqlFunc : SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            if (args[6] == null)
                return false;

            var opr = args[5] as string;

            var leftItems = AimProp.GetProperty(args);
            if (leftItems == null)
                return false;

            var rightItemsKey = Convert.ToInt32(args[6]);
            var rightItems = SqliteQueryValueStack.GetValue(rightItemsKey) as object[];
            if (rightItems == null || rightItems.Length == 0)
                return false;

            double rightD = 0;

            try
            {
                rightD = Convert.ToDouble(rightItems[0]);
            }
            catch
            {
                Console.WriteLine("Could not convert to double, arg: {0}", rightItems[0]);
            }


            var result = false;

            foreach (var left in leftItems)
            {
                if (left != null)
                {
                    try
                    {
                        var leftD = Convert.ToDouble(left);

                        if (opr == ">")
                            result = (leftD > rightD);
                        else if (opr == ">=")
                            result = (leftD >= rightD);
                        else if (opr == "<")
                            result = (leftD < rightD);
                        else if (opr == "<=")
                            result = (leftD <= rightD);
                    }
                    catch
                    {
                        Console.WriteLine("Could not convert to double, arg: {0}", left);
                    }
                }

                if (!result)
                    return false;
            }

            return result;
        }
    }

    [SQLiteFunction(Name = "AimToGuid", Arguments = 2, FuncType = FunctionType.Scalar)]
    public class AimToGuidSqlFunc : SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            if (args != null && args.Length > 1)
            {
                try
                {
                    var p1 = Convert.ToInt64(args[0]);
                    var p2 = Convert.ToInt64(args[1]);
                    var identifier = GuidConverter.ToGuid(p1, p2).ToString();
                    return identifier;
                }
                catch { }
            }
            return null;
        }
    }

    [SQLiteFunction(Name = "GetConcatPropValue", Arguments = 6, FuncType = FunctionType.Scalar)]
    public class AimGetConcatPropValueSqlFunc : SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            var propValues = AimProp.GetProperty(args);
            if (propValues == null)
                return null;

            var seperator = args[5] as string;
            if (string.IsNullOrEmpty(seperator))
                seperator = ";";

            var result = new StringBuilder();

            foreach(var propVal in propValues)
            {
                if (result.Length > 0)
                    result.Append(seperator);
                result.Append(propVal);
            }

            if (result.Length == 0)
                return null;
            return result.ToString();
        }
    }

    public enum PropConditionType
    {
        None,
        Equal
    }

    internal static class PropConditionChecker
    {
        public static bool Check(object a, PropConditionType condition, object b)
        {
            switch(condition)
            {
                case PropConditionType.Equal:
                    return Equals(a, b);
                default:
                    return false;
            }
        }
    }

}
