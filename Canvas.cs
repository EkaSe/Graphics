using System;
using System.Text;
using System.Collections.Generic;

namespace Graphics
{
	public class Canvas
	{
		public readonly int HSize;
		public readonly int VSize;
		char filler = '■';

		bool[,] canvas;

        public Canvas(int horizontalSize, int verticalSize) {
            HSize = horizontalSize;
            VSize = verticalSize;

            canvas = new bool[HSize, VSize];
            Clear();
        }

        public string ToString (bool border = true) {
			StringBuilder result = new StringBuilder();
            char[,] charArray = GetCharArray();
			for (int i = 0; i < VSize; i++) {
                for (int j = 0; j < HSize; j++) {
                    result.Append(charArray [j, i]);
				}
				if (i < VSize - 1)
					result.AppendLine();
			}
            return result.ToString();
        }

		public bool this[int i, int j]
		{
			get { return canvas [i, j]; }
			private set { canvas [i, j] = value; }
		}

        char[,] GetCharArray (bool border = true) {
            char[,] result = new char[HSize, VSize];
            for (int i = 0; i < VSize; i++) {
				for (int j = 0; j < HSize; j++) {
                    if (!canvas[j, i])
						result[j, i] = ' ';
                    else
                        result[j, i] = filler;
				}
			}
            if (border) {
                for (int i = 1; i < VSize - 1; i++) {
                    result[0, i] = '|';
                    result[HSize - 1, i] = '|';
                }
                for (int i = 1; i < HSize - 1; i++) {
					result[i, 0] = '═';
                    result[i, VSize - 1] = '─';
                }
                result[0, 0] = '╒';
                result[0, VSize - 1] = '└';
				result[HSize - 1, 0] = '╕';
				result[HSize - 1, VSize - 1] = '┘';
            }
            return result;
        }

		public void Clear () {
			for (int i = 0; i < VSize; i++) {
				for (int j = 0; j < HSize; j++) {
					canvas [j, i] = false;
				}
			}
		}

		public bool AddPoint (int x, int y) {
			if (!CheckCoords (x, y))
				return false;
			canvas [x, y] = true;
			return true;
		}

		public bool AddPoint (Point point) {
			int[] coords = point.DrawPoint ();
			return AddPoint (coords [0], coords [1]);
		}

		public bool AddPoint (double x, double y) {
			Point point = new Point (x, y);
			return AddPoint (point);
		}

		private bool CheckCoords (int x, int y) {
			if (x >= HSize || x < 0)
				return false;
			if (y >= VSize || y < 0)
				return false;
			return true;
		}

		public void AddLineBresenham (int x0, int y0, int x1, int y1) {
			if (x0 == x1) {
				for (int y = Math.Min (y0, y1); y <= Math.Max (y0, y1); y++) {
					AddPoint (x0, y);
				}
			} else {
				int x1_ = x1;
				int x0_ = x0;
				int y1_ = y1;
				int y0_ = y0;
				if (x0 > x1) {
					x1_ = x0;
					x0_ = x1;
					y1_ = y0;
					y0_ = y1;
				}
				int dx = x1_ - x0_;
				int dy = y1_ - y0_;
				int D = 2 * dy - dx;
				int y = y0_;

				if (dy >= 0) {
					for (int x = x0_; x <= x1_; x++) {
						AddPoint (x, y);
						if (D > 0) {
							y++;
							D = D - 2 * dx;
						}
						D = D + 2 * dy;
					}
				} else {
					D = -D;
					for (int x = x0_; x <= x1_; x++) {
						AddPoint (x, y);
						if (D > 0) {
							y--;
							D = D - 2 * dx;
						}
						D = D - 2 * dy;
					}
				}
			}
		}

		public void AddTriangle (int x0, int y0, int x1, int y1, int x2, int y2) {
			AddLineBresenham (x0, y0, x1, y1);
			AddLineBresenham (x0, y0, x2, y2);
			AddLineBresenham (x1, y1, x2, y2);
		}

		public void AddRectangle (int x0, int y0, int x1, int y1) {
			AddLineBresenham (x0, y0, x0, y1);
			AddLineBresenham (x1, y0, x1, y1);
			AddLineBresenham (x0, y1, x1, y1);
			AddLineBresenham (x0, y0, x1, y0);
		}

		public void AddCircle (int x0, int y0, int R) {
			for (double t = 0; t < 6.3; t += (1.0 / HSize / VSize)) {
				AddPoint (x0 + R * Math.Sin (t), y0 + R * Math.Cos (t));
			}
		}

		public void AddTriangle (Triangle triangle) {
			AddTriangle (
				(int)Math.Round (triangle.x0), 
				(int)Math.Round (triangle.y0), 
				(int)Math.Round (triangle.x1), 
				(int)Math.Round (triangle.y1), 
				(int)Math.Round (triangle.x2), 
				(int)Math.Round (triangle.y2)
			);
		}

		public void AddSierpinski (double x0, double y0, double hx, double hy, int LOD) {
			Sierpinski triangle = new Sierpinski (x0, y0, hx, hy);
			List <List <Triangle>> lods = triangle.GetLOD (3);
			for (int i = 0; i <= LOD; i++) {
				for (int j = 0; j < lods [i].Count; j++) {
					AddTriangle (lods[i] [j]);
				}
			}
		}

		public void AddBezier (params Point[] controlPoints) {
			Bezier curve = new Bezier (controlPoints);
			for (double t = 0; t < 1; t += (1.0 / HSize / VSize)) {
				AddPoint (curve.GetPoint (t));
			}
			AddPoint (curve.GetPoint (1));
		}

		public Canvas Clone () {
			Canvas clone = new Canvas (HSize, VSize);
			for (int i = 0; i < HSize; i++) {
				for (int j = 0; j < VSize; j++) {
					if (this [i, j])
						clone.AddPoint (i, j);
				}
			}
			return clone;
		}
	}

	public struct Point { 
		public double X { get; private set; }
		public double Y { get; private set; }

		public Point (double x, double y) {
			this.X = x;
			this.Y = y;
		}

		public int[] DrawPoint (){
			return new int[] { (int) Math.Round (X), (int) Math.Round (Y) };
		}
	}
}

