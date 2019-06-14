using System;
using System.Windows.Forms;
using Aran.Geometries;
using Aran.PANDA.Common;

namespace Aran.PANDA.RNAV.Approach
{
	internal class Parameter
	{
		internal Action<object, EventArgs> OnValueChanged;

		public bool Active { get; internal set; }
		public Point InConvertionPoint { get; internal set; }
		public Interval Range { get; internal set; }
		public double MaxValue { get; internal set; }
		public double MinValue { get; internal set; }

		public double Value { get; internal set; }
		public bool Visible { get; internal set; }
		public bool ReadOnly { get; internal set; }

		public Label Description { get; internal set; }
		public Label Unit { get; internal set; }
		public TextBox Text { get; internal set; }

		public void BeginUpdate()
		{

		}

		internal void EndUpdate()
		{

		}

		internal void ChangeValue(int v)
		{

		}

		internal void ChangeValue(double v)
		{
		}

	}
}