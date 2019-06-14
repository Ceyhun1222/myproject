// Decompiled with JetBrains decompiler
// Type: Technewlogic.Stasy.EntityContext
// Assembly: Technewlogic.Stasy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B911826E-2508-4686-A374-AE5BC883E603
// Assembly location: C:\Users\AliguluR\Desktop\CrudGenerator_src\Dependencies\Technewlogic.Stasy-1\Technewlogic.Stasy.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Framework.Stasy.Core;
using Framework.Stasy.SyncProvider;

namespace Framework.Stasy
{
  public abstract class EntityContext
  {
    private List<object> _entityReferences = new List<object>();
    private Dictionary<Type, PropertyInfo> _dependentAggregations = new Dictionary<Type, PropertyInfo>();
    public readonly EntityContextManager _entityContextManager;
    public ISyncProvider _syncProvider;

    public EntityContext(ISyncProvider syncProvider)
    {
      this._entityContextManager = new EntityContextManager();
      this._syncProvider = syncProvider;
    }

    public bool IsModified
    {
      get
      {
        return this._entityContextManager.GetEntitiesWithState().Count<KeyValuePair<SynchronizationOperation, object>>() > 0;
      }
    }

    internal EntityCollection GetDataCollection<T>() where T : class
    {
      return this._entityContextManager.GetEntityCollection<T>();
    }

    public IEnumerable<T> GetReadableCollection<T>() where T : class
    {
      return this._entityContextManager.GetEntityCollection<T>().Cast<T>();
    }

    public CompositeCollection<T> GetWriteableCollection<T>() where T : class
    {
      CompositeCollection<T> compositeCollection = new CompositeCollection<T>();
      compositeCollection.SetEntityCollection(this, this._entityContextManager.GetEntityCollection<T>(), true, (object) null);
      return compositeCollection;
    }

    public virtual T CreateInstance<T>(Func<T> newMethod) where T : class
    {
      T entity = newMethod();
      this.PrepareEntity<T>(entity, false);
      return entity;
    }

    public virtual void Commit()
    {
      //this._syncProvider.Begin();
      //IEnumerable<KeyValuePair<SynchronizationOperation, object>> entitiesWithState = this._entityContextManager.GetEntitiesWithState();
      //this._syncProvider.Insert((IEnumerable) entitiesWithState.Where<KeyValuePair<SynchronizationOperation, object>>((Func<KeyValuePair<SynchronizationOperation, object>, bool>) (kvp => kvp.Key == SynchronizationOperation.Inserted)).Select<KeyValuePair<SynchronizationOperation, object>, object>((Func<KeyValuePair<SynchronizationOperation, object>, object>) (kvp => kvp.Value)));
      //this._syncProvider.Update((IEnumerable) entitiesWithState.Where<KeyValuePair<SynchronizationOperation, object>>((Func<KeyValuePair<SynchronizationOperation, object>, bool>) (kvp => kvp.Key == SynchronizationOperation.Updated)).Select<KeyValuePair<SynchronizationOperation, object>, object>((Func<KeyValuePair<SynchronizationOperation, object>, object>) (kvp => kvp.Value)));
      //this._syncProvider.Delete((IEnumerable) entitiesWithState.Where<KeyValuePair<SynchronizationOperation, object>>((Func<KeyValuePair<SynchronizationOperation, object>, bool>) (kvp => kvp.Key == SynchronizationOperation.Deleted)).Select<KeyValuePair<SynchronizationOperation, object>, object>((Func<KeyValuePair<SynchronizationOperation, object>, object>) (kvp => kvp.Value)));
      this._syncProvider.Commit();
      this._entityContextManager.ResetEntitiesState();
    }

    protected virtual void RegisterType<T>() where T : class
    {
      foreach (object register in (IEnumerable) this._entityContextManager.RegisterCollection<T>(this._syncProvider.GetEntityList<T>()))
        this.PrepareEntity<object>(register, true);

            FeatureTypeRegistration mtr = new FeatureTypeRegistration(typeof(T));           
            _registeredTyes.Add(mtr);
        }

        private readonly List<FeatureTypeRegistration> _registeredTyes
            = new List<FeatureTypeRegistration>();
        public IEnumerable<FeatureTypeRegistration> RegisteredTypes
        {
            get { return _registeredTyes; }
        }

        protected virtual void LinkObjects()
    {
      foreach (object entityReference in this._entityReferences)
        this._entityContextManager.InjectEntityCollections<object>(entityReference, true, this);
    }

    internal bool IsAssigned(object entity)
    {
      return this._entityReferences.SingleOrDefault<object>((Func<object, bool>) (o => object.ReferenceEquals(entity, o))) != null;
    }

    internal void EnsureEntityIsAssigned(object entity)
    {
      if (!this.IsAssigned(entity))
        throw new ApplicationException("The entity is not assigned to context");
    }

    public void PrepareEntity<T>(T entity, bool willBeInDB) where T : class
    {
      if (this.IsAssigned((object) entity))
        throw new ApplicationException("Entity already assigned");
      this._entityReferences.Add((object) entity);
      if (!willBeInDB)
        this._entityContextManager.InjectEntityCollections<T>(entity, willBeInDB, this);
      this.InjectBusinessContext((object) entity);
    }

    internal void InjectBusinessContext(object entity)
    {
      Type type = this.GetType();
      foreach (PropertyInfo propertyInfo in ((IEnumerable<PropertyInfo>) entity.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)).Where<PropertyInfo>((Func<PropertyInfo, bool>) (pi => ((IEnumerable<object>) pi.GetCustomAttributes(typeof (BusinessContextAttribute), false)).Count<object>() > 0)))
      {
        if (type != propertyInfo.PropertyType && !type.IsSubclassOf(propertyInfo.PropertyType))
          throw new ApplicationException("Could not inject context because context types don't match.");
        propertyInfo.SetValue(entity, (object) this, new object[0]);
      }
    }

    public void CheckRemovalDependencies(object child)
    {
    }
  }
}
