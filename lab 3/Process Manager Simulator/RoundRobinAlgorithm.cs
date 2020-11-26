using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;

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
            List<Process> queue = new List<Process>();
            List<List<Process>> queuesByPriotity;
            Processes.Sort();
            queuesByPriotity = SplitIntoQueues(Processes);

            foreach (var items in queuesByPriotity)
            {
                foreach (var item in items)
                {
                    queue.Add(item);
                }

                while (queue.Count != 0)
                {
                    PrintQueue(queue);
                    var elem = queue.First();
                    elem.Time -= Quant;
                    if (elem.Time <= 0)
                    {
                        elem.IsComplete = true;
                        queue.Remove(elem);
                    }
                    else
                    {
                        queue.Remove(elem);
                        queue.Add(elem);
                    }
                }
            }
        }

        public void PrintQueue(List<Process> queue)
        {
            Console.WriteLine("Current Queue:");

            Console.WriteLine("--------------------------");
            foreach (var item in queue)
            {
                Console.WriteLine($"Name: {item.Name}\t Time: {item.Time}");
            }
            Console.WriteLine("--------------------------");
        }

        public void GanttDiagram()
        {
            
            List<Process> queue = new List<Process>();
            List<List<Process>> queuesByPriotity;
            Processes.Sort();
            queuesByPriotity = SplitIntoQueues(Processes);
            StringBuilder ganttDiagram = new StringBuilder();
            int WaitingTime = 0;

            foreach (var items in queuesByPriotity)
            {
                foreach (var item in items)
                {
                    queue.Add(item);
                }
                
                
                while (queue.Count != 0)
                {
                    var elem = queue.First();
                    ganttDiagram.Append($"| {elem.Name} : {WaitingTime} ");
                    if (elem.Time < Quant)
                    {
                        WaitingTime += Quant - elem.Time;
                    }
                    elem.Time -= Quant;
                    if (elem.Time <= 0)
                    {
                        elem.IsComplete = true;
                        queue.Remove(elem);
                    }
                    else
                    {
                        WaitingTime += Quant;
                        queue.Remove(elem);
                        queue.Add(elem);
                    }
                }
            }

            ganttDiagram.Append("|");
            Console.WriteLine(ganttDiagram);

        }

        public  List<List<Process>> SplitIntoQueues(List<Process> queue)
        {
            List<List<Process>> queuesByPriotity = new List<List<Process>>();
            var maxPriority = queue.Max(m => m.Priority);
            for (int i = 1; i <= maxPriority; i++)
            {
                List<Process> temp = new List<Process>();
                temp = queue.Where(s => s.Priority == i).ToList();
                if (temp.Count == 0)
                {
                    continue;
                }
                queuesByPriotity.Add(temp);
            }

            return queuesByPriotity;
        }
        
        static void findWaitingTime(int []processes, int n, int []bt, int []wt, int quantum) {
            int []rem_bt = new int[n];
            for (int i = 0 ; i < n ; i++) 
                rem_bt[i] = bt[i]; 
            int t = 0; 
            while(true) 
            {
                bool done = true; 
                for (int i = 0 ; i < n; i++) 
                { 
                    if (rem_bt[i] > 0) 
                    { 
                        done = false;
                        
                        if (rem_bt[i] > quantum) 
                        { 
                            t += quantum; 
                            rem_bt[i] -= quantum; 
                        }
                        else 
                        { 
                            t = t + rem_bt[i]; 
                            wt[i] = t - bt[i]; 
                            rem_bt[i] = 0; 
                        } 
                    } 
                } 
                if (done == true) 
                    break; 
            } 
        } 
        
    static void findTurnAroundTime(int []processes, int n, int []bt, int []wt, int []tat) 
    {
        for (int i = 0; i < n ; i++) 
            tat[i] = bt[i] + wt[i]; 
    } 
    
     public void findavgTime(int []processes, int n, int []bt, int quantum) 
        { 
            int []wt = new int[n]; 
            int []tat = new int[n]; 
            int total_wt = 0, total_tat = 0;
            findWaitingTime(processes, n, bt, wt, quantum);
            findTurnAroundTime(processes, n, bt, wt, tat);
            Console.WriteLine("Processes " + " Burst time " + " Waiting time " + " Turn around time");
            for (int i = 0; i < n; i++) 
            { 
                total_wt = total_wt + wt[i]; 
                total_tat = total_tat + tat[i]; 
                Console.WriteLine(" " + (i+1) + "\t\t" + bt[i] + "\t " + wt[i] +"\t\t " + tat[i]); 
            }
            Console.WriteLine("Average waiting time = " + (float)total_wt / (float)n); 
            Console.Write("Average turn around time = " + (float)total_tat / (float)n); 
        }
    }
}