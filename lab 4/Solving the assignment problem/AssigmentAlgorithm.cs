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
        
                int[,] resultM = (int[,]) assignmentMatrix.Clone();

                PrepareMatrix();
                Console.WriteLine(assignmentMatrix);
                Console.WriteLine("================================");
                int[,] m = (int[,]) assignmentMatrix.Clone();
                int[,] transformedM = (int[,]) assignmentMatrix.Clone();
                List<Point> pare = findMaxMatching(m);
                
                
                List<int> xPoint = new List<int>();
                List<int> yPoint = new List<int>();

                int k = 0;
                int noPare = -1;
                foreach (var i in pare)
                {
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
                        xPoint.Add(j);
                    }
                }

                for (int j = 0; j < N; j++)
                {
                    if (m[j, xPoint.First()] == 1)
                    {
                        yPoint.Add(j);
                    }
                }


                Console.WriteLine(assignmentMatrix);
                Console.WriteLine("================================");
                DisplayMatrix(m);
                Console.WriteLine("================================");
                transform(ref transformedM, xPoint, yPoint);

                DisplayMatrix(transformedM);
                int minValueOfTransformedMatrix = int.MaxValue;

                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        if (transformedM[i, j] != -1)
                        {
                            if (transformedM[i, j] < minValueOfTransformedMatrix)
                            {
                                minValueOfTransformedMatrix = transformedM[i, j];
                            }
                        }
                    }
                }

                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        if (j == xPoint.First())
                        {
                            Matrix[i, j] = Matrix[i, j] + minValueOfTransformedMatrix;
                        }
                        if (yPoint.Contains(i))
                        {
                            Matrix[i, j] = Matrix[i, j] - minValueOfTransformedMatrix;
                        }
                    }
                }

                pare = findMaxMatching((int[,]) assignmentMatrix.Clone());

                Console.WriteLine(assignmentMatrix);
                Console.WriteLine("================================");

                showResult(resultM,pare);
        }

        public void PrepareMatrix()
        {
            int minElemInLine;
            
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

            int temp = pare.Count;
            List<Point> tempPare = pare;
            pare.Clear();

            for (int i = N - 1; i >= 0; i--)
            {
                for (int j = N - 1; j >= 0; j--)
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

            if (pare.Count > temp)
            {
                return pare;
            }
            else
            {
                return tempPare;
            }

        }

        public void transform(ref int[,] matrix, List<int> x, List<int> y)
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (j == x.First())
                    {
                        matrix[i, j] = -1;
                    }
                    if (!y.Contains(i))
                    {
                        matrix[i, j] = -1;
                    }
                } 
            }
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
        
        
        public void findDestinations(){
    
            int[,] matrix = new int[N,N];
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
        }

        public void showResult(int[,] matrix, List<Point> pare){
            int result = 0;
            StringBuilder resultStr = new StringBuilder();
            resultStr.Append("Lowest cost = ");

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (pare.Contains(new Point(i,j)))
                    {
                        resultStr.Append(matrix[i, j]);
                        result += matrix[i, j];

                        
                        resultStr.Append(" + ");
                        
                    }
                }
            }
            

            resultStr.Remove(resultStr.Length - 2, 2);
            resultStr.Append("= ");
            resultStr.Append(result);
            //Console.WriteLine(resultStr);
            Console.WriteLine($"Lowest cost = {result}");
        }
        
        
    }
    


    public struct Point
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