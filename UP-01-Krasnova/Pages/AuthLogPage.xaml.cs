using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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
    /// Логика взаимодействия для AuthLogPage.xaml
    /// </summary>
    public partial class AuthLogPage : Page
    {
        public AuthLogPage()
        {
            InitializeComponent();
        }

        private bool ParseEmail(string email)
        {
            try
            {
                MailAddress newemail = new MailAddress(email);
                return true;
            }
            catch { return false; }
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LoginLoginTB.Text) || string.IsNullOrWhiteSpace(LoginPasswordTB.Text))
            {
                MessageBox.Show("Заполните необходимые поля!");
            }
            else
            {
                List<User> users = Core.Context.User.ToList();

                User curruser = users.FirstOrDefault(u => u.Login == LoginLoginTB.Text);
                if (curruser != null && curruser.Password == LoginPasswordTB.Text)
                {
                    State.CurrentUserID = curruser.UserID;
                    NavigationService.Navigate(new MainPage());
                }
                else { MessageBox.Show("Неправильный логин или пароль"); }
            }
        }

        private void RegistrBtn_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrWhiteSpace(RegLoginTB.Text) ||
                string.IsNullOrWhiteSpace(RegPasswordTB.Text) ||
                string.IsNullOrWhiteSpace(RegEmailTB.Text) ||
                string.IsNullOrWhiteSpace(RegPasswordTB.Text))
            {
                MessageBox.Show("Данные не заполнены!");
            }
            else
            {
                List<User> users = Core.Context.User.ToList();
                User failUser = users.FirstOrDefault(x => x.Login == RegLoginTB.Text);
                if (failUser != null)
                {
                    MessageBox.Show("Пользователь с данным логином уже существует");
                }
                else
                {
                    if (ParseEmail(RegEmailTB.Text))
                    {
                        User newUser = new User()
                        {
                            Login = RegLoginTB.Text,
                            Password = RegPasswordTB.Text,
                            Email = RegEmailTB.Text,
                            Username = RegUsernameTB.Text,
                            RoleID = 3,
                            IsFrozen = false,
                        };
                        Core.Context.User.Add(newUser);
                        Core.Context.SaveChanges();

                        users = Core.Context.User.ToList();

                        State.CurrentUserID = newUser.UserID;
                        MessageBox.Show($"Вы успешно зарегистрировались!\nДобро пожаловать :)");
                        NavigationService.Navigate(new MainPage());
                    }
                    else
                    {
                        MessageBox.Show("Почта введена в неправильном формате!");
                    }
                }
            }
        }

        private void ChangeToRegBtn_Click(object sender, RoutedEventArgs e)
        {
            LoginSP.Visibility = Visibility.Collapsed;
            AuthSP.Visibility = Visibility.Visible;
        }

        private void ChangeToLogBtn_Click(object sender, RoutedEventArgs e)
        {
            AuthSP.Visibility = Visibility.Collapsed;
            LoginSP.Visibility = Visibility.Visible;
        }
    }
}
