// Decompiled with JetBrains decompiler
// Type: Technewlogic.Stasy.Framework.ManyBase`1
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
  public abstract class ManyBase<T> : EntitySync, IEnumerable<T>, IEnumerable where T : class
  {
    private bool _isInitialized;
    private List<T> _children;

    public override bool HasValue(object value)
    {
      if (value.GetType().IsType(typeof (T), false))
        return this.Children.Contains(value as T);
      return false;
    }

    protected override void OnStateChange()
    {
      base.OnStateChange();
      foreach (T child in this.Children)
        this._strategy.StateChange((object) child, this.WillBeInDB);
    }

    private void Init()
    {
      this._children = (this._master == null ? (IEnumerable) this._entityCollection : this._entityCollection.GetManyByPrimaryKeyPropertyOfEntity(this._master)).Cast<T>().ToList<T>();
      this._isInitialized = true;
    }

    public IEnumerator<T> GetEnumerator()
    {
      return (IEnumerator<T>) this.Children.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.Children.GetEnumerator();
    }

    protected List<T> Children
    {
      get
      {
        if (!this._isInitialized)
          this.Init();
        return this._children;
      }
    }

    protected void DoAdd(object child)
    {
      this.OnBeforeAdd((object) child);
      if (this.HasValue((object) child))
        throw new InvalidOperationException("Sequence already contains element");
      this._entityContext.EnsureEntityIsAssigned((object) child);
      this._strategy.Add((object) child, this.WillBeInDB);
      this.Children.Add((T)child);
    }

    protected void DoRemove(T child)
    {
      this.OnBeforeRemove((object) child);
      if (!this.HasValue((object) child))
        throw new InvalidOperationException("Sequence contains no element");
      this._entityContext.CheckRemovalDependencies((object) child);
      this._strategy.Remove((object) child, this.WillBeInDB);
      this.Children.Remove(child);
    }

    public event EventHandler<EntityChangingEventArgs<T>> BeforeAdd;

    protected void OnBeforeAdd(object entity)
    {
      EntityChangingEventArgs<T> e = new EntityChangingEventArgs<T>((T) entity);
      if (this.BeforeAdd == null)
        return;
      this.BeforeAdd((object) this, e);
    }

    public event EventHandler<EntityChangingEventArgs<T>> BeforeRemove;

    protected void OnBeforeRemove(object entity)
    {
      EntityChangingEventArgs<T> e = new EntityChangingEventArgs<T>((T) entity);
      if (this.BeforeRemove == null)
        return;
      this.BeforeRemove((object) this, e);
    }
  }
}
