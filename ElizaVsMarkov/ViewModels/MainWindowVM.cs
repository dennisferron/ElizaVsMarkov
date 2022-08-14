using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELIZA.NET;
using ElizaVsMarkov.Model;

namespace ElizaVsMarkov.ViewModels
{
    public class MainWindowVM : ViewModelBase
    {
        private ELIZALib eliza = null;
        private Markov.Generator markov = new Markov.Generator();

        public MainWindowVM(string elizaScriptFile, string markovCorpusFile)
        {
            ReactionButtons = new List<ReactionVM>
            {
                new ReactionVM(Reactions.LoveIt, "Images/craiyon_190726_LoveIt_emoji__br_.png"),
                new ReactionVM(Reactions.Hilarious, "Images/craiyon_200908_laughin_emoji_br_.png"),
                new ReactionVM(Reactions.Meh, "Images/craiyon_184522_Disinterested_emoji__br_.png"),
                new ReactionVM(Reactions.Crying, "Images/craiyon_200016_crying_emoji_br_.png"),
                new ReactionVM(Reactions.WTF, "Images/craiyon_190753_Shock_emoji__br_.png")
            };

            string elizaJson = File.ReadAllText(elizaScriptFile);
            eliza = new ELIZALib(elizaJson);

            // Start (TODO: Let tutorial invoke this.)
            AddToLog("ELIZA", eliza.Session.GetGreeting());

            string[] training = File.ReadAllLines(markovCorpusFile);
            foreach (var line in training)
                markov.Learn(line, 0.002);
        }

        private void AddToLog(string user, string message)
        {
            var reaction = ReactionButtons[ChatLog.Count % 5];

            ChatLog.Add(new ChatMessageVM
            {
                User = user,
                Message = message,
                Reaction = reaction
            }); 
        }

        private string _suggestionText;
        public string SuggestionText
        {
            get { return _suggestionText; }
            set 
            {
                if (_suggestionText != value)
                {
                    _suggestionText = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ObservableCollection<ChatMessageVM> ChatLog
        { get; set; } = new ObservableCollection<ChatMessageVM>();

        public void SendSuggestionText()
        {
            AddToLog("Human", SuggestionText);
            string markovResult = markov.RespondTo(SuggestionText??"");
            AddToLog("Markov", markovResult);
            SuggestionText = "";
            string elizaResult = eliza.GetResponse(markovResult);
            AddToLog("ELIZA", elizaResult);
        }

        public List<ReactionVM> ReactionButtons
        {
            get; private set;
        }

        private ReactionVM _selectedReaction;
        public ReactionVM SelectedReaction
        {
            get { return _selectedReaction; }
            set 
            { 
                if (_selectedReaction != value)
                {
                    _selectedReaction = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public void SendReaction()
        {
            var messageVM = ChatLog.LastOrDefault(a => a.User == "Markov");

            if (messageVM != null)
                messageVM.Reaction = SelectedReaction;
        }
    }
}
