
namespace Panda.Controls
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public class ComboBox : System.Windows.Forms.ComboBox
	{
		private bool _readOnly;
		private bool _enabled;

		public ComboBox()
		{
			_readOnly = false;
			_enabled = true;
		}

		public bool ReadOnly
		{
			get { return _readOnly; }
			set
			{
				_readOnly = value;
				SetEanbled();
			}
		}

		public new bool Enabled
		{
			get
			{
				return base.Enabled;
			}
			set
			{
				_enabled = value;
				SetEanbled();
			}
		}

		private void SetEanbled()
		{
			base.Enabled = (_enabled && !_readOnly);
		}
	}
}
