using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElizaVsMarkov.Markov
{
    internal class Tokenizer
    {
        private List<string> indexToWord = new List<string>();
        private Dictionary<string, int> wordToIndex = new Dictionary<string, int>();

        public Tokenizer()
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

        public IEnumerable<int> Tokenize(string input)
        {
            var sentence = input.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            foreach (string word in sentence)
                yield return AddWord(word);
        }

        private string GetWord(int token)
        {
            if (token <= 0 || token >= indexToWord.Count)
                throw new ArgumentOutOfRangeException("token");

            return indexToWord[token];
        }

        public string Detokenize(IEnumerable<int> sequence)
        {
            StringBuilder sb = new StringBuilder();

            bool empty = true;

            foreach (int token in sequence)
            {
                if (!empty)
                    sb.Append(" ");

                sb.Append(GetWord(token));
                empty = false;
            }

            return sb.ToString();
        }
    }
}
