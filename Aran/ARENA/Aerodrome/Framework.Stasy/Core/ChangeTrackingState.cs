// Decompiled with JetBrains decompiler
// Type: Technewlogic.Stasy.Framework.ChangeTrackingState
// Assembly: Technewlogic.Stasy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B911826E-2508-4686-A374-AE5BC883E603
// Assembly location: C:\Users\AliguluR\Desktop\CrudGenerator_src\Dependencies\Technewlogic.Stasy-1\Technewlogic.Stasy.dll

using System;

namespace Framework.Stasy.Core
{
  public class ChangeTrackingState
  {
    private SynchronizationOperation _synchronizationOperation;

    public bool IsCurrentlyInDB
    {
      get
      {
        if (this.SynchronizationOperation != SynchronizationOperation.Nothing && this.SynchronizationOperation != SynchronizationOperation.Deleted)
          return this.SynchronizationOperation == SynchronizationOperation.Updated;
        return true;
      }
    }

    public bool WillBeInDB
    {
      get
      {
        if (this.SynchronizationOperation != SynchronizationOperation.Inserted && this.SynchronizationOperation != SynchronizationOperation.Updated)
          return this.SynchronizationOperation == SynchronizationOperation.Nothing;
        return true;
      }
    }

    public SynchronizationOperation SynchronizationOperation
    {
      get
      {
        return this._synchronizationOperation;
      }
      set
      {
        this._synchronizationOperation = value;
      }
    }

    public ChangeTrackingState(SynchronizationOperation initialOperation)
    {
      switch (initialOperation)
      {
        case SynchronizationOperation.Unknown:
        case SynchronizationOperation.Nothing:
        case SynchronizationOperation.Inserted:
          this._synchronizationOperation = initialOperation;
          break;
        default:
          throw new ArgumentException("The initial operation has to be Unknown, Inserted or Nothing", nameof (initialOperation));
      }
    }
  }
}
