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

namespace UP_01_Krasnova.Windows
{
    /// <summary>
    /// Логика взаимодействия для WholeBookTextWindow.xaml
    /// </summary>
    public partial class WholeBookTextWindow : Window
    {
        public Book book { get; set; }
        public WholeBookTextWindow(Book book)
        {
            InitializeComponent();

            this.book = book;

            BookNameTB.Text += "'" + book.Name + "'";
            WholeTextTB.Text = book.Body;
        }
    }
}
