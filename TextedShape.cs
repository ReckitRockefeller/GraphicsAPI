using System;
using System.Drawing;

namespace MadGrap
{
	public class TextedShape: Shape {
		string text;
		SolidBrush brush;
		Font font;

		public event EventHandler TextChanged;

		protected virtual void OnTextChanged() {
			if (TextChanged != null) {
				TextChanged(this,EventArgs.Empty);
			}
		}

		public string Text {
			get {
				return text;
			}
			set {
				if (text != value && value != null) {
					text = value;
					OnTextChanged();
				}
			}
		}

		public event EventHandler FontChanged;

		protected virtual void OnFontChanged() {
			if (FontChanged != null) {
				FontChanged(this,EventArgs.Empty);
			}
		}

		public Font Font {
			get {
				return font;
			}
			set {
				font = value;
				OnFontChanged();
			}
		}

		public event EventHandler ColorChanged;

		protected virtual void OnColorChanged() {
			if (ColorChanged != null) {
				ColorChanged(this,EventArgs.Empty);
			}
		}

		public Color Color {
			get {
				return brush.Color;
			}
			set {
				if (brush.Color != value) {
					brush.Color = value;
					OnColorChanged();
				}
			}
		}

		public override void InternalDraw(Graphics g) {
			g.DrawString(text,font,brush,new Rectangle(x,y,w,h));
		}

		public override void InternalDraw(Graphics g, Point p) {
			InternalDraw(g,p.X,p.Y);
		}

		public override void InternalDraw(Graphics g, int x, int y) {
			g.DrawString(text,font,brush,new Rectangle(x,y,w,h));
		}

		public override bool OnPoint(Point p) {
			return OnPoint(p.X,p.Y);
		}

		public override bool OnPoint(int x, int y) {
			if (text != "" && brush.Color.A != 0 && (x -= this.x) >= 0 && (y -= this.y) >= 0 && x <= bounds.Width && y <= bounds.Height) {
				Image img = Image.FromHbitmap(new Bitmap(1,1).GetHbitmap());
				Graphics g = Graphics.FromImage(img);
				g.DrawString(text,font,new SolidBrush(Color.Red),bounds);
				bool b = ((Bitmap)img).GetPixel(0,0).R == 255;
				img.Dispose();
				g.Dispose();
				return b;
			}
			return false;
		}

		public TextedShape(string text, Color color, Rectangle rectangle):
			this(text,color,rectangle.X,rectangle.Y,rectangle.Width,rectangle.Height) {
		}

		public TextedShape(string text, Color color, Point position, Size size):
			this(text,color,position.X,position.Y,size.Width,size.Height) {
		}

		public TextedShape(string text, Color color, int x, int y, int w, int h) {
			this.x = x;
			this.y = y;
			this.w = w;
			this.h = h;
			bounds.X = x;
			bounds.Y = y;
			bounds.Width = w;
			bounds.Height = h;
			this.text = text;
			font = SystemFonts.DefaultFont;
			brush = new SolidBrush(color);
		}

		protected TextedShape() {
			font = SystemFonts.DefaultFont;
			brush = new SolidBrush(Color.Empty);
		}
	}
}