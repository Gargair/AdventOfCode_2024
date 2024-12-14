using System.Text.RegularExpressions;

namespace AdventOfCode
{
    internal partial class Day14_Solution : IDaySolution<IEnumerable<Robot>>
    {
        public IEnumerable<Robot> LoadData(string inputPath)
        {
            string[] lines = File.ReadAllLines(inputPath + "/Day14.txt");

            //List<Robot> robots = [];

            foreach (string line in lines)
            {
                Robot robot = new();
                var match = RobotParser().Match(line);
                robot.positionX = int.Parse(match.Groups["posX"].Value);
                robot.positionY = int.Parse(match.Groups["posY"].Value);
                robot.velocityX = int.Parse(match.Groups["velX"].Value);
                robot.velocityY = int.Parse(match.Groups["velY"].Value);
                if (inputPath == "Input_Test")
                {
                    robot.sizeX = 11;
                    robot.sizeY = 7;
                }
                else if (inputPath == "Input")
                {
                    robot.sizeX = 101;
                    robot.sizeY = 103;
                }

                //robots.Add(robot);
                yield return robot;
            }
            //return robots;
        }

        public long Part1(IEnumerable<Robot> robots)
        {
            long quadrant1 = 0;
            long quadrant2 = 0;
            long quadrant3 = 0;
            long quadrant4 = 0;
            foreach (Robot robot in robots)
            {
                robot.positionX += robot.velocityX * 100;
                robot.positionY += robot.velocityY * 100;
                while (robot.positionX >= robot.sizeX)
                {
                    robot.positionX -= robot.sizeX;
                }
                while (robot.positionX < 0)
                {
                    robot.positionX += robot.sizeX;
                }
                while (robot.positionY >= robot.sizeY)
                {
                    robot.positionY -= robot.sizeY;
                }
                while (robot.positionY < 0)
                {
                    robot.positionY += robot.sizeY;
                }

                int cutoffX = (robot.sizeX - 1) / 2;
                int cutoffY = (robot.sizeY - 1) / 2;
                if (robot.positionX < cutoffX)
                {
                    if (robot.positionY < cutoffY)
                    {
                        quadrant1++;
                    }
                    else if (robot.positionY > cutoffY)
                    {
                        quadrant2++;
                    }
                }
                else if (robot.positionX > cutoffX)
                {
                    if (robot.positionY < cutoffY)
                    {
                        quadrant3++;
                    }
                    else if (robot.positionY > cutoffY)
                    {
                        quadrant4++;
                    }
                }
            }
            return quadrant1 * quadrant2 * quadrant3 * quadrant4;
        }

        public long Part2(IEnumerable<Robot> robots)
        {
            char[,] space = new char[robots.First().sizeY, robots.First().sizeX];

            for (int i = 1; i <= 101 * 103; i++)
            {
                for (int x = 0; x < space.GetLength(0); x++)
                {
                    for (int y = 0; y < space.GetLength(1); y++)
                    {
                        space[x, y] = '.';
                    }
                }
                foreach (Robot robot in robots)
                {
                    robot.positionX += robot.velocityX * i;
                    robot.positionY += robot.velocityY * i;
                    while (robot.positionX >= robot.sizeX)
                    {
                        robot.positionX -= robot.sizeX;
                    }
                    while (robot.positionX < 0)
                    {
                        robot.positionX += robot.sizeX;
                    }
                    while (robot.positionY >= robot.sizeY)
                    {
                        robot.positionY -= robot.sizeY;
                    }
                    while (robot.positionY < 0)
                    {
                        robot.positionY += robot.sizeY;
                    }
                    space[robot.positionY, robot.positionX] = '#';
                }
                for (int x = 0; x < space.GetLength(0); x++)
                {
                    for (int y = 0; y < space.GetLength(1); y++)
                    {
                        if (space[x, y] == '#')
                        {
                            bool found = true;
                            for (int t = 1; t <= 15; t++)
                            {
                                if (!IsInMatrix(space, x + t, y) || space[x + t, y] != '#')
                                {
                                    found = false;
                                    break;
                                }
                            }
                            if (found)
                            {
                                //    Console.WriteLine($"Step: {i}");
                                //    for (int x = 0; x < space.GetLength(0); x++)
                                //    {
                                //        for (int y = 0; y < space.GetLength(1); y++)
                                //        {
                                //            Console.Write(space[x, y]);
                                //        }
                                //        Console.WriteLine();
                                //    }
                                //    Console.WriteLine();
                                return i;
                            }
                        }
                    }
                }

            }
            return 0;
        }

        private static bool IsInMatrix<T>(T[,] matrix, int x, int y)
        {
            return x >= 0 && x < matrix.GetLength(0) && y >= 0 && y < matrix.GetLength(1);
        }

        [GeneratedRegex("p=(?'posX'-?\\d+),(?'posY'-?\\d+) v=(?'velX'-?\\d+),(?'velY'-?\\d+)")]
        private static partial Regex RobotParser();
    }

    public class Robot
    {
        public int positionX;
        public int positionY;
        public int velocityX;
        public int velocityY;
        public int sizeX;
        public int sizeY;
    }
}
