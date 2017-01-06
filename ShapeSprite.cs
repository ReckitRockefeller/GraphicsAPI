using System.Timers;
using System.Drawing;
using System.Collections.Generic;

namespace MadGrap
{
	public class ShapeSprite: PicturedShape {
		protected Timer timer;
		protected List<string> imagesIndex;
		protected Dictionary<string,List<Image>> images;
		protected Dictionary<string,List<double>> delays;
		protected string animationKey;
		protected int animationValue;

		void TimerElapsed(object sender, ElapsedEventArgs e) {
			Update();
		}

		public int Count {
			get {
				return images.Count;
			}
		}

		public int FrameIndex {
			get {
				return animationValue;
			}
		}

		public int AnimationIndex {
			get {
				return imagesIndex.IndexOf(animationKey);
			}
		}

		public string AnimationName {
			get {
				return animationKey;
			}
		}

		public int FramesCount(int animationIndex) {
			return animationIndex >= 0 && animationIndex < imagesIndex.Count ? images[imagesIndex[animationIndex]].Count:0;
		}

		public int FramesCount(string animationName) {
			return imagesIndex.Contains(animationName) ? images[animationName].Count:0;
		}

		public void Update() {
			if (imagesIndex.Count > 0) {
				if (animationValue < images[animationKey].Count-1) {
					animationValue++;
					Image = images[animationKey][animationValue];
					timer.Interval = delays[animationKey][animationValue];
				} else {
					animationValue = 0;
					Image = images[animationKey][animationValue];
					timer.Interval = delays[animationKey][animationValue];
				}
			} else if (animationValue != -1) {
				Image = null;
				animationKey = null;
				animationValue = -1;
				timer.Enabled = false;
			}
		}

		public void UpdateTo(int animationIndex) {
			if (animationIndex >= 0 && animationIndex < imagesIndex.Count) {
				UpdateTo(imagesIndex[animationIndex]);
			}
		}

		public void UpdateTo(string animationName) {
			if (animationKey != animationName && imagesIndex.Contains(animationName)) {
				animationKey = animationName;
				Update();
			}
		}

		public void UpdateTo(int animationIndex, int frameIndex) {
			if (animationIndex >= 0 && animationIndex < imagesIndex.Count) {
				UpdateTo(imagesIndex[animationIndex],frameIndex);
			}
		}

		public void UpdateTo(string animationName, int frameIndex) {
			if (animationKey != animationName && imagesIndex.Contains(animationName) && frameIndex >= 0 && frameIndex < images[animationName].Count) {
				animationKey = animationName;
				animationValue = frameIndex;
				Image = images[animationKey][animationValue];
				timer.Interval = delays[animationKey][animationValue];
			}
		}

		public void UpdateTo(int animationIndex, Image frame) {
			if (animationIndex >= 0 && animationIndex < imagesIndex.Count) {
				UpdateTo(imagesIndex[animationIndex],frame);
			}
		}

		public void UpdateTo(string animationName, Image frame) {
			if (animationKey != animationName && imagesIndex.Contains(animationName) && images[animationName].Contains(frame)) {
				animationKey = animationName;
				animationValue = images[animationName].IndexOf(frame);
				Image = images[animationKey][animationValue];
				timer.Interval = delays[animationKey][animationValue];
			}
		}

		public void AddFrame(string animationName, Image frame, double delay) {
			if (animationName != null && frame != null) {
				if (imagesIndex.Contains(animationName)) {
					if (!images[animationName].Contains(frame)) {
						images[animationName].Add(frame);
						delays[animationName].Add(delay);
					}
				} else {
					imagesIndex.Add(animationName);
					images.Add(animationName,new List<Image>());
					images[animationName].Add(frame);
					delays.Add(animationName,new List<double>());
					delays[animationName].Add(delay);
					if (animationValue == -1) {
						Image = frame;
						animationKey = animationName;
						animationValue = 0;
						timer.Enabled = true;
					}
				}
			}
		}

		public void AddFrames(string animationName, Image[] frames, double delay) {
			if (animationName != null && frames != null) {
				foreach (Image frame in frames) {
					AddFrame(animationName,frame,delay);
				}
			}
		}

