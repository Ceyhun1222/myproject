using System;
using System.Collections;

namespace Aran.PANDA.CRMWall
{
	public abstract class SortedArrayList : ArrayList
	{
		protected const int InsertionThreshold = 16;
		abstract public void Sort(int iLo, int iHi);

		public override void Sort()
		{
			if (base.Count > 1)
				Sort(0, base.Count - 1);
		}
	}

	//AE table must be sorted by X
	class AETable : SortedArrayList
	{
		//new public Edge this[int index]
		//{
		//    get { return (Edge)base[index]; }
		//    set { base[index] = (Edge)value; }
		//}

		/// Select a pivot. To ensure a good pivot is select, the median element of a
		/// sample of the data is calculated.
		///
		/// param iLo - the index of the first element
		/// param iHi - the index of the last element
		/// return - the pivot value
		private double selectPivot(int iLo, int iHi)
		{
			int count = (int)(6 * Math.Log10(iHi - iLo));
			count = Math.Min(count, 1024);
			int step = (iHi - iLo) / count;
			int i = iLo + count * ((int)(0.5 * (iHi - iLo) / count));

			return ((SlicerBase.Edge)base[i]).X;
		}

		///  Sort a block using the quicksort algorithm.
		///
		/// param iLo - the index of the first entry to sort
		/// param iHi - the index of the last entry to sort
		override public void Sort(int iLo, int iHi)
		{
			int Lo = iLo;

			do
			{
				if (iHi - iLo < InsertionThreshold)
				{
					for (int i = iLo + 1; i <= iHi; i++)
					{
						SlicerBase.Edge x = (SlicerBase.Edge)base[i];

						int Lu = iLo, Ha = i - 1;
						while (Lu <= Ha)
						{
							int m = (Lu + Ha) >> 1;

							if (x.X >= ((SlicerBase.Edge)base[m]).X)
								Lu = m + 1;
							else
								Ha = m - 1;
						}

						for (int j = i - 1; j >= Lu; j--)
							base[j + 1] = base[j];

						base[Lu] = x;
					}
					return;
				}

				int Hi = iHi;
				double Mid = ((SlicerBase.Edge)base[(Lo + Hi) >> 1]).X;	//Pivot
				//if (((Edge)base[Lo]).X > Mid && ((Edge)base[Lo]).X < ((Edge)base[Hi]).X)
				//    Mid = ((Edge)base[Lo]).X;
				//else if (((Edge)base[Hi]).X > Mid && ((Edge)base[Lo]).X < ((Edge)base[Lo]).X)
				//    Mid = ((Edge)base[Hi]).X;
				//double Mid = selectPivot(Lo, Hi);

				do
				{
					while (((SlicerBase.Edge)base[Lo]).X < Mid) Lo++;
					while (((SlicerBase.Edge)base[Hi]).X > Mid) Hi--;

					if (Lo <= Hi)
					{
						SlicerBase.Edge T = (SlicerBase.Edge)base[Lo];
						base[Lo] = base[Hi];
						base[Hi] = T;
						Lo++;
						Hi--;
					}
				}
				while (Lo < Hi);

				if (Hi > iLo)	Sort(iLo, Hi);
				iLo = Lo;
			} while (Lo < iHi);
		}
	}

	//IE table must be sorted by v0.Y
	class IETable : SortedArrayList
	{
		private double selectPivot(int iLo, int iHi)
		{
			int count = (int)(6 * Math.Log10(iHi - iLo));
			count = Math.Min(count, 1024);
			int step = (iHi - iLo) / count;
			int i = iLo + count * ((int)(0.5 * (iHi - iLo) / count));

			return ((SlicerBase.Edge)base[i]).v0.Y;
		}

		override public void Sort(int iLo, int iHi)
		{
			int Lo = iLo;

			do
			{
				if (iHi - iLo < InsertionThreshold)
				{
					for (int i = iLo + 1; i <= iHi; i++)
					{
						SlicerBase.Edge x = (SlicerBase.Edge)base[i];

						int Lu = iLo, Ha = i - 1;

						while (Lu <= Ha)
						{
							int m = (Lu + Ha) >> 1;
							if (((SlicerBase.Edge)base[m]).v0.Y >= x.v0.Y)
								Lu = m + 1;
							else
								Ha = m - 1;
						}

						for (int j = i - 1; j >= Lu; j--)
							base[j + 1] = base[j];

						base[Lu] = x;
					}
					return;
				}

				int Hi = iHi;
				double Mid = ((SlicerBase.Edge)base[(Lo + Hi) >> 1]).v0.Y;	//chose pivot

				//if (((Edge)base[Lo]).v0.Y > Mid && ((Edge)base[Lo]).v0.Y < ((Edge)base[Hi]).v0.Y)
				//    Mid = ((Edge)base[Lo]).v0.Y;
				//else if (((Edge)base[Hi]).v0.Y > Mid && ((Edge)base[Lo]).v0.Y < ((Edge)base[Lo]).v0.Y)
				//    Mid = ((Edge)base[Hi]).v0.Y;

				//double Mid = selectPivot(iLo, iHi);

				do
				{
					while (((SlicerBase.Edge)base[Lo]).v0.Y > Mid) Lo++;
					while (((SlicerBase.Edge)base[Hi]).v0.Y < Mid) Hi--;

					if (Lo <= Hi)
					{
						SlicerBase.Edge T = (SlicerBase.Edge)base[Lo];
						base[Lo] = base[Hi];
						base[Hi] = T;
						Lo++;
						Hi--;
					}
				} while (Lo < Hi);

				if (Hi > iLo) Sort(iLo, Hi);
				iLo = Lo;
			} while (Lo < iHi);
		}
	}

