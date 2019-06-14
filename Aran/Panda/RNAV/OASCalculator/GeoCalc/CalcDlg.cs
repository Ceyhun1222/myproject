using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OASCalculator;
using GeoCalc;

namespace OASCalculator
{
	public partial class CalcDlg : Form
	{
		double ILatSign;
		double ILonSign;
		double IILatSign;
		double IILonSign;

		double IStationLat;
		double IStationLon;
		double IIStationLat;
		double IIStationLon;

		public CalcDlg()
		{
			InitializeComponent();

			comboBox09.SelectedIndex = 0;
			comboBox10.SelectedIndex = 0;

			comboBox11.SelectedIndex = 0;
			comboBox12.SelectedIndex = 0;


			comboBox05.SelectedIndex = 0;
			comboBox06.SelectedIndex = 0;

			comboBox07.SelectedIndex = 0;
			comboBox08.SelectedIndex = 0;


			comboBox01.SelectedIndex = 0;
			comboBox02.SelectedIndex = 0;

			comboBox03.SelectedIndex = 0;
			comboBox04.SelectedIndex = 0;
		}

		#region  Common events

		private void TextBoxes_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
				e.IsInputKey = true;
		}

		private void DoubleTextBoxes_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			TextBox senderBox = (TextBox)sender;
			//var OnValidating = senderBox.GetType().GetMethod("Validating", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

