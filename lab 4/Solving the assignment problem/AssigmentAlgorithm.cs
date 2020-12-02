using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            assignmentMatrix = new AssignmentMatrix(N, MaxValue);
            Matrix = assignmentMatrix.Matrix;
        }

        public AssignmentMatrix assignmentMatrix;

        public int[,] Matrix;



        public void MatrixTransformation()
        {

            PrepareMatrix();
            Console.WriteLine(assignmentMatrix);
            Console.WriteLine("================================");
            int[,] m = (int[,]) assignmentMatrix.Clone();
            List<Point> pare = findMaxMatching(m);

            List<int> xPoint = new List<int>();
            List<int> yPoint = new List<int>();

            int k = 0;
            int noPare = -1;
            foreach (var i in pare)
            {
                Console.WriteLine($"x = {i.x}; y = {i.y};");
                if (i.x != k)
                {
                    noPare = k;
                }

                k++;
            }

            if (noPare == -1)
            {
                noPare = N - 1;
            }

            for (int j = 0; j < N; j++)
            {
                if (m[noPare, j] == 1)
                {
                    yPoint.Add(noPare);
                    xPoint.Add(j);
                }
            }



            Console.WriteLine(assignmentMatrix);
            Console.WriteLine("================================");
            DisplayMatrix(m);
            Console.WriteLine("================================");
            Console.WriteLine(xPoint.First());
            Console.WriteLine(yPoint.First());
            
            



    }

        public void PrepareMatrix()
        {
            //int maxElemInColumn;
            int minElemInLine;

            // for (int i = 0; i < N; i++)
            // {
            //     Console.WriteLine(assignmentMatrixMatrix);
            //     Console.WriteLine("================================");
            //     maxElemInColumn = int.MinValue;
            //     for (int j = 0; j < N; j++)
            //     {
            //         if (Matrix[j, i] > maxElemInColumn)
            //         {
            //             maxElemInColumn = Matrix[j, i];
            //         }
            //     }
            //     
            //     for (int j = 0; j < N; j++)
            //     {
            //         Matrix[j, i] = maxElemInColumn - Matrix[j, i];
            //     }
            //
            // }
            for (int j = 0; j < N; j++)
            {
                Console.WriteLine(assignmentMatrix);
                Console.WriteLine("================================");
                minElemInLine = int.MaxValue;
                
                for (int i = 0; i < N; i++)
                {
                    if (Matrix[j, i] < minElemInLine)
                    {
                        minElemInLine = Matrix[j, i];
                    }
                }
                for (int i = 0; i < N; i++)
                {
                    Matrix[j, i] =  Matrix[j, i] - minElemInLine;
                }
            }
        }

        List<Point> findMaxMatching(int[,] matrix)
        {
            List<Point> pare = new List<Point>();
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (matrix[i, j] == 0)
                    {
                        matrix[i, j] = 1;
                    }
                    else
                    {
                        matrix[i, j] = 0;
                    }
                } 
            }

            bool contain = false;
            
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    contain = false;
                    for (int k = 0; k < N; k++)
                    {
                        if (pare.Contains( new Point(k,j)))
                        {
                            contain = true;
                        }
                    }
                    if (matrix[i, j] == 1 && contain != true)
                    {
                        pare.Add(new Point(i,j));
                        break;
                    }
                } 
            }

            return pare;
        }
        
        public void DisplayMatrix(int [,] Matrix){
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
            Console.WriteLine(matrix);
        }
    }
    

    struct Point
    {
        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int x;
        public int y;
    }
}