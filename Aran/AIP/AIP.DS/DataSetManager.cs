using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AIP.DataSet.Classes;
using AIP.DataSet.Lib;
using AIP.DataSet.Properties;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.Temporality.CommonUtil.Util;
using Telerik.WinControls;
using Telerik.WinControls.Enumerations;
using Telerik.WinControls.UI;

namespace AIP.DataSet
{
    public partial class DataSetManager : Telerik.WinControls.UI.RadForm
    {
        private FeatureType selected;
        private List<string> IgnoreProps = new List<string>() { "Id", "Identifier", "TimeSlice" };
        private int MaxLevel = 5;

        public DataSetManager()
        {
            InitializeComponent();
        }

        private void DataSetManager_Load(object sender, EventArgs e)
        {
            //lbFeatures.DataSource = Enum.GetNames(typeof(FeatureType)).OrderBy(x=>x);
            //var test = AimMetadata.GetClassInfoByIndex((int)selected);
            cbx_DataSet.ValueMember = "Key";
            cbx_DataSet.DisplayMember = "Value";
            cbx_DataSet.DataSource = new BindingSource(LoadDataSetFeatures(), null);
            radPanel1.PanelElement.PanelBorder.Visibility = ElementVisibility.Collapsed;
            //LoadFeatures();
        }

        private Dictionary<string, FeatureType> LoadDataSetFeatures()
        {
            try
            {
                Dictionary<string, FeatureType> tmp = new Dictionary<string, FeatureType>();
                tmp.Add("ASE",FeatureType.Airspace);
                tmp.Add("RTE",FeatureType.Route);
                tmp.Add("DPN",FeatureType.DesignatedPoint);
                tmp.Add("AHP",FeatureType.AirportHeliport);
                tmp.Add("RWY",FeatureType.Runway);
                tmp.Add("TLA",FeatureType.TouchDownLiftOff);
                tmp.Add("NAV",FeatureType.Navaid);
                tmp.Add("AGL",FeatureType.AeronauticalGroundLight);
                tmp.Add("HPT",FeatureType.HoldingPattern);
                return tmp;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        private void LoadFeatures()
        {
            try
            {
                tvProps.Nodes.Clear();
                //List<string> listFeat = Enum.GetNames(typeof(FeatureType)).OrderBy(x => x).ToList();
                //List<string> listFeat = new List<string>(){"Airspace"};
                List<string> listFeat = new List<string>() { ((KeyValuePair<string, FeatureType>)cbx_DataSet.SelectedItem).Value.ToString()};
                for (int i = 0; i < listFeat.Count; i++)
                {
                    RadTreeNode rt = CreateNode(listFeat[i]);
                    rt.Expand();
                    //tvProps.Nodes.Add(rt);
                    FeatureType ftype = (FeatureType)Enum.Parse(typeof(FeatureType), listFeat[i]);
                    LoadFeature(ftype, tvProps.Nodes[i], 0);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private RadTreeNode CreateNode(string text, RadTreeNode parentNode = null, string name = null, Attributes attr = null)
        {
            try
            {
                RadTreeNode newNode = new RadTreeNode()
                {
                    Text = text,
                    Name = name ?? text
                };

                if(attr != null)
                    newNode.Tag = attr.ToTag();
                if (parentNode == null)
                    tvProps.Nodes.Add(newNode);
                else
                    parentNode.Nodes.Add(newNode);

                return newNode;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        private void LoadFeature(FeatureType ftype, RadTreeNode node, int level)
        {
            try
            {
                if (level > MaxLevel || ftype == 0) return;
                RadTreeNode rtn;
                AimClassInfo props = AimMetadata.GetClassInfoByIndex((int)ftype);
                List<AimPropInfo> tmp = props?.Properties?.Where(x => !IgnoreProps.Contains(x.Name)).ToList();
                if (tmp != null)
                {

                    foreach (AimPropInfo prop in tmp)
                    {
                        if (prop.IsFeatureReference)
                        {
                            rtn = CreateNode(prop.Name + " (xlink:href -> " + prop.ReferenceFeature + ")", node, prop.Name);
                            //rtn = new RadTreeNode(prop.Name + " (xlink:href -> " + prop.ReferenceFeature + ")");
                            //node.Nodes.Add(rtn);
                        }
                        else if (prop.IsList)
                        {
                            rtn = CreateNode(prop.Name + " (List of " + prop.PropType.Name + ")", node, prop.Name);
                            //node.Nodes.Add(rtn);
                            LoadComplexType(prop, rtn, ++level);
                        }
                        else
                        {
                            rtn = CreateNode(prop.Name, node);
                            //node.Nodes.Add(rtn);
                        }

                        
                        if (prop.IsFeatureReference)
                        {
                            LoadFeature(prop.ReferenceFeature, rtn, ++level);
                        }
                    }

                }
                //Console.WriteLine(ftype);
                foreach (FeatureType sub in Common.FeatureRefMe.FirstOrDefault(x => x.Key == ftype).Value)
                {
                    rtn = new RadTreeNode("" + sub);
                    rtn = CreateNode("<< " + sub, node, sub.ToString());
                    //node.Nodes.Add(rtn);
                    LoadFeature(sub, rtn, level);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void LoadComplexType(AimPropInfo props, RadTreeNode node, int level)
        {
            try
            {
                if (level > MaxLevel) return;
                List<AimPropInfo> tmp = props.PropType.Properties.Where(x => !IgnoreProps.Contains(x.Name)).ToList();
                foreach (AimPropInfo prop in tmp)
                {
                    RadTreeNode rtn;
                    if (prop.IsFeatureReference)
                    {
                        //rtn = new RadTreeNode(prop.Name + " (xlink:href -> " + prop.ReferenceFeature + ")");
                        rtn = CreateNode(prop.Name + " (xlink:href -> " + prop.ReferenceFeature + ")", node, prop.Name);
                    }
                    else if (prop.IsList)
                    {
                        //rtn = new RadTreeNode(prop.Name + " (List of " + prop.PropType.Name + ")");
                        rtn = CreateNode(prop.Name + " (List of " + prop.PropType.Name + ")", node, prop.Name);
                        LoadComplexType(prop, rtn, ++level);
                    }
                    else
                    {
                        rtn = CreateNode(prop.Name, node);
                    }

                    //node.Nodes.Add(rtn);
                    if (prop.IsFeatureReference)
                    {
                        LoadFeature(prop.ReferenceFeature, rtn, ++level);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }
        

        private void tvProps_NodeCheckedChanged(object sender, TreeNodeCheckedEventArgs e)
        {
            try
            {
                CheckTreeViewNode(e.Node, e.Node.Checked);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void CheckTreeViewNode(RadTreeNode node, Boolean isChecked)
        {
            try
            {
                foreach (RadTreeNode item in node.Nodes)
                {
                    item.Checked = isChecked;

                    if (item.Nodes.Count > 0)
                    {
                        this.CheckTreeViewNode(item, isChecked);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void btn_SaveXml_Click(object sender, EventArgs e)
        {
            try
            {
                string key = ((KeyValuePair<string, FeatureType>)cbx_DataSet.SelectedItem).Key;
                tvProps.SaveXML($"AipDataSet.Config.{key}.xml");
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void btn_OpenXml_Click(object sender, EventArgs e)
        {
            try
            {
                string key = ((KeyValuePair<string, FeatureType>)cbx_DataSet.SelectedItem).Key;
                tvProps.LoadXML($"AipDataSet.Config.{key}.xml");
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void cbx_DataSet_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadFeatures();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void btn_Exit_Click(object sender, EventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }
    }
}
