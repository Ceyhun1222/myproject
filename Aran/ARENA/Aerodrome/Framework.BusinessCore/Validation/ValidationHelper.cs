// Decompiled with JetBrains decompiler
// Type: Technewlogic.BusinessCore.Validation.ValidationHelper
// Assembly: Technewlogic.BusinessCore, Version=1.0.3255.25915, Culture=neutral, PublicKeyToken=null
// MVID: D639CF35-4181-4DA8-A0AB-2D749BB47D70
// Assembly location: C:\Users\AliguluR\Desktop\CrudGenerator_src\Dependencies\Technewlogic.BusinessCore-1\Technewlogic.BusinessCore.dll

using System.Collections.Generic;
using BusinessCore.Validation.Exceptions;

namespace BusinessCore.Validation
{
  public class ValidationHelper
  {
    private List<ValidationRule> _rules = new List<ValidationRule>();
    private List<ValidationRule> _dynamicRules = new List<ValidationRule>();

    public void AddRule(ValidationRule Rule)
    {
      this._rules.Add(Rule);
    }

    public void AddDynamicRule(ValidationRule Rule)
    {
      this._dynamicRules.Add(Rule);
    }

    public void RemoveDynamicRules()
    {
      this._dynamicRules.Clear();
    }

    public List<string> BrokenRulesMessages
    {
      get
      {
        List<ValidationRule> validationRuleList = this.JoinRules();
        List<string> stringList = new List<string>();
        foreach (ValidationRule validationRule in validationRuleList)
        {
          if (!validationRule.IsValid)
            stringList.Add(validationRule.Exception.Message);
        }
        if (validationRuleList.Count == 0)
          return (List<string>) null;
        return stringList;
      }
    }

    public List<ValidationRule> BrokenRules
    {
      get
      {
        List<ValidationRule> validationRuleList1 = this.JoinRules();
        List<ValidationRule> validationRuleList2 = new List<ValidationRule>();
        foreach (ValidationRule validationRule in validationRuleList1)
        {
          if (!validationRule.IsValid)
            validationRuleList2.Add(validationRule);
        }
        return validationRuleList2;
      }
    }

    public List<ValidationException> BrokenRulesExceptions
    {
      get
      {
        List<ValidationRule> validationRuleList = this.JoinRules();
        List<ValidationException> validationExceptionList = new List<ValidationException>();
        foreach (ValidationRule validationRule in validationRuleList)
        {
          if (!validationRule.IsValid)
            validationExceptionList.Add(validationRule.Exception);
        }
        return validationExceptionList;
      }
    }

    public string ValidationMessage
    {
      get
      {
        if (this.IsValid)
          return string.Empty;
        string str = string.Empty;
        foreach (string brokenRulesMessage in this.BrokenRulesMessages)
          str = str + "\r\n" + brokenRulesMessage;
        return str;
      }
    }

    private List<ValidationRule> JoinRules()
    {
      List<ValidationRule> validationRuleList = new List<ValidationRule>();
      validationRuleList.AddRange((IEnumerable<ValidationRule>) this._rules);
      validationRuleList.AddRange((IEnumerable<ValidationRule>) this._dynamicRules);
      return validationRuleList;
    }

    public bool IsValid
    {
      get
      {
        foreach (ValidationRule rule in this._rules)
        {
          if (!rule.IsValid)
            return false;
        }
        return true;
      }
    }
  }
}
