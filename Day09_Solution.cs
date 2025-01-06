using System.Numerics;

namespace AdventOfCode
{
    internal class Day09_Solution : Helper.IDaySolution
    {
        public string LoadData(string inputPath)
        {
            return File.ReadAllText(inputPath + "/Day09.txt");
        }

        public override string Part1(string inputPath)
        {
            string idNumber = LoadData(inputPath);
            long sum = 0;
            int rightIndex = idNumber.Length - 1;
            int rightAmount = ((byte)idNumber[rightIndex] - (byte)'0');
            int leftIndex = 0;
            int fileSystemIndex = 0;
            while (leftIndex < rightIndex)
            {
                if (leftIndex % 2 == 0)
                {
                    // File
                    int fileAmount = ((byte)idNumber[leftIndex] - (byte)'0');
                    int fileIndex = leftIndex / 2;
                    for (int i = 0; i < fileAmount; i++)
                    {
                        sum += fileSystemIndex * fileIndex;
                        fileSystemIndex++;
                    }
                }
                else
                {
                    // Empty space
                    int emptySpaceSpace = ((byte)idNumber[leftIndex] - (byte)'0');
                    int fileIndex = (rightIndex + 1) / 2;
                    for (int i = 0; i < emptySpaceSpace && leftIndex < rightIndex; i++)
                    {
                        if (rightAmount > 0)
                        {
                            sum += fileSystemIndex * fileIndex;
                            fileSystemIndex++;
                            rightAmount--;
                        }
                        else if (leftIndex < rightIndex - 1)
                        {
                            rightIndex -= 2;
                            fileIndex = (rightIndex + 1) / 2;
                            rightAmount = ((byte)idNumber[rightIndex] - (byte)'0');
                            i--;
                        }
                    }
                }
                leftIndex++;
            }
            while (rightAmount > 0)
            {
                int fileNumber = (rightIndex + 1) / 2;
                sum += fileSystemIndex * fileNumber;
                fileSystemIndex++;
                rightAmount--;
            }
            return sum.ToString();
        }

        public override string Part2(string inputPath)
        {
            string idNumber = LoadData(inputPath);
            long sum = 0;
            bool[] filesConsumed = new bool[(idNumber.Length + 1) / 2];
            int leftIndex = 0;
            int fileSystemIndex = 0;
            while (leftIndex < idNumber.Length)
            {
                if (leftIndex % 2 == 0)
                {
                    // File
                    int fileIndex = leftIndex / 2;
                    int fileAmount = ((byte)idNumber[leftIndex] - (byte)'0');
                    if (!filesConsumed[fileIndex])
                    {
                        filesConsumed[fileIndex] = true;
                        for (int i = 0; i < fileAmount; i++)
                        {
                            sum += fileSystemIndex * fileIndex;
                            fileSystemIndex++;
                        }
                    }
                    else
                    {
                        fileSystemIndex += fileAmount;
                    }
                }
                else
                {
                    // Empty space
                    int emptySpaceSpace = ((byte)idNumber[leftIndex] - (byte)'0');
                    for (int i = idNumber.Length - 1; i >= leftIndex && emptySpaceSpace > 0; i -= 2)
                    {
                        int fileIndex = (i + 1) / 2;
                        if (!filesConsumed[fileIndex])
                        {
                            int fileAmount = ((byte)idNumber[i] - (byte)'0');
                            if (fileAmount <= emptySpaceSpace)
                            {
                                filesConsumed[fileIndex] = true;
                                emptySpaceSpace -= fileAmount;
                                for (int k = 0; k < fileAmount; k++)
                                {
                                    sum += fileSystemIndex * fileIndex;
                                    fileSystemIndex++;
                                }
                            }
                        }
                    }
                    fileSystemIndex += emptySpaceSpace;
                }
                leftIndex++;
            }
            return sum.ToString();
        }
    }
}
