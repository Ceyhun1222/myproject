using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Aran.AranEnvironment;
using Aran.Geometries;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Aran.Panda.Common;

namespace Aran.Panda.VisualManoeuvring.Forms
{
    public partial class VisualFeatureCreator : Form
    {
        AranTool pickTargetPnt;

        FormHelpers.VisualFeatureCreatorHelper helper; 

        String[] LonSubFix = new String[] { "E", "W" };
        String[] LatSubFix = new String[] { "N", "S" };

        Point newPntPrj;        
        Point newPntGeo;

        double newPntX;
        double newPntY;

        double newPntLatDeg;
        double newPntLatMin;
        double newPntLatSec;

        double newPntLonDeg;
        double newPntLonMin;
        double newPntLonSec;

        int newPntLatSign;
        int newPntLonSign;

        MF_Page1 mf_p1;

        MainForm mainForm;

        public VisualFeatureCreator(MainForm mf, MF_Page1 mf_p1)
        {
            InitializeComponent();
            helper = new FormHelpers.VisualFeatureCreatorHelper();
            pickTargetPnt = new AranTool();
            pickTargetPnt.Visible = true;
            pickTargetPnt.Cursor = Cursors.Cross;
            pickTargetPnt.MouseClickedOnMap += new MouseClickedOnMapEventHandler(pickNewPnt_Click);
            GlobalVars.gAranEnv.AranUI.AddMapTool(pickTargetPnt);
            cmbBox_newPntLatSide.Items.Add(LatSubFix[0]);
            cmbBox_newPntLatSide.Items.Add(LatSubFix[1]);
            cmbBox_newPntLonSide.Items.Add(LonSubFix[0]);
            cmbBox_newPntLonSide.Items.Add(LonSubFix[1]);
            this.mf_p1 = mf_p1;
            this.mainForm = mf;
        }

        private void btn_pickNewPnt_Click(object sender, EventArgs e)
        {
            GlobalVars.gAranEnv.AranUI.SetCurrentTool(this.pickTargetPnt);
        }

        private void pickNewPnt_Click(object sender, MapMouseEventArg args)
        {
            double xDeg;
            double xMin;
            double xSec;

            double yDeg;
            double yMin;
            double ySec;

            newPntX = args.X;
            newPntY = args.Y;
            newPntPrj = new Point();

            newPntPrj.X = newPntX;
            newPntPrj.Y = newPntY;            

            newPntGeo = GlobalVars.pspatialReferenceOperation.ToGeo<Point>(newPntPrj);
            newPntX = newPntGeo.X;
            newPntY = newPntGeo.Y;

            Functions.DD2DMS(newPntX, out xDeg, out xMin, out xSec, System.Math.Sign(newPntX));
            Functions.DD2DMS(newPntY, out yDeg, out yMin, out ySec, System.Math.Sign(newPntY));

            txtBox_newPntLatDegree.Text = yDeg.ToString();
            txtBox_newPntLatMinute.Text = yMin.ToString();
            txtBox_newPntLatSecond.Text = System.Math.Round(ySec, 2).ToString();
            if (System.Math.Sign(newPntY) < 0)
                cmbBox_newPntLatSide.SelectedIndex = 1;
            else
                cmbBox_newPntLatSide.SelectedIndex = 0;

            txtBox_newPntLonDegree.Text = xDeg.ToString();
            txtBox_newPntLonMinute.Text = xMin.ToString();
            txtBox_newPntLonSecond.Text = System.Math.Round(xSec, 2).ToString();

            if (System.Math.Sign(newPntX) < 0)
                cmbBox_newPntLonSide.SelectedIndex = 1;
            else
                cmbBox_newPntLonSide.SelectedIndex = 0;
        }

