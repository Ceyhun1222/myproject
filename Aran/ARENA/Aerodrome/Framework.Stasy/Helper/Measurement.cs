// Decompiled with JetBrains decompiler
// Type: Technewlogic.Stasy.Helper.Measurement
// Assembly: Technewlogic.Stasy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B911826E-2508-4686-A374-AE5BC883E603
// Assembly location: C:\Users\AliguluR\Desktop\CrudGenerator_src\Dependencies\Technewlogic.Stasy-1\Technewlogic.Stasy.dll

using System;

namespace Framework.Stasy.Helper
{
  internal static class Measurement
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
