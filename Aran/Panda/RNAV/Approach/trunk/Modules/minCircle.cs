using Aran.Geometries;
using Aran.PANDA.Common;
using System;
using System.Collections.Generic;

namespace Aran.PANDA.RNAV.Approach
{
	[System.Runtime.InteropServices.ComVisible(false)]
	class minCircle
	{
		static bool circlePPP(Point a, Point b, Point c, out Point o, out double r)
		{
			Point ba = new Point(b.X - a.X, b.Y - a.Y);
			Point ca = new Point(c.X - a.X, c.Y - a.Y);
			double p = ba.X * ca.Y - ba.Y * ca.X;

			o = new Point();

			if (p == 0)
			{
				r = 0;
				return false;
			}

			p += p;
			double a2 = a.X * a.X + a.Y * a.Y;
			double b2 = b.X * b.X + b.Y * b.Y - a2;
			double c2 = c.X * c.X + c.Y * c.Y - a2;

			o.X = (b2 * ca.Y - c2 * ba.Y) / p;
			o.Y = (c2 * ba.X - b2 * ca.X) / p;
			r = ARANMath.Hypot(a.X - o.X, a.Y - o.Y);
			return true;
		}

		public static Ring circlePPP(Point a, Point b, Point c)
		{
			Point ba = new Point(b.X - a.X, b.Y - a.Y);
			Point ca = new Point(c.X - a.X, c.Y - a.Y);
			double p = ba.X * ca.Y - ba.Y * ca.X;

			if (p == 0)
				return new Ring();

			p += p;
			double a2 = a.X * a.X + a.Y * a.Y;
			double b2 = b.X * b.X + b.Y * b.Y - a2;
			double c2 = c.X * c.X + c.Y * c.Y - a2;

			Point o = new Point();
			o.X = (b2 * ca.Y - c2 * ba.Y) / p;
			o.Y = (c2 * ba.X - b2 * ca.X) / p;

			double r = ARANMath.Hypot(a.X - o.X, a.Y - o.Y);
			return ARANFunctions.CreateCirclePrj(o, r);
		}

		public static bool minCircleAroundPoints(List<Point> P, out Point o, out double r)
		{
			int n = P.Count;

			if (n < 3)
			{
				r = 0.0;
				if (n == 0)
				{
					o = null;
					return false;
				}

				if (n == 1)
				{
					o = (Point)P[0].Clone();
					return true;
				}

				o = new Point(0.5 * (P[0].X + P[1].X), 0.5 * (P[0].Y + P[1].Y));
				r = ARANMath.Hypot(P[0].X - P[1].X, P[0].Y - P[1].Y);
				return true;
			}

			int i, im = 0;

			double max = 0, t;
			Point p0 = P[0];

			for (i = 1; i < n; ++i)
			{
				t = ARANMath.Hypot(P[i].X - p0.X, P[i].Y - p0.Y);

				if (max < t)
				{
					max = t;
					im = i;
				}
			}

			if (im == 0)
			{
				o = p0;
				r = 0;
				return true;
			}

			int np = 2;
			int[] ip = new int[3];

			ip[0] = 0;
			ip[1] = im;

			o = new Point();
			o.X = 0.5 * (p0.X + P[im].X);
			o.Y = 0.5 * (p0.Y + P[im].Y);

			double q = 0.25 * max, s;

			for (; ; )
			{
				max = 0.0;
				for (i = 0; i < n; ++i)
				{
					t = ARANMath.Hypot(P[i].X - o.X, P[i].Y - o.Y);

					if (max < t)
					{
						max = t;
						im = i;
					}
				}

				if (max <= q || im == ip[0] || im == ip[1])
					break;

				Point pm = P[im];
				int km = 0;

				s = ARANMath.Hypot(pm.X - P[ip[0]].X, pm.Y - P[ip[0]].Y);
				t = ARANMath.Hypot(pm.X - P[ip[1]].X, pm.Y - P[ip[1]].Y);

				if (s < t)
				{
					s = t;
					km = 1;
				}

				Point v = new Point();
				if (np == 2)
				{
					s *= 0.25;
					int iTmp = ip[km];
					ip[km] = ip[0];
					ip[0] = iTmp;

					v.X = 0.5 * (pm.X + P[ip[0]].X);
					v.Y = 0.5 * (pm.Y + P[ip[0]].Y);

					if (ARANMath.Hypot(v.X - P[ip[1]].X, v.Y - P[ip[1]].Y) > s)
					{
						np = 3;
						ip[2] = im;
						circlePPP(P[ip[0]], P[ip[1]], pm, out v, out s);
					}
					else
					{
						ip[1] = im;
					}
				}
				else
				{
					if (im == ip[2])
						break;
					t = ARANMath.Hypot(pm.X - P[ip[2]].X, pm.Y - P[ip[2]].Y);

					if (s < t)
					{
						s = t;
						km = 2;
					}

					s *= 0.25;
					int iTmp = ip[km];
					ip[km] = ip[0];
					ip[0] = iTmp;

					v.X = 0.5 * (pm.X + P[ip[0]].X);
					v.Y = 0.5 * (pm.Y + P[ip[0]].Y);

					double q1 = ARANMath.Hypot(v.X - P[ip[1]].X, v.Y - P[ip[1]].Y);
					double q2 = ARANMath.Hypot(v.X - P[ip[2]].X, v.Y - P[ip[2]].Y);
					if (q1 < q2)
					{
						ip[1] = ip[2];
						q1 = q2;
					}

					if (q1 > s)
					{
						circlePPP(P[ip[0]], P[ip[1]], pm, out v, out s);
						ip[2] = im;
					}
					else
					{
						np = 2;
						ip[1] = im;
					}
				}

				if (s <= q)
					break;

				q = s;
				o = v;
			}

			r = 2 * q;          //r = Math.Sqrt(q);
			return true;
		}

