using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace Aran.Aim.FmdEditor
{
	public partial class NavigatorControl : UserControl
	{
		private int _currentIndex;
		private IList _valueList;
		private Control _itemControl;
		private bool _readOnly;


		public NavigatorControl ()
		{
			InitializeComponent ();

			_valueList = new List<object> ();
			_currentIndex = 0;

			EnabledButtons ();
		}

		public Control ItemControl
		{
			get { return _itemControl; }
			set
			{
				_itemControl = value;
				_valueList.Clear ();
				_valueList.Add (GetControlValue (true));
				EnabledButtons ();
			}
		}

		public int CurrentIndex
		{
			get { return _currentIndex; }
			set
			{
				if (value < 0 || value > _valueList.Count)
					throw new Exception ("Current index is out of range");

				if (_itemControl == null)
					return;

				var prevItem = GetControlValue ();

				if (_valueList.Count == 0)
				{
					_valueList.Add (GetControlValue (true));
					SetCountText ();
				}

				_valueList [_currentIndex] = prevItem;

				_currentIndex = value;

				ui_iOfCountTB.Text = (_currentIndex + 1).ToString ();

				object item = _valueList [_currentIndex];

				SetControlValue (item);

				EnabledButtons ();
			}
		}

		public void SetValue (IList valueList)
		{
			if (valueList == null)
				return;

			_valueList.Clear ();
			foreach (var item in valueList)
				_valueList.Add (item);
			CurrentIndex = 0;

			SetCountText ();
		}

		private void SetCountText ()
		{
			ui_countLabel.Text = "/ " + _valueList.Count;
		}

		public IList GetValue ()
		{
			var currItem = GetControlValue ();

			_valueList [_currentIndex] = currItem;

			return _valueList;
		}

		public bool ReadOnly
		{
			get { return _readOnly; }
			set
			{
				_readOnly = value;

				ui_addButton.Enabled = 
					ui_removeButton.Enabled = !value;

				EnabledButtons ();
			}
		}
		
		
		private void EnabledButtons ()
		{
			ui_prevButton.Enabled =
			ui_firstButton.Enabled = _currentIndex > 0;
			ui_nextButton.Enabled = 
			ui_lastButton.Enabled = (_currentIndex < (_valueList.Count - 1));

			if (!_readOnly)
			{
				ui_addButton.Enabled = (_itemControl != null);
				ui_removeButton.Enabled = (_itemControl != null && _valueList.Count > 0);
			}
		}

		private object GetControlValue (bool isNew = false)
		{
			if (_itemControl is TextBox)
			{
				if (isNew)
					return string.Empty;
				return (_itemControl as TextBox).Text;
			}

			if (_itemControl is INavigationItemControl)
			{
				var nic = _itemControl as INavigationItemControl;

				if (isNew)
					return nic.GetNewValue ();

				return nic.GetValue ();
			}

			return null;
		}

		private void SetControlValue (object value)
		{
			if (_itemControl is TextBox)
			{
				(_itemControl as TextBox).Text = value.ToString ();
				return;
			}

			if (_itemControl is INavigationItemControl)
			{
				var nic = _itemControl as INavigationItemControl;
				nic.SetValue (value);
			}
		}

		private void Add_Click (object sender, EventArgs e)
		{
			var newVal = GetControlValue (true);
			_valueList.Add (newVal);
			CurrentIndex++;
			SetCountText ();
		}

		private void Remove_Click (object sender, EventArgs e)
		{
			_valueList.RemoveAt (_currentIndex);
			_currentIndex--;

			if (_currentIndex < 0)
				_currentIndex = 0;

			if (_currentIndex == _valueList.Count)
			{
				var newItem = GetControlValue (true);
				_valueList.Add (newItem);
			}

			var item = _valueList [_currentIndex];
			SetControlValue (item);
			EnabledButtons ();
			SetCountText ();
		}

		private void Next_Click (object sender, EventArgs e)
		{
			CurrentIndex++;
		}

		private void Prev_Click (object sender, EventArgs e)
		{
			CurrentIndex--;
		}

		private void First_Click (object sender, EventArgs e)
		{
			CurrentIndex = 0;
		}

		private void Last_Click (object sender, EventArgs e)
		{
			CurrentIndex = _valueList.Count - 1;
		}
	}
}
