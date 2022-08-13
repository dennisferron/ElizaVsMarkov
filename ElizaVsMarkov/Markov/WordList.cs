using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElizaVsMarkov.Markov
{
    internal class WordList
    {
        private List<string> indexToWord = new List<string>();
        private Dictionary<string, int> wordToIndex = new Dictionary<string, int>();

        public WordList()
        {
            // Index 0 is a sentinel for sentence start and end.
            indexToWord.Add("<START/STOP>");
        }

        private int AddWord(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
                throw new ArgumentOutOfRangeException(word);

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

        public IEnumerable<int> Tokenize(IEnumerable<string> sentence)
        {
            foreach (string word in sentence)
                yield return AddWord(word);
        }

        private string GetWord(int token)
        {
            if (token <= 0 || token >= indexToWord.Count)
                throw new ArgumentOutOfRangeException("token");

            return indexToWord[token];
        }

        public IEnumerable<string> Detokenize(IEnumerable<int> sequence)
        {
            foreach (int token in sequence)
                yield return GetWord(token);
        }
    }
}
