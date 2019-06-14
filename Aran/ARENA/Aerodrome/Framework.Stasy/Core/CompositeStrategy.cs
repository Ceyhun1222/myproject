// Decompiled with JetBrains decompiler
// Type: Technewlogic.Stasy.Framework.CompositeStrategy
// Assembly: Technewlogic.Stasy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B911826E-2508-4686-A374-AE5BC883E603
// Assembly location: C:\Users\AliguluR\Desktop\CrudGenerator_src\Dependencies\Technewlogic.Stasy-1\Technewlogic.Stasy.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Framework.Stuff.Extensions;

namespace Framework.Stasy.Core
{
  public class CompositeStrategy : IMarkStrategy
  {
    private EntityCollection _entityCollection;
    private object _master;
    private KeyManager _keyManager;

    public CompositeStrategy(EntityCollection entityCollection, object master)
    {
      this._entityCollection = entityCollection;
      this._master = master;
      this._keyManager = new KeyManager();
    }

    public void StateChange(object child, bool willBeInDB)
    {
      ChangeTrackingState state = this._entityCollection.GetState(child);
      if (willBeInDB)
      {
        if (state.WillBeInDB)
          this._entityCollection.MarkForUpdate(child);
        else
          this._entityCollection.MarkForInsert(child);
      }
      else
      {
        if (!state.WillBeInDB)
          return;
        this._entityCollection.MarkForDelete(child);
      }
    }

    public void Add(object child, bool willBeInDB)
    {
      if (this._master != null)
      {
        this.RemoveChildFromFormerParent(child);
        this._keyManager.SetForeignKeyValue(child, this._keyManager.GetPrimaryKeyValue(this._master), this._master.GetType());
        this.SetParentRef(child);
      }
      ChangeTrackingState state = this._entityCollection.GetState(child);
      if (willBeInDB)
      {
        if (state.WillBeInDB)
          this._entityCollection.MarkForUpdate(child);
        else
          this._entityCollection.MarkForInsert(child);
      }
      else
      {
        if (!state.WillBeInDB)
          return;
        this._entityCollection.MarkForDelete(child);
      }
    }

    public void Remove(object child, bool willBeInDB)
    {
      if (willBeInDB && this._entityCollection.GetState(child).WillBeInDB)
        this._entityCollection.MarkForDelete(child);
      if (this._master == null)
        return;
      this._keyManager.ResetForeignKey(child, this._master.GetType());
      this.ResetParentRef(child);
    }

    private void RemoveChildFromFormerParent(object child)
    {
      object me = this.GetMasterRefField(child).GetValue();
      if (me == null)
        return;
      IEnumerable<object> source = me.GetFieldValues(typeof (CompositeCollection<>)).Cast<object>().Where<object>((Func<object, bool>) (cc => ((IEnumerable<Type>) cc.GetType().GetGenericArguments()).First<Type>() == child.GetType()));
      if (source.Count<object>() == 0)
        throw new ApplicationException("There was no child collection found in the parent");
      if (source.Count<object>() > 1)
        throw new ApplicationException("There was more than 1 child collection found in the parent");
      object obj = source.Single<object>();
      obj.GetType().GetMethod("RemoveForRelocate", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(obj, new object[1]
      {
        child
      });
    }

    private void SetParentRef(object child)
    {
      this.SetParentRefValue(child, this._master);
    }

    private void ResetParentRef(object child)
    {
      this.SetParentRefValue(child, (object) null);
    }

    private void SetParentRefValue(object child, object value)
    {
      this.GetMasterRefField(child).SetValue(value);
    }

    private OneBase GetMasterRefField(object child)
    {
      IEnumerable<FieldInfo> source = child.GetFieldsOfType(typeof (MasterRef<>), true).Where<FieldInfo>((Func<FieldInfo, bool>) (it => ((IEnumerable<Type>) it.FieldType.GetGenericArguments()).FirstOrDefault<Type>() == this._master.GetType()));
      if (source.Count<FieldInfo>() == 0)
        throw new ApplicationException("There was no parent reference field found");
      if (source.Count<FieldInfo>() > 1)
        throw new ApplicationException("There was more than 1 parent reference field found");
      return (OneBase) source.Single<FieldInfo>().GetValue(child);
    }
  }
}