		public static Ring minCircleAroundPoints(List<Point> P)
		{
			int n = P.Count;

			if (n < 3)
			{
				if (n == 0) return new Ring();
				if (n == 1) return ARANFunctions.CreateCirclePrj(P[0], 0);
				Point ptCenter = new Point(0.5 * (P[0].X + P[1].X), 0.5 * (P[0].Y + P[1].Y));

				return ARANFunctions.CreateCirclePrj(ptCenter, ARANMath.Hypot(P[0].X - P[1].X, P[0].Y - P[1].Y));
			}

			Point p0 = P[0];
			int i, im = 0;
			double max = 0, t;

			for (i = 1; i < n; ++i)
			{
				Point pI = P[i];
				t = ARANMath.Hypot(pI.X - p0.X, pI.Y - p0.Y); // qmod ( pT );

				if (max < t)
				{
					max = t;
					im = i;
				}
			}

			if (im == 0)
				return ARANFunctions.CreateCirclePrj(p0, 0);

			int np = 2;
			int[] ip = new int[3];

			ip[0] = 0;
			ip[1] = im;

			Point o = new Point();
			o.X = 0.5 * (p0.X + P[im].X);
			o.Y = 0.5 * (p0.Y + P[im].Y);

			double q = 0.25 * max, s;

			for (; ; )
			{
				max = 0;
				for (i = 0; i < n; ++i)
				{
					Point pI = P[i];
					t = ARANMath.Hypot(pI.X - o.X, pI.Y - o.Y);

					if (max < t)
					{
						max = t;
						im = i;
					}
				}

				if (max <= q || im == ip[0] || im == ip[1])
					break;

				Point pm = P[im];
				int km = 0;

				s = ARANMath.Hypot(pm.X - P[ip[0]].X, pm.Y - P[ip[0]].Y);
				t = ARANMath.Hypot(pm.X - P[ip[1]].X, pm.Y - P[ip[1]].Y);
				if (s < t)
				{
					s = t;
					km = 1;
				}

				Point v = new Point();
				if (np == 2)
				{
					s *= 0.25;
					int iTmp = ip[km];
					ip[km] = ip[0];
					ip[0] = iTmp;

					v.X = 0.5 * (pm.X + P[ip[0]].X);
					v.Y = 0.5 * (pm.Y + P[ip[0]].Y);

					if (ARANMath.Hypot(v.X - P[ip[1]].X, v.Y - P[ip[1]].Y) > s)
					{
						np = 3;
						ip[2] = im;
						circlePPP(P[ip[0]], P[ip[1]], pm, out v, out s);
					}
					else
					{
						ip[1] = im;
					}
				}
				else
				{
					if (im == ip[2]) break;
					t = ARANMath.Hypot(pm.X - P[ip[2]].X, pm.Y - P[ip[2]].Y);

					if (s < t)
					{
						s = t;
						km = 2;
					}

					s *= 0.25;
					int iTmp = ip[km];
					ip[km] = ip[0];
					ip[0] = iTmp;

					v.X = 0.5 * (pm.X + P[ip[0]].X);
					v.Y = 0.5 * (pm.Y + P[ip[0]].Y);

					double q1 = ARANMath.Hypot(v.X - P[ip[1]].X, v.Y - P[ip[1]].Y);
					double q2 = ARANMath.Hypot(v.X - P[ip[2]].X, v.Y - P[ip[2]].Y);
					if (q1 < q2)
					{
						ip[1] = ip[2];
						q1 = q2;
					}

					if (q1 > s)
					{
						circlePPP(P[ip[0]], P[ip[1]], pm, out v, out s);
						ip[2] = im;
					}
					else
					{
						np = 2;
						ip[1] = im;
					}
				}
				if (s <= q) break;
				q = s;
				o = v;
			}

			return ARANFunctions.CreateCirclePrj(o, Math.Sqrt(q));
		}
	}

}
