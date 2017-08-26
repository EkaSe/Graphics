using System;

namespace Graphics
{
	public class Bezier
	{
		Point [] points;
		int n;

		public Bezier (params Point [] controlPoints) {
			points = controlPoints;
			n = points.Length - 1;
		}

		/// <summary>
		/// Gets the point of the curve.
		/// </summary>
		/// <returns>The point.</returns>
		/// <param name="p">p should be in [0, 1] interval</param>
		public Point GetPoint (double p) {
			double t = p;
			if (p > 1)
				t = 1;
			if (p < 0)
				t = 0;

			double coef = Math.Pow (1 - t, n);
			double x = 0; 
			double y = 0;
			for (int i = 0; i <= n; i++) {
				x += points [i].X * coef;
				y += points [i].Y * coef;
				coef = coef * t / (1 - t) * (n - i) / (i + 1);
			}
			Point result = new Point (x, y);
			return result;
		}
	}
}

