// Decompiled with JetBrains decompiler
// Type: Technewlogic.Stuff.Extensions.DateTimeExtensions
// Assembly: Technewlogic.Stuff, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BFC979D2-FC0B-46A5-8813-25E741038F56
// Assembly location: C:\Users\AliguluR\Desktop\CrudGenerator_src\Dependencies\Technewlogic.Stuff-1\Technewlogic.Stuff.dll

using System;

namespace Framework.Stuff.Extensions
{
  public static class DateTimeExtensions
  {
    public static DateTime GetCalendarStart(this DateTime me)
    {
      me = me.Date.AddDays((double) -me.Day);
      me = me.AddDays((double) -(int) me.DayOfWeek).AddDays(1.0);
      return me;
    }

    public static int Week(this DateTime me)
    {
      int year = me.Year;
      int num1 = (int) me.DayOfWeek;
      if (num1 == 0)
        num1 = 7;
      me = me.AddDays((double) (7 - num1));
      int num2 = 0;
      for (; me.Year == year; me = me.AddDays(-7.0))
        ++num2;
      return num2;
    }

    public static DateTime GetPresentWeekDay(this DateTime me)
    {
      int dayOfWeek = (int) me.DayOfWeek;
      DateTime date = DateTime.Now.Date;
      return dayOfWeek != 0 ? me.AddDays((double) (1 - dayOfWeek)).Date : me.AddDays(-6.0).Date;
    }
  }
}
