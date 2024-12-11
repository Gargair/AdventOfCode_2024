namespace AdventOfCode
{
    internal class Day11_Solution : IDaySolution
    {
        public Day11_Solution() { }

        List<long> stones;
        Dictionary<Tuple<long, int>, long> cache;

        public void LoadData()
        {
            stones = lines.Split(' ').Select(s => long.Parse(s)).ToList();
        }

        public long Part1()
        {
            if (stones == null)
            {
                throw new InvalidOperationException("You have to call LoadData() before Part1()");
            }
            long count = 0;
            cache = [];
            for (int i = 0; i < stones.Count; i++)
            {
                count += BlinkResultRec(stones[i], 25, cache);
            }
            return count;
        }

        public long Part2()
        {
            if (stones == null)
            {
                throw new InvalidOperationException("You have to call LoadData() before Part1()");
            }
            long count = 0;
            cache = [];
            for (int i = 0; i < stones.Count; i++)
            {
                count += BlinkResultRec(stones[i], 75, cache);
            }
            return count;
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

        private static readonly string lines = @"2 77706 5847 9258441 0 741 883933 12";

        private static readonly string testinput = @"125 17";
    }
}
