using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
	class DayEighteen
	{
		enum AcreState
		{
			Open,
			Wooded,
			Lumberyard
		}

		private static List<List<AcreState>> CalculateNextState(List<List<AcreState>> input)
		{
			List<List<AcreState>> output = new List<List<AcreState>>();

			for (int row = 0; row < input.Count; row++)
			{
				output.Add(new List<AcreState>());
				for (int column = 0; column < input[row].Count; column++)
				{
					switch(input[row][column])
					{
						case AcreState.Open:
						{
							int trees = 0;
							for (int i = Math.Max(0, row - 1); i < Math.Min(input.Count, row + 2); i++)
							{
								for (int j = Math.Max(0, column - 1); j < Math.Min(input[i].Count, column + 2); j++)
								{
									if (i != row || j != column)
									{
										if (input[i][j] == AcreState.Wooded)
										{
											trees++;
										}
									}
								}
							}

							if (trees >= 3)
							{
								output[row].Add(AcreState.Wooded);
							}
							else
							{
								output[row].Add(AcreState.Open);
							}
							break;
						}
						case AcreState.Wooded:
						{
							int lumberyards = 0;
							for (int i = Math.Max(0, row - 1); i < Math.Min(input.Count, row + 2); i++)
							{
								for (int j = Math.Max(0, column - 1); j < Math.Min(input[i].Count, column + 2); j++)
								{
									if (i != row || j != column)
									{
										if (input[i][j] == AcreState.Lumberyard)
										{
											lumberyards++;
										}
									}
								}
							}

							if (lumberyards >= 3)
							{
								output[row].Add(AcreState.Lumberyard);
							}
							else
							{
								output[row].Add(AcreState.Wooded);
							}
							break;
						}
						case AcreState.Lumberyard:
						{
							int lumberyards = 0;
							int trees = 0;
							for (int i = Math.Max(0, row - 1); i < Math.Min(input.Count, row + 2); i++)
							{
								for (int j = Math.Max(0, column - 1); j < Math.Min(input[i].Count, column + 2); j++)
								{
									if (i != row || j != column)
									{
										if (input[i][j] == AcreState.Lumberyard)
										{
											lumberyards++;
										}
										else if (input[i][j] == AcreState.Wooded)
										{
											trees++;
										}
									}
								}
							}

							if (lumberyards >= 1 && trees >= 1)
							{
								output[row].Add(AcreState.Lumberyard);
							}
							else
							{
								output[row].Add(AcreState.Open);
							}
							break;
						}
					}
				}
			}

			return output;
		}

		public static void PartOne()
		{
			List<string> lines = Utils.GetLinesFromFile("input/Day18Input.txt");

			List<List<AcreState>> grid = new List<List<AcreState>>(lines.Select(x => new List<AcreState>(x.Select(y => 
			{
				switch (y)
				{
					case '.':
						return AcreState.Open;
					case '|':
						return AcreState.Wooded;
					case '#':
						return AcreState.Lumberyard;
					default:
						return AcreState.Open;
				}
			}))));

			Dictionary<int, int> values = new Dictionary<int, int>();
			for (int i = 0; i < 1000; i++)
			{
				grid = CalculateNextState(grid);
				
				int totalValue = grid.Select(x => x.Where(y => y == AcreState.Wooded).Count()).Sum() * grid.Select(x => x.Where(y => y == AcreState.Lumberyard).Count()).Sum();

				if (values.ContainsKey(totalValue))
				{
					Console.WriteLine("{1}: We've seen {0} before at iteration {2}", totalValue, i, values[totalValue]);
				}
				else
				{
					values.Add(totalValue, i);
				}
			}

		}

		public static void PartTwo()
		{
			List<string> lines = Utils.GetLinesFromFile("input/Day18Input.txt");

			List<List<AcreState>> grid = new List<List<AcreState>>(lines.Select(x => new List<AcreState>(x.Select(y => 
			{
				switch (y)
				{
					case '.':
						return AcreState.Open;
					case '|':
						return AcreState.Wooded;
					case '#':
						return AcreState.Lumberyard;
					default:
						return AcreState.Open;
				}
			}))));

			Dictionary<int, int> values = new Dictionary<int, int>();
			for (int i = 0; i < 518; i++)
			{
				grid = CalculateNextState(grid);
				
			}

			long remainingIterations = (1000000000 - 518) % (546 - 518);

			for (long j = 0; j < remainingIterations; j++)
			{
				grid = CalculateNextState(grid);
			}

			int totalValue = grid.Select(x => x.Where(y => y == AcreState.Wooded).Count()).Sum() * grid.Select(x => x.Where(y => y == AcreState.Lumberyard).Count()).Sum();

			Console.WriteLine("Total value is {0}", totalValue);
		}
	}
}