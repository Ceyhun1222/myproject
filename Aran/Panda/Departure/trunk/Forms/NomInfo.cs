using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;

namespace Aran.PANDA.Departure
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public partial class CNomInfo : Form
	{
		public void SetInfo(IPolyline pPolyline, double fTurnHeight, double fPDG)
		{
			Label4.Text = GlobalVars.DistanceConverter[(int) GlobalVars.DistanceUnit].Unit;
			Label5.Text = GlobalVars.HeightConverter [ ( int ) GlobalVars.HeightUnit ].Unit;

			Text1.Text = Functions.ConvertDistance(pPolyline.Length, eRoundMode.NEAREST).ToString();
			Text2.Text = Functions.ConvertHeight(fTurnHeight + pPolyline.Length * fPDG, eRoundMode.NEAREST).ToString();
		}

		public void SetInfo(IPointCollection pPolyline, double fTurnHeight, double fPDG)
		{
			SetInfo(pPolyline as IPolyline, fTurnHeight, fPDG);
		}

		public void ShowMessage(System.Drawing.Point pos)
		{
			Show();
			Left = pos.X;
			Top = pos.Y;
		}

		private void CNomInfoForm_Deactivate(System.Object eventSender, System.EventArgs eventArgs)
		{
			this.Hide();
		}
	}
}

