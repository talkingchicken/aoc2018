using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
	class DaySix
	{
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

				if (CalculateArea(coordMap, points, points[i], i, CalculateDistances(points, points[i]).Sum()))
				{
					int size = coordMap.Count;
					maximumSize = Math.Max(maximumSize, size);
				}
			}

			Console.WriteLine("Maximum non-infinite size is {0}", maximumSize);
		}

		public static void PartTwo()
		{
			List<Tuple<int, int>> points = GetPointsList();

			Tuple<int, int> startingPoint = new Tuple<int, int>((int)points.Select(x => x.Item1).Average(), (int)points.Select(x => x.Item2).Average());
			HashSet<Tuple<int, int>> coordMap = new HashSet<Tuple<int, int>>();

			CalculateNearbyArea(coordMap, points, startingPoint.Item1, startingPoint.Item2);

			Console.WriteLine("Maximum size is {0}", coordMap.Count);
		}

		private static void CalculateNearbyArea(HashSet<Tuple<int, int>> coordMap, List<Tuple<int, int>> points, int x, int y)
		{
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
		}

		private static bool CalculateArea(HashSet<Tuple<int, int>> coordMap, List<Tuple<int, int>> points, Tuple<int, int> currentPoint, int index, int previousTotalDistance)
		{
			if (coordMap.Contains(currentPoint))
			{
				return true;
			}

			List<int> distances = CalculateDistances(points, currentPoint);
			int totalDistance = distances.Sum();
			if (totalDistance - previousTotalDistance == distances.Count)
			{
				return false;
			}

			int minimum = distances.Min();

			if (distances[index] == minimum && distances.Where(x => x.Equals(minimum)).Count() == 1)
			{
				coordMap.Add(currentPoint);
				return CalculateArea(coordMap, points, new Tuple<int, int>(currentPoint.Item1 - 1, currentPoint.Item2), index, totalDistance)
					&& CalculateArea(coordMap, points, new Tuple<int, int>(currentPoint.Item1 + 1, currentPoint.Item2), index, totalDistance)
					&& CalculateArea(coordMap, points, new Tuple<int, int>(currentPoint.Item1, currentPoint.Item2 - 1), index, totalDistance)
					&& CalculateArea(coordMap, points, new Tuple<int, int>(currentPoint.Item1, currentPoint.Item2 + 1), index, totalDistance);
			}
			else
			{
				return true;
			}
		}

		private static List<int> CalculateDistances(List<Tuple<int, int>> points, Tuple<int, int> point)
		{
			List<int> returnValue = new List<int>();

			foreach (Tuple<int, int> currentPoint in points)
			{
				returnValue.Add((Math.Abs(currentPoint.Item1 - point.Item1) + Math.Abs(currentPoint.Item2 - point.Item2)));
			}

			return returnValue;
		}
	}
}