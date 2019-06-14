// Decompiled with JetBrains decompiler
// Type: Technewlogic.Stasy.SyncProvider.ISyncProvider
// Assembly: Technewlogic.Stasy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B911826E-2508-4686-A374-AE5BC883E603
// Assembly location: C:\Users\AliguluR\Desktop\CrudGenerator_src\Dependencies\Technewlogic.Stasy-1\Technewlogic.Stasy.dll

using System.Collections;
using System.Collections.Generic;

namespace Framework.Stasy.SyncProvider
{
  public interface ISyncProvider
  {
    void Begin();

    void Commit();

    void Insert(IEnumerable entities);

    void Update(IEnumerable entities);

    void Delete(IEnumerable entities);

    IEnumerable<T> GetEntityList<T>() where T : class;
  }
}
