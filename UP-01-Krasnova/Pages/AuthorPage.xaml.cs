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
using UP_01_Krasnova.Windows;

namespace UP_01_Krasnova.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthorPage.xaml
    /// </summary>
    public partial class AuthorPage : Page
    {
        public AuthorPage()
        {
            InitializeComponent();
        }

        private void ChangeList_Click(object sender, RoutedEventArgs e)
        {
            //BooksLB.ItemsSource = null;
            var butn = (Button)sender;
            List<Book> books = new List<Book>();
            switch (butn.Name)
            {
                case "ShowPublishedBtn":
                    books = Core.Context.Book.Where(x => x.IsFrozen == false).ToList();
                    EditBookBtn.Visibility = Visibility.Visible;
                    UnfreezeBookBtn.Visibility = Visibility.Collapsed;
                    break;
                case "ShowFrozenBtn":
                    books = Core.Context.Book.Where(x => x.IsFrozen == true).ToList();
                    EditBookBtn.Visibility = Visibility.Collapsed;
                    UnfreezeBookBtn.Visibility = Visibility.Visible;
                    break;
                default: break;
            }
            BooksLB.ItemsSource = books;
        }

        private void NewBookBtn_Click(object sender, RoutedEventArgs e)
        {
            Book book = null;
            var window = new MakeEditBookWindow(book);
            if (window.ShowDialog() == true)
            {
                MessageBox.Show($"Книга успешно создана!");
            }
        }

        private void EditBookBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Перед изменением книги убедитесь, что вы выделили её (выбранная книга имеет серый фон) и нажмите 'да'.",
                "Выделена ли книга?",
                MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                if (BooksLB.SelectedItem != null)
                {
                    var book = BooksLB.SelectedItem as Book;

                    var window = new MakeEditBookWindow(book);
                    if (window.ShowDialog() == true)
                    {
                        MessageBox.Show($"Книга успешно отредактированна!");
                    }
                }
                else { MessageBox.Show("Книга не выбрана!"); }
            }
        }

        private void UnfreezeBookBtn_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult result = MessageBox.Show(
                "Перед отправкой запроса на разморозку книги убедитесь, что вы выделили её (выбранная книга имеет серый фон) и нажмите 'да'.",
                "Выделена ли книга?",
                MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                if (BooksLB.SelectedItem != null)
                {
                    var book = BooksLB.SelectedItem as Book;
                    if (Core.Context.UnFreezeApplication.FirstOrDefault(x => x.BookID == book.BookID) == null)
                    {
                        UnFreezeApplication app = new UnFreezeApplication
                        {
                            ApplicationDate = DateTime.Now,
                            BookID = book.BookID,
                            StatusID = 1
                        };
                        Core.Context.UnFreezeApplication.Add(app);
                        Core.Context.SaveChanges();
                    }
                    else { MessageBox.Show("Заявка на разморозку этой книги уже подана!"); }
                }
                else { MessageBox.Show("Книга не выбрана!"); }
            }
        }
    }
}
