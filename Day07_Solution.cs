namespace AdventOfCode
{
    internal class Day07_Solution : Helper.IDaySolution<long[][]>
    {
        public long[][] LoadData(string inputPath)
        {
            return File.ReadAllLines(inputPath + "/Day07.txt").Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(n => n.EndsWith(':') ? long.Parse(n[..^1]) : long.Parse(n)).ToArray()).ToArray();
        }

        public long Part1(long[][] equations)
        {
            long sum = 0;
            foreach (var eq in equations)
            {
                long shouldResult = eq[0];
                if (IsValidPart1(eq, shouldResult, eq[1], 2))
                {
                    sum += shouldResult;
                }
            }
            return sum;
        }

        public long Part2(long[][] equations)
        {
            long sum = 0;
            foreach (var eq in equations)
            {
                long shouldResult = eq[0];
                if (IsValidPart2(eq, shouldResult, eq[1], 2))
                {
                    sum += shouldResult;
                }
            }
            return sum;
        }

        private static bool IsValidPart1(long[] equation, long targetValue, long current, int index)
        {
            if (index == equation.Length)
            {
                return current == targetValue;
            }
            if (current > targetValue)
            {
                return false;
            }

            return IsValidPart1(equation, targetValue, current + equation[index], index + 1) || IsValidPart1(equation, targetValue, current * equation[index], index + 1);
        }

        private static bool IsValidPart2(long[] equation, long targetValue, long current, int index)
        {
            if (index == equation.Length)
            {
                return current == targetValue;
            }
            if (current > targetValue)
            {
                return false;
            }

            return IsValidPart2(equation, targetValue, current + equation[index], index + 1) || IsValidPart2(equation, targetValue, current * equation[index], index + 1) || IsValidPart2(equation, targetValue, current * NextPower(equation[index]) + equation[index], index + 1);
        }

        private static long NextPower(long value)
        {
            if (value < 10)
            {
                return 10;
            }
            else if (value < 100)
            {
                return 100;
            }
            else if (value < 1000)
            {
                return 1000;
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
