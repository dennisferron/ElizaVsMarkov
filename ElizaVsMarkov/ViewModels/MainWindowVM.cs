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
            string elizaJson = File.ReadAllText(elizaScriptFile);
            eliza = new ELIZALib(elizaJson);

            // Start (TODO: Let tutorial invoke this.)
            AddToLog("ELIZA", eliza.Session.GetGreeting());

            var start = DateTime.Now;
            string[] training = File.ReadAllLines(markovCorpusFile);
            foreach (var line in training)
                markov.Learn(line, 0.002);
            var end = DateTime.Now;
            var span = end - start;
            AddToLog("System", String.Format("Loaded Markov training data in {0} seconds", span.TotalSeconds));
        }

        private void AddToLog(string user, string message)
        {
            ChatLog.Add(new ChatMessageVM
            {
                User = user,
                Message = message,
                Reaction = Reactions.None
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
    }
}
