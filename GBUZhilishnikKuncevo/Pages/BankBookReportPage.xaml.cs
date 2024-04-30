using GBUZhilishnikKuncevo.Classes;
using GBUZhilishnikKuncevo.Models;
using LiveCharts;
using LiveCharts.Wpf;
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
    /// Логика взаимодействия для BankBookReportPage.xaml
    /// </summary>
    public partial class BankBookReportPage : Page
    {
        // Переменная для хранения идентификатора выбранного лицевого счета.
        private int bankBookId;
        // Коллекция для хранения данных, используемых для построения графика.
        public SeriesCollection SeriesCollection { get; set; } 

        public BankBookReportPage(BankBook bankBook)
        {
            InitializeComponent();
            bankBookId = bankBook.id;
            SeriesCollection = new SeriesCollection();
            menshakova_publicUtilitiesEntities context = new menshakova_publicUtilitiesEntities();
            // Заполнение элементов дизайна/управления из базы данных
            var results = context.Accounting.Where(item => item.bankBookId == bankBookId).GroupBy(item => item.accountingEnd).Select(item=>item.FirstOrDefault());
            CmbDate.DisplayMemberPath = "accountingEnd";
            CmbDate.SelectedValuePath = "id";
            CmbDate.ItemsSource = results.ToList();
            TxbBankBookId.Text = bankBook.bankBookNumber;
            DataContext = this;
        }

        // Переход на предыдущую страницую
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Navigation.frameNav.GoBack();
        }
        /// <summary>
        /// Метод для установки данных графика на основе выбранной даты.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetChart(System.DateTime date)
        {
            menshakova_publicUtilitiesEntities context = new menshakova_publicUtilitiesEntities();
            SeriesCollection.Clear();
            // Получаем данные из базы данных для заполнения графика.
            var results = context.Accounting.Where(item => item.bankBookId == bankBookId && item.accountingEnd == date);
            // Создаем графикю
            foreach(var acc in results)
            {
                var value = Decimal.ToDouble(context.ServiceCheck.Where(item => item.accountingId == acc.id).FirstOrDefault().totalPayble ?? default(decimal));
                var label = acc.Service.TypeOfService.serviceName;
                SeriesCollection.Add(new PieSeries
                {
                    Title = label,
                    DataLabels = true,
                    LabelPoint = chartPoint => string.Format("{0} ₽", (int)chartPoint.Y),
                    Values = new ChartValues<double> { value }
                });
            }
        }
        /// <summary>
        /// Обработчик события щелчка на графике.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Chart_OnDataClick(object sender, ChartPoint chartpoint)
        {
            var chart = (LiveCharts.Wpf.PieChart)chartpoint.ChartView;

            // Очищаем выбранную долю.
            // clear selected slice.
            foreach (PieSeries series in chart.Series)
                series.PushOut = 0;

            // Выделяем выбранную долю.
            var selectedSeries = (PieSeries)chartpoint.SeriesView;
            selectedSeries.PushOut = 8;
        }
        /// <summary>
        /// Обработчик события щелчка на графике.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbDate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Получаем выбранную дату.
            var date = (e.AddedItems[0] as Accounting).accountingEnd;
            // Обновляем данные графика.
            SetChart(date);
        }
    }
}
