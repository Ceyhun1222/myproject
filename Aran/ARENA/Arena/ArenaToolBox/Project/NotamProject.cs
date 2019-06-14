using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows.Forms;
using ARENA.Environment;
using ARENA.Util;
using ESRI.ArcGIS.Carto;
using EsriWorkEnvironment;
using PDM;
using Timer = System.Windows.Forms.Timer;
using ARENA.Enums_Const;

namespace ARENA.Project
{
    public class NotamProject : AbstractProject
    {
        public NotamProject(Environment.Environment environment) : base(environment)
        {
        }

        private Timer _timer;

        private void InitTimer()
        {
            if (_timer != null) return;

            _timer = new Timer
                         {
                             Interval = 500,
                         };
            _timer.Tick += OnNextAnimationFrame;
            _timer.Enabled = true;
        }

        private void OnNextAnimationFrame(object sender, EventArgs e)
        {
            if (Environment.FeatureTreeView.Nodes.Count==0) return;

            var selectedIndex = 0;
            for (; selectedIndex < Environment.FeatureTreeView.Nodes.Count; selectedIndex++)
            {
                if (Environment.FeatureTreeView.Nodes[selectedIndex].Checked) break;
            }

            //uncheck old
            if (Environment.FeatureTreeView.Nodes[selectedIndex].Checked)
            {
                Environment.FeatureTreeView.Nodes[selectedIndex].Checked = false;
            }

            selectedIndex = (selectedIndex + 1)%Environment.FeatureTreeView.Nodes.Count;
            //check new
            Environment.FeatureTreeView.Nodes[selectedIndex].Checked = true;

        }

        private void OnAnimationAction(bool playAnimation)
        {
            InitTimer();
            _timer.Enabled = playAnimation; 
        }

      
        public static Action<Environment.Environment> RunNotamConfig { get; set; }

        private static string DateFormat = "dd/MM/yyyy HH:mm";

        private readonly Dictionary<string, HashSet<string>> _visibleObjects = new Dictionary<string, HashSet<string>>();

        private void UpdateObjectVisibility(PDMObject pdmObject, bool visible)
        {
            if (pdmObject is Airspace)
            {
                var airspace = pdmObject as Airspace;
                if (airspace.AirspaceVolumeList==null) return;
                foreach (var volume in airspace.AirspaceVolumeList)
                {
                    UpdateObjectVisibility(volume, visible);
                }
            }
            if (!(pdmObject is AirspaceVolume)) return;

            var featureType = pdmObject.GetType().Name;
            HashSet<string> ids;
            if (!_visibleObjects.TryGetValue(featureType,out ids))
            {
                ids = new HashSet<string>();
                _visibleObjects[featureType] = ids;
            }

            if (visible)
            {
                ids.Add(pdmObject.ID);
            }
            else
            {
                ids.Remove(pdmObject.ID);
            }
        }

        private void UpdateObjectsVisibility()
        {
            foreach (var pair in _visibleObjects)
            {
                var name = pair.Key;
                var layer = EsriUtils.getLayerByName(Environment.pMap, name);
                var filter = layer as IFeatureLayerDefinition;
                if (filter==null) continue;

                HashSet<string> ids=pair.Value;
                if (ids.Count==0)
                {
                    filter.DefinitionExpression = "FeatureGUID = 'no'";
                }
                else
                {
                    string query = null;
                    foreach (var id in ids)
                    {
                        if (query==null)
                        {
                           query= "FeatureGUID = '" +id+"'";
                        }
                        else
                        {
                            query+=" or FeatureGUID = '" +id+"'";
                        }
                    }

                    filter.DefinitionExpression = query;

                }
            }
        }

        #region Overrides of AbstractProject

        public override ArenaProjectType ProjectType
        {
            get { return ArenaProjectType.NOTAM; }
        }

        #endregion


        //var l = airspaceLayer as IFeatureLayerDefinition;
        //l.DefinitionExpression = "[designator] LIKE 'E*'";



        public override void OnCreate()
        {
            Environment.PlayButton.Enabled = true;
            //MainForm.Instance.OnAnimationAction = OnAnimationAction;
           

            if (RunNotamConfig != null)
            {
                RunNotamConfig(Environment);
            }

            if (PdmObjectList.Count > 0) FillObjectsTree();

            //zoom to layer
            Environment.ZoomToLayerByName("AirspaceVolume");
        }

      

