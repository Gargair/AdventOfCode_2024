namespace AdventOfCode
{
    internal class Day01_Solution : Helper.IDaySolution<string[], long>
    {
        public string[] LoadData(string inputFolder)
        {
            return File.ReadAllLines(inputFolder + "/Day01.txt");
        }

        public long Part1(string[] lines)
        {
            IEnumerable<int> leftList = lines.Select(line => int.Parse(line.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0]));
            IEnumerable<int> rightList = lines.Select(line => int.Parse(line.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]));
            IOrderedEnumerable<int> orderedLeftList = leftList.Order();
            IOrderedEnumerable<int> orderedRightList = rightList.Order();
            IEnumerable<int> distances = orderedLeftList.Zip(orderedRightList, (left, right) => Math.Abs(right - left));
            return distances.Sum();
        }

        public long Part2(string[] lines)
        {
            IEnumerable<int> leftList = lines.Select(line => int.Parse(line.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0]));
            IEnumerable<int> rightList = lines.Select(line => int.Parse(line.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]));
            Dictionary<int, int> rightDict = [];
            foreach (int item in rightList)
            {
                rightDict.TryAdd(item, 0);
                rightDict[item]++;
            }
            IEnumerable<int> similarity = leftList.Select(v => rightDict.GetValueOrDefault(v, 0) * v);
            return similarity.Sum();
        }
    }
}
