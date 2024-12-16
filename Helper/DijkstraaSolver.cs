using System.Security.Cryptography;

namespace AdventOfCode.Helper
{
    public static class DijkstraaSolver
    {
        public static void CalcDijkstraa<T>(T start) where T : GraphNode
        {
            PriorityQueue<GraphNode, long> queue = new();
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
                foreach (var edge in node.next)
                {
                    GraphNode next = edge.to;
                    if (!next.visited)
                    {
                        var newDistance = node.distance.Value + next.cost + edge.cost;
                        if (!next.distance.HasValue || next.distance.Value > newDistance)
                        {
                            next.distance = newDistance;
                            next.predecessors = [node];
                            queue.Enqueue(next, newDistance);
                        }
                        else if (!next.distance.HasValue || next.distance.Value == newDistance)
                        {
                            next.predecessors.Add(node);
                            queue.Enqueue(next, newDistance);
                        }
                    }
                }
            }
        }

        public class GraphNode
        {
            public long cost;
            public long? distance;
            public bool visited = false;
            public List<GraphEdge> next = [];
            public List<GraphNode> predecessors = [];
        }

        public class GraphEdge
        {
            public long cost;
            public required GraphNode to;
        }
    }
}