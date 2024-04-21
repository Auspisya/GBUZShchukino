using GBUZhilishnikKuncevo.Classes;
using GBUZhilishnikKuncevo.Models;
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
using System.Xml.Linq;

namespace GBUZhilishnikKuncevo.Pages
{
    /// <summary>
    /// Логика взаимодействия для ClientPage.xaml
    /// </summary>
    public partial class ClientPage : Page
    {
        public ClientPage()
        {
            InitializeComponent();
            //Обнуляем таблицу, затем добавляем в нее данные
            DataClient.ItemsSource = null;
            DataClient.ItemsSource = DBConnection.DBConnect.Client.ToList();
        }

        /// <summary>
        /// Позволяет посмотреть полную информацию по квартиросъемщику
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnShowInfo_Click(object sender, RoutedEventArgs e)
        {
            //В зависимости от выбранной строки, передаём её данные на следующую страницу и используем там
            Navigation.frameNav.Navigate(new ClientInfoPage((sender as Button).DataContext as Client));
        }

        /// <summary>
        /// Поиск по введённому запросу
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

                    var itemsList = DBConnection.DBConnect.Client.ToList();

                    //Ищем совпадения в таблице по фамилии
                    var searchResults = itemsList.Where(item => item.PersonalInfo1.surname.ToLower().Contains(searchString)).ToList();

                    //Заполняем таблицу записями, где есть совпадения
                    DataClient.ItemsSource = searchResults.ToList();
                }
                else {
                    DataClient.ItemsSource = DBConnection.DBConnect.Client.ToList();
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
            //Убираем подпись
            TxbSearch.Text = "";
        }

        /// <summary>
        /// Переход на страницу редактирования квартиросъемщика
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEditInfo_Click(object sender, RoutedEventArgs e)
        {
            Navigation.frameNav.Navigate(new ClientEditPage((sender as Button).DataContext as Client));
        }

        /// <summary>
        /// Обновляем таблицу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            //Обновляем таблицу. При редактировании данных придется перезапустить приложение
            DataClient.ItemsSource = null;
            DataClient.ItemsSource = DBConnection.DBConnect.Client.ToList();
        }

        /// <summary>
        /// Переход на страницу добавления квартиросъемщика
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Navigation.frameNav.Navigate(new ClientAddPage());
        }
        /// <summary>
        /// Удаление квартиросъемщика из БД
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
                    for (int i = 0; i < DataClient.SelectedItems.Count; i++)
                    {
                        Client client = DataClient.SelectedItems[i] as Client;
                        DBConnection.DBConnect.Client.Remove(client);
                    }

                    DBConnection.DBConnect.SaveChanges();
                    MessageBox.Show("Данные успешно удалены!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information
                        );
                    DataClient.ItemsSource = null;
                    DataClient.ItemsSource = DBConnection.DBConnect.Client.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Критическая обработка");
                }
            }
        }
    }
}
