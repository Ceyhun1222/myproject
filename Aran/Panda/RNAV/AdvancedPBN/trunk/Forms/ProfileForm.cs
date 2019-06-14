using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim.Enums;
using Aran.PANDA.Common;

namespace Aran.PANDA.RNAV.SGBAS
{
	public partial class ProfileForm : Form
	{
		private const int LeftMargin = 10;
		private const int RightMargin = 10;
		private const int TopMargin = 10;
		private const int BottomMargin = 10;

		private const int InnerTopMargin = 30;
		private const int InnerBottomMargin = 30;
		private const int InnerLeftMargin = 100;
		private const int InnerRightMargin = 100;

		private const int MaxPoints = 10;

		private const int ArrowLen = 30;
		private const int ArrowAngle = 20;
		private const int ArrowWingLen = 15;
		private const int ArrowWingAngle = 20;

		private const int TextPadding = 2;
		private const int LegendWidth = 60;

		private int RWYDir;
		private double RWYLen;
		private double RWYAlt;
		private double fRefHeight;

		private double LeftX;
		private double RightX;
		private double TopY;
		private double BottomY;

		private double ScaleWidth; //Single
		private double ScaleHeight; // Single

		private System.Windows.Forms.CheckBox ContainerCheck;
		private ProfilePoint[] Points = new ProfilePoint[MaxPoints];

		private bool bFormInitialised = false;

		public int PointsNo;
		public int MAPtIndex;

		#region Form
		public ProfileForm()
		{
			InitializeComponent();
			PictureBox1.Image = new Bitmap(PictureBox1.ClientSize.Width, PictureBox1.ClientSize.Height);
		}

		private void ProfileForm_Load(object sender, EventArgs e)
		{
			Text = "Approach profile";
			bFormInitialised = true;
		}

