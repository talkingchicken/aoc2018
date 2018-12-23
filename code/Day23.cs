using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
	class DayTwentyThree
	{
		class Nanobot
		{
			public int x {get;set;}
			public int y {get;set;}
			public int z {get;set;}
			public int radius {get;set;}
			public Nanobot(int xParam, int yParam, int zParam, int radiusParam)
			{
				x = xParam;
				y = yParam;
				z = zParam;
				radius = radiusParam;
			}
		}
		public static void PartOne()
		{
			List<string> lines = Utils.GetLinesFromFile("input/Day23Input.txt");

			IEnumerable<Nanobot> nanobots = lines.Select(x =>
			{
				string firstPart = x.Substring(5);

				string[] splitOne = firstPart.Split(">");
				string[] numbers = splitOne[0].Split(",");

				string radiusString = splitOne[1].Split("=")[1];

				return new Nanobot(Convert.ToInt32(numbers[0]), Convert.ToInt32(numbers[1]), Convert.ToInt32(numbers[2]), Convert.ToInt32(radiusString));
			});

			int maxRadius = nanobots.Select(x=> x.radius).Max();
			Nanobot maxBot = nanobots.Where(x => x.radius == maxRadius).ElementAt(0);

			int botsInRange = nanobots.Where(x => 
			{
				int xDiff = Math.Abs(x.x - maxBot.x);
				int yDiff = Math.Abs(x.y - maxBot.y);
				int zDiff = Math.Abs(x.z - maxBot.z);

				return (xDiff + yDiff + zDiff) <= maxBot.radius;
			}).Count();

			Console.WriteLine(botsInRange);
		}

		public static void PartTwo()
		{
			List<string> lines = Utils.GetLinesFromFile("input/Day23Input.txt");

			List<Nanobot> nanobots = new List<Nanobot>(lines.Select(x =>
			{
				string firstPart = x.Substring(5);

				string[] splitOne = firstPart.Split(">");
				string[] numbers = splitOne[0].Split(",");

				string radiusString = splitOne[1].Split("=")[1];

				return new Nanobot(Convert.ToInt32(numbers[0]), Convert.ToInt32(numbers[1]), Convert.ToInt32(numbers[2]), Convert.ToInt32(radiusString));
			}));

			IEnumerable<int> xRange = nanobots.Select(x => x.x);
			IEnumerable<int> yRange = nanobots.Select(x => x.y);
			IEnumerable<int> zRange = nanobots.Select(x => x.z);
			
			int xAverage = (int)xRange.Average();
			int yAverage = (int)yRange.Average();
			int zAverage = (int)zRange.Average();

			int maxRadius = nanobots.Select(x => x.radius).Max();
			int xMin = xRange.Min();// - maxRadius;
			int yMin = yRange.Min();// - maxRadius;
			int zMin = zRange.Min();// - maxRadius;

			int xMax = xRange.Max();// + maxRadius;
			int yMax = yRange.Max();// + maxRadius;
			int zMax = zRange.Max();// + maxRadius;
		
			int xInterval = (xMax - xMin) / 5;
			int yInterval = (yMax - yMin) / 5;
			int zInterval = (zMax - zMin) / 5;

			int currentMaxBots = -100;
			HashSet<Tuple<int, int, int>> maximumPoints = FindMaxValues(nanobots, xMin, xMax, yMin, yMax, zMin, zMax, currentMaxBots);
			
			currentMaxBots = maximumPoints.Select(x => findBotsInDistance(nanobots, x.Item1, x.Item2, x.Item3)).Max();
			
			HashSet<Tuple<int, int, int>> lastLoop = maximumPoints;
			while (xInterval > 0 || yInterval > 0 || zInterval > 0)
			{
				HashSet<Tuple<int, int, int>> tempMax = new HashSet<Tuple<int, int, int>>();
				foreach (Tuple<int, int, int> point in lastLoop)
				{
					HashSet<Tuple<int, int, int>> tempTempList = FindMaxValues(
						nanobots, 
						point.Item1 - xInterval, 
						point.Item1 + xInterval, 
						point.Item2 - yInterval, 
						point.Item2 + yInterval, 
						point.Item3 - zInterval, 
						point.Item3 + zInterval,
						currentMaxBots);
					if (tempTempList.Count > 0)
					{
						tempMax.UnionWith(tempTempList);
					
						currentMaxBots = Math.Max(currentMaxBots, tempTempList.Select(x => findBotsInDistance(nanobots, x.Item1, x.Item2, x.Item3)).Max());
					}
				}

				maximumPoints.UnionWith(tempMax.Where(x => findBotsInDistance(nanobots, x.Item1, x.Item2, x.Item3) == currentMaxBots));
				lastLoop = tempMax;
				xInterval /= 5;
				yInterval /= 5;
				zInterval /= 5;
			}

			int maxBotsInRange = maximumPoints.Select(x => findBotsInDistance(nanobots, x.Item1, x.Item2, x.Item3)).Max();
			IEnumerable<Tuple<int, int, int>> pointsWithMaxBots = maximumPoints.Where(x => findBotsInDistance(nanobots, x.Item1, x.Item2, x.Item3) == maxBotsInRange);
			
			int maxDistance = pointsWithMaxBots.Select(x => Math.Abs(x.Item1) + Math.Abs(x.Item2) + Math.Abs(x.Item3)).Min();

			Console.WriteLine(maxDistance);
		}

		static HashSet<Tuple<int, int, int>> FindMaxValues(
			IEnumerable<Nanobot> nanobots,
			int xMin,
			int xMax,
			int yMin,
			int yMax,
			int zMin,
			int zMax,
			int currentMaxBots
		)
		{
			HashSet<Tuple<int, int, int>> returnValue = new HashSet<Tuple<int, int, int>>();
			int maxValue = currentMaxBots;

			for (int x = xMin; x <= xMax; x += Math.Max((xMax - xMin) / 50, 1))
			{
				for (int y  = yMin; y <= yMax; y += Math.Max((yMax - yMin) / 50, 1))
				{
					for (int z = zMin; z <= zMax; z += Math.Max((zMax - zMin) / 50, 1))
					{
						int bots = findBotsInDistance(nanobots, x, y, z);

						if (bots > maxValue)
						{
							returnValue = new HashSet<Tuple<int, int, int>>();
							returnValue.Add(new Tuple<int, int, int>(x, y, z));
							maxValue = bots;
						}
						else if (bots >= maxValue)
						{
							returnValue.Add(new Tuple<int, int, int>(x, y, z));
						}
					}
				}
			}

			return returnValue;
		}

		static int findBotsInDistance(IEnumerable<Nanobot> nanobots, int x, int y, int z)
		{
			return nanobots.Where(a => 
			{
				int xDiff = Math.Abs(a.x - x);
				int yDiff = Math.Abs(a.y - y);
				int zDiff = Math.Abs(a.z - z);

				return (xDiff + yDiff + zDiff) <= a.radius;
			}).Count();
		}
	}
}