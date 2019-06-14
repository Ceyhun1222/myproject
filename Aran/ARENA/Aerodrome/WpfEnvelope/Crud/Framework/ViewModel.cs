using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace WpfEnvelope.Crud.Framework
{
	public class ViewModel : INotifyPropertyChanged
	{
		public event EventHandler SelectedItemChanged;
		private void OnSelectedItemChanged()
		{
			if (SelectedItemChanged != null)
				SelectedItemChanged(this, new EventArgs());
		}


		private object _selectedItem;
		public object SelectedItem
		{
			get { return _selectedItem; }
			set
			{
				_selectedItem = value;

				//if (_userMode == UserMode.View)
				//	EditedItem = value;

				OnSelectedItemChanged();
				OnPropertyChanged("SelectedItem");
				OnPropertyChanged("IsItemSelected");
				OnPropertyChanged("CanAdd");
				OnPropertyChanged("CanEdit");
				OnPropertyChanged("CanRemove");
			}
		}

		public bool IsItemSelected
		{
			get { return SelectedItem != null; }
		}

		private object _editedItem;
		public object EditedItem
		{
			get { return _editedItem; }
			set
			{
				_editedItem = value;
				OnPropertyChanged("EditedItem");
			}
		}

		#region Mode

		private UserMode _userMode = UserMode.View;

		public bool IsInViewMode
		{
			get { return _userMode == UserMode.View; }
		}

		public bool IsInEditMode
		{
			get { return _userMode == UserMode.Edit; }
		}

		public bool IsInAddMode
		{
			get { return _userMode == UserMode.Add; }
		}

		public void ChangeMode(UserMode mode)
		{
			if (mode != _userMode)
			{
				_userMode = mode;

				OnPropertyChanged("DetailsPanelSize");
                OnPropertyChanged("MasterViewSize");
                OnPropertyChanged("ModificationPanelSize");
				OnPropertyChanged("IsInViewMode");
				OnPropertyChanged("IsInEditMode");
				OnPropertyChanged("IsInAddMode");
				OnPropertyChanged("CanAdd");
				OnPropertyChanged("CanEdit");
				OnPropertyChanged("CanRemove");
				OnPropertyChanged("IsUserInputEnabled");
			}
		}

		public bool IsUserInputEnabled
		{
			get { return IsInEditMode || IsInAddMode; }
		}

		#endregion

		#region Abilities

		public bool CanAdd
		{
			get
			{
				return TypeRegistration.MethodManager.CanAdd
					&& IsInViewMode;
			}
		}

		public bool CanRemove
		{
			get 
			{
				return TypeRegistration.MethodManager.CanRemove
					&& IsItemSelected
					&& IsInViewMode;
			}
		}

		public bool CanEdit
		{
			get 
			{
				return TypeRegistration.MethodManager.CanEdit
				    && IsItemSelected
				    && IsInViewMode;
			}
		}

		#endregion

		#region Details / Modification Panel

		public double DetailsPanelSize
		{
			get
			{
				if (IsInAddMode || IsInEditMode)
					return 0.0;
				else
					return double.NaN;
			}
		}

        public double MasterViewSize
        {
            get
            {
                if (IsInAddMode || IsInEditMode)
                    return 0.0;
                else
                    return double.NaN;
            }
        }

        public double ModificationPanelSize
		{
			get
			{
				if (IsInAddMode || IsInEditMode)
					return double.NaN;
				else
					return 0.0;
			}
		}

		#endregion

		private MetaTypeRegistration _typeRegistration;
		public MetaTypeRegistration TypeRegistration
		{
			get { return _typeRegistration; }
			private set
			{
				_typeRegistration = value;
				OnPropertyChanged("TypeRegistration");
			}
		}

		public ViewModel(MetaTypeRegistration mtr)
		{
			TypeRegistration = mtr;
		}

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion
	}
}
