// Decompiled with JetBrains decompiler
// Type: Technewlogic.Stasy.StateException
// Assembly: Technewlogic.Stasy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B911826E-2508-4686-A374-AE5BC883E603
// Assembly location: C:\Users\AliguluR\Desktop\CrudGenerator_src\Dependencies\Technewlogic.Stasy-1\Technewlogic.Stasy.dll

using System;
using System.Runtime.Serialization;

namespace Framework.Stasy
{
  [Serializable]
  public class StateException : ApplicationException
  {
    public StateException()
    {
    }

    public StateException(string message)
      : base(message)
    {
    }

    public StateException(string message, Exception inner)
      : base(message, inner)
    {
    }

    protected StateException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
