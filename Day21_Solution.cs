using AdventOfCode.Helper;
using static AdventOfCode.Helper.DijkstraaSolver;

namespace AdventOfCode
{
    internal partial class Day21_Solution : IDaySolution
    {
        public Day21_Input LoadData(string inputFolder)
        {
            string[] lines = File.ReadAllLines(inputFolder + "/Day21.txt");



            return new()
            {
                inputs = lines,
                arrowPaths = GetArrowPaths(),
                numberPaths = GetNumberPaths()
            };
        }

        public override string Part1(string inputPath)
        {
            Day21_Input input = LoadData(inputPath);
            // Need to be more performant to beat part 2 reasonable
            long sum = 0;
            foreach (string line in input.inputs)
            {
                //Console.WriteLine(line);
                int minimum = 0;
                foreach (string numberOutput in WorkInput(line, input.numberPaths))
                {
                    foreach (string arrow1Out in WorkInput(numberOutput, input.arrowPaths))
                    {
                        foreach (string arrow2Out in WorkInput(arrow1Out, input.arrowPaths))
                        {
                            if (minimum == 0 || arrow2Out.Length < minimum)
                            {
                                minimum = arrow2Out.Length;
                            }
                        }
                    }
                }
                int number = int.Parse(line[..^1]);
                //Console.WriteLine($"\t{number} * {minimum} = {number * minimum}");
                sum += number * minimum;
            }
            return sum.ToString();
        }

        public override string Part2(string inputPath)
        {
            Day21_Input input = LoadData(inputPath);
            return "";
        }

        private static IEnumerable<string> WorkInput(string line, Dictionary<char, Dictionary<char, char[][]>> dict)
        {
            char previous = 'A';
            string[] outputs = [""];
            foreach (char c in line)
            {
                Dictionary<char, char[][]> current = dict[previous];
                string[] results = current[c].Select(c => string.Join("", c)).ToArray();
                string[] partial = outputs.SelectMany(s => results.Select(r => s + "A" + r)).ToArray();
                outputs = partial;
                previous = c;
            }
            return outputs.Select(o => string.Concat(o.AsSpan(1), "A").Replace(" ", ""));
        }

        private static Dictionary<char, Dictionary<char, char[][]>> GetNumberPaths()
        {
            GraphNode zero = new() { cost = 0 };
            GraphNode one = new() { cost = 0 };
            GraphNode two = new() { cost = 0 };
            GraphNode three = new() { cost = 0 };
            GraphNode four = new() { cost = 0 };
            GraphNode five = new() { cost = 0 };
            GraphNode six = new() { cost = 0 };
            GraphNode seven = new() { cost = 0 };
            GraphNode eight = new() { cost = 0 };
            GraphNode nine = new() { cost = 0 };
            GraphNode a = new() { cost = 0 };

            zero.next.Add(new LabeledEdge() { label = '>', from = zero, to = a, cost = 1 });
            zero.next.Add(new LabeledEdge() { label = '^', from = zero, to = two, cost = 1 });

            one.next.Add(new LabeledEdge() { label = '>', from = one, to = two, cost = 1 });
            one.next.Add(new LabeledEdge() { label = '^', from = one, to = four, cost = 1 });

            two.next.Add(new LabeledEdge() { label = '<', from = two, to = one, cost = 1 });
            two.next.Add(new LabeledEdge() { label = '^', from = two, to = five, cost = 1 });
            two.next.Add(new LabeledEdge() { label = '>', from = two, to = three, cost = 1 });
            two.next.Add(new LabeledEdge() { label = 'v', from = two, to = zero, cost = 1 });

            three.next.Add(new LabeledEdge() { label = '<', from = three, to = two, cost = 1 });
            three.next.Add(new LabeledEdge() { label = '^', from = three, to = six, cost = 1 });
            three.next.Add(new LabeledEdge() { label = 'v', from = three, to = a, cost = 1 });

            four.next.Add(new LabeledEdge() { label = '^', from = four, to = seven, cost = 1 });
            four.next.Add(new LabeledEdge() { label = '>', from = four, to = five, cost = 1 });
            four.next.Add(new LabeledEdge() { label = 'v', from = four, to = one, cost = 1 });

            five.next.Add(new LabeledEdge() { label = '<', from = five, to = four, cost = 1 });
            five.next.Add(new LabeledEdge() { label = '^', from = five, to = eight, cost = 1 });
            five.next.Add(new LabeledEdge() { label = '>', from = five, to = six, cost = 1 });
            five.next.Add(new LabeledEdge() { label = 'v', from = five, to = two, cost = 1 });

            six.next.Add(new LabeledEdge() { label = '<', from = six, to = five, cost = 1 });
            six.next.Add(new LabeledEdge() { label = '^', from = six, to = nine, cost = 1 });
            six.next.Add(new LabeledEdge() { label = 'v', from = six, to = three, cost = 1 });

            seven.next.Add(new LabeledEdge() { label = '>', from = seven, to = eight, cost = 1 });
            seven.next.Add(new LabeledEdge() { label = 'v', from = seven, to = four, cost = 1 });

            eight.next.Add(new LabeledEdge() { label = '<', from = eight, to = seven, cost = 1 });
            eight.next.Add(new LabeledEdge() { label = '>', from = eight, to = nine, cost = 1 });
            eight.next.Add(new LabeledEdge() { label = 'v', from = eight, to = five, cost = 1 });

            nine.next.Add(new LabeledEdge() { label = '<', from = nine, to = eight, cost = 1 });
            nine.next.Add(new LabeledEdge() { label = 'v', from = nine, to = six, cost = 1 });

            a.next.Add(new LabeledEdge() { label = '<', from = a, to = zero, cost = 1 });
            a.next.Add(new LabeledEdge() { label = '^', from = a, to = three, cost = 1 });

            Dictionary<char, Dictionary<char, char[][]>> result = [];

            List<Tuple<char, GraphNode>> tuples = [];
            tuples.Add(Tuple.Create('0', zero));
            tuples.Add(Tuple.Create('1', one));
            tuples.Add(Tuple.Create('2', two));
            tuples.Add(Tuple.Create('3', three));
            tuples.Add(Tuple.Create('4', four));
            tuples.Add(Tuple.Create('5', five));
            tuples.Add(Tuple.Create('6', six));
            tuples.Add(Tuple.Create('7', seven));
            tuples.Add(Tuple.Create('8', eight));
            tuples.Add(Tuple.Create('9', nine));
            tuples.Add(Tuple.Create('A', a));

            result.Add('0', GrabPathsFrom(zero, tuples));
            result.Add('1', GrabPathsFrom(one, tuples));
            result.Add('2', GrabPathsFrom(two, tuples));
            result.Add('3', GrabPathsFrom(three, tuples));
            result.Add('4', GrabPathsFrom(four, tuples));
            result.Add('5', GrabPathsFrom(five, tuples));
            result.Add('6', GrabPathsFrom(six, tuples));
            result.Add('7', GrabPathsFrom(seven, tuples));
            result.Add('8', GrabPathsFrom(eight, tuples));
            result.Add('9', GrabPathsFrom(nine, tuples));
            result.Add('A', GrabPathsFrom(a, tuples));

            return result;
        }

