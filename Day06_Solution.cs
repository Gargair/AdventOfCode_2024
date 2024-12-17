namespace AdventOfCode
{
    internal class Day06_Solution : Helper.IDaySolution<char[][], long>
    {
        public char[][] LoadData(string inputPath)
        {
            return File.ReadAllLines(inputPath + "/Day06.txt").Select(line => line.ToCharArray()).ToArray();
        }

        public long Part1(char[][] block)
        {
            int[] start = Helper.MatrixHelper.FindFirstElement(block, '^');
            DeterminePath(block, start, out List<long> positionsVisited);
            return positionsVisited.Count;
        }

        public long Part2(char[][] block)
        {
            int[] start = Helper.MatrixHelper.FindFirstElement(block, '^');
            long blockWidth = block[0].Length;
            // Take actual path
            DeterminePath(block, start, out List<long> positionsVisited);

            return positionsVisited.AsParallel().WithDegreeOfParallelism(Environment.ProcessorCount).Where(position =>
            {
                int j = (int)(position % blockWidth);
                int i = (int)(position / blockWidth);

                return DetermineLoop(block, start, i, j);
            }).Count();
        }

        private static bool DetermineLoop(char[][] block, int[] start, int blockedX, int blockedY)
        {
            bool[] positionsVisited = new bool[block.Length * block[0].Length * 10 * 10];
            int[] current = start;
            long blockWidth = block[0].Length;
            int deltaX = -1;
            int deltaY = 0;
            while (Helper.MatrixHelper.IsInMatrix(block, current[0], current[1]))
            {
                long posNum = (current[0] * blockWidth + current[1]) * 100 + (deltaX + 1) * 10 + (deltaY + 1);
                if (!positionsVisited[posNum])
                {
                    positionsVisited[posNum] = true;
                }
                else
                {
                    return true;
                }
                int[] next = [current[0] + deltaX, current[1] + deltaY];
                if (Helper.MatrixHelper.IsInMatrix(block, next[0], next[1]))
                {
                    if (block[next[0]][next[1]] == '#' || blockedX == next[0] && blockedY == next[1])
                    {
                        if (deltaX == -1 && deltaY == 0)
                        {
                            deltaX = 0;
                            deltaY = 1;
                        }
                        else if (deltaX == 0 && deltaY == 1)
                        {
                            deltaX = 1;
                            deltaY = 0;
                        }
                        else if (deltaX == 1 && deltaY == 0)
                        {
                            deltaX = 0;
                            deltaY = -1;
                        }
                        else if (deltaX == 0 && deltaY == -1)
                        {
                            deltaX = -1;
                            deltaY = 0;
                        }
                        continue;
                    }
                }
                current = next;
            }
            return false;
        }

        private static void DeterminePath(char[][] block, int[] start, out List<long> positionsVisited)
        {
            positionsVisited = [];
            int[] current = start;
            long blockWidth = block[0].Length;
            int deltaX = -1;
            int deltaY = 0;
            while (Helper.MatrixHelper.IsInMatrix(block, current[0], current[1]))
            {
                long posNum = current[0] * blockWidth + current[1];
                if (!positionsVisited.Contains(posNum))
                {
                    positionsVisited.Add(posNum);
                }
                int[] next = [current[0] + deltaX, current[1] + deltaY];
                if (Helper.MatrixHelper.IsInMatrix(block, next[0], next[1]))
                {
                    if (block[next[0]][next[1]] == '#')
                    {
                        if (deltaX == -1 && deltaY == 0)
                        {
                            deltaX = 0;
                            deltaY = 1;
                        }
                        else if (deltaX == 0 && deltaY == 1)
                        {
                            deltaX = 1;
                            deltaY = 0;
                        }
                        else if (deltaX == 1 && deltaY == 0)
                        {
                            deltaX = 0;
                            deltaY = -1;
                        }
                        else if (deltaX == 0 && deltaY == -1)
                        {
                            deltaX = -1;
                            deltaY = 0;
                        }
                        continue;
                    }
                }
                current = next;
            }
        }
    }
}
