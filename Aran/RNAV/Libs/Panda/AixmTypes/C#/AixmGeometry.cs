using System;
using System.Collections.Generic;
using System.Text;
using ARAN.Common;
using ARAN.GeometryClasses;

namespace ARAN.AIXMTypes
{
	public abstract class AIXMGeometry : AIXM
	{
		public void Assign(AIXM source)
		{
			AIXMGeometry AIXMGeometry;
			base.Assign(source);
			AIXMGeometry = (AIXMGeometry)source;
			_geoGeometry.Assign(AIXMGeometry._geoGeometry);
			_prjGeometry.Assign(AIXMGeometry._prjGeometry);
		}

		public override void Pack(int handle)
		{
			base.Pack(handle);
			_geoGeometry.Pack(handle);
			_prjGeometry.Pack(handle);
		}

		public override void UnPack(int handle)
		{
			base.UnPack(handle);
			_geoGeometry.UnPack(handle);
			_prjGeometry.UnPack(handle);
		}


		public Geometry getGeo()
		{
			return _geoGeometry;
		}

		public Geometry getPrj()
		{
			return _prjGeometry;
		}

		protected AIXMGeometry(AIXMType objType, Geometry geo, Geometry prj)
			: base(objType)
		{

			_geoGeometry = geo;
			_prjGeometry = prj;

		}

		protected Geometry _geoGeometry;
		protected Geometry _prjGeometry;
	}
}
