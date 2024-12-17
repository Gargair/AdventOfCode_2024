using System.Text.RegularExpressions;

namespace AdventOfCode
{
    internal partial class Day16_Solution : Helper.IDaySolution<char[][], long>
    {
        public char[][] LoadData(string inputPath)
        {
            return File.ReadAllLines(inputPath + "/Day16.txt").Select(line => line.ToCharArray()).ToArray();
        }

        public long Part1(char[][] maze)
        {
            BuildGraph(maze, out MazeNode start, out MazeNode[] ends);
            Helper.DijkstraaSolver.CalcDijkstraa(start);
            return ends.Select(end => end.distance ?? -1).Where(dist => dist != -1).Min();
        }

        public long Part2(char[][] maze)
        {
            BuildGraph(maze, out MazeNode start, out MazeNode[] ends);
            Helper.DijkstraaSolver.CalcDijkstraa(start);
            MarkPaths(maze, ends.Where(end => end.distance.HasValue).MinBy(end => end.distance!.Value)!);
            return maze.Select(column => column.Select(p => p == 'O' ? 1L : 0L).Sum()).Sum();
        }

        private class MazeNode : Helper.DijkstraaSolver.GraphNode
        {
            public required Tuple<int, int, char> mazeElement;
        }

        private static void MarkPaths(char[][] maze, MazeNode end)
        {
            Queue<MazeNode> toDo = [];
            toDo.Enqueue(end);
            while (toDo.Count > 0)
            {
                MazeNode current = toDo.Dequeue();
                maze[current.mazeElement.Item1][current.mazeElement.Item2] = 'O';
                foreach (var pred in current.predecessors)
                {
                    toDo.Enqueue((MazeNode)pred);
                }
            }
        }

        private static void BuildGraph(char[][] maze, out MazeNode start, out MazeNode[] ends)
        {
            int[] startPosition = Helper.MatrixHelper.FindFirstElement(maze, 'S');
            int[] endPosition = Helper.MatrixHelper.FindFirstElement(maze, 'E');

            MazeNode[][][] nodes = maze.Select((row, rowIndex) => row.Select((point, columnIndex) =>
            {
                MazeNode[] pointNodes =
                [
                    new MazeNode()
                    {
                        cost = 0,
                        mazeElement = Tuple.Create(rowIndex, columnIndex, '>')
                    },
                    new MazeNode()
                    {
                        cost = 0,
                        mazeElement = Tuple.Create(rowIndex, columnIndex, 'v')
                    },
                    new MazeNode()
                    {
                        cost = 0,
                        mazeElement = Tuple.Create(rowIndex, columnIndex, '<')
                    },
                    new MazeNode()
                    {
                        cost = 0,
                        mazeElement = Tuple.Create(rowIndex, columnIndex, '^')
                    },
                ];
                return pointNodes;
            }).ToArray()).ToArray();

            for (int i = 0; i < maze.Length; i++)
            {
                for (int j = 0; j < maze[i].Length; j++)
                {
                    MazeNode[] currentNodes = nodes[i][j];
                    if (maze[i][j] != '#')
                    {
                        int[] deltaRight = GetDirectionDelta('>');
                        if (Helper.MatrixHelper.IsInMatrix(maze, i + deltaRight[0], i + deltaRight[1]))
                        {
                            if (maze[i + deltaRight[0]][j + deltaRight[1]] != '#')
                            {
                                MazeNode[] targetNodes = nodes[i + deltaRight[0]][j + deltaRight[1]];
                                Helper.DijkstraaSolver.GraphEdge edge = new Helper.DijkstraaSolver.GraphEdge() { to = targetNodes[0], cost = 1 };
                                currentNodes[0].next.Add(edge);
                            }
                        }
                        int[] deltaBottom = GetDirectionDelta('v');
                        if (Helper.MatrixHelper.IsInMatrix(maze, i + deltaBottom[0], i + deltaBottom[1]))
                        {
                            if (maze[i + deltaBottom[0]][j + deltaBottom[1]] != '#')
                            {
                                MazeNode[] targetNodes = nodes[i + deltaBottom[0]][j + deltaBottom[1]];
                                Helper.DijkstraaSolver.GraphEdge edge = new Helper.DijkstraaSolver.GraphEdge() { to = targetNodes[1], cost = 1 };
                                currentNodes[1].next.Add(edge);
                            }
                        }
                        int[] deltaLeft = GetDirectionDelta('<');
                        if (Helper.MatrixHelper.IsInMatrix(maze, i + deltaLeft[0], i + deltaLeft[1]))
                        {
                            if (maze[i + deltaLeft[0]][j + deltaLeft[1]] != '#')
                            {
                                MazeNode[] targetNodes = nodes[i + deltaLeft[0]][j + deltaLeft[1]];
                                Helper.DijkstraaSolver.GraphEdge edge = new Helper.DijkstraaSolver.GraphEdge() { to = targetNodes[2], cost = 1 };
                                currentNodes[2].next.Add(edge);
                            }
                        }
                        int[] deltaTop = GetDirectionDelta('^');
                        if (Helper.MatrixHelper.IsInMatrix(maze, i + deltaTop[0], i + deltaTop[1]))
                        {
                            if (maze[i + deltaTop[0]][j + deltaTop[1]] != '#')
                            {
                                MazeNode[] targetNodes = nodes[i + deltaTop[0]][j + deltaTop[1]];
                                Helper.DijkstraaSolver.GraphEdge edge = new Helper.DijkstraaSolver.GraphEdge() { to = targetNodes[3], cost = 1 };
                                currentNodes[3].next.Add(edge);
                            }
                        }
                        currentNodes[0].next.Add(new Helper.DijkstraaSolver.GraphEdge() { to = currentNodes[1], cost = 1000 });
                        currentNodes[0].next.Add(new Helper.DijkstraaSolver.GraphEdge() { to = currentNodes[3], cost = 1000 });
                        currentNodes[1].next.Add(new Helper.DijkstraaSolver.GraphEdge() { to = currentNodes[0], cost = 1000 });
                        currentNodes[1].next.Add(new Helper.DijkstraaSolver.GraphEdge() { to = currentNodes[2], cost = 1000 });
                        currentNodes[2].next.Add(new Helper.DijkstraaSolver.GraphEdge() { to = currentNodes[1], cost = 1000 });
                        currentNodes[2].next.Add(new Helper.DijkstraaSolver.GraphEdge() { to = currentNodes[3], cost = 1000 });
                        currentNodes[3].next.Add(new Helper.DijkstraaSolver.GraphEdge() { to = currentNodes[0], cost = 1000 });
                        currentNodes[3].next.Add(new Helper.DijkstraaSolver.GraphEdge() { to = currentNodes[2], cost = 1000 });
                    }
                }
            }

            start = nodes[startPosition[0]][startPosition[1]][0];
            ends = nodes[endPosition[0]][endPosition[1]];
        }

