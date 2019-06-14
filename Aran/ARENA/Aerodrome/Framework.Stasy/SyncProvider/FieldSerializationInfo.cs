// Decompiled with JetBrains decompiler
// Type: Technewlogic.Stasy.SyncProvider.FieldSerializationInfo
// Assembly: Technewlogic.Stasy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B911826E-2508-4686-A374-AE5BC883E603
// Assembly location: C:\Users\AliguluR\Desktop\CrudGenerator_src\Dependencies\Technewlogic.Stasy-1\Technewlogic.Stasy.dll

namespace Framework.Stasy.SyncProvider
{
  public class FieldSerializationInfo
  {
    public FieldSerializationInfo()
    {
    }

    public FieldSerializationInfo(string fieldName, object value)
    {
      this.FieldName = fieldName;
      this.Value = value;
    }

    public string FieldName { get; set; }

    public object Value { get; set; }
  }
}
