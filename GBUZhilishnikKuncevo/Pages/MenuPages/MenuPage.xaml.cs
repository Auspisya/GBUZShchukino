using GBUZhilishnikKuncevo.Classes;
using GBUZhilishnikKuncevo.Models;
using GBUZhilishnikKuncevo.Pages.AdminPages;
using GBUZhilishnikKuncevo.Pages.SuperAdminPages;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace GBUZhilishnikKuncevo.Pages.MenuPages
{
    /// <summary>
    /// Логика взаимодействия для MenuPage.xaml
    /// </summary>
    public partial class MenuPage : Page
    {
        public static string Role;
        public MenuPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Переход на страницу с квартиросъемщиками
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClientPage_Click(object sender, RoutedEventArgs e)
        {
            Navigation.frameNav.Navigate(new ClientPage());
        }

        /// <summary>
        /// Переход на страницу с счётчиками
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCounterPage_Click(object sender, RoutedEventArgs e)
        {
            Navigation.frameNav.Navigate(new CounterPage());
        }
        /// <summary>
        /// Переход на страницу с лицевыми счетами
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBankBookPage_Click(object sender, RoutedEventArgs e)
        {
            Navigation.frameNav.Navigate(new BankBookPage());
        }
        /// <summary>
        /// Переход на страницу с показаниями
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAccountingPage_Click(object sender, RoutedEventArgs e)
        {
            Navigation.frameNav.Navigate(new AccountingPage());
        }
        /// <summary>
        /// Переход на страницу к оплате за показания
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnServiceCheckPage_Click(object sender, RoutedEventArgs e)
        {
            Navigation.frameNav.Navigate(new ServiceCheckPage());
        }
        /// <summary>
        /// Переход на страницу с должниками
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDebtPage_Click(object sender, RoutedEventArgs e)
        {
            Navigation.frameNav.Navigate(new DebtorsPage());
        }
        /// <summary>
        /// Переход на страницу профиля авторизовавшегося пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnUserProfilePage_Click(object sender, RoutedEventArgs e)
        {
            //Role = _signIn.RoleUser;
            switch (Role)
            {
                case "Admin":
                    Navigation.frameNav.Navigate(new AdminProfilePage());
                    break;
                case "SuperAdmin":
                    Navigation.frameNav.Navigate(new SuperAdminProfilePage());
                    break;
                default:
                    MessageBox.Show("Неверная обработка данных");
                    break;
            }
        }
    }
}
