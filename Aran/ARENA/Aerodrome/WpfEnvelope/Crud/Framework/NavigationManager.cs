using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace WpfEnvelope.Crud.Framework
{
	public class NavigationManager
	{
		private readonly NavigationStack _navigationHistory
			= new NavigationStack();

		public event EventHandler Navigated;
		private void OnNavigated()
		{
			if (Navigated != null)
				Navigated(this, new EventArgs());
		}

		public event EventHandler BreadcrumpListChanged;
		private void OnBreadcrumpListChanged()
		{
			if (BreadcrumpListChanged != null)
				BreadcrumpListChanged(this, new EventArgs());
		}

		private ViewModel _currentModel;
		public ViewModel CurrentModel
		{
			get { return _currentModel; }
			internal set
			{
				_currentModel = value;
				
				OnNavigated();
				OnBreadcrumpListChanged();
			}
		}

		public IEnumerable<BreadcrumpElement> Breadcrump
		{
			get
			{
				List<BreadcrumpElement> breadcrumps	= new List<BreadcrumpElement>();
				foreach (var videModel in _navigationHistory.ToArray())
					breadcrumps.Add(new BreadcrumpElement(videModel));
				breadcrumps.Reverse();

				return breadcrumps;
			}
		}

		public void Navigate(MetaTypeRegistration mtr, bool clearHistory)
		{
			if (clearHistory)
				_navigationHistory.Clear();

			ViewModel vm = new ViewModel(mtr);
			_navigationHistory.Push(vm);
			CurrentModel = vm;
		}

		public void NavigateBack()
		{
			// TODO: checken, ob leer
			CurrentModel = _navigationHistory.Pop();
		}

		public void NavigateTo(BreadcrumpElement breadcrump)
		{
			// TODO: check if contains

			ViewModel vm = _navigationHistory.Goto(breadcrump.ViewModel);
			CurrentModel = vm;
		}
	}
}
