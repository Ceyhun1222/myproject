using System;

namespace WpfEnvelope.Crud
{
	public class PropertyCollectionConfiguration<TParent, T> : BaseConfiguration
	{
		internal PropertyCollectionConfiguration(
			MetaTypeRegistration mtr,
			string parentPropertyName) : base(mtr)
		{
			// TODO: Type check, etc.

			_mtr.PropertyName = parentPropertyName;
		}

		public PropertyCollectionConfiguration<TParent, T> DefineAddOperation(
			Action<TParent, T> addMethod)
		{
			_mtr.PropertyMethodManager.DefineAddOperation(
				(o1, o2) => addMethod((TParent)o1, (T)o2));

			return this;
		}

		public PropertyCollectionConfiguration<TParent, T> DefineRemoveOperation(
			Action<TParent, T> removeMethod)
		{
			_mtr.PropertyMethodManager.DefineRemoveOperation(
				(o1, o2) => removeMethod((TParent)o1, (T)o2));

			return this;
		}
	}
}
