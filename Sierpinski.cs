using System;
using System.Collections;
using System.Collections.Generic;

namespace Graphics
{
	public class Sierpinski
	{
		List <List <Triangle>> levels;
		int currentLOD = 0;

		public Sierpinski (double x0, double y0, double x1, double y1, double x2, double y2) {
			levels = new List<List<Triangle>> ();
			levels [0] = new List<Triangle> ();
			levels [0] [0] = new Triangle (x0, y0, x1, y1, x2, y2);
		}

		public Sierpinski (double x0, double y0, double hx, double hy) {
			levels = new List<List<Triangle>> ();
			levels.Add (new List<Triangle> ());
			levels [0].Add (new Triangle (x0, y0, hx, hy));
		}

		Triangle [] GetNextLOD (Triangle basis) {
			Func <double, double, double, double, double, double, Triangle> GetOneSide = (x0, y0, x1, y1, x2, y2) => 
				new Triangle (
					(x1 + x2) / 2,
					(y1 + y2) / 2,
					x1 + (x2 - x0) / 2,
					y1 + (y2 - y0) / 2,
					x2 + (x1 - x0) / 2,
					y2 + (y1 - y0) / 2
				);
			Triangle [] result = new Triangle[3];
			result [0] = GetOneSide (basis.x0, basis.y0, basis.x1, basis.y1, basis.x2, basis.y2);
			result [1] = GetOneSide (basis.x1, basis.y1, basis.x2, basis.y2, basis.x0, basis.y0);
			result [2] = GetOneSide (basis.x2, basis.y2, basis.x0, basis.y0, basis.x1, basis.y1);
			return result;
		}

		public List <List <Triangle>> GetLOD (int lod) {
			List <List <Triangle>> result = new List<List<Triangle>> (lod);
			for (int i = currentLOD + 1; i <= lod; i++) {
				levels.Add (new List<Triangle> ((int) Math.Pow (3, i)));
				for (int j = 0; j < (levels [i - 1]).Count; j++) {
					Triangle[] nextLOD = GetNextLOD (levels [i - 1] [j]);
					levels [i].AddRange (nextLOD);
				}
			}
			currentLOD = lod;
			for (int i = 0; i <= lod; i++) {
				result.Add (new List<Triangle> ());
				for (int j = 0; j < levels [i].Count; j++) {
					result [i].Add (new Triangle (levels [i] [j]));
				}
			}
			return result;
		}
	}

	public class Triangle {
		public double x0; public double y0; 
		public double x1; public double y1;
		public double x2; public double y2;

		public Triangle (double x0, double y0, double x1, double y1, double x2, double y2) {
			this.x0 = x0; this.x1 = x1; this.x2 = x2;
			this.y0 = y0; this.y1 = y1; this.y2 = y2;
		}

		public Triangle (Triangle parent) {
			this.x0 = parent.x0; this.x1 = parent.x1; this.x2 = parent.x2;
			this.y0 = parent.y0; this.y1 = parent.y1; this.y2 = parent.y2;
		}

		/// <summary>
		/// Get equilateral triangle via coordinates of one vertex and end of altitude starting at the same vertex
		/// </summary>
		/// <param name="x0">x coordinate of vertex</param>
		/// <param name="y0">y coordinate of vertex</param>
		/// <param name="hx">x coordinate of altitude</param>
		/// <param name="hy">y coordinate of altitude</param>
		public Triangle (double x0, double y0, double Hx, double Hy) {
			double hx = Hx - x0;
			double hy = Hy - y0;
			double l = Math.Sqrt ((hx * hx + hy * hy) / 3.0) * 2.0;

			//a(x - x0)^2 + b(x - x0) + c = 0
			double a = 1.0 + hx*hx / hy / hy;
			double b = -1.5 * l * l * hx / hy / hy;
			double c = 9 / 16 * Math.Pow (l, 4) / hy - l * l;
			double d = b * b - 4 * a * c;
			double x1 = (-b + Math.Sqrt (d)) / 2 / a + x0;
			double x2 = (-b - Math.Sqrt (d)) / 2 / a + x0;
			Func<double, double> YforX = x => (0.75 * l * l - x * hx) / hy;
			double y1 = YforX (x1);
			double y2 = YforX (x2);

			this.x0 = x0; this.x1 = x1; this.x2 = x2;
			this.y0 = y0; this.y1 = y1; this.y2 = y2;
		}
	}
}

