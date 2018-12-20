using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
	class DayTwenty
	{
		public static void PartOne()
		{
			string line;
			StreamReader file = new StreamReader("input/Day20Input.txt");
			
			while ((line = file.ReadLine()) != null)
			{
				Dictionary<Tuple<int, int>, int> paths = new Dictionary<Tuple<int, int>, int>();
				Console.WriteLine(findFurthestDistance(paths, line, 1, 0, 0, 0));
			}
			
			file.Close();
		}

		static int findFurthestDistance(Dictionary<Tuple<int, int>, int> paths, string routes, int index, int x, int y, int currentDistance)
		{
			Tuple<int, int> currentLocation = new Tuple<int, int>(x, y);

			if (paths.ContainsKey(currentLocation) && paths[currentLocation] < currentDistance)
			{
				return -1;
			}

			if (paths.ContainsKey(currentLocation))
			{
				paths[currentLocation] = currentDistance;
			}
			else
			{
				paths.Add(currentLocation, currentDistance);
			}

			switch(routes[index])
			{
				case 'N':
					return findFurthestDistance(paths, routes, index+1, x, y + 1, currentDistance + 1);
				case 'S':
					return findFurthestDistance(paths, routes, index+1, x, y - 1, currentDistance + 1);
				case 'E':
					return findFurthestDistance(paths, routes, index+1, x + 1, y, currentDistance + 1);
				case 'W':
					return findFurthestDistance(paths, routes, index+1, x - 1, y, currentDistance + 1);
				case '(':
				{
					int max = findFurthestDistance(paths, routes, index + 1, x, y, currentDistance);
					
					int nextIndex = index;

					while (routes[nextIndex] != ')')
					{
						nextIndex = findNextBranch(routes, nextIndex + 1);

						if (nextIndex != -1)
						{
							if (routes[nextIndex] == '|')
							{
								max = Math.Max(max, findFurthestDistance(paths, routes, nextIndex+1, x, y, currentDistance));
							}
						}
					}

					return max;
				}
				case ')':
					return findFurthestDistance(paths, routes, index + 1, x, y, currentDistance);
				case '|':
				{
					int nextIndex = index;
					do
					{
						nextIndex = findNextBranch(routes, nextIndex + 1);
					} while (nextIndex != -1 && routes[nextIndex] != ')');

					return findFurthestDistance(paths, routes, nextIndex + 1, x, y, currentDistance);
				}
				case '$':
					return currentDistance;

				default:
					return -1;
			}
		}

		static int findNextBranch(string routes, int index)
		{
			int currentParen = 0;
			while (index < routes.Length)
			{
				if (routes[index] == '(')
					currentParen++;
				else if (routes[index] == ')')
				{
					if (0 == currentParen)
					{
						return index;
					}
					else
					{
						currentParen--;
					}
				}
				else if (routes[index] == '|'  && 0 == currentParen)
					return index;

				index++;
			}

			return -1;
		}

		public static void PartTwo()
		{
			string line;
			StreamReader file = new StreamReader("input/Day20Input.txt");
			
			while ((line = file.ReadLine()) != null)
			{
				Dictionary<Tuple<int, int>, int> paths = new Dictionary<Tuple<int, int>, int>();

				findFurthestDistance(paths, line, 1, 0, 0, 0);
				Console.WriteLine(paths.Where(x => x.Value >= 1000).Count());
			}
			
			file.Close();
		}
	}
}