		private void ProfileForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == System.Windows.Forms.CloseReason.UserClosing)
			{
				e.Cancel = true;
				Hide();
				ContainerCheck.Checked = false;
			}
		}

		private void ProfileForm_Resize(object sender, EventArgs e)
		{
			if (!bFormInitialised)
				return;

			int wd = PictureBox1.ClientSize.Width;
			int hg = PictureBox1.ClientSize.Height;

			PictureBox1.Image = new Bitmap(wd, hg);
			ReDrawGraphics();
		}

		#endregion

		#region Point management
		public void InitWOFAF(double RWY_Len, int RWY_Dir, double RWY_H, double RefHeight, System.Windows.Forms.CheckBox MyCheck)
		{
			RWYLen = RWY_Len;
			RWYAlt = RWY_H - RefHeight;
			RWYDir = RWY_Dir;

			MAPtIndex = 0;
			PointsNo = 0;

			LeftX = -RWYLen;
			RightX = RWYLen;
			TopY = 100;
			BottomY = 0;

			ContainerCheck = MyCheck;
			fRefHeight = RefHeight;
			ReDrawGraphics();
		}

		public bool AddPoint(double X, double Y, double Course, double PDG, CodeProcedureDistance Role, int IsFinal = 0)
		{
			if (PointsNo >= MaxPoints)
				return false;

			Points[PointsNo].X = -RWYDir * X;
			Points[PointsNo].Y = Y;
			Points[PointsNo].Course = Course;
			Points[PointsNo].PDG = PDG;
			Points[PointsNo].Role = Role;
			PointsNo++;

			ReDrawGraphics();
			return true;
		}

		public bool InsertPoint(double X, double Y, double Course, double PDG, CodeProcedureDistance Role, int Index = MaxPoints - 1, int IsFinal = 0)
		{
			if (PointsNo >= MaxPoints)
				return false;

			if (Index > PointsNo) //return false;
				Index = PointsNo;

			if (Index < PointsNo)
				for (int i = PointsNo; i > Index; i--)
					Points[i] = Points[i - 1];

			Points[Index].X = -RWYDir * X;
			Points[Index].Y = Y;
			Points[Index].Course = Course;
			Points[Index].PDG = PDG;
			Points[Index].Role = Role;
			PointsNo++;

			ReDrawGraphics();
			return true;
		}

		public bool ReplacePoint(double X, double Y, double Course, double PDG, CodeProcedureDistance Role, int Index = MaxPoints - 1, int IsFinal = 0)
		{
			if (Index >= PointsNo)
				return false;

			Points[Index].X = -RWYDir * X;
			Points[Index].Y = Y;
			Points[Index].Course = Course;
			Points[Index].PDG = PDG;
			Points[Index].Role = Role;

			ReDrawGraphics();
			return true;
		}

		public ProfilePoint GetPoint(int Index)
		{
			ProfilePoint result = Points[Index];
			result.X = -RWYDir * Points[Index].X;
			return result;
		}

		public string GetPointRole(int Index)
		{
			if (Points[Index].Role < CodeProcedureDistance.HAT)
				return "";

			string[] strArray = new string[] { "HAT", "OM", "MM", "IM", "PFAF", "GSANT", "FAF", "MAP", "THLD", "VDP", "RECH", "SDF" };
			return strArray[(int)Points[Index].Role];
		}

		public bool RemovePoint()
		{
			if (PointsNo <= 0)
				return false;

			PointsNo--;

			ReDrawGraphics();
			return true;
		}

		public bool RemovePointByIndex(int Index)
		{
			if (PointsNo <= 0)
				return false;

			for (int i = Index; i < PointsNo - 1; i++)
				Points[i] = Points[i + 1];

			PointsNo--;

			ReDrawGraphics();
			return true;
		}

		public void ClearPoints()
		{
			PointsNo = 0;
			ReDrawGraphics();
		}

		#endregion

		#region Drawing

		private void UpdateScales()
		{
			// Matrix mx = g.Transform;
			// mx.RotateAt(Angle, new PointF(Location.X + (Area.Right - Location.X) / 2, Location.Y + (Area.Bottom - Location.Y) / 2), MatrixOrder.Append);
			// g.Transform = mx;
			// g.FillRectangle(new SolidBrush(BackColor), Rect);
			// g.DrawString(Data, TextFont, new SolidBrush(ForeColor), Rect, TextFormat);
			// g.DrawRectangle(Pen, Rect.X, Rect.Y, Rect.Width, Rect.Height);

			LeftX = -RWYLen;
			RightX = RWYLen;
			TopY = 100;
			BottomY = RWYAlt;

			for (int i = 0; i < PointsNo; i++)
			{
				if (Points[i].X < LeftX) LeftX = Points[i].X;
				if (Points[i].X > RightX) RightX = Points[i].X;
				if (Points[i].Y < BottomY) BottomY = Points[i].Y;
				if (Points[i].Y > TopY) TopY = Points[i].Y;
			}

			double dLeft = System.Math.Abs(LeftMargin + InnerLeftMargin);
			double dRight = System.Math.Abs(RightMargin + InnerRightMargin);

			double dBottom = System.Math.Abs(BottomMargin + InnerBottomMargin);
			double dTop = System.Math.Abs(TopMargin + InnerTopMargin);

			ScaleWidth = (PictureBox1.ClientSize.Width - dLeft - dRight) / (RightX - LeftX);
			ScaleHeight = (PictureBox1.ClientSize.Height - dBottom - dTop) / (TopY - BottomY);
		}

		private float TransformX(double X)
		{
			return (float)(InnerLeftMargin + LeftMargin + ScaleWidth * (X - LeftX));	//RWYDir * 
		}

		private float TransformY(double Y)
		{
			return (float)(PictureBox1.ClientSize.Height - (InnerBottomMargin + BottomMargin + ScaleHeight * (Y - BottomY)));
		}

		private void ReDrawGraphics()
		{
			SolidBrush pBrush = new SolidBrush(Color.Black);
			PointF pPoint = new PointF();
			Graphics imgGraphics = Graphics.FromImage(PictureBox1.Image);

			//RectangleF rect = new RectangleF(0, 0, 1200, 900);
			//imgGraphics.Clip = new Region(rect);

			imgGraphics.Clear(Color.White);
			pPoint.Y = TransformY(0.0);

			DrawScreen();
			UpdateScales();
			DrawZeroLine();
			DrawRWY();

			ForeColor = System.Drawing.Color.Red;

			for (int i = 1; i < PointsNo; i++)
			{
				DrawSegment(Points[i - 1], Points[i]);

				double dX = GlobalVars.unitConverter.DistanceToDisplayUnits(Points[i - 1].X - Points[i].X, eRoundMode.NEAREST);
				if (System.Math.Abs(dX) > GlobalVars.unitConverter.DistancePrecision)
				{
					string TextToDraw = System.Math.Abs(dX).ToString();

					SizeF stringSize = imgGraphics.MeasureString(TextToDraw, Font);
					pPoint.X = TransformX(0.5 * (Points[i].X + Points[i - 1].X)) - 0.5f * stringSize.Width;
					imgGraphics.DrawString(TextToDraw, this.Font, pBrush, pPoint);
				}
			}

			double PrevH = Points[0].Y - 2;
			for (int i = 1; i < PointsNo; i++)
			{
				DrawPoint(Points[i], 5);
				DrawTick(Points[i]);
				if (System.Math.Abs(PrevH - Points[i].Y) >= 1)
				{
					DrawHLegend(Points[i], RWYDir);
					PrevH = Points[i].Y;
				}
			}

			// if(IsFinalPoint) DrawArrow (Points[PointsNo - 1]);

			PictureBox1.Refresh();
		}

		private void DrawRWY()
		{
			ProfilePoint pPtTHR = default(ProfilePoint);
			Graphics imgGraphics = Graphics.FromImage(PictureBox1.Image);
			Pen pPen = new Pen(Color.Cyan, 4);

			pPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;

			pPtTHR.X = 0;
			pPtTHR.Y = RWYAlt;

			imgGraphics.DrawLine(pPen, TransformX(pPtTHR.X), TransformY(pPtTHR.Y), TransformX(pPtTHR.X + RWYDir * RWYLen), TransformY(pPtTHR.Y));
			DrawHLegend(pPtTHR, RWYDir, true);
		}

		private void DrawZeroLine()
		{
			Graphics imgGraphics = Graphics.FromImage(PictureBox1.Image);
			Pen pPen = new Pen(Color.Black, 1);		//Pen(Color.Red, 1);
			imgGraphics.DrawLine(pPen, LeftMargin, TransformY(0), PictureBox1.ClientSize.Width - RightMargin, TransformY(0));
		}

		private void DrawTick(ProfilePoint pPt)
		{
			Graphics imgGraphics = Graphics.FromImage(PictureBox1.Image);
			Pen pPen = new Pen(Color.DarkGray, 1);
			pPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash; // .Dot
			imgGraphics.DrawLine(pPen, TransformX(pPt.X), TransformY(pPt.Y), TransformX(pPt.X), TransformY(0));
		}

		private void DrawScreen()
		{
			Graphics imgGraphics = Graphics.FromImage(PictureBox1.Image);
			Rectangle rectangle1 = new Rectangle(LeftMargin, TopMargin, PictureBox1.ClientSize.Width - LeftMargin - RightMargin, PictureBox1.ClientSize.Height - BottomMargin - TopMargin);
			Pen pPen = new Pen(Color.Black, 2);
			pPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
			imgGraphics.DrawRectangle(pPen, rectangle1);
		}

		private void DrawSegment(ProfilePoint pPt1, ProfilePoint pPt2)
		{
			Graphics imgGraphics = Graphics.FromImage(PictureBox1.Image);
			Pen pPen = new Pen(Color.Black, 2);
			pPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
			imgGraphics.DrawLine(pPen, TransformX(pPt1.X), TransformY(pPt1.Y), TransformX(pPt2.X), TransformY(pPt2.Y));
		}

		private void DrawPoint(ProfilePoint pPt, float R)
		{
			Graphics imgGraphics = Graphics.FromImage(PictureBox1.Image);
			SolidBrush pBrush = new SolidBrush(Color.Black);
			float X = TransformX(pPt.X);
			float Y = TransformY(pPt.Y);
			RectangleF rect = new RectangleF(X - R, Y - R, 2 * R, 2 * R);

			imgGraphics.FillEllipse(pBrush, rect);
		}

		private void DrawHLegend(ProfilePoint pPtFrom, int Direction, bool IsThr = false)
		{
			Graphics imgGraphics = Graphics.FromImage(PictureBox1.Image);
			Pen pPen = new Pen(Color.Black, 1);

			SolidBrush pBrush = new SolidBrush(Color.Black);
			SizeF stringSize;
			PointF pPoint = new PointF();

			string sElevation = GlobalVars.unitConverter.HeightToDisplayUnits(pPtFrom.Y + fRefHeight, eRoundMode.NEAREST).ToString();
			string sHeight = "(" + GlobalVars.unitConverter.HeightToDisplayUnits(pPtFrom.Y, eRoundMode.NEAREST).ToString() + ")";
			stringSize = imgGraphics.MeasureString(sElevation, Font);

			if (!IsThr)
				imgGraphics.DrawLine(pPen, TransformX(pPtFrom.X), TransformY(pPtFrom.Y), TransformX(pPtFrom.X) - Direction * LegendWidth, TransformY(pPtFrom.Y));

			float leftMargin;

			if (Direction == -1)
			{
				if (IsThr)
					leftMargin = 0;
				else
					leftMargin = 5 * TextPadding;
			}
			else
			{
				if (IsThr)
					leftMargin = -System.Math.Abs(stringSize.Width);	//- 9 * TextPadding;
				else
					leftMargin = -System.Math.Abs(stringSize.Width) - 14 * TextPadding;
			}

			pPoint.X = TransformX(pPtFrom.X) + leftMargin;
			pPoint.Y = TransformY(pPtFrom.Y) - System.Math.Abs(stringSize.Height) - TextPadding;
			imgGraphics.DrawString(sElevation, this.Font, pBrush, pPoint);

			pPoint.Y = pPoint.Y + System.Math.Abs(stringSize.Height) + 2 * TextPadding;	  //-(TextPadding + 20) * ScaleHeight
			imgGraphics.DrawString(sHeight, this.Font, pBrush, pPoint);
		}

		/*
	private Sub DrawArrow(ByRef pPtFrom As ProfilePoint)
	'	Dim ArrowX As Double
	'	Dim ArrowY As Double
	'	Dim ArrowAngleInGrad As Double
	'	Dim imgGraphics As Graphics = Graphics.FromImage(PictureBox1.Image)
	'	Dim pPen As New Pen(Color.Black, 2)
	'	Dim pBrush As SolidBrush = New SolidBrush(Color.Black)
	'	Dim pPoint As New PointF

	'	pPen.DashStyle = Drawing2D.DashStyle.Solid

	'	ArrowAngleInGrad = ArrowAngle * PI / 180.0

	'	ArrowX = pPtFrom.X + RWYDir * ArrowLen * System.Math.Cos(ArrowAngleInGrad)
	'	ArrowY = pPtFrom.Y + ArrowLen * System.Math.Sin(ArrowAngleInGrad)

	'	imgGraphics.DrawLine(pPen, TransformX(pPtFrom.X), TransformY(pPtFrom.Y), TransformX(ArrowX), TransformY(ArrowY))
	'	imgGraphics.DrawLine(pPen, TransformX(ArrowX), TransformY(ArrowY), TransformX(ArrowX - RWYDir * ArrowWingLen), TransformX(ArrowY))
	'	imgGraphics.DrawLine(pPen, TransformX(ArrowX), TransformY(ArrowY), TransformX(ArrowX), TransformY(ArrowY + 60.0 - ArrowWingLen))
	'End Sub

	'Private Sub DrawText(ByRef X As Double, ByRef Y As Double, ByRef S As String)
	'	Dim imgGraphics As Graphics = Graphics.FromImage(PictureBox1.Image)
	'	Dim pBrush As SolidBrush = New SolidBrush(Color.Black)
	'	Dim pPoint As New PointF

	'	pPoint.X = TransformX(X)
	'	pPoint.Y = TransformY(Y)

	'	imgGraphics.DrawString(S, Me.Font, pBrush, pPoint)
	'End Sub
		 */
		#endregion
	}
}
