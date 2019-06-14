using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Geometries;

namespace Aran.PANDA.RNAV.SGBAS
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public enum eSBASCat
	{
		CategoryI = 0,
		APVI = 1,
		APVII = 2
	}

	public struct ProfilePoint
	{
		public double X;
		public double Y;		//Z
		public double Course;
		public double PDG;
		public Aran.Aim.Enums.CodeProcedureDistance Role;
	}
}
