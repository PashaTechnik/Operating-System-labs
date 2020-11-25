using System;
using System.Collections.Generic;
using System.IO.Pipes;

namespace Process_Manager_Simulator
{
    public class RoundRobinAlgorithm
    {
        public int Quant;
        
        public RoundRobinAlgorithm(int quant)
        {
            Quant = quant;
        }

        public List<Process> Processes;

        public void ProcessEnter()
        {
            int num;
            Console.WriteLine("Enter the number of processes:");
            num = int.Parse(Console.ReadLine());
            
            for (int i = 0; i < num; i++)
            {
                Process process = new Process();
                process.Name = "P" + i;
                Console.WriteLine($"Enter time for {process.Name}:");
                process.Time = int.Parse(Console.ReadLine());
                Console.WriteLine($"Enter priority for {process.Name}:");
                process.Priority = int.Parse(Console.ReadLine());
                Processes.Add(process);
            }
        }

        public void ProcessInit(List<Process> processes)
        {
            Processes = processes;
        }

        public void ManageProcess()
        {
            List<Process> queue;
            Processes.Sort();
            

        }




    }
}