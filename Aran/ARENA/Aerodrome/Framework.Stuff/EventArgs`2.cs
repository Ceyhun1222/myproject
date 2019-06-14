// Decompiled with JetBrains decompiler
// Type: Technewlogic.Stuff.EventArgs`2
// Assembly: Technewlogic.Stuff, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BFC979D2-FC0B-46A5-8813-25E741038F56
// Assembly location: C:\Users\AliguluR\Desktop\CrudGenerator_src\Dependencies\Technewlogic.Stuff-1\Technewlogic.Stuff.dll

using System;

namespace Framework.Stuff
{
  public class EventArgs<T1, T2> : EventArgs
  {
    public T1 Property1 { get; set; }

    public T2 Property2 { get; set; }

    public EventArgs(T1 property1, T2 property2)
    {
      this.Property1 = property1;
      this.Property2 = property2;
    }
  }
}
