using System;
using System.IO;
using System.Collections.Generic;

namespace AdventOfCode
{
    class Rect
    {
        public int id { get; set; }
        public int left { get; set; }
        public int top { get; set; }
        public int width { get; set; }
        public int height { get; set; }

        public Rect(string line)
        {
            string[] words = line.Split(" ");

            id = Convert.ToInt32(words[0].Substring(1));
            string[] dimensions = words[2].Split(",");
            top = Convert.ToInt32(dimensions[1].Substring(0, dimensions[1].Length - 1));
            left = Convert.ToInt32(dimensions[0]);

            string[] size = words[3].Split("x");
            width = Convert.ToInt32(size[0]);
            height = Convert.ToInt32(size[1]);
        }
    }

    class DayThree
    {
        public static void PartOne()
        {
            int[,] matrix = new int[1000, 1000];
            int count = 0;
            string line;
            StreamReader file = new StreamReader("Day3Input.txt");
            
            while ((line = file.ReadLine()) != null)
            {
                Rect currentRect = new Rect(line);

                for (int x = currentRect.left; x < currentRect.left + currentRect.width; x++)
                {
                    for (int y = currentRect.top; y < currentRect.top + currentRect.height; y++)
                    {
                        matrix[x,y]++;
                    }
                }
            }
            
            file.Close();

            for (int x = 0; x < 1000; x++)
            {
                for (int y = 0; y < 1000; y++)
                {
                    if (matrix[x,y] > 1)
                    {
                        count++;
                    }
                }
            }

            Console.WriteLine("Number of overlapping squares is {0}", count);
            
        }

        public static void PartTwo()
        {
            int[,] matrix = new int[1000, 1000];

            List<Rect> rects = new List<Rect>();

            string line;
            StreamReader file = new StreamReader("Day3Input.txt");
            
            while ((line = file.ReadLine()) != null)
            {
                Rect currentRect = new Rect(line);
                rects.Add(currentRect);

                for (int x = currentRect.left; x < currentRect.left + currentRect.width; x++)
                {
                    for (int y = currentRect.top; y < currentRect.top + currentRect.height; y++)
                    {
                        matrix[x,y]++;
                    }
                }
            }
            
            file.Close();

            foreach (Rect rect in rects)
            {
                if (!containsOverlap(matrix, rect))
                {
                    Console.WriteLine("Non-overlapping rect has ID {0}", rect.id);
                    return;
                }
            }
        }

        private static bool containsOverlap(int[,] matrix, Rect rect)
        {
            for (int x = rect.left; x < rect.left + rect.width; x++)
            {
                for (int y = rect.top; y < rect.top + rect.height; y++)
                {
                    if (matrix[x, y] > 1)
                    {
                        return true;
                    }
                }
            }
            
            return false;
        }
    }
}