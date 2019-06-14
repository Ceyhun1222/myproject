using System;
using Aran.Geometries;
using Aran.PANDA.Common;
using System.Collections.Generic;

namespace Aran.PANDA.CRMWall
{
	public class Slicer : SlicerBase
	{
		public override void SliceGeometry(Geometry geometry, Point ptO, double dir, double val, double step, int owner, bool accumulate = true)
		{
			if (!accumulate)
				dataTable.Clear();

			if (geometry.Type == GeometryType.MultiLineString)
				SlicePolyline((MultiLineString)geometry, ptO, dir, val, step, owner, true);
			else if (geometry.Type == GeometryType.MultiPolygon)
				SlicePolygon((MultiPolygon)geometry, ptO, dir, val, step, owner, true);
		}

		public override void SlicePolygon(MultiPolygon mPolygon, Point ptO, double dir, double val, double step, int owner, bool accumulate = true)
		{
			if (!accumulate)
				dataTable.Clear();

			SpanTable ZeroLineTab = new SpanTable();

			double minY = double.MaxValue, maxY = double.MinValue;

			IETable IETPositive = new IETable();
			IETableNeg IETNegative = new IETableNeg();

			SortedArrayList[] IEtables = new SortedArrayList[] { IETPositive, IETNegative };

			foreach (Polygon ppoly in mPolygon)
			{
				#region IET filling

				IETPositive.Clear();
				IETNegative.Clear();

				int i, n = 0;

				for (int j = 0; j <= ppoly.InteriorRingList.Count; j++)
				{
					Ring ring = j == 0 ? ppoly.ExteriorRing : ppoly.InteriorRingList[j - 1];

					int m = ring.Count;
					if (m < 3)
						continue;

					n += m;
					m--;
					Edge edge = default(Edge);
					Point pi0 = ARANFunctions.PrjToLocal(ptO, dir, ring[0]);

					if (minY > pi0.Y) minY = pi0.Y;
					if (maxY < pi0.Y) maxY = pi0.Y;

					for (i = 0; i < m; i++)
					{
						Point pi1 = ARANFunctions.PrjToLocal(ptO, dir, ring[i + 1]);

						if (minY > pi1.Y) minY = pi1.Y;
						if (maxY < pi1.Y) maxY = pi1.Y;

						if (pi0.Y > pi1.Y)
						{
							if (pi0.Y < 0)
							{
								edge.v0 = pi1;
								edge.v1 = pi0;
							}
							else
							{
								edge.v0 = pi0;
								edge.v1 = pi1;
							}
						}
						else
						{
							if (pi1.Y < 0)
							{
								edge.v0 = pi0;
								edge.v1 = pi1;
							}
							else
							{
								edge.v0 = pi1;
								edge.v1 = pi0;
							}
						}

						if (Math.Abs(edge.v1.Y - edge.v0.Y) > ARANMath.EpsilonDistance)
							edge.dXdY = (edge.v0.X - edge.v1.X) / (edge.v1.Y - edge.v0.Y);
						else
							edge.dXdY = 0;

						if (edge.v0.Y < 0.0)
							IETNegative.Add(edge);
						else if (edge.v1.Y > 0.0)
							IETPositive.Add(edge);
						else
						{
							Edge edge1 = edge;
							edge1.v0 = edge.v1;

							edge.v1 = new Point();
							edge.v1.X = edge.v0.X + edge.v0.Y * edge.dXdY;
							edge.v1.Y = -ARANMath.EpsilonDistance;
							IETPositive.Add(edge);

							edge1.v1 = (Point)edge.v1;//.Clone();
													  //edge1.v1.Y = ARANMath.EpsilonDistance;
							IETNegative.Add(edge1);
						}

						pi0 = pi1;
					}

					Point p0 = ARANFunctions.PrjToLocal(ptO, dir, ring[0]);
					Point pm = ARANFunctions.PrjToLocal(ptO, dir, ring[m]);

					if (p0.Y > pm.Y)
					{
						if (p0.Y < 0)
						{
							edge.v0 = pm;
							edge.v1 = p0;
						}
						else
						{
							edge.v0 = p0;
							edge.v1 = pm;
						}
					}
					else
					{
						if (pm.Y < 0)
						{
							edge.v0 = p0;
							edge.v1 = pm;
						}
						else
						{
							edge.v0 = pm;
							edge.v1 = p0;
						}
					}

					if (Math.Abs(edge.v1.Y - edge.v0.Y) > ARANMath.EpsilonDistance)
						edge.dXdY = (edge.v0.X - edge.v1.X) / (edge.v1.Y - edge.v0.Y);
					else
						edge.dXdY = 0;

					if (edge.v0.Y < 0.0)
						IETNegative.Add(edge);
					else if (edge.v1.Y > 0.0)
						IETPositive.Add(edge);
					else
					{
						Edge edge1 = edge;
						edge1.v0 = edge.v1;

						edge.v1 = new Point();
						edge.v1.X = edge.v0.X + edge.v0.Y * edge.dXdY;
						edge.v1.Y = -ARANMath.EpsilonDistance;
						IETPositive.Add(edge);

						edge1.v1 = (Point)edge.v1;//.Clone();
												  //edge1.v1.Y = ARANMath.EpsilonDistance;
						IETNegative.Add(edge1);
					}
				}

				IETPositive.Sort();
				IETNegative.Sort();
				#endregion

				#region Polygon Partitioning

				for (int k = 0; k < 2; k++)
				{
					if (IEtables[k].Count == 0)
						continue;

					if (k == 1)
					{
						double fTmp = maxY;
						maxY = minY;
						minY = fTmp;
					}

					double interv = (maxY - minY);
					double minYL = minY;

					if (maxY * minY < 0)
					{
						interv = maxY;
						minYL = 0.0;
					}

					////==================================================
					double sign = 1.0 - 2.0 * k, signedStep = step * sign;
					int cnt = (int)Math.Ceiling(interv / signedStep);
					int TopIET = 0, LastAET = 0;
					double cy = minYL + (cnt - 1) * signedStep;

					SpanTable SpanTab = new SpanTable();
					AETable AET = new AETable();

					do
					{
						SpanTab.Clear();
						//Fill AET
						while (TopIET < IEtables[k].Count && cy * sign <= ((Edge)IEtables[k][TopIET]).v0.Y * sign)
						{
							Edge edge = (Edge)IEtables[k][TopIET];
							bool bAddToTable = edge.v1.Y * sign < cy * sign;

							double x0 = edge.v0.X, x1 = edge.v1.X;

							if (bAddToTable)
							{
								AET.Add(edge);
								LastAET++;
								x1 = x0 + edge.dXdY * (edge.v0.Y - cy);
							}

							//double dx = x0 - x1;
							//if (dx * dx > ARANMath.Epsilon_2Distance)
							SpanTab.Add(new Span(x0, x1, cy, val, owner));

							TopIET++;
						}

						i = 0;
						while (i < LastAET && LastAET > 0)
						{
							Edge tmpE = (Edge)AET[i];

							if (tmpE.v1.Y * sign >= cy * sign)
							{
								LastAET--;

								if (i < LastAET)
									AET[i] = AET[LastAET];

								AET.RemoveAt(LastAET);

								if (tmpE.v0.Y * sign > (cy + signedStep) * sign)
								{
									double x0 = tmpE.v0.X + tmpE.dXdY * (tmpE.v0.Y - cy - signedStep);
									double x1 = tmpE.v1.X;

									//double dx = x0 - x1;
									//if (dx * dx > ARANMath.Epsilon_2Distance)
									SpanTab.Add(new Span(x0, x1, cy, val, owner));
								}
							}
							else
							{
								tmpE.X = tmpE.v0.X + tmpE.dXdY * (tmpE.v0.Y - cy);
								AET[i++] = tmpE;

								if (tmpE.v0.Y * sign > (cy + signedStep) * sign)
								{
									double x0 = tmpE.v0.X + tmpE.dXdY * (tmpE.v0.Y - cy - signedStep);
									double x1 = tmpE.v0.X + tmpE.dXdY * (tmpE.v0.Y - cy);

									//double dx = x0 - x1;
									//if (dx * dx > ARANMath.Epsilon_2Distance)
									SpanTab.Add(new Span(x0, x1, cy, val, owner));
								}
							}
						}

						//==============================================================================

						if (LastAET == 0 && SpanTab.Count == 0)
							break;

						AET.Sort();

						for (i = 0; i < LastAET; i += 2)
						{
							double x0 = ((Edge)AET[i]).X, x1 = ((Edge)AET[i + 1]).X;

							SpanTab.Add(new Span(x0, x1, cy, val, owner));
						}

						SpanTab.Sort();

						Span data = default(Span);
						bool firstSpan = true;

						foreach (Span sp in SpanTab)
						{
							if (firstSpan)
							{
								data = sp;
								firstSpan = false;
								continue;
							}

							if (data.X1 < sp.X0)
							{
								if (cy == 0.0)
									ZeroLineTab.Add(data);
								else
								{
									data.p0 = ARANFunctions.LocalToPrj(ptO, dir, data.p0);
									data.p1 = ARANFunctions.LocalToPrj(ptO, dir, data.p1);
									putdata(data);
								}

								data = sp;
								continue;
							}

							if (data.X1 < sp.X1)
							{
								data.X1 = sp.X1;
								data.p1.X = sp.p1.X;
							}
						}

						if (cy == 0.0)
							ZeroLineTab.Add(data);
						else
						{
							data.p0 = ARANFunctions.LocalToPrj(ptO, dir, data.p0);
							data.p1 = ARANFunctions.LocalToPrj(ptO, dir, data.p1);
							putdata(data);
						}

						cy -= signedStep;
					} while (LastAET > 0);
				}   // negatives

				#endregion

				#region ecuatorials
				if (ZeroLineTab.Count > 0)
				{
					ZeroLineTab.Sort();

					Span data = default(Span);
					bool firstSpan = true;

					foreach (Span sp in ZeroLineTab)
					{
						if (firstSpan)
						{
							data = sp;
							firstSpan = false;
							continue;
						}

						if (data.X1 < sp.X0)
						{
							data.p0 = ARANFunctions.LocalToPrj(ptO, dir, data.p0);
							data.p1 = ARANFunctions.LocalToPrj(ptO, dir, data.p1);
							putdata(data);

							data = sp;
							continue;
						}

						if (data.X1 < sp.X1)
						{
							data.X1 = sp.X1;
							data.p1.X = sp.p1.X;
						}
					}

					data.p0 = ARANFunctions.LocalToPrj(ptO, dir, data.p0);
					data.p1 = ARANFunctions.LocalToPrj(ptO, dir, data.p1);
					putdata(data);
				}
				#endregion
			}       //end of foreach (Polygon ppoly in mPoly)
		}

