namespace AdventOfCode
{
    internal class Day10_Solution : Helper.IDaySolution<byte[][], long>
    {
        public byte[][] LoadData(string inputPath)
        {
            return File.ReadAllLines(inputPath + "/Day10.txt").Select(line => line.Select(c => byte.Parse(c.ToString())).ToArray()).ToArray();
        }

        public long Part1(byte[][] block)
        {
            long sum = 0;
            for (int i = 0; i < block.Length; i++)
            {
                for (int j = 0; j < block[i].Length; j++)
                {
                    if (block[i][j] == 0)
                    {
                        sum += DetermineTrailScore(block, i, j);
                    }
                }
            }
            return sum;
        }

        public long Part2(byte[][] block)
        {
            long sum = 0;
            for (int i = 0; i < block.Length; i++)
            {
                for (int j = 0; j < block[i].Length; j++)
                {
                    if (block[i][j] == 0)
                    {
                        sum += DetermineTrailRating(block, i, j);
                    }
                }
            }
            return sum;
        }

        private static long DetermineTrailScore(byte[][] block, int startX, int startY)
        {
            bool[,] visited = new bool[block.Length, block[0].Length];
            Queue<Tuple<int, int>> toDo = new();
            toDo.Enqueue(Tuple.Create(startX, startY));
            while (toDo.Count > 0)
            {
                Tuple<int, int> current = toDo.Dequeue();
                if (!visited[current.Item1, current.Item2])
                {
                    visited[current.Item1, current.Item2] = true;
                    if (HasNextHeight(block, current.Item1, current.Item2, -1, 0))
                    {
                        toDo.Enqueue(Tuple.Create(current.Item1 - 1, current.Item2));
                    }
                    if (HasNextHeight(block, current.Item1, current.Item2, 0, 1))
                    {
                        toDo.Enqueue(Tuple.Create(current.Item1, current.Item2 + 1));
                    }
                    if (HasNextHeight(block, current.Item1, current.Item2, 1, 0))
                    {
                        toDo.Enqueue(Tuple.Create(current.Item1 + 1, current.Item2));
                    }
                    if (HasNextHeight(block, current.Item1, current.Item2, 0, -1))
                    {
                        toDo.Enqueue(Tuple.Create(current.Item1, current.Item2 - 1));
                    }
                }
            }
            long count = 0;
            for (int i = 0; i < block.Length; i++)
            {
                for (int j = 0; j < block[i].Length; j++)
                {
                    if (visited[i, j] && block[i][j] == 9)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        private static bool HasNextHeight(byte[][] block, int currentX, int currentY, int deltaX, int deltaY)
        {
            if (currentX < 0 || currentX >= block.Length || currentY < 0 || currentY >= block[0].Length)
            {
                return false;
            }
            int nextX = currentX + deltaX;
            int nextY = currentY + deltaY;
            if (nextX < 0 || nextX >= block.Length || nextY < 0 || nextY >= block[0].Length)
            {
                return false;
            }
            byte currentHeight = block[currentX][currentY];
            byte nextHeight = block[nextX][nextY];
            if (currentHeight + 1 == nextHeight)
            {
                return true;
            }
            return false;
        }

        private static long DetermineTrailRating(byte[][] block, int startX, int startY)
        {
            if (startX < 0 || startX >= block.Length || startY < 0 || startY >= block[0].Length)
            {
                return 0;
            }
            if (block[startX][startY] == 9)
            {
                return 1;
            }
            long sum = 0;
            if (HasNextHeight(block, startX, startY, -1, 0))
            {
                sum += DetermineTrailRating(block, startX - 1, startY);
            }
            if (HasNextHeight(block, startX, startY, 0, 1))
            {
                sum += DetermineTrailRating(block, startX, startY + 1);
            }
            if (HasNextHeight(block, startX, startY, 1, 0))
            {
                sum += DetermineTrailRating(block, startX + 1, startY);
            }
            if (HasNextHeight(block, startX, startY, 0, -1))
            {
                sum += DetermineTrailRating(block, startX, startY - 1);
            }
            return sum;
        }
    }
}
