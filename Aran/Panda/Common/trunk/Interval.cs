using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.PANDA.Common;

namespace Aran.PANDA.Common
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public struct Interval
	{
		double _Min;
		double _Max;

		public double Min
		{
			get { return _Min; }

			set
			{
				_Min = value;
				if (!Circular)
					return;

				if (InDegree)
				{
					if (value < 0.0 || value > 360.0)
						_Max = ARANMath.Modulus(value);
				}
				else if (value < 0.0 || value > 2.0 * Math.PI)
					_Min = ARANMath.Modulus(value, ARANMath.C_2xPI);
			}
		}

		public double Max
		{
			get { return _Max; }

			set
			{
				_Max = value;
				if (!Circular)
					return;

				if (InDegree)
				{
					if (value < 0.0 || value > 360.0)
						_Max = ARANMath.Modulus(value);
				}
				else if (value < 0.0 || value > 2.0 * Math.PI)
					_Max = ARANMath.Modulus(value, ARANMath.C_2xPI);
			}
		}

		public bool Circular;
		public bool InDegree;
		public int Tag;

		public double CheckValue(double Val)
		{
			double Range, fTmp;

			if (!Circular)
			{
				if (Val < Min) Val = Min;
				if (Val > Max) Val = Max;
			}
			else if (InDegree)
			{
				Range = Max - Min;
				if (Range < 0.0 || Range > 360.0)
					Range = ARANMath.Modulus(Range, 360.0);

				fTmp = Max - Val;
				if (fTmp < 0.0 || fTmp > 360.0)
					fTmp = ARANMath.Modulus(fTmp, 360.0);

				if (fTmp > Range)
				{
					if (ARANMath.Modulus(fTmp - 0.5 * Range, 360.0) < 180.0)
						Val = Min;
					else Val = Max;
				}

				//Range = ARANMath.Modulus(Max - Min, 360.0);
				//fTmp = ARANMath.Modulus(Max - Val, 360.0);
				//if (fTmp > Range)
				//{
				//	if (ARANMath.Modulus(fTmp - 0.5 * Range, 360.0) < 180.0)
				//		Val = Min;
				//	else Val = Max;
				//}
			}
			else
			{
				Range = Max - Min;
				if (Range < 0.0 || Range > ARANMath.C_2xPI)
					Range = ARANMath.Modulus(Range, ARANMath.C_2xPI);

				fTmp = Max - Val;
				if (fTmp < 0.0 || fTmp > ARANMath.C_2xPI)
					fTmp = ARANMath.Modulus(fTmp, ARANMath.C_2xPI);

				if (fTmp > Range)
				{
					if (ARANMath.Modulus(fTmp - 0.5 * Range, ARANMath.C_2xPI) < ARANMath.C_PI)
						Val = Min;
					else Val = Max;
				}

				//Range = ARANMath.Modulus(Max - Min, ARANMath.C_2xPI);
				//fTmp = ARANMath.Modulus(Max - Val, ARANMath.C_2xPI);
				//if (fTmp > Range)
				//{
				//	if (ARANMath.Modulus(fTmp - 0.5 * Range, ARANMath.C_2xPI) < ARANMath.C_PI)
				//		Val = Min;
				//	else Val = Max;
				//}
			}

			return Val;
		}

		public static Interval[] Difference(Interval A, Interval B)
		{
			Interval[] Result;

			if (B.Min == B.Max || B.Max < A.Min || A.Max < B.Min)
			{
				Result = new Interval[1];
				Result[0] = A;
			}
			else if (A.Min < B.Min && A.Max > B.Max)
			{
				Result = new Interval[2];

				Result[0].Min = A.Min;
				Result[0].Max = B.Min;
				Result[1].Min = B.Max;
				Result[1].Max = A.Max;
			}
			else if (A.Max > B.Max)
			{
				Result = new Interval[1];
				Result[0].Min = B.Max;
				Result[0].Max = A.Max;
			}
			else if (A.Min < B.Min)
			{
				Result = new Interval[1];
				Result[0].Min = A.Min;
				Result[0].Max = B.Min;
			}
			else
				Result = new Interval[0];

			return Result;
		}

		public bool InsideInterval(double Val)
		{
			if (!Circular)
			{
				if (Val < Min) return false;
				if (Val > Max) return false;
				return true;
			}

			double Range, fTmp;

			if (InDegree)
			{
				Range = ARANMath.Modulus(Max - Min, 360.0);
				fTmp = ARANMath.Modulus(Max - Val, 360.0);
				//if (fTmp > Range)
				//{
				//	if (ARANMath.Modulus(fTmp - 0.5 * Range, 360.0) < 180.0)
				//		Val = Min;
				//	else Val = Max;
				//}
			}
			else
			{
				Range = ARANMath.Modulus(Max - Min, ARANMath.C_2xPI);
				fTmp = ARANMath.Modulus(Max - Val, ARANMath.C_2xPI);
			}

			//if (fTmp > Range)
			//	{
			//		if (ARANMath.Modulus(fTmp - 0.5 * Range, ARANMath.C_2xPI) < ARANMath.C_PI)
			//			Val = Min;
			//		else Val = Max;
			//	}

			return fTmp <= Range;
		}
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public class IntervalList
	{
		int _Count;
		Interval[] _Range;

		public double CheckValue(double Val, Interval ValidRange, bool Circular)
		{
			double Range, fTmp;

			if (!Circular)
			{
				if (Val < ValidRange.Min) Val = ValidRange.Min;
				if (Val > ValidRange.Max) Val = ValidRange.Max;
			}
			else
			{
				Range = ARANMath.Modulus(ValidRange.Max - ValidRange.Min, ARANMath.C_2xPI);
				fTmp = ARANMath.Modulus(ValidRange.Max - Val, ARANMath.C_2xPI);

				if (fTmp > Range)
				{
					if (ARANMath.Modulus(fTmp - 0.5 * Range, ARANMath.C_2xPI) < ARANMath.C_PI)
						Val = ValidRange.Min;
					else Val = ValidRange.Max;
				}
			}
			return Val;
		}

		static bool IntersectIntervals(Interval A, Interval B, out Interval C)
		{
			C = new Interval();

			if (A.Min > B.Max || A.Max < B.Min)
				return false;

			if (A.Max < B.Max)
				C.Max = A.Max;
			else
				C.Max = B.Max;

			if (A.Min > B.Min)
				C.Min = A.Min;
			else
				C.Min = B.Min;

			return true;
		}

		static bool SubtractInterval(Interval A, Interval B, ref IntervalList C)
		{
			//C = new IntervalList();
			Interval TmpRange = new Interval();

			if (B.Min == B.Max || B.Max < A.Min || A.Max < B.Min)
				C.AddInterval(A);
			else if (A.Min < B.Min && A.Max > B.Max)
			{
				TmpRange.Min = A.Min;
				TmpRange.Max = B.Min;
				C.AddInterval(TmpRange);

				TmpRange.Min = B.Max;
				TmpRange.Max = A.Max;
				C.AddInterval(TmpRange);
			}
			else if (A.Max > B.Max)
			{
				TmpRange.Min = B.Max;
				TmpRange.Max = A.Max;
				C.AddInterval(TmpRange);
			}
			else if (A.Min < B.Min)
			{
				TmpRange.Min = A.Min;
				TmpRange.Max = B.Min;
				C.AddInterval(TmpRange);
			}
			else
				return false;

			return true;
		}


		public IntervalList()
		{
			_Count = 0;
			_Range = null;
		}

		public IntervalList(Interval Range)
		{
			_Count = 1;
			_Range = new Interval[1];

			if (Range.Max < Range.Min)
			{
				double fTmp = Range.Min;
				Range.Min = Range.Max;
				Range.Max = fTmp;
			}

			_Range[0] = Range;
		}

		//public ~IntervalList(){}

		public void Assign(IntervalList Val)
		{
			_Count = Val._Count;
			System.Array.Resize<Interval>(ref _Range, _Count);

			for (int i = 0; i < _Count; i++)
				_Range[i] = Val._Range[i];
		}

		public IntervalList Clone()
		{
			IntervalList result = new IntervalList();
			result.Assign(this);
			return result;
		}

		public int Contains(double Val)
		{
			for (int i = 0; i < _Count; i++)
				if (_Range[i].Min <= Val && _Range[i].Max >= Val)
					return i;
			return -1;
		}

		public bool AddInterval(Interval Val)
		{
			if (Val.Max < Val.Min)
				return false;

			if (_Count == 0)
			{
				_Count = 1;
				System.Array.Resize<Interval>(ref _Range, _Count);
				_Range[0] = Val;
			}
			else
			{
				int r = 0;

				while (r < _Count && _Range[r].Max < Val.Min)
					r++;

				if (r == _Count)
				{
					if (_Range[r - 1].Max >= Val.Min)
					{
						if (_Range[r - 1].Min > Val.Min)
							_Range[r - 1].Min = Val.Min;

						if (_Range[r - 1].Max < Val.Max)
							_Range[r - 1].Max = Val.Max;
					}
					else
					{
						_Count++;
						System.Array.Resize<Interval>(ref _Range, _Count);
						_Range[_Count - 1] = Val;
					}
				}
				else if (_Range[r].Min > Val.Max)
				{
					_Count++;
					System.Array.Resize<Interval>(ref _Range, _Count);
					for (int i = _Count - 1; i > r; i--)
						_Range[i] = _Range[i - 1];

					_Range[r] = Val;
				}
				else
				{
					if (_Range[r].Min > Val.Min)
						_Range[r].Min = Val.Min;

					if (_Range[r].Max < Val.Max)
						_Range[r].Max = Val.Max;

					int l = r + 1;
					while (l < _Count && _Range[l].Min <= Val.Max)
						l++;

					if (_Range[l - 1].Max > Val.Max)
						_Range[r].Max = _Range[l - 1].Max;

					if (l == _Count)
					{
						if (_Range[r].Max < Val.Max)
							_Range[r].Max = Val.Max;
						_Count = r + 1;
						System.Array.Resize<Interval>(ref _Range, _Count);
					}
					else
					{
						for (int i = r + 1; i < _Count - 1; i++)
							_Range[i] = _Range[i + 1];

						_Count -= l - r;

						System.Array.Resize<Interval>(ref _Range, _Count);
					}
				}
			}

			return true;
		}

		public void Union(IntervalList Val)
		{
			for (int i = 0; i < Val._Count; i++)
				AddInterval(Val._Range[i]);
		}

		public void Intersect(Interval Val)
		{
			Interval C;
			IntervalList TmpInter = new IntervalList();

			for (int I = 0; I < _Count; I++)
				if (IntersectIntervals(Val, _Range[I], out C))
					TmpInter.AddInterval(C);

			Assign(TmpInter);
		}

		public void Intersect(IntervalList Val)
		{
			Interval C;
			IntervalList TmpInter = new IntervalList();

			for (int j = 0; j < Val._Count; j++)
				for (int i = 0; i < _Count; i++)
					if (IntersectIntervals(Val._Range[j], _Range[i], out C))
						TmpInter.AddInterval(C);

			Assign(TmpInter);
		}


		public void Subtract(Interval Val)
		{
			IntervalList TmpInter = new IntervalList();

			for (int i = 0; i < _Count; i++)
				IntervalList.SubtractInterval(Val, _Range[i], ref TmpInter);

			Assign(TmpInter);
		}

		public void Subtract(IntervalList Val)
		{
			IntervalList TmpInter = new IntervalList();

			for (int j = 0; j < Val._Count; j++)
				for (int i = 0; i < _Count; i++)
					SubtractInterval(Val._Range[j], _Range[i], ref TmpInter);

			Assign(TmpInter);
		}

		public int Count { get { return _Count; } }

		public Interval this[int index]
		{
			get
			{
				if (index >= 0 && index < _Count)
					return _Range[index];

				throw new Exception("Interval index out of bound.");
			}

			set
			{
				if (index >= 0 && index < _Count)
					_Range[index] = value;
				else
					throw new Exception("Interval index out of bound.");
			}
		}
	}
}

