namespace AdventOfCode
{
    internal class Day04_Solution : IDaySolutionUpdate<char[][]>
    {
        public char[][] LoadData(string inputPath)
        {
            return File.ReadAllLines(inputPath + "/Day04.txt").Select(line => line.ToCharArray()).ToArray();
        }

        public long Part1(char[][] block)
        {
            long count = 0;
            for (int i = 0; i < block.Length; i++)
            {
                for (int j = 0; j < block[i].Length; j++)
                {
                    if (block[i][j] == 'X')
                    {
                        for (int dirX = -1; dirX <= 1; dirX++)
                        {
                            for (int dirY = -1; dirY <= 1; dirY++)
                            {
                                if (IsInDirection(block, i, j, dirX, dirY, "XMAS"))
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
            for (int i = 0; i < block.Length; i++)
            {
                for (int j = 0; j < block[i].Length; j++)
                {
                    if (block[i][j] == 'A')
                    {
                        if (IsInDirection(block, i - 1, j - 1, 1, 1, "MAS") && (IsInDirection(block, i + 1, j - 1, -1, 1, "MAS") || IsInDirection(block, i - 1, j + 1, 1, -1, "MAS")))
                        {
                            count++;
                        }
                        else if (IsInDirection(block, i + 1, j - 1, -1, 1, "MAS") && IsInDirection(block, i + 1, j + 1, -1, -1, "MAS"))
                        {
                            count++;
                        }
                        else if (IsInDirection(block, i - 1, j + 1, 1, -1, "MAS") && IsInDirection(block, i + 1, j + 1, -1, -1, "MAS"))
                        {
                            count++;
                        }
                    }
                }
            }
            return count;
        }

        private static bool IsInDirection(char[][] block, int startX, int startY, int deltaX, int deltaY, string toSearch)
        {
            for (int l = 0; l < toSearch.Length; l++)
            {
                int blockX = startX + deltaX * l;
                if (blockX < 0 || blockX >= block.Length)
                {
                    return false;
                }
                int blockY = startY + deltaY * l;
                if (blockY < 0 || blockY >= block[blockX].Length)
                {
                    return false;
                }
                if (block[blockX][blockY] != toSearch[l])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
