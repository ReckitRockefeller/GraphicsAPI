using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Collections.Generic;

namespace MadGrap
{
	public class Render: Control {
		Graphics _Graphics;
		Graphics _BufferedGraphics;
		Bitmap _BufferedBitmap;

		SolidBrush _Brush;
		SmoothingMode _SmoothingMode;
		Rectangle _Bounds;

		List<Shape> _Shapes;
		Rectangle _ShapeBoundsEvent;

		static Bitmap ViewBitmap(Bitmap bmp, Rectangle rect) {
			Rectangle r = new Rectangle(0,0,bmp.Width,bmp.Height);
			BitmapData bmpData = bmp.LockBits(r,ImageLockMode.ReadWrite,bmp.PixelFormat);
			IntPtr ptr = bmpData.Scan0;
			bmp.UnlockBits(bmpData);
			int pixelFormatSize = Image.GetPixelFormatSize(bmp.PixelFormat)/8;
			int wPixelFormatSize = bmp.Width*pixelFormatSize;
			rect = Rectangle.Intersect(rect,r);
			return new Bitmap(rect.Width,rect.Height,wPixelFormatSize,bmp.PixelFormat,(IntPtr)(ptr+wPixelFormatSize*rect.Y+rect.X*pixelFormatSize));
		}

		void RedrawRect(Rectangle rect) {
			if (rect.X < 0) {
				rect.Width += rect.X;
				rect.X = 0;
			}
			if (rect.Y < 0) {
				rect.Height += rect.Y;
				rect.Y = 0;
			}
			if (rect.Width > 0 && rect.Height > 0) {
				lock (_Graphics) {
					Bitmap bmp = new Bitmap(rect.Width,rect.Height);
					Graphics g = Graphics.FromImage(bmp);
					g.DrawImage(ViewBitmap(_BufferedBitmap,rect),0,0);
					g.SmoothingMode = _SmoothingMode;
					foreach (Shape shape in _Shapes) {
						if (shape.Visible && shape.IntersectsWith(rect)) {
							shape.InternalDraw(g,shape.X-rect.X,shape.Y-rect.Y);
						}
					}
					_Graphics.DrawImage(bmp,rect);
					bmp.Dispose();
					g.Dispose();
				}
			}
		}

		void Redraw() {
			RedrawRect(_Bounds);
		}

		void RedrawWindow() {
			lock (_Graphics) {
				if (BackgroundImage == null) {
					_BufferedGraphics.FillRectangle(_Brush,_Bounds);
				} else {
					_BufferedGraphics.DrawImage(BackgroundImage,_Bounds);
				}
			}
		}

		void ShapeMoveBeginEvent(object sender, EventArgs e) {
			if (((Shape)sender).Visible) {
				_ShapeBoundsEvent = ((Shape)sender).Bounds;
			}
		}

		void ShapeMoveEvent(object sender, EventArgs e) {
			if (((Shape)sender).Visible) {
				RedrawRect(_ShapeBoundsEvent);
				RedrawRect(((Shape)sender).Bounds);
			}
		}

		void ShapeResizeBeginEvent(object sender, EventArgs e) {
			if (((Shape)sender).Visible) {
				_ShapeBoundsEvent = ((Shape)sender).Bounds;
			}
		}

		void ShapeResizeEvent(object sender, EventArgs e) {
			if (((Shape)sender).Visible) {
				RedrawRect(_ShapeBoundsEvent);
				RedrawRect(((Shape)sender).Bounds);
			}
		}

		void ShapeBorderWidthChangeBeginEvent(object sender, EventArgs e) {
			if (((Shape)sender).Visible) {
				_ShapeBoundsEvent = ((Shape)sender).Bounds;
			}
		}

		void ShapeBorderWidthChangedEvent(object sender, EventArgs e) {
			if (((Shape)sender).Visible) {
				RedrawRect(_ShapeBoundsEvent);
				RedrawRect(((Shape)sender).Bounds);
			}
		}

		void ShapeBorderColorChangedEvent(object sender, EventArgs e) {
			if (((Shape)sender).Visible) {
				RedrawRect(((Shape)sender).Bounds);
			}
		}

		void ShapeBackgroundColorChangedEvent(object sender, EventArgs e) {
			if (((Shape)sender).Visible) {
				RedrawRect(((Shape)sender).Bounds);
			}
		}

