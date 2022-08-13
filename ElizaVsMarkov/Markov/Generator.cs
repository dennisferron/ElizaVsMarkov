using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElizaVsMarkov.Markov
{
    internal class Generator
    {
        private WordList words = new WordList();
        private Digraph forward = new Digraph();
        private Digraph backward = new Digraph();

        private void Learn(IEnumerable<string> sentence, double weight)
        {
            List<int> tokens = words
                .Tokenize(sentence)
                .ToList();

            forward.Learn(tokens, weight);

            IEnumerable<int> rev_tokens = tokens
                .AsEnumerable()
                .Reverse();

            backward.Learn(rev_tokens, weight);
        }

        private IEnumerable<int> WalkFrom(int source)
        {
            if (source == Digraph.START_STOP_TOKEN)
                return forward.WalkFromStart();
            else // Generate sentence from a word.
            {
                return Enumerable.Concat(
                    backward.WalkFrom(source).Reverse(),
                    forward.WalkFrom(source).Skip(1)  
                    // Skip(1) because both include "source".
                );
            }
        }

    }
}
