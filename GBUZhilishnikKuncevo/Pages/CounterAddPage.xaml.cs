using GBUZhilishnikKuncevo.Classes;
using GBUZhilishnikKuncevo.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для CounterAddPage.xaml
    /// </summary>
    public partial class CounterAddPage : Page
    {
        public CounterAddPage()
        {
            InitializeComponent();
            //Заполняем поле выбора данными из БД
            CmbCounterType.DisplayMemberPath = "counterName";
            CmbCounterType.SelectedValuePath = "id";
            CmbCounterType.ItemsSource = DBConnection.DBConnect.TypeOfCounter.ToList();
            //Заполняем поле выбора данными из БД
            CmbAddress.DisplayMemberPath = "Address.fullAddress";
            CmbAddress.SelectedValuePath = "id";
            CmbAddress.ItemsSource = DBConnection.DBConnect.Apartment.ToList();
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
        /// <summary>
        /// Добавление счётчика в базу данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (TxbCounterNumber.Text == "" || CmbAddress.Text == "" || CmbCounterType.Text == "" || DPDateOfOperationStart.Text == "" || DPDateOfOperationEnd.Text == "")
            {
                MessageBox.Show("Нужно заполнить все поля!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                if (MessageBox.Show("Вы точно хотите добавить данные?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {

                }
                else
                {
                    try
                    {
                        if (DPDateOfOperationEnd.DisplayDate < DPDateOfOperationStart.DisplayDate)
                        {
                            MessageBox.Show("Начало эксплуатаций должно быть раньше чем конец", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        else
                        {
                            Counter counter = new Counter()
                            {
                                counterNumber = TxbCounterNumber.Text,
                                apartmentId = (CmbAddress.SelectedItem as Apartment).id,
                                TypeOfCounter = CmbCounterType.SelectedItem as TypeOfCounter,
                                startOfOperation = DPDateOfOperationStart.DisplayDate,
                                endOfOperation = DPDateOfOperationEnd.DisplayDate,
                            };

                            DBConnection.DBConnect.Counter.Add(counter);
                            DBConnection.DBConnect.SaveChanges();
                            MessageBox.Show("Данные успешно добавлены!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                            Navigation.frameNav.GoBack();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString(), "Критическая ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
        }
        /// <summary>
        /// Разрешение на ввод только цифр и некоторых символов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbNum_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string pattern = @"[^0-9+-,.]+";
            if (Regex.IsMatch(e.Text, pattern))
            {
                e.Handled = true;
            }
        }

        private void DP_PreviewTextInput_End(object sender, TextCompositionEventArgs e)
        {
            var dp = (DatePicker)sender;
            if (dp != null)
            {
                // Получаем текст, который будет добавлен к текущему содержимому DatePicker
                string newText = dp.Text + e.Text;

                DateTime selectedDate;
                var formats = new[] { "MM/dd/yyyy", "ddd MMM d, yyyy", "M-d-yy", "MMM.d.yyyy", "MM.dd.yyyy", "M.d.yyyy", "d.M.yyyy", "dd/MM/yyyy", "d-M-yy", "d.MMM.yyyy", "dd.MM.yyyy",
                "d/M/yyyy", "d/MMM/yyyy", "d/M/yy"};
                if (DateTime.TryParseExact(newText, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out selectedDate))
                {
                    // Получаем максимально допустимую дату из свойства DisplayDateEnd

                    // Проверяем, не превышает ли выбранная дата максимально допустимую
                    if (selectedDate > dp.DisplayDateEnd.Value)
                    {
                        // Отменяем ввод, если дата превышает максимально допустимую
                        dp.DisplayDate = dp.DisplayDateEnd.Value;
                        dp.Text = dp.DisplayDate.ToString();
                        e.Handled = false;
                    }
                }
                else
                {
                    // Отменяем ввод, если введенный текст не является датой
                    e.Handled = false;
                }
            }
        }

        private void DP_PreviewTextInput_Start(object sender, TextCompositionEventArgs e)
        {
            var dp = (DatePicker)sender;
            if (dp != null)
            {
                // Получаем текст, который будет добавлен к текущему содержимому DatePicker
                string newText = dp.Text + e.Text;

                DateTime selectedDate;
                var formats = new[] { "MM/dd/yyyy", "ddd MMM d, yyyy", "M-d-yy", "MMM.d.yyyy", "MM.dd.yyyy", "M.d.yyyy", "d.M.yyyy", "dd/MM/yyyy", "d-M-yy", "d.MMM.yyyy", "dd.MM.yyyy",
                "d/M/yyyy", "d/MMM/yyyy", "d/M/yy"};
                if (DateTime.TryParseExact(newText, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out selectedDate))
                {
                    // Получаем максимально допустимую дату из свойства DisplayDateEnd

                    // Проверяем, не превышает ли выбранная дата максимально допустимую
                    if (selectedDate < dp.DisplayDateStart.Value)
                    {
                        // Отменяем ввод, если дата превышает максимально допустимую
                        dp.DisplayDate = dp.DisplayDateStart.Value;
                        dp.Text = dp.DisplayDate.ToString();
                        e.Handled = false;
                    }
                }
                else
                {
                    // Отменяем ввод, если введенный текст не является датой
                    e.Handled = false;
                }
            }
        }

    }
}
