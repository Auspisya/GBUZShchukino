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
    /// Логика взаимодействия для BankBookPage.xaml
    /// </summary>
    public partial class BankBookPage : Page
    {
        public BankBookPage()
        {
            InitializeComponent();

            DataBankBook.ItemsSource = null;
            DataBankBook.ItemsSource = DBConnection.DBConnect.BankBook.ToList();
        }
        /// <summary>
        /// Убирает подсказку из поисковика
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            TxbSearch.Text = "";
        }
        /// <summary>
        /// Поиск по лицевому счёту, наполняет таблицу результатами поиска
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

                    var itemsList = DBConnection.DBConnect.BankBook.ToList();

                    var searchResults = itemsList.Where(item => item.bankBookNumber.Contains(searchString)).ToList();
                    DataBankBook.ItemsSource = searchResults.ToList();
                }
                else
                {
                    DataBankBook.ItemsSource = DBConnection.DBConnect.BankBook.ToList();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Непредвиденная ошибка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Обновляет таблицу актуальными записями из БД
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            DataBankBook.ItemsSource = null;
            DataBankBook.ItemsSource = DBConnection.DBConnect.BankBook.ToList();
        }
        /// <summary>
        /// Переадресация на страницу добавления лицевого счёта и данных о нём
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Navigation.frameNav.Navigate(new BankBookAddPage());
        }
        /// <summary>
        /// Переадресация на страницу редактирования данных о лицевом счёте
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEditInfo_Click(object sender, RoutedEventArgs e)
        {
            Navigation.frameNav.Navigate(new BankBookEditPage((sender as Button).DataContext as BankBook));
        }
        /// <summary>
        /// Переадресация на страницу с конкретно выбранным лицевым счётом (его данными)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnShowInfo_Click(object sender, RoutedEventArgs e)
        {
            Navigation.frameNav.Navigate(new BankBookInfoPage((sender as Button).DataContext as BankBook));
        }
        /// <summary>
        /// Удаление лицевого счёта из БД
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите удалить данные?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {

            }
            else
            {
                try
                {
                    for (int i = 0; i < DataBankBook.SelectedItems.Count; i++)
                    {
                        BankBook bankBook = DataBankBook.SelectedItems[i] as BankBook;
                        DBConnection.DBConnect.BankBook.Remove(bankBook);
                    }

                    DBConnection.DBConnect.SaveChanges();
                    MessageBox.Show("Данные успешно удалены!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information
                        );
                    DataBankBook.ItemsSource = null;
                    DataBankBook.ItemsSource = DBConnection.DBConnect.BankBook.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Критическая обработка");
                }
            }
        }

        private void BtnReport_Click(object sender, RoutedEventArgs e)
        {
            Navigation.frameNav.Navigate(new BankBookReportPage((sender as Button).DataContext as BankBook));
        }
    }
}
