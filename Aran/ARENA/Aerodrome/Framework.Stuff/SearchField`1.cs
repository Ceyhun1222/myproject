// Decompiled with JetBrains decompiler
// Type: Technewlogic.Stuff.SearchField`1
// Assembly: Technewlogic.Stuff, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BFC979D2-FC0B-46A5-8813-25E741038F56
// Assembly location: C:\Users\AliguluR\Desktop\CrudGenerator_src\Dependencies\Technewlogic.Stuff-1\Technewlogic.Stuff.dll

using System;

namespace Framework.Stuff
{
  internal class SearchField<T>
  {
    public Func<T, string> GetProperty { get; private set; }

    public bool SearchZeroIndex { get; private set; }

    public SearchField(Func<T, string> getProperty, bool searchZeroIndex)
    {
      this.GetProperty = getProperty;
      this.SearchZeroIndex = searchZeroIndex;
    }
  }
}
