using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
	class DayTwelve
	{
		
		static List<bool> ProcessState(List<bool> grammars, List<bool> input)
		{
			List<bool> returnValue = new List<bool>();
			for (int i = -2; i <= input.Count + 1; i++)
			{
				int total = 0;
				
				for (int j = i - 2; j <= i + 2 && j < input.Count; j++)
				{
					if (j >= 0 && input[j])
					{
						total += 1 << (j - i + 2);
					}
				}

				returnValue.Add(grammars[total]);
			}

			return returnValue;
		}
		
		public static void PartOne()
		{
			List<string> input = Utils.GetLinesFromFile("input/Day12Input.txt");

			List<bool> grammars = new List<bool>(new bool[32]);

			string stateStr = input[0].Substring(15);

			List<bool> state = new List<bool>(stateStr.Select(x => (x == '#')));

			for (int i = 2; i < input.Count; i++)
			{
				int value = 0;
				string condition = input[i].Substring(0, 5);
				for (int j = 0; j < condition.Length; j++)
				{
					if (condition[j] == '#')
					{
						value += 1 << j;
					}
				}
				grammars[value] = (input[i][9] == '#');
			}

			for (int i = 0; i < 20; i++)
			{
				state = ProcessState(grammars, state);
			}

			int total = 0;
			for (int i = 0; i < state.Count; i++)
			{
				if (state[i])
				{
					total += i - 40;
				}
			}

			Console.WriteLine("total is {0}", total);
		}

		public static void PartTwo()
		{
			List<string> input = Utils.GetLinesFromFile("input/Day12Input.txt");

			List<bool> grammars = new List<bool>(new bool[32]);

			string stateStr = input[0].Substring(15);

			List<bool> state = new List<bool>(stateStr.Select(x => (x == '#')));
			
			for (int i = 2; i < input.Count; i++)
			{
				int value = 0;
				string condition = input[i].Substring(0, 5);
				for (int j = 0; j < condition.Length; j++)
				{
					if (condition[j] == '#')
					{
						value += 1 << j;
					}
				}
				grammars[value] = (input[i][9] == '#');
			}

			Dictionary<int, int> totals = new Dictionary<int, int>();
			List<int> totalList = new List<int>();
			{
				int start = 0;
				for (int j = 0; j < state.Count; j++)
				{
					if (state[j])
					{
						start += j;
					}
				}
				Console.WriteLine("{0}: {1}", 0, start);
				totals.Add(start, 0);
				totalList.Add(start);
			}
			
			int total = 0;

			for (int i = 1; i <= 500; i++)
			{
				state = ProcessState(grammars, state);

				total = 0;
				for (int j = 0; j < state.Count; j++)
				{
					if (state[j])
					{
						total += j - 2 * (i);
					}
				}

				if(totals.ContainsKey(total))
				{
					Console.WriteLine("Loop at iteration {0} with loop {1}", i, totals[total]);
					totalList.Add(total);
					//return;
				}
				else
				{
				Console.WriteLine("{0}: {1}", i, total);
				totals.Add(total, i);
				totalList.Add(total);
				}

				Console.WriteLine("difference: {0}", totalList[i] - totalList[i - 1]);
			}

			
			Console.WriteLine("total is {0}", total);
		}
	}
}