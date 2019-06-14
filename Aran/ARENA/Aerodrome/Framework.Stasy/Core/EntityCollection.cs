// Decompiled with JetBrains decompiler
// Type: Technewlogic.Stasy.Framework.EntityCollection
// Assembly: Technewlogic.Stasy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B911826E-2508-4686-A374-AE5BC883E603
// Assembly location: C:\Users\AliguluR\Desktop\CrudGenerator_src\Dependencies\Technewlogic.Stasy-1\Technewlogic.Stasy.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Framework.Stuff.Extensions;

namespace Framework.Stasy.Core
{
  public class EntityCollection : IEnumerable
  {
    private Dictionary<object, ChangeTrackingState> _stateLink = new Dictionary<object, ChangeTrackingState>();
    private readonly PropertyChangedEventHandler _propertyChangedEventHandler;
    private RelationshipManager _relationshipManager;

    public EntityCollection(EntityContextManager ecm, IEnumerable sourceList, Type storedType)
    {
      this._propertyChangedEventHandler = (PropertyChangedEventHandler) ((source, args) => this.MarkForUpdate(source));
      foreach (object source in sourceList)
        this.InsertEntity(source, SynchronizationOperation.Nothing);
      this._relationshipManager = new RelationshipManager(storedType);
      //this._relationshipManager.Reload(this._stateLink);
    }

    private void InsertEntity(object entity, SynchronizationOperation initialState)
    {
      INotifyPropertyChanged notifyPropertyChanged = entity as INotifyPropertyChanged;
      if (notifyPropertyChanged == null)
        throw new ApplicationException("The entity of type " + entity.GetType().Name + " must implement INotifyPropertyChanged");
      notifyPropertyChanged.PropertyChanged += this._propertyChangedEventHandler;
      this._stateLink.Add(entity, new ChangeTrackingState(initialState));
    }

    private void RemoveEntity(object entity)
    {
      INotifyPropertyChanged notifyPropertyChanged = entity as INotifyPropertyChanged;
      if (notifyPropertyChanged == null)
        throw new ApplicationException("The entity of type " + entity.GetType().Name + " must implement INotifyPropertyChanged");
      notifyPropertyChanged.PropertyChanged -= this._propertyChangedEventHandler;
      this._stateLink.Remove(entity);
    }

    public object GetByPrimaryKeyValue(object primaryKeyValue)
    {
      return this._relationshipManager.GetByPrimaryKeyValue(primaryKeyValue);
    }

    public IEnumerable GetByForeignKeyValue(Type masterType, object foreignKeyValue)
    {
      return this._relationshipManager.GetByForeignKeyValue(masterType, foreignKeyValue);
    }

    public object GetByForeignKeyPropertyOfEntity(object entity)
    {
      return this._relationshipManager.GetByForeignKeyPropertyOfEntity(entity);
    }

    public object GetByPrimaryKeyPropertyOfEntity(object entity)
    {
      return this._relationshipManager.GetByPrimaryKeyPropertyOfEntity(entity);
    }

    public IEnumerable GetManyByPrimaryKeyPropertyOfEntity(object entity)
    {
      return this._relationshipManager.GetManyByPrimaryKeyPropertyOfEntity(entity);
    }

    public void MarkForInsert(object entity)
    {
      switch (this.GetState(entity).SynchronizationOperation)
      {
        case SynchronizationOperation.Unknown:
          this.InsertEntity(entity, SynchronizationOperation.Inserted);
          this.SetState(entity, SynchronizationOperation.Inserted);
          break;
        case SynchronizationOperation.Nothing:
        case SynchronizationOperation.Updated:
          throw new StateException();
        case SynchronizationOperation.Inserted:
          break;
        case SynchronizationOperation.Deleted:
          this.SetState(entity, SynchronizationOperation.Updated);
          break;
        default:
          throw new ApplicationException("State unknown");
      }
    }

