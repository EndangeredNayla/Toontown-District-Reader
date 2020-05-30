using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace DDR.TTR
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			for (;;)
			{
				using (FileStream stream = File.Open((from f in Program.logsDirectory.GetFiles()
				orderby f.LastWriteTime descending
				select f).First<FileInfo>().FullName, FileMode.Open, FileAccess.Read, FileShare.Read | FileShare.Write | FileShare.Delete))
				{
					using (StreamReader reader = new StreamReader(stream))
					{
						while (!reader.EndOfStream)
						{
							string line = string.Empty;
							string lastLine = string.Empty;
							while ((line = reader.ReadLine()) != null)
							{
								if (line.Contains("Entering shard"))
								{
									lastLine = line;
								}
							}
							string shardNumber = string.Empty;
							if (lastLine.Length != 0)
							{
								shardNumber = lastLine.Substring(lastLine.IndexOf(":") + ":OTPClientRepository: Entering shard ".Length).Remove(4, 5);
							}
							else if (lastLine.Length == 0)
							{
								break;
							}
							int shardNumberInt = int.Parse(shardNumber);
							if (Program.districtDictionary.ContainsKey(shardNumberInt))
							{
								string districtName = Program.districtDictionary[shardNumberInt];
								if (File.Exists(Program.districtFile.FullName))
								{
									File.WriteAllText(Program.districtFile.FullName, districtName);
								}
								else if (!File.Exists(Program.districtFile.FullName))
								{
									File.Create(Program.districtFile.FullName);
									File.WriteAllText(Program.districtFile.FullName, districtName);
								}
								Program.ClearCurrentConsoleLine();
								Console.Write(string.Format("{0} = {1}\r", shardNumberInt, districtName));
								Thread.Sleep(TimeSpan.FromSeconds(1.0));
							}
						}
					}
				}
			}
		}

		public static void ClearCurrentConsoleLine()
		{
			int currentLineCursor = Console.CursorTop;
			Console.SetCursorPosition(0, Console.CursorTop);
			Console.Write(new string(' ', Console.WindowWidth));
			Console.SetCursorPosition(0, currentLineCursor);
		}

		private static DirectoryInfo currentDirectory = new DirectoryInfo(Environment.CurrentDirectory);

		private static DirectoryInfo logsDirectory = new DirectoryInfo(Program.currentDirectory.FullName + "\\logs");

		private static FileInfo districtFile = new FileInfo(Program.currentDirectory.FullName + "\\district.txt");

		private const string shardIndicator = "Entering shard";

		private static Dictionary<int, string> districtDictionary = new Dictionary<int, string>
		{
			{
				5000,
				"Gulp Gulch"
			},
			{
				5010,
				"Splashport"
			},
			{
				5020,
				"Fizzlefield"
			},
			{
				5030,
				"Whoosh Rapids"
			},
			{
				5040,
				"Blam Canyon"
			},
			{
				5050,
				"Hiccup Hills"
			},
			{
				5060,
				"Splat Summit"
			},
			{
				5070,
				"Thwackville"
			},
			{
				5080,
				"Zoink Falls"
			},
			{
				5090,
				"Kaboom Cliffs"
			},
			{
				5100,
				"Bounceboro"
			},
			{
				5110,
				"Boingbury"
			}
		};
	}
}
