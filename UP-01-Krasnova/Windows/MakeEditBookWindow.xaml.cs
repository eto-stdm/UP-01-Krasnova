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
    /// Логика взаимодействия для MakeEditBookWindow.xaml
    /// </summary>
    public partial class MakeEditBookWindow : Window
    {
        private Book book {  get; set; }
        public MakeEditBookWindow(Book book)
        {
            InitializeComponent();
            this.book = book;


        }
    }
}
