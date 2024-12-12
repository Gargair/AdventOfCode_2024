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

            timer.Stop();
            Console.WriteLine($"[{DateTime.Now}]: Total: {timer.Elapsed}");
        }

        private static void StartSolution(IDaySolution solution, Parts parts = Parts.Part1 | Parts.Part2)
        {
            string solutionName = solution.GetType().Name;
            Console.WriteLine($"[{DateTime.Now}]: {solutionName}");
            Stopwatch timer = new();

            timer.Start();
            solution.LoadData();
            timer.Stop();
            Console.WriteLine($"[{DateTime.Now}]: {solutionName} - Load Data: {timer.Elapsed}");

            if ((parts & Parts.Part1) != 0)
            {
                timer.Restart();
                long result = solution.Part1();
                timer.Stop();
                Console.WriteLine($"[{DateTime.Now}]: {solutionName} - Part 01: {timer.Elapsed}\t{result}");
            }

            if ((parts & Parts.Part2) != 0)
            {
                timer.Restart();
                long result = solution.Part2();
                timer.Stop();
                Console.WriteLine($"[{DateTime.Now}]: {solutionName} - Part 02: {timer.Elapsed}\t{result}");
            }

            Console.WriteLine();
        }


        private static void StartSolution<T>(IDaySolution<T> solution, string inputPath = "Input", Parts parts = Parts.Part1 | Parts.Part2)
        {
            string solutionName = solution.GetType().Name;
            Console.WriteLine($"[{DateTime.Now}]: {solutionName}");
            Stopwatch timer = new();

            if ((parts & Parts.Part1) != 0)
            {
                timer.Restart();
                T input = solution.LoadData(inputPath);
                long result = solution.Part1(input);
                timer.Stop();
                Console.WriteLine($"[{DateTime.Now}]: {solutionName} - Part 01: {timer.Elapsed}\t{result}");
            }

            if ((parts & Parts.Part2) != 0)
            {
                timer.Restart();
                T input = solution.LoadData(inputPath);
                long result = solution.Part2(input);
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
