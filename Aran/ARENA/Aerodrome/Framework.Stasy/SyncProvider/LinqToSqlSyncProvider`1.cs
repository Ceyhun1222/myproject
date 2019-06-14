// Decompiled with JetBrains decompiler
// Type: Technewlogic.Stasy.SyncProvider.LinqToSqlSyncProvider`1
// Assembly: Technewlogic.Stasy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B911826E-2508-4686-A374-AE5BC883E603
// Assembly location: C:\Users\AliguluR\Desktop\CrudGenerator_src\Dependencies\Technewlogic.Stasy-1\Technewlogic.Stasy.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;

namespace Framework.Stasy.SyncProvider
{
  public class LinqToSqlSyncProvider<T> : ISyncProvider where T : DataContext, new()
  {
    private readonly T _readContext;
    private T _writeContext;

    public LinqToSqlSyncProvider()
    {
      this._readContext = Activator.CreateInstance<T>();
      this._readContext.ObjectTrackingEnabled = false;
    }

    public void Begin()
    {
      this._writeContext = Activator.CreateInstance<T>();
    }

    public void Commit()
    {
      this._writeContext.SubmitChanges();
      this._writeContext.Dispose();
    }

    public void Insert(IEnumerable entities)
    {
      entities.Cast<object>().GroupBy<object, Type>((Func<object, Type>) (o => o.GetType())).ToList<IGrouping<Type, object>>().ForEach((Action<IGrouping<Type, object>>) (g => this.OnGetTable(g.Key).InsertAllOnSubmit((IEnumerable) g)));
    }

    public void Update(IEnumerable entities)
    {
      entities.Cast<object>().GroupBy<object, Type>((Func<object, Type>) (o => o.GetType())).ToList<IGrouping<Type, object>>().ForEach((Action<IGrouping<Type, object>>) (g => this.OnGetTable(g.Key).AttachAll((IEnumerable) g, true)));
    }

    public void Delete(IEnumerable entities)
    {
      entities.Cast<object>().GroupBy<object, Type>((Func<object, Type>) (o => o.GetType())).ToList<IGrouping<Type, object>>().ForEach((Action<IGrouping<Type, object>>) (g =>
      {
        ITable table = this.OnGetTable(g.Key);
        table.AttachAll((IEnumerable) g, true);
        table.DeleteAllOnSubmit((IEnumerable) g);
      }));
    }

    protected virtual ITable OnGetTable(Type t)
    {
      return this._writeContext.GetTable(t);
    }

    public virtual IEnumerable<TEntity> GetEntityList<TEntity>() where TEntity : class
    {
      if ((object) this._writeContext != null)
        throw new InvalidOperationException("Provider is locked");
      return (IEnumerable<TEntity>) this._readContext.GetTable<TEntity>();
    }
  }
}
