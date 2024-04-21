using GBUZhilishnikKuncevo.Classes;
using GBUZhilishnikKuncevo.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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

namespace GBUZhilishnikKuncevo.Pages.SuperAdminPages
{

    /// <summary>
    /// Логика взаимодействия для SessionPage.xaml
    /// </summary>
    public partial class SessionPage : Page
    {

        public SessionPage()
        {
            InitializeComponent();
            SessionData.ItemsSource = null;
            SessionData.ItemsSource = DBConnection.DBConnect.Session.ToList();
        }

        /// <summary>
        /// Перекрашивает в оранжевый цвет строки, в которых 3 или больше попыток
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SessionData_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            //menshakova_publicUtilitiesEntities context = new menshakova_publicUtilitiesEntities();
            var context = e.Row.DataContext as Session;
            if (context != null && context.authAttempts >= 3)
            {
                e.Row.Background = new SolidColorBrush(Color.FromRgb(255, 127, 80));
                e.Row.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
            else
            {
                e.Row.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                e.Row.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            }
        }

        /// <summary>
        /// Обновление таблицы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            SessionData.ItemsSource = null;
            SessionData.ItemsSource = DBConnection.DBConnect.Session.ToList();
        }
        /// <summary>
        /// Переход на предыдущую страницу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Navigation.frameNav.GoBack();
        }
        /// <summary>
        /// Поиск по логину и вывод совпадений в таблицу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TxbSearch.Text != "")
                {
                    string searchString = TxbSearch.Text.ToLower();
                    var itemsList = DBConnection.DBConnect.Session.ToList();
                    var searchResults = itemsList.Where(item => item.User.login.ToLower().Contains(searchString)).ToList();
                    SessionData.ItemsSource = searchResults.ToList();
                }
                else
                {
                    SessionData.ItemsSource = DBConnection.DBConnect.Session.ToList();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Непредвиденная ошибка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Убирает подсказку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            TxbSearch.Text = "";
        }
    }
}
