using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using WpfEnvelope.Crud.Framework;
using System.Reflection;
using Framework.Attributes;
using System.ComponentModel;

namespace WpfEnvelope.Crud
{
	public class MetaTypeRegistration
	{
		#region Constructors

		public MetaTypeRegistration(Type t)
		{
			Type = t;

            var descriptionAttr = t.GetCustomAttribute(typeof(DescriptionAttribute));
            if(descriptionAttr!=null&& !((DescriptionAttribute)descriptionAttr).Description.Equals(string.Empty))
            {
                Description = ((DescriptionAttribute)descriptionAttr).Description;
            }

            RegisterCrudProperties();
		}

		public MetaTypeRegistration(Type t, Func<IEnumerable> dataSourceGetMethod)
			: this(t)
		{
			_dataSourceManager.SetDataSourceGetMethod(dataSourceGetMethod);
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

		private readonly MethodManager _methodManager = new MethodManager();
		internal MethodManager MethodManager
		{
			get { return _methodManager; }
		}

		private readonly MethodManager _propertyMethodManager
			= new MethodManager();
		internal MethodManager PropertyMethodManager
		{
			get { return _propertyMethodManager; }
		}

		private readonly DataSourceManager _dataSourceManager
			= new DataSourceManager();
		internal DataSourceManager DataSourceManager
		{
			get { return _dataSourceManager; }
		}

		#endregion

		private List<MetaTypeRegistration> _childRegistrations
			= new List<MetaTypeRegistration>();
		public IEnumerable<MetaTypeRegistration> ChildRegistrations
		{
			get { return _childRegistrations; }
		}

		#region Child Type Management

		public MetaTypeRegistration TryGetChildRegistration(Type childType)
		{
			return ChildRegistrations.FirstOrDefault(mtr => mtr.Type == childType);
		}

		public MetaTypeRegistration RegisterChildType(Type childType)
		{
			// TODO: check if T really has a property of type TChild

			MetaTypeRegistration mtr = new MetaTypeRegistration(childType);
			_childRegistrations.Add(mtr);

			return mtr;
		}

		#endregion

		private List<MetaTypeRegistration> _collectionPropertyRegistrations
			= new List<MetaTypeRegistration>();
		public IEnumerable<MetaTypeRegistration> CollectionPropertyRegistrations
		{
			get { return _collectionPropertyRegistrations; }
		}

        #region Collecion Property Management

        //  Регистрации свойств пока не используются
        public MetaTypeRegistration TryGetCollectionPropertyRegistration(string propertyName)
		{
			return CollectionPropertyRegistrations.FirstOrDefault(
				mtr => mtr.PropertyName == propertyName);
		}

		public MetaTypeRegistration RegisterCollectionProperty(
			Type propertyType, string propertyName)
		{
			
			MetaTypeRegistration mtr = new MetaTypeRegistration(propertyType)
			{
				PropertyName = propertyName
			};
			_collectionPropertyRegistrations.Add(mtr);

			return mtr;
		}

		#endregion

		private List<KeyValuePair<PropertyInfo, ICrudPropertyMetadata>> _crudProperties
			= new List<KeyValuePair<PropertyInfo, ICrudPropertyMetadata>>();
		public IEnumerable<KeyValuePair<PropertyInfo, ICrudPropertyMetadata>> CrudProperties
		{
			get { return _crudProperties; }
		}

		private void RegisterCrudProperties()
		{
			foreach (var it in Type.GetProperties())
			{
				var crudAttribute = CrudPropertyConfigurationAttribute
					.TryGetCrudConfigurationAttribute(it);
				if (crudAttribute != null)
					AddCrudPropertyMetadata(it, crudAttribute);
			}
		}

		public void AddCrudPropertyMetadata(PropertyInfo pi,
			ICrudPropertyMetadata crudPropertyMetadata)
		{
			var exists = CrudProperties
				.Any(it => it.Key.Name == pi.Name);
			if (exists)
				throw new CrudConfigurationException(
					"The property " + pi.Name + " " +
					"of type " + this.Type.FullName + " is already defined.");

			_crudProperties.Add(
				new KeyValuePair<PropertyInfo, ICrudPropertyMetadata>(
					pi, crudPropertyMetadata));
		}

		public override string ToString()
		{
			return Description;
		}
	}
}
