using Framework.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Stasy
{
    public class FeatureTypeRegistration
    {
        #region Constructors

        public FeatureTypeRegistration(Type t)
        {
            Type = t;

            var descriptionAttr = t.GetCustomAttribute(typeof(DescriptionAttribute));
            if (descriptionAttr != null && !((DescriptionAttribute)descriptionAttr).Description.Equals(string.Empty))
            {
                Description = ((DescriptionAttribute)descriptionAttr).Description;
            }

            RegisterFeatureProperties();
        }

        #endregion

        #region Properties

        private string _description;
        public string Description
        {
            get { return _description ?? Type.Name; }
            internal set { _description = value; }
        }

        public Type Type { get; private set; }

        private object _parent;
        public object Parent
        {
            get { return _parent; }
            internal set { _parent = value; }
        }

        public string PropertyName { get; set; }
        public bool HasParent
        {
            get { return Parent != null; }
        }

        #endregion


        private List<KeyValuePair<PropertyInfo, ICrudPropertyMetadata>> _featureProperties
            = new List<KeyValuePair<PropertyInfo, ICrudPropertyMetadata>>();
        public IEnumerable<KeyValuePair<PropertyInfo, ICrudPropertyMetadata>> FeatureProperties
        {
            get { return _featureProperties; }
        }

        private void RegisterFeatureProperties()
        {
            foreach (var it in Type.GetProperties())
            {
                var crudAttribute = CrudPropertyConfigurationAttribute
                    .TryGetCrudConfigurationAttribute(it);
                if (crudAttribute != null)
                    AddFeaturePropertyMetadata(it, crudAttribute);
            }
        }

        public void AddFeaturePropertyMetadata(PropertyInfo pi,
            ICrudPropertyMetadata featPropertyMetadata)
        {
            var exists = FeatureProperties
                .Any(it => it.Key.Name == pi.Name);
            if (exists)
                throw new Exception(
                    "The property " + pi.Name + " " +
                    "of type " + this.Type.FullName + " is already defined.");

            _featureProperties.Add(
                new KeyValuePair<PropertyInfo, ICrudPropertyMetadata>(
                    pi, featPropertyMetadata));
        }

        public override string ToString()
        {
            return Description;
        }
    }
}
