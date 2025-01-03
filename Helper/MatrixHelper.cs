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

        public static bool IsInMatrix<T>(T[][] matrix, Tuple<int, int> pos)
        {
            return IsInMatrix(matrix, pos.Item1, pos.Item2);
        }

        public static bool IsInMatrix<T>(T[,] matrix, Tuple<int, int> pos)
        {
            return IsInMatrix(matrix, pos.Item1, pos.Item2);
        }

        public static Tuple<int, int>? FindFirstElement<T>(T[][] matrix, T element)
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    if (EqualityComparer<T>.Default.Equals(matrix[i][j], element))
                    {
                        return Tuple.Create(i, j);
                    }
                }
            }
            return null;
        }

        public static Tuple<int,int>? FindFirstElement<T>(T[,] matrix, T element)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (EqualityComparer<T>.Default.Equals(matrix[i, j], element))
                    {
                        return Tuple.Create(i, j);
                    }
                }
            }
            return null;
        }

        public static bool IsInDirection<T>(T[][] matrix, int startX, int startY, int deltaX, int deltaY, T[] toSearch)
        {
            for (int l = 0; l < toSearch.Length; l++)
            {
                int posX = startX + deltaX * l;
                if (posX < 0 || posX >= matrix.Length)
                {
                    return false;
                }
                int posY = startY + deltaY * l;
                if (posY < 0 || posY >= matrix[posX].Length)
                {
                    return false;
                }
                if (!EqualityComparer<T>.Default.Equals(matrix[posX][posY], toSearch[l]))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsInDirection<T>(T[,] matrix, int startX, int startY, int deltaX, int deltaY, T[] toSearch)
        {
            for (int l = 0; l < toSearch.Length; l++)
            {
                int posX = startX + deltaX * l;
                if (posX < 0 || posX >= matrix.GetLength(0))
                {
                    return false;
                }
                int posY = startY + deltaY * l;
                if (posY < 0 || posY >= matrix.GetLength(1))
                {
                    return false;
                }
                if (!EqualityComparer<T>.Default.Equals(matrix[posX, posY], toSearch[l]))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
