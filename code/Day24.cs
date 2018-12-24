using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
	class DayTwentyFour
	{
		class Target : IComparable
		{
			public int source {get;}
			public int destination {get;}
			public int initiative {get;}

			public Target(
				int sourceParam,
				int destinationParam,
				int initiativeParam
			)
			{
				source = sourceParam;
				destination = destinationParam;
				initiative = initiativeParam;
			}

			int IComparable.CompareTo(object obj)
			{
				return -initiative.CompareTo(((Target)obj).initiative);
			}
		}
		class UnitGroup : IComparable
		{
			public bool immuneSystem {get;}
			public int units {get; set;}
			public int health {get;}
			public List<string> weaknesses {get; set;}

			public List<string> immunities {get; set;}
			public int attack {get;}
			public string damageType {get;}
			public int initiative {get;}

			public int CalcDamage(UnitGroup target)
			{
				int baseDamage = EffectivePower();

				if (target.immunities.Contains(damageType))
				{
					baseDamage = 0;
				}
				else if (target.weaknesses.Contains(damageType))
				{
					baseDamage *= 2;
				}

				return baseDamage;
			}

			public int EffectivePower()
			{
				if (units < 0)
					return 0;

				return units * attack;
			}

			public UnitGroup(
				bool immuneSystemParam,
				int unitsParam,
				int healthParam,
				int attackParam,
				int initiativeParam,
				string damageTypeParam,
				List<string> weaknessesParam,
				List<string> immunitiesParam
			)
			{
				immuneSystem = immuneSystemParam;
				units = unitsParam;
				health = healthParam;
				attack = attackParam;
				initiative = initiativeParam;
				damageType = damageTypeParam;
				weaknesses = weaknessesParam;
				immunities = immunitiesParam;
			}

			int IComparable.CompareTo(Object obj)
			{
				UnitGroup otherGroup = (UnitGroup)(obj);

				if ((units * attack) != (otherGroup.units * otherGroup.attack))
				{
					return -((units * attack).CompareTo(otherGroup.units * otherGroup.attack));
				}
				else
				{
					return -(initiative.CompareTo(otherGroup.initiative));
				}
			}
		}
		public static void PartOne()
		{
			Console.WriteLine(Math.Abs(SimulateFight(0)));
		}

		private static int SimulateFight(int boost)
		{
			List<string> lines = Utils.GetLinesFromFile("input/Day24Input.txt");

			bool addToImmuneSystem = true;

			List<UnitGroup> unitGroups = new List<UnitGroup>();

			foreach (string line in lines)
			{
				if (line.StartsWith("I"))
				{
					continue;
				}
				else if (line.Length == 0)
				{
					addToImmuneSystem = false;
					continue;
				}

				string[] splitLine = line.Split(" ");

				int units = Convert.ToInt32(splitLine[0]);
				int health = Convert.ToInt32(splitLine[4]);

				List<string> weaknesses = new List<string>();
				List<string> immunities = new List<string>();

				int currentIndex = 7;

				if (splitLine[7].StartsWith("("))
				{
					splitLine[7] = splitLine[7].Substring(1);
					while (true)
					{
						bool setWeaknesses = splitLine[currentIndex] == "weak";
						if (splitLine[currentIndex] == "weak" || splitLine[currentIndex] == "immune")
						{
							currentIndex += 1;
							do
							{
								currentIndex++;
								if (setWeaknesses)
									weaknesses.Add(splitLine[currentIndex].Substring(0, splitLine[currentIndex].Length - 1));
								else
									immunities.Add(splitLine[currentIndex].Substring(0, splitLine[currentIndex].Length - 1));
		
							} while (!splitLine[currentIndex].EndsWith(";") && !splitLine[currentIndex].EndsWith(")"));

							currentIndex++;
						}
						else
						{
							break;
						}
					}
				}

				int attack = Convert.ToInt32(splitLine[currentIndex + 5]) + (addToImmuneSystem ? boost : 0);
				string damageType = splitLine[currentIndex + 6];
				int initiative = Convert.ToInt32(splitLine[currentIndex + 10]);

				unitGroups.Add(new UnitGroup(addToImmuneSystem, units, health, attack, initiative, damageType, weaknesses, immunities));
			}

			int immuneUnits = unitGroups.Where(x => x.immuneSystem && x.units != 0).Count();
			int infectionUnits = unitGroups.Where(x => !x.immuneSystem && x.units != 0).Count();

			while (immuneUnits > 0 && infectionUnits > 0)
			{
				unitGroups.Sort();
				HashSet<int> chosenTargets = new HashSet<int>();
				List<Target> targets = new List<Target>();
				for (int i = 0; i < unitGroups.Count; i++)
				{
					if (unitGroups[i].units <= 0)
						continue;

					int maxDamage = -1;
					int index = -1;

					for (int j = 0; j < unitGroups.Count; j++)
					{
						if (chosenTargets.Contains(j))
							continue;

						if (unitGroups[j].units <= 0)
							continue;

						if (i == j)
						{
							continue;
						}

						if (unitGroups[i].immuneSystem == unitGroups[j].immuneSystem)
						{
							continue;
						}

						int potentialDamage = unitGroups[i].CalcDamage(unitGroups[j]);

						if (potentialDamage > maxDamage)
						{
							maxDamage = potentialDamage;
							index = j;
						}
						else if (potentialDamage == maxDamage)
						{
							if (unitGroups[j].EffectivePower() > unitGroups[index].EffectivePower())
							{
								index = j;
							}
							else if (unitGroups[j].EffectivePower() == unitGroups[index].EffectivePower() && unitGroups[j].initiative > unitGroups[index].initiative)
							{
								index = j;
							}
						}
					}

					if (index != -1 && maxDamage > 0)
					{
						targets.Add(new Target(i, index, unitGroups[i].initiative));
						chosenTargets.Add(index);
					}
				}

				targets.Sort();

				int totalUnitsLost = 0;
				foreach (Target target in targets)
				{
					int damage = unitGroups[target.source].CalcDamage(unitGroups[target.destination]);

					int unitsLost = damage / unitGroups[target.destination].health;
					totalUnitsLost += unitsLost;

					unitGroups[target.destination].units -= unitsLost;
				}

				if (totalUnitsLost == 0)
					return 0;

				immuneUnits = unitGroups.Where(x => x.immuneSystem && x.units > 0).Count();
				infectionUnits = unitGroups.Where(x => !x.immuneSystem && x.units > 0).Count();
			}

			int totalUnitsRemaining = unitGroups.Where(x => x.units > 0).Select(y => y.units).Sum();

			if (infectionUnits > 0)
				totalUnitsRemaining *= -1;

			return totalUnitsRemaining;
		}

		public static void PartTwo()
		{
			int boost = 0;
			int result;
			do
			{
				result = SimulateFight(boost++);
			} while (result <= 0);

			Console.WriteLine(result);
		}
	}
}