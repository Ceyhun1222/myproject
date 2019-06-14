// Decompiled with JetBrains decompiler
// Type: Technewlogic.Stasy.Framework.OneBase`1
// Assembly: Technewlogic.Stasy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B911826E-2508-4686-A374-AE5BC883E603
// Assembly location: C:\Users\AliguluR\Desktop\CrudGenerator_src\Dependencies\Technewlogic.Stasy-1\Technewlogic.Stasy.dll

using System;

namespace Framework.Stasy.Core
{
  public abstract class OneBase<T> : OneBase where T : class
  {
    protected override Type GetTargetType()
    {
      return typeof (T);
    }

    internal T GetValue()
    {
      return (T) base.GetValue();
    }

    internal void SetValue(T value)
    {
      this.SetValue((object) value);
    }

    public event EventHandler<EntityChangingEventArgs<T>> BeforeValueSet;

    protected override void OnBeforeValueSet(object value)
    {
      EntityChangingEventArgs<T> e = new EntityChangingEventArgs<T>((T) value);
      if (this.BeforeValueSet == null)
        return;
      this.BeforeValueSet((object) this, e);
    }
  }
}
