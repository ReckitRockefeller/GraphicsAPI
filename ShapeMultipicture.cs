using System.Drawing;
using System.Collections.Generic;

namespace MadGrap
{
	public class ShapeMultipicture: PicturedShape {
		protected List<Image> images;
		protected int imageIndex;

		public int Count {
			get {
				return images.Count;
			}
		}

		public int ImageIndex {
			get {
				return imageIndex;
			}
		}

		public void Update() {
			if (images.Count > 0) {
				if (imageIndex < images.Count-1) {
					imageIndex++;
					Image = images[imageIndex];
				} else if (imageIndex != 0) {
					imageIndex = 0;
					Image = images[imageIndex];
				}
			} else if (imageIndex != -1) {
				Image = null;
				imageIndex = -1;
			}
		}

		public void UpdateTo(int index) {
			if (index >= 0 && index < images.Count && imageIndex != index) {
				imageIndex = index;
				Image = images[imageIndex];
			}
		}

		public void UpdateTo(Image img) {
			int index = images.IndexOf(img);
			if (index != -1 && imageIndex != index) {
				imageIndex = index;
				Image = images[imageIndex];
			}
		}

		public void AddImage(Image img) {
			if (img != null) {
				images.Add(img);
				if (imageIndex == -1) {
					Image = img;
					imageIndex = 0;
				}
			}
		}

		public void AddImages(Image[] imgs) {
			foreach (Image img in imgs) {
				AddImage(img);
			}
		}

		public void RemoveImage(int index) {
			if (index >= 0 && index < images.Count) {
				images.RemoveAt(index);
				if (imageIndex == index) {
					Update();
				}
			}
		}

		public void RemoveImage(Image img) {
			int index = images.IndexOf(img);
			if (index != -1) {
				images.RemoveAt(index);
				if (imageIndex == index) {
					Update();
				}
			}
		}

		public void RemoveImages() {
			images.Clear();
			Update();
		}

		public void SetImage(int index, Image img) {
			if (index >= 0 && index < images.Count && images[index] != img) {
				images[index] = img;
				if (imageIndex == index) {
					Image = img;
				}
			}
		}

		public void SetImage(Image source, Image img) {
			int index = images.IndexOf(source);
			if (index != -1 && source != img) {
				images[index] = img;
				if (imageIndex == index) {
					Image = img;
				}
			}
		}

		public void InvertImages(int imgIndex1, int imgIndex2) {
			if (imgIndex1 != imgIndex2 && imgIndex1 >= 0 && imgIndex1 < images.Count && imgIndex2 >= 0 && imgIndex2 < images.Count) {
				Image img = images[imgIndex1];
				images[imgIndex1] = images[imgIndex2];
				images[imgIndex2] = img;
				Image = images[imageIndex];
			}
		}

		public void InvertImages(Image img1, Image img2) {
			InvertImages(images.IndexOf(img1),images.IndexOf(img2));
		}

		public void InvertImages() {
			images.Reverse();
			if (imageIndex != -1) {
				Image = images[imageIndex];
			}
		}

		public Image GetImage(int index) {
			return index >= 0 && index < images.Count ? images[index]:null;
		}

		public int GetImageIndex(Image img) {
			return images.IndexOf(img);
		}

		public bool Contains(Image img) {
			return images.Contains(img);
		}

		public ShapeMultipicture(Rectangle rectangle):
			this(rectangle.X,rectangle.Y,rectangle.Width,rectangle.Height) {
		}

		public ShapeMultipicture(Point position, Size size):
			this(position.X,position.Y,size.Width,size.Height) {
		}

		public ShapeMultipicture(int x, int y, int w, int h) {
			this.x = x;
			this.y = y;
			this.w = w;
			this.h = h;
			bounds.X = x;
			bounds.Y = y;
			bounds.Width = w;
			bounds.Height = h;
			images = new List<Image>();
			imageIndex = -1;
		}
	}
}