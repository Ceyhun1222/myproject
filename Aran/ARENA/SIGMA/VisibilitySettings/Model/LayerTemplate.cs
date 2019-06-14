using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisibilityTool.Model
{
    public class LayerTemplate
    {
        public LayerTemplate()
        {
            RelatedLayers = new ObservableCollection<RefLayer>();
        }

        public string GroupByField
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string PrimaryTableName
        {
            get;
            set;
        }

        public string DescriptField
        {
            get;
            set;
        }

        public string IdField
        {
            get;
            set;
        }

        public ObservableCollection<RefLayer> RelatedLayers
        {
            get;
            set;
        }

        public bool CanSplitLayers { get; set; }
        public string SplittedLayerName { get; set; }

        public ObservableCollection<string> SplittedLayers
        {
            get
            {
                ObservableCollection<string> resList = new ObservableCollection<string>();
                resList.Add("All");
                resList.Add(SplittedLayerName);
                foreach(var lay in RelatedLayers)
                    if (!string.IsNullOrEmpty(lay.SplittedLayerName))
                        resList.Add(lay.SplittedLayerName);
                return resList;
            }
        }

        public int ApplyRuleOn
        {
            get;
            set;
        }

    }
}