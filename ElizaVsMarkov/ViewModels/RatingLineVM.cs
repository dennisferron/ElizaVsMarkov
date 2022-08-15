using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElizaVsMarkov.ViewModels
{
    public class RatingLineVM : ViewModelBase
    {
        private double x1 = 0.0;

        public double X1
        {
            get { return x1; }
            set
            {
                if (x1 != value)
                {
                    x1 = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private double y1 = 135.0;

        public double Y1
        {
            get { return y1; }
            set
            {
                if (y1 != value)
                {
                    y1 = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private double x2 = 50.0 + Random.Shared.NextDouble()*10;

        public double X2
        {
            get { return x2; }
            set
            {
                if (x2 != value)
                {
                    x2 = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private double y2 = 50.0 + Random.Shared.NextDouble() * 10;

        public double Y2
        {
            get { return y2; }
            set
            {
                if (y2 != value)
                {
                    y2 = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}
