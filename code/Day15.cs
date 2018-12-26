using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
	class DayFifteen
	{
		enum Direction
		{
			Up = 0,
			Left = 1,
			Right = 2,
			Down = 3
		}
		public class Unit : IComparable
		{
			public char faction {get;}
			public int attackPower { get; }
			public int health {get; set; }
			public int row {get; set;}
			public int column {get; set;}
			public bool alive {get; set;}
			public bool moved {get; set;}

			public Unit(char factionParam, int rowParam, int columnParam, int attackParam)
			{
				attackPower = attackParam;
				health = 200;
				faction = factionParam;
				row = rowParam;
				column = columnParam;
				alive = true;
				moved = false;
			}

			public List<Tuple<int, int>> Move(char[,] map, IEnumerable<Unit> livingUnits)
			{
				List<Tuple<int, int>> targets = new List<Tuple<int, int>>();

				foreach(Unit enemyUnit in livingUnits.Where(x => x.faction != faction))
				{
					if (!enemyUnit.alive)
						continue;

					if (Math.Abs(enemyUnit.row - row) + Math.Abs(enemyUnit.column - column) == 1)
						return new List<Tuple<int, int>>{new Tuple<int, int>(0, 0)};

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

				Queue<Tuple<int, int, int, Direction>> queue = new Queue<Tuple<int, int, int, Direction>>();
				Dictionary<Tuple<int, int>, Tuple<int, Direction>> paths = new Dictionary<Tuple<int, int>, Tuple<int, Direction>>();
				paths.Add(new Tuple<int,int>(row, column), new Tuple<int, Direction>(0, Direction.Up));
				queue.Enqueue(new Tuple<int, int, int, Direction>(row - 1, column, 1, Direction.Up));
				queue.Enqueue(new Tuple<int, int, int, Direction>(row + 1, column, 1, Direction.Down));
				queue.Enqueue(new Tuple<int, int, int, Direction>(row, column - 1, 1, Direction.Left));
				queue.Enqueue(new Tuple<int, int, int, Direction>(row, column + 1, 1, Direction.Right));

				while (queue.Count > 0)
				{
					Tuple<int, int, int, Direction> queueItem = queue.Dequeue();

					Tuple<int, int> coords = new Tuple<int, int>(queueItem.Item1, queueItem.Item2);
					int distance = queueItem.Item3;
					Direction direction = queueItem.Item4;

					if (map[coords.Item1, coords.Item2] != '.')
						continue;

					if (paths.ContainsKey(coords))
					{
						if (paths[coords].Item1 < distance)
						{
							continue;
						}
						else if (paths[coords].Item1 == distance)
						{
							if (paths[coords].Item2 <= direction)
							{
								continue;
							}
						}

						paths[coords] = new Tuple<int, Direction>(distance, direction);
					}
					else
					{
						paths.Add(coords, new Tuple<int, Direction>(distance, direction));
					}

					queue.Enqueue(new Tuple<int, int, int, Direction>(coords.Item1 - 1, coords.Item2, distance + 1, direction));
					queue.Enqueue(new Tuple<int, int, int, Direction>(coords.Item1 + 1, coords.Item2, distance + 1, direction));
					queue.Enqueue(new Tuple<int, int, int, Direction>(coords.Item1, coords.Item2 - 1, distance + 1, direction));
					queue.Enqueue(new Tuple<int, int, int, Direction>(coords.Item1, coords.Item2 + 1, distance + 1, direction));
				}

				targets.Sort();

				Tuple<int, int> closestTarget = null;

				int closestDistance = int.MaxValue;
				Direction closestDirection = Direction.Up;

				foreach (Tuple<int, int> target in targets)
				{
					if (!paths.ContainsKey(target))
					{
						continue;
					}

					Tuple<int, Direction> path = paths[target];
					if (path.Item1 < closestDistance)
					{
						closestDistance = path.Item1;
						closestDirection = path.Item2;
						closestTarget = target;
					}
					else if (path.Item1 == closestDistance && path.Item2 < closestDirection)
					{
						closestDirection = path.Item2;
						closestTarget = target;
					}
				}

				if (closestTarget != null)
				{
					map[row, column] = '.';

					switch (closestDirection)
					{
						case Direction.Up:
							map[row - 1, column] = faction;
							row--;
							break;
						case Direction.Down:
							map[row + 1, column] = faction;
							row++;
							break;
						case Direction.Left:
							map[row, column - 1] = faction;
							column--;
							break;
						case Direction.Right:
							map[row, column + 1] = faction;
							column++;
							break;
					}
				}

				return targets;
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
			Console.WriteLine(SimulateCombat(0).Item2);
		}

		public static void PrintGrid(char[,] map, List<Unit> units)
		{
			for (int i = 0; i < 32; i++)
			{
				for (int j = 0; j < 32; j++)
				{
					Console.Write(map[i, j]);
				}
				Console.WriteLine();
			}

			foreach (Unit unit in units.Where(x=>x.alive))
			{
				Console.WriteLine("{0}, {1}, {2}", unit.row, unit.column, unit.health);
			}
		}

		public static Tuple<int, int> SimulateCombat(int boost)
		{
			char[,] map = new char[32,32];

			List<string> lines = Utils.GetLinesFromFile("input/Day15Input.txt");

			List<Unit> units = new List<Unit>();

			for (int i = 0; i < lines.Count; i++)
			{
				string line = lines[i];
				for(int j = 0; j < line.Length; j++)
				{
					if (line[j] == 'G' || line[j] == 'E')
					{
						int attack = 3;
						if (line[j] == 'E')
							attack += boost;

						units.Add(new Unit(line[j], i, j, attack));
					}

					map[i, j] = line[j];
				}
			}

			int rounds = 0;
			while(units.Where(x => x.faction == 'G' && x.alive).Count() > 0 && units.Where(x => x.faction == 'E' && x.alive).Count() > 0)
			{
				units.Sort();
				foreach (Unit unit in units)
				{
					unit.moved = false;
				}

				List<Unit> livingUnits = new List<Unit>(units.Where(x => x.alive));

				foreach (Unit currentUnit in livingUnits)
				{
					currentUnit.moved = true;

					if (!currentUnit.alive)
						continue;

					if (units.Where(x => x.faction != currentUnit.faction && x.alive).Count() == 0)
					{
						break;
					}

					// step 1: calculate ranges of enemy units
					currentUnit.Move(map, livingUnits);

					Unit enemyUnit = null;

					// step 3: if in range, attack
					if (map[currentUnit.row + 1, currentUnit.column] == (currentUnit.faction == 'E' ? 'G' : 'E'))
					{
						enemyUnit = livingUnits.Where(x => x.alive && x.row == currentUnit.row + 1 && x.column == currentUnit.column).ElementAt(0);
					}
					
					if (map[currentUnit.row, currentUnit.column + 1] == (currentUnit.faction == 'E' ? 'G' : 'E'))
					{
						Unit possibleUnit = livingUnits.Where(x => x.alive && x.row == currentUnit.row && x.column == currentUnit.column + 1).ElementAt(0);
						if (enemyUnit == null || possibleUnit.health <= enemyUnit.health)
						{
							enemyUnit = possibleUnit;
						}
					}
					
					if (map[currentUnit.row, currentUnit.column - 1] == (currentUnit.faction == 'E' ? 'G' : 'E'))
					{
						Unit possibleUnit = livingUnits.Where(x => x.alive && x.row == currentUnit.row && x.column == currentUnit.column - 1).ElementAt(0);
						if (enemyUnit == null || possibleUnit.health <= enemyUnit.health)
						{
							enemyUnit = possibleUnit;
						}
					}
					
					if (map[currentUnit.row - 1, currentUnit.column] == (currentUnit.faction == 'E' ? 'G' : 'E'))
					{
						Unit possibleUnit = livingUnits.Where(x => x.alive && x.row == currentUnit.row -1 && x.column == currentUnit.column).ElementAt(0);
						if (enemyUnit == null || possibleUnit.health <= enemyUnit.health)
						{
							enemyUnit = possibleUnit;
						}
					}

					if (enemyUnit != null)
					{
						enemyUnit.health -= currentUnit.attackPower;
						if (enemyUnit.health <= 0)
						{
							enemyUnit.alive = false;
							map[enemyUnit.row, enemyUnit.column] = '.';
						}
					}
				}

				if (units.Where(x => x.alive).Count() == units.Where(x => x.alive && x.moved).Count())
					rounds++;
			}

			int remainingUnits = units.Where(x => x.alive).Count();
			int outcome = units.Where(x => x.alive).Select(y => y.health).Sum() * rounds;

			if (units.Where(x => x.alive).ElementAt(0).faction == 'G')
			{
				remainingUnits *= -1;
				outcome *= -1;
			}
			
			return new Tuple<int, int>(remainingUnits, outcome);
		}
		public static void PartTwo()
		{
			int boost = 0;
			Tuple<int, int> result;
			do
			{
				boost++;
				Console.WriteLine(boost);
				result = SimulateCombat(boost);
			} while(result.Item1 < 10);

			Console.WriteLine(result.Item2);
		}
	}
}