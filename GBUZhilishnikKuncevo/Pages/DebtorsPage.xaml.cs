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
    /// Логика взаимодействия для DebtorsPage.xaml
    /// </summary>
    public partial class DebtorsPage : Page
    {
        public DebtorsPage()
        {
            InitializeComponent();

            #region Правильное решение
            //List<Client> client = DBConnection.DBConnect.Client.ToList();
            //List<BankBook> bankBookClient = DBConnection.DBConnect.BankBook.ToList();
            //List<TotalCheck> tCheck = DBConnection.DBConnect.TotalCheck.ToList();

            //var result = client
            //    .Join(bankBookClient, c => c.id, b => b.clientId, (c, b) => new { Client = c, BankBook = b })
            //    .Join(tCheck, b => b.BankBook.id, t => t.bankBookId, (b, t) => new { b.Client, b.BankBook, TotalCheck = t })
            //    .Where(x => x.TotalCheck.paymentStateId == 2)
            //    .Select(x => new
            //    {
            //        ClientId = x.Client.id,
            //        Name = x.Client.name,
            //        Surname = x.Client.surname,
            //        BankBookNumber = x.BankBook.bankBookNumber
            //    }).ToList();

            //DataDebtors.ItemsSource = result;
            #endregion


            #region Костыль, выводящий должников

            DataDebtors.ItemsSource = null;
            var debtorsList = DBConnection.DBConnect.TotalCheck.ToList();
            var clientsList = DBConnection.DBConnect.Client.ToList();
            //Смотрим квитанции, которые оплачены несвоевременно
            var debtors = debtorsList.Where(item => item.PaymentState.paymentStateName.Contains("Оплачено несвоевременно")).ToList();
            //Сохраняем идентификаторы клиентов, которые оплатили несвоевременно
            var clientsId = debtors.Select(item => item.BankBook.clientId).ToList();

            List<Client> clientData = new List<Client>();

            for (int i = 0; i < clientsList.Count; i++)
            {
                for (int j = 0; j < clientsId.Count; j++)
                {
                    if (clientsList[i].id == clientsId[j])
                    {
                        clientData.Add(clientsList[i]);
                    }
                }
            }

            DataDebtors.ItemsSource = clientData.ToList();
            #endregion

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
        /// Поиск совпадений в базе данных, и вывод по ним записей в таблицу
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
                    #region Костыль, выводящий должников
                    var debtorsList = DBConnection.DBConnect.TotalCheck.ToList();
                    var clientsList = DBConnection.DBConnect.Client.ToList();
                    //Смотрим квитанции, которые оплачены несвоевременно
                    var debtors = debtorsList.Where(item => item.PaymentState.paymentStateName.Contains("Оплачено несвоевременно")).ToList();
                    //Сохраняем идентификаторы клиентов, которые оплатили несвоевременно
                    var clientsId = debtors.Select(item => item.BankBook.clientId).ToList();

                    List<Client> clientData = new List<Client>();

                    for (int i = 0; i < clientsList.Count; i++)
                    {
                        for (int j = 0; j < clientsId.Count; j++)
                        {
                            if (clientsList[i].id == clientsId[j])
                            {
                                clientData.Add(clientsList[i]);
                            }
                        }
                    }
                    #endregion
                    //Ищем совпадения в таблице по фамилии
                    var searchResults = clientData.Where(item => item.PersonalInfo1.surname.ToLower().Contains(searchString)).ToList();

                    //Заполняем таблицу записями, где есть совпадения
                    DataDebtors.ItemsSource = searchResults.ToList();
                }
                else
                {
                    #region Костыль, выводящий должников

                    DataDebtors.ItemsSource = null;
                    var debtorsList = DBConnection.DBConnect.TotalCheck.ToList();
                    var clientsList = DBConnection.DBConnect.Client.ToList();
                    //Смотрим квитанции, которые оплачены несвоевременно
                    var debtors = debtorsList.Where(item => item.PaymentState.paymentStateName.Contains("Оплачено несвоевременно")).ToList();
                    //Сохраняем идентификаторы клиентов, которые оплатили несвоевременно
                    var clientsId = debtors.Select(item => item.BankBook.clientId).ToList();

                    List<Client> clientData = new List<Client>();

                    for (int i = 0; i < clientsList.Count; i++)
                    {
                        for (int j = 0; j < clientsId.Count; j++)
                        {
                            if (clientsList[i].id == clientsId[j])
                            {
                                clientData.Add(clientsList[i]);
                            }
                        }
                    }

                    DataDebtors.ItemsSource = clientData.ToList();
                    #endregion
                }


            }
            catch (Exception)
            {
                MessageBox.Show("Непредвиденная ошибка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Поиск должников
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            #region Костыль, выводящий должников
            DataDebtors.ItemsSource = null;
            var debtorsList = DBConnection.DBConnect.TotalCheck.ToList();
            var clientsList = DBConnection.DBConnect.Client.ToList();
            //Смотрим квитанции, которые оплачены несвоевременно
            var debtors = debtorsList.Where(item => item.PaymentState.paymentStateName.Contains("Оплачено несвоевременно")).ToList();
            //Сохраняем идентификаторы клиентов, которые оплатили несвоевременно
            var clientsId = debtors.Select(item => item.BankBook.clientId).ToList();

            List<Client> clientData = new List<Client>();

            for (int i = 0; i < clientsList.Count; i++)
            {
                for (int j = 0; j < clientsId.Count; j++)
                {
                    if (clientsList[i].id == clientsId[j])
                    {
                        clientData.Add(clientsList[i]);
                    }
                }
            }

            DataDebtors.ItemsSource = clientData.ToList();
            #endregion
        }
        /// <summary>
        /// Перенаправляет на страницу с дополнительной информацией по выбранному объекту
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnShowInfo_Click(object sender, RoutedEventArgs e)
        {
            Navigation.frameNav.Navigate(new ClientInfoPage((sender as Button).DataContext as Client));
        }
    }
}