        public override void TreeViewAfterCheck(object sender, TreeViewEventArgs e)
        {
            base.TreeViewAfterCheck(sender, e);
            var node = e.Node;
            if (node.Tag is TreeNodeHandler)
            {
                (node.Tag as TreeNodeHandler).OnChecked(node);
            }
            else
            {
                UpdateFeatureVisibility();
            }
        }

        private bool _updateAllowed;
        private void UpdateFeatureVisibility()
        {
            if (!_updateAllowed) return;

            foreach (TreeNode node in FeatureTreeView.Nodes)
            {
                UpdateFeatureVisibilityByNode(node);
            }

            UpdateObjectsVisibility();

            Environment.ClearGraphics();
            ((IActiveView)Environment.pMap).Refresh();
        }

        private void UpdateFeatureVisibilityByNode(TreeNode node, bool parentVisible = true)
        {
            parentVisible &= node.Checked;

            if (node.Nodes.Count > 0)
            {
                foreach (TreeNode subNode in node.Nodes)
                {
                    UpdateFeatureVisibilityByNode(subNode, parentVisible);
                }
            }
            else
            {
                if (node.Tag is PDMObject)
                {
                    var pdmObject = node.Tag as PDMObject;
                    UpdateObjectVisibility(pdmObject, parentVisible);
                }
            }
        }

      

        public override void FillObjectsTree()
        {
            _updateAllowed = false;

            FeatureTreeView.CheckBoxes = true;
            FeatureTreeView.BeginUpdate();
            FeatureTreeView.Nodes.Clear();

            
            //create date nodes
            //TODO: add group by Feature Type later
            var dateGroups=PdmObjectList.GroupBy(t => t.ActualDate);
            foreach (var dateGroup in dateGroups)
            {
                
                var dateNode = new TreeNode(dateGroup.Key.ToString(DateFormat))
                                   {
                                       Name = dateGroup.Key.ToString(DateFormat), //this should be unique
                                   };

                var dateNoteTag = new TreeNodeHandler
                                      {
                                          Node = dateNode,
                                          OnChecked = node =>
                                                          {
                                                              _updateAllowed = false;

                                                              if (node.Checked)
                                                              {
                                                                  foreach (var treeNode in FeatureTreeView.Nodes.Cast<TreeNode>().
                                                                 Where(treeNode => treeNode != node))
                                                                  {
                                                                      treeNode.Checked = false;
                                                                  }
                                                              }

                                                              _updateAllowed = true;

                                                              UpdateFeatureVisibility();
                                                          }
                                      };

                dateNode.Tag = dateNoteTag;

                dateNode.Checked = false;
                
                //create FeatureType node here
                const string featureType = "AirSpace";
                var featureTypeNode = new TreeNode(featureType)
                                          {
                                              Name = featureType,
                                              Tag = featureType,
                                              Checked = true
                                          };
                dateNode.Nodes.Add(featureTypeNode);

                foreach(var item in dateGroup.Cast<Airspace>().OrderBy(t=>t.TxtName))
                {

                    featureTypeNode.Nodes.Add(new TreeNode(item.TxtName)
                                   {
                                       Name = item.ID,
                                       Tag = item,
                                       Checked = true
                                   });
                }

                FeatureTreeView.Nodes.Add(dateNode);
            }

            if (FeatureTreeView.Nodes.Count>0)
            {
                FeatureTreeView.Nodes[0].Checked = true;
            }

            FeatureTreeView.EndUpdate();
            _updateAllowed = true;
        }

        public override void TreeViewAfterSelect(object sender, TreeViewEventArgs e)
        {
            //MessageBox.Show("NOTAM");
            ReadOnlyPropertyGrid.SelectedObject = null;
            if (e.Node.Tag is TreeNodeHandler)
            {
                var data = (e.Node.Tag as TreeNodeHandler).NodeData;
                ReadOnlyPropertyGrid.ReadOnly = !(data is Airspace);
                ReadOnlyPropertyGrid.SelectedObject = data is PDMObject ? e.Node.Tag : null;
            }
            else
            {
                var data = e.Node.Tag;
                ReadOnlyPropertyGrid.ReadOnly = !(data is Airspace);
                ReadOnlyPropertyGrid.SelectedObject = data is PDMObject ? e.Node.Tag : null;
            }

            base.TreeViewAfterSelect(sender,e);
        }
    }
}
