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

            if (Core.Context.User.First(x => x.UserID == State.CurrentUserID).Role.Name == "Admin")
            {
                FrozeMenu.Visibility = Visibility.Visible;
            }
            NameTB.Text += book.Name;
            DescriptionTB.Text += book.Description;
            AuthorTB.Text += book.User.Username;
            GenresTB.Text += book.Genre;
            RatingTB.Text += book.TotalRating;
        }
    }
}
