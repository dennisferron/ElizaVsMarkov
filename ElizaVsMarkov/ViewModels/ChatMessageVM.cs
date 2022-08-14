using ElizaVsMarkov.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
