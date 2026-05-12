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
    /// Логика взаимодействия для MakeReportWindow.xaml
    /// </summary>
    public partial class MakeReportWindow : Window
    {
        public string typeName { get; set; }
        public object obj { get; set; }
        public MakeReportWindow(string typeName, object obj)
        {
            InitializeComponent();

            this.typeName = typeName;
            this.obj = obj;

            NameTB.Text += typeName;
        }

        private void MakeReportBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TextTB.Text))
            {
                Report newReport = new Report();
                switch (typeName)
                {
                    case "книгу":
                        Book book = obj as Book;
                        newReport = new Report
                        {
                            ComplainantID = State.CurrentUserID,
                            BookID = book.BookID,
                            Reason = TextTB.Text,
                        };
                        break;
                    case "автора":
                        User user = obj as User;
                        newReport = new Report
                        {
                            ComplainantID = State.CurrentUserID,
                            UserID = user.UserID,
                            Reason = TextTB.Text,
                        };
                        break;
                    case "отзыв":
                        Review review = obj as Review;
                        newReport = new Report
                        {
                            ComplainantID = State.CurrentUserID,
                            ReviewID = review.ReviewID,
                            Reason = TextTB.Text,
                        };
                        break;
                    default: break;
                }

                Core.Context.Report.Add(newReport);
                Core.Context.SaveChanges();

                this.DialogResult = true;
            }
            else { MessageBox.Show("Напишите причину жалобы!"); }
        }
    }
}
