// Decompiled with JetBrains decompiler
// Type: Technewlogic.Stuff.Extensions.TypeExtension
// Assembly: Technewlogic.Stuff, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BFC979D2-FC0B-46A5-8813-25E741038F56
// Assembly location: C:\Users\AliguluR\Desktop\CrudGenerator_src\Dependencies\Technewlogic.Stuff-1\Technewlogic.Stuff.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Framework.Stuff.Extensions
{
  public static class TypeExtension
  {
    public static bool IsCollection(this Type me)
    {
      if (me == typeof (string))
        return false;
      if (!me.IsArray)
        return me.GetInterface("IEnumerable") != null;
      return true;
    }

    public static Type GetCollectionType(this Type me)
    {
      if (!me.IsCollection())
        return (Type) null;
      if (!me.IsGenericType)
        return typeof (object);
      return ((IEnumerable<Type>) me.GetInterface("IEnumerable`1").GetGenericArguments()).First<Type>();
    }

    public static bool IsType(this Type me, Type t, bool ignoreGeneric)
    {
      Type type = me;
      Type c = t;
      if (ignoreGeneric)
      {
        if (((IEnumerable<Type>) type.GetGenericArguments()).Count<Type>() > 0)
          type = type.GetGenericTypeDefinition();
        if (((IEnumerable<Type>) c.GetGenericArguments()).Count<Type>() > 0)
          c = c.GetGenericTypeDefinition();
      }
      if (!type.IsSubclassOf(c))
        return type == c;
      return true;
    }

    public static bool IsReferenceType(this Type me)
    {
      if (!me.IsPrimitive && !me.IsType(typeof (string), false) && !me.IsEnum)
        return !me.IsValueType;
      return false;
    }

    public static object GetDefault(this Type me)
    {
      if (me.IsValueType)
        return Activator.CreateInstance(me);
      return (object) null;
    }

    public static IEnumerable<PropertyInfo> GetPropertiesOfType<T>(this Type me, bool ignoreGeneric)
    {
      return TypeExtension.GetPropertiesOfType(me, typeof (T), ignoreGeneric);
    }

    public static IEnumerable<PropertyInfo> GetPropertiesOfType(this Type me, Type t, bool ignoreGeneric)
    {
      return ((IEnumerable<PropertyInfo>) me.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)).Where<PropertyInfo>((Func<PropertyInfo, bool>) (pi => pi.PropertyType.IsType(t, ignoreGeneric)));
    }

    public static IEnumerable<FieldInfo> GetFieldsOfType<T>(this Type me, bool ignoreGeneric)
    {
      return TypeExtension.GetFieldsOfType(me, typeof (T), ignoreGeneric);
    }

    public static IEnumerable<FieldInfo> GetFieldsOfType(this Type me, Type t, bool ignoreGeneric)
    {
      return ((IEnumerable<FieldInfo>) me.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)).Where<FieldInfo>((Func<FieldInfo, bool>) (it => it.FieldType.IsType(t, ignoreGeneric)));
    }
  }
}
