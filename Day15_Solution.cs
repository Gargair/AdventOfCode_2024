using static System.Net.Mime.MediaTypeNames;

namespace AdventOfCode
{
    internal partial class Day15_Solution : Helper.IDaySolution<Day15_Input, long>
    {
        public Day15_Input LoadData(string inputPath)
        {
            string[] lines = File.ReadAllLines(inputPath + "/Day15.txt");
            return new()
            {
                warehouse = lines.TakeWhile(line => !string.IsNullOrWhiteSpace(line)).Select(line => line.ToCharArray()).ToArray(),
                robotInput = string.Join("", lines.SkipWhile(line => !string.IsNullOrWhiteSpace(line)))
            };
        }

        public long Part1(Day15_Input input)
        {
            int[] current = Helper.MatrixHelper.FindFirstElement(input.warehouse, '@');
            foreach (char direction in input.robotInput)
            {
                current = MoveInDirection(input.warehouse, current[0], current[1], direction);
            }
            long sum = 0;
            for (int i = 0; i < input.warehouse.Length; i++)
            {
                for (int j = 0; j < input.warehouse[i].Length; j++)
                {
                    if (input.warehouse[i][j] == 'O')
                    {
                        sum += i * 100 + j;
                    }
                }
            }
            return sum;
        }

        public long Part2(Day15_Input input)
        {
            char[,] warehouse = new char[input.warehouse.Length, input.warehouse[0].Length * 2];
            for (int i = 0; i < input.warehouse.Length; i++)
            {
                for (int j = 0; j < input.warehouse[i].Length; j++)
                {
                    if (input.warehouse[i][j] == 'O')
                    {
                        warehouse[i, 2 * j] = '[';
                        warehouse[i, 2 * j + 1] = ']';
                    }
                    else if (input.warehouse[i][j] == '.')
                    {
                        warehouse[i, 2 * j] = '.';
                        warehouse[i, 2 * j + 1] = '.';
                    }
                    else if (input.warehouse[i][j] == '#')
                    {
                        warehouse[i, 2 * j] = '#';
                        warehouse[i, 2 * j + 1] = '#';
                    }
                    else if (input.warehouse[i][j] == '@')
                    {
                        warehouse[i, 2 * j] = '@';
                        warehouse[i, 2 * j + 1] = '.';
                    }
                }
            }
            int[] current = Helper.MatrixHelper.FindFirstElement(warehouse, '@');
            foreach (char direction in input.robotInput)
            {
                current = MoveInDirectionPart2(warehouse, current[0], current[1], direction);
            }
            long sum = 0;
            for (int i = 0; i < warehouse.GetLength(0); i++)
            {
                for (int j = 0; j < warehouse.GetLength(1); j++)
                {
                    if (warehouse[i, j] == '[')
                    {
                        sum += i * 100 + j;
                    }
                }
            }
            return sum;
        }

        private static int[] MoveInDirection(char[][] warehouse, int currentX, int currentY, char direction)
        {
            int[] delta = GetDirectionDelta(direction);
            int nextX = currentX + delta[0];
            int nextY = currentY + delta[1];

            if (Helper.MatrixHelper.IsInMatrix(warehouse, nextX, nextY))
            {
                if (warehouse[nextX][nextY] == '.')
                {
                    // Free: Move robot
                    warehouse[nextX][nextY] = '@';
                    warehouse[currentX][currentY] = '.';
                    return [nextX, nextY];
                }
                else if (warehouse[nextX][nextY] == '#')
                {
                    // Wall: Do nothing
                }
                else if (warehouse[nextX][nextY] == 'O')
                {
                    // Box: Check for space after box
                    int boxX = nextX;
                    int boxY = nextY;
                    while (warehouse[boxX][boxY] == 'O')
                    {
                        boxX += delta[0];
                        boxY += delta[1];
                    }
                    if (warehouse[boxX][boxY] == '.')
                    {
                        // Free after row of boxes: move boxes
                        warehouse[boxX][boxY] = 'O';
                        warehouse[nextX][nextY] = '@';
                        warehouse[currentX][currentY] = '.';
                        return [nextX, nextY];
                    }
                    else if (warehouse[boxX][boxY] == '#')
                    {
                        // Wall after boxes: do nothing
                    }
                }
            }
            return [currentX, currentY];
        }

