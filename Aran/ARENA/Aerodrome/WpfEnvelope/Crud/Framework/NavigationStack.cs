using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfEnvelope.Crud.Framework
{
	internal class NavigationStack : Stack<ViewModel>
	{
		public ViewModel Goto(ViewModel vm)
		{
			// TODO: check contains

			while (this.Count > 0)
			{
				ViewModel stackElement = this.Pop();
				if (stackElement == vm)
				{
					this.Push(stackElement);
					return stackElement;
				}
			}

			return null; // oder check von oben: throw exception
		}
	}
}
