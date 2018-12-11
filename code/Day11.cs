using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
	class DayEleven
	{
		public static void PartOne()
		{
			int[,] matrix = new int[300, 300];

			int input = 4172;

			for (int i = 0; i < 300; i++)
			{
				for (int j = 0; j < 300; j++)
				{
					int x = i + 1;
					int y = j + 1;
					int id = x + 10;
					int power = id * y;
					power += input;
					power *= id;
					power = (power / 100) % 10;
					power -= 5;
					matrix[i, j] = power;
				}
			}

			int maximum = -100000;
			int maxX = 0;
			int maxY = 0;

			int size = 3;
			
			for (int i = 0; i < (300 - size + 1); i++)
			{
				for (int j = 0; j < (300 - size + 1); j++)
				{
					int total = 0;
					for (int k = i; k <= i + size - 1; k++)
					{
						for (int l = j; l <= j + size - 1; l++)
						{
							total += matrix[k, l];
						}
					}

					if (total > maximum)
					{
						maximum = total;
						maxX = i + 1;
						maxY = j + 1;
					}
				}
			}

			Console.WriteLine("Max value is {0},{1}", maxX, maxY);
		}

		public static void PartTwo()
		{
			Dictionary<Tuple<int, int, int>, int> totals = new Dictionary<Tuple<int, int, int>, int>();

			int input = 4172;

			for (int i = 0; i < 300; i++)
			{
				for (int j = 0; j < 300; j++)
				{
					int x = i + 1;
					int y = j + 1;
					int id = x + 10;
					int power = id * y;
					power += input;
					power *= id;
					power = (power / 100) % 10;
					power -= 5;
					totals[new Tuple<int, int, int>(x, y, 1)] = power;
				}
			}

			int maximum = -1000000;
			int maxX = 0;
			int maxY = 0;
			int maxSize = 0;

			for (int size = 2; size <= 300; size++)
			{
				for (int x = 1; x < 300 - size + 1; x++)
				{
					for (int y = 1; y < 300 - size + 1; y++)
					{
						Tuple<int, int, int> location = new Tuple<int, int, int>(x, y, size);

						totals[location] = 	totals[new Tuple<int, int, int>(x, y, size - 1)] +
											totals[new Tuple<int, int, int>(x + 1, y, size - 1)] +
											totals[new Tuple<int, int, int>(x, y + 1, size - 1)] +
											totals[new Tuple<int, int, int>(x + 1, y + 1, size - 1)] -
											(size > 2 ? totals[new Tuple<int, int, int>(x + 1, y + 1, size - 2)] * 4 : 0);

						if (totals[location] > maximum)
						{
							maximum = totals[location];
							maxX = x;
							maxY = y;
							maxSize = size;
						}
					}
				}
			}

			Console.WriteLine("Max coords are {0},{1},{2}", maxX, maxY, maxSize);
		}
	}
}