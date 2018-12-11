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
			int maxSize = 0;

			for (int size = 1; size < 300; size++)
				{
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
							maxSize = size;
						}
					}
				}
			}

			Console.WriteLine("Max value is {0},{1} with size {2}", maxX, maxY, maxSize);
		}

		public static void PartTwo()
		{
			
		}
	}
}