using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using GeoAPI.Geometries;
using System.Drawing;
using System.Windows.Forms;
using NetTopologySuite.Geometries;
using System.Drawing.Drawing2D;
using CDOTMA.Geometries;
using CDOTMA.Drawings;
using CDOTMA.CoordinatSystems;

namespace CDOTMA.Controls
{
	public class ViewPort
	{
		public const int RegularStyle = 127;
		public const int DrawVerts = 128;
		public const int Blink = 256;

		public event EventHandler ProjectionChanged;
		public event EventHandler ViewChanged;

		public class ViewFrame
		{
			public Bitmap offScreen;
			public Envelope Extend;
			public List<ILayer> Layers;

			public double xoffset;
			public double yoffset;
			public double scale;

			public ViewFrame(Bitmap bmp)
			{
				Extend = new Envelope();
				Layers = new List<ILayer>();
				offScreen = bmp;

				xoffset = yoffset = 0.0;
				scale = 1.0;
			}
		}
		public struct GraphicItem
		{
			public IElement elem;
			public Geometry geom;
		}

		private int _CurrExtend;
		private ViewFrame _currentView;

		private int _nextUpGraphicID;
		private int _nextDownGraphicID;
		private Dictionary<int, GraphicItem> _UpGraphics;
		private Dictionary<int, GraphicItem> _DownGraphics;

		private CoordinatSystem _pSpRef;

		private System.Drawing.Graphics _gr;
		private List<GeoAPI.Geometries.Envelope> _ExtendsSeq;

		public ViewPort(int width, int height)
		{
			_gr = null;
			_CurrExtend = -1;
			_nextUpGraphicID = 1;
			_nextDownGraphicID = -1;
			_UpGraphics = new Dictionary<int, GraphicItem>();
			_DownGraphics = new Dictionary<int, GraphicItem>();

			_ExtendsSeq = new List<GeoAPI.Geometries.Envelope>();

			Bitmap bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			_currentView = new ViewFrame(bmp);
		}


		#region layer managment

		public int LayersCount
		{
			get { return _currentView.Layers.Count; }
		}

		public void AddLayer(ILayer layer)
		{
			_currentView.Layers.Add(layer);
		}

		public void InsertLayer(int index, ILayer layer)
		{
			_currentView.Layers.Insert(index, layer);
		}

		public bool RemoveLayer(ILayer layer)
		{
			return _currentView.Layers.Remove(layer);
		}

		public void RemoveAt(int lIx)
		{
			_currentView.Layers.RemoveAt(lIx);
		}

		public void RemoveRange(int index, int count)
		{
			_currentView.Layers.RemoveRange(index, count);
		}

		public void RemoveAllLayers()
		{
			_currentView.Layers.Clear();
		}

		public void ClearAllDrawings()
		{
			_UpGraphics.Clear();
			_DownGraphics.Clear();
		}

		public ILayer GetLayer(int lIx)
		{
			return _currentView.Layers[lIx];
		}

		public int AddToGraphics(NetTopologySuite.Geometries.Geometry geometry, CDOTMA.Drawings.IElement element, int order = 1)
		{
			int result;
			GraphicItem gitem = new GraphicItem { elem = element, geom = geometry };

			if (order >= 0)
			{
				result = _nextUpGraphicID;
				_nextUpGraphicID++;

				_UpGraphics.Add(result, gitem);
				return result;
			}

			result = _nextDownGraphicID;
			_nextDownGraphicID--;

			_DownGraphics.Add(result, gitem);
			return result;
		}

		public bool RemoveFromGraphics(int id)
		{
			if (_UpGraphics.ContainsKey(id))
			{
				_UpGraphics.Remove(id);
				return true;
			}

			if (_DownGraphics.ContainsKey(id))
			{
				_DownGraphics.Remove(id);
				return true;
			}

			return false;
		}

		#endregion

		#region extend & projection

		public void SetSize(int width, int height)
		{
			Bitmap bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			if (_gr != null)
			{
				_gr.Dispose();
				_gr = null;
			}

			_currentView.offScreen = bmp;
			//ZoomToExtend();
		}

		public void SetViewProjection(CoordinatSystem pSpRef)
		{
			_pSpRef = pSpRef;
			foreach (ILayer lr in _currentView.Layers)
				lr.ViewCS = pSpRef;
			if (ProjectionChanged != null)
				ProjectionChanged(this, new EventArgs());
			if (ViewChanged != null)
				ViewChanged(this, new EventArgs());
		}

