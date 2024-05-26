using GBUZhilishnikKuncevo.Classes;
using GBUZhilishnikKuncevo.Models;
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

namespace GBUZhilishnikKuncevo.Pages.ClientRequests
{
    /// <summary>
    /// Логика взаимодействия для ClientRequestIssuePage.xaml
    /// </summary>
    public partial class ClientRequestIssuePage : Page
    {
        private int clientRequestId;
        public ClientRequestIssuePage( ClientRequest request)
        {
            InitializeComponent();
            clientRequestId = request.id;
            TxbRequestCreated.Text = request.requestCreated.ToString("dd.MM.yyyy");
            TxbRequest.Text = request.request;
            if (request.issuedById == null)
            {
                GBIssuedBy.Visibility = Visibility.Hidden;
                BtnIssue.Visibility = Visibility.Visible;
            }
            else
            {
                GBIssuedBy.Visibility = Visibility.Visible;
                TxbIssuedBy.Text = DBConnection.DBConnect.User.Where(item => item.id == AuthPages.AuthorizationPage.userId).First().PersonalInfo.fullName;
                BtnIssue.Visibility = Visibility.Collapsed;
            }
        }

        private void BtnIssue_Click(object sender, RoutedEventArgs e)
        {
            ClientRequest request = DBConnection.DBConnect.ClientRequest.Where(item => item.id == clientRequestId).First();
            request.issuedById = AuthPages.AuthorizationPage.userId;
            DBConnection.DBConnect.SaveChanges();
            Navigation.frameNav.GoBack();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Navigation.frameNav.GoBack();
        }
    }
}
