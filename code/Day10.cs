using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
	class Star
	{
		public int x { get; set; }
		public int y { get; set; }
		public int vx { get; set; }
		public int vy { get; set; }

		public Star(int xVal, int yVal, int vxVal, int vyVal)
		{
			x = xVal;
			y = yVal;
			vx = vxVal;
			vy = vyVal;
		}

		public void Iterate()
		{
			x += vx;
			y += vy;
		}

	}
	class DayTen
	{
		public static bool PrintIteration(List<Star> stars)
		{
			IEnumerable<int> xValues = stars.Select(star => star.x);
			int minX = xValues.Min();
			int maxX = xValues.Max();

			IEnumerable<int> yValues = stars.Select(star => star.y);
			int minY = yValues.Min();
			int maxY = yValues.Max();

			int width = maxX - minX + 1;

			int height = maxY - minY + 1;

			if (height == 10)
			{
				bool[,] matrix = new bool[height, width];

				foreach (Star star in stars)
				{
					matrix[star.y - minY, star.x - minX] = true;
				}

				for (int i = 0; i < matrix.GetLength(0); i++)
				{
					for (int j = 0; j < matrix.GetLength(1); j++)
					{
						if (matrix[i, j])
						{
							Console.Write('#');
						}
						else
						{
							Console.Write('.');
						}
					}
					Console.Write('\n');
				}

				return true;
			}
			return false;
		}

		public static void PartOne()
		{
			List<string> input = Utils.GetLinesFromFile("input/Day10Input.txt");

			List<Star> positions = new List<Star>(input.Select(currentLine =>
			{
				int x = Convert.ToInt32(currentLine.Substring(10, 6));
				int y = Convert.ToInt32(currentLine.Substring(18, 6));
				int vx = Convert.ToInt32(currentLine.Substring(36, 2));
				int vy = Convert.ToInt32(currentLine.Substring(40, 2));

				return new Star(x, y, vx, vy);
			}));

			int iteration = 0;
			while(true)
			{
				bool done = PrintIteration(positions);

				if (done)
					break;

				foreach(Star star in positions)
				{
					star.Iterate();
				}
				iteration++;
			}
		}

		public static void PartTwo()
		{
			List<string> input = Utils.GetLinesFromFile("input/Day10Input.txt");

			List<Star> positions = new List<Star>(input.Select(currentLine =>
			{
				int x = Convert.ToInt32(currentLine.Substring(10, 6));
				int y = Convert.ToInt32(currentLine.Substring(18, 6));
				int vx = Convert.ToInt32(currentLine.Substring(36, 2));
				int vy = Convert.ToInt32(currentLine.Substring(40, 2));

				return new Star(x, y, vx, vy);
			}));

			int iteration = 0;
			while(true)
			{
				bool done = PrintIteration(positions);

				if (done)
					break;

				foreach(Star star in positions)
				{
					star.Iterate();
				}
				iteration++;
			}
			
			Console.WriteLine("Iteration: {0}", iteration);
		}
	}
}