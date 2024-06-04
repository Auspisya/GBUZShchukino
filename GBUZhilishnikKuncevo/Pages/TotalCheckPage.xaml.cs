using GBUZhilishnikKuncevo.Classes;
using GBUZhilishnikKuncevo.Models;
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
using QRCoder;
using System.IO;
using System.Globalization;

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

        private class PrintableCheck
        {
            public string Num { get; set; }
            public string ServiceName { get; set; }
            public string Value { get; set; }
            public string TotalPayable { get; set; }
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            var totalCheckToPrint = (sender as Button).DataContext as TotalCheck;
            var services = DBConnection.DBConnect.ServiceCheck
                .Where(item => item.Accounting.bankBookId == totalCheckToPrint.bankBookId)
                .Where(item => item.Accounting.accountingEnd == totalCheckToPrint.requiredPaymentDate);
            var totalValue = (services.Sum(item => item.totalPayble));
            if (totalCheckToPrint.fine != null)
                totalValue = (services.Sum(item => item.totalPayble) + totalCheckToPrint.fine);
            DataGrid printable = new DataGrid();
            string[] labels = new string[] { "Номер Счётчика", "Услуга", "Показания", "Сумма" };
            string[] bindings = new string[] { "Num", "ServiceName", "Value", "TotalPayable" };
            for(int i = 0; i < labels.Length; i++)
            {
                DataGridTextColumn column = new DataGridTextColumn();
                column.Header = labels[i];
                column.Binding = new Binding(bindings[i]);
                printable.Columns.Add(column);
            }
            foreach (ServiceCheck serviceCheck in services)
            {
                var counterNum = "";
                if (serviceCheck.Accounting.counterId != null)
                    counterNum = serviceCheck.Accounting.Counter.counterNumber;
                var item = new PrintableCheck() { 
                    Num = counterNum,
                    ServiceName = serviceCheck.Accounting.Service.TypeOfService.serviceName,
                    Value = serviceCheck.Accounting.counterReading.ToString(),
                    TotalPayable = serviceCheck.totalPayble.ToString() 
                };
                printable.Items.Add(item);
            }
            if (totalCheckToPrint.fine != null && totalCheckToPrint.fine > 0) {
                var fineItem = new PrintableCheck() { 
                    Num = "",
                    ServiceName = "Пеня",
                    Value = "",
                    TotalPayable = totalCheckToPrint.fine.ToString()
                };
                printable.Items.Add(fineItem);
            }
            var totalItem = new PrintableCheck() { 
                Num = "",
                ServiceName = "Итого",
                Value = "",
                TotalPayable = totalValue.ToString() 
            };
            printable.Items.Add(totalItem);
            printDG(printable, totalCheckToPrint, services.First().Accounting.accountingStart, totalValue ?? 0);
        }

        private Paragraph CreateTextBlock(string title, FontStyle style, FontFamily family, double FontSize = 14, TextAlignment allign = TextAlignment.Left) 
        {
            Paragraph p = new Paragraph(new Run(title));

            p.FontStyle = style;
            p.FontFamily = family;
            p.FontSize = FontSize;
            p.TextAlignment = allign;
            return p;
        }

        public void printDG(DataGrid dataGrid, TotalCheck totalCheck, DateTime startDate, decimal totalValue)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                FlowDocument fd = new FlowDocument();
                CultureInfo culture = new CultureInfo("ru-RU");
                /*
                culture.DateTimeFormat.MonthNames = culture.DateTimeFormat.MonthGenitiveNames;
                culture.DateTimeFormat.AbbreviatedMonthNames = culture.DateTimeFormat.AbbreviatedMonthGenitiveNames;
                var startDateStr = startDate.ToString("d MMMM yyyy", culture);
                var endDateStr = totalCheck.requiredPaymentDate.ToString("d MMMM yyyy", culture);
                var title = CreateTextBlock(
                    $"Единый платёжный документ за коммунальные услуги за период: {startDateStr} - {endDateStr}",
                    dataGrid.FontStyle,
                    dataGrid.FontFamily,
                    18,
                    TextAlignment.Center
                    );
                */
                culture.DateTimeFormat.MonthNames = culture.DateTimeFormat.MonthNames;
                culture.DateTimeFormat.AbbreviatedMonthNames = culture.DateTimeFormat.AbbreviatedMonthGenitiveNames;
                var endDateStr = totalCheck.requiredPaymentDate.ToString("MMMM yyyy", culture);
                var title = CreateTextBlock(
                    $"Единый платёжный документ за коммунальные услуги за период: {endDateStr}",
                    dataGrid.FontStyle,
                    dataGrid.FontFamily,
                    18,
                    TextAlignment.Center
                    );
                var FIOTextBlock = CreateTextBlock($"ФИО: {totalCheck.BankBook.Client.PersonalInfo1.fullName}", dataGrid.FontStyle, dataGrid.FontFamily);
                var BankbookTextBlock = CreateTextBlock($"Лицевой счёт: {totalCheck.BankBook.bankBookNumber}", dataGrid.FontStyle, dataGrid.FontFamily);
                var AddressTextBlock = CreateTextBlock($"Адрес: {totalCheck.BankBook.Apartment.Address.fullAddress}", dataGrid.FontStyle, dataGrid.FontFamily);
                var AuthorTextBlock = CreateTextBlock($"Документ создан: {DBConnection.DBConnect.User.Where(item => item.id == AuthPages.AuthorizationPage.userId).First().PersonalInfo.fullName}", dataGrid.FontStyle, dataGrid.FontFamily);
                fd.Blocks.Add(title);
                fd.Blocks.Add(FIOTextBlock);
                fd.Blocks.Add(BankbookTextBlock);
                fd.Blocks.Add(AddressTextBlock);
                fd.Blocks.Add(AuthorTextBlock);
                if (totalCheck.benefitId != null)
                {
                    var BenefitTextBlock = CreateTextBlock($"Льгота: {totalCheck.Benefit.benefitName}", dataGrid.FontStyle, dataGrid.FontFamily);
                    fd.Blocks.Add(BenefitTextBlock);
                }

                Table table = new Table();
                var temp = new TableColumn() { Width = new GridLength(75)};
                TableRowGroup tableRowGroup = new TableRowGroup();
                TableRow r = new TableRow();
                fd.PageWidth = printDialog.PrintableAreaWidth;
                fd.PageHeight = printDialog.PrintableAreaHeight;
                fd.BringIntoView();

                fd.TextAlignment = TextAlignment.Center;
                fd.ColumnWidth = 500;
                table.CellSpacing = 0;
                var headers = dataGrid.Columns.Where(e => e.Header != null && e.Header.ToString() != "ID").ToList();
                var headerList = headers.Select(e => e.Header.ToString()).ToList();

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
                for (int i = 0; i < dataGrid.Items.Count; i++)
                {

                    var check = (PrintableCheck)dataGrid.Items.GetItemAt(i);

                    table.BorderBrush = Brushes.Gray;
                    table.BorderThickness = new Thickness(1, 1, 0, 0);
                    table.FontStyle = dataGrid.FontStyle;
                    table.FontFamily = dataGrid.FontFamily;
                    table.FontSize = 13;
                    tableRowGroup = new TableRowGroup();
                    r = new TableRow();
                    var buffer = new List<TableCell>();
                    buffer.Add(new TableCell(new Paragraph(new Run(check.Num))));
                    buffer.Add(new TableCell(new Paragraph(new Run(check.ServiceName))));
                    buffer.Add(new TableCell(new Paragraph(new Run(check.Value))));
                    buffer.Add(new TableCell(new Paragraph(new Run(check.TotalPayable))));
                    for (int j = 0; j < headers.Count(); j++)
                    {
                        r.Cells.Add(buffer[j]);
                        r.Cells[j].ColumnSpan = 4;
                        r.Cells[j].Padding = new Thickness(4);
                        r.Cells[j].BorderBrush = Brushes.DarkGray;
                        if (i == dataGrid.Items.Count - 1 && j == headers.Count - 1)
                            r.Cells[j].BorderThickness = new Thickness(1, 0, 1, 1);
                        else if (i == dataGrid.Items.Count - 1 && j < headers.Count - 1)
                            r.Cells[j].BorderThickness = new Thickness(0, 0, 0, 1);
                        else
                            r.Cells[j].BorderThickness = new Thickness(0, 0, 1, 1);
                    }
                    tableRowGroup.Rows.Add(r);
                    table.RowGroups.Add(tableRowGroup);

                }
                fd.Blocks.Add(table);

                var account = "40702810138250123017";
                var bic = "044525225";
                var bankName = "ОАО \"БАНК\"";
                var name = "ООО «Три кита»";
                var correspAcc = "30101810965770000413";
                var fields = new PayloadGenerator.RussiaPaymentOrder.OptionalFields();
                fields.TechCode = PayloadGenerator.RussiaPaymentOrder.TechCode.Коммунальные_услуги_ЖКХAFN;
                fields.Sum = Decimal.Round(totalValue).ToString();
                var generator = new PayloadGenerator.RussiaPaymentOrder(name, account, bankName, bic, correspAcc, fields);
                string payload = generator.ToString();

                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
                var qrCode = new PngByteQRCode(qrCodeData);
                var qrCodeBitmap = qrCode.GetGraphic(20);
                var ms = new MemoryStream(qrCodeBitmap);
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = ms;
                bitmap.EndInit();
                Image elImage = new Image
                {
                    Source = bitmap,
                    Width = 290,
                    Height = 120
                };
                Paragraph par = new Paragraph();
                par.FontSize = 12;
                par.TextAlignment = TextAlignment.Right;
                par.Inlines.Add(elImage);
                fd.Blocks.Add(par);
                printDialog.PrintDocument(((IDocumentPaginatorSource)fd).DocumentPaginator, "");

            }
        }

    }
}