		public void SetExtend(Envelope extend)
		{
			_currentView.Extend = extend;

			_ExtendsSeq.RemoveRange(_CurrExtend + 1, _ExtendsSeq.Count - _CurrExtend - 1);
			_CurrExtend = _ExtendsSeq.Count;
			_ExtendsSeq.Add(extend);

			ZoomToExtend();
		}

		public bool NextExtendIsPosible()
		{
			return _CurrExtend < _ExtendsSeq.Count - 1;
		}

		public bool PrevExtendIsPosible()
		{
			return _CurrExtend > 0;
		}

		public bool SetNextExtend()
		{
			if (_CurrExtend < _ExtendsSeq.Count - 1)
			{
				_CurrExtend++;
				_currentView.Extend = _ExtendsSeq[_CurrExtend];

				ZoomToExtend();
			}

			return _CurrExtend < _ExtendsSeq.Count - 1;
		}

		public bool SetPrevExtend()
		{
			if (_CurrExtend > 0)
			{
				_CurrExtend--;
				_currentView.Extend = _ExtendsSeq[_CurrExtend];

				ZoomToExtend();
			}
			return _CurrExtend > 0;
		}

		public Envelope GetExtend()
		{
			return _currentView.Extend;
		}

		//public void SetOffset(double x, double y)
		//{
		//    _view.xoffset = x;
		//    _view.yoffset = y;
		//}

		//public void ShiftOffset(double x, double y)
		//{
		//    _view.xoffset += x;
		//    _view.yoffset += y;
		//}

		public void ShiftExtend(double x, double y)
		{
			double InvScale = 1.0 / _currentView.scale;
			Envelope NewExtend = new Envelope(_currentView.Extend);

			NewExtend.Translate(-x * InvScale, y * InvScale);
			SetExtend(NewExtend);
			//if (ViewChanged != null)	ViewChanged(this, new EventArgs());
		}

		public void ZoomToExtend()
		{
			double kx = 0.9 * _currentView.offScreen.Width / _currentView.Extend.Width;
			double ky = 0.9 * _currentView.offScreen.Height / _currentView.Extend.Height;

			_currentView.scale = Math.Min(kx, ky);
			_currentView.xoffset = 0.5 * (_currentView.offScreen.Width - _currentView.Extend.Width * _currentView.scale);
			_currentView.yoffset = 0.5 * (_currentView.offScreen.Height - _currentView.Extend.Height * _currentView.scale);

			if (ViewChanged != null)
				ViewChanged(this, new EventArgs());
		}

		public PointF ProjectToScr(Coordinate pt)
		{
			PointF result = new PointF();

			result.X = (float)(_currentView.xoffset + (pt.X - _currentView.Extend.MinX) * _currentView.scale);
			result.Y = (float)(_currentView.yoffset + (_currentView.Extend.MaxY - pt.Y) * _currentView.scale);

			return result;
		}

		public NetTopologySuite.Geometries.Point ProjectToMap(double xScr, double yScr)
		{
			double x = _currentView.Extend.MinX - (_currentView.xoffset - xScr) / _currentView.scale;
			double y = _currentView.Extend.MaxY + (_currentView.yoffset - yScr) / _currentView.scale;

			NetTopologySuite.Geometries.Point result = new NetTopologySuite.Geometries.Point(x, y);
			return result;
		}

		public NetTopologySuite.Geometries.Point ProjectToMap(PointF pt)
		{
			double x = _currentView.Extend.MinX - (_currentView.xoffset - pt.X) / _currentView.scale;
			double y = _currentView.Extend.MaxY + (_currentView.yoffset - pt.Y) / _currentView.scale;

			NetTopologySuite.Geometries.Point result = new NetTopologySuite.Geometries.Point(x, y);
			return result;
		}

		public NetTopologySuite.Geometries.Point ProjectToMap(System.Drawing.Point pt)
		{
			double x = _currentView.Extend.MinX - (_currentView.xoffset - pt.X) / _currentView.scale;
			double y = _currentView.Extend.MaxY + (_currentView.yoffset - pt.Y) / _currentView.scale;

			NetTopologySuite.Geometries.Point result = new NetTopologySuite.Geometries.Point(x, y);
			return result;
		}

		public PointF ProjectToMap(NetTopologySuite.Geometries.Point pt)
		{
			PointF result = new PointF();

			result.X = (float)(_currentView.Extend.MinX - (_currentView.xoffset - pt.X) / _currentView.scale);
			result.Y = (float)(_currentView.Extend.MaxY + (_currentView.yoffset - pt.Y) / _currentView.scale);

			return result;
		}

