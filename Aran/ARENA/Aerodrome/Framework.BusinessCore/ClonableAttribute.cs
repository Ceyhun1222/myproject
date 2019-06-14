// Decompiled with JetBrains decompiler
// Type: Technewlogic.BusinessCore.ClonableAttribute
// Assembly: Technewlogic.BusinessCore, Version=1.0.3255.25915, Culture=neutral, PublicKeyToken=null
// MVID: D639CF35-4181-4DA8-A0AB-2D749BB47D70
// Assembly location: C:\Users\AliguluR\Desktop\CrudGenerator_src\Dependencies\Technewlogic.BusinessCore-1\Technewlogic.BusinessCore.dll

using System;

namespace BusinessCore
{
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
  public sealed class ClonableAttribute : Attribute
  {
    public bool TryCloneReference { get; set; }
  }
}
