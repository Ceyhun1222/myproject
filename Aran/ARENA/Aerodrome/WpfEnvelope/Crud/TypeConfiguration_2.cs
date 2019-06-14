using Framework.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfEnvelope.Crud
{
	public class TypeConfiguration<TParent, T> : BaseTypeConfiguration<T>
	{
		internal TypeConfiguration(MetaTypeRegistration mtr)
			: base(mtr) { }

		public TypeConfiguration<TParent, T> DefinePropertyMetadata(string propertyName,
			CrudPropertyMetadata crudPropertyMetadata)
		{
			var pi = AssertCrudProperty(propertyName);
			_mtr.AddCrudPropertyMetadata(pi, crudPropertyMetadata);

			return this;
		}

		public TypeConfiguration<TParent, T> DefineAddOperation(
			Action<TParent, T> addMethod, Func<T> newMethod, Action<T> cancelMethod)
		{
			_mtr.MethodManager.DefineAddOperation(
				(o1, o2) => addMethod((TParent)o1, (T)o2),
				() => newMethod(),
				(o) => cancelMethod((T)o));

			return this;
		}

		public TypeConfiguration<TParent, T> DefineRemoveOperation(
			Action<TParent, T> removeMethod)
		{
			_mtr.MethodManager.DefineRemoveOperation(
				(o1, o2) => removeMethod((TParent)o1, (T)o2));
			return this;
		}

		public TypeConfiguration<TParent, T> DefineEditOperation(Func<T, T> editMethod,
			Action<T> confirmMethod, Action<T> cancelMethod)
		{
			_mtr.MethodManager.DefineEditOperation(
				(o) => editMethod((T)o),
				(o) => confirmMethod((T)o),
				(o) => cancelMethod((T)o));
			return this;
		}

		public TypeConfiguration<TParent, T> DefineCustomOperation(Action<T> operation, string operationName)
		{
			_mtr.MethodManager.CustomOperations.Add(
				(o) => operation((T)o),
				operationName);
			return this;
		}
	}
}
