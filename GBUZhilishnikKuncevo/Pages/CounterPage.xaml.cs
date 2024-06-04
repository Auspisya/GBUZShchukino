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
    /// Логика взаимодействия для CounterPage.xaml
    /// </summary>
    public partial class CounterPage : Page
    {
        public CounterPage()
        {
            InitializeComponent();
            //Обнуляем таблицу, затем добавляем в нее данные
            DataCounter.ItemsSource = null;
            DataCounter.ItemsSource = DBConnection.DBConnect.Counter.ToList();
        }

        /// <summary>
        /// Переход на страницу с доп. информацией
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnShowInfo_Click(object sender, RoutedEventArgs e)
        {
            //В зависимости от выбранной строки, передаём её данные на следующую страницу и используем там
            Navigation.frameNav.Navigate(new CounterInfoPage((sender as Button).DataContext as Counter));
        }

        /// <summary>
        /// Поиск записи по номеру счётчика
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

                    var itemsList = DBConnection.DBConnect.Counter.ToList();

                    //Ищем совпадения в таблице по номеру счётчика
                    var searchResults = itemsList.Where(item => item.counterNumber.ToLower().Contains(searchString)).ToList();

                    //Заполняем таблицу записями, где есть совпадения
                    DataCounter.ItemsSource = searchResults.ToList();
                }
                else
                {
                    DataCounter.ItemsSource = DBConnection.DBConnect.Client.ToList();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Непредвиденная ошибка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Заполнение таблицы актуальными данными из БД
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            DataCounter.ItemsSource = null;
            DataCounter.ItemsSource = DBConnection.DBConnect.Counter.ToList();
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
        /// Переадресация на страницу с формой добавления счётчика в БД
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Navigation.frameNav.Navigate(new CounterAddPage());
        }
        /// <summary>
        /// Удаление интересующего счётчика
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
                    for (int i = 0; i < DataCounter.SelectedItems.Count; i++)
                    {
                        Counter counter = DataCounter.SelectedItems[i] as Counter;
                        DBConnection.DBConnect.Counter.Remove(counter);
                    }

                    DBConnection.DBConnect.SaveChanges();
                    MessageBox.Show("Данные успешно удалены!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information
                        );
                    DataCounter.ItemsSource = null;
                    DataCounter.ItemsSource = DBConnection.DBConnect.Counter.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Критическая обработка");
                }
            }
        }
        /// <summary>
        /// Переадресация на страницу редактирования конкретного счётчика
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEditInfo_Click(object sender, RoutedEventArgs e)
        {
            Navigation.frameNav.Navigate(new CounterEditPage((sender as Button).DataContext as Counter));
        }
        private void BtnHistory_Click(object sender, RoutedEventArgs e)
        {
            Navigation.frameNav.Navigate(new CounterHistoryPage((sender as Button).DataContext as Counter));
        }

        private void DataCounter_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            var counter = e.Row.DataContext as Counter;
            if(counter.endOfOperation <= DateTime.Now)
                e.Row.Background = new SolidColorBrush(Colors.Orange);
            else
                e.Row.Background = new SolidColorBrush(Colors.White);
        }
    }
}
