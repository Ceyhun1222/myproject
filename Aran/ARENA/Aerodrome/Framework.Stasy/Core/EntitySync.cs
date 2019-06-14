// Decompiled with JetBrains decompiler
// Type: Technewlogic.Stasy.Framework.EntitySync
// Assembly: Technewlogic.Stasy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B911826E-2508-4686-A374-AE5BC883E603
// Assembly location: C:\Users\AliguluR\Desktop\CrudGenerator_src\Dependencies\Technewlogic.Stasy-1\Technewlogic.Stasy.dll

namespace Framework.Stasy.Core
{
  public abstract class EntitySync
  {
    protected bool _willBeInDB;
    internal IMarkStrategy _strategy;
    internal EntityContext _entityContext;
    internal EntityCollection _entityCollection;
    protected object _master;

    internal bool WillBeInDB
    {
      get
      {
        return this._willBeInDB;
      }
      set
      {
        if (this._willBeInDB != value)
          this._willBeInDB = value;
        this.OnStateChange();
      }
    }

    protected virtual void OnStateChange()
    {
    }

    public abstract bool HasValue(object value);

    internal void SetEntityCollection(EntityContext entityContext, EntityCollection collection, bool willBeInDB, object master)
    {
      this._entityContext = entityContext;
      this._willBeInDB = willBeInDB;
      this._entityCollection = collection;
      this._master = master;
      this.SetStrategy();
    }

    protected abstract void SetStrategy();
  }
}
