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
    /// Логика взаимодействия для MakeEditBookWindow.xaml
    /// </summary>
    public partial class MakeEditBookWindow : Window
    {
        private Book book {  get; set; }
        public MakeEditBookWindow(Book book)
        {
            InitializeComponent();
            this.book = book;
            List<Box> box = new List<Box>();

            if (book != null)
            {
                HeaderTB.Text = "Редактирование книги";
                NameTB.Text = book.Name;
                DescriptionTB.Text = book.Description;
                TextTB.Text = book.Body;
                CoverTB.Text = book.Cover;
                MakeEditBookBtn.Content = "Применить изменения";

                List<BookGenre> bg = Core.Context.BookGenre.Where(x => x.BookID == book.BookID).ToList();
                foreach (Genre g in Core.Context.Genre)
                {
                    Box tempBox;
                    if (bg.FirstOrDefault(x => x.GenreID == g.GenreID) != null) //у книги есть выбранный жанр
                    {
                        tempBox = new Box
                        {
                            ID = g.GenreID,
                            Name = g.Name,
                            Check = true
                        };
                    }
                    else
                    {
                        tempBox = new Box
                        {
                            ID = g.GenreID,
                            Name = g.Name,
                            Check = false
                        };
                    }
                    box.Add(tempBox);
                }
            }
            else
            {
                foreach (Genre g in Core.Context.Genre)
                {
                    box.Add(new Box { ID = g.GenreID, Name = g.Name, Check = false });
                }
            }
            GenresLB.ItemsSource = box;
        }

        private void MakeBook()
        {
            book = new Book
            {
                Name = NameTB.Text,
                Description = DescriptionTB.Text,
                Cover = CoverTB.Text,
                Body = TextTB.Text,
                AuthorID = State.CurrentUserID,
                IsFrozen = false,
            };
            Core.Context.Book.Add(book);

            foreach (Box item in GenresLB.ItemsSource)
            {
                if (item.Check)
                {
                    BookGenre bg = new BookGenre
                    {
                        BookID = book.BookID,
                        GenreID = item.ID
                    };
                    Core.Context.BookGenre.Add(bg);
                }
            }
            Core.Context.SaveChanges();
        }

        private void EditBook()
        {
            book.Name = NameTB.Text;
            book.Description = DescriptionTB.Text;
            book.Body = TextTB.Text;
            book.Cover = CoverTB.Text;

            foreach (Box item in GenresLB.ItemsSource)
            {
                if (item.Check) // выбранно
                {
                    if (Core.Context.BookGenre.FirstOrDefault(x => x.BookID == book.BookID && x.GenreID == item.ID) == null) // нет записи
                    {
                        BookGenre bg = new BookGenre
                        {
                            BookID = book.BookID,
                            GenreID = item.ID
                        };
                        Core.Context.BookGenre.Add(bg);
                    }
                }
                else // не выбранно
                {
                    if (Core.Context.BookGenre.FirstOrDefault(x => x.BookID == book.BookID && x.GenreID == item.ID) != null) // есть запись
                    {
                        Core.Context.BookGenre.Remove(Core.Context.BookGenre.FirstOrDefault(x => x.BookID == book.BookID && x.GenreID == item.ID));
                    }
                }
            }
            Core.Context.SaveChanges();
        }

        private void MakeEditBookBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(NameTB.Text) || string.IsNullOrEmpty(DescriptionTB.Text) ||
                string.IsNullOrEmpty(TextTB.Text) || string.IsNullOrEmpty(CoverTB.Text))
            {
                MessageBox.Show("Заполните все поля!");
            }
            else
            {
                if (book == null) { MakeBook(); }
                else { EditBook(); }
                this.DialogResult = true;
            }
        }
    }
}
