using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Attributes
{
    public class CrudPropertyMetadata : ICrudPropertyMetadata
    {
        public CrudPropertyMetadata()
            : this(true) { }

        public CrudPropertyMetadata(bool initialState)
        {
            DisplayInGrid = initialState;
            DisplayInDetails = initialState;
            DisplayInModification = initialState;
            IsEnabledInEdit = initialState;
            IsEnabledInNew = initialState;          
        }

        public CrudPropertyMetadata(string displayName)
            : this(true, displayName) { }

        public CrudPropertyMetadata(bool initialState, string displayName)
            : this(initialState)
        {
            _displayName = displayName;
        }

        private readonly string _displayName = string.Empty;

        #region ICrudPropertyMetadata Members

        public bool DisplayInDetails { get; set; }

        public bool DisplayInGrid { get; set; }

        public bool DisplayInModification { get; set; }

        public bool IsEnabledInEdit { get; set; }

        public bool IsEnabledInNew { get; set; }

        public PropertyRequirements PropertyRequirement { get; set; }

        public PropertyCategories PropertyCategory { get; set; }
        public string[] SetterPropertyNames { get; set; }
        
        public string GetDisplayName(PropertyInfo pi)
        {
            return string.IsNullOrEmpty(_displayName)
                ? pi.Name
                : _displayName;
        }

        #endregion
    }
}
