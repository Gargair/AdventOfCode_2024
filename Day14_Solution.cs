using System.Text.RegularExpressions;

namespace AdventOfCode
{
    internal partial class Day14_Solution : Helper.IDaySolution<Day14_Input>
    {
        public Day14_Input LoadData(string inputPath)
        {
            // Input in File needs to be prepended with a line stating the size of the field.
            // f.e. 
            // 11,7
            string[] lines = File.ReadAllLines(inputPath + "/Day14.txt");

            string firstLine = lines.First();
            var lineMatch = SizeParser().Match(firstLine);
            return new()
            {
                sizeX = int.Parse(lineMatch.Groups["sizeX"].Value),
                sizeY = int.Parse(lineMatch.Groups["sizeY"].Value),
                robots = ParseRobots(lines.Skip(1))
            };
        }

        public long Part1(Day14_Input input)
        {
            long quadrant1 = 0;
            long quadrant2 = 0;
            long quadrant3 = 0;
            long quadrant4 = 0;
            int cutoffX = (input.sizeX - 1) / 2;
            int cutoffY = (input.sizeY - 1) / 2;

            IEnumerable<Robot> robots = input.robots;
            foreach (Robot robot in robots)
            {
                robot.positionX += robot.velocityX * 100;
                robot.positionY += robot.velocityY * 100;
                while (robot.positionX >= input.sizeX)
                {
                    robot.positionX -= input.sizeX;
                }
                while (robot.positionX < 0)
                {
                    robot.positionX += input.sizeX;
                }
                while (robot.positionY >= input.sizeY)
                {
                    robot.positionY -= input.sizeY;
                }
                while (robot.positionY < 0)
                {
                    robot.positionY += input.sizeY;
                }

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

        public long Part2(Day14_Input input)
        {
            IEnumerable<Robot> robots = input.robots;

            int stepNumber = 0;

            List<char?> treeStem = [];
            for(int i = 0; i < 15; i++)
            {
                treeStem.Add('#');
            }

            char?[] toSearch = treeStem.ToArray();

            Enumerable.Range(1, input.sizeX * input.sizeY).AsParallel().WithDegreeOfParallelism(Environment.ProcessorCount).Where(i =>
            {
                char?[,] space = new char?[input.sizeY, input.sizeX];
                foreach (Robot robot in robots)
                {
                    robot.positionX += robot.velocityX * i;
                    robot.positionY += robot.velocityY * i;
                    while (robot.positionX >= input.sizeX)
                    {
                        robot.positionX -= input.sizeX;
                    }
                    while (robot.positionX < 0)
                    {
                        robot.positionX += input.sizeX;
                    }
                    while (robot.positionY >= input.sizeY)
                    {
                        robot.positionY -= input.sizeY;
                    }
                    while (robot.positionY < 0)
                    {
                        robot.positionY += input.sizeY;
                    }
                    space[robot.positionY, robot.positionX] = '#';
                }
                for (int x = 0; x < space.GetLength(0); x++)
                {
                    for (int y = 0; y < space.GetLength(1); y++)
                    {
                        if (space[x, y] == '#')
                        {
                            if (Helper.MatrixHelper.IsInDirection(space, x, y, 1, 0, toSearch))
                            {
                                //Console.WriteLine($"Step: {i}");
                                //for (int a = 0; a < space.GetLength(0); a++)
                                //{
                                //    for (int b = 0; b < space.GetLength(1); b++)
                                //    {
                                //        Console.Write(space[a, b] ?? '.');
                                //    }
                                //    Console.WriteLine();
                                //}
                                //Console.WriteLine();
                                stepNumber = i;
                                return true;
                            }
                        }
                    }
                }
                return false;
            }).First();

            return stepNumber;
        }

        private static IEnumerable<Robot> ParseRobots(IEnumerable<string> lines)
        {
            foreach (string line in lines)
            {
                Robot robot = new();
                var match = RobotParser().Match(line);
                robot.positionX = int.Parse(match.Groups["posX"].Value);
                robot.positionY = int.Parse(match.Groups["posY"].Value);
                robot.velocityX = int.Parse(match.Groups["velX"].Value);
                robot.velocityY = int.Parse(match.Groups["velY"].Value);

                yield return robot;
            }
        }

        [GeneratedRegex("p=(?'posX'-?\\d+),(?'posY'-?\\d+) v=(?'velX'-?\\d+),(?'velY'-?\\d+)")]
        private static partial Regex RobotParser();

        [GeneratedRegex("(?'sizeX'\\d+),(?'sizeY'\\d+)")]
        private static partial Regex SizeParser();
    }

    public class Robot
    {
        public int positionX;
        public int positionY;
        public int velocityX;
        public int velocityY;
    }

    public class Day14_Input
    {
        public required int sizeX;
        public required int sizeY;
        public required IEnumerable<Robot> robots;
    }
}
