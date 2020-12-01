using System;

namespace Solving_the_assignment_problem
{
    public class AssigmentAlgorithm
    {
        private int N { get; set; }
        public int MaxValue { get; set; }

        public AssigmentAlgorithm(int n, int maxValue)
        {
            N = n;
            MaxValue = maxValue;
            assignmentMatrixMatrix = new AssignmentMatrix(N, MaxValue);
            Matrix = assignmentMatrixMatrix.Matrix;
        }

        public AssignmentMatrix assignmentMatrixMatrix;

        public int[,] Matrix;
        
        

        public void MatrixTransformation()
        {
            // Console.WriteLine(assignmentMatrixMatrix);
            // Console.WriteLine("================================");
            PrepareMatrix();
            // Console.WriteLine(assignmentMatrixMatrix);
            // Console.WriteLine("================================");
            
            
        }

        public void PrepareMatrix()
        {
            int maxElemInColumn;
            int minElemInLine;

            for (int i = 0; i < N; i++)
            {
                Console.WriteLine(assignmentMatrixMatrix);
                Console.WriteLine("================================");
                maxElemInColumn = int.MinValue;
                for (int j = 0; j < N; j++)
                {
                    if (Matrix[j, i] > maxElemInColumn)
                    {
                        maxElemInColumn = Matrix[j, i];
                    }
                }
                
                for (int j = 0; j < N; j++)
                {
                    Matrix[j, i] = maxElemInColumn - Matrix[j, i];
                }

            }
            for (int j = 0; j < N; j++)
            {
                Console.WriteLine(assignmentMatrixMatrix);
                Console.WriteLine("================================");
                minElemInLine = int.MaxValue;
                
                for (int i = 0; i < N; i++)
                {
                    if (Matrix[i, j] < minElemInLine)
                    {
                        minElemInLine = Matrix[j, i];
                    }
                }
                Console.WriteLine("В строке номер {0} -> {1}", j, minElemInLine);
                
                for (int i = 0; i < N; i++)
                {
                    Matrix[i, j] = Matrix[i, j] - minElemInLine;
                }

            }

        }

    }
    
}