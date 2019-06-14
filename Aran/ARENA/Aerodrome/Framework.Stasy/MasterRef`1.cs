// Decompiled with JetBrains decompiler
// Type: Technewlogic.Stasy.MasterRef`1
// Assembly: Technewlogic.Stasy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B911826E-2508-4686-A374-AE5BC883E603
// Assembly location: C:\Users\AliguluR\Desktop\CrudGenerator_src\Dependencies\Technewlogic.Stasy-1\Technewlogic.Stasy.dll

using Framework.Stasy.Core;

namespace Framework.Stasy
{
  public class MasterRef<T> : OneBase<T> where T : class
  {
    protected override void SetStrategy()
    {
      this._strategy = (IMarkStrategy) new DummyStrategy();
    }

    protected override bool MasterIsChild()
    {
      return true;
    }

    public T Value
    {
      get
      {
        return this.GetValue();
      }
    }
  }
}
