using System;
using System.Drawing;

namespace MadGrap
{
	public abstract class ColouredShape: Shape {
		protected Pen pen;
		protected int penWidth;
		protected SolidBrush brush;

		public event EventHandler BorderWidthChangeBegin;

		protected virtual void OnBorderWidthChangeBegin() {
			if (BorderWidthChangeBegin != null) {
				BorderWidthChangeBegin(this,EventArgs.Empty);
			}
		}

		public event EventHandler BorderWidthChanged;

		protected virtual void OnBorderWidthChanged() {
			if (BorderWidthChanged != null) {
				BorderWidthChanged(this,EventArgs.Empty);
			}
		}

		public int BorderWidth {
			get {
				return (int)pen.Width;
			}
			set {
				if ((int)pen.Width != value) {
					OnBorderWidthChangeBegin();
					pen.Width = value;
					penWidth = (int)pen.Width/2+1;
					sizeOffset = (int)pen.Width+2;
					bounds.Width = w+sizeOffset;
					bounds.Height = h+sizeOffset;
					OnBorderWidthChanged();
				}
			}
		}

		public event EventHandler BorderColorChanged;

		protected virtual void OnBorderColorChanged() {
			if (BorderColorChanged != null) {
				BorderColorChanged(this,EventArgs.Empty);
			}
		}

		public Color BorderColor {
			get {
				return pen.Color;
			}
			set {
				if (pen.Color != value) {
					pen.Color = value;
					OnBorderColorChanged();
				}
			}
		}

		public event EventHandler BackgroundColorChanged;

		protected virtual void OnBackgroundColorChanged() {
			if (BackgroundColorChanged != null) {
				BackgroundColorChanged(this,EventArgs.Empty);
			}
		}

		public Color BackgroundColor {
			get {
				return brush.Color;
			}
			set {
				if (brush.Color != value) {
					brush.Color = value;
					OnBackgroundColorChanged();
				}
			}
		}

		protected ColouredShape() {
			pen = new Pen(Color.Empty);
			penWidth = (int)pen.Width/2+1;
			sizeOffset = (int)pen.Width+2;
			bounds.Width = w+sizeOffset;
			bounds.Height = h+sizeOffset;
		}
	}
}