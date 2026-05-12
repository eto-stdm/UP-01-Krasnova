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
    /// Логика взаимодействия для BookCatalogPage.xaml
    /// </summary>
    public partial class BookCatalogPage : Page
    {
        List<Book> curbooks = Core.Context.Book.ToList(); // хранятся книги после поиска (фильтр и сортировка не перезаписывают его)
        public BookCatalogPage()
        {
            InitializeComponent();

            List<Book> unfrozenBooks = Core.Context.Book.Where(x => x.IsFrozen == false).ToList();
            BooksLB.ItemsSource = unfrozenBooks;

            List<string> genreName = new List<string>();
            foreach (Genre g in Core.Context.Genre.ToList())
            {
                genreName.Add(g.Name);
            }
            genreName.Add("(нет)");
            genreName.Sort();
            GenreFilterCB.ItemsSource = genreName;
        }

        private void Search(string type, string search)
        {
            GenreFilterCB.SelectedIndex = 0; // сброс фильтров
            if (!string.IsNullOrEmpty(SearchTB.Text))
            {
                if (type == "Name")
                {
                    curbooks = Core.Context.Book.Where(x => x.Name.ToLower() == search.ToLower()).ToList();
                }

                if (type == "Author")
                {
                    curbooks = Core.Context.Book.Where(x => x.User.Username.ToLower() == search.ToLower()).ToList();
                }
            }
            else 
            {
                curbooks = Core.Context.Book.ToList();
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e) // обработчик для поиска
        {
            var butn = (Button)sender;
            switch (butn.Name)
            {
                case "SearchByNameBtn":
                    Search("Name", SearchTB.Text);
                    break;
                case "SearchByAuthorBtn":
                    Search("Author", SearchTB.Text);
                    break;
                default: break;
            }
            BooksLB.ItemsSource = curbooks;
        }

        private void Filter_SelectionChanged(object sender, SelectionChangedEventArgs e) // обработчик для фильтра
        {
            var combo = (ComboBox)sender;
            if (combo.SelectedIndex == 0)
            {
                BooksLB.ItemsSource = curbooks;
            }
            else
            {
                int genreid = Core.Context.Genre.First(x => x.Name == combo.SelectedItem.ToString()).GenreID;
                List<BookGenre> bookGenres = Core.Context.BookGenre.Where(x => x.GenreID == genreid).ToList();

                List<Book> selectedBooks = new List<Book>();
                foreach (Book b in curbooks)
                {
                    foreach (BookGenre bg in bookGenres)
                    {
                        if (b.BookID == bg.BookID)
                        {
                            selectedBooks.Add(b);
                        }
                    }
                }
                BooksLB.ItemsSource = selectedBooks;
            }
        }

        private void Sort_Click(object sender, RoutedEventArgs e) // обработчик для сортировки
        {
            var butn = (MenuItem)sender;
            List<Book> changedBooks = new List<Book>();
            switch (butn.Name)
            {
                case "Default":
                    BooksLB.ItemsSource = curbooks;
                    return;
                case "NameSortUp":
                    changedBooks = curbooks.OrderBy(x => x.Name).ToList();
                    break;
                case "NameSortDown":
                    changedBooks = curbooks.OrderByDescending(x => x.Name).ToList();
                    break;
                case "RatingSortUp":
                    changedBooks = curbooks.OrderBy(x => x.TotalRating).ToList();
                    break;
                case "RatingSortDown":
                    changedBooks = curbooks.OrderByDescending(x => x.TotalRating).ToList();
                    break;
                default: break;
            }
            BooksLB.ItemsSource = changedBooks;
        }

        private void BooksLB_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectBook = BooksLB.SelectedItem as Book;

            if (selectBook == null) return;

            BookPage page = new BookPage(selectBook);

            NavigationService.Navigate(page);
        }

        private void AddToListBtn_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button?.Tag is Book book)
            {
                var window = new AddToListWindow(book);
                if (window.ShowDialog() == true)
                {
                    MessageBox.Show($"Книга '{book.Name}' добавлена в список {window.ToListCB.Text}");
                }
            }
        }
    }
}
