using System;
using System.IO;
using System.Collections.Generic;

namespace AdventOfCode
{
    class DayOne
    {
        public static void PartOne()
        {
            Dictionary<int, int> dictionary = new Dictionary<int, int>();
            dictionary.Add(0, 1);
            int value = 0;
            string line;

            StreamReader file = new StreamReader("Day1Input.txt");

            while((line = file.ReadLine()) != null)  
            {  
                int modifier = Convert.ToInt32(line);
                value += modifier;
            }  

            file.Close();  

            Console.WriteLine("Final value is {0}", value);
        }

        public static void PartTwo()
        {
            Dictionary<long, int> dictionary = new Dictionary<long, int>();
            dictionary.Add(0, 1);
            long value = 0;
            string line;

            while (true)
            {
                StreamReader file = new StreamReader("Day1Input.txt");

                while((line = file.ReadLine()) != null)  
                {  
                    long modifier = Convert.ToInt64(line);
                    value += modifier;
                    if (dictionary.ContainsKey(value))
                    {
                        file.Close();
                        System.Console.WriteLine("Duplicated value is {0}", value);
                        return;
                    }
                    else
                    {
                        dictionary.Add(value, 1);
                    }
                }  

                file.Close();  
            }
        }
    }
}
