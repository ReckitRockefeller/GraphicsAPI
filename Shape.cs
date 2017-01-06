using System;
using System.Drawing;

namespace MadGrap
{
	public abstract class Shape {
		protected bool visible;
		protected Rectangle bounds;
		protected int locationOffset;
		protected int sizeOffset;
		protected int x, y;
		protected int w, h;

		public Rectangle Bounds {
			get {
				return bounds;
			}
		}

		public event EventHandler MoveBegin;

		protected virtual void OnMoveBegin() {
			if (MoveBegin != null) {
				MoveBegin(this,EventArgs.Empty);
			}
		}

		public event EventHandler Move;

		protected virtual void OnMove() {
			if (Move != null) {
				Move(this,EventArgs.Empty);
			}
		}

		public int X {
			get {
				return x;
			}
			set {
				OnMoveBegin();
				x = value;
				bounds.X = x-locationOffset;
				OnMove();
			}
		}

		public int Y {
			get {
				return y;
			}
			set {
				OnMoveBegin();
				y = value;
				bounds.Y = y-locationOffset;
				OnMove();
			}
		}

		public Point Location {
			get {
				return new Point(x,y);
			}
			set {
				OnMoveBegin();
				x = value.X;
				bounds.X = x-locationOffset;
				y = value.Y;
				bounds.Y = y-locationOffset;
				OnMove();
			}
		}

		public event EventHandler ResizeBegin;

		protected virtual void OnResizeBegin() {
			if (ResizeBegin != null) {
				ResizeBegin(this,EventArgs.Empty);
			}
		}

		public event EventHandler Resize;

		protected virtual void OnResize() {
			if (Resize != null) {
				Resize(this,EventArgs.Empty);
			}
		}

		public int Width {
			get {
				return w;
			}
			set {
				OnResizeBegin();
				w = value;
				bounds.Width = w+sizeOffset;
				OnResize();
			}
		}

		public int Height {
			get {
				return h;
			}
			set {
				OnResizeBegin();
				h = value;
				bounds.Height = h+sizeOffset;
				OnResize();
			}
		}

		public Size Size {
			get {
				return new Size(w,h);
			}
			set {
				OnResizeBegin();
				w = value.Width;
				bounds.Width = w+sizeOffset;
				y = value.Height;
				bounds.Height = h+sizeOffset;
				OnResize();
			}
		}

		public event EventHandler VisibleChanged;

		protected virtual void OnVisibleChanged() {
			if (VisibleChanged != null) {
				VisibleChanged(this,EventArgs.Empty);
			}
		}

		public bool Visible {
			get {
				return visible;
			}
			set {
				if (visible != value) {
					visible = value;
					OnVisibleChanged();
				}
			}
		}

		public bool IntersectsWith(Rectangle rect) {
			return bounds.IntersectsWith(rect);
		}

		public abstract void InternalDraw(Graphics g);
		public abstract void InternalDraw(Graphics g, Point p);
		public abstract void InternalDraw(Graphics g, int x, int y);
		public abstract bool OnPoint(Point p);
		public abstract bool OnPoint(int x, int y);

		protected Shape() {
			visible = true;
		}
	}
}