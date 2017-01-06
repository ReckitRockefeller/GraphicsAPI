using System;
using System.Drawing;

namespace MadGrap
{
	public class PicturedShape: Shape {
		protected Image image;

		public event EventHandler ImageChanged;

		protected virtual void OnImageChanged() {
			if (ImageChanged != null) {
				ImageChanged(this,EventArgs.Empty);
			}
		}

		public Image Image {
			get {
				return image;
			}
			set {
				if (image != value) {
					image = value;
					OnImageChanged();
				}
			}
		}

		public override void InternalDraw(Graphics g) {
			if (image != null) {
				lock (image) {
					g.DrawImage(image,x,y,w,h);
				}
			}
		}

		public override void InternalDraw(Graphics g, Point p) {
			if (image != null) {
				lock (image) {
					g.DrawImage(image,p.X,p.Y,w,h);
				}
			}
		}

		public override void InternalDraw(Graphics g, int x, int y) {
			if (image != null) {
				lock (image) {
					g.DrawImage(image,x,y,w,h);
				}
			}
		}

		public override bool OnPoint(Point p) {
			return OnPoint(p.X,p.Y);
		}

		public override bool OnPoint(int x, int y) {
			if (image != null && (x -= this.x) >= 0 && (y -= this.y) >= 0 && x <= bounds.Width && y <= bounds.Height) {
				Image img = Image.FromHbitmap(new Bitmap(1,1).GetHbitmap());
				Graphics g = Graphics.FromImage(img);
				lock (image) {
					g.DrawImage(image,x,y,w,h);
				}
				bool b = ((Bitmap)img).GetPixel(0,0).A != 0;
				img.Dispose();
				g.Dispose();
				return b;
			}
			return false;
		}

		public PicturedShape(Image img, Rectangle rectangle):
			this(img,rectangle.X,rectangle.Y,rectangle.Width,rectangle.Height) {
		}

		public PicturedShape(Image img, Point position, Size size):
			this(img,position.X,position.Y,size.Width,size.Height) {
		}

		public PicturedShape(Image img, int x, int y, int w, int h) {
			this.x = x;
			this.y = y;
			this.w = w;
			this.h = h;
			bounds.X = x;
			bounds.Y = y;
			bounds.Width = w;
			bounds.Height = h;
			image = img;
		}

		protected PicturedShape() {
		}
	}
}