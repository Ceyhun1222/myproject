using System;
using ARAN.Contracts.Registry;

namespace ARAN.Contracts.UI
{
	public enum ePointStyle
	{
		smsCircle,
		smsSquare,
		smsCross,
		smsX,
		smsDiamond
	};

	public enum eLineStyle
	{
		slsSolid,
		slsDash,
		slsDot,
		slsDashDot,
		slsDashDotDot,
		slsNull,
		slsInsideFrame
	}

	public enum eFillStyle
	{
		sfsSolid,
		sfsNull,
		sfsHorizontal,
		sfsVertical,
		sfsForwardDiagonal,
		sfsBackwardDiagonal,
		sfsCross,
		sfsDiagonalCross,
		sfsHollow = eFillStyle.sfsNull
	}

	//----------------------------------------------------------------------------
	public class BaseSymbol
	{
		protected Int32 FColor, FSize;

		protected BaseSymbol(Int32 InitColor, Int32 InitSize)
			: base()
		{
			FColor = InitColor;
			FSize = InitSize;
		}

		public BaseSymbol()
			: base()
		{
			System.Random rnd = new System.Random();
			FColor = UIContract.RGB(rnd.Next(256), rnd.Next(256), rnd.Next(256));
			FSize = 1;
			//rnd.Finalize();
		}

		virtual public void Pack(Int32 handle)
		{
			Registry_Contract.PutInt32(handle, FColor);
			Registry_Contract.PutInt32(handle, FSize);
		}

		virtual public void UnPack(Int32 handle)
		{
			FColor = Registry_Contract.GetInt32(handle);
			FSize = Registry_Contract.GetInt32(handle);
		}

		virtual public void Assign(BaseSymbol src)
		{
			FColor = src.FColor;
			FSize = src.FSize;
		}

		virtual public BaseSymbol Clone()
		{
			return new BaseSymbol(FColor, FSize);
		}

		virtual public void AssignTo(BaseSymbol dst)
		{
			dst.Assign(this);
		}

		public Int32 Size
		{
			get { return FSize; }
			set { FSize = value; }
		}

		public Int32 Color
		{
			get { return FColor; }
			set { FColor = value; }
		}
	}
	//----------------------------------------------------------------------------
	public class PointSymbol : BaseSymbol
	{
		protected ePointStyle FStyle;

		public PointSymbol()
			: base()
		{
			FStyle = ePointStyle.smsSquare;
			FSize = 5;
		}

		public PointSymbol(Int32 InitColor, Int32 InitSize)
			: base(InitColor, InitSize)
		{
		}

		public PointSymbol(ePointStyle InitStyle, Int32 InitColor, Int32 InitSize)
			: base(InitColor, InitSize)
		{
			FStyle = InitStyle;
		}

		public override void Pack(Int32 handle)
		{
			base.Pack(handle);
			Registry_Contract.PutInt32(handle, (Int32)FStyle);
		}

		public override void UnPack(Int32 handle)
		{
			base.UnPack(handle);
			FStyle = (ePointStyle)Registry_Contract.GetInt32(handle);
		}

		public override void Assign(BaseSymbol src)
		{
			base.Assign(src);
			FStyle = ((PointSymbol)src).FStyle;
		}

		public override BaseSymbol Clone()
		{
			return new PointSymbol(FStyle, FColor, FSize);
		}

		public ePointStyle Style
		{
			get { return FStyle; }
			set { FStyle = value; }
		}
	}

	//----------------------------------------------------------------------------
	public class LineSymbol : BaseSymbol
	{
		private eLineStyle FStyle;
		public LineSymbol()
			: base()
		{
			FStyle = eLineStyle.slsSolid;
		}

		public LineSymbol(eLineStyle InitStyle, Int32 InitColor, Int32 InitSize)
			: base(InitColor, InitSize)
		{
			FStyle = InitStyle;
		}

		public override void Pack(Int32 handle)
		{
			base.Pack(handle);
			Registry_Contract.PutInt32(handle, (Int32)FStyle);
		}

		public override void UnPack(Int32 handle)
		{
			base.UnPack(handle);
			FStyle = (eLineStyle)Registry_Contract.GetInt32(handle);
		}

		public override void Assign(BaseSymbol src)
		{
			base.Assign(src);
			FStyle = ((LineSymbol)src).FStyle;
		}

		public override BaseSymbol Clone()
		{
			return new LineSymbol(FStyle, FColor, FSize);
		}

		eLineStyle Style
		{
			get { return FStyle; }
			set { FStyle = value; }
		}
	}
	//----------------------------------------------------------------------------

	public class FillSymbol : BaseSymbol
	{
		private eFillStyle FStyle;
		private LineSymbol FOutline;
		private void SetOutline(LineSymbol Val)
		{
			FOutline.Assign(Val);
		}

		public FillSymbol()
			: base()
		{
			FStyle = eFillStyle.sfsSolid;
			FOutline = new LineSymbol();
		}

		public override void Pack(Int32 handle)
		{
			base.Pack(handle);
			Registry_Contract.PutInt32(handle, (Int32)FStyle);
			FOutline.Pack(handle);
		}

		public override void UnPack(Int32 handle)
		{
			base.UnPack(handle);
			FStyle = (eFillStyle)Registry_Contract.GetInt32(handle);
			FOutline.UnPack(handle);
		}

		public override void Assign(BaseSymbol src)
		{
			base.Assign(src);
			FStyle = ((FillSymbol)src).FStyle;
			FOutline.Assign(((FillSymbol)src).FOutline);
		}

		public override BaseSymbol Clone()
		{
			FillSymbol result = new FillSymbol();
			result.Assign(this);
			return result;
		}

		public eFillStyle Style
		{
			get { return FStyle; }
			set { FStyle = value; }
		}

		public LineSymbol Outline
		{
			get { return FOutline; }
			set { FOutline.Assign(value); }
		}
	}
}
