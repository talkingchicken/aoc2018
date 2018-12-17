using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
	enum GridContents
	{
		None = 0,
		Clay = 1,
		NonstagnantWater = 2,
		StagnantWater = 3,
	}
	class DaySeventeen
	{
		public static GridContents GetAt(GridContents[,] grid, int x, int y)
		{
			if (x == -1)
				PrintGrid(grid);

			return grid[y, x];
		}

		public static void SetAt(GridContents[,] grid, int x, int y, GridContents value)
		{
			grid[y, x] = value;
		}

		public static void PartOne()
		{
			List<string> lines = Utils.GetLinesFromFile("input/Day17Input.txt");
			List<Tuple<int, int>> clay = new List<Tuple<int, int>>();
			foreach(string line in lines)
			{
				
					string[] coords = line.Split(", ");
					int firstValue = Convert.ToInt32(coords[0].Substring(2));

					string[] range = coords[1].Split("..");

					int secondStart = Convert.ToInt32(range[0].Substring(2));
					int secondEnd = Convert.ToInt32(range[1]);
					
					for (int i = secondStart; i <= secondEnd; i++)
					{
						Tuple<int, int> newClay;
						if (line.StartsWith("y"))
						{
							newClay = new Tuple<int, int>(i, firstValue);
						}
						else
						{
							newClay = new Tuple<int, int>(firstValue, i);
						}

						clay.Add(newClay);
					}
			}

			int minX = clay.Select(x => x.Item1).Min() - 1;
			int maxX = clay.Select(x => x.Item1).Max() + 1;
			int minY = clay.Select(x => x.Item2).Min();
			int maxY = clay.Select(x => x.Item2).Max();

			GridContents[,] grid = new GridContents[maxY + 1, maxX - minX + 1];

			foreach (Tuple<int, int> item in clay)
			{
				SetAt(grid, item.Item1 - minX, item.Item2, GridContents.Clay);
			}
			
			FillWithWaterFrom(grid, 500 - minX, minY - 1);

			int total = 0;
			for (int currentY = 0; currentY < grid.GetLength(0); currentY++)
			{
				for (int currentX = 0; currentX < grid.GetLength(1); currentX++)
				{
					switch(grid[currentY, currentX])
					{
						case GridContents.None:
							break;
						case GridContents.Clay:
							break;
						case GridContents.NonstagnantWater:
							total++;
							break;
						case GridContents.StagnantWater:
							total++;
							break;
					}
				}
			}

			Console.WriteLine("We can make {0} squares of water", total);
		}

		public static void PrintGrid(GridContents[,] grid)
		{
			for (int currentY = 0; currentY < grid.GetLength(0); currentY++)
			{
				for (int currentX = 0; currentX < grid.GetLength(1); currentX++)
				{
					char currentChar = '\0';
					switch(grid[currentY, currentX])
					{
						case GridContents.None:
							currentChar = ' ';
							break;
						case GridContents.Clay:
							currentChar = '#';
							break;
						case GridContents.NonstagnantWater:
							currentChar = '|';
							break;
						case GridContents.StagnantWater:
							currentChar = '~';
							break;
					}

					Console.Write(currentChar);
				}

				Console.WriteLine();
			}
		}
		public static void FillWithWaterFrom(GridContents[,] grid, int x, int y)
		{
			for(int currentY = findFloor(grid, x, y); currentY > y; currentY--)
			{
				bool wallsOnBothSides = true;

				SetAt(grid, x, currentY, GridContents.NonstagnantWater);
				if (currentY == grid.GetLength(0) - 1 || GetAt(grid, x, currentY + 1) == GridContents.NonstagnantWater)
				{
					continue;
				}

				int leftWall = 0;
				int rightWall = 0;

				int currentX = x;
				while(true)
				{
					currentX--;
					GridContents contents = GetAt(grid, currentX, currentY);

					if (contents == GridContents.Clay)
					{
						leftWall = currentX;
						break;
					}
					else if (contents == GridContents.NonstagnantWater)
					{
						wallsOnBothSides = false;
						break;
					}
					else
					{
						SetAt(grid, currentX, currentY, GridContents.NonstagnantWater);

						if (GetAt(grid, currentX, currentY + 1) == GridContents.None)
						{
							FillWithWaterFrom(grid, currentX, currentY);
							if (GetAt(grid, currentX, currentY + 1) == GridContents.NonstagnantWater)
							{
								wallsOnBothSides = false;
								break;
							}
						}
					}
				}

				currentX = x;

				while(true)
				{
					currentX++;
					
					GridContents contents = GetAt(grid, currentX, currentY);

					if (contents == GridContents.Clay)
					{
						rightWall = currentX;
						break;
					}
					else if (contents == GridContents.NonstagnantWater)
					{
						wallsOnBothSides = false;
						break;
					}
					else
					{
						SetAt(grid, currentX, currentY, GridContents.NonstagnantWater);

						if (GetAt(grid, currentX, currentY + 1) == GridContents.None)
						{
							FillWithWaterFrom(grid, currentX, currentY);
							if (GetAt(grid, currentX, currentY + 1) == GridContents.NonstagnantWater)
							{
								wallsOnBothSides = false;
								break;
							}
						}
					}
				}

				if (wallsOnBothSides)
				{
					for (currentX = leftWall + 1; currentX < rightWall; currentX++)
					{
						SetAt(grid, currentX, currentY, GridContents.StagnantWater);
					}
				}
			}
		}

		public static int findFloor(GridContents[,] grid, int x, int top)
		{
			for (int y = top + 1; y + 1 < grid.GetLength(0); y++)
			{
				if (GetAt(grid, x, y + 1) != GridContents.None)
				{
					return y;
				}
			}

			return grid.GetLength(0) - 1;

		}
		public static void PartTwo()
		{
			List<string> lines = Utils.GetLinesFromFile("input/Day17Input.txt");
			List<Tuple<int, int>> clay = new List<Tuple<int, int>>();
			foreach(string line in lines)
			{
				
					string[] coords = line.Split(", ");
					int firstValue = Convert.ToInt32(coords[0].Substring(2));

					string[] range = coords[1].Split("..");

					int secondStart = Convert.ToInt32(range[0].Substring(2));
					int secondEnd = Convert.ToInt32(range[1]);
					
					for (int i = secondStart; i <= secondEnd; i++)
					{
						Tuple<int, int> newClay;
						if (line.StartsWith("y"))
						{
							newClay = new Tuple<int, int>(i, firstValue);
						}
						else
						{
							newClay = new Tuple<int, int>(firstValue, i);
						}

						clay.Add(newClay);
					}
			}

			int minX = clay.Select(x => x.Item1).Min() - 1;
			int maxX = clay.Select(x => x.Item1).Max() + 1;
			int minY = clay.Select(x => x.Item2).Min();
			int maxY = clay.Select(x => x.Item2).Max();

			GridContents[,] grid = new GridContents[maxY + 1, maxX - minX + 1];

			foreach (Tuple<int, int> item in clay)
			{
				SetAt(grid, item.Item1 - minX, item.Item2, GridContents.Clay);
			}

			FillWithWaterFrom(grid, 500 - minX, minY - 1);

			int total = 0;
			for (int currentY = 0; currentY < grid.GetLength(0); currentY++)
			{
				for (int currentX = 0; currentX < grid.GetLength(1); currentX++)
				{
					switch(grid[currentY, currentX])
					{
						case GridContents.None:
							break;
						case GridContents.Clay:
							break;
						case GridContents.NonstagnantWater:
							break;
						case GridContents.StagnantWater:
							total++;
							break;
					}
				}
			}

			Console.WriteLine("{0} squares of water are stagnant", total);
		}
	}
}