    public void MarkForUpdate(object entity)
    {
      switch (this.GetState(entity).SynchronizationOperation)
      {
        case SynchronizationOperation.Unknown:
          throw new StateException();
        case SynchronizationOperation.Nothing:
          this.SetState(entity, SynchronizationOperation.Updated);
          break;
        case SynchronizationOperation.Inserted:
          break;
        case SynchronizationOperation.Updated:
          break;
        case SynchronizationOperation.Deleted:
          break;
        default:
          throw new ApplicationException("State unknown");
      }
    }

    public void MarkForDelete(object entity)
    {
      switch (this.GetState(entity).SynchronizationOperation)
      {
        case SynchronizationOperation.Unknown:
          throw new StateException();
        case SynchronizationOperation.Nothing:
        case SynchronizationOperation.Updated:
          this.SetState(entity, SynchronizationOperation.Deleted);
          break;
        case SynchronizationOperation.Inserted:
          this.SetState(entity, SynchronizationOperation.Unknown);
          this.RemoveEntity(entity);
          break;
        case SynchronizationOperation.Deleted:
          break;
        default:
          throw new ApplicationException("State unknown");
      }
    }

    public ChangeTrackingState GetState(object entity)
    {
      if (this._stateLink.ContainsKey(entity))
        return this._stateLink[entity];
      return new ChangeTrackingState(SynchronizationOperation.Unknown);
    }

    private void SetState(object entity, SynchronizationOperation synchronizationOperation)
    {
      ChangeTrackingState e = this.GetChangeTrackingStateByEntity(entity);
      e.SynchronizationOperation = synchronizationOperation;
      entity.GetFieldValues<EntitySync>().ToList<EntitySync>().ForEach((Action<EntitySync>) (cc => cc.WillBeInDB = e.WillBeInDB));
    }

    private ChangeTrackingState GetChangeTrackingStateByEntity(object entity)
    {
      if (!this._stateLink.ContainsKey(entity))
        throw new ApplicationException("Entity not found");
      return this._stateLink[entity];
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this._stateLink.Where<KeyValuePair<object, ChangeTrackingState>>((Func<KeyValuePair<object, ChangeTrackingState>, bool>) (csm => csm.Value.SynchronizationOperation != SynchronizationOperation.Deleted)).Select<KeyValuePair<object, ChangeTrackingState>, object>((Func<KeyValuePair<object, ChangeTrackingState>, object>) (csm => csm.Key)).GetEnumerator();
    }

    public IEnumerable<KeyValuePair<SynchronizationOperation, object>> GetEntitiesWithState()
    {
      return this._stateLink.Keys.Where<object>((Func<object, bool>) (ent =>
      {
        if (this._stateLink[ent].SynchronizationOperation != SynchronizationOperation.Inserted && this._stateLink[ent].SynchronizationOperation != SynchronizationOperation.Updated)
          return this._stateLink[ent].SynchronizationOperation == SynchronizationOperation.Deleted;
        return true;
      })).Select<object, KeyValuePair<SynchronizationOperation, object>>((Func<object, KeyValuePair<SynchronizationOperation, object>>) (ent => new KeyValuePair<SynchronizationOperation, object>(this._stateLink[ent].SynchronizationOperation, ent)));
    }

    public void ResetEntitiesState()
    {
      this._stateLink = this._stateLink.Where<KeyValuePair<object, ChangeTrackingState>>((Func<KeyValuePair<object, ChangeTrackingState>, bool>) (kvp => kvp.Value.WillBeInDB)).Select<KeyValuePair<object, ChangeTrackingState>, object>((Func<KeyValuePair<object, ChangeTrackingState>, object>) (kvp => kvp.Key)).ToDictionary<object, object, ChangeTrackingState>((Func<object, object>) (entity => entity), (Func<object, ChangeTrackingState>) (entity => new ChangeTrackingState(SynchronizationOperation.Nothing)));
      //this._relationshipManager.Reload(this._stateLink);
    }
  }
}
