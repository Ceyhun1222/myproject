using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using Aran.Aim.Metadata.UI;
using Aran.Aim;
using Aran.Aim.Metadata.Geo;
using Aran.Aim.Data.Filters;
using MapEnv.Layers;

namespace MapEnv
{
    public partial class FeatureStyleControl : UserControl
    {
        private const string DEFAULT_TEXT_KEY = "<value>";
        private AimClassInfo _classInfo;
        private string _textProperty;
        private ITextSymbol _textSymbol;
        public event EventHandler ValueChanged;

        public FeatureStyleControl ()
        {
            InitializeComponent ();

            ui_shapeInfoLV.LargeImageList = new ImageList ();
            ui_shapeInfoLV.LargeImageList.ImageSize = new Size (24, 24);
            ui_symbCatValsListView.LargeImageList = new ImageList ();
            ui_symbCatValsListView.LargeImageList.ImageSize = new Size (24, 24);

            //SetTextSymbol (Globals.CreateDefaultTextSymbol () as ITextSymbol);
        }

		public bool SetShapeInfos(FeatureType featureType, List<TableShapeInfo> shapeInfoList = null)
		{
			ui_textPropCB.Items.Clear();
			ui_geoPropComboBox.Items.Clear();
			ui_shapeInfoLV.Items.Clear();
			ui_shapeInfoLV.LargeImageList.Images.Clear();
			ui_featureTypeTB.Text = string.Empty;
			ui_symbCatPropNameCB.Items.Clear();
			ui_symbCatValsListView.Items.Clear();
			ui_addSymCatButton.Enabled = false;
			ui_removeSymCatButton.Enabled = false;

			_classInfo = UIMetadata.Instance.GetClassInfo((int)featureType);
			if (_classInfo == null)
				return false;
			GeoClassInfo geoClassInfo = GeoMetadata.GetGeoInfoByAimInfo(_classInfo);

			ui_featureTypeTB.Text = featureType.ToString();

			if (geoClassInfo == null)
			{
				Enabled = false;
				return false;
			}

			Enabled = true;

			ui_textPropCB.Items.Add(string.Empty);
			ui_symbCatPropNameCB.Items.Add(string.Empty);
			foreach (var propInfo in _classInfo.Properties)
			{
				if (propInfo.PropType.AimObjectType == AimObjectType.Field)
				{
					ui_textPropCB.Items.Add(propInfo.Name);
					ui_symbCatPropNameCB.Items.Add(propInfo.Name);
				}
			}

			List<GeoPropComboItem> geoPropItemList = null;

			if (geoClassInfo != null)
			{
				List<List<AimPropInfo>> allGeoPropInfos = geoClassInfo.GetProps();
				geoPropItemList = new List<GeoPropComboItem>();
				ui_geoPropComboBox.Tag = geoPropItemList;

				foreach (List<AimPropInfo> geoPropInfos in allGeoPropInfos)
					geoPropItemList.Add(new GeoPropComboItem(geoPropInfos));
			}

			if (shapeInfoList != null && shapeInfoList.Count > 0)
			{
				var firstShapeInfo = shapeInfoList[0];
				_textProperty = firstShapeInfo.TextProperty;
				ui_textPropCB.SelectedItem = _textProperty;
				if (firstShapeInfo.TextSymbol != null)
				{
					SetTextSymbol(firstShapeInfo.TextSymbol);
				}

				ImageList imageList = ui_shapeInfoLV.LargeImageList;
				int backColor = Globals.ToRGBColor(ui_shapeInfoLV.BackColor).RGB;

				using (Graphics gr = ui_shapeInfoLV.CreateGraphics())
				{
					foreach (var shapeInfoItem in shapeInfoList)
					{
						Bitmap bitmap = StyleFunctions.SymbolToBitmap(shapeInfoItem.CategorySymbol.DefaultSymbol, imageList.ImageSize, gr, backColor);
						imageList.Images.Add(bitmap);

						var lvi = ui_shapeInfoLV.Items.Add(shapeInfoItem.GeoProperty);
						lvi.Tag = shapeInfoItem;
						lvi.ImageIndex = imageList.Images.Count - 1;
					}
				}

				geoPropItemList = GetNotAddedGeoPropList();
			}

			foreach (var geoPropItem in geoPropItemList)
			{
				if (geoPropItem.ToString () != null)
					ui_geoPropComboBox.Items.Add(geoPropItem);
			}
			
			if (ui_geoPropComboBox.Items.Count > 0)
				ui_geoPropComboBox.SelectedIndex = 0;

			return true;
		}

