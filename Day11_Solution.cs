namespace AdventOfCode
{
    internal class Day11_Solution : Helper.IDaySolution
    {
        public List<long> LoadData(string inputPath)
        {
            return File.ReadAllText(inputPath + "/Day11.txt").Split(' ').Select(s => long.Parse(s)).ToList();
        }

        public override string Part1(string inputPath)
        {
            List<long> stones = LoadData(inputPath);
            long count = 0;
            Dictionary<Tuple<long, int>, long> cache = [];
            for (int i = 0; i < stones.Count; i++)
            {
                count += BlinkResultRec(stones[i], 25, cache);
            }
            return count.ToString();
        }

        public override string Part2(string inputPath)
        {
            List<long> stones = LoadData(inputPath);
            long count = 0;
            Dictionary<Tuple<long, int>, long> cache = [];
            for (int i = 0; i < stones.Count; i++)
            {
                count += BlinkResultRec(stones[i], 75, cache);
            }
            return count.ToString();
        }

        private static long BlinkResultRec(long stone, int blinkAmount, Dictionary<Tuple<long, int>, long> cache)
        {
            if (blinkAmount == 0)
            {
                return 1;
            }
            if (cache.TryGetValue(Tuple.Create(stone, blinkAmount), out long stoneCount))
            {
                return stoneCount;
            }
            long res;
            if (stone == 0)
            {
                res = BlinkResultRec(1, blinkAmount - 1, cache);

            }
            else if (NumberOfDigits(stone) % 2 == 0)
            {
                int digits = NumberOfDigits(stone);
                long divisor = (long)Math.Pow(10, digits / 2);
                long stone1 = stone / divisor;
                long stone2 = stone % divisor;
                res = BlinkResultRec(stone1, blinkAmount - 1, cache);
                res += BlinkResultRec(stone2, blinkAmount - 1, cache);
            }
            else
            {
                res = BlinkResultRec(stone * 2024, blinkAmount - 1, cache);
            }
            cache.Add(Tuple.Create(stone, blinkAmount), res);
            return res;
        }

        private static int NumberOfDigits(long number)
        {
            int count = 0;
            while (number > 0)
            {
                number /= 10;
                count++;
            }
            return count;
        }
    }
}
