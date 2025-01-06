namespace AdventOfCode
{
    internal class Day02_Solution : Helper.IDaySolution
    {
        public IEnumerable<int[]> LoadData(string inputPath)
        {
            return File.ReadAllLines(inputPath + "/Day02.txt").Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(l => int.Parse(l)).ToArray());
        }

        public override string Part1(string inputPath)
        {
            IEnumerable<int[]> reports = LoadData(inputPath);
            var validReports = reports.Where(IsValidReport);
            return validReports.Count().ToString();
        }

        public override string Part2(string inputPath)
        {
            IEnumerable<int[]> reports = LoadData(inputPath);
            var validReports = reports.Where(report =>
            {
                if (IsValidReport(report))
                {
                    return true;
                }
                for (int i = 0; i < report.Length; i++)
                {
                    var derivative = report.Where((v, index) => index != i).ToArray();
                    if (IsValidReport(derivative))
                    {
                        return true;
                    }
                }
                return false;
            });
            return validReports.Count().ToString();
        }

        private static bool IsValidReport(int[] report)
        {
            if (report.Length <= 1)
            {
                return true;
            }
            var ascending = report[0] <= report[1];
            for (int i = 0; i < report.Length - 1; i++)
            {
                if (ascending && report[i + 1] < report[i])
                {
                    return false;
                }
                if (!ascending && report[i] < report[i + 1])
                {
                    return false;
                }
                var distance = Math.Abs(report[i + 1] - report[i]);
                if (distance < 1 || distance > 3)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