        public FeatureType FeatureType
        {
            get
            {
                if (_classInfo == null)
                    return 0;
                return (FeatureType) _classInfo.Index;
            }
        }

        public TableShapeInfo [] GetShapeInfos ()
        {
            TableShapeInfo [] shapeInfoArr = new TableShapeInfo [ui_shapeInfoLV.Items.Count];

            for (int i = 0; i < ui_shapeInfoLV.Items.Count; i++)
            {
                TableShapeInfo shi = ui_shapeInfoLV.Items [i].Tag as TableShapeInfo;
                shapeInfoArr [i] = shi.Clone () as TableShapeInfo;
            }

			if (_textProperty != null && shapeInfoArr.Length > 0)
            {
                var shapeInfo = shapeInfoArr [0];
                shapeInfo.TextProperty = _textProperty;
                shapeInfo.TextSymbol = _textSymbol;
            }

            return shapeInfoArr;
        }

        
        private TableShapeInfo CurrentShapeInfo
        {
            get
            {
                if (ui_shapeInfoLV.SelectedItems.Count == 0)
                    return null;

                return (TableShapeInfo) ui_shapeInfoLV.SelectedItems [0].Tag;
            }
        }

        private void SetSymbolToPictureBox (Button button, ISymbol symbol)
        {
            int backColor = Globals.ToRGBColor (button.BackColor).RGB;
            using (Graphics gr = button.CreateGraphics ())
            {
                Bitmap bitmap = StyleFunctions.SymbolToBitmap (symbol, button.Size - new Size (2, 2), gr, backColor);
                button.Image = bitmap;
            }
        }

        private void SetTextSymbol (ITextSymbol textSymbol)
        {
            _textSymbol = textSymbol;
            SetSymbolToPictureBox (ui_textSymbolButton, textSymbol as ISymbol);
        }


        private void AddShapeInfo_Click (object sender, EventArgs e)
        {
            GeoPropComboItem comboItem = ui_geoPropComboBox.SelectedItem as GeoPropComboItem;
            if (comboItem == null)
                return;

            ISymbol outSymbol;
            ISymbol inSymbol = Globals.CreateDefaultSymbol (comboItem.EsriGeomType, FeatureType);

            if (inSymbol == null)
                return;

            if (Globals.SelectSymbol (inSymbol, out outSymbol))
            {
                ImageList imageList = ui_shapeInfoLV.LargeImageList;
                int backColor = Globals.ToRGBColor (ui_shapeInfoLV.BackColor).RGB;

                using (Graphics gr = ui_symbCatValsListView.CreateGraphics ())
                {
                    Bitmap bitmap = StyleFunctions.SymbolToBitmap (outSymbol, imageList.ImageSize, gr, backColor);
                    imageList.Images.Add (bitmap);

                    TableShapeInfo shapeInfo = new TableShapeInfo ();
                    shapeInfo.GeoProperty = comboItem.PropNamePath;
                    shapeInfo.CategorySymbol.DefaultSymbol = outSymbol;

                    ListViewItem lvi = new ListViewItem ();
                    lvi.Tag = shapeInfo;
                    lvi.Text = comboItem.PropNamePath;
                    lvi.ImageIndex = imageList.Images.Count - 1;

                    ui_shapeInfoLV.Items.Add (lvi);
                }

                ui_geoPropComboBox.Items.Remove (comboItem);
                ui_geoPropComboBox.SelectedItem = null;
                GeoPropComboBox_SelectedIndexChanged (null, null);

                DoValueChanged ();
            }
        }

