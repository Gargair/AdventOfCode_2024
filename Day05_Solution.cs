namespace AdventOfCode
{
    internal class Day05_Solution : Helper.IDaySolution
    {
        private static Day05_Input LoadData(string inputPath)
        {
            string[] lines = File.ReadAllLines(inputPath + "/Day05.txt");
            Day05_Input input = new();
            bool ordering = true;
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    ordering = false;
                    continue;
                }
                if (ordering)
                {
                    int[] ord = line.Split('|').Select(int.Parse).ToArray();
                    input.pageOrdering.TryAdd(ord[0], []);
                    input.pageOrdering[ord[0]].Add(ord[1]);
                }
                else
                {
                    int[] upd = line.Split(',').Select(int.Parse).ToArray();
                    input.updates.Add(upd);
                }
            }
            return input;
        }

        public override string Part1(string inputPath)
        {
            Day05_Input input = LoadData(inputPath);
            long count = 0;
            foreach (int[] upd in input.updates)
            {
                if (IsValidOrder(upd, input.pageOrdering))
                {
                    count += upd[(upd.Length - 1) / 2];
                }
            }
            return count.ToString();
        }

        public override string Part2(string inputPath)
        {
            Day05_Input input = LoadData(inputPath);
            long count = 0;
            foreach (int[] upd in input.updates)
            {
                if (!IsValidOrder(upd, input.pageOrdering))
                {
                    OrderUpdate(upd, input.pageOrdering);
                    count += upd[(upd.Length - 1) / 2];
                }
            }
            return count.ToString();
        }

        private static bool IsValidOrder(int[] update, Dictionary<int, List<int>> pageOrdering)
        {
            for (int i = 0; i < update.Length; i++)
            {
                int second = update[i];
                if (pageOrdering.TryGetValue(second, out List<int>? afterPages))
                {
                    for (int j = 0; j < i; j++)
                    {
                        int first = update[j];
                        if (afterPages.Contains(first))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private static void OrderUpdate(int[] update, Dictionary<int, List<int>> pageOrdering)
        {
            bool reordered = true;
            while (reordered)
            {
                reordered = false;
                for (int i = 0; i < update.Length; i++)
                {
                    int second = update[i];
                    if (pageOrdering.TryGetValue(second, out List<int>? afterPages))
                    {
                        for (int j = 0; j < i; j++)
                        {
                            int first = update[j];
                            if (afterPages.Contains(first))
                            {
                                reordered = true;
                                for (int k = j; k < i; k++)
                                {
                                    update[k] = update[k + 1];
                                }
                                update[i] = first;
                            }
                        }
                    }
                }
            }
        }

        public class Day05_Input
        {
            public readonly Dictionary<int, List<int>> pageOrdering = [];
            public readonly List<int[]> updates = [];
        }
    }
}