		void ShapeImageChangedEvent(object sender, EventArgs e) {
			if (((Shape)sender).Visible) {
				RedrawRect(((Shape)sender).Bounds);
			}
		}

		void ShapeTextChangedEvent(object sender, EventArgs e) {
			if (((Shape)sender).Visible) {
				RedrawRect(((Shape)sender).Bounds);
			}
		}
		void ShapeColorChangedEvent(object sender, EventArgs e) {
			if (((Shape)sender).Visible) {
				RedrawRect(((Shape)sender).Bounds);
			}
		}

		void ShapeFontChangedEvent(object sender, EventArgs e) {
			if (((Shape)sender).Visible) {
				RedrawRect(((Shape)sender).Bounds);
			}
		}

		void ShapeVisibleChangedEvent(object sender, EventArgs e) {
			RedrawRect(((Shape)sender).Bounds);
		}

		void RegisterShapeEvents(bool b, Shape shape) {
			if (b) {
				shape.MoveBegin += ShapeMoveBeginEvent;
				shape.Move += ShapeMoveEvent;
				shape.ResizeBegin += ShapeResizeBeginEvent;
				shape.Resize += ShapeResizeEvent;
				if (shape as ColouredShape != null) {
					((ColouredShape)shape).BorderWidthChangeBegin += ShapeBorderWidthChangeBeginEvent;
					((ColouredShape)shape).BorderWidthChanged += ShapeBorderWidthChangedEvent;
					((ColouredShape)shape).BorderColorChanged += ShapeBorderColorChangedEvent;
					((ColouredShape)shape).BackgroundColorChanged += ShapeBackgroundColorChangedEvent;
				} else if (shape as PicturedShape != null) {
					((PicturedShape)shape).ImageChanged += ShapeImageChangedEvent;
				} else if (shape as TextedShape != null) {
					((TextedShape)shape).TextChanged += ShapeTextChangedEvent;
					((TextedShape)shape).ColorChanged += ShapeColorChangedEvent;
					((TextedShape)shape).FontChanged += ShapeFontChangedEvent;
				}
				shape.VisibleChanged += ShapeVisibleChangedEvent;
			} else {
				shape.MoveBegin -= ShapeMoveBeginEvent;
				shape.Move -= ShapeMoveEvent;
				shape.ResizeBegin -= ShapeResizeBeginEvent;
				shape.Resize -= ShapeResizeEvent;
				if (shape as ColouredShape != null) {
					((ColouredShape)shape).BorderWidthChangeBegin -= ShapeBorderWidthChangeBeginEvent;
					((ColouredShape)shape).BorderWidthChanged -= ShapeBorderWidthChangedEvent;
					((ColouredShape)shape).BorderColorChanged -= ShapeBorderColorChangedEvent;
					((ColouredShape)shape).BackgroundColorChanged -= ShapeBackgroundColorChangedEvent;
				} else if (shape as PicturedShape != null) {
					((PicturedShape)shape).ImageChanged -= ShapeImageChangedEvent;
				} else if (shape as TextedShape != null) {
					((TextedShape)shape).TextChanged -= ShapeTextChangedEvent;
					((TextedShape)shape).ColorChanged -= ShapeColorChangedEvent;
					((TextedShape)shape).FontChanged -= ShapeFontChangedEvent;
				}
				shape.VisibleChanged -= ShapeVisibleChangedEvent;
			}
		}

		public SmoothingMode SmoothingMode {
			get {
				return _SmoothingMode;
			}
			set {
				_SmoothingMode = value;
				_Graphics.SmoothingMode = _SmoothingMode;
				_BufferedGraphics.SmoothingMode = _SmoothingMode;
				RedrawWindow();
				Redraw();
			}
		}

		public int ShapesCount {
			get {
				return _Shapes.Count;
			}
		}

		public void AddShape(Shape shape) {
			if (shape != null && !_Shapes.Contains(shape)) {
				_Shapes.Add(shape);
				RegisterShapeEvents(true,shape);
				if (shape.Visible) {
					RedrawRect(shape.Bounds);
				}
			}
		}

		public void AddShapes(Shape[] shapes) {
			foreach (Shape shape in shapes) {
				AddShape(shape);
			}
		}

