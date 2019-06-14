using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GeoAPI.Geometries;
using CDOTMA.Drawings;
using System.Runtime.InteropServices;
using CDOTMA.CoordinatSystems;

namespace CDOTMA.Controls
{
	public enum ViewerMode : int
	{
		None,
		Pan,
		ZoomIn,
		ZoomOut,
		Edit,
		Info,
		Select,
		SelectBox,
		Super
	}

	/*
		None, Pan, ZoomIn, ZoomOut, Measure,
		Info, Select, SelectBox,
		SelectBase, AddPoint, AddPline, DelVertex,
		MoveVertex, Splite, AddVertex, AddPgone,
		Overland, LineCut, d3D
	 */

	public partial class ViewControl : UserControl, IMessageFilter
	{
		private ViewPort _screen;
		private GeoAPI.Geometries.Envelope _fullExtend;
		private GeoAPI.Geometries.Envelope _viewExtend;

		private NetTopologySuite.Geometries.Point _mdMap;
		private NetTopologySuite.Geometries.Point _currMap;
		private NetTopologySuite.Geometries.Point _muMap;
		private Point _mdScreen;
		private Point _currScreen;
		private Point _muScreen;
		private ViewerMode _mode;
		private bool _isDown;

		public ViewControl()
		{
			InitializeComponent();

			_mdScreen = new Point();
			_muScreen = new Point();
			_currScreen = new Point();

            _isDown = false;

			Mode = ViewerMode.None;

			_screen = new ViewPort(pictureBox.ClientSize.Width, pictureBox.ClientSize.Height);
			_screen.Clear();
			_screen.FlipBackPage(pictureBox);

			_screen.ProjectionChanged += OnProjectionChanged;
			_screen.ViewChanged += OnViewChanged;

			_fullExtend = new GeoAPI.Geometries.Envelope();
			_viewExtend = new GeoAPI.Geometries.Envelope();
		}

		public ViewPort Screen { get { return _screen; } }

		public ViewerMode Mode
		{
			get { return _mode; }

			set
			{
				_mode = value;
				Cursor cur = Cursors.Cross;				//cur = Cursors.NoMove2D;

				switch (_mode)
				{
					case ViewerMode.Pan:
						cur = new Cursor(Properties.Resources.HandFlat.Handle);
						break;
					case ViewerMode.ZoomIn:
						cur = new Cursor(Properties.Resources.ZoomIn.Handle);
						break;
					case ViewerMode.ZoomOut:
						cur = new Cursor(Properties.Resources.ZoomOut.Handle);
						break;
					case ViewerMode.Edit:
						cur = new Cursor(Properties.Resources.Edit.Handle);
						break;
					case ViewerMode.Info:
						//cur = new Cursor(Properties.Resources.Info.Handle);
						break;
				}
				pictureBox.Cursor = cur;
			}
		}

		//
		// Summary:
		//     Gets or sets the cursor that is displayed when the mouse pointer is over
		//     the control.
		//
		// Returns:
		//     A System.Windows.Forms.Cursor that represents the cursor to display when
		//     the mouse pointer is over the control.
		//[SRCategory("CatAppearance")]
		//[SRDescription("ControlCursorDescr")]

		[AmbientValue("")]
		public override Cursor Cursor
		{
			get { return pictureBox.Cursor; }
			set { pictureBox.Cursor = value; }
		}

		//
		// Summary:
		//     Gets the window handle that the control is bound to.
		//
		// Returns:
		//     An System.IntPtr that contains the window handle (HWND) of the control.

