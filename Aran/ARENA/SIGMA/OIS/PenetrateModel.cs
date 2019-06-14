using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OIS
{
	public class PenetrateModel
	{
		public string Name;
		public double Distance;
		public double Gradient;
		public PDM.VerticalStructure Obstacle;

		public PenetrateModel ( string name, double dist, double gradient, PDM.VerticalStructure pdmObj)
		{
			Name = name;
			Distance = dist;
			Gradient = gradient;
			Obstacle = pdmObj;
		}
	}
}
