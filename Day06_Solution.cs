﻿using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode
{
    internal class Day06_Solution : IDaySolution
    {
        public Day06_Solution() { }

        char[][]? block;

        public void LoadData()
        {
            block = lines.Select(line => line.ToCharArray()).ToArray();
        }

        public long Part1()
        {
            if (block == null)
            {
                throw new InvalidOperationException("You have to call LoadData() before Part1()");
            }
            DeterminePathAndLoop(block, false, (current, blockWidth, deltaX, deltaY) => current[0] * blockWidth + current[1], out List<long> positionsVisited);
            return positionsVisited.Count;
        }

        public long Part2()
        {
            if (block == null)
            {
                throw new InvalidOperationException("You have to call LoadData() before Part2()");
            }
            // Take actual path
            DeterminePathAndLoop(block, false, (current, blockWidth, deltaX, deltaY) => current[0] * blockWidth + current[1], out List<long> positionsVisited);
            long blockWidth = block[0].Length;
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
                    if (DeterminePathAndLoop(block, true, (current, blockWidth, deltaX, deltaY) => (current[0] * blockWidth + current[1]) * 100 + deltaX * 10 + deltaY, out List<long> _))
                    {
                        count++;
                    }
                    block[i][j] = '.';
                }
            }
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

        private static bool DeterminePathAndLoop(char[][] block, bool stopOnVisitedPosition, Func<int[], long, int, int, long> posNumCalc, out List<long> positionsVisited)
        {
            positionsVisited = [];
            int[] current = GrabStart(block);
            long blockWidth = block[0].Length;
            int deltaX = -1;
            int deltaY = 0;
            while (current[0] >= 0 && current[1] >= 0 && current[0] < block.Length && current[1] < blockWidth)
            {
                long posNum = posNumCalc(current, blockWidth, deltaX, deltaY);
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

        private static readonly string[] lines = @"..........#.....................#...................#.......................................................................#....#
.........#................................................................#....##......#........##.............#..................
............#..............#..#.................................................................#.................................
..............#.....#................................................................................#...........................#
............................................................................................................#.............#.......
.........#..........##......................................#......................................#......#...#.......#...#.....#.
..#...................................................#.................#....#........#.....#.................#...................
.....#............#.#..#..................#.......................#................................#......#..............#........
#.................................................................#..........#.......................................#.#..........
..##...................#..................................#..#.................................#...#..#.#.#.........#...##........
..#..........................#...............#...............#..........................#.................................#.......
................##......#......#......................##.............#........#................................#.#..#.............
.............................................................#.#.......................................................#..........
.....##.............................................#.......................#...............#...............#..#...#....#.........
.#.........................#..................................#..........#....#...............................................#...
.......................#..#.............#......#.....#..#................................................#....#........#..........
...............#.....................#................#...................................#......#........................#.......
.......................#....#..............#.......#...........................#....................................#.#...........
..#..................#...........#........................#............#...................#......................#.............#.
..............................................#........................................#..............#...........#.........#.....
............#.................#...#..........#.#.....................##.........#................##...........##............#.....
..................#.......#..................................................................#....#..................#.......#....
..........##......#...............................................#.................................#.......#.............#.......
................................................................................#.......................................#.........
.....#........................#.............##......................#.......#.......#...............................#.............
..#.......................................##........#.............#...............................................#...............
.............#.....#..........................................#...................#.................................#..........#..
.........#............#..#....................#............##.............#..................................#....................
........................................................#..........#.............................#................................
......................#..............................................................................................#............
.......#............#..#........................#...............#.......#.............................#......................#....
...................................................................................#........................#........#..#.....#.#.
..#.......#......#...................................................................................#..#.....#................#..
.....#...#......................................................#...................#.#..............................#.#..........
..#.#.....................#....................#...........#.................#........#..#.........#........#..................#..
....#......#.......#.....................................................##...........#...........................................
........................#......#......................#......#.....................................................#.......#......
............................................................#..........................#...#.#..................................#.
.#..........#..............................#.....#.............................................#..................................
#..........................................#......#.............#...........................................#.........#...........
......#...............................#..#......................#.....#.......#.....#..#......................#.......#...........
........................................#..#.................................#......#.......................#.....................
.#................#...........#.........#.............................................................#...........................
.....#.......................#....................................................................................................
.............#................#............................................#...........#......................#..................#
#........................#....#.....................#................#..##.............#......................#...#...............
....#....................#....#.#..................#...........................................................#......##..........
#............#..........##............................................................#...........................................
...........#..................................#..##................#..............................................................
..........................#..............................................................................#...............#........
.......................................................................#...........#...#...........#..#.....#..................#..
.........................#....#.......#......................#....................................#...............................
..#..................#...................................................#......................................#.................
..............#....##........................................#..................#.................................................
..........................#............................................#...................#............................#.........
......#...............................#..........................#.............................#..................................
...................................#.............................................................#..............#.................
.................#...#.......#..............#.#...........................................#.......#.....#.........................
........................#.......................................#..#.........................#...##...#...........................
....#..#.............#........#.............#..........#......#.........................................#....#..#.................
.......................................................................#...........#......#.....................#...........#.....
#.#..............................#................................................................................................
.#.................................................#...................#..........................................................
..............................#.............#.............................#.....#............#....................................
.#...............................#.......#...##.#.................#............................................#..................
.............#....#.................#.........#..............#....#...............................................................
......................#.#..........................................................................#................#.............
.#....................#.....#................#.....................#..................#......................#....................
.............#.#............#......................................#.......................#......................#...............
.#..#...#...................#.....................................................................................................
...........................................................................#.....................#................................
...........#.............................#......#.................................................................................
..............#.................................................#.##........#..........................#.........................#
..................#...#...........................................................................................................
........#.....#......................#..#....#...#.#......................................#.#............#........................
.....#..#.........................................................#......#.....#...........#..................#...#..........#....
......#.........................................#......................#........#...................................#.............
..........................................#........................#........#.............#........#..#...........................
.......#..............#...................#...........................................#.............#.............................
..............#....#....#......#...........................................................#..........................#...........
..................................................................................#.......#...................#...#....#..........
.................#........................#........#...#.....................................................#........#...........
...........................#......^.#..............#.#.....................#.#...........#....##.......#..........................
.............#.....#.........................................................................#..........#......#..#.#.............
...................................................................#.......#.......#...........................................#..
.......#.........#.......................#...................................................................#..........#.....#..#
.....................#...............#...#..#.....................................................#.....##.##..........#..........
.......................#.......#..................................#.................#.....................................#.......
................................................#........#....................#........................#..........................
......#....#..#....#.......................#....#...#.............................#............................#......#...........
...................................................................................................##.#.....#.................#...
.................#...#....................#........................#...............##.....#...........#........#.............#....
.....#............#......................................................................#................#....#..................
........#.............#..................................................................#....#.......#...........................
.#.......................................#.......................................#....#.............................#.............
...#.............................#..................#............................#.....................#.....#...........#........
....#......#.................................................................................................................#....
.....................................#..........#.............#......................................#............................
..#...#....#............#.....................................................#..#................................................
.....#.......................................................#............#..........................#...........#................
#.....................#..............#................................................#..........................#......#........#
........#.........................#........................#................#.......................#.............................
...........................#........#............................#.......#..............#.........................................
......###.........................................................................#.........#........#................#...#.......
............#........................#................#.................#........................#.............#.......#..........
........#.#................................#.....................................................#................................
.#...#............#........#........#..#...............................................#............................#............#
...#.............#.........#....................................#..............#.......................................##.........
...#...........#.....................................#.........#................#......................#............#.............
..................#....................................................##.......##..........#...............................##....
........................#.........#...........#............................................................###....................
....................#.......#....................................................#...#..............................#............#
............................#.....................................................................................................
......#........#........................#..........#..........#.......#..........................#................................
....................................#..............#...#.#......................................................................#.
.....#.....#...#..............................................###......#..............#.........##......#...............#.........
........................#.........................#...............#...............#.#........#.....................#.#............
...............#..............#........#........................................#..........#........#......#......................
.....................#..........................#...............................#........#.......#.....#...................#......
.........#...........................................#.............................#....#.......#......#..#....#..................
.........#...#........#....................#.......#..#...#..............................................................#........
..................#......................................#........................................................................
........#.....................................................#..#...........#........#......................#...#....#.....#.....
...........................#........#....#.........................................................#.............#................
.....#.#.........#................................................................................#...............................
...#............................................#......................#.......#.#...............#............#...................
.........#................................#.....#...........#.#....................#...#.....................................#.#..
..........#.#....#......#.........#....#.....#........#...........................#...............................................
............#...............#............#..........#.............................#...............................................
#.#............#.....................##................................................#....#.................##..................".Split(Environment.NewLine);
    }
}
