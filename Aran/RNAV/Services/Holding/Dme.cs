using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Holding
{
	public abstract class  FeatureBase
	{
		public string Desinator { get; set; }
		public string Name { get; set; }
		public int Uid { get; set; }
		public ARAN.GeometryClasses.Point Point { get; set; }
	}
	public class DmePanda:FeatureBase
	{
		
	}

	public class VorPanda:FeatureBase
	{
		
	}

}
