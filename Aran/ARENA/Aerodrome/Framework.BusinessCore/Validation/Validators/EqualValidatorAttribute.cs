// Decompiled with JetBrains decompiler
// Type: Technewlogic.BusinessCore.Validation.Validators.EqualValidatorAttribute
// Assembly: Technewlogic.BusinessCore, Version=1.0.3255.25915, Culture=neutral, PublicKeyToken=null
// MVID: D639CF35-4181-4DA8-A0AB-2D749BB47D70
// Assembly location: C:\Users\AliguluR\Desktop\CrudGenerator_src\Dependencies\Technewlogic.BusinessCore-1\Technewlogic.BusinessCore.dll

namespace BusinessCore.Validation.Validators
{
  public class EqualValidatorAttribute : ValidatorBaseAttribute
  {
    public object Value { get; set; }

    public EqualValidatorAttribute(object Value)
    {
      this.Value = Value;
    }

    public override bool Validate()
    {
      return this.Value.Equals(this.PropertyValue);
    }

    protected override void SetValidationMessage()
    {
      this.ValidationMessage = "Der Wert muss gleich '" + this.Value.ToString() + "' sein";
    }
  }
}
