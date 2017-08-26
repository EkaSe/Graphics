using System;
using System.IO;

namespace Graphics
{
	public class OutputPrinter
	{
		static public void ClearLog () {
			if (File.Exists (LogPath))
				File.Delete (LogPath);
		}

		public static string LogPath {
			get {
				if (logPath == null) {
					string currentPath = Directory.GetCurrentDirectory ();
					logPath = currentPath.Substring (0, currentPath.IndexOf (@"Graphics/Graphics")) + @"Graphics/Graphics/Log.txt";
				}
				return logPath;
			}
		}
		static string logPath;

		static public bool PrintToConsoleAndTxt (string message) {
			try {
				if (!File.Exists (LogPath)) {
					using (StreamWriter sw = File.CreateText(LogPath)) 
					{
						sw.WriteLine(message);
					}
				} else {
					using (StreamWriter sw = new StreamWriter (LogPath, true)) {
						sw.WriteLine(message);
					}
				}
				Console.WriteLine (message);
				return true;
			} catch (Exception e) {
				Console.WriteLine ("Couldn't write to log " + LogPath);
				Console.WriteLine (e.Message);
				return false;
			}
		}

		static public void MessageReceived (object sender, string message) {
			PrintToConsoleAndTxt (message);
		}
	}
}