		public void RemoveFrame(int animationIndex, int frameIndex) {
			if (animationIndex >= 0 && animationIndex < imagesIndex.Count) {
				RemoveFrame(imagesIndex[animationIndex],frameIndex);
			}
		}

		public void RemoveFrame(string animationName, int frameIndex) {
			if (imagesIndex.Contains(animationName) && frameIndex >= 0 && frameIndex < images[animationName].Count) {
				int count = images[animationName].Count;
				if (count == 1) {
					imagesIndex.Remove(animationName);
					images.Remove(animationName);
					delays.Remove(animationName);
					if (animationKey == animationName && animationValue == frameIndex) {
						UpdateTo(0);
					}
				} else {
					images[animationName].RemoveAt(frameIndex);
					delays[animationName].RemoveAt(frameIndex);
					if (animationKey == animationName && animationValue == frameIndex) {
						UpdateTo(animationKey,0);
					}
				}
			}
		}

		public void RemoveFrame(int animationIndex, Image frame) {
			if (animationIndex >= 0 && animationIndex < imagesIndex.Count) {
				RemoveFrame(imagesIndex[animationIndex],frame);
			}
		}

		public void RemoveFrame(string animationName, Image frame) {
			if (imagesIndex.Contains(animationName) && images[animationName].Contains(frame)) {
				int count = images[animationName].Count;
				int frameIndex = images[animationName].IndexOf(frame);
				if (count == 1) {
					imagesIndex.Remove(animationName);
					images.Remove(animationName);
					delays.Remove(animationName);
					if (animationKey == animationName && animationValue == frameIndex) {
						UpdateTo(0);
					}
				} else {
					images[animationName].RemoveAt(frameIndex);
					delays[animationName].RemoveAt(frameIndex);
					if (animationKey == animationName && animationValue == frameIndex) {
						UpdateTo(animationKey,0);
					}
				}
			}
		}

		public void RemoveFrames(int animationIndex) {
			if (animationIndex >= 0 && animationIndex < imagesIndex.Count) {
				RemoveFrames(imagesIndex[animationIndex]);
			}
		}

		public void RemoveFrames(string animationName) {
			if (imagesIndex.Contains(animationName)) {
				imagesIndex.Remove(animationName);
				images.Remove(animationName);
				delays.Remove(animationName);
				if (animationKey == animationName) {
					UpdateTo(0);
				}
			}
		}

		public void RemoveFrames() {
			imagesIndex.Clear();
			images.Clear();
			delays.Clear();
			if (animationValue != -1) {
				Image = null;
				animationKey = null;
				animationValue = -1;
				timer.Enabled = false;
			}
		}

		public void SetFrame(int animationIndex, int sourceIndex, Image img, double delay) {
			if (animationIndex >= 0 && animationIndex < imagesIndex.Count) {
				SetFrame(imagesIndex[animationIndex],sourceIndex,img,delay);
			}
		}

		public void SetFrame(string animationName, int sourceIndex, Image img, double delay) {
			if (img != null && imagesIndex.Contains(animationName) && sourceIndex >= 0 && sourceIndex < images[animationName].Count) {
				images[animationName][sourceIndex] = img;
				delays[animationName][sourceIndex] = delay;
				if (animationKey == animationName && animationValue == sourceIndex) {
					Image = img;
					timer.Interval = delay;
				}
			}
		}

		public void SetFrame(int animationIndex, Image frame, Image img, double delay) {
			if (animationIndex >= 0 && animationIndex < imagesIndex.Count) {
				SetFrame(imagesIndex[animationIndex],frame,img,delay);
			}
		}

		public void SetFrame(string animationName, Image frame, Image img, double delay) {
			int sourceIndex;
			if (img != null && imagesIndex.Contains(animationName) && (sourceIndex = images[animationName].IndexOf(frame)) != -1) {
				images[animationName][sourceIndex] = img;
				delays[animationName][sourceIndex] = delay;
				if (animationKey == animationName && animationValue == sourceIndex) {
					Image = img;
					timer.Interval = delay;
				}
			}
		}

		public void InvertFrames(int animationIndex, int frameIndex1, int frameIndex2) {
			if (animationIndex >= 0 && animationIndex < imagesIndex.Count) {
				InvertFrames(imagesIndex[animationIndex],frameIndex1,frameIndex2);
			}
		}

