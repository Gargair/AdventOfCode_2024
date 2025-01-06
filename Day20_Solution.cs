using static AdventOfCode.Helper.DijkstraaSolver;

namespace AdventOfCode
{
    internal partial class Day20_Solution : Helper.IDaySolution
    {
        public Day20_Input LoadData(string inputFolder)
        {
            string[] lines = File.ReadAllLines(inputFolder + "/Day20.txt");

            return new()
            {
                maze = lines.Skip(1).Select(r => r.ToCharArray()).ToArray(),
                minSavedPart1 = int.Parse(lines[0].Split(',')[0]),
                minSavedPart2 = int.Parse(lines[0].Split(',')[1]),
            };
        }

        public override string Part1(string inputPath)
        {
            Day20_Input input = LoadData(inputPath);
            var maze = input.maze;

            Tuple<int, int>? start = Helper.MatrixHelper.FindFirstElement(maze, 'S');
            Tuple<int, int>? end = Helper.MatrixHelper.FindFirstElement(maze, 'E');

            if (start == null)
            {
                throw new Exception("Did not find start node");
            }
            if (end == null)
            {
                throw new Exception("Did not find end node");
            }

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
                        nodes[i][j].next.Add(new GraphEdge() { from = nodes[i][j], to = nodes[i - 1][j] });
                    }
                    if (i < maze.Length - 1 && maze[i + 1][j] != '#')
                    {
                        nodes[i][j].next.Add(new GraphEdge() { from = nodes[i][j], to = nodes[i + 1][j] });
                    }
                    if (j > 0 && maze[i][j - 1] != '#')
                    {
                        nodes[i][j].next.Add(new GraphEdge() { from = nodes[i][j], to = nodes[i][j - 1] });
                    }
                    if (j < maze[i].Length - 1 && maze[i][j + 1] != '#')
                    {
                        nodes[i][j].next.Add(new GraphEdge() { from = nodes[i][j], to = nodes[i][j + 1] });
                    }
                }
            }
            CalcDijkstraa(nodes[end.Item1][end.Item2]);

            MazeNode startNode = nodes[start.Item1][start.Item2];
            List<Tuple<int, int>> shortestPathElements = new(startNode.distance!.Value > int.MaxValue ? int.MaxValue : (int)startNode.distance!.Value);
            Queue<MazeNode> queue = new();
            queue.Enqueue(startNode);
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                var pathElement = Tuple.Create(current.Position[0], current.Position[1]);
                if (!shortestPathElements.Contains(pathElement))
                {
                    shortestPathElements.Add(pathElement);
                    foreach (IGraphEdge pred in current.predecessors)
                    {
                        queue.Enqueue((MazeNode)pred.From);
                    }
                }
            }

            return shortestPathElements
                     .AsParallel()
                     .WithDegreeOfParallelism(Environment.ProcessorCount)
                     .SelectMany(current => GenerateCheats(input.maze, current, 2, NoMove, true))
                     .Where(cheat =>
                     {
                         var startNode = nodes[cheat[0].Item1][cheat[0].Item2];
                         var endNode = nodes[cheat[^1].Item1][cheat[^1].Item2];
                         var cheatLength = cheat.Count;
                         long? savedTime = startNode.distance - endNode.distance - cheatLength + 1;
                         if (savedTime.HasValue && savedTime.Value >= input.minSavedPart1)
                         {
                             return true;
                         }
                         return false;
                     })
                     .Select(cheat => Tuple.Create(cheat[0], cheat[^1]))
                     .Distinct()
                     .Count().ToString();
        }

        public override string Part2(string inputPath)
        {
            Day20_Input input = LoadData(inputPath);
            var maze = input.maze;

            Tuple<int, int>? start = Helper.MatrixHelper.FindFirstElement(maze, 'S');
            Tuple<int, int>? end = Helper.MatrixHelper.FindFirstElement(maze, 'E');

            if (start == null)
            {
                throw new Exception("Did not find start node");
            }
            if (end == null)
            {
                throw new Exception("Did not find end node");
            }

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
                        nodes[i][j].next.Add(new GraphEdge() { from = nodes[i][j], to = nodes[i - 1][j] });
                    }
                    if (i < maze.Length - 1 && maze[i + 1][j] != '#')
                    {
                        nodes[i][j].next.Add(new GraphEdge() { from = nodes[i][j], to = nodes[i + 1][j] });
                    }
                    if (j > 0 && maze[i][j - 1] != '#')
                    {
                        nodes[i][j].next.Add(new GraphEdge() { from = nodes[i][j], to = nodes[i][j - 1] });
                    }
                    if (j < maze[i].Length - 1 && maze[i][j + 1] != '#')
                    {
                        nodes[i][j].next.Add(new GraphEdge() { from = nodes[i][j], to = nodes[i][j + 1] });
                    }
                }
            }
            CalcDijkstraa(nodes[end.Item1][end.Item2]);

            MazeNode startNode = nodes[start.Item1][start.Item2];
            List<Tuple<int, int>> shortestPathElements = new(startNode.distance!.Value > int.MaxValue ? int.MaxValue : (int)startNode.distance!.Value);
            Queue<MazeNode> queue = new();
            queue.Enqueue(startNode);
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                var pathElement = Tuple.Create(current.Position[0], current.Position[1]);
                if (!shortestPathElements.Contains(pathElement))
                {
                    shortestPathElements.Add(pathElement);
                    foreach (IGraphEdge pred in current.predecessors)
                    {
                        queue.Enqueue((MazeNode)pred.From);
                    }
                }
            }

            return shortestPathElements
                     .AsParallel()
                     .WithDegreeOfParallelism(Environment.ProcessorCount)
                     .SelectMany(current => Enumerable.Range(2, 19)
                                        .AsParallel()
                                        .WithDegreeOfParallelism(Environment.ProcessorCount)
                                        .SelectMany(a => GenerateCheats(input.maze, current, a, NoMove, true)))
                     .Where(cheat =>
                     {
                         var startNode = nodes[cheat[0].Item1][cheat[0].Item2];
                         var endNode = nodes[cheat[^1].Item1][cheat[^1].Item2];
                         var cheatLength = cheat.Count;
                         long? savedTime = startNode.distance - endNode.distance - cheatLength + 1;
                         if (savedTime.HasValue && savedTime.Value >= input.minSavedPart2)
                         {
                             return true;
                         }
                         return false;
                     })
                     .Select(cheat => Tuple.Create(cheat[0], cheat[^1]))
                     .Distinct()
                     .Count().ToString();
        }

        private static long? ShortestPathLength(char[][] maze)
        {
            Tuple<int, int>? start = Helper.MatrixHelper.FindFirstElement(maze, 'S');
            Tuple<int, int>? end = Helper.MatrixHelper.FindFirstElement(maze, 'E');

            if (start == null)
            {
                throw new Exception("Did not find start node");
            }
            if (end == null)
            {
                throw new Exception("Did not find end node");
            }

            int?[,] distances = new int?[maze.Length, maze[0].Length];

            PriorityQueue<Tuple<int, int>, int> queue = new();
            queue.Enqueue(start, 0);
            distances[start.Item1, start.Item2] = 0;

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                var currentDistance = distances[current.Item1, current.Item2];

                if (!currentDistance.HasValue)
                {
                    throw new Exception("Got node which wasn't visited");
                }
                CheckAndMove(maze, current, Right, currentDistance.Value, distances, queue);
                CheckAndMove(maze, current, Bottom, currentDistance.Value, distances, queue);
                CheckAndMove(maze, current, Left, currentDistance.Value, distances, queue);
                CheckAndMove(maze, current, Top, currentDistance.Value, distances, queue);
            }
            return distances[end.Item1, end.Item2];
        }

        private static bool IsFree(char[][] maze, Tuple<int, int> current, Tuple<int, int> delta)
        {
            if (Helper.MatrixHelper.IsInMatrix(maze, current.Item1 + delta.Item1, current.Item2 + delta.Item2))
            {
                if (maze[current.Item1 + delta.Item1][current.Item2 + delta.Item2] != '#')
                {
                    return true;
                }
            }
            return false;
        }

        private static IEnumerable<List<Tuple<int, int>>> GenerateCheats(char[][] maze, Tuple<int, int> start, int cheatFields, Tuple<int, int> blocked, bool needsWall)
        {
            if (start.Item1 > 0 && blocked != Top)
            {
                var next = Tuple.Create(start.Item1 - 1, start.Item2);
                if (cheatFields == 1)
                {
                    if (maze[start.Item1 - 1][start.Item2] != '#')
                    {
                        yield return new List<Tuple<int, int>>(2) { start, next };
                    }
                }
                else if (!needsWall || maze[start.Item1 - 1][start.Item2] == '#')
                {
                    var inner = GenerateCheats(maze, next, cheatFields - 1, Bottom, false);
                    foreach (var f in inner)
                    {
                        yield return new List<Tuple<int, int>>(cheatFields) { start }.Concat(f).ToList();
                    }
                }
            }
            if (start.Item1 < maze.Length - 1 && blocked != Bottom)
            {
                var next = Tuple.Create(start.Item1 + 1, start.Item2);
                if (cheatFields == 1)
                {
                    if (maze[start.Item1 + 1][start.Item2] != '#')
                    {
                        yield return new List<Tuple<int, int>>(2) { start, next };
                    }
                }
                else if (!needsWall || maze[start.Item1 + 1][start.Item2] == '#')
                {
                    var inner = GenerateCheats(maze, next, cheatFields - 1, Top, false);
                    foreach (var f in inner)
                    {
                        yield return new List<Tuple<int, int>>(cheatFields) { start }.Concat(f).ToList();
                    }
                }
            }
            if (start.Item2 > 0 && blocked != Left)
            {
                var next = Tuple.Create(start.Item1, start.Item2 - 1);
                if (cheatFields == 1)
                {
                    if (maze[start.Item1][start.Item2 - 1] != '#')
                    {
                        yield return new List<Tuple<int, int>>(2) { start, next };
                    }
                }
                else if (!needsWall || maze[start.Item1][start.Item2 - 1] == '#')
                {
                    var inner = GenerateCheats(maze, Tuple.Create(start.Item1, start.Item2 - 1), cheatFields - 1, Right, false);
                    foreach (var f in inner)
                    {
                        yield return new List<Tuple<int, int>>(cheatFields) { start }.Concat(f).ToList();
                    }
                }
            }
            if (start.Item2 < maze[start.Item1].Length - 1 && blocked != Right)
            {
                var next = Tuple.Create(start.Item1, start.Item2 + 1);
                if (cheatFields == 1)
                {
                    if (maze[start.Item1][start.Item2 + 1] != '#')
                    {
                        yield return new List<Tuple<int, int>>(2) { start, next };
                    }
                }
                else if (!needsWall || maze[start.Item1][start.Item2 + 1] == '#')
                {
                    var inner = GenerateCheats(maze, Tuple.Create(start.Item1, start.Item2 + 1), cheatFields - 1, Left, false);
                    foreach (var f in inner)
                    {
                        yield return new List<Tuple<int, int>>(cheatFields) { start }.Concat(f).ToList();
                    }
                }
            }
        }

        private static void CheckAndMove(char[][] maze, Tuple<int, int> current, Tuple<int, int> delta, int currentDistance, int?[,] distances, PriorityQueue<Tuple<int, int>, int> queue)
        {
            if (IsFree(maze, current, delta))
            {
                if (!distances[current.Item1 + delta.Item1, current.Item2 + delta.Item2].HasValue || distances[current.Item1 + delta.Item1, current.Item2 + delta.Item2] > currentDistance + 1)
                {
                    distances[current.Item1 + delta.Item1, current.Item2 + delta.Item2] = currentDistance + 1;
                    queue.Enqueue(Tuple.Create(current.Item1 + delta.Item1, current.Item2 + delta.Item2), currentDistance + 1);
                }
            }
        }

        private static List<Tuple<int, int>> DetermineShortestPathElements(char[][] maze)
        {
            Tuple<int, int>? start = Helper.MatrixHelper.FindFirstElement(maze, 'S');
            Tuple<int, int>? end = Helper.MatrixHelper.FindFirstElement(maze, 'E');

            if (start == null)
            {
                throw new Exception("Did not find start node");
            }
            if (end == null)
            {
                throw new Exception("Did not find end node");
            }

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
                        nodes[i][j].next.Add(new GraphEdge() { from = nodes[i][j], to = nodes[i - 1][j] });
                    }
                    if (i < maze.Length - 1 && maze[i + 1][j] != '#')
                    {
                        nodes[i][j].next.Add(new GraphEdge() { from = nodes[i][j], to = nodes[i + 1][j] });
                    }
                    if (j > 0 && maze[i][j - 1] != '#')
                    {
                        nodes[i][j].next.Add(new GraphEdge() { from = nodes[i][j], to = nodes[i][j - 1] });
                    }
                    if (j < maze[i].Length - 1 && maze[i][j + 1] != '#')
                    {
                        nodes[i][j].next.Add(new GraphEdge() { from = nodes[i][j], to = nodes[i][j + 1] });
                    }
                }
            }
            CalcDijkstraa(nodes[start.Item1][start.Item2]);
            MazeNode endNode = nodes[end.Item1][end.Item2];
            List<Tuple<int, int>> pathElements = new(endNode.distance!.Value > int.MaxValue ? int.MaxValue : (int)endNode.distance!.Value);
            Queue<MazeNode> queue = new();
            queue.Enqueue(endNode);
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

        private static readonly Tuple<int, int> NoMove = Tuple.Create(0, 0);
        private static readonly Tuple<int, int> Top = Tuple.Create(-1, 0);
        private static readonly Tuple<int, int> Bottom = Tuple.Create(1, 0);
        private static readonly Tuple<int, int> Left = Tuple.Create(0, -1);
        private static readonly Tuple<int, int> Right = Tuple.Create(0, 1);

        private class MazeNode : GraphNode
        {
            public required int[] Position;
        }
    }

    public class Day20_Input
    {
        public required char[][] maze;
        public required int minSavedPart1;
        public required int minSavedPart2;
    }
}
