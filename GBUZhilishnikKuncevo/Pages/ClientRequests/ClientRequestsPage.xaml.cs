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
    /// Логика взаимодействия для ClientRequests.xaml
    /// </summary>
    public partial class ClientRequestsPage : Page
    {
        private bool is_issued = false;
        public ClientRequestsPage()
        {
            InitializeComponent();
            is_issued = false;
            DataRequests.ItemsSource = null;
            DataRequests.ItemsSource = DBConnection.DBConnect.ClientRequest.Where(item => is_issued ? item.issuedById != null : item.issuedById == null).ToList();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Navigation.frameNav.GoBack();
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            DataRequests.ItemsSource = null;
            DataRequests.ItemsSource = DBConnection.DBConnect.ClientRequest.Where(item => is_issued ? item.issuedById != null : item.issuedById == null).ToList();
        }

        private void BtnIssued_Click(object sender, RoutedEventArgs e)
        {
            is_issued = !is_issued;
            BtnIssued.Content = is_issued ? "Не рассмотренные" : "Расмотренные";
            DataRequests.ItemsSource = null;
            DataRequests.ItemsSource = DBConnection.DBConnect.ClientRequest.Where(item => is_issued ? item.issuedById != null : item.issuedById == null).ToList();
        }

        private void BtnIssueInfo_Click(object sender, RoutedEventArgs e)
        {
            var request = (sender as Button).DataContext as ClientRequest;
            Navigation.frameNav.Navigate(new ClientRequestIssuePage(request));
        }
    }
}
