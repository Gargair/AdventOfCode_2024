using AdventOfCode.Helper;
using BenchmarkDotNet.Running;
using System.Diagnostics;

namespace AdventOfCode
{
    public class Summary
    {
        public static void Main(string[] _args)
        {
            DoFullBenchmark();
            FullResults();
        }

        private static void DoFullBenchmark()
        {
            BenchmarkRunner.Run<FullBenchmark>();
        }

        private static void FullResults()
        {
            Stopwatch timer = new();
            timer.Start();
            foreach (IDaySolution solution in FullBenchmark.SolutionsToBenchmark())
            {
                StartSolution(solution);
            }
            timer.Stop();
            Console.WriteLine($"[{DateTime.Now}]: Total: {timer.Elapsed}");
        }

        private static void StartSolution(IDaySolution solution, string inputPath = "Input", Parts parts = Parts.Part1 | Parts.Part2)
        {
            string solutionName = solution.GetType().Name;
            Console.WriteLine($"[{DateTime.Now}]: {solutionName}");

            if ((parts & Parts.Part1) != 0)
            {
                string result = solution.Part1(inputPath);
                Console.WriteLine($"[{DateTime.Now}]: {solutionName} - Part 01: \t{result}");
            }

            if ((parts & Parts.Part2) != 0)
            {
                string result = solution.Part2(inputPath);
                Console.WriteLine($"[{DateTime.Now}]: {solutionName} - Part 02: \t{result}");
            }

            Console.WriteLine();
        }

        public enum Parts
        {
            Part1 = 1,
            Part2 = 2
        }
    }
}
