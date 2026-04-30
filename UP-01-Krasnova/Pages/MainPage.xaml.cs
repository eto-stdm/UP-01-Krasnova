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
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();

            User user = Core.Context.User.FirstOrDefault(x => x.UserID == State.CurrentUserID);

            switch (user.Role.Name)
            {
                case "Администратор":
                    AdminMI.Visibility = Visibility.Visible;
                    break;
                case "Автор":
                    AuthorMI.Visibility = Visibility.Visible;
                    break;
                default: break;
            }
            if (user.IsFrozen) { WarningMI.Visibility = Visibility.Visible; }
        }

        private void SideBarItem_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as MenuItem).Name;
            switch (item)
            {
                case "BookCatalogMI":
                    MainPageFrame.NavigationService.Navigate(new BookCatalogPage());
                    break;
                case "BookListMI":
                    MainPageFrame.NavigationService.Navigate(new BookListPage());
                    break;
                case "ProfileMI":
                    MainPageFrame.NavigationService.Navigate(new ProfilePage());
                    break;
                case "AdminMI":
                    MainPageFrame.NavigationService.Navigate(new AdminPage());
                    break;
                case "AuthorMI":
                    MainPageFrame.NavigationService.Navigate(new AuthorPage());
                    break;
                case "WarningMI":
                    MessageBox.Show("Ваш аккаунт заморожен!");
                    break;
                default: break;
            }
        }
    }
}
