// Decompiled with JetBrains decompiler
// Type: Technewlogic.Stasy.Framework.RelationshipManager
// Assembly: Technewlogic.Stasy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B911826E-2508-4686-A374-AE5BC883E603
// Assembly location: C:\Users\AliguluR\Desktop\CrudGenerator_src\Dependencies\Technewlogic.Stasy-1\Technewlogic.Stasy.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Framework.Stuff.Extensions;

namespace Framework.Stasy.Core
{
  internal class RelationshipManager
  {
    private readonly KeyManager _keyManager;
    private readonly Type _storedType;
    private Dictionary<object, object> _primaryKeyLink;
    private Dictionary<Type, ILookup<object, object>> _foreignKeyLinks;

    public RelationshipManager(Type storedType)
    {
      this._keyManager = new KeyManager();
      this._storedType = storedType;
    }

    public void Reload(Dictionary<object, ChangeTrackingState> entities)
    {
      PropertyInfo pkProperty = this._keyManager.GetPrimaryKeyProperty(this._storedType);
      this._primaryKeyLink = entities.Keys.ToDictionary<object, object>((Func<object, object>) (entity => this._keyManager.GetPrimaryKeyValue(entity, pkProperty)));
      this._foreignKeyLinks = new Dictionary<Type, ILookup<object, object>>();
      try
      {
        IDictionary<Type, PropertyInfo> fkProperties = this._keyManager.GetForeignKeyProperties(this._storedType);
        using (IEnumerator<Type> enumerator = fkProperties.Keys.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            Type foreignKeyType = enumerator.Current;
            this._foreignKeyLinks.Add(foreignKeyType, entities.Keys.ToLookup<object, object>((Func<object, object>) (entity => this._keyManager.GetForeignKeyValue(entity, fkProperties[foreignKeyType]))));
          }
        }
      }
      catch
      {
      }
    }

    public object GetByPrimaryKeyValue(object primaryKeyValue)
    {
      return this._primaryKeyLink[primaryKeyValue];
    }

    public IEnumerable GetByForeignKeyValue(Type masterType, object foreignKeyValue)
    {
      return (IEnumerable) this._foreignKeyLinks[masterType][foreignKeyValue];
    }

    public object GetByForeignKeyPropertyOfEntity(object entity)
    {
      PropertyInfo foreignKeyProperty = this._keyManager.GetForeignKeyProperty(entity.GetType(), this._storedType);
      object foreignKeyValue = this._keyManager.GetForeignKeyValue(entity, foreignKeyProperty);
      if (foreignKeyValue != null && !foreignKeyValue.Equals(foreignKeyProperty.PropertyType.GetDefault()))
        return this.GetByPrimaryKeyValue(foreignKeyValue);
      return (object) null;
    }

    public object GetByPrimaryKeyPropertyOfEntity(object entity)
    {
      return this.GetByForeignKeyValue(entity.GetType(), this._keyManager.GetPrimaryKeyValue(entity)).Cast<object>().SingleOrDefault<object>();
    }

    public IEnumerable GetManyByPrimaryKeyPropertyOfEntity(object entity)
    {
      return this.GetByForeignKeyValue(entity.GetType(), this._keyManager.GetPrimaryKeyValue(entity));
    }
  }
}
