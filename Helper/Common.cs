namespace AdventOfCode.Helper
{
    public class Common
    {
        /**
         * Greatest common divisor
         */
        public static long GCD(long _z1, long _z2)
        {
            long z1 = _z1;
            long z2 = _z2;
            while (z1 % z2 != 0)
            {
                var tmp = z1 % z2;
                z1 = z2;
                z2 = tmp;
            }
            return z2;
        }

        /**
         *  Least common multiple
         */
        public static long LCM(long z1, long z2)
        {
            if (z1 == 0 || z2 == 0) return 0;
            return (z1 * z2) / GCD(z1, z2);
        }
    }

    public interface IDaySolution<T>
    {
        T LoadData(string inputFolder);
        long Part1(T input);
        long Part2(T input);
    }
}
