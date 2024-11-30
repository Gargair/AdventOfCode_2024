namespace AdventOfCode
{
    public static class Helper
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

    public interface IDaySolution
    {
        void LoadData();
        long Part1();
        long Part2();
    }

    public static class DijkstraaSolver
    {
        public static void CalcDijkstraa(GraphNode start)
        {
            PriorityQueue<GraphNode, int> queue = new();
            queue.Enqueue(start, 0);
            start.distance = 0;
            while (queue.Count > 0)
            {
                GraphNode node = queue.Dequeue();
                if (node.visited)
                {
                    continue;
                }
                node.visited = true;
                if (!node.distance.HasValue)
                {
                    throw new Exception($"Found node to be processed without distance set.");
                }
                foreach (var next in node.next)
                {
                    if (!next.visited)
                    {
                        var newDistance = node.distance.Value + next.cost;
                        if (!next.distance.HasValue || next.distance.Value > newDistance)
                        {
                            next.distance = newDistance;
                            next.predecessor = node;
                            queue.Enqueue(next, newDistance);
                        }
                    }
                }
            }
        }

        public class GraphNode
        {
            public int cost;
            public int? distance;
            public bool visited = false;
            public int directionLength;
            public List<GraphNode> next = [];
            public GraphNode? predecessor = null;
        }
    }
}