        private static Dictionary<char, Dictionary<char, char[][]>> GetArrowPaths()
        {
            GraphNode up = new() { cost = 0 };
            GraphNode left = new() { cost = 0 };
            GraphNode right = new() { cost = 0 };
            GraphNode down = new() { cost = 0 };
            GraphNode a = new() { cost = 0 };

            up.next.Add(new LabeledEdge() { label = '>', from = up, to = a, cost = 1 });
            up.next.Add(new LabeledEdge() { label = 'v', from = up, to = down, cost = 1 });

            left.next.Add(new LabeledEdge() { label = '>', from = left, to = down, cost = 1 });

            down.next.Add(new LabeledEdge() { label = '<', from = down, to = left, cost = 1 });
            down.next.Add(new LabeledEdge() { label = '^', from = down, to = up, cost = 1 });
            down.next.Add(new LabeledEdge() { label = '>', from = down, to = right, cost = 1 });

            right.next.Add(new LabeledEdge() { label = '^', from = right, to = a, cost = 1 });
            right.next.Add(new LabeledEdge() { label = '<', from = right, to = down, cost = 1 });

            a.next.Add(new LabeledEdge() { label = '<', from = a, to = up, cost = 1 });
            a.next.Add(new LabeledEdge() { label = 'v', from = a, to = right, cost = 1 });

            Dictionary<char, Dictionary<char, char[][]>> result = [];

            List<Tuple<char, GraphNode>> tuples = [];
            tuples.Add(Tuple.Create('<', left));
            tuples.Add(Tuple.Create('^', up));
            tuples.Add(Tuple.Create('>', right));
            tuples.Add(Tuple.Create('v', down));
            tuples.Add(Tuple.Create('A', a));

            result.Add('^', GrabPathsFrom(up, tuples));
            result.Add('<', GrabPathsFrom(left, tuples));
            result.Add('>', GrabPathsFrom(right, tuples));
            result.Add('v', GrabPathsFrom(down, tuples));
            result.Add('A', GrabPathsFrom(a, tuples));

            return result;
        }

        private static Dictionary<char, char[][]> GrabPathsFrom(GraphNode start, List<Tuple<char, GraphNode>> tuples)
        {
            Dictionary<char, char[][]> dict = [];

            DijkstraaSolver.CalcDijkstraa(start);

            foreach (Tuple<char, GraphNode> tuple in tuples)
            {
                dict.Add(tuple.Item1, DijkstraaSolver.GetShortestPaths<char>(tuple.Item2, (node) => ' ', (edge) => ((LabeledEdge)edge).label).ToArray());
            }

            start.Reset();

            return dict;
        }

        public class LabeledEdge : GraphEdge
        {
            public required char label;
        }
    }

    public class Day21_Input
    {
        public required string[] inputs;
        public required Dictionary<char, Dictionary<char, char[][]>> numberPaths;
        public required Dictionary<char, Dictionary<char, char[][]>> arrowPaths;
    }
}