		[DispId(-515)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		//[SRDescription("ControlHandleDescr")]
		[Browsable(false)]
		public new IntPtr Handle { get { return pictureBox.Handle; } }

		#region layer managment

		public void RemoveAllLayers()
		{
			_screen.RemoveAllLayers();
			_fullExtend.Init();
		}

		public void ClearAllDrawings()
		{
			_screen.ClearAllDrawings();
		}

		public void AddLayer(ILayer layer)
		{
			NetTopologySuite.Geometries.Point p01 = new NetTopologySuite.Geometries.Point(layer.Extend.MinX, layer.Extend.MinY);
			//if (p01.X < ((ProjectedCoordinatSystem)(GlobalVars.pSpRefPrj)).CentralMeridian - 10.0)
			//	p01.X = ((ProjectedCoordinatSystem)(GlobalVars.pSpRefPrj)).CentralMeridian - 10.0;

			//if (p01.X > ((ProjectedCoordinatSystem)(GlobalVars.pSpRefPrj)).CentralMeridian + 10.0)
			//	p01.X = ((ProjectedCoordinatSystem)(GlobalVars.pSpRefPrj)).CentralMeridian + 10.0;

			NetTopologySuite.Geometries.Point p1 = (NetTopologySuite.Geometries.Point)Functions.ToPrj(p01);

			p01 = new NetTopologySuite.Geometries.Point(layer.Extend.MaxX, layer.Extend.MaxY);
			//if (p01.X < ((ProjectedCoordinatSystem)(GlobalVars.pSpRefPrj)).CentralMeridian - 10.0)
			//	p01.X = ((ProjectedCoordinatSystem)(GlobalVars.pSpRefPrj)).CentralMeridian - 10.0;

			//if (p01.X > ((ProjectedCoordinatSystem)(GlobalVars.pSpRefPrj)).CentralMeridian + 10.0)
			//	p01.X = ((ProjectedCoordinatSystem)(GlobalVars.pSpRefPrj)).CentralMeridian + 10.0;
			NetTopologySuite.Geometries.Point p2 = (NetTopologySuite.Geometries.Point)Functions.ToPrj(p01);
			Envelope prjExtend = new Envelope(p1.X, p2.X, p1.Y, p2.Y);

			_fullExtend.ExpandToInclude(prjExtend);
			_screen.AddLayer(layer);
		}

		#endregion

		#region view managment

		public void DrawView()
		{
			_screen.Clear();
			_screen.DrawDownGraphics();
			_screen.DrawLayers();
			_screen.DrawUpGraphics();
			_screen.FlipBackPage(pictureBox);
		}

		public void SetViewProjection(CoordinatSystem pSpRef)
		{
			_screen.SetViewProjection(pSpRef);
			DrawView();
		}

		public void ResetView()
		{
			_screen.SetExtend(_fullExtend);
			DrawView();
		}

		public void SetExtend(Envelope extend)
		{
			_screen.SetExtend(extend);
			DrawView();
		}

		public void CenterAtWithZoom(NetTopologySuite.Geometries.Point Center, double minSize = 10000.0)
		{
			Envelope extend = new Envelope(Center.X - minSize, Center.X + minSize,
											Center.Y - minSize, Center.Y + minSize);
			_screen.SetExtend(extend);
			DrawView();
		}

		public void CenterAt(NetTopologySuite.Geometries.Point Center)
		{
			Envelope extend = _screen.GetExtend();
			Coordinate oldCenter = extend.Centre;
			extend.Translate(Center.X - oldCenter.X, Center.Y - oldCenter.Y);

			_screen.SetExtend(extend);
			DrawView();
		}

		public bool NextExtendIsPosible()
		{
			return _screen.NextExtendIsPosible();
		}

		public bool PrevExtendIsPosible()
		{
			return _screen.PrevExtendIsPosible();
		}

		public bool PrevExtend()
		{
			bool result = _screen.SetPrevExtend();
			DrawView();
			return result;
		}

		public bool NextExtend()
		{
			bool result = _screen.SetNextExtend();
			DrawView();
			return result;
		}

		#endregion

		#region view events

		//
		// Summary:
		//     Occurs when the view projection is changed.
		//[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public event EventHandler ProjectionChanged;

		//
		// Summary:
		//     Occurs when the view area (extend) is changed.
		//[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public event EventHandler ViewChanged;

		private void OnProjectionChanged(object sender, EventArgs e)
		{
			// TO DO:

			// standart handler
			if (ProjectionChanged != null)
				ProjectionChanged(sender, e);
		}

		private void OnViewChanged(object sender, EventArgs e)
		{
			// TO DO:

			// standart handler
			if (ViewChanged != null)
				ViewChanged(sender, e);
		}

		#endregion

		#region graphic drawings

		public int DrawGeometry(NetTopologySuite.Geometries.Geometry geom, CDOTMA.Drawings.IElement element, int order = 1)
		{
			int id = _screen.AddToGraphics(geom, element, order);
			DrawView();
			return id;
		}

		public void DeleteGeometry(int id)
		{
			if (_screen.RemoveFromGraphics(id))
				DrawView();
		}

		#endregion

		#region pictureBox events

		//OnMouseDown;
		//MouseEnter
		//MouseMove
		//MouseHover / MouseDown / MouseWheel
		//MouseUp
		//MouseLeave

		private void pictureBox_SizeChanged(object sender, EventArgs e)
		{
            if (_screen == null || pictureBox.ClientSize.Width == 0 || pictureBox.ClientSize.Height == 0)
                return;

			_screen.SetSize(pictureBox.ClientSize.Width, pictureBox.ClientSize.Height);
			_screen.ZoomToExtend();
			DrawView();
		}

		private void pictureBox_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				_isDown = true;

				_currScreen.X = _mdScreen.X = e.X;
				_currScreen.Y = _mdScreen.Y = e.Y;
				_mdMap = _screen.ProjectToMap(_mdScreen);

				if (_mode == ViewerMode.Edit)
				{

				}
			}