		public void RemoveShape(int shapeIndex) {
			if (shapeIndex >= 0 && shapeIndex < _Shapes.Count) {
				RemoveShape(_Shapes[shapeIndex]);
			}
		}

		public void RemoveShape(Shape shape) {
			if (_Shapes.Contains(shape)) {
				_Shapes.Remove(shape);
				RegisterShapeEvents(false,shape);
				if (shape.Visible) {
					RedrawRect(shape.Bounds);
				}
			}
		}

		public void RemoveShapes() {
			foreach (Shape shape in _Shapes) {
				RegisterShapeEvents(false,shape);
			}
			_Shapes.Clear();
			Redraw();
		}

		public void InvertShapes(int shapeIndex1, int shapeIndex2) {
			if (shapeIndex1 != shapeIndex2 && shapeIndex1 >= 0 && shapeIndex1 < _Shapes.Count && shapeIndex2 >= 0 && shapeIndex2 < _Shapes.Count) {
				Shape shape = _Shapes[shapeIndex1];
				_Shapes[shapeIndex1] = _Shapes[shapeIndex2];
				_Shapes[shapeIndex2] = shape;
				RedrawRect(_Shapes[shapeIndex1].Bounds);
				RedrawRect(_Shapes[shapeIndex2].Bounds);
			}
		}

		public void InvertShapes(Shape shape1, Shape shape2) {
			InvertShapes(_Shapes.IndexOf(shape1),_Shapes.IndexOf(shape2));
		}

		public void InvertShapes() {
			_Shapes.Reverse();
			Redraw();
		}

		public Shape GetShape(int shapeIndex) {
			return shapeIndex >= 0 && shapeIndex < _Shapes.Count ? _Shapes[shapeIndex]:null;
		}

		public int GetShapeIndex(Shape shape) {
			return _Shapes.IndexOf(shape);
		}

		public bool Contains(Shape shape) {
			return _Shapes.Contains(shape);
		}

		public Shape ShapeOnPoint(Point p) {
			return ShapeOnPoint(p.X,p.Y);
		}

		public Shape ShapeOnPoint(int x, int y) {
			for (int i = _Shapes.Count-1; i >= 0; i--) {
				if (_Shapes[i].OnPoint(x,y)) {
					return _Shapes[i];
				}
			}
			return null;
		}

		public Shape[] ShapesOnPoint(Point p) {
			return ShapesOnPoint(p.X,p.Y);
		}

		public Shape[] ShapesOnPoint(int x, int y) {
			List<Shape> h = new List<Shape>();
			foreach (Shape shape in _Shapes) {
				if (shape.OnPoint(x,y)) {
					h.Add(shape);
				}
			}
			return h.ToArray();
		}

		protected override void OnBackColorChanged(EventArgs e) {
			base.OnBackColorChanged(e);
			_Brush = new SolidBrush(BackColor);
			RedrawWindow();
			Redraw();
		}

		protected override void OnSizeChanged(EventArgs e) {
			base.OnSizeChanged(e);
			_Bounds = new Rectangle(0,0,Size.Width,Size.Height);
			lock (_Graphics) {
				_Graphics = Graphics.FromHwnd(Handle);
				_Graphics.SmoothingMode = _SmoothingMode;
				_BufferedGraphics.SmoothingMode = _SmoothingMode;
			}
			RedrawWindow();
			Redraw();
		}

		protected override void OnEnter(EventArgs e) {
			base.OnEnter(e);
			_Brush = new SolidBrush(BackColor);
			_Bounds = new Rectangle(0,0,Size.Width,Size.Height);
			lock (_Graphics) {
				_Graphics = Graphics.FromHwnd(Handle);
				_Graphics.SmoothingMode = _SmoothingMode;
				_BufferedGraphics.SmoothingMode = _SmoothingMode;
			}
			RedrawWindow();
		}

		protected override void OnPaint(PaintEventArgs e) {
			base.OnPaint(e);
			RedrawWindow();
			Redraw();
		}

		public Render() {
			_Shapes = new List<Shape>();
			_Graphics = Graphics.FromHwnd(Handle);
			_BufferedBitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width,Screen.PrimaryScreen.Bounds.Height,_Graphics);
			_BufferedGraphics = Graphics.FromImage(_BufferedBitmap);
			_SmoothingMode = SmoothingMode.AntiAlias;
			_Brush = new SolidBrush(Color.Empty);
		}
	}
}