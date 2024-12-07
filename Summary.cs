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
            //StartSolution(new Day08_Solution());

            timer.Stop();
            Console.WriteLine($"[{DateTime.Now}]: Total: {timer.Elapsed}");
        }

        private static void StartSolution(IDaySolution solution)
        {
            string solutionName = solution.GetType().Name;
            Console.WriteLine($"[{DateTime.Now}]: {solutionName}");
            Stopwatch timer = new();

            timer.Start();
            solution.LoadData();
            timer.Stop();
            Console.WriteLine($"[{DateTime.Now}]: {solutionName} - Load Data: {timer.Elapsed}");

            timer.Restart();
            long result = solution.Part1();
            timer.Stop();
            Console.WriteLine($"[{DateTime.Now}]: {solutionName} - Part 01: {timer.Elapsed}\t{result}");

            timer.Restart();
            result = solution.Part2();
            timer.Stop();
            Console.WriteLine($"[{DateTime.Now}]: {solutionName} - Part 02: {timer.Elapsed}\t{result}");

            Console.WriteLine();
        }
    }
}
