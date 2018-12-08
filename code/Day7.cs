using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    class GraphNode
    {
        public HashSet<char> dependencies { get; set; }
        public List<char> targets { get; set; }

        public GraphNode()
        {
            dependencies = new HashSet<char>();
            targets = new List<char>();
        }
    }

    class DaySeven
    {
        public static void PartOne()
        {
            List<string> input = Utils.GetLinesFromFile("input/Day7Input.txt");
            Dictionary<char, GraphNode> graph = new Dictionary<char, GraphNode>();

            foreach (string line in input)
            {
                if (!graph.ContainsKey(line[5]))
                {
                    graph.Add(line[5], new GraphNode());
                }

                graph[line[5]].targets.Add(line[36]);

                if (!graph.ContainsKey(line[36]))
                {
                    graph.Add(line[36], new GraphNode());
                }

                graph[line[36]].dependencies.Add(line[5]);
            }

            HashSet<char> targets = new HashSet<char>();

            foreach (KeyValuePair<char, GraphNode> entry in graph)
            {
                foreach (char value in entry.Value.targets)
                {
                    targets.Add(value);
                }
            }

            SortedSet<char> queue = new SortedSet<char>();
            
            foreach (char letter in "qwertyuiopasdfghjklzxcvbnm".ToUpper())
            {
                if (!targets.Contains(letter))
                {
                    queue.Add(letter);
                }
            }
            
            string result = "";

            while (queue.Count > 0)
            {
                char nextChar = queue.First();
                queue.Remove(nextChar);

                result += nextChar;

                if (graph.ContainsKey(nextChar))
                {
                    foreach (char nextItems in graph[nextChar].targets)
                    {
                        graph[nextItems].dependencies.Remove(nextChar);
                        if (graph[nextItems].dependencies.Count == 0)
                            queue.Add(nextItems);
                    }
                }
            }

            Console.WriteLine("Order is {0}", result);
        }

        class Worker
        {
            public char task { get; set; }
            private int timeRemaining { get; set; }

            public bool IsIdle() { return timeRemaining == 0; }
            public void QueueTask(char newTask) 
            {
                task = newTask;
                timeRemaining = newTask - 'A' + 61;
            }

            public void Work() 
            { 
                if (timeRemaining > 0) 
                    timeRemaining--; 
            }
        }

        public static void PartTwo()
        {
            int counter = 0;

            List<string> input = Utils.GetLinesFromFile("input/Day7Input.txt");
            Dictionary<char, GraphNode> graph = new Dictionary<char, GraphNode>();

            foreach (string line in input)
            {
                if (!graph.ContainsKey(line[5]))
                {
                    graph.Add(line[5], new GraphNode());
                }

                graph[line[5]].targets.Add(line[36]);

                if (!graph.ContainsKey(line[36]))
                {
                    graph.Add(line[36], new GraphNode());
                }

                graph[line[36]].dependencies.Add(line[5]);
            }

            HashSet<char> targets = new HashSet<char>();

            foreach (KeyValuePair<char, GraphNode> entry in graph)
            {
                foreach (char value in entry.Value.targets)
                {
                    targets.Add(value);
                }
            }

            SortedSet<char> queue = new SortedSet<char>();
            
            foreach (char letter in "qwertyuiopasdfghjklzxcvbnm".ToUpper())
            {
                if (!targets.Contains(letter))
                {
                    queue.Add(letter);
                }
            }

            List<Worker> workers = new List<Worker>();

            for (int i = 0; i < 5; i++)
            {
                workers.Add(new Worker());
            }

            do
            {
                counter++;

                foreach (Worker worker in workers)
                {
                    if (worker.IsIdle())
                    {
                        if (queue.Count > 0)
                        {
                            char newTask = queue.First();
                            worker.QueueTask(newTask);
                            queue.Remove(newTask);
                        }
                    }
                }

                foreach (Worker worker in workers)
                {
                    if (!worker.IsIdle())
                    {
                        worker.Work();
                    
                        if (worker.IsIdle())
                        {
                            foreach (char nextTarget in graph[worker.task].targets)
                            {
                                if (graph.ContainsKey(nextTarget))
                                {
                                    graph[nextTarget].dependencies.Remove(worker.task);
                                    if (graph[nextTarget].dependencies.Count == 0)
                                    {
                                        queue.Add(nextTarget);
                                    }
                                }
                            }
                        }
                    }
                }
            } while (queue.Count > 0 || workers.Where(x => x.IsIdle()).Count() != workers.Count);

            Console.WriteLine("Total time spent: {0}", counter);
        }
    }
}