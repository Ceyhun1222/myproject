using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChartValidator
{
	public class SegmentPointLink
	{
		public AttributeEnum AttributeIndex;

		public List<PDMSegmentPntTree> SegmntPntTrees;

        public SegmentPointLink(AttributeEnum attributeIndex)
        {
            AttributeIndex = attributeIndex;
            SegmntPntTrees = new List<PDMSegmentPntTree>();
        }
        
		//public PDMSegmentPntTree RightItem;
		//List<PDM.Enroute> EnrouteList;
		//public List<PDM.RouteSegment> RouteSegmentList;
		//public List<SegmentPointType> TypeList;
 
		//public SegmentPointLink ()
		//{
			//LeftItem = new PDMSegmentPntTree ();
			//EnrouteList = new List<PDM.Enroute> ();
			//RouteSegmentList = new List<PDM.RouteSegment> ();
			//TypeList = new List<SegmentPointType> ();
		//}
	}
	
	public class PDMSegmentPntTree
	{
        public PDMSegmentPntTree(PDMSegmentPntTree sourceSegmntPntTree)
        {
            Item = sourceSegmntPntTree.Item;
            SegmentType = sourceSegmntPntTree.SegmentType;
        }

		public PDMSegmentPntTree (PDM.PDMObject rootItem)
		{
			Item = new TreeItem (rootItem);
		}

		public TreeItem Item;
		public SegmentPointType SegmentType;
	}

	public class TreeItem
	{
        public TreeItem(TreeItem treeItem)
        {
            Value = treeItem.Value;
            Child = treeItem.Child;
        }

		public TreeItem (PDM.PDMObject basedOn)
		{
			Value = basedOn;
		}

        public int ChildCount
        {
            get 
            {
                int result = 0;
                TreeItem child = Child;
                while (child != null)
                {
                    result++;
                    child = child.Child;
                }
                return result;
            }                       
        }

		public PDM.PDMObject Value;
		public TreeItem Child;
		
	}
		 
	public enum SegmentPointType
	{
 		Start,
		End
	}

	public enum AttributeEnum
	{
		ReportingATC,
		FlyOver
	}
}
