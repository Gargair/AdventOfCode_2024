namespace AdventOfCode
{
    internal class Day08_Solution : Helper.IDaySolution
    {
        public char[][] LoadData(string inputPath)
        {
            return File.ReadAllLines(inputPath + "/Day08.txt").Select(line => line.ToCharArray()).ToArray();
        }

        public override string Part1(string inputPath)
        {
            char[][] block = LoadData(inputPath);
            bool[] positionsFound = new bool[block.Length * block[0].Length];
            long count = 0;
            for (int i = 0; i < block.Length; i++)
            {
                for (int j = 0; j < block[i].Length; j++)
                {
                    if (block[i][j] != '.')
                    {
                        for (int a = 0; a < block.Length; a++)
                        {
                            for (int b = 0; b < block[i].Length; b++)
                            {
                                if (block[i][j] == block[a][b] && (i != a || j != b))
                                {
                                    int deltaX = a - i;
                                    int deltaY = b - j;
                                    // We found matching antennas calculate antinodes
                                    {
                                        int nodeX = a + deltaX;
                                        int nodeY = b + deltaY;
                                        if (nodeX >= 0 && nodeX < block.Length && nodeY >= 0 && nodeY < block[0].Length)
                                        {
                                            // Anti node within block
                                            long posNum = nodeX * block[0].Length + nodeY;
                                            if (!positionsFound[posNum])
                                            {
                                                positionsFound[posNum] = true;
                                                count++;
                                            }
                                        }
                                    }
                                    {
                                        int nodeX = i - deltaX;
                                        int nodeY = j - deltaY;
                                        if (nodeX >= 0 && nodeX < block.Length && nodeY >= 0 && nodeY < block[0].Length)
                                        {
                                            // Anti node within block
                                            long posNum = nodeX * block[0].Length + nodeY;
                                            if (!positionsFound[posNum])
                                            {
                                                positionsFound[posNum] = true;
                                                count++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return count.ToString();
        }

        public override string Part2(string inputPath)
        {
            char[][] block = LoadData(inputPath);
            bool[] positionsFound = new bool[block.Length * block[0].Length];
            long count = 0;
            for (int i = 0; i < block.Length; i++)
            {
                for (int j = 0; j < block[i].Length; j++)
                {
                    if (block[i][j] != '.')
                    {
                        for (int a = 0; a < block.Length; a++)
                        {
                            for (int b = 0; b < block[i].Length; b++)
                            {
                                if (block[i][j] == block[a][b] && (i != a || j != b))
                                {
                                    int deltaX = a - i;
                                    int deltaY = b - j;
                                    // We found matching antennas calculate antinodes
                                    for (int k = -block.Length; k <= block.Length; k++)
                                    {
                                        int nodeX = i + deltaX * k;
                                        int nodeY = j + deltaY * k;
                                        if (nodeX >= 0 && nodeX < block.Length && nodeY >= 0 && nodeY < block[0].Length)
                                        {
                                            // Anti node within block
                                            long posNum = nodeX * block[0].Length + nodeY;
                                            if (!positionsFound[posNum])
                                            {
                                                positionsFound[posNum] = true;
                                                count++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return count.ToString();
        }
    }
}
