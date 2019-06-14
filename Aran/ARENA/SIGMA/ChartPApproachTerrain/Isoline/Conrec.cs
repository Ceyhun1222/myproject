using Aran.PANDA.Common;
using System;

namespace ChartPApproachTerrain
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public class Conrec
	{
		public static double NoDataValue = double.MaxValue;
		public static bool circular = false;

		delegate void CalcSec(int p1, int p2, out double x, out double y);

		public static void Contour(ISegmentsHolder ConArray, double x0, double hx, int nx,
											double y0, double hy, int ny,
											double z0, double hz, int nz,
											double[,] data)
		{
			int[] im = { 0, 1, 1, 0 };
			int[] jm = { 0, 0, 1, 1 };

			int[, ,] castab = {	{ { 0, 0, 8 }, { 0, 2, 5 }, { 7, 6, 9 } },
								{ { 0, 3, 4 }, { 1, 3, 1 }, { 4, 3, 0 } },
								{ { 9, 6, 7 }, { 5, 2, 0 }, { 8, 0, 0 } }};
			//------------------------------------------------------------------------------
			double[] xh = new double[5];
			double[] yh = new double[5];
			double[] h = new double[5];
			int[] sh = new int[5];

			double begX = 0, endX = 0, begY = 0, endY = 0;
			double Invhz = 1.0 / hz;
			z0 = Math.Floor(z0 * Invhz) * hz;
			if (!circular)
				nz++;

			//-------------------- service sec lin. interpol -------------------------------
			CalcSec sec = delegate(int p1, int p2, out double x, out double y)
			{
				double c = 1.0 / (h[p2] - h[p1]);
				x = (h[p2] * xh[p1] - h[p1] * xh[p2]) * c;
				y = (h[p2] * yh[p1] - h[p1] * yh[p2]) * c;
			};

			//------------------------------------------------------------------------------
			const int m2 = 0; int m;

			for (int j = 0; j < ny - 1; j++)		// over all north - south and
			{
				for (m = 1; m < 5; m++)
					yh[m] = y0 + (j + jm[m - 1]) * hy;
				yh[0] = y0 + (j + 0.5) * hy;

				for (int i = 0; i < nx - 1; i++)	// east - west coordinates of datafield
				{
					// set casting bounds from array
					double val00 = data[j, i];
					double val01 = data[j, i + 1];
					double val10 = data[j + 1, i];
					double val11 = data[j + 1, i + 1];

					if (double.Equals(val00, NoDataValue) || double.Equals(val10, NoDataValue) || double.Equals(val01, NoDataValue) || double.Equals(val11, NoDataValue))
						continue;

					double dmin = val00;
					double dmax = val00;

					if (dmin > val01) dmin = val01;
					if (dmin > val10) dmin = val10;
					if (dmin > val11) dmin = val11;

					if (dmax < val01) dmax = val01;
					if (dmax < val10) dmax = val10;
					if (dmax < val11) dmax = val11;

					bool mod = false;

					if (circular)
					{
						if (dmin < 90 && dmax > 270)
						{
							double tmp = dmax;
							dmax = dmin;
							dmin = tmp;

							if (val00 > 180.0 && val00 < dmin) dmin = val00;
							if (val01 > 180.0 && val01 < dmin) dmin = val01;
							if (val10 > 180.0 && val10 < dmin) dmin = val10;
							if (val11 > 180.0 && val11 < dmin) dmin = val11;

							if (val00 <= 180.0 && val00 > dmax) dmax = val00;
							if (val01 <= 180.0 && val01 > dmax) dmax = val01;
							if (val10 <= 180.0 && val10 > dmax) dmax = val10;
							if (val11 <= 180.0 && val11 > dmax) dmax = val11;
							dmax += 360.0;

							mod = true;
						}
					}
					else if (dmax < z0 || dmin > z0 + nz * hz)	// ask horzintal cut avail.
						continue;

					int nz0 = (int)Math.Floor((dmin - z0) * Invhz);
					int nzn = (int)Math.Ceiling((dmax - z0) * Invhz);

					if (nz0 < 0) nz0 = 0;
					if (!mod && (nzn > nz)) nzn = nz;

					for (m = 1; m < 5; m++)
						xh[m] = x0 + (i + im[m - 1]) * hx;
					xh[0] = x0 + (i + 0.5) * hx;

					for (int k = nz0; k <= nzn; k++)		// over all possible cuts
					{
						double zk = z0 + k * hz;
						// aks for cut intervall
						if (zk < dmin || zk > dmax)
							continue;

						// deteriening the cut casts and set the
						// height and coordinate vectors
						for (m = 1; m < 5; m++)
						{
							double val = data[j + jm[m - 1], i + im[m - 1]];
							if (mod && val <= 180.0)
								val += 360.0;
							h[m] = val - zk;
							sh[m] = Math.Sign(h[m]) + 1;
						}

						h[0] = 0.25 * (h[1] + h[2] + h[3] + h[4]);
						sh[0] = Math.Sign(h[0]) + 1;

						//-----------------------------------------------------------------------
						for (int m1 = 1; m1 < 5; m1++)
						{
							// set directional casttable
							//m1 is remainder variable
							int m3 = 1; if (m1 != 4) m3 += m1;
							int deside = castab[sh[m1], sh[m2], sh[m3]];

							if (deside <= 0)// ask is there a desition available
								continue;

							switch (deside) 	//determin the by desided cast cuts
							{
								case 1:
									begX = xh[m1];
									begY = yh[m1];

									endX = xh[m2];
									endY = yh[m2];
									break;
								case 2:
									begX = xh[m2];
									begY = yh[m2];

									endX = xh[m3];
									endY = yh[m3];
									break;
								case 3:
									begX = xh[m3];
									begY = yh[m3];

									endX = xh[m1];
									endY = yh[m1];
									break;
								case 4:
									begX = xh[m1];
									begY = yh[m1];
									sec(m2, m3, out endX, out endY);
									break;
								case 5:
									begX = xh[m2];
									begY = yh[m2];
								    sec(m3, m1, out  endX, out  endY);
									break;
								case 6:
									begX = xh[m3];
									begY = yh[m3];
								    sec(m1, m2, out endX, out endY);
									break;
								case 7:
								    sec(m1, m2, out begX, out begY);
								    sec(m2, m3, out endX, out endY);
									break;
								case 8:
								    sec(m2, m3, out begX, out begY);
								    sec(m3, m1, out endX, out endY);
									break;
								case 9:
								    sec(m3, m1, out begX, out begY);
								    sec(m1, m2, out endX, out endY);
									break;
							} // -Case deside;

							int lk = k;
							double lzk = zk;

							if (mod)
							{
								lk %= nz;
                                lzk = NativeMethods.Modulus(zk);
							}

							ConArray.Add(begX + 0.5 * hx, begY + 0.5 * hy, endX + 0.5 * hx, endY + 0.5 * hy, lzk, lk);
						} // -----  -for m1
					} // ---------  -for k
				} // -------------  -for i
			} // --------------  -for j
		}
		//=======================================
	}
}


