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

            forward.Learn(tokens, weight);
            backward.Learn(tokens.AsEnumerable().Reverse(), weight);

            return tokens;
        }


        public string RespondTo(string input)
        {
            List<int> input_tokens = Learn(input, 1.0);

            IEnumerable<int> output_tokens;

            if (input_tokens.Any())
            {
                int source = input_tokens[random.Next(input_tokens.Count)];

                output_tokens = Enumerable.Concat(
                    backward.WalkFrom(source).Reverse(),    // Both chains include source token 
                    forward.WalkFrom(source).Skip(1)        // so skip one to not repeat it.
                );  // Also, skip is safe here because chain started from a source token cannot be empty. 
            }
            else 
            {
                // Generate a completely random sentence from the forward graph root.
                // This one can be empty if the model learned an empty string.
                output_tokens = forward.WalkFromStart();
            }

            return tokenizer.Detokenize(output_tokens);
        }
    }
}
