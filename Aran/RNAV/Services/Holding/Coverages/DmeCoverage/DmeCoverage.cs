using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Collections;
using Aran.Aim.Features;
using Aran.Geometries;


namespace Holding
{
    public class DmeCoverage
	{
		public MultiPolygon geom { get; set; }
		
		public List<Navaid> CovDmeList { get; set; }
		
		public bool IsValidated { get; set; }

		public string  DesCription { get; set; }
	}
}
