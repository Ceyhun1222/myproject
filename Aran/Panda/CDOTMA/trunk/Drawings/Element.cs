using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CDOTMA.Drawings
{
	public interface IElement
	{
		int Color { get; set; }
		int Style { get; set; }
	}

	public static class PointElementStyle
	{
		public const int
			//
			// Summary:
			//     Specifies a circle.
		Circle = 0,
			//
			// Summary:
			//     Specifies a square.
		Square = 1,
			//
			// Summary:
			//     Specifies a Cross.
		Cross = 2,
			//
			// Summary:
			//     Specifies a X.
		X = 3,
			//
			// Summary:
			//     Specifies a Diamond.
		Diamond = 4;
	}

	public class PointElement : IElement
	{
		public int Color { get; set; }
		public int Style { get; set; }		//PointStyle
		public int Size { get; set; }		//float
	}

	public static class LineElementStyle
	{
		public const int
			//
			// Summary:
			//     Specifies a solid line.
		Solid = 0,
			//
			// Summary:
			//     Specifies a line consisting of dashes.
		Dash = 1,
			//
			// Summary:
			//     Specifies a line consisting of dots.
		Dot = 2,
			//
			// Summary:
			//     Specifies a line consisting of a repeating pattern of dash-dot.
		DashDot = 3,
			//
			// Summary:
			//     Specifies a line consisting of a repeating pattern of dash-dot-dot.
		DashDotDot = 4,
			//
			// Summary:
			//     Specifies a null style.
		Null = 5;
	}

	public class LineElement : IElement
	{
		public int Color { get; set; }
		public int Style { get; set; }		//LineStyle
		public int Width { get; set; }		//float
	}

	public static class FillElementStyle
	{
		public const int
			//
			// Summary:
			//     Specifies Hollow style.
		Null = 0,
			//
			// Summary:
			//     Specifies Hollow style.
		Hollow = 0,
			//
			// Summary:
			//     Specifies hatch style System.Drawing.Drawing2D.HatchStyle.Horizontal.
		Min = 1,
			//
			// Summary:
			//     A pattern of horizontal lines.
		Horizontal = 1,
			//
			// Summary:
			//     A pattern of vertical lines.
		Vertical = 2,
			//
			// Summary:
			//     A pattern of lines on a diagonal from upper left to lower right.
		ForwardDiagonal = 3,
			//
			// Summary:
			//     A pattern of lines on a diagonal from upper right to lower left.
		BackwardDiagonal = 4,
			//
			// Summary:
			//     Specifies hatch style System.Drawing.Drawing2D.HatchStyle.SolidDiamond.
		Max = 5,
			//
			// Summary:
			//     Specifies horizontal and vertical lines that cross.
		Cross = 5,
			//
			// Summary:
			//     Specifies the hatch style System.Drawing.Drawing2D.HatchStyle.Cross.
		LargeGrid = 5,
			//
			// Summary:
			//     A pattern of crisscross diagonal lines.
		DiagonalCross = 6,
			//
			// Summary:
			//     Specifies a 5-percent hatch. The ratio of foreground color to background
			//     color is 5:95.
		Percent05 = 7,
			//
			// Summary:
			//     Specifies a 10-percent hatch. The ratio of foreground color to background
			//     color is 10:90.
		Percent10 = 8,
			//
			// Summary:
			//     Specifies a 20-percent hatch. The ratio of foreground color to background
			//     color is 20:80.
		Percent20 = 9,
			//
			// Summary:
			//     Specifies a 25-percent hatch. The ratio of foreground color to background
			//     color is 25:75.
		Percent25 = 10,
			//
			// Summary:
			//     Specifies a 30-percent hatch. The ratio of foreground color to background
			//     color is 30:70.
		Percent30 = 11,
			//
			// Summary:
			//     Specifies a 40-percent hatch. The ratio of foreground color to background
			//     color is 40:60.
		Percent40 = 12,
			//
			// Summary:
			//     Specifies a 50-percent hatch. The ratio of foreground color to background
			//     color is 50:50.
		Percent50 = 13,
			//
			// Summary:
			//     Specifies a 60-percent hatch. The ratio of foreground color to background
			//     color is 60:40.
		Percent60 = 14,
			//
			// Summary:
			//     Specifies a 70-percent hatch. The ratio of foreground color to background
			//     color is 70:30.
		Percent70 = 15,
			//
			// Summary:
			//     Specifies a 75-percent hatch. The ratio of foreground color to background
			//     color is 75:25.
		Percent75 = 16,
			//
			// Summary:
			//     Specifies a 80-percent hatch. The ratio of foreground color to background
			//     color is 80:100.
		Percent80 = 17,
			//
			// Summary:
			//     Specifies a 90-percent hatch. The ratio of foreground color to background
			//     color is 90:10.
		Percent90 = 18,
			//
			// Summary:
			//     Specifies diagonal lines that slant to the right from top points to bottom
			//     points and are spaced 50 percent closer together than System.Drawing.Drawing2D.HatchStyle.ForwardDiagonal,
			//     but are not antialiased.
		LightDownwardDiagonal = 19,
			//
			// Summary:
			//     Specifies diagonal lines that slant to the left from top points to bottom
			//     points and are spaced 50 percent closer together than System.Drawing.Drawing2D.HatchStyle.BackwardDiagonal,
			//     but they are not antialiased.
		LightUpwardDiagonal = 20,
			//
			// Summary:
			//     Specifies diagonal lines that slant to the right from top points to bottom
			//     points, are spaced 50 percent closer together than, and are twice the width
			//     of System.Drawing.Drawing2D.HatchStyle.ForwardDiagonal. This hatch pattern
			//     is not antialiased.
		DarkDownwardDiagonal = 21,
			//
			// Summary:
			//     Specifies diagonal lines that slant to the left from top points to bottom
			//     points, are spaced 50 percent closer together than System.Drawing.Drawing2D.HatchStyle.BackwardDiagonal,
			//     and are twice its width, but the lines are not antialiased.
		DarkUpwardDiagonal = 22,
			//
			// Summary:
			//     Specifies diagonal lines that slant to the right from top points to bottom
			//     points, have the same spacing as hatch style System.Drawing.Drawing2D.HatchStyle.ForwardDiagonal,
			//     and are triple its width, but are not antialiased.
		WideDownwardDiagonal = 23,
			//
			// Summary:
			//     Specifies diagonal lines that slant to the left from top points to bottom
			//     points, have the same spacing as hatch style System.Drawing.Drawing2D.HatchStyle.BackwardDiagonal,
			//     and are triple its width, but are not antialiased.
		WideUpwardDiagonal = 24,
			//
			// Summary:
			//     Specifies vertical lines that are spaced 50 percent closer together than
			//     System.Drawing.Drawing2D.HatchStyle.Vertical.
		LightVertical = 25,
			//
			// Summary:
			//     Specifies horizontal lines that are spaced 50 percent closer together than
			//     System.Drawing.Drawing2D.HatchStyle.Horizontal.
		LightHorizontal = 26,
			//
			// Summary:
			//     Specifies vertical lines that are spaced 75 percent closer together than
			//     hatch style System.Drawing.Drawing2D.HatchStyle.Vertical (or 25 percent closer
			//     together than System.Drawing.Drawing2D.HatchStyle.LightVertical).
		NarrowVertical = 27,
			//
			// Summary:
			//     Specifies horizontal lines that are spaced 75 percent closer together than
			//     hatch style System.Drawing.Drawing2D.HatchStyle.Horizontal (or 25 percent
			//     closer together than System.Drawing.Drawing2D.HatchStyle.LightHorizontal).
		NarrowHorizontal = 28,
			//
			// Summary:
			//     Specifies vertical lines that are spaced 50 percent closer together than
			//     System.Drawing.Drawing2D.HatchStyle.Vertical and are twice its width.
		DarkVertical = 29,
			//
			// Summary:
			//     Specifies horizontal lines that are spaced 50 percent closer together than
			//     System.Drawing.Drawing2D.HatchStyle.Horizontal and are twice the width of
			//     System.Drawing.Drawing2D.HatchStyle.Horizontal.
		DarkHorizontal = 30,
			//
			// Summary:
			//     Specifies dashed diagonal lines, that slant to the right from top points
			//     to bottom points.
		DashedDownwardDiagonal = 31,
			//
			// Summary:
			//     Specifies dashed diagonal lines, that slant to the left from top points to
			//     bottom points.
		DashedUpwardDiagonal = 32,
			//
			// Summary:
			//     Specifies dashed horizontal lines.
		DashedHorizontal = 33,
			//
			// Summary:
			//     Specifies dashed vertical lines.
		DashedVertical = 34,
			//
			// Summary:
			//     Specifies a hatch that has the appearance of confetti.
		SmallConfetti = 35,
			//
			// Summary:
			//     Specifies a hatch that has the appearance of confetti, and is composed of
			//     larger pieces than System.Drawing.Drawing2D.HatchStyle.SmallConfetti.
		LargeConfetti = 36,
			//
			// Summary:
			//     Specifies horizontal lines that are composed of zigzags.
		ZigZag = 37,
			//
			// Summary:
			//     Specifies horizontal lines that are composed of tildes.
		Wave = 38,
			//
			// Summary:
			//     Specifies a hatch that has the appearance of layered bricks that slant to
			//     the left from top points to bottom points.
		DiagonalBrick = 39,
			//
			// Summary:
			//     Specifies a hatch that has the appearance of horizontally layered bricks.
		HorizontalBrick = 40,
			//
			// Summary:
			//     Specifies a hatch that has the appearance of a woven material.
		Weave = 41,
			//
			// Summary:
			//     Specifies a hatch that has the appearance of a plaid material.
		Plaid = 42,
			//
			// Summary:
			//     Specifies a hatch that has the appearance of divots.
		Divot = 43,
			//
			// Summary:
			//     Specifies horizontal and vertical lines, each of which is composed of dots,
			//     that cross.
		DottedGrid = 44,
			//
			// Summary:
			//     Specifies forward diagonal and backward diagonal lines, each of which is
			//     composed of dots, that cross.
		DottedDiamond = 45,
			//
			// Summary:
			//     Specifies a hatch that has the appearance of diagonally layered shingles
			//     that slant to the right from top points to bottom points.
		Shingle = 46,
			//
			// Summary:
			//     Specifies a hatch that has the appearance of a trellis.
		Trellis = 47,
			//
			// Summary:
			//     Specifies a hatch that has the appearance of spheres laid adjacent to one
			//     another.
		Sphere = 48,
			//
			// Summary:
			//     Specifies horizontal and vertical lines that cross and are spaced 50 percent
			//     closer together than hatch style System.Drawing.Drawing2D.HatchStyle.Cross.
		SmallGrid = 49,
			//
			// Summary:
			//     Specifies a hatch that has the appearance of a checkerboard.
		SmallCheckerBoard = 50,
			//
			// Summary:
			//     Specifies a hatch that has the appearance of a checkerboard with squares
			//     that are twice the size of System.Drawing.Drawing2D.HatchStyle.SmallCheckerBoard.
		LargeCheckerBoard = 51,
			//
			// Summary:
			//     Specifies forward diagonal and backward diagonal lines that cross but are
			//     not antialiased.
		OutlinedDiamond = 52,
			//
			// Summary:
			//     Specifies a hatch that has the appearance of a checkerboard placed diagonally.
		SolidDiamond = 53,
			//
			// Summary:
			//     Specifies solid style.
		Solid = 54;
	}

	public class FillElement : IElement
	{
		public int Color { get; set; }
		public int Style { get; set; }		//FillStyle
	}
}
