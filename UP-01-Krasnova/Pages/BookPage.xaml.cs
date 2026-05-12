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
using UP_01_Krasnova.Classes;
using UP_01_Krasnova.Windows;

namespace UP_01_Krasnova.Pages
{
    /// <summary>
    /// Логика взаимодействия для BookPage.xaml
    /// </summary>
    public partial class BookPage : Page
    {
        public Book book { get; set; }
        public BookPage(Book book)
        {
            InitializeComponent();
            this.book = book;

            if (Core.Context.Review.FirstOrDefault(x => x.BookID == book.BookID && x.UserID == State.CurrentUserID) != null)
            {
                ReviewBtn.Visibility = Visibility.Collapsed;
            }

            if (Core.Context.User.First(x => x.UserID == State.CurrentUserID).Role.Name == "Admin")
            {
                FrozeMenu.Visibility = Visibility.Visible;
            }
            NameTB.Text += book.Name;
            DescriptionTB.Text += book.Description;
            AuthorTB.Text += book.User.Username;
            GenresTB.Text += book.Genre;
            RatingTB.Text += book.TotalRating;

            ReviewsLB.ItemsSource = Core.Context.Review.Where(x => x.BookID == book.BookID).ToList();
        }

        private void ReadBtn_Click(object sender, RoutedEventArgs e)
        {
            var window = new WholeBookTextWindow(book);
            window.ShowDialog();
        }

        private void ReviewBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Core.Context.Review.FirstOrDefault(x => x.BookID == book.BookID && x.UserID == State.CurrentUserID) == null)
            {
                var window = new MakeReviewWindow(book);
                //window.ShowDialog();
                if (window.ShowDialog() == true)
                {
                    MessageBox.Show($"Вы оставили отзыв на книгу '{book.Name}'");
                }
            }
            else { MessageBox.Show("Вы уже оставляли отзыв на эту книгу!"); }
        }

        private void Report_Click(object sender, RoutedEventArgs e)
        {
            var butn = (MenuItem)sender;
            string objType = null;
            object newObj = null;
            bool caughtDup = false;
            switch (butn.Name)
            {
                case "ReportBookMI":
                    if (Core.Context.Report.FirstOrDefault(x => x.BookID == book.BookID && x.ComplainantID == State.CurrentUserID) == null)
                    {
                        objType = "книгу";
                        newObj = book;
                    }
                    else { MessageBox.Show("Этот пользователь уже оставлял жалобу на эту книгу!"); caughtDup = true; }
                    break;
                case "ReportAuthorMI":
                    if (Core.Context.Report.FirstOrDefault(x => x.UserID == book.AuthorID && x.ComplainantID == State.CurrentUserID) == null)
                    {
                        objType = "автора";
                        newObj = Core.Context.User.First(x => x.UserID == book.AuthorID);
                    }
                    else { MessageBox.Show("Этот пользователь уже оставлял жалобу на этого автора!"); caughtDup = true; }
                    break;
                case "ReportReviewMI":
                    if (butn?.Tag is Review review)
                    {
                        if (Core.Context.Report.FirstOrDefault(x => x.ReviewID == review.ReviewID && x.ComplainantID == State.CurrentUserID) == null)
                        {
                            newObj = review;
                            objType = "отзыв";
                        }
                        else { MessageBox.Show("Этот пользователь уже оставлял жалобу на этот отзыв!"); caughtDup = true; }
                    }
                    break;
                default: break;
            }

            if (!caughtDup)
            {
                var window = new MakeReportWindow(objType, newObj);
                if (window.ShowDialog() == true)
                {
                    MessageBox.Show($"Жалоба отправлена!");
                }
            }
        }

        private void Froze_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
