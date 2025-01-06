using AdventOfCode.Helper;

namespace AdventOfCode
{
    internal partial class Day22_Solution : IDaySolution
    {
        public long[] LoadData(string inputFolder)
        {
            return File.ReadAllLines(inputFolder + "/Day22.txt").Select(s => long.Parse(s)).ToArray();
        }

        public override string Part1(string inputPath)
        {
            long[] inputs = LoadData(inputPath);
            long sum = 0;
            foreach (long input in inputs)
            {
                long m = input;
                for (int i = 0; i < 2000; i++)
                {
                    m = CalculateNext(m);
                }
                //Console.WriteLine($"{input}: {m}");
                sum += m;
            }
            return sum.ToString();
        }

        public override string Part2(string inputPath)
        {
            long[] inputs = LoadData(inputPath);
            return "";
        }

        private static long CalculateNext(long number)
        {
            long p = number * 64; // (2^6)
            number = number ^ p;
            number = number % 16777216; // (2^24)
            p = number / 32; // (2^5)
            number = number ^ p;
            number = number % 16777216;
            p = number * 2048; // (2^11)
            number = number ^ p;
            number = number % 16777216;
            return number;
        }
    }
}