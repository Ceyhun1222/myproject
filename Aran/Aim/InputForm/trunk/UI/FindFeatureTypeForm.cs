using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim.Metadata.UI;

namespace Aran.Aim.InputForm
{
	public partial class FindFeatureTypeForm : Form
	{
		public FindFeatureTypeForm ()
		{
			InitializeComponent ();
		}

		private void Find_Click (object sender, EventArgs e)
		{
			var classInfo = AimMetadata.AimClassInfoList.Find (ci =>
				string.Equals (ci.Name, ui_featureTypeCB.Text, StringComparison.CurrentCultureIgnoreCase));

			if (classInfo == null)
				return;
			
			var strList = new List<string> ();
			var tmpCI = classInfo;

			while (tmpCI != null)
			{
				strList.Add (tmpCI.Name);

				if (tmpCI.UiInfo ().DependsFeature != null)
				{
					var ft = Enum.Parse (typeof (FeatureType), tmpCI.UiInfo ().DependsFeature);
					tmpCI = AimMetadata.GetClassInfoByIndex ((int) ft);
				}
				else
				{
					break;
				}
			}

			strList.Reverse ();
			var s = string.Empty;
			s = strList [0];
			var tmp = "- - - ";
			for (int i = 1; i < strList.Count; i++)
			{
				s += "\n" + tmp + strList [i];
				tmp = tmp + tmp;
			}
			ui_infoLabel.Text = s;
			ui_infoPanel.Visible = true;
		}

		private void FindFeatureTypeForm_Load (object sender, EventArgs e)
		{
			var ftValues = Enum.GetValues (typeof (FeatureType));

			foreach (FeatureType ft in ftValues)
				ui_featureTypeCB.Items.Add (ft);
		}

		private void Close_Click (object sender, EventArgs e)
		{
			Close ();
		}
	}
}
