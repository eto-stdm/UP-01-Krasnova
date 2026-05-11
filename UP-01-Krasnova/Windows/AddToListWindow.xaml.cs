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
    /// Логика взаимодействия для AddToListWindow.xaml
    /// </summary>
    public partial class AddToListWindow : Window
    {
        public Book book { get; set; }
        public AddToListWindow(Book book)
        {
            InitializeComponent();
            this.book = book;
            BookNameTB.Text += book.Name;

            List<string> listTypesName = new List<string>();
            foreach (ListType l in Core.Context.ListType)
            {
                listTypesName.Add(l.Name);
            }

            ReadingList changeBook = Core.Context.ReadingList.FirstOrDefault(x => x.UserID == State.CurrentUserID && x.BookID == book.BookID);
            if (changeBook != null)
            {
                FromListCB.Items.Add(changeBook.ListType.Name);
                listTypesName.Remove(changeBook.ListType.Name); // удаляем до привязки
            }
            else
            {
                FromListCB.Items.Add("(пусто)");
            }
            FromListCB.SelectedIndex = 0;
            ToListCB.ItemsSource = listTypesName;
            ToListCB.SelectedIndex = 0;
        }

        private void AddToListBtn_Click(object sender, RoutedEventArgs e)
        {
            ReadingList changeBook = Core.Context.ReadingList.FirstOrDefault(x => x.UserID == State.CurrentUserID && x.BookID == book.BookID);
            if (changeBook != null)
            {
                Core.Context.ReadingList.Remove(changeBook);
            }

            ReadingList newRL = new ReadingList
            {
                UserID = State.CurrentUserID,
                BookID = book.BookID,
                ListTypeID = Core.Context.ListType.First(x => x.Name == ToListCB.Text).ListTypeID
            };
            Core.Context.ReadingList.Add(newRL);
            Core.Context.SaveChanges();

            this.DialogResult = true;
        }
    }
}
