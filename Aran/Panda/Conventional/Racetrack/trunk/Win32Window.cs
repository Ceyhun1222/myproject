using System;
using System.Windows.Forms;

namespace Aran.PANDA.Conventional.Racetrack
{
	public class Win32Windows : IWin32Window
	{
		private readonly IntPtr _mHandle;
		public Win32Windows(Int32 handle)
		{
			_mHandle = new IntPtr(handle);
		}

		#region IWin32Window Members

		IntPtr IWin32Window.Handle => _mHandle;

		#endregion
	}


}
