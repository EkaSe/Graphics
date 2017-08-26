using System;

namespace Graphics
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			OutputPrinter.ClearLog ();

			MessageReceived += OutputPrinter.MessageReceived;

			string drawing = 
				"        000\n     0000 0\n   000    0\n   0    000\n   0 0000 0\n   000    0\n   0      0\n" +
				"   0      0\n   0      0\n   0    000\n 000   0000\n0000   000 \n000        \n";

			OnMessageReceived (drawing);

			Canvas canvas = new Canvas (32, 32);
			canvas.AddPoint (5, 0);
			canvas.AddLineBresenham (30, 2, 1, 20);
			OnMessageReceived (canvas.ToString ());

			Canvas canvasBezier = new Canvas (64, 32);
			canvasBezier.AddBezier (
				new Point (1, 10),
				new Point (50, 10),
				new Point (10, 30),
				new Point (60, 30)
			);
			OnMessageReceived (canvasBezier.ToString ());

			Canvas canvasSierpinski = new Canvas (64, 50);
			canvasSierpinski.AddSierpinski (32, 1, 32, 20, 2);
			OnMessageReceived (canvasSierpinski.ToString ());

			Canvas canvasSierpinskiAnimated = new Canvas (80, 25);
			canvasSierpinskiAnimated.AddSierpinski (32, 1, 32, 15, 0);
			OnMessageReceived (canvasSierpinskiAnimated.ToString ());
			Console.ReadKey ();
			canvasSierpinskiAnimated.AddSierpinski (32, 1, 32, 15, 1);
			OnMessageReceived (canvasSierpinskiAnimated.ToString ());
			Console.ReadKey ();
			canvasSierpinskiAnimated.AddSierpinski (32, 1, 32, 15, 2);
			OnMessageReceived (canvasSierpinskiAnimated.ToString ());
			Console.ReadKey ();

			Canvas canvasGeometry = new Canvas (40, 30);
			canvasGeometry.AddRectangle (2, 20, 10, 5);
			canvasGeometry.AddCircle (20, 20, 8);
			OnMessageReceived (canvasGeometry.ToString ());

			Canvas canvasLife = (new Canvas (80, 25));
			canvasLife.AddSierpinski (32, 1, 32, 15, 2);
			ConwaysLife life = new ConwaysLife (canvasLife);
			while (Console.Read () != 'q') {
				OnMessageReceived (life.Step().ToString ());
			}

		}

		static public event EventHandler<string> MessageReceived;

		static public void OnMessageReceived(string message)
		{
			EventHandler<string> handler = MessageReceived;
			if (handler != null)
			{
				handler(null, message);
			}
		}
	}
}
