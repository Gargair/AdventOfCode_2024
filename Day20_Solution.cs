using static AdventOfCode.Helper.DijkstraaSolver;

namespace AdventOfCode
{
    internal partial class Day20_Solution : Helper.IDaySolution<Day20_Input, long>
    {
        public Day20_Input LoadData(string inputFolder)
        {
            string[] lines = File.ReadAllLines(inputFolder + "/Day20.txt");

            return new()
            {
                maze = lines.Skip(1).Select(r => r.ToCharArray()).ToArray(),
                minsaved = int.Parse(lines[0])
            };
        }

        public long Part1(Day20_Input input)
        {
            long? defaultRoute = ShortestPathLength(input.maze, []);
            if (!defaultRoute.HasValue)
            {
                throw new Exception("Unsolvable");
            }
            List<Tuple<int, int>> shortestPathElements = DetermineShortestPathElements(input.maze);
            Dictionary<long, int> saved = [];
            List<List<int[]>> calculatedCheats = [];
            long amount = 0;
            foreach (var current in shortestPathElements)
            {
                foreach (var possibleCheat in GenerateCheats(input.maze, [current.Item1, current.Item2], 2, [0, 0]))
                {
                    if (HasWallEndAvailable(input.maze, possibleCheat))
                    {
                        if (calculatedCheats.Any(calc => calc.Zip(possibleCheat).All(pair => pair.First[0] == pair.Second[0] && pair.First[1] == pair.Second[1])))
                        {
                            continue;
                        }
                        long? dist = ShortestPathLength(input.maze, possibleCheat);
                        if (dist.HasValue && dist.Value < defaultRoute.Value)
                        {
                            long savedTime = defaultRoute.Value - dist.Value;
                            //Console.WriteLine($"[{i},{j}] => {string.Join(" => ", possibleCheat.Select(p => $"[{p[0]},{p[1]}]"))}: {savedTime}");
                            //Console.WriteLine($"{string.Join(" => ", possibleCheat.Select(p => $"[{p[0]},{p[1]}]"))}: {savedTime}");
                            if (savedTime >= input.minsaved)
                            {
                                saved.TryAdd(savedTime, 0);
                                saved[savedTime] += 1;
                                amount += 1;
                                calculatedCheats.Add(possibleCheat);
                            }
                        }
                    }
                }
            }
            //Console.WriteLine();
            //foreach (var pair in saved.OrderBy(pair => pair.Key))
            //{
            //    Console.WriteLine($"There are {pair.Value} cheats that save {pair.Key} picoseconds.");
            //}
            return amount;
            //return saved.Where(pair => pair.Key >= input.minsaved).Select(pair => pair.Value).Sum();
        }

        public long Part2(Day20_Input input)
        {
            return 0;
        }

        private static long? ShortestPathLength(char[][] maze, List<int[]> cheatFields)
        {
            int[] start = Helper.MatrixHelper.FindFirstElement(maze, 'S');
            int[] end = Helper.MatrixHelper.FindFirstElement(maze, 'E');

            int?[,] distances = new int?[maze.Length, maze[0].Length];

            PriorityQueue<Tuple<int, int>, int> queue = new();
            queue.Enqueue(Tuple.Create(start[0], start[1]), 0);
            distances[start[0], start[1]] = 0;

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                var currentDistance = distances[current.Item1, current.Item2];

                if (!currentDistance.HasValue)
                {
                    throw new Exception("Got node which wasn't visited");
                }
                //if()
                CheckAndMove(maze, current, [0, 1], cheatFields, currentDistance.Value, distances, queue);
                CheckAndMove(maze, current, [1, 0], cheatFields, currentDistance.Value, distances, queue);
                CheckAndMove(maze, current, [0, -1], cheatFields, currentDistance.Value, distances, queue);
                CheckAndMove(maze, current, [-1, 0], cheatFields, currentDistance.Value, distances, queue);
            }
            return distances[end[0], end[1]];
        }

        private static bool IsFree(char[][] maze, Tuple<int, int> current, int[] delta)
        {
            if (Helper.MatrixHelper.IsInMatrix(maze, current.Item1 + delta[0], current.Item2 + delta[1]))
            {
                if (maze[current.Item1 + delta[0]][current.Item2 + delta[1]] != '#')
                {
                    return true;
                }
            }
            return false;
        }

        private static IEnumerable<List<int[]>> GenerateCheats(char[][] maze, int[] start, int cheatFields, int[] blocked)
        {
            if (start[0] > 0 && (blocked[0] != -1 || blocked[1] != 0))
            {
                var cheatList = new List<int[]>() { new int[] { start[0] - 1, start[1] } };
                if (cheatFields == 1)
                {
                    if (maze[start[0] - 1][start[1]] != '#')
                    {
                        yield return cheatList;
                    }
                }
                else
                {
                    var inner = GenerateCheats(maze, [start[0] - 1, start[1]], cheatFields - 1, [1, 0]);
                    foreach (var f in inner)
                    {
                        yield return cheatList.Concat(f).ToList();
                    }
                }
            }
            if (start[0] < maze.Length - 1 && (blocked[0] != 1 || blocked[1] != 0))
            {
                var cheatList = new List<int[]>() { new int[] { start[0] + 1, start[1] } };
                if (cheatFields == 1)
                {
                    if (maze[start[0] + 1][start[1]] != '#')
                    {
                        yield return cheatList;
                    }
                }
                else
                {
                    var inner = GenerateCheats(maze, [start[0] + 1, start[1]], cheatFields - 1, [-1, 0]);
                    foreach (var f in inner)
                    {
                        yield return cheatList.Concat(f).ToList();
                    }
                }
            }
            if (start[1] > 0 && (blocked[0] != 0 || blocked[1] != -1))
            {
                var cheatList = new List<int[]>() { new int[] { start[0], start[1] - 1 } };
                if (cheatFields == 1)
                {
                    if (maze[start[0]][start[1] - 1] != '#')
                    {
                        yield return cheatList;
                    }
                }
                else
                {
                    var inner = GenerateCheats(maze, [start[0], start[1] - 1], cheatFields - 1, [0, 1]);
                    foreach (var f in inner)
                    {
                        yield return cheatList.Concat(f).ToList();
                    }
                }
            }
            if (start[1] < maze[start[0]].Length - 1 && (blocked[0] != 0 || blocked[1] != 1))
            {
                var cheatList = new List<int[]>() { new int[] { start[0], start[1] + 1 } };
                if (cheatFields == 1)
                {
                    if (maze[start[0]][start[1] + 1] != '#')
                    {
                        yield return cheatList;
                    }
                }
                else
                {
                    var inner = GenerateCheats(maze, [start[0], start[1] + 1], cheatFields - 1, [0, -1]);
                    foreach (var f in inner)
                    {
                        yield return cheatList.Concat(f).ToList();
                    }
                }
            }
        }

