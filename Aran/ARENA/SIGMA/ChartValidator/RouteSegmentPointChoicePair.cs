using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDM;

namespace ChartValidator
{
	/// <summary>
	/// Choice Type and ID
	/// </summary>
	internal class SegmentPointData
	{
		public PointChoice Choice;
		public string ChoiceId;
		//public string RouteSegmentId;
		public string Description;

		public SegmentPointData (PointChoice choice, string choiceId, /*string routeSegmentId, */string description)
		{
			Choice = choice;
			ChoiceId = choiceId;
			//RouteSegmentId = routeSegmentId;
			Description = description;
		}

		public bool Equal(SegmentPointData other)
		{
			if (this.Choice == other.Choice && this.ChoiceId == other.ChoiceId)
				return true;
			return false;
		}
	}
}
