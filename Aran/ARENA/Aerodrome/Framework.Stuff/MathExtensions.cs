// Decompiled with JetBrains decompiler
// Type: Technewlogic.Stuff.MathExtensions
// Assembly: Technewlogic.Stuff, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BFC979D2-FC0B-46A5-8813-25E741038F56
// Assembly location: C:\Users\AliguluR\Desktop\CrudGenerator_src\Dependencies\Technewlogic.Stuff-1\Technewlogic.Stuff.dll

using System;

namespace Framework.Stuff
{
  public static class MathExtensions
  {
    public static double HalfCeiling(this double me)
    {
      double num = Math.Truncate(me);
      if (me == num || me == num + 0.5)
        return me;
      if (me - num >= 0.5)
        return num + 1.0;
      return num + 0.5;
    }
  }
}
