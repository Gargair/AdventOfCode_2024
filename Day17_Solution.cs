using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventOfCode
{
    internal partial class Day17_Solution : Helper.IDaySolution<Day17_Input, string>
    {
        public Day17_Input LoadData(string inputPath)
        {
            string[] lines = File.ReadAllLines(inputPath + "/Day17.txt");

            return new()
            {
                RegisterA = ulong.Parse(lines[0].Split(": ")[1]),
                RegisterB = ulong.Parse(lines[1].Split(": ")[1]),
                RegisterC = ulong.Parse(lines[2].Split(": ")[1]),
                Program = lines[4].Split(": ")[1].Split(',').Select(s => byte.Parse(s)).ToArray()
            };
        }

        public string Part1(Day17_Input input)
        {
            return string.Join(',', RunProgram(input.Program, input.RegisterA, input.RegisterB, input.RegisterC));
        }

        public string Part2(Day17_Input input)
        {
            string solution = string.Join(',', input.Program);
            if (!solution.EndsWith("5,5,3,0"))
            {
                throw new NotImplementedException("Day 17: The input needs to end with 5,5,3,0 for proper algorithm");
            }
            Dictionary<byte, Dictionary<byte, bool?[]>> lookup = BuildBLookup(input.Program.Take(input.Program.Length - 2).ToArray());

            int bitCount = input.Program.Length * 3 + 12;
            bool?[] definiteRegisterA = new bool?[bitCount];

            List<bool?[]> currentInput = [];
            currentInput.Add(definiteRegisterA);

            //if (File.Exists("Output/Day17.txt"))
            //{
            //    File.Delete("Output/Day17.txt");
            //}

            //var stream = File.OpenWrite("Output/Day17.txt");
            //var writer = new StreamWriter(stream);

            for (int i = 0; i < input.Program.Length; i++)
            {
                //Console.WriteLine($"Round {i + 1}");
                //writer.WriteLine($"Round {i + 1}");
                List<bool?[]> newInput = [];

                foreach (var registerA in currentInput)
                {
                    //writer.WriteLine(string.Join('.', registerA.Reverse().Select(b => b.HasValue ? b.Value ? '1' : '0' : '?').Chunk(3).Select(b => string.Join("", b))));

                    foreach (var next in CheckOutput(input.Program[i], registerA, i * 3, i, lookup))
                    {
                        newInput.Add(next);
                        //writer.Write("\t=> ");
                        //writer.WriteLine(string.Join('.', next.Reverse().Select(b => b.HasValue ? b.Value ? '1' : '0' : '?').Chunk(3).Select(b => string.Join("", b))));
                    }
                }

                currentInput = newInput;
            }

            return currentInput.Select(b => ToNumber(bitCount, b, true)).Min().ToString();
        }

        private static IEnumerable<bool?[]> CheckOutput(byte currentOutput, bool?[] definiteRegisterA, int fixedBits, int processedInputs, Dictionary<byte, Dictionary<byte, bool?[]>> lookup)
        {
            var possibleAOutput = lookup[currentOutput];

            foreach (var possibleA in GetPossibleA(definiteRegisterA, fixedBits))
            {
                if (possibleAOutput.TryGetValue(possibleA, out var aPart))
                {
                    if (FitsInto(definiteRegisterA, aPart, fixedBits))
                    {
                        yield return FitInto(definiteRegisterA, aPart, fixedBits);
                    }
                }
            }
        }

        private static IEnumerable<byte> GetPossibleA(bool?[] bits, int fixedBits)
        {
            for (byte i = 0; i < 8; i++)
            {
                if (SameOrUnsetBit(bits, fixedBits, i % 2 == 1) && SameOrUnsetBit(bits, fixedBits + 1, (i / 2) % 2 == 1) && SameOrUnsetBit(bits, fixedBits + 2, (i / 4) % 2 == 1))
                {
                    yield return i;
                }
            }
        }

        private static bool FitsInto(bool?[] maskBits, bool?[] bitsToFit, int offsetInMask)
        {
            for (int i = 0; i < bitsToFit.Length; i++)
            {
                if (!SameOrUnsetBit(maskBits, offsetInMask + i, bitsToFit[i]))
                {
                    return false;
                }
            }
            return true;
        }

        private static bool?[] FitInto(bool?[] maskBits, bool?[] bitsToFit, int offsetInMask)
        {
            bool?[] newMask = new bool?[maskBits.Length];
            maskBits.CopyTo(newMask, 0);
            for (int i = 0; i < bitsToFit.Length; i++)
            {
                if (bitsToFit[i].HasValue)
                {
                    newMask[offsetInMask + i] = bitsToFit[i];
                }
            }
            return newMask;
        }

        private static bool SameOrUnsetBit(bool?[] bits, int bitNumber, bool? bit)
        {
            return !bits[bitNumber].HasValue || !bit.HasValue || bits[bitNumber].HasValue && bit.HasValue && bits[bitNumber]!.Value == bit.Value;
        }

        private static List<byte> RunProgram(byte[] Program, ulong RegisterA, ulong RegisterB, ulong RegisterC, byte[]? solution = null)
        {
            List<byte> output = [];
            ulong programLength = (ulong)Program.Length;
            for (ulong i = 0; i < programLength; i += 2)
            {
                ulong opCode = Program[i];
                ulong operand = Program[i + 1];

                if (opCode == 0)
                {
                    // adv
                    ulong numerator = RegisterA;
                    ulong denominator = (ulong)Math.Pow(2, GetComboOperand(RegisterA, RegisterB, RegisterC, operand));
                    RegisterA = numerator / denominator;
                }
                else if (opCode == 1)
                {
                    // bxl
                    RegisterB ^= operand;
                }
                else if (opCode == 2)
                {
                    // bst
                    RegisterB = GetComboOperand(RegisterA, RegisterB, RegisterC, operand) % 8;
                }
                else if (opCode == 3)
                {
                    // jnz
                    if (RegisterA != 0)
                    {
                        i = operand - 2;
                    }
                }
                else if (opCode == 4)
                {
                    // bxc
                    RegisterB ^= RegisterC;
                }
                else if (opCode == 5)
                {
                    // out
                    output.Add((byte)(GetComboOperand(RegisterA, RegisterB, RegisterC, operand) % 8));
                    if (solution != null)
                    {
                        if (output.Count > solution.Length)
                        {
                            return [];
                        }
                        if (output[output.Count - 1] != solution[output.Count - 1])
                        {
                            return [];
                        }
                    }
                }
                else if (opCode == 6)
                {
                    // bdv
                    ulong numerator = RegisterA;
                    ulong denominator = (ulong)Math.Pow(2, GetComboOperand(RegisterA, RegisterB, RegisterC, operand));
                    RegisterB = numerator / denominator;
                }
                else if (opCode == 7)
                {
                    // cdv
                    ulong numerator = RegisterA;
                    ulong denominator = (ulong)Math.Pow(2, GetComboOperand(RegisterA, RegisterB, RegisterC, operand));
                    RegisterC = numerator / denominator;
                }
            }
            return output;
        }

        private static ulong GetComboOperand(ulong RegisterA, ulong RegisterB, ulong RegisterC, ulong operand)
        {
            return operand switch
            {
                0 => 0,
                1 => 1,
                2 => 2,
                3 => 3,
                4 => RegisterA,
                5 => RegisterB,
                6 => RegisterC,
                _ => throw new NotImplementedException()
            };
        }

        private static Dictionary<byte, Dictionary<byte, bool?[]>> BuildBLookup(byte[] ProgramWithoutCycle)
        {
            Dictionary<byte, Dictionary<byte, bool?[]>> output = [];
            Dictionary<byte, Dictionary<byte, List<bool[]>>> middle = [];
            int end = (int)Math.Pow(2, 12);
            for (uint i = 0; i < end; i++)
            {
                List<byte> runOutList = RunProgram(ProgramWithoutCycle, i, 0, 0);
                byte runOut = runOutList[0];
                middle.TryAdd(runOut, []);
                byte aEnd = (byte)(i % 8);
                Dictionary<byte, List<bool[]>> sub = middle[runOut];
                sub.TryAdd(aEnd, []);
                var bits = ToBits(12, i);
                sub[aEnd].Add(bits);
            }

            foreach (KeyValuePair<byte, Dictionary<byte, List<bool[]>>> pair in middle)
            {
                output.Add(pair.Key, []);
                var inner = output[pair.Key];
                foreach (KeyValuePair<byte, List<bool[]>> innerPair in pair.Value)
                {
                    inner.Add(innerPair.Key, CombineBits(12, innerPair.Value));
                }
            }

            return output;
        }

        private static bool?[] CombineBits(int bitCount, List<bool[]> candidates)
        {
            bool?[] r = new bool?[bitCount];
            for (int i = 0; i < bitCount; i++)
            {
                if (candidates.All(b => b[i] == true))
                {
                    r[i] = true;
                }
                else if (candidates.All(b => b[i] == false))
                {
                    r[i] = false;
                }
            }
            return r;
        }

        private static bool[] ToBits(int bitCount, long number)
        {
            return Enumerable.Range(0, bitCount).Select(b => (number / (long)Math.Pow(2, b)) % 2 == 1).ToArray();
        }

        private static long ToNumber(int bitCount, bool?[] bits, bool nullAllowed = false)
        {
            return Enumerable.Range(0, bitCount).Select(b => bits[b].HasValue && bits[b]!.Value ? (long)Math.Pow(2, b) : !bits[b].HasValue && !nullAllowed ? throw new Exception("Bit not set") : 0).Sum();
        }
    }

    public class Day17_Input
    {
        public required ulong RegisterA;
        public required ulong RegisterB;
        public required ulong RegisterC;
        public required byte[] Program;
    }
}
