using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace ElizaVsMarkov.Markov
{
    internal class Digraph
    {
        public const int START_STOP_TOKEN = 0; // Same as WordList "<Start/Stop>"

        private Dictionary<int, Dictionary<int, double>> links = new Dictionary<int, Dictionary<int, double>>();
        private Random random = new Random();

        private void AddLink(int source, int target, double weight)
        {
            if (links.TryGetValue(source, out var edges))
            {
                if (edges.TryGetValue(target, out var old_weight))
                    edges[target] = old_weight + weight;
                else
                    edges[target] = weight;
            }
            else
            {
                links[source] = new Dictionary<int, double> 
                {
                    { target, weight } 
                };
            }
        }

        public void Learn(IEnumerable<int> sequence, double weight)
        {
            using (var iter = sequence.GetEnumerator())
            {
                int prev = START_STOP_TOKEN;

                while (iter.MoveNext())
                {
                    int next = iter.Current;
                    AddLink(prev, next, weight);
                    prev = next;
                }

                AddLink(prev, START_STOP_TOKEN, weight);
            }
        }

        private int PickTarget(int source)
        {
            Dictionary<int, double> edges = links[source];

            double targetWeight = random.NextDouble() * edges.Values.Sum();
            double weightSoFar = 0.0;

            foreach (var edge in edges)
            {
                weightSoFar += edge.Value;
                if (weightSoFar >= targetWeight)
                {
                    return edge.Key;
                }
            }

            return 0;
        }

        public IEnumerable<int> WalkFrom(int source)
        {
            int node = source;

            while (node != START_STOP_TOKEN)
            {
                yield return node;
                node = PickTarget(node);
            }
        }

        public IEnumerable<int> WalkFromStart()
        {
            return WalkFrom(PickTarget(START_STOP_TOKEN));
        }
    }
}