			if (eventChar == 13)
			{
				//var fildInfo = senderBox.GetType().GetField("Validating", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField);
				//var eventDelegate = (MulticastDelegate)senderBox.GetType().GetField("Validating", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(senderBox);
				//if (eventDelegate != null)
				//	foreach (var handler in eventDelegate.GetInvocationList())
				//		handler.Method.Invoke(handler.Target, new object[] { senderBox, new object[] { new System.ComponentModel.CancelEventArgs() } });

				//OnValidating.Invoke(senderBox, new object[] { new System.ComponentModel.CancelEventArgs() });

				string methodName = senderBox.Name + "_Validating";
				System.Reflection.MethodInfo methodInfo = this.GetType().GetMethod(methodName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
				methodInfo.Invoke(this, new object[] { senderBox, new System.ComponentModel.CancelEventArgs() });
			}
			else
				Functions.TextBoxFloat(ref eventChar, senderBox.Text);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		private void IntegerTextBoxes_KeyPress(object sender, KeyPressEventArgs e)
		{
			char eventChar = e.KeyChar;
			TextBox senderBox = (TextBox)sender;

			//var eventInfo = senderBox.GetType().GetEvent("Validating", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public| System.Reflection.BindingFlags.InvokeMethod );
			////var methodInfo = senderBox.GetType().GetMethod("Validating");
			////var methodInfos = senderBox.GetType().GetMethods();
			////var eventInfos = senderBox.GetType().GetEvents(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.InvokeMethod);

			//var fildInfo = senderBox.GetType().GetField("Validating", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField);
			//var fildInfos = senderBox.GetType().GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.);


			//var OnValidating = eventInfo.GetRaiseMethod(false);// .GetMethod("Validating", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

			//var methodInfo = eventInfo.EventHandlerType.GetMethod("Invoke");
			//				//Object o = Activator.CreateInstance(eventInfo.EventHandlerType);
			//	//Type delegateType = typeof(MainClass).GetEvent("ev").EventHandlerType;
			//	//MethodInfo invoke = delegateType.GetMethod("Invoke");

			//	System.Reflection.ParameterInfo[] pars = methodInfo.GetParameters();
			//	methodInfo.Invoke(senderBox, new object[] { senderBox, new System.ComponentModel.CancelEventArgs() });

			////senderBox.GetType().InvokeMember("Validating",System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public| System.Reflection.BindingFlags.InvokeMethod , new System.Reflection.Binder (),   );
			// //EventInfoInstance.EventHandlerType.GetMethod("Invoke")

			////if (eventInfo != null)
			////{
			//	//var event_member = eventInfo.GetRaiseMethod(true);//	 .GetValue(this);
			////	// Note : If event_member is null, nobody registered to the event, you can't call it.
			////	if (event_member != null)
			////		event_member.GetType().GetMethod("Invoke").Invoke(event_member, new object[] { this, eventArgs });
			////}

			////EventArgs e = new EventArgs(instanceId);

			////MulticastDelegate eventDelagate = (MulticastDelegate)senderBox.GetType().GetField("Validating", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(senderBox);
			////Delegate[] delegates = eventDelagate.GetInvocationList();
			////foreach (Delegate dlg in delegates)
			////{
			////	dlg.Method.Invoke(dlg.Target, new object[] { this, e });
			////} 


			if (eventChar == 13)
			{
				string methodName = senderBox.Name + "_Validating";
				System.Reflection.MethodInfo methodInfo = this.GetType().GetMethod(methodName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
				methodInfo.Invoke(this, new object[] { senderBox, new System.ComponentModel.CancelEventArgs() });
			}
			else
				Functions.TextBoxInteger(ref eventChar);

			e.KeyChar = eventChar;
			if (eventChar == 0)
				e.Handled = true;
		}

		//============== 1 station ================================
		private void comboBox01_SelectedIndexChanged(object sender, EventArgs e)
		{
			ComboBox senderBox = (ComboBox)sender;
			if (senderBox.SelectedIndex < 0)
				return;

			ILatSign = 1 - 2 * senderBox.SelectedIndex;
			IStationLat = ILatSign * Math.Abs(IStationLat);
		}

		private void comboBox02_SelectedIndexChanged(object sender, EventArgs e)
		{
			ComboBox senderBox = (ComboBox)sender;
			if (senderBox.SelectedIndex < 0)
				return;

			ILonSign = 1 - 2 * senderBox.SelectedIndex;
			IStationLon = ILonSign * Math.Abs(IStationLon);
		}

		//============== 2 station ================================
		private void comboBox03_SelectedIndexChanged(object sender, EventArgs e)
		{
			ComboBox senderBox = (ComboBox)sender;
			if (senderBox.SelectedIndex < 0)
				return;

			IILatSign = 1 - 2 * senderBox.SelectedIndex;
			IIStationLat = IILatSign * Math.Abs(IIStationLat);
		}

		private void comboBox04_SelectedIndexChanged(object sender, EventArgs e)
		{
			ComboBox senderBox = (ComboBox)sender;
			if (senderBox.SelectedIndex < 0)
				return;

			IILonSign = 1 - 2 * senderBox.SelectedIndex;
			IIStationLon = IILonSign * Math.Abs(IIStationLon);
		}

		#endregion

		private void btnCalc_Click(object sender, EventArgs e)
		{
			TGCoordinate Pt0, Pt1;
			double Dist, Az0, Az1;

			Pt0.X = IStationLon;
			Pt0.Y = IStationLat;

			Pt1.X = IIStationLon;
			Pt1.Y = IIStationLat;

			Dist = GeoCalc.GeoCalc.ReturnAntipodalDistance(Pt0, Pt1, out Az0, out Az1);

			Az1 = Utils.Mod360(Az1 + 180.0);

			distanceEdit.Text = Dist.ToString("N");
			InitialAEdit.Text = Az0.ToString("0.0000");
			FinalAEdit.Text = Az1.ToString("0.0000");
		}

		private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
			double IStationAbsLat = Math.Abs(IStationLat);
			double IStationAbsLon = Math.Abs(IStationLon);
			double IIStationAbsLat = Math.Abs(IIStationLat);
			double IIStationAbsLon = Math.Abs(IIStationLon);
			//========================================================
			int degILat = (int)Math.Truncate(IStationAbsLat);
			double minILat = IStationAbsLat - degILat;

			int degILon = (int)Math.Truncate(IStationAbsLon);
			double minILon = IStationAbsLon - degILon;

			int degIILat = (int)Math.Truncate(IIStationAbsLat);
			double minIILat = IIStationAbsLat - degIILat;

			int degIILon = (int)Math.Truncate(IIStationAbsLon);
			double minIILon = IIStationAbsLon - degIILon;
				//========================================================
			int iMinILat = (int)Math.Round(60.0 * minILat, 10);
			double secILat = 3600.0 * (IStationAbsLat - degILat - iMinILat / 60.0);

			int iMinILon = (int)Math.Round(60.0 * minILon, 10);
			double secILon = 3600.0 * (IStationAbsLon - degILon - iMinILon / 60.0);

			int iMinIILat = (int)Math.Round(60.0 * minIILat, 10);
			double secIILat = 3600.0 * (IIStationAbsLat - degIILat - iMinIILat / 60.0);

			int iMinIILon = (int)Math.Round(60.0 * minIILon, 10);
			double secIILon = 3600.0 * (IIStationAbsLon - degIILon - iMinIILon / 60.0);

			switch (tabControl1.SelectedIndex)
			{
				case 0:
					comboBox01.SelectedIndex = (int)Math.Round(0.5 * (1 - ILatSign));
					comboBox02.SelectedIndex = (int)Math.Round(0.5 * (1 - ILonSign));

					comboBox03.SelectedIndex = (int)Math.Round(0.5 * (1 - IILatSign));
					comboBox04.SelectedIndex = (int)Math.Round(0.5 * (1 - IILonSign));

					textBox01.Text = IStationAbsLat.ToString("0.00000");
					textBox02.Text = IStationAbsLon.ToString("0.00000");
					textBox03.Text = IIStationAbsLat.ToString("0.00000");
					textBox04.Text = IIStationAbsLon.ToString("0.00000");

					break;
				case 1:
					comboBox05.SelectedIndex = (int)Math.Round(0.5 * (1 - ILatSign));
					comboBox06.SelectedIndex = (int)Math.Round(0.5 * (1 - ILonSign));

					comboBox07.SelectedIndex = (int)Math.Round(0.5 * (1 - IILatSign));
					comboBox08.SelectedIndex = (int)Math.Round(0.5 * (1 - IILonSign));

					textBox05.Text = degILat.ToString("00");
					textBox06.Text = (60.0* minILat).ToString("00.0000");

					textBox07.Text = degILon.ToString("000");
					textBox08.Text = (60.0 * minILon).ToString("00.0000");

					textBox09.Text = degIILat.ToString("00");
					textBox10.Text = (60.0 * minIILat).ToString("00.0000");

					textBox11.Text = degIILon.ToString("000");
					textBox12.Text = (60.0 * minIILon).ToString("00.0000");

					break;
				case 2:
					comboBox09.SelectedIndex = (int)Math.Round(0.5 * (1 - ILatSign));
					comboBox10.SelectedIndex = (int)Math.Round(0.5 * (1 - ILonSign));

					comboBox11.SelectedIndex = (int)Math.Round(0.5 * (1 - IILatSign));
					comboBox12.SelectedIndex = (int)Math.Round(0.5 * (1 - IILonSign));

					textBox13.Text = degILat.ToString("00");
					textBox14.Text = iMinILat.ToString("00");
					textBox15.Text = secILat.ToString("00.0000");

					textBox16.Text = degILon.ToString("000");
					textBox17.Text = iMinILon.ToString("00");
					textBox18.Text = secILon.ToString("00.0000");


					textBox19.Text = degIILat.ToString("00");
					textBox20.Text = iMinIILat.ToString("00");
					textBox21.Text = secIILat.ToString("00.0000");

					textBox22.Text = degIILon.ToString("000");
					textBox23.Text = iMinIILon.ToString("00");
					textBox24.Text = secIILon.ToString("00.0000");

					break;
			}
		}

		#region DD
		//============== 1 station ================================

		private void textBox01_Validating(object sender, CancelEventArgs e)
		{
			double IStationAbsLat = Math.Abs(IStationLat);

			double fTmp;
			if (!double.TryParse(textBox01.Text, out fTmp))
			{
				//if (double.TryParse((string)textBox01.Tag, out fTmp))
				//	textBox01.Text = (string)textBox01.Tag;
				//else

				textBox01.Text = IStationAbsLat.ToString("00.00000");
				return;
			}

			IStationLat = ILatSign * fTmp;
		}

		private void textBox02_Validating(object sender, CancelEventArgs e)
		{
			double IStationAbsLon = Math.Abs(IStationLon);

			double fTmp;
			if (!double.TryParse(textBox02.Text, out fTmp))
			{
				//if (double.TryParse((string)textBox02.Tag, out fTmp))
				//	textBox02.Text = (string)textBox02.Tag;
				//else

				textBox02.Text = IStationAbsLon.ToString("000.00000");
				return;
			}

			IStationLon = ILonSign * fTmp;
		}

		//============== 2 station ================================
		private void textBox03_Validating(object sender, CancelEventArgs e)
		{
			double IIStationAbsLat = Math.Abs(IIStationLat);
			double fTmp;
			if (!double.TryParse(textBox03.Text, out fTmp))
			{
				//if (double.TryParse((string)textBox03.Tag, out fTmp))
				//	textBox03.Text = (string)textBox03.Tag;
				//else

				textBox03.Text = IIStationAbsLat.ToString("00.00000");
				return;
			}

			IIStationLat = IILatSign * fTmp;
		}

		private void textBox04_Validating(object sender, CancelEventArgs e)
		{
			double IIStationAbsLon = Math.Abs(IIStationLon);
			double fTmp;
			if (!double.TryParse(textBox04.Text, out fTmp))
			{
				//if (double.TryParse((string)textBox04.Tag, out fTmp))
				//	textBox04.Text = (string)textBox04.Tag;
				//else

				textBox04.Text = IIStationAbsLon.ToString("000.00000");
				return;
			}

			IIStationLon = IILonSign * fTmp;
		}

		#endregion

		#region DM

		//============== 1 station ================================
		private void textBox05_Validating(object sender, CancelEventArgs e)
		{
			double IStationAbsLat = Math.Abs(IStationLat);
			int iTmp, deg = (int)Math.Truncate(IStationAbsLat);
			double min = IStationAbsLat - deg;

			if (!int.TryParse(textBox05.Text, out iTmp))
			{
				//if (int.TryParse((string)textBox05.Tag, out iTmp))
				//	textBox05.Text = (string)textBox05.Tag;
				//else

				textBox05.Text = deg.ToString("00");
				return;
			}

			IStationLat = ILatSign * (iTmp + min);
		}

		private void textBox06_Validating(object sender, CancelEventArgs e)
		{
			double IStationAbsLat = Math.Abs(IStationLat);
			int deg = (int)Math.Truncate(IStationAbsLat);
			double fTmp, min = 60.0 * (IStationAbsLat - deg);

			if (!double.TryParse(textBox06.Text, out fTmp))
			{
				//if (double.TryParse((string)textBox06.Tag, out fTmp))
				//	textBox06.Text = (string)textBox06.Tag;
				//else

				textBox06.Text = min.ToString("00.0000");
				return;
			}

			while (fTmp >= 60)
				fTmp = -60;

			IStationLat = ILatSign * (deg + fTmp / 60.0);
		}

		private void textBox07_Validating(object sender, CancelEventArgs e)
		{
			double IStationAbsLon = Math.Abs(IStationLon);
			int iTmp, deg = (int)Math.Truncate(IStationAbsLon);
			double min = IStationAbsLon - deg;

			if (!int.TryParse(textBox07.Text, out iTmp))
			{
				//if (int.TryParse((string)textBox07.Tag, out iTmp))
				//	textBox07.Text = (string)textBox07.Tag;
				//else

				textBox07.Text = deg.ToString("000");
				return;
			}

			IStationLon = ILonSign * (iTmp + min);
		}

		private void textBox08_Validating(object sender, CancelEventArgs e)
		{
			double IStationAbsLon = Math.Abs(IStationLon);
			int deg = (int)Math.Truncate(IStationAbsLon);
			double fTmp, min = 60.0 * (IStationAbsLon - deg);

			if (!double.TryParse(textBox08.Text, out fTmp))
			{
				//if (double.TryParse((string)textBox08.Tag, out fTmp))
				//	textBox08.Text = (string)textBox08.Tag;
				//else

				textBox08.Text = min.ToString("00.0000");
				return;
			}

			while (fTmp >= 60)
				fTmp = -60;

			IStationLon = ILonSign * (deg + fTmp / 60.0);
		}

		//============== 2 station ================================
		private void textBox09_Validating(object sender, CancelEventArgs e)
		{
			double IIStationAbsLat = Math.Abs(IIStationLat);
			int iTmp, deg = (int)Math.Truncate(IIStationAbsLat);
			double min = IIStationAbsLat - deg;

			if (!int.TryParse(textBox09.Text, out iTmp))
			{
				//if (int.TryParse((string)textBox09.Tag, out iTmp))
				//	textBox09.Text = (string)textBox09.Tag;
				//else

				textBox09.Text = deg.ToString("00");
				return;
			}

			IIStationLat = IILatSign * (iTmp + min);
		}

		private void textBox10_Validating(object sender, CancelEventArgs e)
		{
			double IIStationAbsLat = Math.Abs(IIStationLat);
			int deg = (int)Math.Truncate(IIStationAbsLat);
			double fTmp, min = 60.0 * (IIStationAbsLat - deg);

			if (!double.TryParse(textBox10.Text, out fTmp))
			{
				//if (double.TryParse((string)textBox10.Tag, out fTmp))
				//	textBox10.Text = (string)textBox10.Tag;
				//else

				textBox10.Text = min.ToString("00.0000");
				return;
			}

			while (fTmp >= 60)
				fTmp = -60;

			IIStationLat = IILatSign * (deg + fTmp / 60.0);
		}

		private void textBox11_Validating(object sender, CancelEventArgs e)
		{
			double IIStationAbsLon = Math.Abs(IIStationLon);
			int iTmp, deg = (int)Math.Truncate(IIStationAbsLon);
			double min = IIStationAbsLon - deg;

			if (!int.TryParse(textBox11.Text, out iTmp))
			{
				//if (int.TryParse((string)textBox11.Tag, out iTmp))
				//	textBox11.Text = (string)textBox11.Tag;
				//else

				textBox11.Text = deg.ToString("000");
				return;
			}

			IIStationLon = IILonSign * (iTmp + min);
		}

		private void textBox12_Validating(object sender, CancelEventArgs e)
		{
			double IIStationAbsLon = Math.Abs(IIStationLon);
			int deg = (int)Math.Truncate(IIStationAbsLon);
			double fTmp, min = 60.0 * (IIStationAbsLon - deg);

			if (!double.TryParse(textBox12.Text, out fTmp))
			{
				//if (double.TryParse((string)textBox12.Tag, out fTmp))
				//	textBox12.Text = (string)textBox12.Tag;
				//else

				textBox12.Text = min.ToString("00.0000");
				return;
			}

			while (fTmp >= 60)
				fTmp = -60;

			IIStationLon = IILonSign * (deg + fTmp / 60.0);
		}

		#endregion

		#region DMS
		//============== 1 station ================================

		//-	Lat
		private void textBox13_Validating(object sender, CancelEventArgs e)
		{
			double IStationAbsLat = Math.Abs(IStationLat);
			int iTmp, deg = (int)Math.Truncate(IStationAbsLat);
			double min = IStationAbsLat - deg;

			if (!int.TryParse(textBox13.Text, out iTmp))
			{
				//if (int.TryParse((string)textBox13.Tag, out iTmp))
				//	textBox13.Text = (string)textBox13.Tag;
				//else

				textBox13.Text = deg.ToString("00");
				return;
			}

			IStationLat = ILatSign * (iTmp + min);
		}

		private void textBox14_Validating(object sender, CancelEventArgs e)
		{
			double IStationAbsLat = Math.Abs(IStationLat);
			int iTmp, deg = (int)Math.Truncate(IStationAbsLat);
			int min = (int)Math.Round(60.0 * (IStationAbsLat - deg), 10);

			double sec = IStationAbsLat - deg - min / 60.0;

			if (!int.TryParse(textBox14.Text, out iTmp))
			{
				//if (int.TryParse((string)textBox14.Tag, out iTmp))
				//	textBox14.Text = (string)textBox14.Tag;
				//else

				textBox14.Text = min.ToString("00");
				return;
			}

			while (iTmp >= 60)
				iTmp = -60;

			IStationLat = ILatSign * (deg + iTmp / 60.0 + sec);
		}

		private void textBox15_Validating(object sender, CancelEventArgs e)
		{
			double fTmp, IStationAbsLat = Math.Abs(IStationLat);
			int deg = (int)Math.Truncate(IStationAbsLat);
			int min = (int)Math.Round(60.0 * (IStationAbsLat - deg), 10);

			double sec = 3600.0 * (IStationAbsLat - deg - min / 60.0);

			if (!double.TryParse(textBox15.Text, out fTmp))
			{
				//if (double.TryParse((string)textBox15.Tag, out fTmp))
				//	textBox15.Text = (string)textBox15.Tag;
				//else

				textBox15.Text = sec.ToString("00.0000");
				return;
			}

			while (fTmp >= 60)
				fTmp = -60;

			IStationLat = ILatSign * (deg + min / 60.0 + fTmp / 3600.0);
		}

		//-	Lon
		private void textBox16_Validating(object sender, CancelEventArgs e)
		{
			double IStationAbsLon = Math.Abs(IStationLon);
			int iTmp, deg = (int)Math.Truncate(IStationAbsLon);
			double min = IStationAbsLon - deg;

			if (!int.TryParse(textBox16.Text, out iTmp))
			{
				//if (int.TryParse((string)textBox16.Tag, out iTmp))
				//	textBox16.Text = (string)textBox16.Tag;
				//else

				textBox16.Text = deg.ToString("000");
				return;
			}

			IStationLon = ILonSign * (iTmp + min);
		}

		private void textBox17_Validating(object sender, CancelEventArgs e)
		{
			double IStationAbsLon = Math.Abs(IStationLon);
			int iTmp, deg = (int)Math.Truncate(IStationAbsLon);
			int min = (int)Math.Round(60.0 * (IStationAbsLon - deg), 10);

			double sec = IStationAbsLon - deg - min / 60.0;

			if (!int.TryParse(textBox17.Text, out iTmp))
			{
				//if (int.TryParse((string)textBox17.Tag, out iTmp))
				//	textBox17.Text = (string)textBox17.Tag;
				//else

				textBox17.Text = min.ToString("00");
				return;
			}

			while (iTmp >= 60)
				iTmp = -60;

			IStationLon = ILonSign * (deg + iTmp / 60.0 + sec);
		}

		private void textBox18_Validating(object sender, CancelEventArgs e)
		{
			double fTmp, IStationAbsLon = Math.Abs(IStationLon);
			int deg = (int)Math.Truncate(IStationAbsLon);
			int min = (int)Math.Round(60.0 * (IStationAbsLon - deg), 10);

			double sec = 3600.0 * (IStationAbsLon - deg - min / 60.0);

			if (!double.TryParse(textBox18.Text, out fTmp))
			{
				//if (double.TryParse((string)textBox18.Tag, out fTmp))
				//	textBox18.Text = (string)textBox18.Tag;
				//else

				textBox18.Text = sec.ToString("00.0000");
				return;
			}

			while (fTmp >= 60)
				fTmp = -60;

			IStationLon = ILonSign * (deg + min / 60.0 + fTmp / 3600.0);
		}

		//============== 2 station ================================
		//-	Lat
		private void textBox19_Validating(object sender, CancelEventArgs e)
		{
			double IIStationAbsLat = Math.Abs(IIStationLat);
			int iTmp, deg = (int)Math.Truncate(IIStationAbsLat);
			double min = IIStationAbsLat - deg;

			if (!int.TryParse(textBox19.Text, out iTmp))
			{
				//if (int.TryParse((string)textBox19.Tag, out iTmp))
				//	textBox19.Text = (string)textBox19.Tag;
				//else

				textBox19.Text = deg.ToString("000");
				return;
			}

			IIStationLat = IILatSign * (iTmp + min);
		}

		private void textBox20_Validating(object sender, CancelEventArgs e)
		{
			double IIStationAbsLat = Math.Abs(IIStationLat);
			int iTmp, deg = (int)Math.Truncate(IIStationAbsLat);
			int min = (int)Math.Round(60.0 * (IIStationAbsLat - deg), 10);

			double sec = IIStationAbsLat - deg - min / 60.0;

			if (!int.TryParse(textBox20.Text, out iTmp))
			{
				//if (int.TryParse((string)textBox20.Tag, out iTmp))
				//	textBox20.Text = (string)textBox20.Tag;
				//else

				textBox20.Text = min.ToString("00");
				return;
			}

			while (iTmp >= 60)
				iTmp = -60;

			IIStationLat = IILatSign * (deg + iTmp / 60.0 + sec);
		}

		private void textBox21_Validating(object sender, CancelEventArgs e)
		{
			double fTmp, IIStationAbsLat = Math.Abs(IIStationLat);
			int deg = (int)Math.Truncate(IIStationAbsLat);
			int min = (int)Math.Round(60.0 * (IIStationAbsLat - deg), 10);

			double sec = 3600.0 * (IIStationAbsLat - deg - min / 60.0);

			if (!double.TryParse(textBox21.Text, out fTmp))
			{
				//if (double.TryParse((string)textBox21.Tag, out fTmp))
				//	textBox21.Text = (string)textBox21.Tag;
				//else

				textBox21.Text = sec.ToString("00.0000");
				return;
			}

			while (fTmp >= 60)
				fTmp = -60;

			IIStationLat = IILatSign * (deg + min / 60.0 + fTmp / 3600.0);
		}

		//-	Lon
		private void textBox22_Validating(object sender, CancelEventArgs e)
		{
			double IIStationAbsLon = Math.Abs(IIStationLon);
			int iTmp, deg = (int)Math.Truncate(IIStationAbsLon);
			double min = IIStationAbsLon - deg;

			if (!int.TryParse(textBox22.Text, out iTmp))
			{
				//if (int.TryParse((string)textBox22.Tag, out iTmp))
				//	textBox22.Text = (string)textBox22.Tag;
				//else

				textBox22.Text = deg.ToString("000");
				return;
			}

			IIStationLon = IILonSign * (iTmp + min);
		}

		private void textBox23_Validating(object sender, CancelEventArgs e)
		{
			double IIStationAbsLon = Math.Abs(IIStationLon);
			int iTmp, deg = (int)Math.Truncate(IIStationAbsLon);
			int min = (int)Math.Round(60.0 * (IIStationAbsLon - deg), 10);

			double sec = IIStationAbsLon - deg - min / 60.0;

			if (!int.TryParse(textBox23.Text, out iTmp))
			{
				//if (int.TryParse((string)textBox23.Tag, out iTmp))
				//	textBox23.Text = (string)textBox23.Tag;
				//else

				textBox23.Text = min.ToString("00");
				return;
			}

			while (iTmp >= 60)
				iTmp = -60;

			IIStationLon = IILonSign * (deg + iTmp / 60.0 + sec);
		}

		private void textBox24_Validating(object sender, CancelEventArgs e)
		{
			double fTmp, IIStationAbsLon = Math.Abs(IIStationLon);
			int deg = (int)Math.Truncate(IIStationAbsLon);
			int min = (int)Math.Round(60.0 * (IIStationAbsLon - deg), 10);

			double sec = 3600.0 * (IIStationAbsLon - deg - min / 60.0);

			if (!double.TryParse(textBox24.Text, out fTmp))
			{
				//if (double.TryParse((string)textBox24.Tag, out fTmp))
				//	textBox24.Text = (string)textBox24.Tag;
				//else

				textBox24.Text = sec.ToString("00.0000");
				return;
			}

			while (fTmp >= 60)
				fTmp = -60;

			IIStationLon = IILonSign * (deg + min / 60.0 + fTmp / 3600.0);
		}

		#endregion

	}
}
