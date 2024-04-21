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
    /// Логика взаимодействия для CounterHistoryPage.xaml
    /// </summary>
    public partial class CounterHistoryPage : Page
    {
        private int counterId;
        public SeriesCollection SeriesCollection { get; set; }
        public List<string> Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }


        public CounterHistoryPage(Counter counter)
        {
            InitializeComponent();
            TxbCounterNumber.Text = counter.counterNumber;
            TxbCounterType.Text = counter.TypeOfCounter.counterName;
            counterId = counter.id;
            menshakova_publicUtilitiesEntities context = new menshakova_publicUtilitiesEntities();
            var results = context.Accounting.Where(item => item.counterId == counterId).Select(item => new
            {
                value = item.counterReading,
                date = item.accountingEnd
            });
            Labels = new List<string>();
            YFormatter = value => value.ToString("C");
            ChartValues<double> values = new ChartValues<double>();
            foreach( var item in results)
            {
                values.Add(item.value);
                Labels.Add(item.date.ToString());
            }
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "template",
                    Values = values
                }
            };
            DataContext = this;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Navigation.frameNav.GoBack();
        }

    }
}
