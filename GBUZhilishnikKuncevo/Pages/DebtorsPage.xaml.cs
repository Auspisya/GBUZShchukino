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
                for (int j = 0; j < debtors.Count; j++)
                {
                    if (clientsList[i].id == debtors[j].BankBook.clientId)
                    {
                        clientData.Add(clientsList[i]);
                    }
                }
            }

            DataDebtors.ItemsSource = debtors;
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
                    var searchResults = debtors.Where(item => item.BankBook.Client.PersonalInfo1.surname.ToLower().Contains(searchString)).ToList();

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

                    DataDebtors.ItemsSource = debtors.ToList();
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

            DataDebtors.ItemsSource = debtors.ToList();
            #endregion
        }
        /// <summary>
        /// Перенаправляет на страницу с дополнительной информацией по выбранному объекту
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnShowInfo_Click(object sender, RoutedEventArgs e)
        {
            var debtorCheck = (sender as Button).DataContext as TotalCheck;
            Navigation.frameNav.Navigate(new ClientInfoPage(debtorCheck.BankBook.Client));
        }
        /// <summary>
        /// Печать таблицы с должниками
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            printDG(DataDebtors, "Должники");
        }

        /// <summary>
        /// Метод для печати содержимого таблицы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void printDG(DataGrid dataGrid, string title)
        {
            // Создание диалогового окна для печати.
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                // Создание потокового документа для содержимого печати.
                FlowDocument fd = new FlowDocument();

                // Добавление заголовка в потоковый документ.
                Paragraph p = new Paragraph(new Run(title));
                p.FontStyle = dataGrid.FontStyle;
                p.FontFamily = dataGrid.FontFamily;
                p.FontSize = 18;
                fd.Blocks.Add(p);

                // Создание таблицы для данных из DataGrid.
                Table table = new Table();
                TableRowGroup tableRowGroup = new TableRowGroup();
                TableRow r = new TableRow();
                fd.PageWidth = printDialog.PrintableAreaWidth;
                fd.PageHeight = printDialog.PrintableAreaHeight;
                fd.BringIntoView();

                // Установка размеров страницы и выравнивания текста.
                fd.TextAlignment = TextAlignment.Center;
                fd.ColumnWidth = 500;
                table.CellSpacing = 0;

                // Получение заголовков столбцов DataGrid.
                var headers = dataGrid.Columns.Where(e => e.Header != null && e.Header.ToString() != "ID").ToList();
                var headerList = headers.Select(e => e.Header.ToString()).ToList();

                // Добавление заголовков в таблицу.
                for (int j = 0; j < headerList.Count; j++)
                {

                    r.Cells.Add(new TableCell(new Paragraph(new Run(headerList[j]))));
                    r.Cells[j].ColumnSpan = 4;
                    r.Cells[j].Padding = new Thickness(4);

                    r.Cells[j].BorderBrush = Brushes.Black;
                    r.Cells[j].FontWeight = FontWeights.Bold;
                    r.Cells[j].Background = (Brush)new BrushConverter().ConvertFrom("#FF005197");
                    r.Cells[j].Foreground = Brushes.White;
                    r.Cells[j].BorderThickness = new Thickness(1, 1, 1, 1);
                }
                tableRowGroup.Rows.Add(r);
                table.RowGroups.Add(tableRowGroup);
                // Добавление данных из DataGrid в таблицу.
                for (int i = 0; i < dataGrid.Items.Count; i++)
                {
                    // Получение объекта TotalCheck из элемента DataGrid.
                    var check = (TotalCheck)dataGrid.Items.GetItemAt(i);
                    // Создание строки для каждого элемента DataGrid.
                    table.BorderBrush = Brushes.Gray;
                    table.BorderThickness = new Thickness(1, 1, 0, 0);
                    table.FontStyle = dataGrid.FontStyle;
                    table.FontFamily = dataGrid.FontFamily;
                    table.FontSize = 13;
                    tableRowGroup = new TableRowGroup();
                    r = new TableRow();
                    var buffer = new List<TableCell>();
                    buffer.Add(new TableCell(new Paragraph(new Run(check.BankBook.Client.PersonalInfo1.surname))));
                    buffer.Add(new TableCell(new Paragraph(new Run(check.BankBook.Client.PersonalInfo1.name))));
                    buffer.Add(new TableCell(new Paragraph(new Run(check.BankBook.Client.PersonalInfo1.patronymic))));
                    buffer.Add(new TableCell(new Paragraph(new Run(check.BankBook.Client.PersonalInfo1.phoneNumber))));
                    buffer.Add(new TableCell(new Paragraph(new Run(check.fine.ToString()))));
                    buffer.Add(new TableCell(new Paragraph(new Run(check.totalPayble.ToString()))));
                    // Добавление данных в строки таблицы.
                    for (int j = 0; j < headers.Count(); j++)
                    {
                        r.Cells.Add(buffer[j]);
                        r.Cells[j].ColumnSpan = 4;
                        r.Cells[j].Padding = new Thickness(4);
                        r.Cells[j].BorderBrush = Brushes.DarkGray;
                        r.Cells[j].BorderThickness = new Thickness(0, 0, 1, 1);
                    }
                    tableRowGroup.Rows.Add(r);
                    table.RowGroups.Add(tableRowGroup);

                }
                // Добавление таблицы в потоковый документ.
                fd.Blocks.Add(table);
                // Печать документа.
                printDialog.PrintDocument(((IDocumentPaginatorSource)fd).DocumentPaginator, "");

            }
        }
    }
}
