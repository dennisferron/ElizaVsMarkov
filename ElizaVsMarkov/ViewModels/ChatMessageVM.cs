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

        private Reactions _reaction;

        public Reactions Reaction
        {
            get { return _reaction; }
            set { _reaction = value; }
        }

    }
}
