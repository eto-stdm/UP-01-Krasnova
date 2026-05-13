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
    /// Логика взаимодействия для ChangePasswordWindow.xaml
    /// </summary>
    public partial class ChangePasswordWindow : Window
    {
        private User user {  get; set; }
        public ChangePasswordWindow(User user)
        {
            InitializeComponent();
            this.user = user;
        }

        private void ChangePasswordBtn_Click(object sender, RoutedEventArgs e)
        {
            if (user.Password == OldPB.Password)
            {
                user.Password = NewPB.Password;
                Core.Context.SaveChanges();
                this.DialogResult = true;
            }
            else
            {
                MessageBox.Show("Старый пароль был введён некорректно!");
            }
        }
    }
}
