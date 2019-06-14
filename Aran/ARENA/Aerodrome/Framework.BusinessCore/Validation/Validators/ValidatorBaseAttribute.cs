// Decompiled with JetBrains decompiler
// Type: Technewlogic.BusinessCore.Validation.Validators.ValidatorBaseAttribute
// Assembly: Technewlogic.BusinessCore, Version=1.0.3255.25915, Culture=neutral, PublicKeyToken=null
// MVID: D639CF35-4181-4DA8-A0AB-2D749BB47D70
// Assembly location: C:\Users\AliguluR\Desktop\CrudGenerator_src\Dependencies\Technewlogic.BusinessCore-1\Technewlogic.BusinessCore.dll

using System;

namespace BusinessCore.Validation.Validators
{
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
  public abstract class ValidatorBaseAttribute : Attribute
  {
    public abstract bool Validate();

    protected abstract void SetValidationMessage();

    public string ValidationMessage { get; protected set; }

    public object PropertyValue { get; set; }
  }
}
