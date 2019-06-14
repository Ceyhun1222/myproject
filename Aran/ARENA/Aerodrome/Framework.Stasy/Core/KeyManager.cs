// Decompiled with JetBrains decompiler
// Type: Technewlogic.Stasy.Framework.KeyManager
// Assembly: Technewlogic.Stasy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B911826E-2508-4686-A374-AE5BC883E603
// Assembly location: C:\Users\AliguluR\Desktop\CrudGenerator_src\Dependencies\Technewlogic.Stasy-1\Technewlogic.Stasy.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Framework.Attributes;
using Framework.Stuff.Extensions;

namespace Framework.Stasy.Core
{
  internal class KeyManager
  {
    public object GetPrimaryKeyValue(object entity)
    {
      return this.GetPrimaryKeyProperty(entity).GetValue(entity, (object[]) null);
    }

    public object GetPrimaryKeyValue(object entity, PropertyInfo primaryKeyProperty)
    {
      return primaryKeyProperty.GetValue(entity, (object[]) null);
    }

    public PropertyInfo GetPrimaryKeyProperty(object child)
    {
      return this.GetPrimaryKeyProperty(child.GetType());
    }

    public PropertyInfo GetPrimaryKeyProperty(Type t)
    {
      IEnumerable<PropertyInfo> source = ((IEnumerable<PropertyInfo>) t.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)).Where<PropertyInfo>((Func<PropertyInfo, bool>) (pi => ((IEnumerable<object>) pi.GetCustomAttributes(typeof (PrimaryKeyAttribute), false)).Count<object>() > 0));
            if (source.Count<PropertyInfo>() == 0)
                throw new ApplicationException("There was no primary key property found");
            if (source.Count<PropertyInfo>() > 1)
                throw new ApplicationException("There was more than 1 primary key property found");
            return source.Single<PropertyInfo>();
    }

    public object GetForeignKeyValue(object child, Type parentType)
    {
      return this.GetForeignKeyProperty(child.GetType(), parentType).GetValue(child, (object[]) null);
    }

    public object GetForeignKeyValue(object child, PropertyInfo foreignKeyProperty)
    {
      return foreignKeyProperty.GetValue(child, (object[]) null);
    }

    public PropertyInfo GetForeignKeyProperty(object child, object parent)
    {
      return this.GetForeignKeyProperty(child.GetType(), parent.GetType());
    }

    public PropertyInfo GetForeignKeyProperty(Type childType, Type parentType)
    {
      IDictionary<Type, PropertyInfo> foreignKeyProperties = this.GetForeignKeyProperties(childType);
      if (!foreignKeyProperties.Keys.Contains(parentType))
        throw new ApplicationException("There was no foreign key property found");
      return foreignKeyProperties[parentType];
    }

    public IDictionary<Type, PropertyInfo> GetForeignKeyProperties(object child)
    {
      return this.GetForeignKeyProperties(child.GetType());
    }

    public IDictionary<Type, PropertyInfo> GetForeignKeyProperties(Type t)
    {
      Dictionary<Type, PropertyInfo> dictionary = ((IEnumerable<PropertyInfo>) t.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)).Where<PropertyInfo>((Func<PropertyInfo, bool>) (pi => ((IEnumerable<object>) pi.GetCustomAttributes(typeof (ForeignKeyAttribute), false)).Count<object>() > 0)).ToDictionary<PropertyInfo, Type>((Func<PropertyInfo, Type>) (pi => pi.GetCustomAttributes(typeof (ForeignKeyAttribute), false).Cast<ForeignKeyAttribute>().Single<ForeignKeyAttribute>().ParentType));
      if (dictionary.Count<KeyValuePair<Type, PropertyInfo>>() == 0)
        throw new ApplicationException("There was no foreign key property found");
      return (IDictionary<Type, PropertyInfo>) dictionary;
    }

    public void SetForeignKeyValue(object child, object value, Type parentType)
    {
      PropertyInfo foreignKeyProperty = this.GetForeignKeyProperty(child.GetType(), parentType);
      if (!value.GetType().IsType(foreignKeyProperty.PropertyType, false))
        throw new ApplicationException("The foreign key value to set has the wrong type");
      foreignKeyProperty.SetValue(child, value, (object[]) null);
    }

    public void ResetForeignKey(object child, Type parentType)
    {
      PropertyInfo foreignKeyProperty = this.GetForeignKeyProperty(child.GetType(), parentType);
      object obj = foreignKeyProperty.PropertyType.GetDefault();
      foreignKeyProperty.SetValue(child, obj, (object[]) null);
    }
  }
}
