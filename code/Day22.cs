using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AdventOfCode
{
	class DayTwentyTwo
	{
		
		enum RegionType
		{
			Rocky = 0,
			Wet = 1,
			Narrow = 2,
		}
		
		public static void PartOne()
		{
			int depth = 10914;
			int targetX = 9;
			int targetY = 739;

			List<List<RegionType>> grid = new List<List<RegionType>>();
			List<List<int>> erosionLevels = new List<List<int>>();

			for (int row = 0; row <= targetY; row++)
			{
				grid.Add(new List<RegionType>());
				erosionLevels.Add(new List<int>());
				for (int column = 0; column <= targetX; column++)
				{				
					int geoIndex = 0;

					if (row == 0)
						geoIndex = (column * 16807); //7 ^ 5
					else if (column == 0)
						geoIndex = (row * 48271);
					else if (row == targetY && column == targetX)
						geoIndex = 0;
					else
					{
						geoIndex = (erosionLevels[row - 1][column] * erosionLevels[row][column - 1]);
						if (geoIndex < 0)
							throw new Exception();
					}

					int erosionLevel = (geoIndex + depth) % 20183;

					erosionLevels[row].Add(erosionLevel);

					switch(erosionLevel % 3)
					{
						case 0:
							grid[row].Add(RegionType.Rocky);
							break;
						case 1:
							grid[row].Add(RegionType.Wet);
							break;
						case 2:
							grid[row].Add(RegionType.Narrow);
							break;
						default:
							throw new Exception();
					}

					//Console.Write(grid[row][column]);
				}
				//Console.WriteLine(row);
			}

			for (int row = 0; row <= targetY; row++)
			{
				for (int column = 0; column <= targetX; column++)
				{
					char currentChar = '0';

					switch(grid[row][column])
					{
						case RegionType.Narrow:
							currentChar = '|';
							break;
						case RegionType.Rocky:
							currentChar = '.';
							break;
						case RegionType.Wet:
							currentChar = '=';
							break;
					}

					Console.Write(currentChar);
				}
				Console.WriteLine();
			}
			int riskLevel = grid.Select(x => x.Select(y => (int)y).Sum()).Sum();

			Console.WriteLine(riskLevel);

		}

		enum Equipment
		{
			Neither,
			Torch,
			Gear
		}
		public static void PartTwo()
		{
			int depth = 10914;
			int targetX = 9;
			int targetY = 739;

			List<List<RegionType>> grid = new List<List<RegionType>>();
			List<List<int>> erosionLevels = new List<List<int>>();

			for (int row = 0; row <= targetY + 50; row++)
			{
				grid.Add(new List<RegionType>());
				erosionLevels.Add(new List<int>());
				for (int column = 0; column <= targetX + 50; column++)
				{				
					int geoIndex = 0;

					if (row == 0)
						geoIndex = (column * 16807); //7 ^ 5
					else if (column == 0)
						geoIndex = (row * 48271);
					else if (row == targetY && column == targetX)
						geoIndex = 0;
					else
					{
						geoIndex = (erosionLevels[row - 1][column] * erosionLevels[row][column - 1]);
						if (geoIndex < 0)
							throw new Exception();
					}

					int erosionLevel = (geoIndex + depth) % 20183;

					erosionLevels[row].Add(erosionLevel);

					switch(erosionLevel % 3)
					{
						case 0:
							grid[row].Add(RegionType.Rocky);
							break;
						case 1:
							grid[row].Add(RegionType.Wet);
							break;
						case 2:
							grid[row].Add(RegionType.Narrow);
							break;
						default:
							throw new Exception();
					}

					//Console.Write(grid[row][column]);
				}
				//Console.WriteLine(row);
			}

			Dictionary<Tuple<int, int, Equipment>, int> graph = new Dictionary<Tuple<int, int, Equipment>, int>();

			Queue<Tuple<int, int, Equipment, int>> queue = new Queue<Tuple<int, int, Equipment, int>>();

			queue.Enqueue(new Tuple<int, int, Equipment, int>(0, 0, Equipment.Torch, 0));

			while (queue.Count != 0)
			{
				Tuple<int, int, Equipment, int> value = queue.Dequeue();

				int row = value.Item1;
				int column = value.Item2;
				Equipment equipment = value.Item3;
				int time = value.Item4;

				if (row == targetY && column == targetX)
				{
					if (equipment != Equipment.Torch)
					{
						equipment = Equipment.Torch;
						time += 7;
					}
				}

				Tuple<int, int, Equipment> key = new Tuple<int, int, Equipment>(row, column, equipment);

				if (graph.ContainsKey(key))
				{
					if (time >= graph[key])
					{
						continue;
					}
					else
					{
						graph[key] = time;
					}
				}
				else
				{
					graph.Add(key, time);
				}

				if (row == targetY && column == targetX)
				{
					continue;
				}

				if (row > 0)
				{
					QueueForSquare(grid, queue, grid[row][column], equipment, row - 1, column, time);
				}

				if (column > 0)
				{
					QueueForSquare(grid, queue, grid[row][column], equipment, row, column - 1, time);
				}

				if (row < grid.Count - 1)
				{
					QueueForSquare(grid, queue, grid[row][column], equipment, row + 1, column, time);
				}

				if (column < grid[row].Count - 1)
				{
					QueueForSquare(grid, queue, grid[row][column], equipment, row, column + 1, time);
				}
			}
			Console.WriteLine(graph[new Tuple<int, int, Equipment>(targetY, targetX, Equipment.Torch)]);
		}
		
		static void QueueForSquare(List<List<RegionType>> grid, Queue<Tuple<int, int, Equipment, int>> queue, RegionType currentRegion, Equipment currentEquip, int row, int column, int currentTime)
		{
			RegionType region = grid[row][column];

			switch(region)
			{
				case RegionType.Rocky:
					switch(currentEquip)
					{
						case Equipment.Gear:
						case Equipment.Torch:
							queue.Enqueue(new Tuple<int, int, Equipment, int>(row, column, currentEquip, currentTime + 1));
							break;
						case Equipment.Neither:
							if (currentRegion == RegionType.Wet)
								queue.Enqueue(new Tuple<int, int, Equipment, int>(row, column, Equipment.Gear, currentTime + 8));
							if (currentRegion == RegionType.Narrow)
								queue.Enqueue(new Tuple<int, int, Equipment, int>(row, column, Equipment.Torch, currentTime + 8));
							break;
					}
					break;
				case RegionType.Narrow:
					switch(currentEquip)
					{
						case Equipment.Neither:
						case Equipment.Torch:
							queue.Enqueue(new Tuple<int, int, Equipment, int>(row, column, currentEquip, currentTime + 1));
							break;
						case Equipment.Gear:
							if (currentRegion == RegionType.Wet)
								queue.Enqueue(new Tuple<int, int, Equipment, int>(row, column, Equipment.Neither, currentTime + 8));
							if (currentRegion == RegionType.Rocky)
								queue.Enqueue(new Tuple<int, int, Equipment, int>(row, column, Equipment.Torch, currentTime + 8));
							break;
					}
					break;
				case RegionType.Wet:
					switch(currentEquip)
					{
						case Equipment.Gear:
						case Equipment.Neither:
							queue.Enqueue(new Tuple<int, int, Equipment, int>(row, column, currentEquip, currentTime + 1));
							break;
						case Equipment.Torch:
							if (currentRegion == RegionType.Rocky)
								queue.Enqueue(new Tuple<int, int, Equipment, int>(row, column, Equipment.Gear, currentTime + 8));
							if (currentRegion == RegionType.Narrow)
								queue.Enqueue(new Tuple<int, int, Equipment, int>(row, column, Equipment.Neither, currentTime + 8));
							break;
					}
					break;
			}
		}
	}
}