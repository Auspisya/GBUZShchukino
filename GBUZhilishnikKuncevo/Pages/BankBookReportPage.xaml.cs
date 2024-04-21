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
        private int bankBookId;
        public SeriesCollection SeriesCollection { get; set; }

        public BankBookReportPage(BankBook bankBook)
        {
            InitializeComponent();
            bankBookId = bankBook.id;
            SeriesCollection = new SeriesCollection();
            menshakova_publicUtilitiesEntities context = new menshakova_publicUtilitiesEntities();
            var results = context.Accounting.Where(item => item.bankBookId == bankBookId).GroupBy(item => item.accountingEnd).Select(item=>item.FirstOrDefault());
            CmbDate.DisplayMemberPath = "accountingEnd";
            CmbDate.SelectedValuePath = "id";
            CmbDate.ItemsSource = results.ToList();
            TxbBankBookId.Text = bankBook.bankBookNumber;
            DataContext = this;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Navigation.frameNav.GoBack();
        }
        private void SetChart(System.DateTime date)
        {
            menshakova_publicUtilitiesEntities context = new menshakova_publicUtilitiesEntities();
            SeriesCollection.Clear();
            var results = context.Accounting.Where(item => item.bankBookId == bankBookId && item.accountingEnd == date);
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

        private void Chart_OnDataClick(object sender, ChartPoint chartpoint)
        {
            var chart = (LiveCharts.Wpf.PieChart)chartpoint.ChartView;

            //clear selected slice.
            foreach (PieSeries series in chart.Series)
                series.PushOut = 0;

            var selectedSeries = (PieSeries)chartpoint.SeriesView;
            selectedSeries.PushOut = 8;
        }

        private void CmbDate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var date = (e.AddedItems[0] as Accounting).accountingEnd;
            SetChart(date);
        }
    }
}
