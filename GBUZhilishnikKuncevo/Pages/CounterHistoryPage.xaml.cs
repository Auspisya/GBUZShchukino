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
            // Устанавливаем текстовое значение номера выбранного счетчика.
            TxbCounterNumber.Text = counter.counterNumber;
            // Устанавливаем текстовое значение типа выбранного счетчика.
            TxbCounterType.Text = counter.TypeOfCounter.counterName;
            // Устанавливаем значение counterId равным идентификатору выбранного счетчика.
            counterId = counter.id;
            // Получаем данные из базы данных для заполнения графика.
            var results = DBConnection.DBConnect.Accounting.Where(item => item.counterId == counterId).Select(item => new
            {
                value = item.counterReading,
                date = item.accountingEnd
            });
            Labels = new List<string>();

            string unitStr = "";
            if (counter.Accounting.Count() != 0)
            {
                unitStr = counter.Accounting.First().Service.unit;
            }
            ChartValues<double> values = new ChartValues<double>();

            YFormatter = value => value.ToString() + " " + unitStr;
            
            // Заполняем коллекции значений и меток данными из результатов запроса.
            foreach ( var item in results)
            {
                values.Add(item.value);
                Labels.Add(item.date.ToString());
            }
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "",
                    Values = values
                }
            };
            DataContext = this;
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
