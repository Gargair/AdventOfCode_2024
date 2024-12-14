namespace AdventOfCode
{
    internal class Day12_Solution : Helper.IDaySolution<char[][]>
    {
        public char[][] LoadData(string inputPath)
        {
            return File.ReadAllLines(inputPath + "/Day12.txt").Select(s => s.ToCharArray()).ToArray();
        }

        public long Part1(char[][] plots)
        {
            bool[,] calculated = new bool[plots.Length, plots[0].Length];
            long sum = 0;
            for (int i = 0; i < plots.Length; i++)
            {
                for (int j = 0; j < plots[i].Length; j++)
                {
                    if (!calculated[i, j])
                    {
                        sum += CalculateRegionByPerimeter(plots, i, j, calculated);
                    }
                }
            }
            return sum;
        }

        public long Part2(char[][] plots)
        {
            bool[,] calculated = new bool[plots.Length, plots[0].Length];
            long sum = 0;
            for (int i = 0; i < plots.Length; i++)
            {
                for (int j = 0; j < plots[i].Length; j++)
                {
                    if (!calculated[i, j])
                    {
                        sum += CalculateRegionBySides(plots, i, j, calculated);
                    }
                }
            }
            return sum;
        }

        private static long CalculateRegionByPerimeter(char[][] plots, int startX, int startY, bool[,] calculated)
        {
            long area = 0;
            long perimeter = 0;
            Queue<Tuple<int, int>> queue = new();
            queue.Enqueue(Tuple.Create(startX, startY));
            char plotChar = plots[startX][startY];
            while (queue.Count > 0)
            {
                Tuple<int, int> current = queue.Dequeue();
                if (IsInMatrix(plots, current.Item1, current.Item2))
                {
                    if (!calculated[current.Item1, current.Item2])
                    {
                        area++;
                        if (IsInMatrix(plots, current.Item1 + 1, current.Item2) && plots[current.Item1 + 1][current.Item2] == plotChar)
                        {
                            queue.Enqueue(Tuple.Create(current.Item1 + 1, current.Item2));
                        }
                        else
                        {
                            perimeter++;
                        }
                        if (IsInMatrix(plots, current.Item1, current.Item2 + 1) && plots[current.Item1][current.Item2 + 1] == plotChar)
                        {

                            queue.Enqueue(Tuple.Create(current.Item1, current.Item2 + 1));
                        }
                        else
                        {
                            perimeter++;
                        }
                        if (IsInMatrix(plots, current.Item1 - 1, current.Item2) && plots[current.Item1 - 1][current.Item2] == plotChar)
                        {
                            queue.Enqueue(Tuple.Create(current.Item1 - 1, current.Item2));
                        }
                        else
                        {
                            perimeter++;
                        }
                        if (IsInMatrix(plots, current.Item1, current.Item2 - 1) && plots[current.Item1][current.Item2 - 1] == plotChar)
                        {
                            queue.Enqueue(Tuple.Create(current.Item1, current.Item2 - 1));
                        }
                        else
                        {
                            perimeter++;
                        }
                    }
                    calculated[current.Item1, current.Item2] = true;
                }

            }
            return area * perimeter;
        }

        private static long CalculateRegionBySides(char[][] plots, int startX, int startY, bool[,] calculated)
        {
            long area = 0;
            long sides = 0;
            List<Tuple<int, int, int>> sideList = new();
            Queue<Tuple<int, int>> queue = new();
            queue.Enqueue(Tuple.Create(startX, startY));
            char plotChar = plots[startX][startY];
            while (queue.Count > 0)
            {
                Tuple<int, int> current = queue.Dequeue();
                if (IsInMatrix(plots, current.Item1, current.Item2))
                {
                    if (!calculated[current.Item1, current.Item2])
                    {
                        area++;
                        if (IsInMatrix(plots, current.Item1 + 1, current.Item2) && plots[current.Item1 + 1][current.Item2] == plotChar)
                        {
                            queue.Enqueue(Tuple.Create(current.Item1 + 1, current.Item2));
                        }
                        else
                        {
                            sideList.Add(Tuple.Create(current.Item1, current.Item2, 1));
                        }
                        if (IsInMatrix(plots, current.Item1, current.Item2 + 1) && plots[current.Item1][current.Item2 + 1] == plotChar)
                        {

                            queue.Enqueue(Tuple.Create(current.Item1, current.Item2 + 1));
                        }
                        else
                        {
                            sideList.Add(Tuple.Create(current.Item1, current.Item2, 2));
                        }
                        if (IsInMatrix(plots, current.Item1 - 1, current.Item2) && plots[current.Item1 - 1][current.Item2] == plotChar)
                        {
                            queue.Enqueue(Tuple.Create(current.Item1 - 1, current.Item2));
                        }
                        else
                        {
                            sideList.Add(Tuple.Create(current.Item1, current.Item2, 3));
                        }
                        if (IsInMatrix(plots, current.Item1, current.Item2 - 1) && plots[current.Item1][current.Item2 - 1] == plotChar)
                        {
                            queue.Enqueue(Tuple.Create(current.Item1, current.Item2 - 1));
                        }
                        else
                        {
                            sideList.Add(Tuple.Create(current.Item1, current.Item2, 0));
                        }
                    }
                    calculated[current.Item1, current.Item2] = true;
                }

            }
            while (sideList.Count > 0)
            {
                var first = sideList[0];
                int direction = first.Item3;
                sides++;
                Queue<Tuple<int, int>> sideQueue = new();
                sideQueue.Enqueue(Tuple.Create(first.Item1, first.Item2));
                while (sideQueue.Count > 0)
                {
                    Tuple<int, int> current = sideQueue.Dequeue();
                    int index = sideList.IndexOf(Tuple.Create(current.Item1, current.Item2, direction));
                    if (index != -1)
                    {
                        sideList.RemoveAt(index);
                    }
                    else
                    {
                        continue;
                    }
                    if (direction == 0 || direction == 2)
                    {
                        sideQueue.Enqueue(Tuple.Create(current.Item1 + 1, current.Item2));
                        sideQueue.Enqueue(Tuple.Create(current.Item1 - 1, current.Item2));
                    }
                    else if (direction == 1 || direction == 3)
                    {
                        sideQueue.Enqueue(Tuple.Create(current.Item1, current.Item2 + 1));
                        sideQueue.Enqueue(Tuple.Create(current.Item1, current.Item2 - 1));
                    }
                }
            }
            return area * sides;
        }

        private static bool IsInMatrix<T>(T[][] matrix, int x, int y)
        {
            return x >= 0 && x < matrix.Length && y >= 0 && y < matrix[x].Length;
        }
    }
}
