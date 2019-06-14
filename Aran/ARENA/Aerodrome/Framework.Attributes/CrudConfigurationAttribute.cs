using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Attributes
{
    [global::System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class CrudPropertyConfigurationAttribute : Attribute, ICrudPropertyMetadata
    {
        public CrudPropertyConfigurationAttribute()
        {
            _metadata = new CrudPropertyMetadata();
        }

        public CrudPropertyConfigurationAttribute(bool initialState)
        {
            _metadata = new CrudPropertyMetadata(initialState);
        }

        public CrudPropertyConfigurationAttribute(string displayName)
        {
            _metadata = new CrudPropertyMetadata(displayName);
        }

        public CrudPropertyConfigurationAttribute(bool initialState, string displayName)
        {
            _metadata = new CrudPropertyMetadata(initialState, displayName);
        }

        private readonly CrudPropertyMetadata _metadata;

        public bool DisplayInGrid
        {
            get { return _metadata.DisplayInGrid; }
            set { _metadata.DisplayInGrid = value; }
        }

        public bool DisplayInDetails
        {
            get { return _metadata.DisplayInDetails; }
            set { _metadata.DisplayInDetails = value; }
        }

        public bool DisplayInModification
        {
            get { return _metadata.DisplayInModification; }
            set { _metadata.DisplayInModification = value; }
        }

        public bool IsEnabledInNew
        {
            get { return _metadata.IsEnabledInNew; }
            set { _metadata.IsEnabledInNew = value; }
        }

        public bool IsEnabledInEdit
        {
            get { return _metadata.IsEnabledInEdit; }
            set { _metadata.IsEnabledInEdit = value; }
        }

        public PropertyCategories PropertyCategory
        {
            get { return _metadata.PropertyCategory; }
            set { _metadata.PropertyCategory = value; }
        }

        public string[] SetterPropertyNames
        {
            get { return _metadata.SetterPropertyNames; }
            set { _metadata.SetterPropertyNames = value; }
        }

        public PropertyRequirements PropertyRequirement
        {
            get { return _metadata.PropertyRequirement; }
            set { _metadata.PropertyRequirement = value; }
        }

public string GetDisplayName(PropertyInfo pi)
        {
            return _metadata.GetDisplayName(pi);
        }

        public static CrudPropertyConfigurationAttribute TryGetCrudConfigurationAttribute(
            PropertyInfo pi)
        {
            return pi.GetCustomAttributes(typeof(CrudPropertyConfigurationAttribute), false)
                .Cast<CrudPropertyConfigurationAttribute>()
                .FirstOrDefault();
        }
    }

}
