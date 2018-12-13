using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
	enum Direction
	{
		Left = 0,
		Up = 1,
		Right = 2,
		Down = 3
	}

	enum TurnState
	{
		Left,
		Straight,
		Right
	}

	class Car : IComparable
	{
		public int id { get; }
		public int x {get; private set;}
		public int y {get; private set;}

		public bool dead { get; set; }
		public Direction direction {get; private set;}

		public TurnState turnState {get; private set;}

		public Car(int idParam, int xParam, int yParam, Direction directionParam)
		{
			id = idParam;
			x = xParam;
			y = yParam;
			direction = directionParam;
			turnState = TurnState.Left;
			dead = false;
		}
		void MoveStraight()
		{
			switch (direction)
			{
				case Direction.Left:
					x--;
					break;
				case Direction.Right:
					x++;
					break;
				case Direction.Up:
					y--;
					break;
				case Direction.Down:
					y++;
					break;
			}
		}

		void TurnLeft()
		{
			switch (direction)
			{
				case Direction.Right:
					direction = Direction.Up;
					y--;
					break;
				case Direction.Up:
					direction = Direction.Left;
					x--;
					break;
				case Direction.Down:
					direction = Direction.Right;
					x++;
					break;
				case Direction.Left:
					direction = Direction.Down;
					y++;
					break;
			}
		}

		void TurnRight()
		{
			switch (direction)
			{
				case Direction.Right:
					direction = Direction.Down;
					y++;
					break;
				case Direction.Up:
					direction = Direction.Right;
					x++;
					break;
				case Direction.Down:
					direction = Direction.Left;
					x--;
					break;
				case Direction.Left:
					direction = Direction.Up;
					y--;
					break;
			}
		}
		public void Move(char[,] grid)
		{
			switch(grid[y, x])
			{
				case '-':
				case '|':
					MoveStraight();
					break;
				case '\\':
					switch (direction)
					{
						case Direction.Right:
							TurnRight();
							break;
						case Direction.Up:
							TurnLeft();
							break;
						case Direction.Down:
							TurnLeft();
							break;
						case Direction.Left:
							TurnRight();
							break;
					}
					break;
				case '/':
					switch (direction)
					{
						case Direction.Right:
							TurnLeft();
							break;
						case Direction.Up:
							TurnRight();
							break;
						case Direction.Down:
							TurnRight();
							break;
						case Direction.Left:
							TurnLeft();
							break;
					}
					break;
				case '+':
					switch(turnState)
					{
						case TurnState.Left:
							TurnLeft();
							turnState = TurnState.Straight;
							break;
						case TurnState.Straight:
							MoveStraight();
							turnState = TurnState.Right;
							break;
						case TurnState.Right:
							TurnRight();
							turnState = TurnState.Left;
							break;
					}
					break;
				default:
					throw new Exception();
			}
		}

		int IComparable.CompareTo(object obj)
		{
			Car car = (Car)obj;
			if (y != car.y)
			{
				return y.CompareTo(car.y);
			}
			else
			{
				return x.CompareTo(car.x);
			}
		}
	}
	class DayThirteen
	{
		public static void PartOne()
		{
			List<Car> cars = new List<Car>();

			char[,] grid = new char[150, 150];

			List<string> lines = Utils.GetLinesFromFile("input/Day13Input.txt");

			int currentId = 0;

			for(int y = 0; y < lines.Count; y++)
			{
				string line = lines[y];
				for(int x = 0; x < line.Length; x++)
				{
					char letter = line[x];
					switch(letter)
					{
						case '^':
							grid[y, x] = '|';
							cars.Add(new Car(currentId++, x, y, Direction.Up));
							break;
						case 'v':
							grid[y, x] = '|';
							cars.Add(new Car(currentId++, x, y, Direction.Down));
							break;
						case '<':
							grid[y, x] = '-';
							cars.Add(new Car(currentId++, x, y, Direction.Left));
							break;
						case '>':
							grid[y, x] = '-';
							cars.Add(new Car(currentId++, x, y, Direction.Right));
							break;
						default:
							grid[y, x] = letter;
							break;
					}
				}
			}

			while (true)
			{
				cars.Sort();
				foreach (Car car in cars)
				{
					car.Move(grid);

					foreach (Car otherCar in cars)
					{
						if (car.x == otherCar.x && car.y == otherCar.y && car.id != otherCar.id)
						{
							Console.WriteLine("Collision at {0},{1}", car.x, car.y);
							return;
						}
					}
				}
			}
		}

		public static void PartTwo()
		{
			List<Car> cars = new List<Car>();

			char[,] grid = new char[150, 150];

			List<string> lines = Utils.GetLinesFromFile("input/Day13Input.txt");

			int currentId = 0;

			for(int y = 0; y < lines.Count; y++)
			{
				string line = lines[y];
				for(int x = 0; x < line.Length; x++)
				{
					char letter = line[x];
					switch(letter)
					{
						case '^':
							grid[y, x] = '|';
							cars.Add(new Car(currentId++, x, y, Direction.Up));
							break;
						case 'v':
							grid[y, x] = '|';
							cars.Add(new Car(currentId++, x, y, Direction.Down));
							break;
						case '<':
							grid[y, x] = '-';
							cars.Add(new Car(currentId++, x, y, Direction.Left));
							break;
						case '>':
							grid[y, x] = '-';
							cars.Add(new Car(currentId++, x, y, Direction.Right));
							break;
						default:
							grid[y, x] = letter;
							break;
					}
				}
			}

			while (true)
			{
				cars.Sort();
				foreach (Car car in cars)
				{
					if (!car.dead)
					{
						car.Move(grid);

						foreach (Car otherCar in cars)
						{
							if (!otherCar.dead && car.x == otherCar.x && car.y == otherCar.y && car.id != otherCar.id)
							{
								car.dead = true;
								otherCar.dead = true;
							}
						}
					}
				}

				IEnumerable<Car> remainingCars = cars.Where(x => !x.dead);
				if (remainingCars.Count() == 1)
				{
					Console.WriteLine("Last remaining car is at {0},{1}", remainingCars.ElementAt(0).x, remainingCars.ElementAt(0).y);
					return;
				}
			}
		}
	}
}