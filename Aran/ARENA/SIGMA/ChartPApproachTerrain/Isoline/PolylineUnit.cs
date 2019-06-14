using System;
using System.Collections.Generic;
using ESRI.ArcGIS.Geometry;
using System.Windows.Media.Media3D;

namespace ChartPApproachTerrain
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public class PolylineBuilder : ISegmentsHolder
	{
		[System.Runtime.InteropServices.ComVisible(false)]
		public struct sPolyline
		{
			public double Value;
			public int Level;
			public List<Point3D> Points;
		}

		public bool isCircular = false;

		public const int ArrayInc = 1024;
		public const double Multiplier = 10e-7;
		public static double EpsilonLat = 0.0000001;
		public static double EpsilonLon = 0.0000001;

		sPolyline[] _PolyLines;
		bool PolylinesCreated;

		public PolylineBuilder()
		{
			PolylinesCreated = false;
		}

		//==============================================================================

		public int MergeSegments(SegmentArray ConArray, double stValue, double hValue, int nVal)
		{
			int i, j, k, l, m,
			UsedSegs;
			Segment CurrSeg;
			Point3D CurrPnt, CurrPntN,
			TmpPnt0, TmpPntN;
			double Eps2, dX, dY,
			d0, d1, d2, d3;

			bool bRepeat;

			int ArrayCapacity = ArrayInc;

			_PolyLines = new sPolyline[ArrayCapacity];

			ConArray.MergeColinears();
			ConArray.ClearUsedFlags();
			m = -1;
			Eps2 = EpsilonLon * EpsilonLat;

			for (i = 0; i <= nVal; i++)
			{
				int CountI = ConArray.GetCount(i);
				double Value = stValue + i * hValue;

				if (CountI > 1)
				{
					l = m;
					UsedSegs = 0;
					k = -1;

					//Merge consequent segments into polyline
					while (UsedSegs < CountI)
					{
						k++;


						for (j = k; j < CountI; j++)
						{
							CurrSeg = ConArray.GetSegment(i, j);
							if (!CurrSeg.Used)
							{
								k = j;
								break;
							}
						}

						if (k != j)
							break;

						ConArray.SetUsed(i, k, true);
						UsedSegs++;
						m++;

						if (m >= ArrayCapacity)
						{
							ArrayCapacity += ArrayInc;
							Array.Resize(ref _PolyLines, ArrayCapacity);
						}

						CurrSeg = ConArray.GetSegment(i, k);
						CurrPnt = CurrSeg.ptEnd;

					    _PolyLines[m].Points = new List<Point3D> {CurrSeg.ptStart, CurrPnt};
					    _PolyLines[m].Level = CurrSeg.Level;

						do
						{
							bRepeat = false;

							for (j = k; j < CountI; j++)
							{
								CurrSeg = ConArray.GetNextUnused(i, ref j);
								if (CurrSeg.Used)
									break;

								dX = CurrSeg.ptStart.X - CurrPnt.X;
								dY = CurrSeg.ptStart.Y - CurrPnt.Y;
								d0 = dX * dX + dY * dY;

								if (d0 < Eps2)
								{
									ConArray.SetUsed(i, j, true);
									UsedSegs++;
									if (j == k)
										k++;

									_PolyLines[m].Points.Add(CurrSeg.ptEnd);

									CurrPnt = CurrSeg.ptEnd;
									bRepeat = true;
									break;
								}
							}
						} while (bRepeat);

						_PolyLines[m].Value = Value;
					}

					//Merge Polylines
					if (m - l > 1)
					{
						k = l + 1;
						while (k < m)
						{
							CurrPnt = _PolyLines[k].Points[0];
							CurrPntN = _PolyLines[k].Points[_PolyLines[k].Points.Count - 1];
							j = k + 1;

							while (j <= m)
							{
								TmpPnt0 = _PolyLines[j].Points[0];
								TmpPntN = _PolyLines[j].Points[_PolyLines[j].Points.Count - 1];
								dX = TmpPnt0.X - CurrPnt.X;
								dY = TmpPnt0.Y - CurrPnt.Y;
								d0 = dX * dX + dY * dY;

								dX = TmpPnt0.X - CurrPntN.X;
								dY = TmpPnt0.Y - CurrPntN.Y;
								d1 = dX * dX + dY * dY;

								dX = TmpPntN.X - CurrPnt.X;
								dY = TmpPntN.Y - CurrPnt.Y;
								d2 = dX * dX + dY * dY;

								dX = TmpPntN.X - CurrPntN.X;
								dY = TmpPntN.Y - CurrPntN.Y;
								d3 = dX * dX + dY * dY;

								if (d0 < Eps2)
								{
									List<Point3D> Points = new List<Point3D>();

									for (l = 0; l < _PolyLines[k].Points.Count; l++)
										Points.Add(_PolyLines[k].Points[_PolyLines[k].Points.Count - l - 1]);

									for (l = 1; l < _PolyLines[j].Points.Count; l++)
										Points.Add(_PolyLines[j].Points[l]);

									_PolyLines[k].Points = Points;

									CurrPnt = _PolyLines[k].Points[0];
									CurrPntN = _PolyLines[k].Points[Points.Count - 1];

									if (j < m)
										_PolyLines[j] = _PolyLines[m];
									j = k + 1;
									m--;
								}
								else if (d1 < Eps2)
								{
									for (l = 1; l < _PolyLines[j].Points.Count; l++)
										_PolyLines[k].Points.Add(_PolyLines[j].Points[l]);

									CurrPntN = _PolyLines[k].Points[_PolyLines[k].Points.Count - 1];

									if (j < m)
										_PolyLines[j] = _PolyLines[m];
									j = k + 1;
									m--;
								}
								else if (d2 < Eps2)
								{
									for (l = 1; l < _PolyLines[k].Points.Count; l++)
										_PolyLines[j].Points.Add(_PolyLines[k].Points[l]);

									_PolyLines[k].Points = _PolyLines[j].Points;

									CurrPnt = _PolyLines[k].Points[0];

									if (j < m)
										_PolyLines[j] = _PolyLines[m];
									j = k + 1;
									m--;
								}
								else if (d3 < Eps2)
								{
									for (l = 1; l < _PolyLines[j].Points.Count; l++)
										_PolyLines[k].Points.Add(_PolyLines[j].Points[_PolyLines[j].Points.Count - l - 1]);

									CurrPntN = _PolyLines[k].Points[_PolyLines[k].Points.Count - 1];

									_PolyLines[j].Points = null;
									if (j < m)
										_PolyLines[j] = _PolyLines[m];

									j = k + 1;
									m--;
								}
								else
									j++;
							}
							k++;
						}
					}
				}
				else if (CountI == 1)
				{
					m++;
					if (m >= ArrayCapacity)
					{
						ArrayCapacity += ArrayInc;
						Array.Resize(ref _PolyLines, ArrayCapacity);
					}

					CurrSeg = ConArray.GetSegment(i, 0);

					_PolyLines[m].Value = Value;
					_PolyLines[m].Points.Clear();
					_PolyLines[m].Points.Add(CurrSeg.ptStart);
					_PolyLines[m].Points.Add(CurrSeg.ptEnd);
					_PolyLines[m].Level = CurrSeg.Level;
				}
			}

			PolylinesCreated = true;
			if (m < 0)
			{
				_PolyLines = new sPolyline[0];
				return 0;
			}

			Array.Resize(ref _PolyLines, m + 1);
			return _PolyLines.Length;
		}

		public int Get_PolylineCount()
		{
			if (PolylinesCreated)
				return _PolyLines.Length;
			return 0;
		}

		//==============================================================================
		SegmentArray ConArray;
		public void Add(double x0, double y0, double x1, double y1, double z, int Level)
		{
			ConArray.Add(x0, y0, x1, y1, z, Level);
		}
		/*
		public int CreateContour(double stLon, double hLon, int nLon, double stLat, double hLat, int nLat, double stVal, double hVal, int nVal, CalcFunction function)
		{
			PolylinesCreated = false;

			EpsilonLat = Multiplier * hLat;
			EpsilonLon = Multiplier * hLon;

			ConArray = new SegmentArray(nVal + 1);

			Conrec.circular = Circular;
			Conrec.Contour(this, stLon, hLon, nLon, stLat, hLat, nLat, stVal, hVal, nVal, function);
			return MergeSegments(ConArray, stVal, hVal, nVal);
		}
		*/
		public int CreateContour(double stLon, double hLon, int nLon, double stLat, double hLat, int nLat, double stVal, double hVal, int nVal, double[,] data)
		{
			PolylinesCreated = false;

			EpsilonLat = Multiplier * hLat;
			EpsilonLon = Multiplier * hLon;

			ConArray = new SegmentArray(nVal + 1);

			Conrec.circular = isCircular;
			Conrec.Contour(this, stLon, hLon, nLon, stLat, hLat, nLat, stVal, hVal, nVal, data);
			return MergeSegments(ConArray, stVal, hVal, nVal);
		}

		public IPolyline GetPolyline(int i)
		{
			if (!PolylinesCreated || i >= _PolyLines.Length)
				return null;

			IPointCollection PointColl;

			IPolyline result = new Polyline() as IPolyline;
			PointColl = result as IPointCollection;

			IZAware pZAware = (IZAware)result;
			IMAware pMAware = (IMAware)result;
			pZAware.ZAware = true;
			pMAware.MAware = true;

			foreach (var t in _PolyLines[i].Points)
			{
			    IPoint tempPoint = new Point();
			    tempPoint.PutCoords(t.X, t.Y);
			    tempPoint.Z = _PolyLines[i].Value;
			    tempPoint.M = _PolyLines[i].Level;

			    PointColl.AddPoint(tempPoint);
			}

			//TempPoint.Set_M(_PolyLines[i].Value);
			//TempPoint.Set_Z(_PolyLines[i].Points[j].Z);

			return result;
		}
	}
}
