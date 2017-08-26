using System;

namespace Graphics
{
	public class ConwaysLife
	{
		Canvas canvas;

		public ConwaysLife (Canvas initialState) {
			canvas = initialState.Clone ();
		}

		public Canvas Step () {
			Canvas step = new Canvas (canvas.HSize, canvas.VSize);
			for (int i = 0; i < canvas.HSize; i++) {
				for (int j = 0; j < canvas.VSize; j++) {
					int count = CountNeighbours (i, j);
					//if (canvas [i, j] && count < 2)
					//	step [i, j] = false;
					if (canvas [i, j] && count >= 2 && count <= 3)
						step.AddPoint (i, j);
					//if (canvas [i, j] && count > 3)
					//	step [i, j] = false;
					if (!canvas [i, j] && count == 3)
						step.AddPoint (i, j);
				}
			}
			canvas = step;
			return step;
		}

		private int CountNeighbours (int x, int y) {
			int count = 0;
			for (int i = -1; i < 2; i++) {
				for (int j = -1; j < 2; j++) {
					int xi = x + i;
					int yj = y + j;
					if (xi < 0)
						xi = canvas.HSize - 1;
					if (yj < 0)
						yj = canvas.VSize - 1;
					if (xi >= canvas.HSize)
						xi = 0;
					if (yj >= canvas.VSize)
						yj = 0;
					if (canvas [xi, yj] && (xi != x || yj != y) )
						count++;
				}
			}

			return count;
		}
	}
}

