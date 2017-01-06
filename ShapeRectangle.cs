using System.Drawing;

namespace MadGrap
{
	public class ShapeRectangle: ColouredShape {
		public override void InternalDraw(Graphics g) {
			g.DrawRectangle(pen,x+penWidth,y+penWidth,w,h);
			g.FillRectangle(brush,x+penWidth+0.5F,y+penWidth+0.5F,w-1.0F,h-1.0F);
		}

		public override void InternalDraw(Graphics g, Point p) {
			InternalDraw(g,p.X,p.Y);
		}

		public override void InternalDraw(Graphics g, int x, int y) {
			g.DrawRectangle(pen,x+penWidth,y+penWidth,w,h);
			g.FillRectangle(brush,x+penWidth+0.5F,y+penWidth+0.5F,w-1.0F,h-1.0F);
		}

		public override bool OnPoint(Point p) {
			return OnPoint(p.X,p.Y);
		}

		public override bool OnPoint(int x, int y) {
			if ((x -= this.x) >= 0 && (y -= this.y) >= 0 && x <= bounds.Width && y <= bounds.Height) {
				Image img = Image.FromHbitmap(new Bitmap(1,1).GetHbitmap());
				Graphics g = Graphics.FromImage(img);
				if (pen.Color.A != 0) {
					g.DrawRectangle(new Pen(Color.Red,(int)pen.Width),penWidth-x,penWidth-y,w,h);
				}
				if (brush.Color.A != 0) {
					g.FillRectangle(new SolidBrush(Color.Red),penWidth-x+0.5F,penWidth-y+0.5F,w-1.0F,h-1.0F);
				}
				bool b = ((Bitmap)img).GetPixel(0,0).R == 255;
				img.Dispose();
				g.Dispose();
				return b;
			}
			return false;
		}

		public ShapeRectangle(Color color, Rectangle rectangle):
			this(color,rectangle.X,rectangle.Y,rectangle.Width,rectangle.Height) {
		}

		public ShapeRectangle(Color color, Point position, Size size):
			this(color,position.X,position.Y,size.Width,size.Height) {
		}

		public ShapeRectangle(Color color, int x, int y, int w, int h) {
			this.x = x;
			this.y = y;
			this.w = w;
			this.h = h;
			bounds.X = x;
			bounds.Y = y;
			bounds.Width = w+sizeOffset;
			bounds.Height = h+sizeOffset;
			brush = new SolidBrush(color);
		}
	}
}