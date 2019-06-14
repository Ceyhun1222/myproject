using System;
using Aran.Package;

namespace Aran.AranEnvironment.Symbols
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
		sfsHollow = eFillStyle.sfsNull,
		sfsHorizontal,
		sfsVertical,
		sfsForwardDiagonal,
		sfsBackwardDiagonal,
		sfsCross,
		sfsDiagonalCross		
	}

	//----------------------------------------------------------------------------
	public class BaseSymbol : IPackable
	{
		protected Int32 _color;
        protected Int32 _size;

		protected BaseSymbol(Int32 InitColor, Int32 InitSize)
			: base()
		{
			_color = InitColor;
			_size = InitSize;
		}

		public BaseSymbol()
			: base()
		{
			System.Random rnd = new System.Random();
			_color = ( int ) ( rnd.Next ( 256 ) | ( rnd.Next ( 256 ) << 8 ) | ( rnd.Next ( 256 ) << 16 ) );
			_size = 1;
		}

        public virtual void Assign (BaseSymbol src)
		{
			_color = src._color;
			_size = src._size;
		}

        public virtual BaseSymbol Clone ()
		{
			return new BaseSymbol(_color, _size);
		}

        public virtual void AssignTo (BaseSymbol dst)
		{
			dst.Assign(this);
		}

		public Int32 Size
		{
			get { return _size; }
			set { _size = value; }
		}

		public Int32 Color
		{
			get { return _color; }
			set { _color = value; }
		}

        public virtual void Pack (PackageWriter writer)
        {
            writer.PutInt32 (_color);
            writer.PutInt32 (_size);
        }

        public virtual void Unpack (PackageReader reader)
        {
            _color = reader.GetInt32 ();
            _size = reader.GetInt32 ();
        }
    }
	//----------------------------------------------------------------------------
	public class PointSymbol : BaseSymbol, IPackable
	{
		protected ePointStyle _style;

		public PointSymbol()
			: base()
		{
			_style = ePointStyle.smsSquare;
			_size = 5;
		}

		public PointSymbol(Int32 InitColor, Int32 InitSize)
			: base(InitColor, InitSize)
		{
		}

		public PointSymbol(ePointStyle InitStyle, Int32 InitColor, Int32 InitSize)
			: base(InitColor, InitSize)
		{
			_style = InitStyle;
		}

		public override void Assign(BaseSymbol src)
		{
			base.Assign(src);
			_style = ((PointSymbol)src)._style;
		}

		public override BaseSymbol Clone()
		{
			return new PointSymbol(_style, _color, _size);
		}

		public ePointStyle Style
		{
			get { return _style; }
			set { _style = value; }
		}

        public override void Pack (PackageWriter writer)
        {
            base.Pack (writer);
            writer.PutInt32 ((Int32) _style);
        }

        public override void Unpack (PackageReader reader)
        {
            base.Unpack (reader);
            _style = (ePointStyle) reader.GetInt32 ();
        }
	}

	//----------------------------------------------------------------------------
	public class LineSymbol : BaseSymbol
	{
		private eLineStyle _style;
		
        public LineSymbol()
			: base()
		{
			_style = eLineStyle.slsSolid;
		}

		public LineSymbol(eLineStyle InitStyle, Int32 InitColor, Int32 InitSize)
			: base(InitColor, InitSize)
		{
			_style = InitStyle;
		}

		public override void Assign(BaseSymbol src)
		{
			base.Assign(src);
			_style = ((LineSymbol)src)._style;
		}

		public override BaseSymbol Clone()
		{
			return new LineSymbol(_style, _color, _size);
		}

		public eLineStyle Style
		{
			get { return _style; }
			set { _style = value; }
		}

		public int Width
		{
            get { return _size; }
            set { _size = value; }
		}

        public override void Pack (PackageWriter writer)
        {
            base.Pack (writer);
            writer.PutInt32 ((Int32) _style);
        }

        public override void Unpack (PackageReader reader)
        {
            base.Unpack (reader);
            _style = (eLineStyle) reader.GetInt32 ();
        }
	}
	//----------------------------------------------------------------------------

	public class FillSymbol : BaseSymbol
	{
		private eFillStyle _style;
		private LineSymbol _outline;

		private void SetOutline(LineSymbol Val)
		{
			_outline.Assign(Val);
		}

		public FillSymbol()
			: base()
		{
			_style = eFillStyle.sfsSolid;
			_outline = new LineSymbol();
		}

		public override void Assign(BaseSymbol src)
		{
			base.Assign(src);
			_style = ((FillSymbol)src)._style;
			_outline.Assign(((FillSymbol)src)._outline);
		}

		public override BaseSymbol Clone()
		{
			FillSymbol result = new FillSymbol();
			result.Assign(this);
			return result;
		}

		public eFillStyle Style
		{
			get { return _style; }
			set { _style = value; }
		}

		public LineSymbol Outline
		{
			get { return _outline; }
			set { _outline.Assign(value); }
		}

        public override void Pack (PackageWriter writer)
        {
            base.Pack (writer);
            writer.PutInt32 ((Int32) _style);
            _outline.Pack (writer);
        }

        public override void Unpack (PackageReader reader)
        {
            base.Unpack (reader);
            _style = (eFillStyle) reader.GetInt32 ();
            _outline.Unpack (reader);
        }
	}
}