		public override void SlicePolyline(MultiLineString mPolyline, Point ptO, double dir, double val, double step, int owner, bool accumulate = true)
		{
			if (!accumulate)
				dataTable.Clear();

			SpanTable SpanTab = new SpanTable();

			#region Polyline Partitioning

			int i, k = 0;
			double minY = double.MaxValue, maxY = double.MinValue;

			List<Point> ptList = new List<Point>();

			foreach (LineString ppoly in mPolyline)
			{
				int n = ppoly.Count;
				if (n < 2)
					continue;

				k += n;

				for (i = 0; i < n; i++)
				{
					Point pi = ARANFunctions.PrjToLocal(ptO, dir, ppoly[i]);
					if (pi.Y > maxY) maxY = pi.Y;
					if (pi.Y < minY) minY = pi.Y;
				}
			}

			if (k == 0)
				return;

			////==================================================
			double invStep = 1.0 / step;
			double interv = (maxY - minY);
			int cnt = (int)Math.Ceiling(interv * invStep);

			double baseLine = minY;

			if (maxY < 0)
				baseLine = maxY;
			else if (maxY * minY < 0)
				baseLine = 0.0;

			foreach (LineString ppoly in mPolyline)
			{
				int n = ppoly.Count;
				if (n < 2)
					continue;

				Point pi0 = ARANFunctions.PrjToLocal(ptO, dir, ppoly[0]);

				for (i = 1; i < n; i++)
				{
					Point pi1 = ARANFunctions.PrjToLocal(ptO, dir, ppoly[i]);
					double x0, x1, y0, y1;

					if (pi0.Y > pi1.Y)
					{
						x0 = pi0.X; x1 = pi1.X;
						y0 = pi0.Y; y1 = pi1.Y;
					}
					else
					{
						x0 = pi1.X; x1 = pi0.X;
						y0 = pi1.Y; y1 = pi0.Y;
					}

					double c0 = baseLine - Math.Ceiling((baseLine - y0) * invStep) * step;
					double cl = baseLine - Math.Ceiling((baseLine - y1) * invStep) * step;
					int m = (int)Math.Round((c0 - cl) * invStep);

					double dxdy, dx = x1 - x0, dy = y1 - y0;

					if (Math.Abs(dy) > ARANMath.Epsilon_2Distance)
						dxdy = dx / dy;
					else
						dxdy = 0.0;

					for (int j = 0; j <= m; j++)
					{
						double cy0 = c0 - j * step;
						double cy1 = cy0 + step;
						double sY = cy0, sX0, sX1;

						if (cy0 < 0) sY = cy1;

						if (y0 < cy1) sX0 = x0;
						else sX0 = x0 + dxdy * (cy1 - y0);

						if (y1 >= cy0) sX1 = x1;
						else sX1 = x1 + dxdy * (cy0 - y1);

						//dx = sX0 - sX1;
						//if (dx * dx > ARANMath.Epsilon_2Distance)
						SpanTab.Add(new Span(sX0, sX1, sY, val, owner));
					}

					pi0 = pi1;
				}

			}
			#endregion

			#region ecuatorials

			if (SpanTab.Count > 0)
			{
				SpanTab.Sort(new SpanComparer());

				Span data = default(Span);
				bool firstSpan = true;

				foreach (Span sp in SpanTab)
				{
					if (firstSpan)
					{
						data = sp;
						firstSpan = false;
						continue;
					}

					if (data.Y != sp.Y || data.X1 < sp.X0)
					{
						data.p0 = ARANFunctions.LocalToPrj(ptO, dir, data.p0);
						data.p1 = ARANFunctions.LocalToPrj(ptO, dir, data.p1);
						putdata(data);

						data = sp;
						continue;
					}

					if (data.X1 < sp.X1)
					{
						data.X1 = sp.X1;
						data.p1.X = sp.p1.X;
					}
				}

				data.p0 = ARANFunctions.LocalToPrj(ptO, dir, data.p0);
				data.p1 = ARANFunctions.LocalToPrj(ptO, dir, data.p1);
				putdata(data);
			}
			#endregion
		}

		class SpanComparer : System.Collections.IComparer
		{
			public int Compare(object x, object y)
			{
				Span s0 = (Span)x;
				Span s1 = (Span)y;

				double dy = s0.Y - s1.Y;
				double dyy = dy * dy;

				if (dyy < ARANMath.Epsilon_2Distance)
					return s0.X0.CompareTo(s1.X0);

				return s0.Y.CompareTo(s1.Y);
			}
		}
	}
}
