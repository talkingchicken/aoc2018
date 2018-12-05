using System;
using System.IO;
using System.Collections.Generic;

namespace AdventOfCode
{
    class DayTwo
    {
        public static void PartOne()
        {
            int twoCount = 0;
            int threeCount = 0;

            string line;
            StreamReader file = new StreamReader("Day2Input.txt");
            
            while ((line = file.ReadLine()) != null)
            {
                bool containsTwo = false;
                bool containsThree = false;

                char[] lineChars = line.ToCharArray();
                Array.Sort(lineChars);

                int lastNewCharIndex = 0;

                for (int i = 1; i < lineChars.Length; i++)
                {
                    if (lineChars[i] != lineChars[lastNewCharIndex])
                    {
                        int length = i - lastNewCharIndex;
                        if (length == 2)
                        {
                            containsTwo = true;
                        }
                        else if (length == 3)
                        {
                            containsThree = true;
                        }

                        lastNewCharIndex = i;
                    }
                }

                int lastLength = lineChars.Length - lastNewCharIndex;
                if (lastLength == 2)
                {
                    containsTwo = true;
                }
                else if (lastLength == 3)
                {
                    containsThree = true;
                }

                if (containsTwo)
                    twoCount++;

                if (containsThree)
                    threeCount++;
            }
            
            file.Close();

            Console.WriteLine("Checksum is {0}", twoCount * threeCount);
        }

        public static void PartTwo()
        {
            for (int i = 0; i < 26; i++)
            {
                HashSet<string> stringSet = new HashSet<string>();
                string line;
                StreamReader file = new StreamReader("Day2Input.txt");
                
                while ((line = file.ReadLine()) != null)
                {
                    string lineMinusOne = line.Substring(0, i) + line.Substring(i+1);
                    if (stringSet.Contains(lineMinusOne))
                    {
                        Console.WriteLine("Common letters are {0}", lineMinusOne);
                        return;
                    }
                    else
                    {
                        stringSet.Add(lineMinusOne);
                    }
                }
                
                file.Close();
            }
        }
    }
}