        private static int[] MoveInDirectionPart2(char[,] warehouse, int currentX, int currentY, char direction)
        {
            int[] delta = GetDirectionDelta(direction);
            int nextX = currentX + delta[0];
            int nextY = currentY + delta[1];

            if (Helper.MatrixHelper.IsInMatrix(warehouse, nextX, nextY))
            {
                if (warehouse[nextX, nextY] == '.')
                {
                    // Free: Move robot
                    warehouse[nextX, nextY] = '@';
                    warehouse[currentX, currentY] = '.';
                    return [nextX, nextY];
                }
                else if (warehouse[nextX, nextY] == '#')
                {
                    // Wall: Do nothing
                }
                else if (warehouse[nextX, nextY] == '[' || warehouse[nextX, nextY] == ']')
                {
                    // Box: Check for spaces after box
                    List<Tuple<int, int>> pushBlocks = [];
                    Queue<Tuple<int, int>> toCheck = [];

                    toCheck.Enqueue(Tuple.Create(nextX, nextY));
                    if (warehouse[nextX, nextY] == '[')
                    {
                        toCheck.Enqueue(Tuple.Create(nextX, nextY + 1));
                    }
                    else if (warehouse[nextX, nextY] == ']')
                    {
                        toCheck.Enqueue(Tuple.Create(nextX, nextY - 1));
                    }
                    bool canPush = true;
                    while (toCheck.Count > 0)
                    {
                        Tuple<int, int> box = toCheck.Dequeue();
                        if (!pushBlocks.Contains(box))
                        {
                            pushBlocks.Add(box);
                        }
                        Tuple<int, int> nextBox = Tuple.Create(box.Item1 + delta[0], box.Item2 + delta[1]);
                        if (!pushBlocks.Contains(nextBox))
                        {
                            if (warehouse[nextBox.Item1, nextBox.Item2] == '.')
                            {
                                // Found empty space. All ok
                                continue;
                            }
                            else if (warehouse[nextBox.Item1, nextBox.Item2] == '[')
                            {
                                // Found another box. Add to check.
                                toCheck.Enqueue(nextBox);
                                toCheck.Enqueue(Tuple.Create(nextBox.Item1, nextBox.Item2 + 1));
                            }
                            else if (warehouse[nextBox.Item1, nextBox.Item2] == ']')
                            {
                                // Found another box. Add to check.
                                toCheck.Enqueue(nextBox);
                                toCheck.Enqueue(Tuple.Create(nextBox.Item1, nextBox.Item2 - 1));
                            }
                            else if (warehouse[nextBox.Item1, nextBox.Item2] == '#')
                            {
                                // Found wall. Nothing pushed.
                                canPush = false;
                                break;
                            }
                        }

                    }
                    if (canPush)
                    {
                        pushBlocks.Reverse();
                        foreach (Tuple<int, int> pushBlock in pushBlocks)
                        {
                            warehouse[pushBlock.Item1 + delta[0], pushBlock.Item2 + delta[1]] = warehouse[pushBlock.Item1, pushBlock.Item2];
                            warehouse[pushBlock.Item1, pushBlock.Item2] = '.';
                        }
                        warehouse[nextX, nextY] = '@';
                        warehouse[currentX, currentY] = '.';
                        return [nextX, nextY];
                    }
                }
            }
            return [currentX, currentY];
        }

        private static int[] GetDirectionDelta(char direction)
        {
            return direction switch
            {
                '^' => [-1, 0],
                '>' => [0, 1],
                'v' => [1, 0],
                '<' => [0, -1],
                _ => [0, 0],
            };
        }

    }

    public class Day15_Input
    {
        public required char[][] warehouse;
        public required string robotInput;
    }
}
