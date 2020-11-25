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
            
            
        }
    }
}