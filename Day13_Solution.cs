using System.Text.RegularExpressions;

namespace AdventOfCode
{
    internal partial class Day13_Solution : Helper.IDaySolution<IEnumerable<ClawMachine>, long>
    {
        public IEnumerable<ClawMachine> LoadData(string inputPath)
        {
            string[] lines = File.ReadAllLines(inputPath + "/Day13.txt");
            ClawMachine machine = new();
            foreach (string line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    yield return machine;
                    machine = new();
                    continue;
                }
                var match = PositionParser().Match(line);
                var type = match.Groups["Type"].Value;
                var x = long.Parse(match.Groups["X"].Value);
                var y = long.Parse(match.Groups["Y"].Value);

                if (type == "Button A")
                {
                    machine.aDeltaX = x;
                    machine.aDeltaY = y;
                }
                else if (type == "Button B")
                {
                    machine.bDeltaX = x;
                    machine.bDeltaY = y;
                }
                else if (type == "Prize")
                {
                    machine.prizeX = x;
                    machine.prizeY = y;
                }
                else
                {
                    Console.WriteLine("Invalid input line: " + line);
                }
            }
            yield return machine;
        }

        public long Part1(IEnumerable<ClawMachine> machines)
        {
            // c1 * ax + c2 * bx = px
            // c1 * ay + c2 * by = py

            // c1 * ax + c2 * bx = px
            // c2 * by * ax - c2 * bx * ay = py * ax - px * ay
            // c2 * (by * ax - bx * ay) = py * ax - px * ay
            // c2 = py * ax - px * ay / (by * ax - bx * ay)
            long sum = 0;

            foreach (ClawMachine machine in machines)
            {
                long dividend = machine.bDeltaY * machine.aDeltaX - machine.bDeltaX * machine.aDeltaY;
                long top = machine.prizeY * machine.aDeltaX - machine.prizeX * machine.aDeltaY;
                if (dividend != 0)
                {
                    if (top % dividend == 0)
                    {
                        long c2 = top / dividend;
                        long c1 = (machine.prizeX - c2 * machine.bDeltaX) / machine.aDeltaX;
                        if (c1 <= 100 && c2 <= 100)
                        {
                            sum += c1 * 3 + c2 * 1;
                        }
                    }
                    else
                    {
                        // No solution
                    }
                }
                else
                {
                    // Multiple solutions (or no solutions). Determine minimum in price. Does not happen in input, so omitted
                    Console.WriteLine("Mutiple");
                }
            }
            return sum;
        }

        public long Part2(IEnumerable<ClawMachine> machines)
        {
            long sum = 0;

            foreach (ClawMachine machine in machines)
            {
                machine.prizeX += 10000000000000L;
                machine.prizeY += 10000000000000L;
                long dividend = machine.bDeltaY * machine.aDeltaX - machine.bDeltaX * machine.aDeltaY;
                long top = machine.prizeY * machine.aDeltaX - machine.prizeX * machine.aDeltaY;
                if (dividend != 0)
                {
                    if (top % dividend == 0)
                    {
                        long c2 = top / dividend;
                        if((machine.prizeX - c2 * machine.bDeltaX) % machine.aDeltaX != 0)
                        {
                            // c1 is fractional => no solution
                            continue;
                        }
                        long c1 = (machine.prizeX - c2 * machine.bDeltaX) / machine.aDeltaX;
                        sum += c1 * 3 + c2 * 1;
                    }
                    else
                    {
                        // No solution
                    }
                }
                else
                {
                    // Multiple solutions (or no solutions). Determine minimum in price. Does not happen in input, so omitted
                    Console.WriteLine("Mutiple");
                }
            }
            return sum;
        }

        [GeneratedRegex("(?'Type'[\\w\\s]+): X[\\+=](?'X'\\d+), Y[\\+=](?'Y'\\d+)")]
        private static partial Regex PositionParser();
    }

    internal class ClawMachine
    {
        public long aDeltaX;
        public long aDeltaY;
        public long bDeltaX;
        public long bDeltaY;
        public long prizeX;
        public long prizeY;
    }
}
