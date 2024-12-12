﻿namespace AdventOfCode
{
    internal class Day06_Solution : IDaySolutionUpdate<char[][]>
    {
        public char[][] LoadData(string inputPath)
        {
            return File.ReadAllLines(inputPath + "/Day06.txt").Select(line => line.ToCharArray()).ToArray();
        }

        public long Part1(char[][] block)
        {
            int[] start = GrabStart(block);
            long blockWidth = block[0].Length;
            DeterminePathAndLoop(block, start, false, (current, deltaX, deltaY) => current[0] * blockWidth + current[1], out List<long> positionsVisited);
            return positionsVisited.Count;
        }

        public long Part2(char[][] block)
        {
            int[] start = GrabStart(block);
            long blockWidth = block[0].Length;
            // Take actual path
            DeterminePathAndLoop(block, start, false, (current, deltaX, deltaY) => current[0] * blockWidth + current[1], out List<long> positionsVisited);
            long count = 0;
            foreach (long position in positionsVisited)
            {
                // Check for each position on path if blocker there would create a loop.
                // All positions not on initial path have no impact on path taken.
                int j = (int)(position % blockWidth);
                int i = (int)(position / blockWidth);
                if (block[i][j] == '.')
                {
                    block[i][j] = '#';
                    if (DeterminePathAndLoop(block, start, true, (current, deltaX, deltaY) => (current[0] * blockWidth + current[1]) * 100 + (deltaX + 1) * 10 + (deltaY + 1)))
                    {
                        count++;
                    }
                    block[i][j] = '.';
                }
            }
            // 3.22s => 2.4s
            return count;
        }

        private static int[] GrabStart(char[][] block)
        {
            for (int i = 0; i < block.Length; i++)
            {
                for (int j = 0; j < block[i].Length; j++)
                {
                    if (block[i][j] == '^')
                    {
                        return [i, j];
                    }
                }
            }
            return [0, 0];
        }

        private static bool DeterminePathAndLoop(char[][] block, int[] start, bool stopOnVisitedPosition, Func<int[], int, int, long> posNumCalc)
        {
            bool[] positionsVisited = new bool[block.Length * block[0].Length * 10 * 10];
            int[] current = start;
            long blockWidth = block[0].Length;
            int deltaX = -1;
            int deltaY = 0;
            while (current[0] >= 0 && current[1] >= 0 && current[0] < block.Length && current[1] < blockWidth)
            {
                long posNum = posNumCalc(current, deltaX, deltaY);
                if (!positionsVisited[posNum])
                {
                    positionsVisited[posNum] = true;
                }
                else if (stopOnVisitedPosition)
                {
                    return true;
                }
                int[] next = [current[0] + deltaX, current[1] + deltaY];
                if (next[0] >= 0 && next[1] >= 0 && next[0] < block.Length && next[1] < blockWidth)
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
            return false;
        }

        private static bool DeterminePathAndLoop(char[][] block, int[] start, bool stopOnVisitedPosition, Func<int[], int, int, long> posNumCalc, out List<long> positionsVisited)
        {
            positionsVisited = [];
            int[] current = start;
            long blockWidth = block[0].Length;
            int deltaX = -1;
            int deltaY = 0;
            while (current[0] >= 0 && current[1] >= 0 && current[0] < block.Length && current[1] < blockWidth)
            {
                long posNum = posNumCalc(current, deltaX, deltaY);
                if (!positionsVisited.Contains(posNum))
                {
                    positionsVisited.Add(posNum);
                }
                else if (stopOnVisitedPosition)
                {
                    return true;
                }
                int[] next = [current[0] + deltaX, current[1] + deltaY];
                if (next[0] >= 0 && next[1] >= 0 && next[0] < block.Length && next[1] < blockWidth)
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
            return false;
        }
    }
}
