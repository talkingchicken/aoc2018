using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace AdventOfCode
{
	class GuardLog : IComparable
	{
		public DateTime date { get; set; }  
		public string value { get; set; }
		public List<int> sleepTime { get; set; }
		public GuardLog(DateTime dateParam, string valueParam)
		{
			date = dateParam;
			value = valueParam;
		}

		int IComparable.CompareTo(object obj)
		{
			GuardLog g = (GuardLog)obj;
			return date.CompareTo(g.date);
		}
	}

	class DayFour
	{
		private static List<GuardLog> GetLogsFromFile(string filename)
		{
			string line;
			StreamReader file = new StreamReader(filename);
			
			List<GuardLog> logs = new List<GuardLog>();

			while ((line = file.ReadLine()) != null)
			{
				DateTime currentDate = DateTime.ParseExact(line.Substring(0, 18), "[yyyy-MM-dd HH:mm]", CultureInfo.InvariantCulture);
				string logValue = line.Substring(19);

				logs.Add(new GuardLog(currentDate, logValue));
			}
			
			file.Close();

			logs.Sort();
			return logs;
		}
		
		private static Dictionary<int, List<int>> GetGuardSleepRatesFromLogs(List<GuardLog> logs)
		{
			Dictionary<int, List<int>> guards = new Dictionary<int, List<int>>();

			int currentGuardId = 0;
			int startingMinute = 0;
			foreach (GuardLog log in logs)
			{
				if (log.value[0] == 'G')
				{
					string[] words = log.value.Split(" ");
					currentGuardId = Convert.ToInt32(words[1].Substring(1));

					if (!guards.ContainsKey(currentGuardId))
					{
						guards[currentGuardId] = new List<int>(new int[60]);
					}
				}
				else if (log.value[0] == 'f')
				{
					startingMinute = log.date.Hour == 0 ? log.date.Minute : 0;
				}
				else
				{
					for (int i = startingMinute; i < log.date.Minute; i++)
					{
						guards[currentGuardId][i]++;
					}
				}
			}

			return guards;
		}

		public static void PartOne()
		{
			List<GuardLog> logs = GetLogsFromFile("input/Day4Input.txt");
			Dictionary<int, List<int>> guards = GetGuardSleepRatesFromLogs(logs);
			
			int maxId = -1;
			int maxMinutes = 0;
			int maxValue = 0;

			foreach (KeyValuePair<int, List<int>> guard in guards)
			{
				int totalSleepMinutes = guard.Value.Sum();
				if (totalSleepMinutes > maxMinutes)
				{
					maxMinutes = totalSleepMinutes;
					maxId = guard.Key;
					int minuteValue = guard.Value[0];

					for (int i = 1; i < guard.Value.Count; i++)
					{
						if (guard.Value[i] > minuteValue)
						{
							maxValue = i;
							minuteValue = guard.Value[i];
						}
					}
				}
			}

			Console.WriteLine("Guard {0}, minute {1}, final value = {2}", maxId, maxValue, maxId * maxValue);
		}

		public static void PartTwo()
		{
			List<GuardLog> logs = GetLogsFromFile("input/Day4Input.txt");
			Dictionary<int, List<int>> guards = GetGuardSleepRatesFromLogs(logs);

			int maxId = -1;
			int maxValue = -1;
			int minuteValue = -1;

			foreach (KeyValuePair<int, List<int>> guard in guards)
			{
				for (int i = 0; i < 60; i++)
				{
					if (guard.Value[i] > minuteValue)
					{
						maxId = guard.Key;
						maxValue = i;
						minuteValue = guard.Value[i];
					}
				}
			}

			Console.WriteLine("Guard {0}, minute {1}, final value = {2}", maxId, maxValue, maxId * maxValue);
		}
	}
}