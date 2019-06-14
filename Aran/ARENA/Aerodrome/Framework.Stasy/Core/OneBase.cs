// Decompiled with JetBrains decompiler
// Type: Technewlogic.Stasy.Framework.OneBase
// Assembly: Technewlogic.Stasy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B911826E-2508-4686-A374-AE5BC883E603
// Assembly location: C:\Users\AliguluR\Desktop\CrudGenerator_src\Dependencies\Technewlogic.Stasy-1\Technewlogic.Stasy.dll

using System;

namespace Framework.Stasy.Core
{
  public abstract class OneBase : EntitySync
  {
    private bool _isInitialized;
    private object _value;

    public override bool HasValue(object value)
    {
      return this.GetValue().Equals(value);
    }

    protected override void OnStateChange()
    {
      base.OnStateChange();
      if (this.GetValue() == null)
        return;
      this._strategy.StateChange(this.GetValue(), this.WillBeInDB);
    }

    private void Init()
    {
      this._value = this.MasterIsChild() ? this._entityCollection.GetByForeignKeyPropertyOfEntity(this._master) : this._entityCollection.GetByPrimaryKeyPropertyOfEntity(this._master);
      this._isInitialized = true;
    }

    protected abstract Type GetTargetType();

    protected abstract bool MasterIsChild();

    internal object GetValue()
    {
      if (!this._isInitialized)
        this.Init();
      return this._value;
    }

    internal void SetValue(object value)
    {
      if (!this._isInitialized)
        this.Init();
      this.OnBeforeValueSet(value);
      if (this.GetValue() != null)
        this._strategy.Remove(this.GetValue(), this.WillBeInDB);
      if (value != null)
      {
        this._entityContext.EnsureEntityIsAssigned(value);
        this._strategy.Add(value, this.WillBeInDB);
      }
      this._value = value;
    }

    protected abstract void OnBeforeValueSet(object value);
  }
}
