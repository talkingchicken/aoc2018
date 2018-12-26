using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
	class DayTwentyFive
	{
		class ConstellationPart
		{
			public int x {get;}
			public int y {get;}
			public int z {get;}
			public int t {get;}
			public List<ConstellationPart> connections {get;set;}
			public ConstellationPart(int xParam, int yParam, int zParam, int tParam)
			{
				x = xParam;
				y = yParam;
				z = zParam;
				t = tParam;
				connections = new List<ConstellationPart>();
			}
			public int CalcDistance(ConstellationPart otherPoint)
			{
				return Math.Abs(otherPoint.x - x) + Math.Abs(otherPoint.y - y) + Math.Abs(otherPoint.z - z) + Math.Abs(otherPoint.t - t);
			}
		}
		public static void PartOne()
		{
			List<string> lines = Utils.GetLinesFromFile("input/Day25Input.txt");

			List<ConstellationPart> points = new List<ConstellationPart>(lines.Select(x =>
			{
				string[] lineSplit = x.Split(",");

				return new ConstellationPart(
					Convert.ToInt32(lineSplit[0]),
					Convert.ToInt32(lineSplit[1]),
					Convert.ToInt32(lineSplit[2]),
					Convert.ToInt32(lineSplit[3]));
			}));

			foreach (ConstellationPart firstPoint in points)
			{
				foreach (ConstellationPart secondPoint in points)
				{
					if (secondPoint == firstPoint)
					{
						continue;
					}

					if (firstPoint.CalcDistance(secondPoint) <= 3)
					{
						firstPoint.connections.Add(secondPoint);
						secondPoint.connections.Add(firstPoint);
					}
				}
			}

			HashSet<ConstellationPart> visited = new HashSet<ConstellationPart>();
			int constellations = 0;
			foreach (ConstellationPart point in points)
			{
				if (visited.Contains(point))
					continue;
				
				constellations++;
				Visit(visited, point);
			}

			Console.WriteLine(constellations);
		}

		private static void Visit(HashSet<ConstellationPart> visited, ConstellationPart point)
		{
			if (visited.Contains(point))
				return;

			visited.Add(point);

			foreach (ConstellationPart connection in point.connections)
			{
				Visit(visited, connection);
			}
		}

		public static void PartTwo()
		{
			Console.WriteLine("You win!");
		}
	}
}