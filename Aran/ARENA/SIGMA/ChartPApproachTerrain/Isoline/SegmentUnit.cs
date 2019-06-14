using System;
using System.Windows.Media.Media3D;

namespace ChartPApproachTerrain
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public struct Segment
	{
		public Point3D ptStart, ptEnd;
		public double k;
		public int Level;
		public bool Used;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public interface ISegmentsHolder
	{
		//int CreateContour(double stLon, double hLon, int width, double stLat, double hLat, int height, double stVal,
		//				  double hVal, int nVal, CalcFunction function);

		int CreateContour(double stLon, double hLon, int width, double stLat, double hLat, int height, double stVal,
						  double hVal, int nVal, double[,] data);

		void Add(double x0, double y0, double x1, double y1, double z,int Level);
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public class SegmentArray
	{
		private int _LevelCnt;
		int[] Capacity;
		int[] Indices;
		Segment[][] _Data;

		const double EpsilonLin = 0.01;	// 0.008726;
		const int ArrayInc = 1024;

		int CompareItems(Point3D item1, Point3D item2)
		{
			if (item1.X < item2.X) return -1;
			if (item1.X > item2.X) return 1;
			if (item1.Y < item2.Y) return -1;
			if (item1.Y > item2.Y) return 1;
			return 0;
		}

		void Clear()
		{
			Capacity = new int[_LevelCnt];
			Indices = new int[_LevelCnt];
			_Data = new Segment[_LevelCnt][];

			for (int i = 0; i < _LevelCnt; i++)
			{
				_Data[i] = new Segment[ArrayInc];
				Capacity[i] = ArrayInc;
				Indices[i] = 0;
			}
		}

		public SegmentArray(int nLevels)
		{
			_LevelCnt = nLevels;
			Clear();
		}

		public Segment GetSegment(int level, int index)
		{
			if (level >= 0 && level < _LevelCnt && index >= 0 && index < Indices[level])
				return _Data[level][index];

			return new Segment();
		}

		public Segment GetNextUnused(int level, ref int index)
		{
			//if (level >= 0 && level < _LevelCnt && index >= 0 && index < Indices[level])
			{
				while (index < Indices[level] && _Data[level][index].Used)
					index++;
				if (index < Indices[level])
					return _Data[level][index];
			}

			return new Segment() { Used = true };
		}

		public int GetCount(int level)
		{
			if (level >= 0 && level < _LevelCnt)
				return Indices[level];
			return -1;
		}

		public void ClearUsedFlags()
		{
			int i, j;

			for (i = 0; i < _LevelCnt; i++)
				for (j = 0; j < Indices[i]; j++)
					_Data[i][j].Used = false;
		}

		public void SetUsed(int i, int j, bool Val)
		{
			_Data[i][j].Used = Val;
		}

		public void Add(double x0, double y0, double x1, double y1, double z, int Level)
		{
			int i;
			double dLat;
			Point3D pt;

			if (Indices[Level] >= Capacity[Level])
			{
				Capacity[Level] += ArrayInc;
				Array.Resize(ref _Data[Level], Capacity[Level]);
			}

			i = Indices[Level];
			_Data[Level][i].ptStart.X = x0;
			_Data[Level][i].ptStart.Y = y0;

			_Data[Level][i].ptEnd.X = x1;
			_Data[Level][i].ptEnd.Y = y1;
			_Data[Level][i].Used = false;
			_Data[Level][i].Level = Level;

			if (CompareItems(_Data[Level][i].ptStart, _Data[Level][i].ptEnd) > 0)
			{
				pt = _Data[Level][i].ptStart;
				_Data[Level][i].ptStart = _Data[Level][i].ptEnd;
				_Data[Level][i].ptEnd = pt;
			}

			dLat = _Data[Level][i].ptEnd.Y - _Data[Level][i].ptStart.Y;

			_Data[Level][i].k = Math.Atan2(dLat, _Data[Level][i].ptEnd.X - _Data[Level][i].ptStart.X);
			Indices[Level]++;
		}

		void SortLines(int lb, int ub, int Level)
		{
			int i = lb, j = ub;


			Point3D pt = _Data[Level][(lb + ub) >> 1].ptStart;
			do
			{
				while (CompareItems(_Data[Level][i].ptStart, pt) < 0)
					i++;

				while (CompareItems(_Data[Level][j].ptStart, pt) > 0)
					j--;

				if (i <= j)
				{
					Segment temp = _Data[Level][i];
					_Data[Level][i] = _Data[Level][j];
					_Data[Level][j] = temp;
					i++;
					j--;
				}
			}
			while (i <= j);

			if (j > lb)
				SortLines(lb, j, Level);

			if (i < ub)
				SortLines(i, ub, Level);
		}

		// Merge colinear segments
		const double PIx2 = 2.0 * Math.PI;

		public void MergeColinears()
		{
			int i, j, n, k, chk;
			double dAngle;

			for (i = 0; i < _LevelCnt; i++)
			{
				n = Indices[i];
				if (n > 0)
				{
					if (n > 1)
						SortLines(0, n - 1, i);

					Segment[] WorkDATA = new Segment[n];
					k = 0;
					j = 0;

					while (k < n)
					{
						while (j < n && _Data[i][j].Used)
							j++;

						if (j >= n)
							break;

						WorkDATA[k] = _Data[i][j];

						for (chk = j + 1; chk < n; chk++)
						{
							if (_Data[i][chk].ptStart.X - WorkDATA[k].ptEnd.X > PolylineBuilder.EpsilonLon)
								break;
							if (!_Data[i][chk].Used)
							{
								//if (WorkDATA[k].k * _Data[i][chk].k < 0.0)
								//	dAngle = WorkDATA[k].k - _Data[i][chk].k;
								//else
								//	dAngle = WorkDATA[k].k - _Data[i][chk].k;

								//if (dAngle < 0.0)
								//	dAngle = PIx2 + dAngle;

								dAngle = (WorkDATA[k].k - _Data[i][chk].k);

								if (dAngle < 0.0)
									dAngle = PIx2 + dAngle;
								if (dAngle > Math.PI)
									dAngle = PIx2 - dAngle;

								if (dAngle < EpsilonLin &&
									Math.Abs(WorkDATA[k].ptEnd.X - _Data[i][chk].ptStart.X) < PolylineBuilder.EpsilonLon &&
									Math.Abs(WorkDATA[k].ptEnd.Y - _Data[i][chk].ptStart.Y) < PolylineBuilder.EpsilonLat)
								{
									WorkDATA[k].ptEnd = _Data[i][chk].ptEnd;
									_Data[i][chk].Used = true;
								}
							}
						}
						j++;
						k++;
					}

					Indices[i] = k;
					Array.Resize(ref _Data[i], k);
					Array.Copy(WorkDATA, _Data[i], k);
				}
			}
		}
	}
	//===================================
}