using System.Text.RegularExpressions;

namespace AdventOfCode
{
    internal partial class Day03_Solution : Helper.IDaySolution<string, long>
    {
        public string LoadData(string inputPath)
        {
            return File.ReadAllText(inputPath + "/Day03.txt");
        }

        public long Part1(string input)
        {
            MatchCollection matches = RegExPart1().Matches(input);
            return matches.Select(match =>
            {
                GroupCollection capturegroups = match.Groups;
                int left = int.Parse(capturegroups["left"].Value);
                int right = int.Parse(capturegroups["right"].Value);
                return left * right;
            }).Sum();
        }

        public long Part2(string input)
        {
            MatchCollection matches = RegExPart2().Matches(input);
            long result = 0;
            bool enabled = true;
            foreach (Match match in matches)
            {
                if (match.Value == "do()")
                {
                    enabled = true;
                }
                else if (match.Value == "don't()")
                {
                    enabled = false;
                }
                else if (enabled)
                {
                    GroupCollection capturegroups = match.Groups;
                    int left = int.Parse(capturegroups["left"].Value);
                    int right = int.Parse(capturegroups["right"].Value);
                    result += left * right;
                }
            }
            return result;
        }

        [GeneratedRegex("mul\\((?'left'\\d{1,3}),(?'right'\\d{1,3})\\)")]
        private static partial Regex RegExPart1();
        [GeneratedRegex("mul\\((?'left'\\d{1,3}),(?'right'\\d{1,3})\\)|(do\\(\\))|(don't\\(\\))")]
        private static partial Regex RegExPart2();
    }
}
