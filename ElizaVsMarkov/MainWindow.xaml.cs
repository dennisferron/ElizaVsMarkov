using ElizaVsMarkov.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ElizaVsMarkov
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowVM viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new MainWindowVM(
                "DOCTOR.json",
                "Corpus\\training.txt"
            );

            viewModel.PropertyChanged += ViewModel_PropertyChanged;

            DataContext = viewModel;
        }

        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "ScrollOffset":
                    this.BottomPanelScrollViewer.ScrollToVerticalOffset(viewModel.ScrollOffset);
                    break;
                case "BottomPanelMode":
                    switch (viewModel.BottomPanelMode)
                    {
                        case 0:
                            {
                                BeginStoryboard sb = this.FindResource("BottomPanelStoryboardUp") as BeginStoryboard;
                                sb.Storyboard.Begin();
                                break;
                            }
                        case 1:
                            {
                                BeginStoryboard sb = this.FindResource("BottomPanelStoryboardDown") as BeginStoryboard;
                                sb.Storyboard.Begin();
                                break;
                            }
                    }
                    break;
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.SendSuggestionText();
            chatLogView.ScrollIntoView(viewModel.ChatLog.Last());
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                viewModel.SendSuggestionText();
                chatLogView.ScrollIntoView(viewModel.ChatLog.Last());
            }
        }

        private void ReactionBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                viewModel.SendReaction();
                chatLogView.ScrollIntoView(viewModel.ChatLog.Last());
            }
        }

        private void RateButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.SendReaction();
            chatLogView.ScrollIntoView(viewModel.ChatLog.Last());
        }

        private void MarkovMessageStoryboard_Completed(object sender, EventArgs e)
        {
            viewModel.MarkovMessageCompleted();
            chatLogView.ScrollIntoView(viewModel.ChatLog.Last());
            ReactionBox.Focus();
        }

        private void ElizaMessageStoryboard_Completed(object sender, EventArgs e)
        {
            viewModel.ElizaMessageCompleted();
            chatLogView.ScrollIntoView(viewModel.ChatLog.Last());
            SuggestionText.Focus();
        }

        private void HumanMessageStoryboard_Completed(object sender, EventArgs e)
        {
            viewModel.HumanMessageCompleted();
            chatLogView.ScrollIntoView(viewModel.ChatLog.Last());
        }
    }
}
