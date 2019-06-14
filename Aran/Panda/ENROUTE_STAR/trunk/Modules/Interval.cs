using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Panda.Common;

namespace Aran.Panda.EnrouteStar
{
	//[System.Runtime.InteropServices.ComVisible(false)]
	//public struct Interval
	//{
	//    public double Left;
	//    public double Right;
	//}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct Interval
	{
		public Double Left, Right;
	    //Boolean Ciclic;
	};

	[System.Runtime.InteropServices.ComVisible(false)]
	public class IntervalList
	{

		int _Count;
		Interval[] _Range;

		static public Double CheckValue(Double Val, Interval ValidRange, Boolean Circular)
		{
			Double Range, fTmp;

			if (!Circular)
			{
				if (Val < ValidRange.Left) Val = ValidRange.Left;
				if (Val > ValidRange.Right) Val = ValidRange.Right;
			}
			else
			{
				Range = ARANMath.Modulus(ValidRange.Right - ValidRange.Left, ARANMath.C_2xPI);
				fTmp = ARANMath.Modulus(ValidRange.Right - Val, ARANMath.C_2xPI);
				if (fTmp > Range)
				{
					if (ARANMath.Modulus(fTmp - 0.5 * Range, ARANMath.C_2xPI) < ARANMath.C_PI)
						Val = ValidRange.Left;
					else Val = ValidRange.Right;
				}
			}
			return Val;
		}

		static Boolean IntersectIntervals(Interval A, Interval B, out Interval C)
		{
			C = new Interval();

			if (A.Left > B.Right || A.Right < B.Left)
				return false;

			if (A.Right < B.Right)
				C.Right = A.Right;
			else
				C.Right = B.Right;

			if (A.Left > B.Left)
				C.Left = A.Left;
			else
				C.Left = B.Left;

			return true;
		}

		static Boolean SubtractInterval(Interval A, Interval B, ref IntervalList C)
		{
			//C = new IntervalList();
			Interval TmpRange = new Interval();

			if (B.Left == B.Right || B.Right < A.Left || A.Right < B.Left)
				C.AddInterval(A);
			else if (A.Left < B.Left && A.Right > B.Right)
			{
				TmpRange.Left = A.Left;
				TmpRange.Right = B.Left;
				C.AddInterval(TmpRange);

				TmpRange.Left = B.Right;
				TmpRange.Right = A.Right;
				C.AddInterval(TmpRange);
			}
			else if (A.Right > B.Right)
			{
				TmpRange.Left = B.Right;
				TmpRange.Right = A.Right;
				C.AddInterval(TmpRange);
			}
			else if (A.Left < B.Left)
			{
				TmpRange.Left = A.Left;
				TmpRange.Right = B.Left;
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

			if (Range.Right < Range.Left)
			{
				fTmp = Range.Left;
				Range.Left = Range.Right;
				Range.Right = fTmp;
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
				if (_Range[I].Left <= Val && _Range[I].Right >= Val)
					return I;
			return -1;
		}

		public Boolean AddInterval(Interval Val)
		{
			if (Val.Right < Val.Left) return false;

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
				while (R < _Count && _Range[R].Right < Val.Left)
					R++;

				if (R == _Count)
				{
					if (_Range[R - 1].Right >= Val.Left)
					{
						if (_Range[R - 1].Left > Val.Left)
							_Range[R - 1].Left = Val.Left;

						if (_Range[R - 1].Right < Val.Right)
							_Range[R - 1].Right = Val.Right;
					}
					else
					{
						_Count++;
						System.Array.Resize<Interval>(ref _Range, _Count);
						_Range[_Count - 1] = Val;
					}
				}
				else if (_Range[R].Left > Val.Right)
				{
					_Count++;
					System.Array.Resize<Interval>(ref _Range, _Count);
					for (I = _Count - 1; I >= R; I++)
						_Range[I] = _Range[I - 1];

					_Range[R] = Val;
				}
				else
				{
					if (_Range[R].Left > Val.Left)
						_Range[R].Left = Val.Left;

					if (_Range[R].Right < Val.Right)
						_Range[R].Right = Val.Right;

					L = R + 1;
					while (L < _Count && _Range[L].Left <= Val.Right)
						L++;

					if (_Range[L - 1].Right > Val.Right)
						_Range[R].Right = _Range[L - 1].Right;

					if (L == _Count)
					{
						if (_Range[R].Right < Val.Right)
							_Range[R].Right = Val.Right;
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

