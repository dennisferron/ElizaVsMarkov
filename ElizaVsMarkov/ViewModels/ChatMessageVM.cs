using ElizaVsMarkov.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ElizaVsMarkov.ViewModels
{
    public class ChatMessageVM : ViewModelBase
    {
		private string _user;

		public string User
		{
			get { return _user; }
			set 
			{
				if (_user != value)
				{
                    _user = value;
					NotifyPropertyChanged();
                }
            }
		}

        private string _message;

        public string Message
        {
            get { return _message; }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private ReactionVM _reaction;

        public ReactionVM Reaction
        {
            get { return _reaction; }
            set 
            {
                if (_reaction != value)
                {
                    _reaction = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged("EmojiImage");
                }
            }
        }

        public string EmojiImage
        {
            get
            {
                return Reaction?.ImageFile;
            }
        }

        public string AnimatedMessage
        {
            get
            {
                if (Message == null)
                    return "";
                else
                {
                    int numLetters = Convert.ToInt32(Message.Length * TypingProgress);
                    return Message.Substring(0, numLetters);
                }
            }
        }


        public double TypingProgress
        {
            get { return (double)GetValue(TypingProgressProperty); }
            set { SetValue(TypingProgressProperty, value); }
        }
        public static readonly DependencyProperty TypingProgressProperty =
            DependencyProperty.Register("TypingProgress", typeof(double), typeof(ChatMessageVM), new PropertyMetadata(1D, OnTypingProgressChangedCallBack)
           );

        private static void OnTypingProgressChangedCallBack(
                DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ChatMessageVM c = sender as ChatMessageVM;
            if (c != null)
            {
                c.OnTypingProgressChanged();
            }
        }

        protected virtual void OnTypingProgressChanged()
        {
            NotifyPropertyChanged("AnimatedMessage");
        }

        public string TypingAnimationTime 
        { 
            get 
            {
                return string.Format("0:0:{0}", 0.06 * _message.Length);
            }
        }
    }
}
