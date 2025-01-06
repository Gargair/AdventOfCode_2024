using AdventOfCode.Helper;

namespace AdventOfCode
{
    internal class Day06_Solution : Helper.IDaySolution
    {
        public char[][] LoadData(string inputPath)
        {
            return File.ReadAllLines(inputPath + "/Day06.txt").Select(line => line.ToCharArray()).ToArray();
        }

        public override string Part1(string inputPath)
        {
            char[][] block = LoadData(inputPath);
            Tuple<int, int>? start = Helper.MatrixHelper.FindFirstElement(block, '^') ?? throw new Exception("Start not found");
            return PathLength(block, start.Item1, start.Item2).ToString();
        }

        public override string Part2(string inputPath)
        {
            char[][] block = LoadData(inputPath);
            Tuple<int, int>? start = Helper.MatrixHelper.FindFirstElement(block, '^') ?? throw new Exception("Start not found");
            WalkPath(block, start.Item1, start.Item2);
            // Take actual path

            return Enumerable.Range(0, block.Length)
                    .SelectMany(x => Enumerable.Range(0, block[x].Length)
                                        .Select(y => Tuple.Create(x, y)))
                    .AsParallel()
                    .WithDegreeOfParallelism(Environment.ProcessorCount)
                    .Where(position => block[position.Item1][position.Item2] == 'X')
                    .Where(position => DetermineLoop(block, start.Item1, start.Item2, position.Item1, position.Item2))
                    .Count().ToString();
        }

        private static bool DetermineLoop(char[][] block, int startX, int startY, int blockedX, int blockedY)
        {
            List<Tuple<int, int, Direction>> positionsVisited = [];

            Direction direction = Direction.UP;
            int currentX = startX;
            int currentY = startY;
            Tuple<int, int>? nextStay = SearchPosition(block, currentX, currentY, direction, blockedX, blockedY);
            while (nextStay != null)
            {
                Tuple<int, int, Direction> currentVisited = Tuple.Create(currentX, currentY, direction);
                if (positionsVisited.Contains(currentVisited))
                {
                    return true;
                }
                positionsVisited.Add(currentVisited);
                currentX = nextStay.Item1;
                currentY = nextStay.Item2;
                direction = NextDirection(direction);
                nextStay = SearchPosition(block, currentX, currentY, direction, blockedX, blockedY);
            }
            return false;
        }

        private static void WalkPath(char[][] block, int startX, int startY)
        {
            Direction direction = Direction.UP;
            int currentX = startX;
            int currentY = startY;
            Tuple<int, int>? nextStay = SearchDirection(block, currentX, currentY, direction);
            while (nextStay != null)
            {
                currentX = nextStay.Item1;
                currentY = nextStay.Item2;
                direction = NextDirection(direction);
                nextStay = SearchDirection(block, currentX, currentY, direction);
            }
        }

        private static long PathLength(char[][] block, int startX, int startY)
        {
            WalkPath(block, startX, startY);
            return MatrixHelper.CountElement(block, 'X');
        }

        private static Tuple<int, int>? SearchDirection(char[][] block, int startX, int startY, Direction direction)
        {
            switch (direction)
            {
                case Direction.UP:
                    for (int currentX = startX; currentX >= 0; currentX--)
                    {
                        if (block[currentX][startY] == '#')
                        {
                            return Tuple.Create(currentX + 1, startY);
                        }
                        else
                        {
                            block[currentX][startY] = 'X';
                        }
                    }
                    return null;
                case Direction.DOWN:
                    for (int currentX = startX; currentX < block.Length; currentX++)
                    {
                        if (block[currentX][startY] == '#')
                        {
                            return Tuple.Create(currentX - 1, startY);
                        }
                        else
                        {
                            block[currentX][startY] = 'X';
                        }
                    }
                    return null;
                case Direction.LEFT:
                    for (int currentY = startY; currentY >= 0; currentY--)
                    {
                        if (block[startX][currentY] == '#')
                        {
                            return Tuple.Create(startX, currentY + 1);
                        }
                        else
                        {
                            block[startX][currentY] = 'X';
                        }
                    }
                    return null;
                case Direction.RIGHT:
                    for (int currentY = startY; currentY < block[startX].Length; currentY++)
                    {
                        if (block[startX][currentY] == '#')
                        {
                            return Tuple.Create(startX, currentY - 1);
                        }
                        else
                        {
                            block[startX][currentY] = 'X';
                        }
                    }
                    return null;
                default:
                    throw new NotImplementedException();
            }
        }

        private static Tuple<int, int>? SearchPosition(char[][] block, int startX, int startY, Direction direction, int blockedX, int blockedY)
        {
            switch (direction)
            {
                case Direction.UP:
                    for (int currentX = startX; currentX >= 0; currentX--)
                    {
                        if (block[currentX][startY] == '#' || (currentX == blockedX && startY == blockedY))
                        {
                            return Tuple.Create(currentX + 1, startY);
                        }
                    }
                    return null;
                case Direction.DOWN:
                    for (int currentX = startX; currentX < block.Length; currentX++)
                    {
                        if (block[currentX][startY] == '#' || (currentX == blockedX && startY == blockedY))
                        {
                            return Tuple.Create(currentX - 1, startY);
                        }
                    }
                    return null;
                case Direction.LEFT:
                    for (int currentY = startY; currentY >= 0; currentY--)
                    {
                        if (block[startX][currentY] == '#' || (startX == blockedX && currentY == blockedY))
                        {
                            return Tuple.Create(startX, currentY + 1);
                        }
                    }
                    return null;
                case Direction.RIGHT:
                    for (int currentY = startY; currentY < block[startX].Length; currentY++)
                    {
                        if (block[startX][currentY] == '#' || (startX == blockedX && currentY == blockedY))
                        {
                            return Tuple.Create(startX, currentY - 1);
                        }
                    }
                    return null;
                default:
                    throw new NotImplementedException();
            }
        }

        private static Direction NextDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.UP:
                    return Direction.RIGHT;
                case Direction.DOWN:
                    return Direction.LEFT;
                case Direction.LEFT:
                    return Direction.UP;
                case Direction.RIGHT:
                    return Direction.DOWN;
                default:
                    throw new NotImplementedException();
            }
        }

        private enum Direction
        {
            UP,
            DOWN,
            LEFT,
            RIGHT
        }
    }
}
