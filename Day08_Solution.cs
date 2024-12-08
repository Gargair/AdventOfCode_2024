namespace AdventOfCode
{
    internal class Day08_Solution : IDaySolution
    {
        public Day08_Solution() { }

        char[][]? block;

        public void LoadData()
        {
            block = lines.Select(line => line.ToCharArray()).ToArray();
        }

        public long Part1()
        {
            if (block == null)
            {
                throw new InvalidOperationException("You have to call LoadData() before Part1()");
            }
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
            return count;
        }

        public long Part2()
        {
            if (block == null)
            {
                throw new InvalidOperationException("You have to call LoadData() before Part2()");
            }
            bool[] positionsFound = new bool[block.Length * block[0].Length];
            long count = 0;
            int half = (block.Length + 1) / 2;
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
            return count;
        }

        private static void CheckForAntiNodes(char[][] block, int startX, int startY, int deltaX, int deltaY, ref long count, ref bool[] positionsFound)
        {
            if (startX >= 0 && startX < block.Length && startY >= 0 && startY < block[0].Length)
            {
                int nextX = startX + deltaX;
                int nextY = startY + deltaY;
                if (nextX >= 0 && nextX < block.Length && nextY >= 0 && nextY < block[0].Length)
                {
                    if (block[startX][startY] == block[nextX][nextY])
                    {
                        // We found matching antennas calculate antinodes
                        {
                            int nodeX = nextX + deltaX;
                            int nodeY = nextY + deltaY;
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
                            int nodeX = startX - deltaX;
                            int nodeY = startY - deltaY;
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


        private static readonly string[] lines = @"....K..........................8.................z
.....n..............r...........z.................
.......................w....8.............3...E...
.....Q.....U..4.........8.........................
...............rc...w........i....................
...........K.........i..2.........................
..................4.....i.........................
K.....n....................w...........z..........
..U......Q........................I...............
..........i.....I.....Q....g....................5E
..Q......................................5........
..........c............8......w...g..........5....
.............................I.O..................
.Z.............4....b.....................k.......
..n........4......r..g..6..c.............3........
....Z............c................................
...................................x..............
.......................................O..........
...............U...................E..........5...
.....f..........................OI3......k........
..m.......o......F.......R........x...............
m...........o..v6..3...............X..............
..............H6v.....F.g.....................W...
...........o....Fb....v...............E...........
...Z.............a................................
......U6.............V............................
.9.............b..............pTk.................
.......m........V.........H1....x.................
...m.................H....................MX......
............t.a............H......................
........Z...a............v.....1..T..p.W..X.......
.............................9...x.......p........
.....J.....................V..1................0..
...........r..j..........a............pT..........
.G..................J...N......f..................
...........G......T....B........W.e...........M...
..........j.............Rk.............M..........
.........q.............MB......R.F..1..P....X...f.
............................V....o...........h....
...........................................W......
......b......u............................e.......
.............................................0....
..CA....Gt..O........................7.....e....0.
C.u......A..9J..N........................h.....e..
uj....q..........N.2..................7...........
G....N.....uJ...............................0.....
.................B................P.......h.......
...C....q...........R.........P...................
.....q..tC....2.9.....B............P....f.........
...............2.................................7".Split(Environment.NewLine);

        private static readonly string[] testinput = @"............
........0...
.....0......
.......0....
....0.......
......A.....
............
............
........A...
.........A..
............
............".Split(Environment.NewLine);
    }
}