		public void InvertFrames(string animationName, int frameIndex1, int frameIndex2) {
			if (frameIndex1 != frameIndex2 && imagesIndex.Contains(animationName) && frameIndex1 >= 0 && frameIndex1 < images[animationName].Count && frameIndex2 >= 0 && frameIndex2 < images[animationName].Count) {
				Image frame = images[animationName][frameIndex1];
				images[animationName][frameIndex1] = images[animationName][frameIndex2];
				images[animationName][frameIndex2] = frame;
				if (animationKey == animationName) {
					if (animationValue == frameIndex1) {
						animationValue = frameIndex2;
					} else if (animationValue == frameIndex2) {
						animationValue = frameIndex1;
					}
				}
			}
		}

		public void InvertFrames(int animationIndex, Image frame1, Image frame2) {
			if (animationIndex >= 0 && animationIndex < imagesIndex.Count) {
				InvertFrames(imagesIndex[animationIndex],frame1,frame2);
			}
		}

		public void InvertFrames(string animationName, Image frame1, Image frame2) {
			int frameIndex1, frameIndex2;
			if (frame1 != frame2 && imagesIndex.Contains(animationName) && (frameIndex1 = images[animationName].IndexOf(frame1)) != -1 && (frameIndex2 = images[animationName].IndexOf(frame2)) != -1) {
				Image frame = images[animationName][frameIndex1];
				images[animationName][frameIndex1] = images[animationName][frameIndex2];
				images[animationName][frameIndex2] = frame;
				if (animationKey == animationName) {
					if (animationValue == frameIndex1) {
						animationValue = frameIndex2;
					} else if (animationValue == frameIndex2) {
						animationValue = frameIndex1;
					}
				}
			}
		}

		public void InvertFrames(int animationIndex) {
			InvertFrames(imagesIndex[animationIndex]);
		}

		public void InvertFrames(string animationName) {
			if (imagesIndex.Contains(animationName)) {
				images[animationName].Reverse();
				if (animationKey == animationName) {
					animationValue = (images[animationKey].Count-1)-animationValue;
				}
			}
		}

		public void InvertFrames() {
			imagesIndex.Reverse();
		}

		public Image GetFrame(int animationIndex, int frameIndex) {
			return animationIndex >= 0 && animationIndex < imagesIndex.Count ? images[imagesIndex[animationIndex]][frameIndex]:null;
		}

		public Image GetFrame(string animationName, int frameIndex) {
			return imagesIndex.Contains(animationName) ? images[animationName][frameIndex]:null;
		}

		public int GetFrameIndex(int animationIndex, Image frame) {
			return animationIndex >= 0 && animationIndex < imagesIndex.Count ? images[imagesIndex[animationIndex]].IndexOf(frame):-1;
		}

		public int GetFrameIndex(string animationName, Image frame) {
			return imagesIndex.Contains(animationName) ? images[animationName].IndexOf(frame):-1;
		}

		public bool Contains(int animationIndex, Image frame) {
			return animationIndex >= 0 && animationIndex < imagesIndex.Count && images[imagesIndex[animationIndex]].Contains(frame);
		}

		public bool Contains(string animationName, Image frame) {
			return imagesIndex.Contains(animationName) && images[animationName].Contains(frame);
		}

		public bool Contains(string animationName) {
			return imagesIndex.Contains(animationName);
		}

		public ShapeSprite(Rectangle rectangle):
			this(rectangle.X,rectangle.Y,rectangle.Width,rectangle.Height) {
		}

		public ShapeSprite(Point position, Size size):
			this(position.X,position.Y,size.Width,size.Height) {
		}

		public ShapeSprite(int x, int y, int w, int h) {
			this.x = x;
			this.y = y;
			this.w = w;
			this.h = h;
			bounds.X = x;
			bounds.Y = y;
			bounds.Width = w;
			bounds.Height = h;
			timer = new Timer();
			timer.AutoReset = true;
			timer.Elapsed += TimerElapsed;
			imagesIndex = new List<string>();
			images = new Dictionary<string,List<Image>>();
			delays = new Dictionary<string,List<double>>();
			animationValue = -1;
		}
	}
}