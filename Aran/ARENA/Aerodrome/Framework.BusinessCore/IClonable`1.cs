// Decompiled with JetBrains decompiler
// Type: Technewlogic.BusinessCore.IClonable`1
// Assembly: Technewlogic.BusinessCore, Version=1.0.3255.25915, Culture=neutral, PublicKeyToken=null
// MVID: D639CF35-4181-4DA8-A0AB-2D749BB47D70
// Assembly location: C:\Users\AliguluR\Desktop\CrudGenerator_src\Dependencies\Technewlogic.BusinessCore-1\Technewlogic.BusinessCore.dll

namespace BusinessCore
{
  internal interface IClonable<T> where T : new()
  {
    T CloneInstance();

    void CloneInstance(T target);
  }
}
