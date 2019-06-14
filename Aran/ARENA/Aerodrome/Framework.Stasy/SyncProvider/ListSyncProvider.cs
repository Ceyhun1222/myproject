// Decompiled with JetBrains decompiler
// Type: Technewlogic.Stasy.SyncProvider.ListSyncProvider
// Assembly: Technewlogic.Stasy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B911826E-2508-4686-A374-AE5BC883E603
// Assembly location: C:\Users\AliguluR\Desktop\CrudGenerator_src\Dependencies\Technewlogic.Stasy-1\Technewlogic.Stasy.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Framework.Stasy.SyncProvider
{
  public class ListSyncProvider : ISyncProvider
  {
    private readonly Dictionary<Type, IList> _entityLists = new Dictionary<Type, IList>();
    private readonly string _path;

    public ListSyncProvider(string path)
    {
      this._path = path;
    }

    public bool IsDummy { get; set; }

    private IList GetInternalList(Type t)
    {
      if (!this._entityLists.ContainsKey(t))
      {
        string fileName = this.GetFileName(t);
        IList list = !Directory.Exists(this._path) || !File.Exists(fileName) ? (IList) new ArrayList() : new ListSerializer().Deserialize(File.ReadAllText(fileName), t);
        this._entityLists.Add(t, list);
      }
      return this._entityLists[t];
    }

    private string GetFileName(Type t)
    {
      return Path.GetFullPath(this._path) + "\\" + t.Name + ".obj";
    }

    public IEnumerable<T> GetEntityList<T>() where T : class
    {
      return this.GetInternalList(typeof (T)).Cast<T>();
    }

    public void Begin()
    {
    }

    public void Commit()
    {
      if (this.IsDummy)
        return;
      if (!Directory.Exists(this._path))
        Directory.CreateDirectory(this._path);
      foreach (KeyValuePair<Type, IList> entityList in this._entityLists)
      {
        ListSerializer listSerializer = new ListSerializer();
        //StringWriter stringWriter = new StringWriter();
        string json = listSerializer.Serialize( (IEnumerable) entityList.Value, entityList.Key);
        File.WriteAllText(this.GetFileName(entityList.Key), json);
      }
    }

    public void Update(IEnumerable entities)
    {
    }

    public void Delete(IEnumerable entities)
    {
      foreach (object entity in entities)
        this.GetInternalList(entity.GetType()).Remove(entity);
    }

    public void Insert(IEnumerable entities)
    {
      foreach (object entity in entities)
        this.GetInternalList(entity.GetType()).Add(entity);
    }
  }
}