	//IE table must be sorted by v0.Y
	class IETableNeg : SortedArrayList
	{
		private double selectPivot(int iLo, int iHi)
		{
			int count = (int)(6 * Math.Log10(iHi - iLo));
			count = Math.Min(count, 1024);
			int step = (iHi - iLo) / count;
			int i = iLo + count * ((int)(0.5 * (iHi - iLo) / count));

			return ((SlicerBase.Edge)base[i]).v0.Y;
		}

		override public void Sort(int iLo, int iHi)
		{
			int Lo = iLo;

			do
			{
				if (iHi - iLo < InsertionThreshold)
				{
					for (int i = iLo + 1; i <= iHi; i++)
					{
						SlicerBase.Edge x = (SlicerBase.Edge)base[i];

						int Lu = iLo, Ha = i - 1;

						while (Lu <= Ha)
						{
							int m = (Lu + Ha) >> 1;
							if (((SlicerBase.Edge)base[m]).v0.Y <= x.v0.Y)
								Lu = m + 1;
							else
								Ha = m - 1;
						}

						for (int j = i - 1; j >= Lu; j--)
							base[j + 1] = base[j];

						base[Lu] = x;
					}
					return;
				}

				int Hi = iHi;
				double Mid = ((SlicerBase.Edge)base[(Lo + Hi) >> 1]).v0.Y;	//chose pivot

				//if (((Edge)base[Lo]).v0.Y > Mid && ((Edge)base[Lo]).v0.Y < ((Edge)base[Hi]).v0.Y)
				//    Mid = ((Edge)base[Lo]).v0.Y;
				//else if (((Edge)base[Hi]).v0.Y > Mid && ((Edge)base[Lo]).v0.Y < ((Edge)base[Lo]).v0.Y)
				//    Mid = ((Edge)base[Hi]).v0.Y;

				//double Mid = selectPivot(iLo, iHi);

				do
				{
					while (((SlicerBase.Edge)base[Lo]).v0.Y < Mid) Lo++;
					while (((SlicerBase.Edge)base[Hi]).v0.Y > Mid) Hi--;

					if (Lo <= Hi)
					{
						SlicerBase.Edge T = (SlicerBase.Edge)base[Lo];
						base[Lo] = base[Hi];
						base[Hi] = T;
						Lo++;
						Hi--;
					}
				} while (Lo < Hi);

				if (Hi > iLo) Sort(iLo, Hi);
				iLo = Lo;
			} while (Lo < iHi);
		}
	}

	//Span table must be sorted by X0
	public class SpanTable : SortedArrayList
	{
		private double selectPivot(int iLo, int iHi)
		{
			int count = (int)(6 * Math.Log10(iHi - iLo));
			count = Math.Min(count, 1024);
			int step = (iHi - iLo) / count;
			int i = iLo + count * ((int)(0.5 * (iHi - iLo) / count));

			return ((SlicerBase.Span)base[i]).X0;
		}

		override public void Sort(int iLo, int iHi)
		{
			int Lo = iLo;

			do
			{
				if (iHi - iLo < InsertionThreshold)
				{
					for (int i = iLo + 1; i <= iHi; i++)
					{
						SlicerBase.Span x = (SlicerBase.Span)base[i];

						int Lu = iLo, Ha = i - 1;

						while (Lu <= Ha)
						{
							int m = (Lu + Ha) >> 1;
							if (((SlicerBase.Span)base[m]).X0 <= x.X0)
								Lu = m + 1;
							else
								Ha = m - 1;
						}

						for (int j = i - 1; j >= Lu; j--)
							base[j + 1] = base[j];

						base[Lu] = x;
					}
					return;
				}

				int Hi = iHi;
				//double Mid = selectPivot(iLo, iHi);
				double Mid = ((SlicerBase.Span)base[(Lo + Hi) >> 1]).X0;	//chose pivot

				do
				{
					while (((SlicerBase.Span)base[Lo]).X0 < Mid) Lo++;
					while (((SlicerBase.Span)base[Hi]).X0 > Mid) Hi--;

					if (Lo <= Hi)
					{
						SlicerBase.Span T = (SlicerBase.Span)base[Lo];
						base[Lo] = base[Hi];
						base[Hi] = T;
						Lo++;
						Hi--;
					}
				} while (Lo < Hi);

				if (Hi > iLo) Sort(iLo, Hi);
				iLo = Lo;
			} while (Lo < iHi);
		}
	}
}
