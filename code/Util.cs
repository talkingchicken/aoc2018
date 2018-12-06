using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
	class Utils
	{
		private static IEnumerable<string> ReadFileLines(string filename)
		{
			string line;
			using (var reader = File.OpenText(filename))
			{
				while ((line = reader.ReadLine()) != null)
				{
					yield return line;
				}
			}
		}

		public static List<String> GetLinesFromFile(string filename)
		{
			return new List<string>(ReadFileLines(filename));
		}
	}
}