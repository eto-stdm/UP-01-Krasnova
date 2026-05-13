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
            ReportsLB.ItemsSource = Core.Context.Report.ToList();
            UnfreezeAppLB.ItemsSource = Core.Context.Report.ToList();
            RoleAppLB.ItemsSource = Core.Context.RoleApplication.ToList();
            FrozenBooksLB.ItemsSource = Core.Context.Book.Where(x => x.IsFrozen == true).ToList();
            FrozenUsersLB.ItemsSource = Core.Context.User.Where(x => x.IsFrozen == true).ToList();
            FrozenReviewsLB.ItemsSource = Core.Context.Review.Where(x => x.IsFrozen == true).ToList();
            UsersLB.ItemsSource = Core.Context.User.ToList();
        }

        private void ChangeList_Click(object sender, RoutedEventArgs e)
        {
            HideAllLB();
            var mi = (MenuItem)sender;
            //string objType = null;
            //object newObj = null;
            //bool caughtDup = false;
            switch (mi.Name)
            {
                case "ReportsMI": break;
                case "UnfreezeAppMI": break;
                case "RoleAppMI": break;
                case "FrozenBooksMI": break;
                case "FrozenUsersMI": break;
                case "FrozenReviewsMI": break;
                case "UsersMI": break;
                default: break;
            }

        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            switch (btn.Name)
            {
                case "AcceptReportsBtn": break;
                case "AcceptUnfreezeBtn": break;
                case "AcceptRoleBtn": break;
                default: break;
            }
        }

        private void Decline_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            switch (btn.Name)
            {
                case "DeclineReportBtn": break;
                case "DeclineUnfreezeBtn": break;
                case "DeclineRoleBtn": break;
                default: break;
            }
        }
    }
}
