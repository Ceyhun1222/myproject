using System.Windows;

namespace Aran.PANDA.LegCreator.Helpers
{
	public class MessageDialog : IMessageDialog
	{
		public void ShowMessage ( string message )
		{
			MessageBox.Show ( message );
		}
	}
}
