using System.Text.RegularExpressions;
using static AdventOfCode.Helper.DijkstraaSolver;

namespace AdventOfCode
{
    internal partial class Day18_Solution : Helper.IDaySolution
    {
        public Day18_Input LoadData(string inputFolder)
        {
            // Input in File needs to be prepended with a line stating the size of the field and the amount of bytes to use for part 1.
            // f.e. 
            // 7,12
            string[] lines = File.ReadAllLines(inputFolder + "/Day18.txt");

            string firstLine = lines.First();
            var lineM = PositionParser().Match(firstLine);

            return new()
            {
                Size = int.Parse(lineM.Groups["sizeX"].Value),
                Part1Amount = int.Parse(lineM.Groups["sizeY"].Value),
                Positions = lines.Skip(1).Select(line =>
                {
                    var m = PositionParser().Match(line);
                    return new int[] { int.Parse(m.Groups["sizeX"].Value), int.Parse(m.Groups["sizeY"].Value) };
                })
            };
        }

        public override string Part1(string inputPath)
        {
            Day18_Input input = LoadData(inputPath);
            bool[,] field = new bool[input.Size, input.Size];
            for (int i = 0; i < input.Part1Amount; i++)
            {
                var pos = input.Positions.ElementAt(i);
                field[pos[0], pos[1]] = true;
            }
            BuildGraph(field, out var start, out var end);
            CalcDijkstraa(start);
            return end.distance!.Value.ToString();
        }

        public override string Part2(string inputPath)
        {
            Day18_Input input = LoadData(inputPath);
            bool[,] field = new bool[input.Size, input.Size];
            foreach (int[] pos in input.Positions)
            {
                field[pos[0], pos[1]] = true;
                BuildGraph(field, out var start, out var end);
                CalcDijkstraa(start);
                if (!end.distance.HasValue)
                {
                    return $"{pos[0]},{pos[1]}";
                }
            }

            return string.Empty;
        }

        [GeneratedRegex("(?'sizeX'\\d+),(?'sizeY'\\d+)")]
        private static partial Regex PositionParser();

        private static void BuildGraph(bool[,] maze, out GraphNode start, out GraphNode end)
        {
            GraphNode[][] nodes = Enumerable.Range(0, maze.GetLength(0)).Select(rowIndex => Enumerable.Range(0, maze.GetLength(1)).Select(columnIndex =>
            {
                return new GraphNode()
                {
                    cost = 0
                };
            }).ToArray()).ToArray();

            start = nodes[0][0];
            end = nodes[nodes.Length - 1][nodes[0].Length - 1];

            for (int i = 0; i < maze.GetLength(0); i++)
            {
                for (int j = 0; j < maze.GetLength(1); j++)
                {
                    GraphNode currentNode = nodes[i][j];

                    if (!maze[i, j])
                    {
                        int[] deltaRight = [0, 1];
                        if (Helper.MatrixHelper.IsInMatrix(maze, i + deltaRight[0], j + deltaRight[1]))
                        {
                            if (!maze[i + deltaRight[0], j + deltaRight[1]])
                            {
                                GraphNode targetNode = nodes[i + deltaRight[0]][j + deltaRight[1]];
                                Helper.DijkstraaSolver.GraphEdge edge = new Helper.DijkstraaSolver.GraphEdge() { from = currentNode, to = targetNode, cost = 1 };
                                currentNode.next.Add(edge);
                            }
                        }
                        int[] deltaBottom = [1, 0];
                        if (Helper.MatrixHelper.IsInMatrix(maze, i + deltaBottom[0], j + deltaBottom[1]))
                        {
                            if (!maze[i + deltaBottom[0], j + deltaBottom[1]])
                            {
                                GraphNode targetNode = nodes[i + deltaBottom[0]][j + deltaBottom[1]];
                                Helper.DijkstraaSolver.GraphEdge edge = new Helper.DijkstraaSolver.GraphEdge() { from = currentNode, to = targetNode, cost = 1 };
                                currentNode.next.Add(edge);
                            }
                        }
                        int[] deltaLeft = [0, -1];
                        if (Helper.MatrixHelper.IsInMatrix(maze, i + deltaLeft[0], j + deltaLeft[1]))
                        {
                            if (!maze[i + deltaLeft[0], j + deltaLeft[1]])
                            {
                                GraphNode targetNode = nodes[i + deltaLeft[0]][j + deltaLeft[1]];
                                Helper.DijkstraaSolver.GraphEdge edge = new Helper.DijkstraaSolver.GraphEdge() { from = currentNode, to = targetNode, cost = 1 };
                                currentNode.next.Add(edge);
                            }
                        }
                        int[] deltaTop = [-1, 0];
                        if (Helper.MatrixHelper.IsInMatrix(maze, i + deltaTop[0], j + deltaTop[1]))
                        {
                            if (!maze[i + deltaTop[0], j + deltaTop[1]])
                            {
                                GraphNode targetNode = nodes[i + deltaTop[0]][j + deltaTop[1]];
                                Helper.DijkstraaSolver.GraphEdge edge = new Helper.DijkstraaSolver.GraphEdge() { from = currentNode, to = targetNode, cost = 1 };
                                currentNode.next.Add(edge);
                            }
                        }
                    }
                }
            }
        }
    }

    public class Day18_Input
    {
        public required int Size;
        public required int Part1Amount;
        public required IEnumerable<int[]> Positions;
    }
}
