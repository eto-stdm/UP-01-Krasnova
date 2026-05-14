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
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();

            UpdateAllLB();
            ReportsSP.Visibility = Visibility.Visible;
        }

        private void HideAllLB()
        {
            ReportsSP.Visibility = Visibility.Collapsed;
            UnfreezeAppSP.Visibility = Visibility.Collapsed;
            RoleAppSP.Visibility = Visibility.Collapsed;
            FrozenBooksSP.Visibility = Visibility.Collapsed;
            FrozenUsersSP.Visibility = Visibility.Collapsed;
            FrozenReviewsSP.Visibility = Visibility.Collapsed;
            UsersSP.Visibility = Visibility.Collapsed;
        }

        private void UpdateAllLB()
        {
            ReportsLB.ItemsSource = Core.Context.Report.Where(x => x.IsDone == false).ToList();
            UnfreezeAppLB.ItemsSource = Core.Context.UnFreezeApplication.Where(x => x.StatusID == 1).ToList();
            RoleAppLB.ItemsSource = Core.Context.RoleApplication.Where(x => x.StatusID == 1).ToList();
            FrozenBooksLB.ItemsSource = Core.Context.Book.Where(x => x.IsFrozen == true).ToList();
            FrozenUsersLB.ItemsSource = Core.Context.User.Where(x => x.IsFrozen == true).ToList();
            FrozenReviewsLB.ItemsSource = Core.Context.Review.Where(x => x.IsFrozen == true).ToList();
            UsersLB.ItemsSource = Core.Context.User.ToList();
        }

        private void ChangeList_Click(object sender, RoutedEventArgs e)
        {
            HideAllLB();
            var mi = (MenuItem)sender;
            switch (mi.Name)
            {
                case "ReportsMI":
                    ReportsSP.Visibility = Visibility.Visible;
                    break;
                case "UnfreezeAppMI":
                    UnfreezeAppSP.Visibility = Visibility.Visible;
                    break;
                case "RoleAppMI":
                    RoleAppSP.Visibility = Visibility.Visible;
                    break;
                case "FrozenBooksMI":
                    FrozenBooksSP.Visibility = Visibility.Visible;
                    break;
                case "FrozenUsersMI":
                    FrozenUsersSP.Visibility = Visibility.Visible;
                    break;
                case "FrozenReviewsMI":
                    FrozenReviewsSP.Visibility = Visibility.Visible;
                    break;
                case "UsersMI":
                    UsersSP.Visibility = Visibility.Visible;
                    break;
                default: break;
            }
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            switch (btn.Name)
            {
                case "AcceptReportBtn":
                    if (btn?.Tag is Report report)
                    {
                        if (report.UserID != null)
                        {
                            //User bannedUser = Core.Context.User.FirstOrDefault(x => x.UserID == report.UserID);
                            //bannedUser.IsFrozen = true;
                            Core.Context.User.FirstOrDefault(x => x.UserID == report.UserID).IsFrozen = true;
                        }
                        else if (report.ReviewID != null)
                        {
                            Core.Context.Review.FirstOrDefault(x => x.ReviewID == report.ReviewID).IsFrozen = true;
                        }
                        else if (report.BookID != null)
                        {
                            Core.Context.Book.FirstOrDefault(x => x.BookID == report.BookID).IsFrozen = true;
                        }
                        report.IsDone = true;
                        MessageBox.Show("Жалоба была принята!");
                    }
                    break;
                case "AcceptUnfreezeBtn":
                    if (btn?.Tag is UnFreezeApplication unFreeze)
                    {
                        if (unFreeze.UserID != null)
                        {
                            Core.Context.User.FirstOrDefault(x => x.UserID == unFreeze.UserID).IsFrozen = false;
                        }
                        else if (unFreeze.ReviewID != null)
                        {
                            Core.Context.Review.FirstOrDefault(x => x.ReviewID == unFreeze.ReviewID).IsFrozen = false;
                        }
                        else if (unFreeze.BookID != null)
                        {
                            Core.Context.Book.FirstOrDefault(x => x.BookID == unFreeze.BookID).IsFrozen = false;
                        }
                        unFreeze.StatusID = 2;
                        MessageBox.Show("Разморозка была принята!");
                    }
                    break;
                case "AcceptRoleBtn":
                    if (btn?.Tag is RoleApplication roleApp)
                    {
                        Core.Context.User.FirstOrDefault(x => x.UserID == roleApp.UserID).RoleID = 2;
                        roleApp.StatusID = 2;
                        MessageBox.Show("Изменение роли было принято!");
                    }
                    break;
                default: break;
            }
            Core.Context.SaveChanges();
            UpdateAllLB();
        }

        private void Decline_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            switch (btn.Name)
            {
                case "DeclineReportBtn":
                    if (btn?.Tag is Report report)
                    {
                        report.IsDone = true;
                        MessageBox.Show("Жалоба была отклонена!");
                    }
                    break;
                case "DeclineUnfreezeBtn":
                    if (btn?.Tag is UnFreezeApplication unFreeze)
                    {
                        unFreeze.StatusID = 3;
                        MessageBox.Show("Разморозка была отклонена!");
                    }
                    break;
                case "DeclineRoleBtn":
                    if (btn?.Tag is RoleApplication roleApp)
                    {
                        roleApp.StatusID = 3;
                        MessageBox.Show("Изменение роли было отклонено!");
                    }
                    break;
                default: break;
            }
            Core.Context.SaveChanges();
            UpdateAllLB();
        }

        private void Change(User user, int roleID)
        {
            if (user.RoleID != roleID)
            {
                user.RoleID = roleID;
                Core.Context.SaveChanges();
                MessageBox.Show($"Роль пользователя {user.Username} была изменена на {user.Role.Name}");
            }
            else { MessageBox.Show($"Роль {user.Role.Name} совпадает с текущей. Нет изменений"); }
        }

        private void ChangeRole_Click(object sender, RoutedEventArgs e)
        {
            var mi = (MenuItem)sender;
            User user = mi.DataContext as User;

            if (user.UserID != State.CurrentUserID)
            {
                switch (mi.Name)
                {
                    case "ChangeRoleReaderMI":
                        Change(user, 3);
                        break;
                    case "ChangeRoleAuthorMI":
                        Change(user, 2);
                        break;
                    case "ChangeRoleAdminMI":
                        Change(user, 1);
                        break;
                    default: break;
                }
                UpdateAllLB();
            }
            else { MessageBox.Show("Нельзя изменить роль текущего пользователя!"); }
        }

        private void ChangePasswordBtn_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button?.Tag is User user)
            {
                var window = new ChangePasswordWindow(user);
                if (window.ShowDialog() == true)
                {
                    MessageBox.Show($"Пароль пользователя '{user.Username}' успешно изменён!");
                }
            }
        }
    }
}
