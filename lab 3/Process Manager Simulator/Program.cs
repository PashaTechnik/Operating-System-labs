using System;
using System.Collections.Generic;

namespace Process_Manager_Simulator
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Process> processes = new List<Process>();
            processes.Add(new Process
            {
                Time = 12,
                Name = "P1",
                Priority = 1
            });
            processes.Add(new Process
            {
                Time = 12,
                Name = "P1",
                Priority = 3
            });
            processes.Add(new Process
            {
                Time = 12,
                Name = "P1",
                Priority = 1
            });
            processes.Add(new Process
            {
                Time = 12,
                Name = "P1",
                Priority = 2
            });
            processes.Sort();

            foreach (var i in processes)
            {
                Console.WriteLine(i);
            }
            
            RoundRobinAlgorithm alg = new RoundRobinAlgorithm(10);
            List<List<Process>> queuesByPriotity = alg.SplitIntoQueues(processes);

            foreach (var items in queuesByPriotity)
            {
                Console.WriteLine("queue:");
                foreach (var item in items)
                {
                    Console.WriteLine(item);
                }
            }

        }
    }
}