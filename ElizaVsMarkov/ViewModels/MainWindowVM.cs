using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
                new ReactionVM(Reactions.Crying, "Images/sad_emoji.jpg"),
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
            ChatLog.Add(new ChatMessageVM
            {
                User = user,
                Message = message,
                Reaction = null
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
            if (IsSuggestionEnabled)
            {
                AddToLog("Human", SuggestionText);
                SuggestionText = "";
                IsSuggestionEnabled = false;
                IsReactionEnabled = true;
            }
        }

        private bool isSuggestionEnabled = true;
        public bool IsSuggestionEnabled
        {
            get { return isSuggestionEnabled; }
            set { isSuggestionEnabled = value; }
        }

        private bool isReactionEnabled = false;
        public bool IsReactionEnabled
        {
            get { return isReactionEnabled; }
            set { isReactionEnabled = value; }
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
                if (_selectedReaction != value && IsReactionEnabled)
                {
                    _selectedReaction = value;
                    NotifyPropertyChanged();

                    // Also display the selection on the message so we can see it update in realtime.
                    var markovMsg = ChatLog.LastOrDefault(a => a.User == "Markov");
                    if (markovMsg != null)
                    {
                        markovMsg.Reaction = value;
                    }
                }
            }
        }

        public void SendReaction()
        {
            if (IsReactionEnabled)
            {
                var messageVM = ChatLog.LastOrDefault(a => a.User == "Markov");

                if (messageVM != null)
                    messageVM.Reaction = SelectedReaction;

                BetweenRatings.AddRating(SelectedReaction.Reaction);

                IsSuggestionEnabled = true;
                IsReactionEnabled = false;

                ReactionCompleted();

                // Clear any previous text.
                SuggestionText = "";
            }
        }

        public void ReactionCompleted()
        {
            string markovResult = ChatLog.LastOrDefault(a => a.User == "Markov")?.Message ?? "";
            string elizaResult = eliza.GetResponse(markovResult);
            AddToLog("ELIZA", elizaResult);
        }

        public void MarkovMessageCompleted()
        {
            BottomPanelMode = 1;
        }

        public void ElizaMessageCompleted()
        {
            BottomPanelMode = 0;
        }

        public void HumanMessageCompleted()
        {
            string suggestionText = ChatLog.LastOrDefault(a => a.User == "Human")?.Message ?? "";
            string markovResult = markov.RespondTo(SuggestionText ?? "");
            AddToLog("Markov", markovResult);
        }

        public static readonly DependencyProperty ScrollOffsetProperty =
           DependencyProperty.Register("ScrollOffset", typeof(double), typeof(MainWindowVM),
           new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(OnScrollOffsetChanged)));

        // Check MainWindow.xaml.cs property change event if changing the name of this.
        public double ScrollOffset
        {
            get { return (double)GetValue(ScrollOffsetProperty); }
            set { SetValue(ScrollOffsetProperty, value); }
        }

        private static void OnScrollOffsetChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            MainWindowVM myObj = obj as MainWindowVM;

            // Handled by MainWindow.xaml.cs because it needs reference to scroll viewer UI element.
            if (myObj != null)
                myObj.NotifyPropertyChanged("ScrollOffset");
        }

        private int bottomPanelMode;

        public int BottomPanelMode
        {
            get { return bottomPanelMode; }
            set 
            {
                if (bottomPanelMode != value)
                {
                    bottomPanelMode = value;
                    NotifyPropertyChanged();

                    if (bottomPanelMode == 0)
                    {
                        IsSuggestionEnabled = true;
                        IsReactionEnabled = false;
                    }
                    else 
                    {
                        IsSuggestionEnabled = false;
                        IsReactionEnabled = true;
                    }
                }
            }
        }


        #region Between Rating Lines
        public BetweenRatingVM BetweenRatings
        {
            get; private set;
        } = new BetweenRatingVM();
        #endregion
    }
}


