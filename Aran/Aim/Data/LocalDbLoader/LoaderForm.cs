using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Aran.Aim.Enums;
using Aran.Aim.Features;

namespace Aran.Aim.Data.LocalDbLoader
{
    public partial class LoaderForm : Form
    {
        //private CacheInfo _info;
        private List<FeatureType> _featureTypes;

        public LoaderForm()
        {
            InitializeComponent();

            //_info = new CacheInfo();
            _featureTypes = new List<FeatureType>();
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var localDbPro = GetLocalDbProvider();
            if (localDbPro != null)
            {
                var featList = localDbPro.GetAllStoredFeatTypes().GetListAs<FeatureType>();
                _featureTypes.AddRange(featList);
                FillList(_featureTypes, false);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Global.LoaderForm = null;
        }

        private void FillList(IEnumerable<FeatureType> featureTypes, bool isChecked)
        {
            foreach (var featType in featureTypes)
            {
                var lvi = ui_featTypeLV.Items.Add(featType.ToString());
                lvi.Tag = featType;
                lvi.Checked = isChecked;
            }
        }

        private void AddFeatureType_Click(object sender, EventArgs e)
        {
            var fts = new FeatureTypeSelectorForm();
            fts.FeatureTypeList = new ReadOnlyCollection<FeatureType>(_featureTypes);

            if (fts.ShowDialog() != DialogResult.OK)
                return;

            _featureTypes.AddRange(fts.FeatureTypeList);

            FillList(fts.FeatureTypeList, true);
        }

        private void RemoveFeatureType_Click(object sender, EventArgs e)
        {
            if (ui_featTypeLV.CheckedItems.Count == 0)
                return;

            var mbRes = MessageBox.Show("Do you want to remove the selected items?", "Remove Feature Type", 
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (mbRes != DialogResult.Yes)
                return;

            var checkedItems = ui_featTypeLV.CheckedItems.Cast<ListViewItem>();
            var removedFeatTypes = new List<FeatureType>();

            foreach (var checkedItem in checkedItems)
            {
                ui_featTypeLV.Items.Remove(checkedItem);
                removedFeatTypes.Add((FeatureType)checkedItem.Tag);
            }

            var localDbPro = GetLocalDbProvider();
            if (localDbPro != null)
            {
                foreach(var featType in removedFeatTypes)
                    localDbPro.RemoveTable(featType);
            }
        }

        private void FeatureType_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            var b = (ui_featTypeLV.CheckedItems.Count > 0);
            ui_removeTSB.Enabled = b;
            ui_reloadTSB.Enabled = b;
        }

        private void ReloadChecked_Click(object sender, EventArgs e)
        {
            var featTypes = new List<FeatureType>();
            foreach (ListViewItem checkedItem in ui_featTypeLV.CheckedItems)
                featTypes.Add((FeatureType)checkedItem.Tag);
            ReloadFeature(featTypes);
        }

        private void ReloadAll_Click(object sender, EventArgs e)
        {
            var featTypes = new List<FeatureType>();
            foreach (ListViewItem checkedItem in ui_featTypeLV.Items)
                featTypes.Add((FeatureType)checkedItem.Tag);
            ReloadFeature(featTypes);
        }

        private void ReloadFeature(IEnumerable<FeatureType> featureTypes)
        {
            var cawDbPro = Global.AranEnv.DbProvider as CawDbProvider;
            var localDbPro = cawDbPro.CacheDbProvider as Local.LocalDbProvider;

            var thread = new Thread(ReloadFeatureThread);
            thread.Start(new object[] {
                featureTypes, 
                cawDbPro,
                localDbPro
            });

            //if (localDbPro == null)
            //{
            //    var docFileName = Global.AranEnv.DocumentFileName;
            //    var fileName = Path.GetFileNameWithoutExtension(docFileName);
            //    var localDbFileName = Path.Combine(Path.GetDirectoryName(docFileName), fileName + ".cache");

            //    if (File.Exists(localDbFileName))
            //        File.Delete(localDbFileName);

            //    localDbPro = new Local.LocalDbProvider();
            //    localDbPro.Open(localDbFileName);

            //    var strExtData = new Aran.AranEnvironment.StringExtData();
            //    strExtData.Value = localDbFileName;
            //    Global.AranEnv.PutExtData("cache-db-path", strExtData);
            //}

            //cawDbPro.CacheDbProvider = null;

            //var gr = cawDbPro.GetVersionsOf(featureType, TimeSliceInterpretationType.BASELINE);

            //if (!gr.IsSucceed)
            //{
            //    ShowMessage("Error on load:\n" + gr.Message);
            //    cawDbPro.CacheDbProvider = localDbPro;
            //    return;
            //}

            //var featList = gr.GetListAs<Feature>();

            //var ir = localDbPro.ReInsert(featureType, featList);

            //if (!ir.IsSucceed)
            //{
            //    ShowMessage("Error on insert:\n" + ir.Message);
            //    cawDbPro.CacheDbProvider = localDbPro;
            //    return;
            //}

            //cawDbPro.CacheDbProvider = localDbPro;
        }

        private void ReloadFeatureThread(object param)
        {
            var objArr = (object[])param;
            var featureTypes = objArr[0] as IEnumerable<FeatureType>;
            var cawDbPro = objArr[1] as CawDbProvider;
            var localDbPro = objArr[2] as Local.LocalDbProvider;

            if (localDbPro == null)
            {
                var docFileName = Global.AranEnv.DocumentFileName;
                var fileName = Path.GetFileNameWithoutExtension(docFileName);
                var localDbFileName = Path.Combine(Path.GetDirectoryName(docFileName), fileName + ".cache");

                if (File.Exists(localDbFileName))
                    File.Delete(localDbFileName);

                localDbPro = new Local.LocalDbProvider();
                localDbPro.Open(localDbFileName);

                var strExtData = new Aran.AranEnvironment.StringExtData();
                strExtData.Value = localDbFileName;
                Global.AranEnv.PutExtData("cache-db-path", strExtData);
            }

            cawDbPro.CacheDbProvider = null;

            foreach (var featureType in featureTypes)
            {
                Invoke(new Action(() => ShowBusyIndicator(featureType)));

                var gr = cawDbPro.GetVersionsOf(featureType, TimeSliceInterpretationType.BASELINE);

                if (!gr.IsSucceed)
                {
                    Invoke(new Action(() => ShowMessage(string.Format("Error on load {0}\n   Details: {1}", featureType.ToString(), gr.Message))));
                    cawDbPro.CacheDbProvider = localDbPro;
                    return;
                }

                var featList = gr.GetListAs<Feature>();

                var ir = localDbPro.ReInsert(featureType, featList);

                if (!ir.IsSucceed)
                {
                    Invoke(new Action(() => ShowMessage(string.Format("Error on insert {0}\n   Details:{1}", featureType.ToString(), ir.Message))));
                    cawDbPro.CacheDbProvider = localDbPro;
                    return;
                }
            }

            cawDbPro.CacheDbProvider = localDbPro;

            Invoke(new Action(() => ShowBusyIndicator((FeatureType)0)));
        }

        private void ShowMessage(string message)
        {
            ui_messagePanel.Visible = true;
            ui_messagesTB.AppendText(message + "\r\n");
        }

        private Local.LocalDbProvider GetLocalDbProvider()
        {
            var cawDbPro = Global.AranEnv.DbProvider as CawDbProvider;
            return cawDbPro.CacheDbProvider as Local.LocalDbProvider;
        }

        private void Close_Click(object sender, EventArgs e)
        {
            ui_messagePanel.Visible = false;
        }

        private void ShowBusyIndicator(FeatureType featType)
        {
            ui_loadingLabel.Text = string.Format("{0} is loading...", featType.ToString());
            ui_loadingPanel.Visible = (featType != 0);
        }

    }
}
