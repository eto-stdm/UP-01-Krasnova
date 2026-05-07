using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UP_01_Krasnova
{
    public partial class Book
    {
        public double TotalRating
        {
            get
            {
                List<Review> reviews = Core.Context.Review.Where(x => x.BookID == BookID).ToList();
                double reviewAmount = reviews.Count();
                if (reviewAmount == 0) { return 0; }
                else
                {
                    double total = 0;
                    foreach (Review r in reviews)
                    {
                        total += r.Rating;
                    }
                    return total / reviewAmount;
                }
            }
        }
        public string Genre
        {
            get
            {
                List<BookGenre> bookGenres = Core.Context.BookGenre.Where(x => x.BookID == BookID).ToList();
                string genres = "";
                foreach (BookGenre bg in bookGenres)
                {
                    genres += $"{Core.Context.Genre.First(x => x.GenreID == bg.GenreID).Name}, ";
                }
                return genres.Substring(0, genres.Length - 2);
            }
        }
    }
}
