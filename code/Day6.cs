using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
	class DaySix
	{
		class PointDist
		{
			public int x { get; set; }
			public int y { get; set; }
			public int prevDist { get; set; }
			public PointDist(int xParam, int yParam, int prevDistParam)
			{
				x = xParam;
				y = yParam;
				prevDist = prevDistParam;
			}
		}

		private static List<int> CalculateDistances(List<Tuple<int, int>> points, Tuple<int, int> point)
		{
			return new List<int>(points.Select(x => Math.Abs(x.Item1 - point.Item1) + Math.Abs(x.Item2 - point.Item2)));
		}

		private static List<Tuple<int, int>> GetPointsList()
		{
			return new List<Tuple<int, int>>(Utils.GetLinesFromFile("input/Day6Input.txt").Select(x =>
			{ 
				string[] split = x.Split(", ");
				return new Tuple<int, int>(Convert.ToInt32(split[0]), Convert.ToInt32(split[1]));
			}));
		}

		public static void PartOne()
		{
			List<Tuple<int, int>> points = GetPointsList();

			int maximumSize = points.Select(x => CalculatePart1Area(points, x, points.IndexOf(x))).Max();
			Console.WriteLine("Maximum non-infinite size is {0}", maximumSize);
		}

		private static int CalculatePart1Area(List<Tuple<int, int>> points, Tuple<int, int> startingPoint, int index)
		{
			HashSet<Tuple<int, int>> coordMap = new HashSet<Tuple<int, int>>();
			Queue<PointDist> pointQueue = new Queue<PointDist>();

			pointQueue.Enqueue(new PointDist(startingPoint.Item1, startingPoint.Item2, CalculateDistances(points, startingPoint).Sum()));

			while (pointQueue.Count > 0)
			{
				PointDist currentPointDist = pointQueue.Dequeue();

				Tuple<int, int> point = new Tuple<int, int>(currentPointDist.x, currentPointDist.y);

				if (coordMap.Contains(point))
				{
					continue;
				}

				List<int> distances = CalculateDistances(points, point);
				int totalDistance = distances.Sum();
				if (totalDistance - currentPointDist.prevDist == distances.Count)
				{
					return -1;
				}

				int minimum = distances.Min();

				if (distances[index] == minimum && distances.Where(x => x.Equals(minimum)).Count() == 1)
				{
					coordMap.Add(point);
					
					pointQueue.Enqueue(new PointDist(currentPointDist.x + 1, currentPointDist.y, totalDistance));
					pointQueue.Enqueue(new PointDist(currentPointDist.x - 1, currentPointDist.y, totalDistance));
					pointQueue.Enqueue(new PointDist(currentPointDist.x, currentPointDist.y - 1, totalDistance));
					pointQueue.Enqueue(new PointDist(currentPointDist.x, currentPointDist.y + 1, totalDistance));
				}
			}

			return coordMap.Count;
		}
		public static void PartTwo()
		{
			List<Tuple<int, int>> points = GetPointsList();

			Tuple<int, int> startingPoint = new Tuple<int, int>((int)points.Select(x => x.Item1).Average(), (int)points.Select(x => x.Item2).Average());

			Console.WriteLine("Maximum size is {0}", CalculatePart2Area(points, startingPoint));
		}

		private static int CalculatePart2Area(List<Tuple<int, int>> points, Tuple<int, int> startingPoint)
		{
			HashSet<Tuple<int, int>> coordMap = new HashSet<Tuple<int, int>>();
			Queue<Tuple<int, int>> pointQueue = new Queue<Tuple<int, int>>();

			pointQueue.Enqueue(startingPoint);

			while (pointQueue.Count > 0)
			{
				Tuple<int, int> point = pointQueue.Dequeue();

				if (coordMap.Contains(point))
					continue;

				if (CalculateDistances(points, point).Sum() >= 10000)
					continue;

				coordMap.Add(point);
				
				pointQueue.Enqueue(new Tuple<int, int>(point.Item1 - 1, point.Item2));
				pointQueue.Enqueue(new Tuple<int, int>(point.Item1 + 1, point.Item2));
				pointQueue.Enqueue(new Tuple<int, int>(point.Item1, point.Item2 - 1));
				pointQueue.Enqueue(new Tuple<int, int>(point.Item1, point.Item2 + 1));
			}

			return coordMap.Count;
		}
	}
}