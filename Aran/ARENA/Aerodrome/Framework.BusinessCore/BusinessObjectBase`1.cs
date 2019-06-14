// Decompiled with JetBrains decompiler
// Type: Technewlogic.BusinessCore.BusinessObjectBase`1
// Assembly: Technewlogic.BusinessCore, Version=1.0.3255.25915, Culture=neutral, PublicKeyToken=null
// MVID: D639CF35-4181-4DA8-A0AB-2D749BB47D70
// Assembly location: C:\Users\AliguluR\Desktop\CrudGenerator_src\Dependencies\Technewlogic.BusinessCore-1\Technewlogic.BusinessCore.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using BusinessCore.Validation;
using BusinessCore.Validation.Validators;

namespace BusinessCore
{
    public abstract class BusinessObjectBase<T> : INotifyPropertyChanged, IValidatable, IDescriptor, IComparable, IClonable<T> where T : BusinessObjectBase<T>, new()
    {
        private string _previousValidationMessage = string.Empty;
        private readonly PropertyInfo _entityKeyProperty;

        public static string DefaultEntityKeyName { get; set; }

        static BusinessObjectBase()
        {
            BusinessObjectBase<T>.DefaultEntityKeyName = "featureID";
        }

        protected string EntityKeyName { get; private set; }

        public ValidationHelper ValidationHelper { get; private set; }

        public BusinessObjectBase(string entityKeyName)
        {
            this.ValidationHelper = new ValidationHelper();
            this.EntityKeyName = entityKeyName;
            IEnumerable<PropertyInfo> source = ((IEnumerable<PropertyInfo>)this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)).Where<PropertyInfo>((Func<PropertyInfo, bool>)(pi => pi.Name == entityKeyName));
            if (source.Count<PropertyInfo>() == 0)
                throw new ArgumentException("The specified entity key name '" + entityKeyName + "' is not a member of the type.", nameof(entityKeyName));
            this._entityKeyProperty = source.First<PropertyInfo>();
        }

        public BusinessObjectBase()
          : this(BusinessObjectBase<T>.DefaultEntityKeyName)
        {
        }

        public virtual T CloneInstance()
        {
            T instance = Activator.CreateInstance<T>();
            this.CloneInstance(instance);
            return instance;
        }

        public virtual void CloneInstance(T target)
        {
            foreach (PropertyInfo propertyInfo in ((IEnumerable<PropertyInfo>)target.GetType().GetProperties()).Where<PropertyInfo>((Func<PropertyInfo, bool>)(pi => pi.GetSetMethod(true) != null)))
            {
                object[] customAttributes = propertyInfo.GetCustomAttributes(typeof(ClonableAttribute), true);
                if (((IEnumerable<object>)customAttributes).Count<object>() > 0)
                {
                    ClonableAttribute clonableAttribute = (ClonableAttribute)((IEnumerable<object>)customAttributes).Single<object>();
                    object obj = propertyInfo.GetValue((object)this, (object[])null);
                    if (obj is IClonable<T> && clonableAttribute.TryCloneReference)
                    {
                        IClonable<T> clonable = obj as IClonable<T>;
                        propertyInfo.SetValue((object)target, (object)clonable.CloneInstance(), (object[])null);
                    }
                    else
                        propertyInfo.SetValue((object)target, obj, (object[])null);
                }
            }
        }

        public List<string> BrokenRules
        {
            get
            {
                this._previousValidationMessage = this.ValidationHelper.ValidationMessage;
                return this.ValidationHelper.BrokenRulesMessages;
            }
        }

        [Browsable(false)]
        public string ValidationMessage
        {
            get
            {
                string validationMessage = this.ValidationHelper.ValidationMessage;
                this._previousValidationMessage = validationMessage;
                return validationMessage;
            }
        }

        protected void AddRule(ValidationRule Rule)
        {
            this.ValidationHelper.AddRule(Rule);
        }

        public bool Validate()
        {
            this.ValidationHelper.RemoveDynamicRules();
            this.AddDynamicRules();
            if (this.ValidationHelper.ValidationMessage != this._previousValidationMessage)
                this.SendPropertyChanged("BrokenRules");
            return this.ValidationHelper.IsValid;
        }

        protected virtual void ThrowValidationFailedException(ValidationRule rule)
        {
            if (!rule.IsValid)
                throw rule.Exception;
        }

        private void AddDynamicRules()
        {
            foreach (PropertyInfo property in this.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.GetProperty))
            {
                foreach (Attribute customAttribute in Attribute.GetCustomAttributes((MemberInfo)property, typeof(ValidatorBaseAttribute)))
                {
                    ValidatorBaseAttribute validatorBaseAttribute = customAttribute as ValidatorBaseAttribute;
                    if (validatorBaseAttribute != null)
                        validatorBaseAttribute.PropertyValue = property.GetValue((object)this, (object[])null);
                }
            }
        }

        protected bool ValidateStringNotEmpty(string s)
        {
            if (s == null)
                return false;
            return s.Trim() != string.Empty;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void SendPropertyChanged([CallerMemberName]string propertyName=null)
        {
            if (this.PropertyChanged == null)
                return;
            this.PropertyChanged((object)this, new PropertyChangedEventArgs(propertyName));
        }

        [Browsable(false)]
        public abstract string Descriptor { get; }

        public virtual int CompareTo(object obj)
        {
            return this.Descriptor.CompareTo(obj.ToString());
        }

        public override bool Equals(object obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
                return false;
            object obj1 = this._entityKeyProperty.GetValue((object)this, (object[])null);
            object obj2 = this._entityKeyProperty.GetValue(obj, (object[])null);
            if (obj1 != null)
                return obj1.Equals(obj2);
            return obj2 == null;
        }

        public override int GetHashCode()
        {
            return this._entityKeyProperty.GetValue((object)this, (object[])null).GetHashCode();
        }

        public override string ToString()
        {
            return this.Descriptor;
        }
    }
}