		#endregion extend & projection

		#region drawings & image

		public void Clear()
		{
			if (_gr != null)
				_gr.Dispose();

			_gr = System.Drawing.Graphics.FromImage(_currentView.offScreen);
			_gr.FillRectangle(Pens.White.Brush, 0, 0, _currentView.offScreen.Width, _currentView.offScreen.Height);
		}

		public void FlipBackPage(PictureBox pBox)
		{
			if (_gr == null)
				return;
			pBox.Image = _currentView.offScreen;
		}

		public Bitmap GetViewImage()
		{
			return (Bitmap)_currentView.offScreen.Clone();
		}

		public static int RGB(int r, int g, int b)
		{
			r &= 255; g &= 255; b &= 255;
			return (b << 16) | (g << 8) | r;
		}

		public void DrawMarkers(double step)
		{
			double xmin = _currentView.Extend.MinX;
			double ymin = _currentView.Extend.MinY;

			if (_gr == null)
				_gr = System.Drawing.Graphics.FromImage(_currentView.offScreen);
		}

		public void DrawLeg(LegPoint legPt, int color = -1, int style = 0)
		{
			if (!legPt.Modified())
				return;

			if (legPt.pPtPrj != null)
				DrawPoint(legPt.pPtPrj.Coordinate, color);

			foreach (var trLeg in legPt.legs)
			{
				DrawLeg(trLeg, color, style);
			}
		}

		public void DrawLeg(TraceLeg trLeg, int color = -1, int style = 0)
		{
			//if (trLeg.ptStartPrj != null)
			//    DrawPoint(trLeg.ptStartPrj.Coordinate, color);

			//if (trLeg.ptEndPrj != null)
			//    DrawPoint(trLeg.ptEndPrj.Coordinate, color);

			if (trLeg.pProtectArea != null)
				DrawPolygon(trLeg.pProtectArea, color, style);

			if (trLeg.PathGeomPrj != null)
				DrawMultiLineString(trLeg.PathGeomPrj, color, 2, style);
		}

		public void DrawDownGraphics()
		{
			foreach (var item in _DownGraphics)
			{
				switch (item.Value.geom.OgcGeometryType)
				{
					case OgcGeometryType.Point:
						DrawPoint(((NetTopologySuite.Geometries.Point)item.Value.geom).Coordinate, item.Value.elem.Color, ((PointElement)item.Value.elem).Size, ((PointElement)item.Value.elem).Style);
						break;
					case OgcGeometryType.LineString:
						DrawLineString((LineString)item.Value.geom, item.Value.elem.Color, ((LineElement)item.Value.elem).Width, ((LineElement)item.Value.elem).Style);
						break;
					case OgcGeometryType.Polygon:
						DrawPolygon((Polygon)item.Value.geom, item.Value.elem.Color, ((FillElement)item.Value.elem).Style);
						break;
					case OgcGeometryType.MultiPoint:
						DrawMultiPoint((MultiPoint)item.Value.geom, item.Value.elem.Color, ((PointElement)item.Value.elem).Size);
						break;
					case OgcGeometryType.MultiLineString:
						DrawMultiLineString((MultiLineString)item.Value.geom, item.Value.elem.Color, ((LineElement)item.Value.elem).Width, ((LineElement)item.Value.elem).Style);
						break;
					case OgcGeometryType.MultiPolygon:
						DrawMultiPolygon((MultiPolygon)item.Value.geom, item.Value.elem.Color, ((FillElement)item.Value.elem).Style);
						break;
					case OgcGeometryType.GeometryCollection:
						break;
					case OgcGeometryType.CircularString:
						break;
					case OgcGeometryType.CompoundCurve:
						break;
					case OgcGeometryType.CurvePolygon:
						break;
					case OgcGeometryType.MultiCurve:
						break;
					case OgcGeometryType.MultiSurface:
						break;
					case OgcGeometryType.Curve:
						break;
					case OgcGeometryType.Surface:
						break;
					case OgcGeometryType.PolyhedralSurface:
						break;
					case OgcGeometryType.TIN:
						break;
				}
			}
		}

