using System.Collections.Concurrent;

namespace AdventOfCode
{
    internal partial class Day19_Solution : Helper.IDaySolution
    {
        public Day19_Input LoadData(string inputFolder)
        {
            string[] lines = File.ReadAllLines(inputFolder + "/Day19.txt");

            return new()
            {
                towels = lines[0].Split(", ").Select(t => t.ToCharArray()).ToArray(),
                patterns = lines.Skip(2).Select(p => p.ToCharArray()).ToArray()
            };
        }

        public override string Part1(string inputPath)
        {
            Day19_Input input = LoadData(inputPath);
            ConcurrentDictionary<int, ConcurrentDictionary<string, bool>> cache = [];
            int count = 0;
            foreach (char[] towel in input.towels)
            {
                cache.TryAdd(towel.Length, []);
                cache[towel.Length].TryAdd(string.Join("", towel), true);
            }
            for (int i = 0; i < input.patterns.Length; i++)
            {
                if (IsPatternPossible(input.patterns[i], input.towels, 0, cache))
                {
                    count++;
                }
            }
            return count.ToString();
        }

        public override string Part2(string inputPath)
        {
            Day19_Input input = LoadData(inputPath);
            ConcurrentDictionary<int, ConcurrentDictionary<string, long>> cache = [];
            long count = 0;
            for (int i = 0; i < input.patterns.Length; i++)
            {
                count += PatternAmountPossible(input.patterns[i], input.towels, 0, cache);
            }
            return count.ToString();
        }

        public static bool IsPatternPossible(char[] pattern, char[][] towels, int startInPattern, ConcurrentDictionary<int, ConcurrentDictionary<string, bool>> patternPossible)
        {
            string rest = string.Join("", pattern.Skip(startInPattern));
            int restLength = rest.Length;
            if (patternPossible.TryGetValue(restLength, out var rests) && rests.TryGetValue(rest, out bool result))
            {
                return result;
            }
            if (startInPattern == pattern.Length)
            {
                return true;
            }
            foreach (char[] towel in towels)
            {
                if (TowelFitsInPattern(pattern, towel, startInPattern))
                {
                    if (IsPatternPossible(pattern, towels, startInPattern + towel.Length, patternPossible))
                    {
                        patternPossible.TryAdd(restLength, []);
                        patternPossible[restLength].TryAdd(rest, true);
                        return true;
                    }
                }
            }
            patternPossible.TryAdd(restLength, []);
            patternPossible[restLength].TryAdd(rest, false);
            return false;
        }

        public static long PatternAmountPossible(char[] pattern, char[][] towels, int startInPattern, ConcurrentDictionary<int, ConcurrentDictionary<string, long>> patternPossible)
        {
            string rest = string.Join("", pattern.Skip(startInPattern));
            int restLength = rest.Length;
            if (patternPossible.TryGetValue(restLength, out var rests) && rests.TryGetValue(rest, out long result))
            {
                return result;
            }
            if (startInPattern == pattern.Length)
            {
                return 1;
            }
            long count = 0;
            foreach (char[] towel in towels)
            {
                if (TowelFitsInPattern(pattern, towel, startInPattern))
                {
                    count += PatternAmountPossible(pattern, towels, startInPattern + towel.Length, patternPossible);
                }
            }
            patternPossible.TryAdd(restLength, []);
            patternPossible[restLength].TryAdd(rest, count);
            return count;
        }

        private static bool TowelFitsInPattern(char[] pattern, char[] towel, int startInPattern)
        {
            for (int i = 0; i < towel.Length; i++)
            {
                if (i + startInPattern >= pattern.Length)
                {
                    return false;
                }
                if (pattern[i + startInPattern] != towel[i])
                {
                    return false;
                }
            }
            return true;
        }

    }

    public class Day19_Input
    {
        public required char[][] towels;
        public required char[][] patterns;
    }
}
