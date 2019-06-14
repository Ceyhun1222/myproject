using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Framework.Attributes;

namespace WpfEnvelope.Crud
{
	public abstract class BaseTypeConfiguration<T> : BaseConfiguration
	{
		internal BaseTypeConfiguration(MetaTypeRegistration mtr)
			: base(mtr) { }

		public TypeConfiguration<T, TChild> RegisterChildType<TChild>()
		{
			MetaTypeRegistration childMtr = _mtr.RegisterChildType(typeof(TChild));
			TypeConfiguration<T, TChild> newTc = new TypeConfiguration<T, TChild>(childMtr);

			return newTc;
		}

		public PropertyCollectionConfiguration<T, TProperty> DefineCollectionProperty<TProperty>(
			string propertyName,
			Func<IEnumerable<TProperty>> dataSourceGetMethod)
		{
			throw new NotImplementedException();

			MetaTypeRegistration propertyMtr = _mtr.RegisterCollectionProperty(
				typeof(TProperty), propertyName);
			propertyMtr.DataSourceManager.SetDataSourceGetMethod(
				() => dataSourceGetMethod());
			PropertyCollectionConfiguration<T, TProperty> newPc
				= new PropertyCollectionConfiguration<T, TProperty>(propertyMtr, propertyName);

			return newPc;
		}

		protected PropertyInfo AssertCrudProperty(string propertyName)
		{
			var pi = typeof(T).GetProperties()
				.FirstOrDefault(it => it.Name == propertyName);
			if (pi == null)
				throw new CrudConfigurationException(
					"The property " + propertyName + " " +
					"of type " + typeof(T).FullName + " does not exist.");

			var crudAttribute = CrudPropertyConfigurationAttribute.TryGetCrudConfigurationAttribute(pi);
			if (crudAttribute != null)
				throw new CrudConfigurationException(
					"The property " + propertyName + " " +
					"of type " + typeof(T).FullName + " cannot be registered " +
					"because it is already defined via attribute.");

			return pi;
		}
	}
}
