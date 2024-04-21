using GBUZhilishnikKuncevo.Classes;
using GBUZhilishnikKuncevo.Pages.Resources;
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
    /// Логика взаимодействия для TotalCheckPage.xaml
    /// </summary>
    public partial class TotalCheckPage : Page
    {
        public TotalCheckPage()
        {
            InitializeComponent();

            DataTotalCheck.ItemsSource = null;
            DataTotalCheck.ItemsSource = DBConnection.DBConnect.TotalCheck.ToList();
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
        /// Поиск информации и вывод в таблицу результатов поиска
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

                    var itemsList = DBConnection.DBConnect.TotalCheck.ToList();

                    var searchResults = itemsList.Where(item => item.BankBook.bankBookNumber.Contains(searchString)).ToList();
                    DataTotalCheck.ItemsSource = searchResults.ToList();
                }
                else
                {
                    DataTotalCheck.ItemsSource = DBConnection.DBConnect.TotalCheck.ToList();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Непредвиденная ошибка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Обновление таблицы актуальными данными
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            DataTotalCheck.ItemsSource = null;
            DataTotalCheck.ItemsSource = DBConnection.DBConnect.TotalCheck.ToList();
        }

        /// <summary>
        /// Переадресация на форму добавления чека в базу данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddTotalCheck_Click(object sender, RoutedEventArgs e)
        {
            Navigation.frameNav.Navigate(new TotalCheckAddPage());
        }
        /// <summary>
        /// Переадресация на предыдущую страницу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Navigation.frameNav.GoBack();
        }
    }
}
