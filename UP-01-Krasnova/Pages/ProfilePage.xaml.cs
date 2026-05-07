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

            User cur = Core.Context.User.First(x => x.UserID == State.CurrentUserID);
            LoginTB.Text += cur.Login;
            UsernameTB.Text += cur.Username;
            EmailTB.Text += cur.Email;
            RoleTB.Text += cur.Role.Name;

            List<Review> userReviews = Core.Context.Review.ToList().FindAll(x => x.UserID == State.CurrentUserID);
            ReviewsLB.ItemsSource = userReviews;

            if (cur.Role.Name != "Author" && cur.IsFrozen == false)
            {
                BecomeAuthorSP.Visibility = Visibility.Visible;
            }

            if (cur.IsFrozen == true)
            {
                UnfreezeAccSP.Visibility = Visibility.Visible;
            }

            //UnfreezeAccTB.Text +=
        }
    }
}
