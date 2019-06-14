// Decompiled with JetBrains decompiler
// Type: Technewlogic.Stuff.EventArgs`1
// Assembly: Technewlogic.Stuff, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BFC979D2-FC0B-46A5-8813-25E741038F56
// Assembly location: C:\Users\AliguluR\Desktop\CrudGenerator_src\Dependencies\Technewlogic.Stuff-1\Technewlogic.Stuff.dll

using System;

namespace Framework.Stuff
{
  public class EventArgs<T> : EventArgs
  {
    public T Property { get; set; }

    public EventArgs(T property)
    {
      this.Property = property;
    }
  }
}
