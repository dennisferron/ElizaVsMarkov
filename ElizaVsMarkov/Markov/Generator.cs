using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElizaVsMarkov.Markov
{
    internal class Generator
    {
        private Tokenizer tokenizer = new Tokenizer();
        private Digraph forward = new Digraph();
        private Digraph backward = new Digraph();
        private Random random = new Random();

        public List<int> Learn(string input, double weight = 1.0)
        {
            List<int> tokens = tokenizer
                .Tokenize(input)
                .ToList();
                        
            if (tokens.Any()) // Don't learn empty sentences.
            {
                forward.Learn(tokens, weight);
                backward.Learn(tokens.AsEnumerable().Reverse(), weight);
            }

            return tokens;
        }

        private IEnumerable<int> Generate(List<int> input_tokens)
        {
            if (input_tokens.Any())
            {
                int source = input_tokens[random.Next(input_tokens.Count)];

                return Enumerable.Concat(
                    backward.WalkFrom(source).Reverse(),    // Both chains include source token 
                    forward.WalkFrom(source).Skip(1)        // so skip one to not repeat it.
                );  // Also, skip is safe here because chain started from a source token cannot be empty. 
            }
            else
            {
                // Generate a completely random sentence from the forward graph root.
                // This one can be empty if the model learned an empty string.
                return forward.WalkFromStart();
            }
        }

        public string RespondTo(string input)
        {
            List<int> input_tokens = Learn(input, 1.0);

            // First try to generate a sentence from the input, but different.
            const int MAX_TRIES = 100;
            for (int i=0; i < MAX_TRIES; i++)
            {
                List<int> output_tokens = Generate(input_tokens).ToList();

                if (output_tokens.SequenceEqual(input_tokens))
                    continue;

                double avg_len = 0.5 * (input_tokens.Count + output_tokens.Count);
                double len_dif = Math.Abs(input_tokens.Count - output_tokens.Count);

                if (len_dif > 0.40 * avg_len)
                    continue;

                return tokenizer.Detokenize(output_tokens);
            }

            // If that doesn't work, just make something up.
            return tokenizer.Detokenize(forward.WalkFromStart());
        }
    }
}