        private void RemoveShapeInfo_Click (object sender, EventArgs e)
        {
            if (ui_shapeInfoLV.SelectedItems.Count == 0)
                return;

            var lvi = ui_shapeInfoLV.SelectedItems [0];
            TableShapeInfo shapeInfo = lvi.Tag as TableShapeInfo;

            var mbRes = MessageBox.Show ("Do you want to delete the selected Symbol?" + 
                            (string.IsNullOrEmpty (shapeInfo.CategorySymbol.PropertyName) ?
                            "" : "All Category symbols also will we deleted."), "Feature Style",
                            MessageBoxButtons.YesNoCancel);
                        
            if (mbRes != DialogResult.Yes)
                return;

            ui_shapeInfoLV.Items.Remove (lvi);
            if (ui_shapeInfoLV.Items.Count > 0)
                ui_shapeInfoLV.Items [0].Selected = true;

            List<GeoPropComboItem> geoPropInfoList = GetNotAddedGeoPropList ();
            ui_geoPropComboBox.Items.Clear ();
            ui_geoPropComboBox.Items.AddRange (geoPropInfoList.ToArray ());

            if (ui_geoPropComboBox.Items.Count > 0)
                ui_geoPropComboBox.SelectedIndex = 0;

            GeoPropComboBox_SelectedIndexChanged (null, null);
            ShapeInfoLV_SelectedIndexChanged (null, null);

            DoValueChanged ();
        }

        private void GeoPropComboBox_SelectedIndexChanged (object sender, EventArgs e)
        {
            ui_addShapeInfoButton.Enabled = (ui_geoPropComboBox.SelectedItem != null);
        }

        private void ShapeInfoLV_SelectedIndexChanged (object sender, EventArgs e)
        {
            ui_removeShapeInfoButton.Enabled = (ui_shapeInfoLV.SelectedItems.Count > 0);

            ui_symbCatValsListView.Items.Clear ();
            var imageList = ui_symbCatValsListView.LargeImageList;
            imageList.Images.Clear ();
            ui_symbCatPropNameCB.SelectedItem = null;

            var shapeInfo = CurrentShapeInfo;
            ui_symbCatGroupBox.Enabled = (shapeInfo != null);

            if (shapeInfo == null)
                return;

            int backColor = Globals.ToRGBColor (ui_symbCatValsListView.BackColor).RGB;

            using (Graphics gr = ui_symbCatValsListView.CreateGraphics ())
            {
                foreach (var key in shapeInfo.CategorySymbol.Symbols.Keys)
                {
                    ISymbol symbol = shapeInfo.CategorySymbol.Symbols [key];
                    var lvi = ui_symbCatValsListView.Items.Add (key);

                    Bitmap bitmap = StyleFunctions.SymbolToBitmap (symbol, imageList.ImageSize, gr, backColor);
                    imageList.Images.Add (bitmap);

                    lvi.Tag = symbol;
                    lvi.ImageIndex = imageList.Images.Count - 1;
                }
            }
        }

        private void ShapeInfoLV_DoubleClick (object sender, EventArgs e)
        {
            if (ui_shapeInfoLV.SelectedItems.Count == 0)
                return;

            var lvi = ui_shapeInfoLV.SelectedItems [0];
            TableShapeInfo shapeInfo = lvi.Tag as TableShapeInfo;

            ISymbol outSymbol;
            ISymbol inSymbol = shapeInfo.CategorySymbol.DefaultSymbol;

            if (Globals.SelectSymbol (inSymbol, out outSymbol))
            {
                ImageList imageList = ui_shapeInfoLV.LargeImageList;
                int backColor = Globals.ToRGBColor (ui_shapeInfoLV.BackColor).RGB;

                using (Graphics gr = ui_shapeInfoLV.CreateGraphics ())
                {
                    Bitmap bitmap = StyleFunctions.SymbolToBitmap (outSymbol, imageList.ImageSize, gr, backColor);
                    imageList.Images [lvi.ImageIndex] = bitmap;

                    shapeInfo.CategorySymbol.DefaultSymbol = outSymbol;
                }

                ui_shapeInfoLV.Refresh ();
                DoValueChanged ();
            }
        }

        private void TextSymbol_Click (object sender, EventArgs e)
        {
            ISymbol outSymbol;
            if (Globals.SelectSymbol (_textSymbol as ISymbol, out outSymbol))
            {
                SetTextSymbol (outSymbol as ITextSymbol);
                DoValueChanged ();
            }
        }

