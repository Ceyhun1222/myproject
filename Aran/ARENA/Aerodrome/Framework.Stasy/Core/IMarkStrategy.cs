// Decompiled with JetBrains decompiler
// Type: Technewlogic.Stasy.Framework.IMarkStrategy
// Assembly: Technewlogic.Stasy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B911826E-2508-4686-A374-AE5BC883E603
// Assembly location: C:\Users\AliguluR\Desktop\CrudGenerator_src\Dependencies\Technewlogic.Stasy-1\Technewlogic.Stasy.dll

namespace Framework.Stasy.Core
{
  internal interface IMarkStrategy
  {
    void Add(object child, bool willBeInDB);

    void Remove(object child, bool willBeInDB);

    void StateChange(object child, bool willBeInDB);
  }
}
