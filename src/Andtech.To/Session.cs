﻿using Andtech.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Andtech.To
{

	public class Session
	{
		public Hotspot[] Hotspots { get; set; }
		public static bool UseGlobalMode { get; set; }

		private static readonly char PathDelimiter = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? ';' : ':';

		public static Session Load()
		{
			List<string> directories = new List<string>();

			if (!UseGlobalMode)
			{
				if (ShellUtil.Find(Environment.CurrentDirectory, ".to", out var toDirectory, FindOptions.RecursiveUp))
				{
					directories.Add(toDirectory);
				}
			}

			var toPath = Environment.GetEnvironmentVariable("TOPATH");
			directories.AddRange(toPath.Split(PathDelimiter, StringSplitOptions.RemoveEmptyEntries));
			
			var hotspots = new List<Hotspot>();
			foreach (var directory in directories)
			{
				var files = Directory.EnumerateFiles(directory, "*.json", SearchOption.TopDirectoryOnly)
					.Concat(Directory.EnumerateFiles(directory, "*.yaml", SearchOption.TopDirectoryOnly))
					.Concat(Directory.EnumerateFiles(directory, "*.yml", SearchOption.TopDirectoryOnly));
				hotspots.AddRange(files.SelectMany(Hotspot.ReadMany));
			}

			return new Session
			{
				Hotspots = hotspots.ToArray()
			};
		}
	}
}
