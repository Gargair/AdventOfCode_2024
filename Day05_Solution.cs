namespace AdventOfCode
{
    internal class Day05_Solution : IDaySolutionUpdate<string[]>
    {
        public string[] LoadData(string inputPath)
        {
            return File.ReadAllLines(inputPath + "/Day05.txt");
        }

        public long Part1(string[] lines)
        {
            List<int[]> pageOrdering = [];
            List<int[]> updates = [];
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
                    int[] ord = line.Split('|').Select(c => int.Parse(c)).ToArray();
                    pageOrdering.Add(ord);
                }
                else
                {
                    int[] upd = line.Split(',').Select(c => int.Parse(c)).ToArray();
                    updates.Add(upd);
                }
            }
            long count = 0;
            foreach (int[] upd in updates)
            {
                if (IsValidOrder(upd, pageOrdering))
                {
                    count += upd[(upd.Length - 1) / 2];
                }
            }
            return count;
        }

        public long Part2(string[] lines)
        {
            List<int[]> pageOrdering = [];
            List<int[]> updates = [];
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
                    int[] ord = line.Split('|').Select(c => int.Parse(c)).ToArray();
                    pageOrdering.Add(ord);
                }
                else
                {
                    int[] upd = line.Split(',').Select(c => int.Parse(c)).ToArray();
                    updates.Add(upd);
                }
            }
            long count = 0;
            foreach (int[] upd in updates)
            {
                if (!IsValidOrder(upd, pageOrdering))
                {
                    OrderUpdate(upd, pageOrdering);
                    count += upd[(upd.Length - 1) / 2];
                }
            }
            return count;
        }

        private static bool IsValidOrder(int[] update, List<int[]> pageOrdering)
        {
            for (int i = 0; i < update.Length; i++)
            {
                int second = update[i];
                for (int j = 0; j < i; j++)
                {
                    int first = update[j];
                    foreach (int[] ord in pageOrdering)
                    {
                        if (ord[0] == second && ord[1] == first)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private static void OrderUpdate(int[] update, List<int[]> pageOrdering)
        {
            bool reordered = true;
            while (reordered)
            {
                reordered = false;
                for (int i = 0; i < update.Length; i++)
                {
                    int second = update[i];
                    for (int j = 0; j < i; j++)
                    {
                        int first = update[j];
                        foreach (int[] ord in pageOrdering)
                        {
                            if (ord[0] == second && ord[1] == first)
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
    }
}