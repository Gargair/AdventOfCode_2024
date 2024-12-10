namespace AdventOfCode
{
    internal class Day10_Solution : IDaySolution
    {
        public Day10_Solution() { }

        byte[][]? block;

        public void LoadData()
        {
            block = lines.Select(line => line.Select(c => byte.Parse(c.ToString())).ToArray()).ToArray();
        }

        public long Part1()
        {
            if (block == null)
            {
                throw new InvalidOperationException("You have to call LoadData() before Part1()");
            }
            long sum = 0;
            for (int i = 0; i < block.Length; i++)
            {
                for (int j = 0; j < block[i].Length; j++)
                {
                    if (block[i][j] == 0)
                    {
                        sum += DetermineTrailScore(block, i, j);
                    }
                }
            }
            return sum;
        }

        public long Part2()
        {
            if (block == null)
            {
                throw new InvalidOperationException("You have to call LoadData() before Part2()");
            }
            long sum = 0;
            for (int i = 0; i < block.Length; i++)
            {
                for (int j = 0; j < block[i].Length; j++)
                {
                    if (block[i][j] == 0)
                    {
                        sum += DetermineTrailRating(block, i, j);
                    }
                }
            }
            return sum;
        }

        private static long DetermineTrailScore(byte[][] block, int startX, int startY)
        {
            bool[,] visited = new bool[block.Length, block[0].Length];
            Queue<Tuple<int, int>> toDo = new();
            toDo.Enqueue(Tuple.Create(startX, startY));
            while (toDo.Count > 0)
            {
                Tuple<int, int> current = toDo.Dequeue();
                if (!visited[current.Item1, current.Item2])
                {
                    visited[current.Item1, current.Item2] = true;
                    if (HasNextHeight(block, current.Item1, current.Item2, -1, 0))
                    {
                        toDo.Enqueue(Tuple.Create(current.Item1 - 1, current.Item2));
                    }
                    if (HasNextHeight(block, current.Item1, current.Item2, 0, 1))
                    {
                        toDo.Enqueue(Tuple.Create(current.Item1, current.Item2 + 1));
                    }
                    if (HasNextHeight(block, current.Item1, current.Item2, 1, 0))
                    {
                        toDo.Enqueue(Tuple.Create(current.Item1 + 1, current.Item2));
                    }
                    if (HasNextHeight(block, current.Item1, current.Item2, 0, -1))
                    {
                        toDo.Enqueue(Tuple.Create(current.Item1, current.Item2 - 1));
                    }
                }
            }
            long count = 0;
            for (int i = 0; i < block.Length; i++)
            {
                for (int j = 0; j < block[i].Length; j++)
                {
                    if (visited[i, j] && block[i][j] == 9)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        private static bool HasNextHeight(byte[][] block, int currentX, int currentY, int deltaX, int deltaY)
        {
            if (currentX < 0 || currentX >= block.Length || currentY < 0 || currentY >= block[0].Length)
            {
                return false;
            }
            int nextX = currentX + deltaX;
            int nextY = currentY + deltaY;
            if (nextX < 0 || nextX >= block.Length || nextY < 0 || nextY >= block[0].Length)
            {
                return false;
            }
            byte currentHeight = block[currentX][currentY];
            byte nextHeight = block[nextX][nextY];
            if (currentHeight + 1 == nextHeight)
            {
                return true;
            }
            return false;
        }

        private static long DetermineTrailRating(byte[][] block, int startX, int startY)
        {
            if(startX < 0 || startX >= block.Length || startY < 0 || startY >= block[0].Length)
            {
                return 0;
            }
            if (block[startX][startY] == 9)
            {
                return 1;
            }
            long sum = 0;
            if(HasNextHeight(block, startX, startY, -1, 0))
            {
                sum += DetermineTrailRating(block, startX - 1, startY);
            }
            if(HasNextHeight(block, startX, startY, 0, 1))
            {
                sum += DetermineTrailRating(block, startX, startY + 1);
            }
            if(HasNextHeight(block, startX, startY, 1, 0))
            {
                sum += DetermineTrailRating(block, startX + 1, startY);
            }
            if(HasNextHeight(block, startX, startY, 0, -1))
            {
                sum += DetermineTrailRating(block, startX, startY - 1);
            }
            return sum;
        }

        private static readonly string[] lines = @"0987667654101034989654101432348985610123
1296558983290125678723296541201276765434
2345054321785678732012387890320345896521
3432169100634019141003456345410450787010
6589078234521121056934321256521061071012
1670123310130434567856780767639872310329
0121038981676549654015491898948765421458
0232347432987238723723321782358994530567
1249856521096189019854430651467087649898
4356761014545012345766526710546123456787
5892398723634101876667815897657650147696
6701401658723210961054904108898941038987
4321512349014589752344393219765432121876
5480659834305679834543287001257654100565
6598768743289098721672156100348943210430
7890457650176187610981043234567801223221
6721300540245234534590189878956900334109
7872211231234503425610014561843219445678
8963430343210212018701123890760178587654
7454545654789301329632106721252987696503
6309634785689467612543215430341070123412
5218729896676598503454123436012678986543
4300012987589003454569098587983509677892
3011201056432112167678767696894412589001
2176312340101238098989454345765303478121
0985433421221549196562398210010210365430
1834329854330678187401347890121091236521
2349010765445589012301256321432188767810
3458921321876432101212565430545679658901
8967439410962103456703456980698320147872
7876548568985412549873567871787210238963
9845607678676543432112876012345623547654
4701016569871012561003985321498734569345
5612023423407871679654987430567015678210
4543121016512960988765496549012321076501
5432130987323450345676701238765434989432
6787042346636781212489865430987045672341
5896851256745890004599874121078135101650
0125960167894321123673456012169234326789
1034878998765010012982107893458765012678".Split(Environment.NewLine);
    }
}
