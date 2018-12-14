using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
	class DayFourteen
	{
		public static void PartOne()
		{
			int input = 330121;

			int firstIndex = 0;
			int secondIndex = 1;

			List<int> recipes = new List<int>();

			recipes.Add(3);
			recipes.Add(7);

			while(recipes.Count < input + 10)
			{
				int total = recipes[firstIndex] + recipes[secondIndex];

				if (total >= 10)
				{
					recipes.Add(1);
				}

				recipes.Add(total % 10);

				firstIndex = (firstIndex + recipes[firstIndex] + 1) % recipes.Count;
				secondIndex = (secondIndex + recipes[secondIndex] + 1) % recipes.Count;
			}

			for (int i = input; i < input + 10; i++)
			{
				Console.Write(recipes[i]);
			}
			
			Console.WriteLine();
		}

		public static void PartTwo()
		{
			int[] input = {3,3,0,1,2,1};

			int firstIndex = 0;
			int secondIndex = 1;

			List<int> recipes = new List<int>();

			recipes.Add(3);
			recipes.Add(7);

			while(true)
			{
				int total = recipes[firstIndex] + recipes[secondIndex];

				if (total >= 10)
				{
					recipes.Add(1);
				}

				recipes.Add(total % 10);

				firstIndex = (firstIndex + recipes[firstIndex] + 1) % recipes.Count;
				secondIndex = (secondIndex + recipes[secondIndex] + 1) % recipes.Count;

				if (recipes.Count > input.Length)
				{
					int count;
					for(count = 0; count < input.Length; count++)
					{
						if (recipes[recipes.Count - input.Length + count] != input[count])
						{
							break;
						}
					}

					if (count == input.Length)
					{
						Console.WriteLine("Sequence appears after {0} recipes", recipes.Count - input.Length);
						return;
					}

					if (total > 10 && recipes.Count - 1 > input.Length)
					{
						for(count = 0; count < input.Length; count++)
						{
							if (recipes[recipes.Count - 1 - input.Length + count] != input[count])
							{
								break;
							}
						}

						if (count == input.Length)
						{
							Console.WriteLine("Sequence appears after {0} recipes", recipes.Count - input.Length - 1);
							return;
						}
					}
				}
			}
		}
	}
}