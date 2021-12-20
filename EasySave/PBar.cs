using EasySave.Interfaces;
using System;
using System.Diagnostics;
using System.Threading;

namespace EasySave
{
	static class PBar
	{
		//Progression bar used when copying files
		public static void ProgBar(string FileName)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write(FileName + " being copied  . . . ");
			Console.ForegroundColor = ConsoleColor.White;

			using (var progress = new ProgressBar())		//Update the display of the progress bar depending on the progression
			{
				for (int i = 0; i <= 100; i++)
				{
					progress.Report((double)i / 100);
					Thread.Sleep(3);
				}
			}
			Console.WriteLine("Done");
			Console.ForegroundColor = ConsoleColor.White;

		}

	}
}