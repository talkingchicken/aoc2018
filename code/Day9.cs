using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
	class Marble
	{
		public Marble previous { get; set; }
		public Marble next {get;set;}
		public long value { get; set; }

		public Marble(long valueParam)
		{
			value = valueParam;
		}

		public Marble Clockwise(int value)
		{
			Marble currentMarble = this;

			for (int i= 0; i < value; i++)
			{
				currentMarble = currentMarble.next;
			}

			return currentMarble;
		}

		public Marble CounterClockwise(int value)
		{
			Marble currentMarble = this;

			for (int i= 0; i < value; i++)
			{
				currentMarble = currentMarble.previous;
			}

			return currentMarble;
		}

		public void Remove()
		{
			this.previous.next = this.next;
			this.next.previous = this.previous;
		}
	}

	class DayNine
	{
		public static void PrintMaxScore(int players, long marbles)
		{
			Marble currentMarble = new Marble(0);

			int currentPlayer = 0;

			currentMarble.previous = currentMarble;
			currentMarble.next = currentMarble;

			List<long> scores = new List<long>(new long[players]);

			for (long i = 1; i <= marbles; i++)
			{
				if (i % 23 == 0)
				{
					scores[currentPlayer] += i;
					Marble otherRemovedMarble = currentMarble.CounterClockwise(7);
					scores[currentPlayer] += otherRemovedMarble.value;

					currentMarble = otherRemovedMarble.next;

					otherRemovedMarble.Remove();
				}
				else
				{
					Marble nextMarble = currentMarble.next;

					Marble newMarble = new Marble(i);
					newMarble.previous = nextMarble;
					newMarble.next = nextMarble.next;
					newMarble.next.previous = newMarble;
					nextMarble.next = newMarble;

					currentMarble = newMarble;
				}

				currentPlayer = (currentPlayer + 1) % players;
			}

			Console.WriteLine("winning score is {0}", scores.Max());
		}
		public static void PartOne()
		{
			PrintMaxScore(435, 71184);	
		}

		public static void PartTwo()
		{
			PrintMaxScore(435, 7118400);
		}
	}
}