		public void DrawUpGraphics()
		{
			foreach (var item in _UpGraphics)
			{
				switch (item.Value.geom.OgcGeometryType)
				{
					case OgcGeometryType.Point:
						DrawPoint(((NetTopologySuite.Geometries.Point)item.Value.geom).Coordinate, item.Value.elem.Color, ((PointElement)item.Value.elem).Size, ((PointElement)item.Value.elem).Style);
						break;
					case OgcGeometryType.LineString:
						DrawLineString((LineString)item.Value.geom, item.Value.elem.Color, ((LineElement)item.Value.elem).Width, ((LineElement)item.Value.elem).Style);
						break;
					case OgcGeometryType.Polygon:
						DrawPolygon((Polygon)item.Value.geom, item.Value.elem.Color, ((FillElement)item.Value.elem).Style);
						break;
					case OgcGeometryType.MultiPoint:
						DrawMultiPoint((MultiPoint)item.Value.geom, item.Value.elem.Color, ((PointElement)item.Value.elem).Size);
						break;
					case OgcGeometryType.MultiLineString:
						DrawMultiLineString((MultiLineString)item.Value.geom, item.Value.elem.Color, ((LineElement)item.Value.elem).Width, ((LineElement)item.Value.elem).Style);
						break;
					case OgcGeometryType.MultiPolygon:
						DrawMultiPolygon((MultiPolygon)item.Value.geom, item.Value.elem.Color, ((FillElement)item.Value.elem).Style);
						break;
					case OgcGeometryType.GeometryCollection:
						break;
					case OgcGeometryType.CircularString:
						break;
					case OgcGeometryType.CompoundCurve:
						break;
					case OgcGeometryType.CurvePolygon:
						break;
					case OgcGeometryType.MultiCurve:
						break;
					case OgcGeometryType.MultiSurface:
						break;
					case OgcGeometryType.Curve:
						break;
					case OgcGeometryType.Surface:
						break;
					case OgcGeometryType.PolyhedralSurface:
						break;
					case OgcGeometryType.TIN:
						break;
				}
			}
		}

		public void DrawMultiPolygon(MultiPolygon mpgon, int color = -1, int style = 0)
		{
			for (int i = 0; i < mpgon.Count; i++)
				DrawPolygon((Polygon)mpgon[i], color, style);
		}

		public void DrawPolygon(Polygon pgon, int color = -1, int style = 0)
		{
			double xmin = _currentView.Extend.MinX;
			double ymin = _currentView.Extend.MinY;

			if (_gr == null)
				_gr = System.Drawing.Graphics.FromImage(_currentView.offScreen);

			const int Width = 2;

			Pen pen;

			if (color < 0)
			{
				Random rnd = new Random();
				pen = new Pen(Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)), Width);
			}
			else
				pen = new Pen(Color.FromArgb(color & 255, (color >> 8) & 255, (color >> 16) & 255), Width);

			Brush brush;
			int rStyle = style & RegularStyle;

			if (rStyle > FillElementStyle.Hollow && rStyle < FillElementStyle.Solid)
				brush = new HatchBrush((HatchStyle)(rStyle - 1), pen.Color, Color.Transparent);
			else
				brush = new SolidBrush(pen.Color);

			GraphicsPath path = new GraphicsPath();

