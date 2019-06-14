//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

using Aran.Geometries;
using Aran.Panda.Common;
using Aran.Geometries.Operators;
//using Aran.Panda.Constants;
//using Aran.AranEnvironment.Symbols;

namespace Aran.PANDA.RNAV.Departure
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public class TerminationAtHeight : Straight
	{
		public TerminationAtHeight(RWYType Rwy, double initialTrack, double MocLimit, double Radius, bool bTurnBeforeDer, ADHPType Adhp, AranEnvironment.IAranEnvironment aranEnvironment)
			: base(Rwy, initialTrack, MocLimit, Radius, bTurnBeforeDer, Adhp, aranEnvironment)
		{

		}

		private bool _ensure90m;
		public bool Ensure90m
		{
			get { return _ensure90m; }
			set
			{
				_ensure90m = value;
			}
		}

		protected override void setNewDistance(double newDistance)
		{
			_TermDistance = newDistance;
			_LocalObstacles = new ObstacleType[GlobalVars.ObstacleList.Length];
			int j = -1;
			Point PtEnd = _RWY.pPtPrj[eRWY.PtEnd];

			foreach (ObstacleType obstacle in GlobalVars.ObstacleList)
			{
				double dist = ARANFunctions.Point2LineDistancePrj(obstacle.pPtPrj, PtEnd, _DepDir);
				if (dist <= _TermDistance)
				{
					_LocalObstacles[++j] = obstacle;
					_LocalObstacles[j].Height = obstacle.Height - PtEnd.Z;
				}
			}

			System.Array.Resize<ObstacleType>(ref _LocalObstacles, j + 1);
		}

		protected override void createDrawingPolygons(out MultiPolygon pFullArea, out MultiPolygon pPrimArea)
		{
			GeometryOperators geometryOperators = new GeometryOperators();

			LineString pLineStr = new LineString();
			pLineStr.Add(ARANFunctions.LocalToPrj(_ptCenter, _DepDir, _TermDistance, 50000.0));
			pLineStr.Add(ARANFunctions.LocalToPrj(_ptCenter, _DepDir, _TermDistance, -50000.0));

			Geometry geomLeft = null, geomRight = null;
			geometryOperators.Cut(_FullAreaPolygon, pLineStr, ref geomLeft, ref geomRight);

			pFullArea = (MultiPolygon)geomRight;
			pPrimArea = (MultiPolygon)geometryOperators.Intersect(pFullArea, _PrimatyAreaPolygon);
		}

	}
}
