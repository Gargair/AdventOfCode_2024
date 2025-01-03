namespace AdventOfCode.Helper
{
    public static class DijkstraaSolver
    {
        public static void CalcDijkstraa(IGraphNode start)
        {
            PriorityQueue<IGraphNode, long> queue = new();
            queue.Enqueue(start, 0);
            start.Distance = 0;
            while (queue.Count > 0)
            {
                IGraphNode node = queue.Dequeue();
                if (node.Visited)
                {
                    continue;
                }
                node.Visited = true;
                if (!node.Distance.HasValue)
                {
                    throw new Exception($"Found node to be processed without distance set.");
                }
                foreach (IGraphEdge edge in node.Next)
                {
                    IGraphNode next = edge.To;
                    if (!next.Visited)
                    {
                        long newDistance = node.Distance.Value + next.Cost + edge.Cost;
                        if (!next.Distance.HasValue || next.Distance.Value > newDistance)
                        {
                            next.Distance = newDistance;
                            next.Predecessors.Add(edge);
                            queue.Enqueue(next, newDistance);
                        }
                        else if (!next.Distance.HasValue || next.Distance.Value == newDistance)
                        {
                            next.Predecessors.Add(edge);
                            queue.Enqueue(next, newDistance);
                        }
                    }
                }
            }
        }

        public interface IGraphNode
        {
            public long Cost { get; }
            public long? Distance { get; set; }
            public bool Visited { get; set; }
            public List<IGraphEdge> Next { get; }
            public List<IGraphEdge> Predecessors { get; }

            public void Reset();
        }

        public interface IGraphEdge
        {
            public long Cost { get; }
            public IGraphNode To { get; }
        }

        public class GraphNode : IGraphNode
        {
            public long cost;
            public long? distance;
            public bool visited = false;
            public List<IGraphEdge> next = [];
            public List<IGraphEdge> predecessors = [];

            public long Cost { get { return this.cost; } set { this.cost = value; } }
            public long? Distance { get { return this.distance; } set { this.distance = value; } }
            public bool Visited { get { return this.visited; } set { this.visited = value; } }
            public List<IGraphEdge> Next { get { return this.next; } }
            public List<IGraphEdge> Predecessors { get { return this.predecessors; } }

            public void Reset()
            {
                if (this.visited)
                {
                    this.visited = false;
                    this.distance = null;
                    this.predecessors = [];
                    foreach (var edge in this.next)
                    {
                        edge.To.Reset();
                    }
                }
            }
        }

        public class GraphEdge : IGraphEdge
        {
            public long cost;
            public required IGraphNode to;

            public long Cost
            {
                get { return this.cost; }
            }

            public IGraphNode To
            {
                get { return this.to; }
            }
        }
    }
}