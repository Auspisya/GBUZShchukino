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

namespace GBUZhilishnikKuncevo.Pages
{
    /// <summary>
    /// Логика взаимодействия для ServiceCheckPage.xaml
    /// </summary>
    public partial class ServiceCheckPage : Page
    {
        public ServiceCheckPage()
        {
            InitializeComponent();

            DataServiceCheck.ItemsSource = null;
            DataServiceCheck.ItemsSource = DBConnection.DBConnect.ServiceCheck.ToList();
            
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
        /// <summary>
        /// Ищет по совпадениям записи из базы данных, и заполняет ими таблицу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TxbSearch.Text != "")
                {
                    string searchString = TxbSearch.Text;

                    var itemsList = DBConnection.DBConnect.ServiceCheck.ToList();

                    var searchResults = itemsList.Where(item => item.Accounting.BankBook.bankBookNumber.Contains(searchString)).ToList();
                    DataServiceCheck.ItemsSource = searchResults.ToList();
                }
                else
                {
                    DataServiceCheck.ItemsSource = DBConnection.DBConnect.ServiceCheck.ToList();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Непредвиденная ошибка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Обновляет страницу актуальными данными
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            DataServiceCheck.ItemsSource = null;
            DataServiceCheck.ItemsSource = DBConnection.DBConnect.ServiceCheck.ToList();
        }
        /// <summary>
        /// Переадресация на страницу с итоговыми чеками
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnTotalCheck_Click(object sender, RoutedEventArgs e)
        {
            Navigation.frameNav.Navigate(new TotalCheckPage());
        }
    }
}
