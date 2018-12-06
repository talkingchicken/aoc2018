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

		private static List<Tuple<int, int>> GetPointsList()
		{
			List<Tuple<int, int>> points = new List<Tuple<int, int>>();
			string line;
			StreamReader file = new StreamReader("input/Day6Input.txt");
			
			while ((line = file.ReadLine()) != null)
			{
				string[] pointStr = line.Split(", ");
				Tuple<int, int> point = new Tuple<int, int>(Convert.ToInt32(pointStr[0]), Convert.ToInt32(pointStr[1]));
				points.Add(point);
			}
			
			file.Close();

			return points;
		}

		public static void PartOne()
		{
			List<Tuple<int, int>> points = GetPointsList();

			int maximumSize = -1;

			for (int i = 0; i < points.Count; i++)
			{
				HashSet<Tuple<int, int>> coordMap = new HashSet<Tuple<int, int>>();

				int size = CalculateArea(points, points[i], i);
				maximumSize = Math.Max(maximumSize, size);
			}

			Console.WriteLine("Maximum non-infinite size is {0}", maximumSize);
		}

		public static void PartTwo()
		{
			List<Tuple<int, int>> points = GetPointsList();

			Tuple<int, int> startingPoint = new Tuple<int, int>((int)points.Select(x => x.Item1).Average(), (int)points.Select(x => x.Item2).Average());

			Console.WriteLine("Maximum size is {0}", CalculateNearbyArea(points, startingPoint.Item1, startingPoint.Item2));
		}

		private static int CalculateNearbyArea(List<Tuple<int, int>> points, int x, int y)
		{
			HashSet<Tuple<int, int>> coordMap = new HashSet<Tuple<int, int>>();
			Queue<Tuple<int, int>> pointQueue = new Queue<Tuple<int, int>>();
			
			var startPoint = new Tuple<int, int>(x, y);

			pointQueue.Enqueue(startPoint);

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

		private static int CalculateArea(List<Tuple<int, int>> points, Tuple<int, int> startingPoint, int index)
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

		private static List<int> CalculateDistances(List<Tuple<int, int>> points, Tuple<int, int> point)
		{
			return new List<int>(points.Select(x => Math.Abs(x.Item1 - point.Item1) + Math.Abs(x.Item2 - point.Item2)));
		}
	}
}