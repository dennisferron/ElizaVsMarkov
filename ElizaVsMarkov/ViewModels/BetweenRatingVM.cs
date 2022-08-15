using ElizaVsMarkov.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Point = System.Windows.Point;

namespace ElizaVsMarkov.ViewModels
{
    public class BetweenRatingVM : ViewModelBase
    {
        public BetweenRatingVM()
        {
            ratings[Reactions.None] = 0;
            ratings[Reactions.LoveIt] = 1;
            ratings[Reactions.Hilarious] = 1;
            ratings[Reactions.Meh] = 1;
            ratings[Reactions.Crying] = 1;
            ratings[Reactions.WTF] = 1;

            UpdatePoints();
        }

        private Dictionary<Reactions, double> ratings = new Dictionary<Reactions, double>();

        private double GetAngle(Reactions reaction)  // in radians
        {
            switch (reaction)
            {
                case Reactions.LoveIt:
                    return 1.3;
                case Reactions.Hilarious:
                    return 0.9;
                case Reactions.Meh:
                    return 0.0;
                case Reactions.Crying:
                    return -0.9;
                case Reactions.WTF:
                    return -1.3;
                default:
                    return 0;
            }
        }

        private const double OriginY = 135;

        private Point GetPoint(Reactions reaction)
        {
            double angle = GetAngle(reaction);
            double len = ScaleRating( ratings[reaction] );

            double x = Math.Cos(angle) * len;
            double y = Math.Sin(angle) * len;

            // Flip y because in canvas positive is down,
            // but for sine positive was up.
            y = -y;

            // Origin is at y=135 in the graphic.
            y += OriginY;

            return new Point(x, y);
        }

        private double ScaleRating(double input)
        {
            double scale_factor = (50 / ratings.Values.Average());

            return scale_factor * input;
        }

        public void AddRating(Reactions reaction)
        {
            ratings[reaction] += 1;
            UpdatePoints();
        }

        public void UpdatePoints()
        {
            double min_len = ScaleRating( ratings.Values.Min() );
            double avg_len = ScaleRating(ratings.Values.Average());
            double first_len = ScaleRating(ratings[Reactions.LoveIt]);
            double last_len = ScaleRating(ratings[Reactions.WTF]);

            PointCollection pc = new PointCollection();

            // Top
            double top_len = first_len / 2.0;
            pc.Add(new Point(0, OriginY - top_len));

            // Ratings
            pc.Add(UpdatePoint(RatingLine1, Reactions.LoveIt));
            pc.Add(UpdatePoint(RatingLine2, Reactions.Hilarious));
            pc.Add(UpdatePoint(RatingLine3, Reactions.Meh));
            pc.Add(UpdatePoint(RatingLine4, Reactions.Crying));
            pc.Add(UpdatePoint(RatingLine5, Reactions.WTF));

            // Bottom
            double bot_len =  last_len / 2.0;
            pc.Add(new Point(0, OriginY + last_len));


            this.Points = pc;


        }

        Point UpdatePoint(RatingLineVM ratingLine, Reactions reaction)
        {
            Point p = GetPoint(reaction);
            ratingLine.X1 = 0.0;
            ratingLine.Y1 = OriginY;
            ratingLine.X2 = p.X;
            ratingLine.Y2 = p.Y;
            return p;
        }

        private PointCollection points = new PointCollection();
        public PointCollection Points
        {
            get { return points; }
            set
            {
                points = value;
                NotifyPropertyChanged();
            }
        }

        #region Rating Lines 
        public RatingLineVM RatingLine1
        {
            get; private set;
        } = new RatingLineVM();

        public RatingLineVM RatingLine2
        {
            get; private set;
        } = new RatingLineVM();

        public RatingLineVM RatingLine3
        {
            get; private set;
        } = new RatingLineVM();

        public RatingLineVM RatingLine4
        {
            get; private set;
        } = new RatingLineVM();

        public RatingLineVM RatingLine5
        {
            get; private set;
        } = new RatingLineVM();
        #endregion
    }
}