        private static bool HasWallEndAvailable(char[][] maze, List<int[]> path)
        {
            if (path.Count == 0)
            {
                return false;
            }
            int[] last = path[^1];

            if (maze[last[0]][last[1]] == '#')
            {
                return false;
            }
            foreach (var el in path)
            {
                if (maze[el[0]][el[1]] == '#')
                {
                    return true;
                }
            }
            return false;
        }

        private static void CheckAndMove(char[][] maze, Tuple<int, int> current, int[] delta, List<int[]> cheatFields, int currentDistance, int?[,] distances, PriorityQueue<Tuple<int, int>, int> queue)
        {
            if (IsCheatStart(current, delta, cheatFields))
            {
                for (int i = 0; i < cheatFields.Count - 1; i++)
                {
                    if (!distances[cheatFields[i][0], cheatFields[i][1]].HasValue || distances[cheatFields[i][0], cheatFields[i][1]] > currentDistance + i + 1)
                    {
                        distances[cheatFields[i][0], cheatFields[i][1]] = currentDistance + i + 1;
                    }
                }
                if (!distances[cheatFields[^1][0], cheatFields[^1][1]].HasValue || distances[cheatFields[^1][0], cheatFields[^1][1]] > currentDistance + cheatFields.Count)
                {

                    distances[cheatFields[^1][0], cheatFields[^1][1]] = currentDistance + cheatFields.Count;
                    queue.Enqueue(Tuple.Create(cheatFields[^1][0], cheatFields[^1][1]), currentDistance + cheatFields.Count);
                }
            }
            else if (IsFree(maze, current, delta))
            {
                if (!distances[current.Item1 + delta[0], current.Item2 + delta[1]].HasValue || distances[current.Item1 + delta[0], current.Item2 + delta[1]] > currentDistance + 1)
                {
                    distances[current.Item1 + delta[0], current.Item2 + delta[1]] = currentDistance + 1;
                    queue.Enqueue(Tuple.Create(current.Item1 + delta[0], current.Item2 + delta[1]), currentDistance + 1);
                }
            }
        }

        private static bool IsCheatStart(Tuple<int, int> current, int[] delta, List<int[]> cheatFields)
        {
            if (cheatFields.Count > 0)
            {
                var first = cheatFields[0];
                return first[0] == current.Item1 + delta[0] && first[1] == current.Item2 + delta[1];
            }
            return false;
        }

        private static List<Tuple<int, int>> DetermineShortestPathElements(char[][] maze)
        {
            int[] start = Helper.MatrixHelper.FindFirstElement(maze, 'S');
            int[] end = Helper.MatrixHelper.FindFirstElement(maze, 'E');

            MazeNode[][] nodes = new MazeNode[maze.Length][];

            for (int i = 0; i < maze.Length; i++)
            {
                nodes[i] = new MazeNode[maze[i].Length];
                for (int j = 0; j < maze[i].Length; j++)
                {
                    nodes[i][j] = new MazeNode() { cost = 1, Position = [i, j] };
                }
            }
            for (int i = 0; i < maze.Length; i++)
            {
                for (int j = 0; j < maze[i].Length; j++)
                {
                    if (i > 0 && maze[i - 1][j] != '#')
                    {
                        nodes[i][j].next.Add(new() { to = nodes[i - 1][j] });
                    }
                    if (i < maze.Length - 1 && maze[i + 1][j] != '#')
                    {
                        nodes[i][j].next.Add(new() { to = nodes[i + 1][j] });
                    }
                    if (j > 0 && maze[i][j - 1] != '#')
                    {
                        nodes[i][j].next.Add(new() { to = nodes[i][j - 1] });
                    }
                    if (j < maze[i].Length - 1 && maze[i][j + 1] != '#')
                    {
                        nodes[i][j].next.Add(new() { to = nodes[i][j + 1] });
                    }
                }
            }
            CalcDijkstraa(nodes[start[0]][start[1]]);
            List<Tuple<int, int>> pathElements = [];
            Queue<MazeNode> queue = new();
            queue.Enqueue(nodes[end[0]][end[1]]);
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                var pathElement = Tuple.Create(current.Position[0], current.Position[1]);
                if (!pathElements.Contains(pathElement))
                {
                    pathElements.Add(pathElement);
                    foreach (MazeNode pred in current.predecessors)
                    {
                        queue.Enqueue(pred);
                    }
                }
            }

            return pathElements;
        }

        private class MazeNode : GraphNode
        {
            public required int[] Position;
        }
    }

    public class Day20_Input
    {
        public required char[][] maze;
        public required int minsaved;
    }
}
