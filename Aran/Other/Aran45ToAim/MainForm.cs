using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;
using ESRI.ArcGIS.Geometry;
using Aran.Geometries;
using Aran.Aim.Data;
using ESRI.ArcGIS;

namespace Aran45ToAixm
{
    public partial class MainForm : Form
    {
        private MainController _controller;
        private ConverterToAixm51 _converter;

        public MainForm ()
        {
            InitializeComponent ();

            _controller = new MainController ();
            _controller.ContinureIfError = true;
            _controller.FeatureInserted += Controller_FeatureInserted;
        }

        private void Form_Load (object sender, EventArgs e)
        {
        	#if ARCGIS931
        	#else
            	RuntimeManager.Bind (ProductCode.Desktop);
            #endif
        }

        private void OpenFile_Click (object sender, EventArgs e)
        {
            string fileName = ui_mdbFileNameTB.Text;
            var fileExt = System.IO.Path.GetExtension (fileName);

			if (fileExt == ".mdb")
			{
				//_converter = new Aran45Converter ();
			}
			else if (fileExt == ".xml")
				_converter = new Xml45Converter ();

            if (_converter == null)
                return;

            try
            {
                _converter.OpenFile (fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show (ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var typeList = _converter.GetFeaturesList ();

            foreach (var type in typeList)
            {
                ui_featuresChLB.Items.Add (new ComboBoxItem (type));
            }
        }

        private void SelectMDBFile_Click (object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog ();
            ofd.Filter = 
                "MS Access Files (*.mdb)|*.mdb|" + 
                "XML File (*.xml)|*.xml;*.gml";
            ofd.FileName = ui_mdbFileNameTB.Text;

            if (ofd.ShowDialog () != DialogResult.OK)
                return;

            ui_mdbFileNameTB.Text = ofd.FileName;
            OpenFile_Click (null, null);
        }

        private void ShowResultMessage (List<string> errorList, int insertedFeatureCount)
        {
            if (errorList.Count == 0)
            {
                MessageBox.Show (this, insertedFeatureCount + " Features successfuly inserterd!");
            }
            else
            {
                StringBuilder sb = new StringBuilder ();
                foreach (string item in errorList)
                    sb.AppendLine (item);

                var s = sb.ToString ();

                MessageBoxIcon mbi = MessageBoxIcon.Warning;

                if (insertedFeatureCount == 0)
                    mbi = MessageBoxIcon.Error;
                else
                {
                    s = "Some of Feature was not inserterd.\n" + s;
                }

                MessageBox.Show (this, s, Text, MessageBoxButtons.OK, mbi);
            }
        }

        private bool ThowErrorIsNotSucces (BaseDbResult res)
        {
            if (!res.IsSucceed)
            {
                MessageBox.Show (res.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private bool ShowPreInsertInfo (List<string> errorList, int featureCount)
        {
            var sb = new StringBuilder ();
            sb.Append (featureCount + " Features Converted.");

            if (errorList.Count > 0)
            {
                sb.AppendLine (errorList.Count + " errors occured.");
                sb.AppendLine ("\nDo you want to Insert Converted Features to the DB?\n");
                sb.AppendLine ("Errors:\r\n");

                foreach (var item in errorList)
                    sb.AppendLine (item);
            }

            var mbRes = MessageBox.Show (this, sb.ToString (), Text,
                (errorList.Count == 0 ? MessageBoxButtons.OK : MessageBoxButtons.YesNo),
                (errorList.Count == 0 ? MessageBoxIcon.Information : MessageBoxIcon.Warning));

            if (errorList.Count > 0 && mbRes != DialogResult.Yes)
            {
                return false;
            }

            return true;
        }

        private List<T> GetFeatures <T> () where T : Aran.Aim.Features.Feature, new ()
        {
            var t = new T ();

            var tsFilter = new Aran.Aim.Data.Filters.TimeSliceFilter (DateTime.Now);
            var propList = new List<string> ();
            propList.Add ("<LOAD_ALL_FOR_EXPORT>");

            var gr = Global.DbProvider.GetVersionsOf (t.FeatureType,
                TimeSliceInterpretationType.BASELINE, Guid.Empty, true, tsFilter, propList);

            if (!gr.IsSucceed)
                throw new Exception (gr.Message);

            return gr.List as List<T>;
        }

        private List<T> CheckIfExists<T> (List<T> featList, List<T> exFeatList, CheckIfExistsEventHandle checkHandle)
        {
            var newList = new List<T> ();

            foreach (var feat in featList)
            {
                bool isExists = false;

                foreach (var exFeat in exFeatList)
                {
                    if (checkHandle (feat as Aran.Aim.Features.Feature, exFeat as Aran.Aim.Features.Feature))
                    {
                        isExists = true;
                        break;
                    }
                }

                if (!isExists)
                {
                    newList.Add (feat);
                }
            }

            return newList;
        }

        
        private static bool IsDesignatedPointExists (Aran.Aim.Features.Feature f1, Aran.Aim.Features.Feature f2)
        {
            var dp1 = f1 as DesignatedPoint;
            var dp2 = f2 as DesignatedPoint;

            if (IsStringEqual (dp1.Designator, dp2.Designator))
                return true;

            if (IsStringEqual (dp1.Name, dp2.Name))
                return true;

            return false;
        }

        private static bool IsStringEqual (string s1, string s2)
        {
            if (s1 == null || s2 == null)
                return false;

            if (string.Compare (s1, s2, true) == 0)
                return true;

            return false;
        }

        private void FileName_TextChanged (object sender, EventArgs e)
        {
            ui_openFileButton.Enabled = (ui_mdbFileNameTB.Text.Length > 0);
        }

        private void FeaturesChLB_SelectedIndexChanged (object sender, EventArgs e)
        {
            ui_convertButton.Enabled = (ui_featuresChLB.SelectedItem != null);
        }

        private void Convert_Click (object sender, EventArgs e)
        {
            var cbi = ui_featuresChLB.SelectedItem as ComboBoxItem;
            if (cbi == null)
                return;

            var insertedCount = 0;
            var errorList = new List<string> ();

            try
            {
                Cursor = Cursors.WaitCursor;
                
                var aimFeatureList = _converter.ConvertFeatureType (cbi.Type, errorList);
                var listOfList = _converter.PostConvertType (cbi.Type, aimFeatureList, errorList);

                if (!ShowPreInsertInfo (errorList, aimFeatureList.Count))
                    return;

                ui_progressBar.Minimum = 0;
                ui_progressBar.Value = 0;
                ui_progressBar.Maximum = aimFeatureList.Count;
                ui_progressPanel.Visible = true;

                if (listOfList == null)
                {
                    errorList = _controller.InsertFeatures (aimFeatureList, out insertedCount);
                }
                else
                {
                    foreach (var list in listOfList)
                    {
                        var el = _controller.InsertFeatures (list, out insertedCount);
                        errorList.AddRange (el);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show (ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ui_progressPanel.Visible = false;
                Cursor = Cursors.Default;

                if (errorList != null)
                    ShowResultMessage (errorList, insertedCount);
            }
        }

        private void Controller_FeatureInserted (object sender, EventArgs e)
        {
            try
            {
                ui_progressBar.Value++;
            }
            catch { }
        }
    }

    internal class ComboBoxItem
    {
        public ComboBoxItem (Type type)
        {
            Type = type;
        }

        public override string ToString ()
        {
            return _name;
        }

        public Type Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
                _name = _type.Name;
                if (_name.EndsWith ("Field"))
                    _name = _name.Substring (0, _name.Length - "Field".Length);
            }
        }

        private Type _type;
        private string _name;

    }

    public delegate bool CheckIfExistsEventHandle (Aran.Aim.Features.Feature f1,Aran.Aim.Features.Feature f2);
}
