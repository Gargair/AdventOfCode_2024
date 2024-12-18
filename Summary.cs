using AdventOfCode.Helper;
using System.Diagnostics;

namespace AdventOfCode
{
    public class Summary
    {
        public static void Main(string[] _args)
        {
            Stopwatch timer = new();
            timer.Start();
            StartSolution(new Day01_Solution());
            StartSolution(new Day02_Solution());
            StartSolution(new Day03_Solution());
            StartSolution(new Day04_Solution());
            StartSolution(new Day05_Solution());
            StartSolution(new Day06_Solution());
            StartSolution(new Day07_Solution());
            StartSolution(new Day08_Solution());
            StartSolution(new Day09_Solution());
            StartSolution(new Day10_Solution());
            StartSolution(new Day11_Solution());
            StartSolution(new Day12_Solution());
            StartSolution(new Day13_Solution());
            StartSolution(new Day14_Solution());
            StartSolution(new Day15_Solution());
            StartSolution(new Day16_Solution());
            StartSolution(new Day17_Solution());
            StartSolution(new Day18_Solution());

            timer.Stop();
            Console.WriteLine($"[{DateTime.Now}]: Total: {timer.Elapsed}");
        }

        private static void StartSolution<T, O>(IDaySolution<T, O> solution, string inputPath = "Input", Parts parts = Parts.Part1 | Parts.Part2)
        {
            string solutionName = solution.GetType().Name;
            Console.WriteLine($"[{DateTime.Now}]: {solutionName}");
            Stopwatch timer = new();

            if ((parts & Parts.Part1) != 0)
            {
                timer.Restart();
                T input = solution.LoadData(inputPath);
                O result = solution.Part1(input);
                timer.Stop();
                Console.WriteLine($"[{DateTime.Now}]: {solutionName} - Part 01: {timer.Elapsed}\t{result}");
            }

            if ((parts & Parts.Part2) != 0)
            {
                timer.Restart();
                T input = solution.LoadData(inputPath);
                O result = solution.Part2(input);
                timer.Stop();
                Console.WriteLine($"[{DateTime.Now}]: {solutionName} - Part 02: {timer.Elapsed}\t{result}");
            }

            Console.WriteLine();
        }

        private enum Parts
        {
            Part1 = 1,
            Part2 = 2
        }
    }
}
