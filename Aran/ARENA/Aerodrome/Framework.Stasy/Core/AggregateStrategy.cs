// Decompiled with JetBrains decompiler
// Type: Technewlogic.Stasy.Framework.AggregateStrategy
// Assembly: Technewlogic.Stasy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B911826E-2508-4686-A374-AE5BC883E603
// Assembly location: C:\Users\AliguluR\Desktop\CrudGenerator_src\Dependencies\Technewlogic.Stasy-1\Technewlogic.Stasy.dll

namespace Framework.Stasy.Core
{
  public class AggregateStrategy : IMarkStrategy
  {
    private EntityCollection _entityCollection;
    private object _master;
    private KeyManager _keyManager;

    public AggregateStrategy(EntityCollection entityCollection, object master)
    {
      this._entityCollection = entityCollection;
      this._master = master;
      this._keyManager = new KeyManager();
    }

    public void StateChange(object child, bool willBeInDB)
    {
    }

    public void Add(object child, bool willBeInDB)
    {
      this._keyManager.SetForeignKeyValue(this._master, this._keyManager.GetPrimaryKeyValue(child), child.GetType());
      ChangeTrackingState state = this._entityCollection.GetState(child);
      if (willBeInDB && !state.WillBeInDB)
        throw new StateException("Slave is not in DB");
    }

    public void Remove(object child, bool willBeInDB)
    {
      this._keyManager.ResetForeignKey(this._master, child.GetType());
    }
  }
}
