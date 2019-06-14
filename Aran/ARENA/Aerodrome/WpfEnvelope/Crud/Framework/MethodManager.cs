using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfEnvelope.Crud.Framework
{
	internal class MethodManager
	{
		public MethodManager()
		{
			CustomOperations = new Dictionary<Action<object>, string>();
		}

		public Action<object, object> AddMethod { get; private set; }
		public Func<object> NewMethod { get; private set; }
		public Action<object> CancelAddMethod { get; private set; }

		public Action<object, object> RemoveMethod { get; private set; }

		public Func<object, object> EditMethod { get; private set; }
		public Action<object> ConfirmEditMethod { get; private set; }
		public Action<object> CancelEditMethod { get; private set; }

		public Dictionary<Action<object>, string> CustomOperations { get; private set; }

		public bool CanAdd
		{
			get { return AddMethod != null; }
		}

		public bool CanRemove
		{
			get { return RemoveMethod != null; }
		}

		public bool CanEdit
		{
			get { return EditMethod != null; }
		}

		public void DefineAddOperation(Action<object, object> addMethod)
		{
			AddMethod = addMethod;
		}

		public void DefineAddOperation(Action<object, object> addMethod,
			Func<object> newMethod, Action<object> cancelMethod)
		{
			DefineAddOperation(addMethod);
			NewMethod = newMethod;
			CancelAddMethod = cancelMethod;
		}

		public void DefineRemoveOperation(Action<object, object> removeMethod)
		{
			RemoveMethod = removeMethod;
		}

		// TODO: evtl. cancel method machen
		public void DefineEditOperation(Func<object, object> editMethod,
			Action<object> confirmMethod, Action<object> cancelMethod)
		{
			EditMethod = editMethod;
			ConfirmEditMethod = confirmMethod;
			CancelEditMethod = cancelMethod;
		}
	}
}