        private void AddSymCat_Click (object sender, EventArgs e)
        {
            TableShapeInfo shapeInfo = CurrentShapeInfo;
            if (shapeInfo == null)
                return;

            ISymbol outSymbol;
            ISymbol inSymbol = shapeInfo.CategorySymbol.DefaultSymbol;

            if (shapeInfo.CategorySymbol.Symbols.ContainsKey (DEFAULT_TEXT_KEY))
            {
                MessageBox.Show ("First change added symbol value.",
                        Globals.MainForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ImageList imageList = ui_symbCatValsListView.LargeImageList;
            int backColor = Globals.ToRGBColor (ui_symbCatValsListView.BackColor).RGB;
            
            using (Graphics gr = ui_symbCatValsListView.CreateGraphics ())
            {
                if (Globals.SelectSymbol (inSymbol, out outSymbol))
                {
                    Bitmap bitmap = StyleFunctions.SymbolToBitmap (outSymbol, imageList.ImageSize, gr, backColor);
                    imageList.Images.Add (bitmap);

                    ListViewItem lvi = new ListViewItem ();
                    lvi.Tag = outSymbol;
                    lvi.Text = DEFAULT_TEXT_KEY;
                    lvi.ToolTipText = "Value not saved! Please, rename the value text.";
                    lvi.ImageIndex = imageList.Images.Count - 1;

                    ui_symbCatValsListView.Items.Add (lvi);
                    lvi.BeginEdit ();
                }
            }

            DoValueChanged ();
        }

        private void RemoveSymCat_Click (object sender, EventArgs e)
        {
            TableShapeInfo shapeInfo = CurrentShapeInfo;

            if (shapeInfo == null || ui_symbCatValsListView.SelectedItems.Count == 0)
                return;

            ListViewItem lvi = ui_symbCatValsListView.SelectedItems [0];
            shapeInfo.CategorySymbol.Symbols.Remove (lvi.Text);
            lvi.Remove ();

            DoValueChanged ();
        }

        private void SymbCatValsListView_AfterLabelEdit (object sender, LabelEditEventArgs e)
        {
            var shapeInfo = CurrentShapeInfo;
            if (shapeInfo == null)
                return;

            ListViewItem lvi = ui_symbCatValsListView.Items [e.Item];
            string newKey = e.Label;

            if (string.IsNullOrWhiteSpace (newKey))
            {
                e.CancelEdit = true;
                lvi.Text = DEFAULT_TEXT_KEY;
                return;
            }
            if (newKey == DEFAULT_TEXT_KEY)
                return;

            lvi.ToolTipText = null;

            ISymbol symbol = lvi.Tag as ISymbol;
            if (symbol != null)
            {
                shapeInfo.CategorySymbol.Symbols.Add (newKey, symbol);
                lvi.Tag = null;
                return;
            }

            string oldKey = lvi.Text;
            if (oldKey == newKey)
            {
                e.CancelEdit = true;
                return;
            }

            symbol = shapeInfo.CategorySymbol.Symbols [oldKey];
            shapeInfo.CategorySymbol.Symbols.Remove (oldKey);
            shapeInfo.CategorySymbol.Symbols.Add (newKey, symbol);
        }

        private void SymbCatValsListView_DoubleClick (object sender, EventArgs e)
        {
            var shapeInfo = CurrentShapeInfo;

            if (shapeInfo == null || ui_symbCatValsListView.SelectedItems.Count == 0)
                return;

            ISymbol outSymbol;
            ISymbol inSymbol = shapeInfo.CategorySymbol.DefaultSymbol;

            if (inSymbol == null)
            {
                GeoPropComboItem comboItem = ui_geoPropComboBox.SelectedItem as GeoPropComboItem;
                if (comboItem == null)
                {
                    MessageBox.Show ("First set [Geo Property] field.",
                        Globals.MainForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                inSymbol = Globals.CreateDefaultSymbol (comboItem.EsriGeomType);
            }

            if (Globals.SelectSymbol (inSymbol, out outSymbol))
            {
                ImageList imageList = ui_symbCatValsListView.LargeImageList;
                int backColor = Globals.ToRGBColor (ui_symbCatValsListView.BackColor).RGB;

                using (Graphics gr = ui_symbCatValsListView.CreateGraphics ())
                {
                    Bitmap bitmap = StyleFunctions.SymbolToBitmap (outSymbol, imageList.ImageSize, gr, backColor);
                    imageList.Images.Add (bitmap);

                    ListViewItem lvi = ui_symbCatValsListView.SelectedItems [0];
                    lvi.ImageIndex = imageList.Images.Count - 1;

                    if (shapeInfo.CategorySymbol.Symbols.ContainsKey (lvi.Text))
                        shapeInfo.CategorySymbol.Symbols [lvi.Text] = outSymbol;

                }
            }
        }

        private void FeatureStyleControl_Load (object sender, EventArgs e)
        {
            
        }

        private List<GeoPropComboItem> GetNotAddedGeoPropList ()
        {
            List<GeoPropComboItem> list = ui_geoPropComboBox.Tag as List<GeoPropComboItem>;
            List<GeoPropComboItem> resultList = new List<GeoPropComboItem> ();

            foreach (var item in list)
            {
                bool isExists = false;
                foreach (ListViewItem lvi in ui_shapeInfoLV.Items)
                {
                    TableShapeInfo shapeInfo = lvi.Tag as TableShapeInfo;
                    if (shapeInfo.GeoProperty == item.PropNamePath)
                    {
                        isExists = true;
                        break;
                    }
                }

                if (!isExists)
                    resultList.Add (item);
            }

            return resultList;
        }

        private void SymbCatValsListView_SelectedIndexChanged (object sender, EventArgs e)
        {
            TableShapeInfo shapeInfo = CurrentShapeInfo;
            
            ui_removeSymCatButton.Enabled = (shapeInfo != null && ui_symbCatValsListView.SelectedItems.Count > 0);
        }

        private void TextPropCB_SelectedIndexChanged (object sender, EventArgs e)
        {
            _textProperty = (string) ui_textPropCB.SelectedItem;
            DoValueChanged ();
        }

        private void SymbCatPropNameCB_SelectedIndexChanged (object sender, EventArgs e)
        {
            var shapeInfo = CurrentShapeInfo;
            ui_addSymCatButton.Enabled = false;

            if (shapeInfo == null)
                return;

            ui_addSymCatButton.Enabled = true;
            shapeInfo.CategorySymbol.PropertyName = (string) ui_symbCatPropNameCB.SelectedItem;
        }

        private void DoValueChanged ()
        {
            if (ValueChanged != null)
                ValueChanged (this, new EventArgs ());
        }
    }

    internal class GeoPropComboItem
    {
        public GeoPropComboItem (List<AimPropInfo> aimPropPath)
        {
            AimPropPath = aimPropPath;

            if (aimPropPath.Count > 0)
            {
                switch (aimPropPath [aimPropPath.Count - 1].TypeIndex)
                {
                    case (int) Aran.Aim.AimFieldType.GeoPoint:
                        EsriGeomType = esriGeometryType.esriGeometryPoint;
                        break;
                    case (int) Aran.Aim.AimFieldType.GeoPolyline:
                        EsriGeomType = esriGeometryType.esriGeometryPolyline;
                        break;
                    case (int) Aran.Aim.AimFieldType.GeoPolygon:
                        EsriGeomType = esriGeometryType.esriGeometryPolygon;
                        break;
                    default:
                        EsriGeomType = esriGeometryType.esriGeometryNull;
                        break;
                }
            }
            else
                EsriGeomType = esriGeometryType.esriGeometryNull;
            
        }

        public List<AimPropInfo> AimPropPath
        {
            get
            {
                return _aimPropPath;
            }
            set
            {
                _aimPropPath = value;
                DisplayPropertyName = "";
                
                if (_aimPropPath.Count == 0)
                {
                    PropNamePath = "";
                }
                else
                {
                    string s = _aimPropPath [0].UiPropInfo ().Caption;
                    PropNamePath = _aimPropPath [0].Name;
                    for (int i = 1; i < _aimPropPath.Count; i++)
                    {
                        string tmp = _aimPropPath [i].UiPropInfo ().Caption;
                        if (!string.IsNullOrWhiteSpace (tmp))
                            s += " > " + tmp;

                        PropNamePath += "/" + _aimPropPath [i].Name;
                    }
                    DisplayPropertyName = s;
                }
            }
        }

        public string DisplayPropertyName { get; private set; }

        public string PropNamePath { get; private set; }

        public override string ToString ()
        {
            return DisplayPropertyName;
        }

        public esriGeometryType EsriGeomType { get; set; }

        private List<AimPropInfo> _aimPropPath;
    }
}