			if (pgon.ExteriorRing.Coordinates.Length > 0)
			{
				for (int j = 0; j <= pgon.InteriorRings.Length; j++)
				{
					PointF[] pts;
					if (j == 0)
					{
						pts = new PointF[pgon.ExteriorRing.Coordinates.Length];
						for (int k = 0; k < pgon.ExteriorRing.Coordinates.Length; k++)
							pts[k] = ProjectToScr(pgon.ExteriorRing.Coordinates[k]);
					}
					else
					{
						pts = new PointF[pgon.InteriorRings[j - 1].Coordinates.Length];
						for (int k = 0; k < pgon.InteriorRings[j - 1].Coordinates.Length; k++)
							pts[k] = ProjectToScr(pgon.InteriorRings[j - 1].Coordinates[k]);
					}

					path.AddPolygon(pts);
				}

				if (rStyle != 0)
					_gr.FillPath(brush, path);
				_gr.DrawPath(pen, path);

				//int g = 0;
				if ((style & DrawVerts) != 0)
				{
					int iColor = RGB(255 - pen.Color.R, 255 - pen.Color.G, 255 - pen.Color.B);

					for (int j = 0; j <= pgon.InteriorRings.Length; j++)
						if (j == 0)
							for (int k = 0; k < pgon.ExteriorRing.Coordinates.Length; k++)
								//DrawPointWithText(pgon.ExteriorRing[k], string.Format("({0}; {1})", pgon.ExteriorRing[k].X, pgon.ExteriorRing[k].Y));
								//DrawPointWithText(pgon.ExteriorRing[k], (g++).ToString());
								DrawPoint(pgon.ExteriorRing.Coordinates[k], iColor, PointElementStyle.X);
						else
							for (int k = 0; k < pgon.InteriorRings[j - 1].Coordinates.Length; k++)
								//DrawPointWithText(pgon.InteriorRingList[j - 1][k], string.Format("({0}; {1})", pgon.InteriorRingList[j - 1][k].X, pgon.InteriorRingList[j - 1][k].Y));
								//DrawPointWithText(pgon.InteriorRingList[j - 1][k], (g++).ToString());
								DrawPoint(pgon.InteriorRings[j - 1].Coordinates[k], iColor, PointElementStyle.X);
				}
			}
		}

		public void DrawMultiLineString(MultiLineString polyline, int color = -1, int Width = 1, int style = 0)
		{
			for (int i = 0; i < polyline.Count; i++)
			{
				LineString lineStr = (LineString)polyline.Geometries[i];
				DrawLineString(lineStr, color, Width, style);
			}
		}

		public void DrawLineString(LineString lineStr, int iColor = -1, int Width = 1, int style = 0)
		{
			if (lineStr.IsEmpty)
				return;

			if (_gr == null)
				_gr = System.Drawing.Graphics.FromImage(_currentView.offScreen);

			Pen pen;

			if (iColor < 0)
			{
				Random rnd = new Random();
				pen = new Pen(Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)), Width);
			}
			else
				pen = new Pen(Color.FromArgb(iColor & 255, (iColor >> 8) & 255, (iColor >> 16) & 255), Width);

			pen.DashStyle = (System.Drawing.Drawing2D.DashStyle)(style & RegularStyle);
			iColor = RGB(255 - pen.Color.R, 255 - pen.Color.G, 255 - pen.Color.B);

			if ((style & DrawVerts) != 0)
				DrawPoint(lineStr[0], iColor, PointElementStyle.X);

			int n = lineStr.Count - 1;
			PointF pt1 = ProjectToScr(lineStr[0]);

			for (int i = 1; i < lineStr.Count; i++)
			{
				PointF pt2 = ProjectToScr(lineStr[i]);
				if ((style & RegularStyle) != LineElementStyle.Null)
					_gr.DrawLine(pen, pt1, pt2);

				if ((style & DrawVerts) != 0)
					DrawPoint(lineStr[i], iColor, PointElementStyle.X);

				pt1 = pt2;
			}
		}

		public void DrawMultiPoint(MultiPoint mpt, int iColor = -1, int Size = 8, int style = 0)
		{
			foreach (var pt in mpt.Geometries)
				DrawPoint((NetTopologySuite.Geometries.Point)pt, iColor, Size, style);
		}

		public void DrawPoint(NetTopologySuite.Geometries.Point pt, int iColor = -1, int Size = 8, int style = 0)
		{
			DrawPoint(pt.Coordinate, iColor, Size, style);
		}

		public void DrawPoint(Coordinate pt, int iColor = -1, int Size = 8, int style = 0)
		{
			if (_gr == null)
				_gr = System.Drawing.Graphics.FromImage(_currentView.offScreen);

			Brush brush;
			if (iColor < 0)
			{
				Random rnd = new Random();
				brush = new SolidBrush(Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)));
			}
			else
				brush = new SolidBrush(Color.FromArgb(iColor & 255, (iColor >> 8) & 255, (iColor >> 16) & 255));

			Pen pen = new Pen(brush);
			pen.Width = 3;
			PointF pt1 = ProjectToScr(pt);

			switch (style)
			{
				case PointElementStyle.Square:
					_gr.FillRectangle(brush, pt1.X - 0.5f * Size, pt1.Y - 0.5f * Size, Size, Size);
					break;

				case PointElementStyle.Cross:
					_gr.DrawLine(pen, pt1.X - 0.5f * Size, pt1.Y, pt1.X + 0.5f * Size, pt1.Y);
					_gr.DrawLine(pen, pt1.X, pt1.Y - 0.5f * Size, pt1.X, pt1.Y + 0.5f * Size);
					break;

				case PointElementStyle.X:
					_gr.DrawLine(pen, pt1.X - 0.5f * Size, pt1.Y - 0.5f * Size, pt1.X + 0.5f * Size, pt1.Y + 0.5f * Size);
					_gr.DrawLine(pen, pt1.X - 0.5f * Size, pt1.Y + 0.5f * Size, pt1.X + 0.5f * Size, pt1.Y - 0.5f * Size);
					break;

				case PointElementStyle.Diamond:
					_gr.FillPolygon(brush, new PointF[] { new PointF(pt1.X - 0.5f * Size, pt1.Y), new PointF(pt1.X, pt1.Y - 0.5f * Size),
														new PointF(pt1.X + 0.5f * Size, pt1.Y), new PointF(pt1.X, pt1.Y + 0.5f * Size) });
					break;

				default:
					_gr.FillEllipse(brush, pt1.X - 0.5f * Size, pt1.Y - 0.5f * Size, Size, Size);
					break;
			}
		}

		public void DrawPointWithText(NetTopologySuite.Geometries.Point pt, string text, int iColor = -1, int Size = 10, int Style = 0)
		{
			//SolidBrush brush;
			//if (iColor < 0)
			//{
			//	Random rnd = new Random();
			//	brush = new SolidBrush(Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)));
			//}
			//else
			//	brush = new SolidBrush(Color.FromArgb(iColor & 255, (iColor >> 8) & 255, (iColor >> 16) & 255));

			PointF pt1 = ProjectToScr(pt.Coordinate);

			if (pt1.X < -Size || pt1.X > _currentView.offScreen.Width + Size)
				return;

			if (pt1.Y < -Size || pt1.Y > _currentView.offScreen.Height + Size)
				return;

			DrawPoint(pt, iColor, Size, Style);

			//if (_gr == null)
			//	_gr = System.Drawing.Graphics.FromImage(_currentView.offScreen);

			//Font font = new Font(FontFamily.Families[3], size, FontStyle.Regular);
			//Font font = new Font("Andalus", size + 2);

			Font font = new Font("Arial", Size + 2);

			//pt1.X -= 0.5f * text.Length * size;
			pt1.X += 0.5f * Size;
			pt1.Y -= 0.5f * Size;

			_gr.DrawString(text, font, new SolidBrush(Color.Black), pt1);
		}

		public void DrawLayers()
		{
			Layer<NetTopologySuite.Geometries.Point> pointLayer;
			Layer<NetTopologySuite.Geometries.MultiLineString> polylineLayer;
			Layer<NetTopologySuite.Geometries.MultiPolygon> polygonLayer;
			LegLayer legLayer;

			for (int i = 0; i < _currentView.Layers.Count; i++)
				if (_currentView.Layers[i].Visible)
				{
					ILayer layer = _currentView.Layers[i];

					if (layer.Count == 0)
						continue;

					switch (layer.GeometryType)
					{
						case OgcGeometryType.Point:
							pointLayer = (Layer<NetTopologySuite.Geometries.Point>)layer;
							for (int j = 0; j < pointLayer.Count; j++)
								DrawPointWithText((NetTopologySuite.Geometries.Point)pointLayer.GetPrjGeometry(j),
									((GeometryExtension)pointLayer[j].UserData).Name, pointLayer.Element.Color, ((PointElement)pointLayer.Element).Size, ((PointElement)pointLayer.Element).Style);

							break;

						case OgcGeometryType.MultiPoint:
							break;

						case OgcGeometryType.LineString:

							break;

						case OgcGeometryType.MultiLineString:
							polylineLayer = (Layer<NetTopologySuite.Geometries.MultiLineString>)layer;
							for (int j = 0; j < polylineLayer.Count; j++)
								DrawMultiLineString((NetTopologySuite.Geometries.MultiLineString)polylineLayer.GetPrjGeometry(j),
									polylineLayer.Element.Color, 2, ((FillElement)(polylineLayer.Element)).Style);

							break;

						case OgcGeometryType.Polygon:
							break;

						case OgcGeometryType.MultiPolygon:
							polygonLayer = (Layer<NetTopologySuite.Geometries.MultiPolygon>)layer;

							for (int j = 0; j < polygonLayer.Count; j++)
								DrawMultiPolygon((NetTopologySuite.Geometries.MultiPolygon)polygonLayer.GetPrjGeometry(j),
									polygonLayer.Element.Color, ((FillElement)(polygonLayer.Element)).Style);

							break;

						case OgcGeometryType.GeometryCollection:
							legLayer = (LegLayer)layer;
							for (int j = 0; j < legLayer.Count; j++)
								DrawLeg(legLayer[j], legLayer.Element.Color, ((FillElement)(legLayer.Element)).Style);

							break;
					}
				}
		}

		#endregion drawings
	}
}
