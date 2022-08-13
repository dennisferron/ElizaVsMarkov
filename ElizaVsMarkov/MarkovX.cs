using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace ElizaVsMarkov
{
    internal class MarkovX
    {
        private List<string> indexToWord = new List<string>();
        private Dictionary<string, int> wordToIndex = new Dictionary<string, int>();
        private Dictionary<int, Dictionary<int, double>> backLinks = new Dictionary<int, Dictionary<int, double>>();
        private Dictionary<int, Dictionary<int, double>> foreLinks = new Dictionary<int, Dictionary<int, double>>();

        public MarkovX()
        {
            // Index 0 is a sentinel for sentence start and end.
            indexToWord.Add("<START/STOP>");
        }

        private int AddWord(string word)
        {
            if (wordToIndex.TryGetValue(word, out int index))
                return index;
            else
            {
                index = indexToWord.Count;
                indexToWord.Add(word);
                wordToIndex[word] = index;
                return index;
            }
        }

        private void AddLink(int first, int second)
        {
            backLinks[second][first] += 1.0;
            foreLinks[first][second] += 1.0;
        }

        private int PickTarget(Dictionary<int, double> edges)
        {
            double totalWeight = edges.Values.Sum();
            double targetWeight = totalWeight / 3.0; // TODO: Make this random number 0..totalWeight
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

        private IEnumerable<int> Follow(Dictionary<int, Dictionary<int, double>> links, int source)
        {
            if (source == 0)
                yield break;
            else
            {
                int target = PickTarget(foreLinks[source]);

                yield return target;

                foreach (var next in Follow(links, target))
                    yield return next;
            }
        }

        private IEnumerable<int> ChainFrom(int source)
        {
            if (source == 0)  // Generate sentence from nothing.
                return Follow(foreLinks, PickTarget(foreLinks[0]));
            else // Generate sentence from a word.
                return Enumerable.Concat(
                    Follow(backLinks, source).Reverse(),
                    Follow(foreLinks, source).Skip(1)  // Don't repeat word in the middle.
                );
        }
    }
}
