using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim;
using System.Collections;
using Aran.Aim.Data.Filters;
using Aran.Aim.Objects;
using Aran.Aim.Features;
using System.Windows.Forms;
using Aran.Aim.Metadata.UI;

namespace Aran.Queries.Common
{
	internal static class Global
	{
		public static GetFeatureListHandler GetFeatureList { get; set; }

        public static DateTime DefaultEffectiveDate { get; set; }

		public static void ShowFeatureRefs (FeatureType featureType, IEnumerable featureRefs)
		{
			var identifierList = new List<Guid> ();
			foreach (FeatureRefObject fro in featureRefs)
				identifierList.Add (fro.Feature.Identifier);

			var filter = Filter.CreateComparision (ComparisonOpType.In, "identifier", identifierList);
			var features = Global.GetFeatureList (featureType, filter);
			ShowFeatures (features);
		}

		public static void ShowFeatures (IEnumerable features)
		{
			var enumtor = features.GetEnumerator ();
			enumtor.Reset ();
			if (!enumtor.MoveNext ())
				return;
			var firstFeature = enumtor.Current as Feature;

			ShowFeatures (firstFeature.FeatureType, features);
		}

		public static void ShowFeatures (FeatureType featType, IEnumerable features)
		{
			var classInfo = AimMetadata.GetClassInfoByIndex ((int) featType);

			var form = new Form ();
			form.Height = 300;
			form.ShowInTaskbar = false;
			form.MaximizeBox = false;
			form.ShowIcon = false;
			form.StartPosition = FormStartPosition.CenterParent;

			var dgv = new DataGridView ();
			dgv.ReadOnly = true;
			dgv.AllowUserToAddRows = false;
			dgv.AllowUserToDeleteRows = false;
			dgv.Dock = DockStyle.Fill;
			dgv.BackgroundColor = System.Drawing.SystemColors.Window;

			UIUtilities.FillColumns (classInfo, dgv);

			foreach (Feature feature in features)
				UIUtilities.SetRow (dgv, feature);

			form.Controls.Add (dgv);
			form.Width = dgv.Columns.Count * 120 + 30;
			form.Text = string.Format ("{0} - Count: {1}", featType, dgv.Rows.Count);

			form.ShowDialog ();
		}
	}
}
