using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Geometries;
using Aran.PANDA.Common;
namespace Holding
{
	public class Buffer
	{
		private const double Eps = 0.00001;
		private List<ArrayPoint> circlePoints;
		private double Buf;
		private double dist1;
		private Aran.Geometries.Point lineLineIntersectPt;
		private double dist2;
		private Point circleCircleIntersectPt;

		private Buffer(double buffer)
		{
			Buf = buffer;
			circlePoints = new List<ArrayPoint>(360);

			for (int i = 0; i < 359; i++)
			{
				double iInRad = ARANMath.DegToRad(i);
				circlePoints[i].X = buffer * Math.Cos(iInRad);
				circlePoints[i].Y = buffer * Math.Sin(iInRad);
			}
		}

        /*
		private Aran.Geometries.Polygon CreateBuffer(Aran.Geometries.Polygon polygon)
		{
			Aran.Geometries.Point ptCurr, ptPrev, ptNext, ptDoublePrev, ptDoubleNext;
			double dX0, dY0, dX1, dY1, dX2, dY2;
			int count = polygon.ExteriorRing.Count;
			Ring pPoly = polygon.ExteriorRing;
			ptPrev = pPoly[count - 1];
			ptDoublePrev = pPoly[count - 2];
			int iCurr = 0;
			double L0, L1, L2 = 0;
			bool addConcavePoint, isLineLineIntersect;
            int  isCircleCircleIntersect;
			Aran.Geometries.Point First, Second, ptPrevConcave, tmpPt = null;
			double tmpDir;
            double dist3;


			//Find perpenducilar points to Line
			First = new Point();
			Second = new Point();
			ptPrevConcave = new Point();
			do
			{
				ptCurr = pPoly[iCurr];
				dX0 = ptCurr.X - ptPrev.X;
				dY0 = ptCurr.Y - ptPrev.Y;
				L0 = Math.Sqrt(dX0 * dX0 + dY0 * dY0);
				iCurr += 1;
				if (iCurr >= count)
					iCurr = 0;
			} while (L0 < Eps);

			double InvL = Buf / L0;
			dX0 = dX0 * InvL;
			dY0 = dY0 * InvL;

			int iNext = iCurr;
			int iDoubleNext = iNext + 1;

			for (int i = 0; i < count; i++)
			{
				do
				{
					ptNext = pPoly[iNext];
					dX1 = ptNext.X - ptCurr.X;
					dY1 = ptNext.Y - ptCurr.Y;
					L1 = Math.Sqrt(dX1 * dX1 + dY1 * dY1);

					iNext = iNext + 1;
					if (iNext >= count)
						iNext = 0;

				} while (L1 <= Eps);

				InvL = Buf / L1;
				dX1 = dX1 * InvL;
				dY1 = dY1 * InvL;

				iDoubleNext = iNext;

				do
				{
					ptDoubleNext = pPoly[iDoubleNext];
					dX2 = ptDoubleNext.X - ptNext.X;
					dY2 = ptDoubleNext.Y - ptNext.Y;
					L2 = Math.Sqrt(dX2 * dX2 + dY2 * dY2);

					iDoubleNext = iDoubleNext + 1;
					if (iDoubleNext >= count)
						iDoubleNext = 0;

				} while (L2 <= Eps);

				InvL = Buf / L2;
				dX2 = dX2 * InvL;
				dY2 = dY2 * InvL;

				addConcavePoint = false;

				//Current point is not Concave
				if ((ptPrev.X - ptCurr.X) * (ptCurr.Y - ptNext.Y) - (ptCurr.X - ptNext.X) * (ptPrev.Y - ptCurr.Y) <= 0)
				{
					First.X = ptCurr.X - dY0;
					First.Y = ptCurr.Y + dX0;

					//Fix first point
					//If Previus point is concave then check first point hara qoyulmalidi
					if ((ptDoublePrev.X - ptPrev.X) * (ptPrev.Y - ptCurr.Y) - (ptPrev.X - ptCurr.X) * (ptDoublePrev.Y - ptPrev.Y) > 0)
					{
						//Find perpendicular ptTmp point to prev Line

						if (tmpPt == null)
						{
							double dX = ptPrev.X - ptDoublePrev.X;
							double dY = ptPrev.Y - ptDoublePrev.Y;
							double L = Math.Sqrt(dX * dX + dY * dY);
							InvL = Buf / L;
							dX = dX * InvL;
							dY = dY * InvL;
							tmpPt = new Point();
							tmpPt.X = ptDoublePrev.X - dY;
							tmpPt.Y = ptDoublePrev.Y + dX;
						}
						else
							tmpPt = Second;
						// End

						double dirVector = Math.Atan2(ptPrev.Y - ptDoublePrev.Y, ptPrev.X - ptDoublePrev.X);
						double distance = -1;
						Aran.Geometries.Point curCircleLineIntersect = ARANFunctions.CircleVectorIntersect(ptCurr, Buf, tmpPt, dirVector, out distance);
						if (distance > -1)
						{
							tmpDir = Math.Atan2(curCircleLineIntersect.Y - tmpPt.Y, curCircleLineIntersect.Y - tmpPt.Y);
							if (Math.Abs(tmpDir - dirVector) < Eps)
							{
								isCircleLineIntersect = true;
								dist1 = ARANFunctions.ReturnDistanceInMeters(tmpPt, curCircleLineIntersect);
							}
						}

						lineLineIntersectPt = (Aran.Geometries.Point)ARANFunctions.LineLineIntersect(First, Math.Atan2(ptPrev.Y - ptCurr.Y, ptPrev.X - ptCurr.X),
										tmpPt, Math.Atan2(ptPrev.Y - ptDoublePrev.Y, ptPrev.X - ptDoublePrev.X), out isLineLineIntersect);
						if (isLineLineIntersect)
							dist2 = ARANFunctions.ReturnDistanceInMeters(tmpPt, lineLineIntersectPt);

						Aran.Geometries.Point pt1;
						isCircleCircleIntersect = ARANFunctions.FindCircleCircleIntersections(ptDoublePrev.X, ptDoublePrev.Y, Buf, ptCurr.X, ptCurr.Y, Buf, out pt1, out circleCircleIntersectPt);
						if (isCircleCircleIntersect>0)
						{
							dist3 = ARANFunctions.ReturnDistanceInMeters(tmpPt, circleCircleIntersectPt);
						}
					}
				}

			}
		}
         */ 

		public bool isCircleLineIntersect { get; set; }
	}
}
