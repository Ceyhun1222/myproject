using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Panda.Common;

namespace Aran.Panda.Common
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public struct Interval
	{
		public Double Min, Max;
		public Boolean Circular;
		public Boolean InDegree;

		public Double CheckValue(Double Val)
		{
			Double Range, fTmp;

			if (!Circular)
			{
				if (Val < Min) Val = Min;
				if (Val > Max) Val = Max;
			}
			else if (InDegree)
			{
				Range = ARANMath.Modulus(Max - Min, 360.0);
				fTmp = ARANMath.Modulus(Max - Val, 360.0);
				if (fTmp > Range)
				{
					if (ARANMath.Modulus(fTmp - 0.5 * Range, 360.0) < 180.0)
						Val = Min;
					else Val = Max;
				}
			}
			else
			{
				Range = ARANMath.Modulus(Max - Min, ARANMath.C_2xPI);
				fTmp = ARANMath.Modulus(Max - Val, ARANMath.C_2xPI);
				if (fTmp > Range)
				{
					if (ARANMath.Modulus(fTmp - 0.5 * Range, ARANMath.C_2xPI) < ARANMath.C_PI)
						Val = Min;
					else Val = Max;
				}
			}
			return Val;
		}
	};

	[System.Runtime.InteropServices.ComVisible(false)]
	public class IntervalList
	{
		int _Count;
		Interval[] _Range;

		public Double CheckValue(Double Val, Interval ValidRange, Boolean Circular)
		{
			Double Range, fTmp;

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

		static Boolean IntersectIntervals(Interval A, Interval B, out Interval C)
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

		static Boolean SubtractInterval(Interval A, Interval B, ref IntervalList C)
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
			Double fTmp;

			_Count = 1;
			System.Array.Resize<Interval>(ref _Range, 1);

			if (Range.Max < Range.Min)
			{
				fTmp = Range.Min;
				Range.Min = Range.Max;
				Range.Max = fTmp;
			}

			_Range[0] = Range;
		}

		//public ~IntervalList(){}

		public void Assign(IntervalList Val)
		{
			int I;

			_Count = Val._Count;
			System.Array.Resize<Interval>(ref _Range, _Count);

			for (I = 0; I < _Count; I++)
				_Range[I] = Val._Range[I];
		}

		public IntervalList Clone()
		{
			IntervalList result = new IntervalList();
			result.Assign(this);
			return result;
		}

		public int Contains(Double Val)
		{
			int I;

			for (I = 0; I < _Count; I++)
				if (_Range[I].Min <= Val && _Range[I].Max >= Val)
					return I;
			return -1;
		}

		public Boolean AddInterval(Interval Val)
		{
			if (Val.Max < Val.Min) return false;

			int I, R, L;
			//Boolean result = false;


			if (_Count == 0)
			{
				_Count = 1;
				System.Array.Resize<Interval>(ref _Range, _Count);

				_Range[0] = Val;
			}
			else
			{
				R = 0;
				while (R < _Count && _Range[R].Max < Val.Min)
					R++;

				if (R == _Count)
				{
					if (_Range[R - 1].Max >= Val.Min)
					{
						if (_Range[R - 1].Min > Val.Min)
							_Range[R - 1].Min = Val.Min;

						if (_Range[R - 1].Max < Val.Max)
							_Range[R - 1].Max = Val.Max;
					}
					else
					{
						_Count++;
						System.Array.Resize<Interval>(ref _Range, _Count);
						_Range[_Count - 1] = Val;
					}
				}
				else if (_Range[R].Min > Val.Max)
				{
					_Count++;
					System.Array.Resize<Interval>(ref _Range, _Count);
					for (I = _Count - 1; I >= R; I++)
						_Range[I] = _Range[I - 1];

					_Range[R] = Val;
				}
				else
				{
					if (_Range[R].Min > Val.Min)
						_Range[R].Min = Val.Min;

					if (_Range[R].Max < Val.Max)
						_Range[R].Max = Val.Max;

					L = R + 1;
					while (L < _Count && _Range[L].Min <= Val.Max)
						L++;

					if (_Range[L - 1].Max > Val.Max)
						_Range[R].Max = _Range[L - 1].Max;

					if (L == _Count)
					{
						if (_Range[R].Max < Val.Max)
							_Range[R].Max = Val.Max;
						_Count = R + 1;
						System.Array.Resize<Interval>(ref _Range, _Count);
					}
					else
					{
						for (I = R + 1; I < _Count - 1; I++)
							_Range[I] = _Range[I + 1];

						_Count -= L - R;

						System.Array.Resize<Interval>(ref _Range, _Count);
					}
				}
			}
			return true;
		}

		public void Union(IntervalList Val)
		{
			for (int I = 0; I < Val._Count; I++)
				AddInterval(Val._Range[I]);
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

			IntervalList TmpInter;

			TmpInter = new IntervalList();
			for (int J = 0; J < Val._Count; J++)
				for (int I = 0; I < _Count; I++)
					if (IntersectIntervals(Val._Range[J], _Range[I], out C))
						TmpInter.AddInterval(C);

			Assign(TmpInter);
		}


		public void Subtract(Interval Val)
		{
			IntervalList TmpInter = new IntervalList();

			for (int I = 0; I < _Count; I++)
				IntervalList.SubtractInterval(Val, _Range[I], ref TmpInter);

			Assign(TmpInter);
		}

		public void Subtract(IntervalList Val)
		{
			IntervalList TmpInter = new IntervalList();
			for (int J = 0; J < Val._Count; J++)
				for (int I = 0; I < _Count; I++)
					SubtractInterval(Val._Range[J], _Range[I], ref TmpInter);

			Assign(TmpInter);
		}

		public int Count { get { return _Count; } }

		public Interval this[int index]
		{
			get
			{
				if (index >= 0 && index < _Count)
					return _Range[index];

				throw new Exception("Interval inindex out of bound.");
			}

			set
			{
				if (index >= 0 && index < _Count)
					_Range[index] = value;
				else
					throw new Exception("Interval inindex out of bound.");
			}
		}
	}
}

