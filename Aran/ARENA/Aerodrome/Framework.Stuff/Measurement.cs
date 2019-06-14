// Decompiled with JetBrains decompiler
// Type: Technewlogic.Stuff.Measurement
// Assembly: Technewlogic.Stuff, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BFC979D2-FC0B-46A5-8813-25E741038F56
// Assembly location: C:\Users\AliguluR\Desktop\CrudGenerator_src\Dependencies\Technewlogic.Stuff-1\Technewlogic.Stuff.dll

using System;

namespace Framework.Stuff
{
  public static class Measurement
  {
    private static DateTime _startTime = DateTime.Now;

    public static void Start()
    {
      Measurement._startTime = DateTime.Now;
    }

    public static TimeSpan Stop(string debugMessage)
    {
      TimeSpan timeSpan = DateTime.Now.Subtract(Measurement._startTime);
      Measurement._startTime = DateTime.Now;
      return timeSpan;
    }
  }
}
