using System;

namespace Process_Manager_Simulator
{
    public class Process : IComparable
    {
        public int Time { get; set; }
        public string Name { get; set; }
        public int WaitTime { get; set; }
        public int Priority { get; set; }
        public bool IsComplete = false;
        public int CompareTo(object? obj)
        {
            Process p = obj as Process;
            if (p != null)
                return this.Priority.CompareTo(p.Priority);
            else
                throw new Exception("Impossible to compare");
        }

        public override string ToString()
        {
            return $"Name: {this.Name}\t Time: {this.Time}\t Priority {this.Priority}";
        }
    }
}