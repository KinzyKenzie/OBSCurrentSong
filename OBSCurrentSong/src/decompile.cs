/// OBSCurrentSong Project by Rotinx
/// https://github.com/Rotinx/OBSCurrentSong
/// 
/// Decompiled with dnSpy
/// Sourced from OBSCurrentSong Release V1.28

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Newtonsoft.Json;

namespace OBSCurrentSong
{
	// Token: 0x02000003 RID: 3
	internal class Program
	{
		// Token: 0x06000001 RID: 1
		[DllImport("Kernel32")]
		private static extern bool SetConsoleCtrlHandler(Program.EventHandler handler, bool add);

		// Token: 0x06000002 RID: 2 RVA: 0x00002050 File Offset: 0x00000250
		private static bool Handler(Program.CtrlType sig)
		{
			File.WriteAllText("./artist.txt", "");
			File.WriteAllText("./song.txt", "");
			File.WriteAllText("./currentsong.txt", "");
			Program.exitSystem = true;
			Environment.Exit(-1);
			return true;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002098 File Offset: 0x00000298
		private static void Main()
		{
			Program._handler = (Program.EventHandler)Delegate.Combine(Program._handler, new Program.EventHandler(Program.Handler));
			Program.SetConsoleCtrlHandler(Program._handler, true);
			new Program().Start();
			while (!Program.exitSystem)
			{
				Thread.Sleep(500);
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020F0 File Offset: 0x000002F0
		public void Start()
		{
			if (!File.Exists("config.json"))
			{
				Program.OBConfig = default(Config);
				string contents = JsonConvert.SerializeObject(Program.OBConfig, Formatting.Indented);
				File.WriteAllText("config.json", contents);
			}
			else
			{
				Program.OBConfig = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));
			}
			if (!File.Exists("temp"))
			{
				File.WriteAllText("temp", "");
				Process.Start("support.html");
			}
			else
			{
				Program.OBConfig = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));
			}
			Console.Title = "OBSCurrentSong";
			string text = "";
			Console.WriteLine("Ready... and waiting!");
			for (;;)
			{
				try
				{
					Process[] processesByName = Process.GetProcessesByName("Spotify");
					if (processesByName.Length == 0)
					{
						Console.Clear();
						Console.WriteLine("Please start Spotify.");
						File.WriteAllText("./artist.txt", "");
						File.WriteAllText("./song.txt", "");
						File.WriteAllText("./currentsong.txt", "");
					}
					Process[] array = processesByName;
					for (int i = 0; i < array.Length; i++)
					{
						string mainWindowTitle = array[i].MainWindowTitle;
						if (mainWindowTitle != "" && mainWindowTitle != text)
						{
							Console.Clear();
							string[] array2 = mainWindowTitle.Split(new char[]
							{
								'-'
							}, 2);
							if (array2[0] != "Spotify" && array2[0] != "Spotify Free" && array2[0] != "Spotify Premium")
							{
								File.WriteAllText("./artist.txt", array2[0]);
								File.WriteAllText("./song.txt", array2[1].TrimStart(new char[]
								{
									' '
								}));
								File.WriteAllText("./currentsong.txt", string.Concat(new string[]
								{
									Program.OBConfig.subprefix,
									array2[1].TrimStart(new char[]
									{
										' '
									}),
									Program.OBConfig.separator,
									array2[0],
									Program.OBConfig.spacing
								}));
								Console.WriteLine("Currently playing: " + mainWindowTitle);
								Console.Title = "OBSCurrentSong | Currently playing: " + mainWindowTitle;
								Console.WriteLine("Prev song: " + text);
								text = mainWindowTitle;
							}
							else
							{
								File.WriteAllText("./artist.txt", "");
								File.WriteAllText("./song.txt", "");
								File.WriteAllText("./currentsong.txt", "");
								Console.WriteLine("Currently playing: ");
								Console.Title = "OBSCurrentSong | Currently playing: ";
								Console.WriteLine("Prev song: " + text);
								text = "";
							}
						}
					}
					Thread.Sleep(3000);
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x04000004 RID: 4
		private static bool exitSystem;

		// Token: 0x04000005 RID: 5
		private static Program.EventHandler _handler;

		// Token: 0x04000006 RID: 6
		private static Config OBConfig;

		// Token: 0x02000004 RID: 4
		// (Invoke) Token: 0x06000008 RID: 8
		private delegate bool EventHandler(Program.CtrlType sig);

		// Token: 0x02000005 RID: 5
		private enum CtrlType
		{
			// Token: 0x04000008 RID: 8
			CTRL_C_EVENT,
			// Token: 0x04000009 RID: 9
			CTRL_BREAK_EVENT,
			// Token: 0x0400000A RID: 10
			CTRL_CLOSE_EVENT,
			// Token: 0x0400000B RID: 11
			CTRL_LOGOFF_EVENT = 5,
			// Token: 0x0400000C RID: 12
			CTRL_SHUTDOWN_EVENT
		}
	}
}
