using GBUZhilishnikKuncevo.Classes;
using GBUZhilishnikKuncevo.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
    /// Логика взаимодействия для CounterEditPage.xaml
    /// </summary>
    public partial class CounterEditPage : Page
    {
        private int counterId;
        public CounterEditPage(Counter counter)
        {
            InitializeComponent();
            //Заполнение текстового блока
            TxbCounterNumber.Text = counter.counterNumber.ToString();
            //Заполняем поле выбора данными из БД
            CmbCounterType.DisplayMemberPath = "counterName";
            CmbCounterType.SelectedValuePath = "id";
            CmbCounterType.ItemsSource = DBConnection.DBConnect.TypeOfCounter.ToList();
            CmbCounterType.Text = counter.TypeOfCounter.counterName.ToString();
            //Заполняем поле выбора данными из БД
            CmbAddress.DisplayMemberPath = "Address.fullAddress";
            CmbAddress.SelectedValuePath = "id";
            CmbAddress.ItemsSource = DBConnection.DBConnect.Apartment.ToList();
            CmbAddress.Text = counter.Apartment.Address.fullAddress.ToString();
            DPDateOfOperationStart.Text = counter.startOfOperation.ToString();
            DPDateOfOperationEnd.Text = counter.endOfOperation.ToString();
            counterId = counter.id;
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
        /// Внести изменения в базу данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (TxbCounterNumber.Text == "" || CmbAddress.Text == "" || CmbCounterType.Text == "" || DPDateOfOperationStart.Text == "" || DPDateOfOperationEnd.Text == "")
            {
                MessageBox.Show("Нужно заполнить все поля!",
                    "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                if (MessageBox.Show("Вы точно хотите внести изменения?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {

                }
                else
                {
                    if (DPDateOfOperationEnd.DisplayDate < DPDateOfOperationStart.DisplayDate)
                    {
                        MessageBox.Show("Начало эксплуатаций должно быть раньше чем конец", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        //Подключаемся к БД
                        //menshakova_publicUtilitiesEntities context = new menshakova_publicUtilitiesEntities();
                        #region Берем значения из элементов управления и вносим их в базу данных
                        var counter = DBConnection.DBConnect.Counter.Where(item => item.id == counterId).FirstOrDefault();
                        counter.counterNumber = TxbCounterNumber.Text;
                        counter.typeOfCounterId = (CmbCounterType.SelectedItem as TypeOfCounter).id;
                        counter.apartmentId = (CmbAddress.SelectedItem as Apartment).id;
                        counter.startOfOperation = DPDateOfOperationStart.DisplayDate;
                        counter.endOfOperation = DPDateOfOperationEnd.DisplayDate;
                        #endregion
                        //Сохраняем данные в БД
                        DBConnection.DBConnect.SaveChanges();
                        MessageBox.Show("Данные успешно изменены!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                        //Возвращаемся обратно
                        Navigation.frameNav.GoBack();
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
