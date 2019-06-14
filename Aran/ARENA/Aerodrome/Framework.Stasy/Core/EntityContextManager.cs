// Decompiled with JetBrains decompiler
// Type: Technewlogic.Stasy.Framework.EntityContextManager
// Assembly: Technewlogic.Stasy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B911826E-2508-4686-A374-AE5BC883E603
// Assembly location: C:\Users\AliguluR\Desktop\CrudGenerator_src\Dependencies\Technewlogic.Stasy-1\Technewlogic.Stasy.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Framework.Stuff.Extensions;

namespace Framework.Stasy.Core
{
  public class EntityContextManager
  {
    private Dictionary<Type, EntityCollection> _entityListCache = new Dictionary<Type, EntityCollection>();

    public void ResetEntitiesState()
    {
      foreach (KeyValuePair<Type, EntityCollection> keyValuePair in this._entityListCache)
        keyValuePair.Value.ResetEntitiesState();
    }

    public IEnumerable<KeyValuePair<SynchronizationOperation, object>> GetEntitiesWithState()
    {
      return this._entityListCache.Values.SelectMany<EntityCollection, KeyValuePair<SynchronizationOperation, object>>((Func<EntityCollection, IEnumerable<KeyValuePair<SynchronizationOperation, object>>>) (ec => ec.GetEntitiesWithState()));
    }

    public IEnumerable<Type> RegisteredTypes
    {
      get
      {
        return (IEnumerable<Type>) this._entityListCache.Keys;
      }
    }

    public EntityCollection RegisterCollection<T>(IEnumerable<T> collection) where T : class
    {
      T[] array = collection.ToArray<T>();
      Type type = typeof (T);
      if (this._entityListCache.Keys.Contains<Type>(type))
        throw new ApplicationException("The type is already registered");
      EntityCollection entityCollection = new EntityCollection(this, (IEnumerable) array, type);
      this._entityListCache.Add(type, entityCollection);
      return entityCollection;
    }

    public EntityCollection TryGetEntityCollection<T>() where T : class
    {
      return this.TryGetEntityCollection(typeof (T));
    }

    public EntityCollection GetEntityCollection<T>() where T : class
    {
      return this.GetEntityCollection(typeof (T));
    }

    public EntityCollection TryGetEntityCollection(Type t)
    {
      if (this._entityListCache.ContainsKey(t))
        return this._entityListCache[t];
      return (EntityCollection) null;
    }

    public EntityCollection GetEntityCollection(Type t)
    {
      EntityCollection entityCollection = this.TryGetEntityCollection(t);
      if (entityCollection == null)
        throw new ApplicationException("The type is not registered");
      return entityCollection;
    }

    public void InjectEntityCollections<T>(T entity, bool willBeInDB, EntityContext context) where T : class
    {
      foreach (EntitySync fieldValue in entity.GetFieldValues<EntitySync>())
      {
        EntityCollection collection = this._entityListCache[((IEnumerable<Type>) fieldValue.GetType().GetGenericArguments()).First<Type>()];
        fieldValue.SetEntityCollection(context, collection, willBeInDB, (object) entity);
      }
    }
  }
}
