// Decompiled with JetBrains decompiler
// Type: Technewlogic.Stuff.Extensions.ObjectExtensions
// Assembly: Technewlogic.Stuff, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BFC979D2-FC0B-46A5-8813-25E741038F56
// Assembly location: C:\Users\AliguluR\Desktop\CrudGenerator_src\Dependencies\Technewlogic.Stuff-1\Technewlogic.Stuff.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Framework.Stuff.Extensions
{
    public static class ObjectExtensions
    {
        public static IEnumerable GetPropertyValues(this object me, Type tValue)
        {
            return (IEnumerable)me.GetPropertiesOfType(tValue, true).Select<PropertyInfo, object>((Func<PropertyInfo, object>)(pi => pi.GetValue(me, (object[])null)));
        }

        public static IEnumerable<TValue> GetPropertyValues<TValue>(this object me)
        {
            return me.GetPropertyValues(typeof(TValue)).Cast<TValue>();
        }

        public static IEnumerable GetFieldValues(this object me, Type tValue)
        {
            return (IEnumerable)me.GetFieldsOfType(tValue, true).Select<FieldInfo, object>((Func<FieldInfo, object>)(it => it.GetValue(me)));
        }

        public static IEnumerable<TValue> GetFieldValues<TValue>(this object me)
        {
            return me.GetFieldValues(typeof(TValue)).Cast<TValue>();
        }

        public static IEnumerable<PropertyInfo> GetPropertiesOfType<T>(this object me, bool ignoreGeneric)
        {
            return TypeExtension.GetPropertiesOfType(me.GetType(), typeof(T), ignoreGeneric);
        }

        public static IEnumerable<PropertyInfo> GetPropertiesOfType(this object me, Type t, bool ignoreGeneric)
        {
            return TypeExtension.GetPropertiesOfType(me.GetType(), t, ignoreGeneric);
        }

        public static IEnumerable<FieldInfo> GetFieldsOfType<T>(this object me, bool ignoreGeneric)
        {
            return TypeExtension.GetFieldsOfType(me.GetType(), typeof(T), ignoreGeneric);
        }

        public static IEnumerable<FieldInfo> GetFieldsOfType(this object me, Type t, bool ignoreGeneric)
        {
            return TypeExtension.GetFieldsOfType(me.GetType(), t, ignoreGeneric);
        }

        public static IEnumerable<Type> GetInheritanceHierarchy(Type type)
        {
            for (var current = type; current != null; current = current.BaseType)
                yield return current;
        }
    }
}
