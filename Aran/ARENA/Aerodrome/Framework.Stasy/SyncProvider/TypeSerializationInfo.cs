// Decompiled with JetBrains decompiler
// Type: Technewlogic.Stasy.SyncProvider.TypeSerializationInfo
// Assembly: Technewlogic.Stasy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B911826E-2508-4686-A374-AE5BC883E603
// Assembly location: C:\Users\AliguluR\Desktop\CrudGenerator_src\Dependencies\Technewlogic.Stasy-1\Technewlogic.Stasy.dll

using System;
using System.Collections.Generic;

namespace Framework.Stasy.SyncProvider
{
  public class TypeSerializationInfo
  {
    public TypeSerializationInfo()
    {
    }

    public TypeSerializationInfo(Type type)
    {
      this.TypeName = type.FullName;
      this.FielValueList = new List<FieldSerializationInfo>();
    }

    public string TypeName { get; set; }
        private List<FieldSerializationInfo> _fieldValueList;
    public List<FieldSerializationInfo> FielValueList
        {
            get
            {
                return _fieldValueList;
            }
            set
            {
                _fieldValueList = value;
            }
        }
  }
}
