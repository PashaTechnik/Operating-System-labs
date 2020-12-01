using System;
using System.Text;

namespace Solving_the_assignment_problem
{
    public class AssignmentMatrix
    {
        private int n;
        public int N { get => this.n; }
        
        public int[,] Matrix;

        public AssignmentMatrix(int n,int maxValue)
        {
            Random r = new Random();
            this.n = n;
            this.Matrix = new int[n,n];

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    Matrix[i, j] = r.Next(maxValue);
                } 
            }
        }

        public override string ToString()
        {
            StringBuilder matrix = new StringBuilder();
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    matrix.Append(Matrix[i, j]);
                    matrix.Append("\t");
                }
                matrix.Append("\n");
            }
            return matrix.ToString();
        }
    }
}