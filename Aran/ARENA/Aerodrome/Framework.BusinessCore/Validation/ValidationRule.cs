// Decompiled with JetBrains decompiler
// Type: Technewlogic.BusinessCore.Validation.ValidationRule
// Assembly: Technewlogic.BusinessCore, Version=1.0.3255.25915, Culture=neutral, PublicKeyToken=null
// MVID: D639CF35-4181-4DA8-A0AB-2D749BB47D70
// Assembly location: C:\Users\AliguluR\Desktop\CrudGenerator_src\Dependencies\Technewlogic.BusinessCore-1\Technewlogic.BusinessCore.dll

using System;
using BusinessCore.Validation.Exceptions;

namespace BusinessCore.Validation
{
  public class ValidationRule
  {
    private readonly Func<bool> _validationMethod;

    public ValidationException Exception { get; set; }

    public ValidationRule(ValidationException Exception, Func<bool> validationMethod)
    {
      this.Exception = Exception;
      this._validationMethod = validationMethod;
    }

    public bool IsValid
    {
      get
      {
        return this._validationMethod();
      }
    }
  }
}
