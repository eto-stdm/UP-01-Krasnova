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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UP_01_Krasnova.Classes;

namespace UP_01_Krasnova.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        public ProfilePage()
        {
            InitializeComponent();
            Loaded += ProfilePage_Loaded;
        }

        private void ProfilePage_Loaded(object sender, RoutedEventArgs e)
        {
            User cur = Core.Context.User.First(x => x.UserID == State.CurrentUserID);
            LoginTB.Text += cur.Login;
            UsernameTB.Text += cur.Username;
            EmailTB.Text += cur.Email;
            RoleTB.Text += cur.Role.Name;

            List<Review> userReviews = Core.Context.Review.ToList().FindAll(x => x.UserID == State.CurrentUserID);
            ReviewsLB.ItemsSource = userReviews;

            if (cur.Role.Name == "Reader" && cur.IsFrozen == false)
            {
                BecomeAuthorSP.Visibility = Visibility.Visible;
                if (Core.Context.RoleApplication.FirstOrDefault(x => x.UserID == State.CurrentUserID && x.Status.StatusID == 1) != null)
                {
                    BecomeAuthorTB.Text = "Вы уже подали заявку на роль автора,\nожидайте ответа!";
                    BecomeAuthorBtn.Visibility = Visibility.Collapsed;
                }
                else if (Core.Context.RoleApplication.FirstOrDefault(x => x.UserID == State.CurrentUserID && x.Status.StatusID == 3) != null)
                {
                    BecomeAuthorTB.Text = "Вашу заявку на роль автора отклонили :(";
                    BecomeAuthorBtn.Visibility = Visibility.Collapsed;
                }
            }

            if (cur.IsFrozen == true)
            {
                UnfreezeAccSP.Visibility = Visibility.Visible;
                if (Core.Context.UnFreezeApplication.FirstOrDefault(x => x.UserID == State.CurrentUserID && x.Status.StatusID == 1) != null)
                {
                    UnfreezeAccTB.Text = "Вы уже подали заявку на разморозку,\nожидайте ответа!";
                    UnfreezeAccBtn.Visibility = Visibility.Collapsed;
                }
                else if (Core.Context.UnFreezeApplication.FirstOrDefault(x => x.UserID == State.CurrentUserID && x.Status.StatusID == 3) != null)
                {
                    UnfreezeAccTB.Text = "Вашу заявку на разморозку отклонили :(";
                    UnfreezeAccBtn.Visibility = Visibility.Collapsed;
                }
                else
                {
                    List<Report> reportsDes = Core.Context.Report.ToList();
                    reportsDes.OrderByDescending(x => x.ReportID);
                    UnfreezeAccTB.Text += reportsDes.First(x => x.UserID == State.CurrentUserID && x.IsDone == true).Reason;
                }
            }
        }

        private void BecomeAuthorBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Core.Context.RoleApplication.FirstOrDefault(x => x.UserID == State.CurrentUserID) == null)
            {
                RoleApplication newApplication = new RoleApplication
                {
                    ApplicationDate = DateTime.Now,
                    UserID = State.CurrentUserID,
                    StatusID = 1,
                };
                Core.Context.RoleApplication.Add(newApplication);
                Core.Context.SaveChanges();
                BecomeAuthorTB.Text = "Вы уже подали заявку на роль автора,\nожидайте ответа!";
                BecomeAuthorBtn.Visibility = Visibility.Collapsed;
            }
            else { MessageBox.Show("Вы уже подали заявку!"); }
        }

        private void UnfreezeAccBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Core.Context.UnFreezeApplication.FirstOrDefault(x => x.UserID == State.CurrentUserID) == null)
            {
                UnFreezeApplication newApplication = new UnFreezeApplication
                {
                    ApplicationDate = DateTime.Now,
                    UserID = State.CurrentUserID,
                    StatusID = 1,
                };
                Core.Context.UnFreezeApplication.Add(newApplication);
                Core.Context.SaveChanges();
                UnfreezeAccTB.Text = "Вы уже подали заявку на разморозку,\nожидайте ответа!";
                UnfreezeAccBtn.Visibility = Visibility.Collapsed;
            }
            else { MessageBox.Show("Вы уже подали заявку!"); }
        }
    }
}
