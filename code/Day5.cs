using System;
using System.IO;
using System.Collections.Generic;

namespace AdventOfCode
{
    class DayFive
    {
        private static bool CheckForReaction(char firstChar, char secondChar)
        {
            return firstChar != secondChar && char.ToUpper(firstChar) == char.ToUpper(secondChar);
        }
        
        private static string ReactPolymer(string filename, char charToIgnore)
        {
            string polymer = "";

            char currentChar;
            StreamReader file = new StreamReader(filename);

            while (file.Peek() >= 0)
            {
                currentChar = (char)file.Read();

                if (char.ToUpper(currentChar) == char.ToUpper(charToIgnore))
                    continue;

                if (polymer.Length > 0 && CheckForReaction(currentChar, polymer[polymer.Length - 1]))
                {
                    polymer = polymer.Substring(0, polymer.Length - 1);
                }
                else
                {
                    polymer += currentChar;
                }
            }
            file.Close();

            return polymer;
        }

        public static void PartOne()
        {
            string polymer = ReactPolymer("input/Day5Input.txt", '0');

            Console.WriteLine("There are {0} units remaining", polymer.Length);
        }

        public static void PartTwo()
        {
            int minimumSize = 50000;
            char shortestChar = '0';

            for (char counter = 'a'; counter <= 'z'; counter++)
            {
                string polymer = ReactPolymer("input/Day5Input.txt", counter);
                
                if (polymer.Length < minimumSize)
                {
                    minimumSize = polymer.Length;
                    shortestChar = counter;
                }
            }

            Console.WriteLine("Shortest Length is {0} by removing {1}", minimumSize, shortestChar);
        }
    }
}