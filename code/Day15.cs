using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
	class DayFifteen
	{
		public class Unit : IComparable
		{
			public char faction {get;}
			public int attackPower { get; }
			public int health {get; set; }
			public int row {get; set;}
			public int column {get; set;}
			public bool alive {get; set;}

			public Unit(char factionParam, int rowParam, int columnParam)
			{
				attackPower = 3;
				health = 200;
				faction = factionParam;
				row = rowParam;
				column = columnParam;
				alive = true;
			}

			int IComparable.CompareTo(object obj)
			{
				Unit otherUnit = (Unit)(obj);

				if (row != otherUnit.row)
				{
					return row.CompareTo(otherUnit.row);
				}
				else
				{
					return column.CompareTo(otherUnit.column);
				}
			}
		}
		public static void PartOne()
		{
			char[,] map = new char[32,32];

			List<string> lines = Utils.GetLinesFromFile("input/Day15.txt");

			List<Unit> units = new List<Unit>();

			for (int i = 0; i < lines.Count; i++)
			{
				string line = lines[i];
				for(int j = 0; j < line.Length; j++)
				{
					if (line[j] == 'G' || line[j] == 'E')
					{
						units.Add(new Unit(line[j], i, j));
					}

					map[i, j] = line[j];
				}
			}

			int rounds = 0;
			while(units.Where(x => x.faction == 'G' && x.alive).Count() > 0 || units.Where(x => x.faction == 'E' && x.alive).Count() > 0)
			{
				rounds++;
				units.Sort();
				IEnumerable<Unit> livingUnits = units.Where(x => x.alive);

				foreach(Unit currentUnit in livingUnits)
				{
					// step 1: calculate ranges of enemy units
					List<Tuple<int, int>> targets = new List<Tuple<int, int>>();

					foreach(Unit enemyUnit in livingUnits.Where(x => x.faction != currentUnit.faction))
					{
						if(map[enemyUnit.row, enemyUnit.column - 1] == '.')
						{
							targets.Add(new Tuple<int, int>(enemyUnit.row, enemyUnit.column - 1));
						}

						if(map[enemyUnit.row - 1, enemyUnit.column] == '.')
						{
							targets.Add(new Tuple<int, int>(enemyUnit.row - 1, enemyUnit.column));
						}

						if(map[enemyUnit.row + 1, enemyUnit.column] == '.')
						{
							targets.Add(new Tuple<int, int>(enemyUnit.row + 1, enemyUnit.column));
						}

						if(map[enemyUnit.row, enemyUnit.column + 1] == '.')
						{
							targets.Add(new Tuple<int, int>(enemyUnit.row, enemyUnit.column + 1));
						}
					}

					// step 2: find the closest square in range and move towards it

					if (targets.Count != 0)
					{
						targets.Sort();
					}

					// step 3: if in range, attack

				}
			}
		}

		public static void PartTwo()
		{
			
		}
	}
}