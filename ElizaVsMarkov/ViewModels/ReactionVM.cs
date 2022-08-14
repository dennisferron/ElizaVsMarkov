using ElizaVsMarkov.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElizaVsMarkov.ViewModels
{
    public class ReactionVM
    {
        public ReactionVM(Reactions reaction, string imageFile)
        {
            Reaction = reaction;
            ImageFile = imageFile;
        }

        public Reactions Reaction
        {
            get; set;
        }

        public string ImageFile
        {
            get; set;
        }
    }
}
