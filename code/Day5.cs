using System;
using System.IO;
using System.Collections.Generic;

namespace AdventOfCode
{
	class DayFive
	{
		private static bool CheckForReaction(char firstChar, char secondChar)
		{
			return firstChar != secondChar && char.ToUpper(firstChar) == char.ToUpper(secondChar);
		}
		
		private static Stack<char> ReactPolymer(string filename, char charToIgnore)
		{
			Stack<char> polymer = new Stack<char>();

			char currentChar;
			StreamReader file = new StreamReader(filename);

			while (file.Peek() >= 0)
			{
				currentChar = (char)file.Read();

				if (char.ToUpper(currentChar) == char.ToUpper(charToIgnore))
					continue;

				if (polymer.Count > 0 && CheckForReaction(currentChar, polymer.Peek()))
				{
					polymer.Pop();
				}
				else
				{
					polymer.Push(currentChar);
				}
			}
			file.Close();

			return polymer;
		}

		public static void PartOne()
		{
			Stack<char> polymer = ReactPolymer("input/Day5Input.txt", '0');

			Console.WriteLine("There are {0} units remaining", polymer.Count);
		}

		public static void PartTwo()
		{
			int minimumSize = 50000;
			char shortestChar = '0';

			for (char counter = 'a'; counter <= 'z'; counter++)
			{
				Stack<char> polymer = ReactPolymer("input/Day5Input.txt", counter);
				
				if (polymer.Count < minimumSize)
				{
					minimumSize = polymer.Count;
					shortestChar = counter;
				}
			}

			Console.WriteLine("Shortest Length is {0} by removing {1}", minimumSize, shortestChar);
		}
	}
}