        private void btn_addPnt_Click(object sender, EventArgs e)
        {
            bool isFormValid = true;

            if (!double.TryParse(txtBox_newPntLatDegree.Text, out newPntLatDeg))
                isFormValid = false;
            if (!double.TryParse(txtBox_newPntLatMinute.Text, out newPntLatMin))
                isFormValid = false;
            if (!double.TryParse(txtBox_newPntLatSecond.Text, out newPntLatSec))
                isFormValid = false;
            if (!double.TryParse(txtBox_newPntLonDegree.Text, out newPntLonDeg))
                isFormValid = false;
            if (!double.TryParse(txtBox_newPntLonMinute.Text, out newPntLonMin))
                isFormValid = false;
            if (!double.TryParse(txtBox_newPntLonSecond.Text, out newPntLonSec))
                isFormValid = false;
            if (txtBox_newPntName.Text.Equals(""))
                isFormValid = false;
            if (txtBox_newPntDescription.Text.Equals(""))
                isFormValid = false;

            if (isFormValid)
            {
                if (cmbBox_newPntLatSide.SelectedIndex == 1)
                    newPntLatSign = -1;
                else
                    newPntLatSign = 1;

                if (cmbBox_newPntLonSide.SelectedIndex == 1)
                    newPntLonSign = -1;
                else
                    newPntLonSign = 1;

                double gNewPntY = Functions.DMS2DD(newPntLatDeg, newPntLatMin, newPntLatSec, newPntLatSign);
                double gNewPntX = Functions.DMS2DD(newPntLonDeg, newPntLonMin, newPntLonSec, newPntLonSign);

                Point newgPnt = new Point();
                newgPnt.SetCoords(gNewPntX, gNewPntY);
                newgPnt.Z = 0;
                //DBModule.CreateVisualFeature(txtBox_newPntName.Text, txtBox_newPntDescription.Text, newgPnt);
                //DBModule.InsertVisualFeatures();                
                VM_VisualFeature visualFeature = new VM_VisualFeature();
                visualFeature.Name = txtBox_newPntName.Text;
                visualFeature.Description = txtBox_newPntDescription.Text;
                visualFeature.gShape = newgPnt;
                visualFeature.pShape = GlobalVars.pspatialReferenceOperation.ToPrj<Point>(newgPnt);
                VMManager.Instance.AllVisualFeatures.Add(visualFeature);
                if (mf_p1.Visible)
                {
                    mf_p1.pageHelper.findVFsWithinDivergenceVFSelectionPoly();
                    mf_p1.cmbBox_DivergenceVF_SelectedIndexChanged(this, null);
                }
                MessageBox.Show("Done!");
                helper.DeleteAllVisualFeaturesDrawings();
                helper.DrawAllVisualFeatures();
                this.Close();
            }
            else
                MessageBox.Show("Please, fill in the form completely");
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_loadVFFromFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.FileName = null;

            // Display the openFile dialog.
            DialogResult result = openFileDialog.ShowDialog();

            // OK button was pressed. 
            if (result == DialogResult.OK)
            {
                try
                {
                    using (var sr = File.OpenText(openFileDialog.FileName))
                    {
                        var str = sr.ReadToEnd();
                        JObject obj = JsonConvert.DeserializeObject<JObject>(str);

                        var jArr = obj["visualFeatures"].Value<JArray>();
                        foreach (JObject jObj in jArr)
                        {
                            var newItem = new VM_VisualFeature();
                            newItem.FromJson(jObj);
                            VMManager.Instance.AllVisualFeatures.Add(newItem);                            
                        }

                        VMManager.Instance.isAllVisualFeaturesVisible = true;
                        helper.DeleteAllVisualFeaturesDrawings();
                        helper.DrawAllVisualFeatures();

                        if (mf_p1.Visible)
                        {
                            mf_p1.pageHelper.findVFsWithinDivergenceVFSelectionPoly();
                            mf_p1.cmbBox_DivergenceVF_SelectedIndexChanged(this, null);
                        }
                    }
                }
                catch (Exception exp)
                {
                    MessageBox.Show("An error occurred while attempting to load the file. The error is:"
                                    + System.Environment.NewLine + exp.ToString() + System.Environment.NewLine);
                }
                Invalidate();
            }

            // Cancel button was pressed. 
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }

        private void btn_saveVFsToFile_Click(object sender, EventArgs e)
        {
            if (VMManager.Instance.AllVisualFeatures.Count == 0)
            {
                MessageBox.Show("Sorry, there is no visual feature to be saved.");
                return;
            }

            Stream myStream;
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "txt files (*.json)|*.json";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog.OpenFile()) != null)
                {
                    JArray jsonArray = new JArray();
                    foreach (VM_VisualFeature visualFeature in VMManager.Instance.AllVisualFeatures)
                    {
                        JObject obj = visualFeature.ToJson();
                        if (obj != null)
                            jsonArray.Add(obj);
                    }

                    JObject tempObj = new JObject(/*new JProperty("effectiveDate", DBModule.pObjectDir.TimeSlice.EffectiveDate),
                        new JProperty("airportHeliportDesignator", GlobalVars.CurrADHP.pAirportHeliport.Designator),
                        new JProperty("finalNavaidName", VMManager.Instance.FinalNavaid.Name),
                        new JProperty("selectedRWYName", VMManager.Instance.SelectedRWY.Name),*/
                        new JProperty("visualFeatures", jsonArray));
                    StreamWriter writer = new StreamWriter(myStream);

                    string str = JsonConvert.SerializeObject(tempObj);
                    writer.WriteLine(str);
                    writer.Close();
                    myStream.Close();
                    MessageBox.Show("Done!");
                    this.Close();
                }
            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void VisualFeatureCreator_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm.isVisualFeatureCreaterOpen = false;
        }
    }
}
