namespace AdventOfCode.Helper
{
    public static class MatrixHelper
    {
        public static bool IsInMatrix<T>(T[,] matrix, int x, int y)
        {
            return x >= 0 && x < matrix.GetLength(0) && y >= 0 && y < matrix.GetLength(1);
        }

        public static bool IsInMatrix<T>(T[][] matrix, int x, int y)
        {
            return x >= 0 && x < matrix.Length && y >= 0 && y < matrix[0].Length;
        }

        public static int[] FindFirstElement<T>(T[][] matrix, T element)
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    if (EqualityComparer<T>.Default.Equals(matrix[i][j], element))
                    {
                        return [i, j];
                    }
                }
            }
            return [-1, -1];
        }

        public static int[] FindFirstElement<T>(T[,] matrix, T element)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (EqualityComparer<T>.Default.Equals(matrix[i, j], element))
                    {
                        return [i, j];
                    }
                }
            }
            return [-1, -1];
        }
    }
}
