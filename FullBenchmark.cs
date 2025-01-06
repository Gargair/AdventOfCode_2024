using AdventOfCode.Helper;
using BenchmarkDotNet.Attributes;

namespace AdventOfCode
{
    public class FullBenchmark
    {
        [ParamsAllValues]
        public Parts PartToBenchmark { get; set; }

        public static IEnumerable<IDaySolution> SolutionsToBenchmark()
        {
            yield return new Day01_Solution();
            yield return new Day02_Solution();
            yield return new Day03_Solution();
            yield return new Day04_Solution();
            yield return new Day05_Solution();
            yield return new Day06_Solution();
            yield return new Day07_Solution();
            yield return new Day08_Solution();
            yield return new Day09_Solution();
            yield return new Day10_Solution();
            //yield return new Day11_Solution();
            //yield return new Day12_Solution();
            //yield return new Day13_Solution();
            //yield return new Day14_Solution();
            //yield return new Day15_Solution();
            //yield return new Day16_Solution();
            //yield return new Day17_Solution();
            //yield return new Day18_Solution();
            //yield return new Day19_Solution();
            //yield return new Day20_Solution();
            //yield return new Day21_Solution();
            //yield return new Day22_Solution();
        }

        [Benchmark]
        [ArgumentsSource(nameof(SolutionsToBenchmark), Priority = -1)]
        public void BenchmarkSolutions(IDaySolution Solution)
        {
            if ((PartToBenchmark & Parts.Part1) != 0)
            {
                string result = Solution.Part1("Input");
            }

            if ((PartToBenchmark & Parts.Part2) != 0)
            {
                string result = Solution.Part2("Input");
            }
        }

        public enum Parts
        {
            Part1 = 1,
            Part2 = 2
        }
    }
}