        private static long? DeterminePoints(char[][] maze, int[] start, int[] end, char direction, Dictionary<Tuple<int, int, char>, long?> cache, long depth)
        {
            Console.WriteLine("Depth: " + depth);
            if (start[0] == end[0] && start[1] == end[1])
            {
                return 0;
            }
            Tuple<int, int, char> current = Tuple.Create(start[0], start[1], direction);
            if (cache.TryGetValue(current, out long? min))
            {
                return min;
            }

            int[] delta = GetDirectionDelta(direction);
            int[] next = [start[0] + delta[0], start[1] + delta[1]];

            long? minimum = null;

            if (Helper.MatrixHelper.IsInMatrix(maze, next[0], next[1]))
            {
                if (maze[next[0]][next[1]] != '#')
                {
                    long? potentialPoints = DeterminePoints(maze, next, end, direction, cache, depth + 1);
                    if (potentialPoints.HasValue && (!minimum.HasValue || potentialPoints.Value + 1 < minimum.Value))
                    {
                        minimum = potentialPoints + 1;
                    }
                }
            }

            char directionLeft = TurnLeft(direction);
            int[] deltaLeft = GetDirectionDelta(directionLeft);
            int[] leftNext = [start[0] + deltaLeft[0], start[1] + deltaLeft[1]];

            if (Helper.MatrixHelper.IsInMatrix(maze, leftNext[0], leftNext[1]))
            {
                if (maze[leftNext[0]][leftNext[1]] != '#')
                {
                    long? potentialPoints = DeterminePoints(maze, start, end, directionLeft, cache, depth + 1);
                    if (potentialPoints.HasValue && (!minimum.HasValue || potentialPoints.Value + 1000 < minimum.Value))
                    {
                        minimum = potentialPoints + 1000;
                    }
                }
            }

            char directionRight = TurnRight(direction);
            int[] deltaRight = GetDirectionDelta(directionRight);
            int[] rightNext = [start[0] + deltaRight[0], start[1] + deltaRight[1]];

            if (Helper.MatrixHelper.IsInMatrix(maze, rightNext[0], rightNext[1]))
            {
                if (maze[rightNext[0]][rightNext[1]] != '#')
                {
                    long? potentialPoints = DeterminePoints(maze, start, end, directionRight, cache, depth + 1);
                    if (potentialPoints.HasValue && (!minimum.HasValue || potentialPoints.Value + 1000 < minimum.Value))
                    {
                        minimum = potentialPoints + 1000;
                    }
                }
            }

            cache.TryAdd(current, minimum);

            return minimum;
        }

        private static int[] GetDirectionDelta(char direction)
        {
            return direction switch
            {
                '^' => [-1, 0],
                '>' => [0, 1],
                'v' => [1, 0],
                '<' => [0, -1],
                _ => [0, 0],
            };
        }

        private static char TurnLeft(char direction)
        {
            return direction switch
            {
                '^' => '<',
                '>' => '^',
                'v' => '>',
                '<' => 'v',
                _ => '>'
            };
        }

        private static char TurnRight(char direction)
        {
            return direction switch
            {
                '^' => '>',
                '>' => 'v',
                'v' => '<',
                '<' => '^',
                _ => '>'
            };
        }
    }
}
