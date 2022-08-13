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
                "At_the_Mountains_of_Madness-Lovecraft.txt"
            );
            DataContext = viewModel;
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.SendSuggestionText();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                viewModel.SendSuggestionText();
            }
        }
    }
}
