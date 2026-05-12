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
using System.Windows.Shapes;
using UP_01_Krasnova.Classes;

namespace UP_01_Krasnova.Windows
{
    /// <summary>
    /// Логика взаимодействия для MakeReviewWindow.xaml
    /// </summary>
    public partial class MakeReviewWindow : Window
    {
        public Book book {  get; set; }
        public MakeReviewWindow(Book book)
        {
            InitializeComponent();

            this.book = book;
            BookNameTB.Text += book.Name;

            List<string> ratingNums = new List<string>();
            for (int i = 1; i < 11; i++) { ratingNums.Add(i.ToString()); }
            RatingCB.ItemsSource = ratingNums;
        }

        private void MakeReviewBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TextTB.Text))
            {
                Review newReview = new Review
                {
                    BookID = book.BookID,
                    UserID = State.CurrentUserID,
                    Rating = Convert.ToInt32(RatingCB.Text),
                    Text = TextTB.Text,
                    Date = DateTime.Now,
                };
                Core.Context.Review.Add(newReview);
                Core.Context.SaveChanges();

                this.DialogResult = true;
            }
            else { MessageBox.Show("Заполните текст отзыва!"); }
        }
    }
}
