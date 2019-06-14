using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Aran.Delta
{
	public class Win32Windows : System.Windows.Forms.IWin32Window
	{
		private IntPtr m_handle;
		public Win32Windows(Int32 handle)
		{
			m_handle = new IntPtr(handle);
		}

		#region IWin32Window Members

		IntPtr IWin32Window.Handle
		{
			get { return m_handle; }
		}

		#endregion
	}


}