			var onMouseDown = this.GetType().GetMethod("OnMouseDown", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			if (onMouseDown != null)
				onMouseDown.Invoke(this, new object[] { e });
		}

		private void pictureBox_MouseMove(object sender, MouseEventArgs e)
		{
			var OnMouseMove = this.GetType().GetMethod("OnMouseMove", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

			if (e.Button != MouseButtons.Left)
			{
				if (OnMouseMove != null)
					OnMouseMove.Invoke(this, new object[] { e });
				return;
			}

			Point prevScreen = _currScreen;

			switch (_mode)
			{
				case ViewerMode.None:
					break;
				case ViewerMode.Pan:
					_currScreen.X = e.X;
					_currScreen.Y = e.Y;
					_currMap = _screen.ProjectToMap(_currScreen);

					_screen.ShiftExtend(_currScreen.X - _mdScreen.X, _currScreen.Y - _mdScreen.Y);

					_mdScreen.X = _currScreen.X;
					_mdScreen.Y = _currScreen.Y;

					DrawView();
					break;
				case ViewerMode.ZoomIn:
				case ViewerMode.ZoomOut:

					if (prevScreen.X != e.X && prevScreen.Y != e.Y)
					{
						_currScreen.X = e.X;
						_currScreen.Y = e.Y;
						_currMap = _screen.ProjectToMap(_currScreen);

						int x0 = Math.Min(_mdScreen.X, prevScreen.X);
						int y0 = Math.Min(_mdScreen.Y, prevScreen.Y);

						int x1 = Math.Max(_mdScreen.X, prevScreen.X);
						int y1 = Math.Max(_mdScreen.Y, prevScreen.Y);

						int oldRop;
						IntPtr oldpen;
						IntPtr hdc = XorDrawing.BeginDraw(pictureBox.Handle, out oldRop, out oldpen);
						XorDrawing.Rectangle(hdc, x0, y0, x1, y1);

						x0 = Math.Min(_mdScreen.X, _currScreen.X);
						y0 = Math.Min(_mdScreen.Y, _currScreen.Y);

						x1 = Math.Max(_mdScreen.X, _currScreen.X);
						y1 = Math.Max(_mdScreen.Y, _currScreen.Y);

						XorDrawing.Rectangle(hdc, x0, y0, x1, y1);
						XorDrawing.FinishDraw(pictureBox.Handle, hdc, oldRop, oldpen);
					}

					break;
				case ViewerMode.Edit:

					break;
				case ViewerMode.Info:

					break;
				case ViewerMode.Select:
				case ViewerMode.SelectBox:
					//case ViewerMode.AddPoint:
					//case ViewerMode.SelectBase:
					//case ViewerMode.AddPline:
					//case ViewerMode.DelVertex:
					//case ViewerMode.MoveVertex:
					//case ViewerMode.Splite:
					//case ViewerMode.AddVertex:
					//case ViewerMode.AddPgone:
					//case ViewerMode.Overland:
					//case ViewerMode.LineCut:
					//case ViewerMode.d3D:
					break;
				default:
					break;
			}

			if (OnMouseMove != null)
				OnMouseMove.Invoke(this, new object[] { e });
		}

		private void pictureBox_MouseUp(object sender, MouseEventArgs e)
		{
			var OnMouseUp = this.GetType().GetMethod("OnMouseUp", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

			if (e.Button == MouseButtons.Left && _isDown)
			{
				_isDown = false;
				_muScreen.X = e.X;
				_muScreen.Y = e.Y;
				_muMap = _screen.ProjectToMap(_muScreen);

				if (Mode == ViewerMode.Pan)
				{
					//Mode = ViewerMode.None;
					if (OnMouseUp != null)
						OnMouseUp.Invoke(this, new object[] { e });

					return;
				}

				if (Mode == ViewerMode.ZoomIn || Mode == ViewerMode.ZoomOut)
				{
					int a = Math.Abs(_mdScreen.X - _muScreen.X);
					int b = Math.Abs(_mdScreen.Y - _muScreen.Y);

					if (a == 0 || b == 0)
					{
						if (OnMouseUp != null)
							OnMouseUp.Invoke(this, new object[] { e });
						return;
					}

					Envelope extend = new Envelope();

					extend.ExpandToInclude(_mdMap.X, _mdMap.Y);
					extend.ExpandToInclude(_muMap.X, _muMap.Y);

					if (Mode == ViewerMode.ZoomOut)
					{
						Envelope screenExtend = _screen.GetExtend();

						double MinX = screenExtend.MinX - (extend.MinX - screenExtend.MinX);
						double MinY = screenExtend.MinY - (extend.MinY - screenExtend.MinY);

						double MaxX = screenExtend.MaxX + (screenExtend.MaxX - extend.MaxX);
						double MaxY = screenExtend.MaxY + (screenExtend.MaxY - extend.MaxY);

						extend.SetToNull();
						extend.ExpandToInclude(MinX, MinY);
						extend.ExpandToInclude(MaxX, MaxY);
					}

					_screen.SetExtend(extend);
					//_Screen.ZoomToExtend();
					DrawView();
				}
			}

			if (OnMouseUp != null)
				OnMouseUp.Invoke(this, new object[] { e });
		}
		#endregion

		// P/Invoke declarations
		[DllImport("user32.dll")]
		private static extern IntPtr WindowFromPoint(Point pt);
		[DllImport("user32.dll")]
		private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

		const int WM_MOUSEWHEEL = 0x20a;

		//public bool PreFilterMessage(ref Message m)
		//{
		//    if (m.Msg == WM_MOUSEWHEEL)
		//    {
		//        // WM_MOUSEWHEEL, find the control at screen position m.LParam
		//        Point pos = new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16);
		//        IntPtr hWnd = WindowFromPoint(pos);
		//        if (hWnd != IntPtr.Zero && hWnd != m.HWnd && Control.FromHandle(hWnd) != null)
		//        {
		//            SendMessage(hWnd, m.Msg, m.WParam, m.LParam);
		//            return true;
		//        }
		//    }
		//    return false;
		//}

		bool IMessageFilter.PreFilterMessage(ref Message m)
		{
			if (m.Msg == WM_MOUSEWHEEL)
			{
				// WM_MOUSEWHEEL, find the control at screen position m.LParam
				Point pos = new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16);
				IntPtr hWnd = WindowFromPoint(pos);
				if (hWnd != IntPtr.Zero && hWnd != m.HWnd && Control.FromHandle(hWnd) != null)
				{
					SendMessage(hWnd, m.Msg, m.WParam, m.LParam);
					return true;
				}
			}
			return false;
		}


	}
}
