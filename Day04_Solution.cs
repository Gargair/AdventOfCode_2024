namespace AdventOfCode
{
    internal class Day04_Solution : Helper.IDaySolution<char[][]>
    {
        public char[][] LoadData(string inputPath)
        {
            return File.ReadAllLines(inputPath + "/Day04.txt").Select(line => line.ToCharArray()).ToArray();
        }

        public long Part1(char[][] block)
        {
            long count = 0;

            char[] toSearch = "XMAS".ToCharArray();

            for (int i = 0; i < block.Length; i++)
            {
                for (int j = 0; j < block[i].Length; j++)
                {
                    if (block[i][j] == toSearch[0])
                    {
                        for (int dirX = -1; dirX <= 1; dirX++)
                        {
                            for (int dirY = -1; dirY <= 1; dirY++)
                            {
                                if (Helper.MatrixHelper.IsInDirection(block, i, j, dirX, dirY, toSearch))
                                {
                                    count++;
                                }
                            }
                        }
                    }
                }
            }
            return count;
        }

        public long Part2(char[][] block)
        {
            long count = 0;

            char[] toSearch = "MAS".ToCharArray();

            for (int i = 0; i < block.Length; i++)
            {
                for (int j = 0; j < block[i].Length; j++)
                {
                    if (block[i][j] == toSearch[toSearch.Length / 2])
                    {
                        if (Helper.MatrixHelper.IsInDirection(block, i - 1, j - 1, 1, 1, toSearch) && (Helper.MatrixHelper.IsInDirection(block, i + 1, j - 1, -1, 1, toSearch) || Helper.MatrixHelper.IsInDirection(block, i - 1, j + 1, 1, -1, toSearch)))
                        {
                            count++;
                        }
                        else if (Helper.MatrixHelper.IsInDirection(block, i + 1, j - 1, -1, 1, toSearch) && Helper.MatrixHelper.IsInDirection(block, i + 1, j + 1, -1, -1, toSearch))
                        {
                            count++;
                        }
                        else if (Helper.MatrixHelper.IsInDirection(block, i - 1, j + 1, 1, -1, toSearch) && Helper.MatrixHelper.IsInDirection(block, i + 1, j + 1, -1, -1, toSearch))
                        {
                            count++;
                        }
                    }
                }
            }
            return count;
        }
    }
}
