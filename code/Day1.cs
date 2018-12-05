using System;
using System.IO;
using System.Collections.Generic;

namespace AdventOfCode
{
	class DayOne
	{
		public static void PartOne()
		{
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			dictionary.Add(0, 1);
			int value = 0;
			string line;

			StreamReader file = new StreamReader("input/Day1Input.txt");

			while((line = file.ReadLine()) != null)  
			{
				value += Convert.ToInt32(line);
			}

			file.Close();  

			Console.WriteLine("Final value is {0}", value);
		}

		public static void PartTwo()
		{
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			dictionary.Add(0, 1);
			int value = 0;
			string line;

			while (true)
			{
				StreamReader file = new StreamReader("input/Day1Input.txt");

				while((line = file.ReadLine()) != null)  
				{
					value += Convert.ToInt32(line);
					if (dictionary.ContainsKey(value))
					{
						file.Close();
						System.Console.WriteLine("Duplicated value is {0}", value);
						return;
					}
					else
					{
						dictionary.Add(value, 1);
					}
				}

				file.Close();  
			}
		}
	}
}
