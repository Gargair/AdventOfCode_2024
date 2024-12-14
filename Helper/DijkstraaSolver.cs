namespace AdventOfCode.Helper
{
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