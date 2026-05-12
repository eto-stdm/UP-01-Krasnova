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
    /// Логика взаимодействия для BookListPage.xaml
    /// </summary>
    public partial class BookListPage : Page
    {
        List<Book> curbooks = new List<Book>(); // хранятся книги после поиска (фильтр и сортировка не перезаписывают его)
        List<Book> lastSelection = new List<Book>();
        public BookListPage()
        {
            InitializeComponent();

            List<ReadingList> r = Core.Context.ReadingList.Where(x => x.UserID == State.CurrentUserID && x.ListType.Name == "Читаю").ToList();
            foreach (var list in r)
            {
                if (Core.Context.Book.FirstOrDefault(x => x.BookID == list.BookID && x.IsFrozen == false) != null)
                {
                    curbooks.Add(Core.Context.Book.FirstOrDefault(x => x.BookID == list.BookID && x.IsFrozen == false));
                    lastSelection.Add(Core.Context.Book.FirstOrDefault(x => x.BookID == list.BookID && x.IsFrozen == false));
                }
            }
            BooksLB.ItemsSource = curbooks;

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
                    curbooks = lastSelection.Where(x => x.Name.ToLower() == search.ToLower() && x.IsFrozen == false).ToList();
                }

                if (type == "Author")
                {
                    curbooks = lastSelection.Where(x => x.User.Username.ToLower() == search.ToLower() && x.IsFrozen == false).ToList();
                }
            }
            else
            {
                curbooks = lastSelection;
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e) // обработчик для поиска
        {
            BooksLB.ItemsSource = null;
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
                        if (b.BookID == bg.BookID && b.IsFrozen == false)
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

        private void ChangeList_Click(object sender, RoutedEventArgs e)
        {
            var butn = (Button)sender;
            List<ReadingList> newList = new List<ReadingList>();
            BooksLB.ItemsSource = null;
            curbooks.Clear();
            lastSelection.Clear();
            switch (butn.Name)
            {
                case "ListReadingBtn":
                    newList = Core.Context.ReadingList.Where(x => x.UserID == State.CurrentUserID && x.ListType.Name == "Читаю").ToList();
                    break;
                case "ListInPlansBtn":
                    newList = Core.Context.ReadingList.Where(x => x.UserID == State.CurrentUserID && x.ListType.Name == "В планах").ToList();
                    break;
                case "ListReadBtn":
                    newList = Core.Context.ReadingList.Where(x => x.UserID == State.CurrentUserID && x.ListType.Name == "Прочитано").ToList();
                    break;
                case "ListDroppedBtn":
                    newList = Core.Context.ReadingList.Where(x => x.UserID == State.CurrentUserID && x.ListType.Name == "Заброшено").ToList();
                    break;
                default: break;
            }

            foreach (var list in newList)
            {
                if (Core.Context.Book.FirstOrDefault(x => x.BookID == list.BookID && x.IsFrozen == false) != null)
                {
                    curbooks.Add(Core.Context.Book.FirstOrDefault(x => x.BookID == list.BookID && x.IsFrozen == false));
                    //lastSelection.Add(Core.Context.Book.First(x => x.BookID == list.BookID));
                }
            }
            BooksLB.ItemsSource = curbooks;
        }
    }
}
