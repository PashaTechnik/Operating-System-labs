using System;
using System.Collections.Generic;
using System.Linq;

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
                Name = "P2",
                Priority = 3
            });
            processes.Add(new Process
            {
                Time = 12,
                Name = "P3",
                Priority = 1
            });
            processes.Add(new Process
            {
                Time = 12,
                Name = "P4",
                Priority = 2
            });
            
            int []processesId = new int[processes.Count] ;
            for (int i = 0; i < processesId.Length; i++)
            {
                processesId[i] = i + 1;
            }
            
            int n = processesId.Length; 
            
            int []burst_time = new int[processes.Count];
            int k = 0;
            foreach (var item in processes)
            {
                burst_time[k] = item.Time;
                k++;
            }
            int quantum = 2;

            processes.Sort();

            foreach (var i in processes)
            {
                Console.WriteLine(i);
            }
            
            RoundRobinAlgorithm alg = new RoundRobinAlgorithm(2);

            alg.ProcessInit(processes);
            //alg.ManageProcess();
            Console.WriteLine("Gantt Diagram");
            alg.GanttDiagram();
            alg.findavgTime(processesId, n, burst_time, quantum); 
            
            
            
            
        }
    }
}