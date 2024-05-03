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
    /// Логика взаимодействия для AccountingAddPage.xaml
    /// </summary>
    public partial class AccountingAddPage : Page
    {
        private TypeOfCounter desiredCounterType = null;

        public AccountingAddPage()
        {
            InitializeComponent();
            ///Наполняем поле выбора из БД
            CmbService.DisplayMemberPath = "TypeOfService.serviceName";
            CmbService.SelectedValuePath = "id";
            CmbService.ItemsSource = DBConnection.DBConnect.Service.ToList();
            ///Наполняем поле выбора из БД
            CmbCounterNumber.DisplayMemberPath = "counterNumber";
            CmbCounterNumber.SelectedValuePath = "id";
            CmbCounterNumber.ItemsSource = DBConnection.DBConnect.Counter.ToList();
            ///Наполняем поле выбора из БД
            CmbBankBook.DisplayMemberPath = "bankBookNumber";
            CmbBankBook.SelectedValuePath = "id";
            CmbBankBook.ItemsSource = DBConnection.DBConnect.BankBook.ToList();

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
        /// Добавление показаний и формирование чека по услуге в базу данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (TxbCounterReading.Text == "" || DPDateOfEnd.Text == "" || DPDateOfStart.Text == "" || CmbService.Text == "")
            {
                MessageBox.Show("Нужно заполнить все поля!",
                    "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                if (MessageBox.Show("Вы точно хотите добавить показания?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {

                }
                else
                {
                    try
                    {
                        var serviceAccountingCheck = decimal.Parse(TxbCounterReading.Text);
                            
                        Accounting accounting = new Accounting()
                        {
                            counterReading = double.Parse(TxbCounterReading.Text),
                            Service = CmbService.SelectedItem as Service,
                            Counter = CmbCounterNumber.SelectedItem as Counter,
                            BankBook = CmbBankBook.SelectedItem as BankBook,
                            accountingStart = DateTime.Parse(DPDateOfStart.Text),
                            accountingEnd = DateTime.Parse(DPDateOfEnd.Text)
                        };

                        ServiceCheck serviceCheck = new ServiceCheck()
                        {
                            accountingId = accounting.id,
                            totalPayble = serviceAccountingCheck * (decimal)accounting.Service.standartTariff,
                        };

                        DBConnection.DBConnect.Accounting.Add(accounting);
                        DBConnection.DBConnect.ServiceCheck.Add(serviceCheck);
                        DBConnection.DBConnect.SaveChanges();
                        MessageBox.Show("Показание успешно добавлено!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                        Navigation.frameNav.GoBack();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString(), "Критическая ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
        }

        /// <summary>
        /// Вывод единицы измерения и фильтрация типа счетчика в зависимости от выбранных услуги и лицевого счета
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbService_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TxbCounterReading.Text = "";
            var service = CmbService.SelectedItem as Service;
            var bankBook = CmbBankBook.SelectedItem as BankBook;
            var unit = service.unit;
            TxbUnit.Text = unit.ToString();
            switch (service.TypeOfService.id)
            {
                case 1:
                    desiredCounterType = DBConnection.DBConnect.TypeOfCounter.ToList().Where(x => x.id == 3).ToList()[0];
                    break;
                case 2:
                    desiredCounterType = DBConnection.DBConnect.TypeOfCounter.ToList().Where(x => x.id == 2).ToList()[0];
                    break;
                case 3:
                    desiredCounterType = DBConnection.DBConnect.TypeOfCounter.ToList().Where(x => x.id == 1).ToList()[0];
                    break;
                case 4:
                    desiredCounterType = DBConnection.DBConnect.TypeOfCounter.ToList().Where(x => x.id == 4).ToList()[0];
                    break;
                case 5:
                    desiredCounterType = null;
                    break;
                case 6:
                    desiredCounterType = DBConnection.DBConnect.TypeOfCounter.ToList().Where(x => x.id == 5).ToList()[0];
                    break;
                case 7:
                    desiredCounterType = null;
                    break;
            }

            //В зависимости от значения desiredCounterType, выпадающий список счетчиков (CmbCounterNumber) и текстовое поле с типом счетчика (TxbCounterType) делаются доступными или недоступными
            CmbCounterNumber.IsEnabled = desiredCounterType != null;
            TxbCounterType.IsEnabled = desiredCounterType != null;
            if (!TxbCounterType.IsEnabled)
                TxbCounterType.Text = "";
            if (!CmbCounterNumber.IsEnabled)
                CmbCounterNumber.Text = "";
            if (bankBook != null && service.TypeOfService.id == 7)
                TxbCounterReading.Text = bankBook.Apartment.numberOfResidents.ToString();
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
        /// <summary>
        /// Вывод типа счётчика в зависимости от выбранного номера счётчика
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbCounterNumber_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbCounterNumber.SelectedIndex == -1) { TxbCounterType.Text = ""; }
            else
            {
                try
                {
                    var counterType = (CmbCounterNumber.SelectedItem as Counter).TypeOfCounter.counterName;
                    TxbCounterType.Text = counterType.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        /// <summary>
        /// Фильтрует поле для выбора счётчика, в зависимости от выбранного лицевого счёта + автоматически подставляет количество проживающих, если выбрана услуга вывоза мусора
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbBankBook_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TxbCounterReading.Text = "";
            if (CmbBankBook.SelectedIndex == -1) //Если индекс выбранного элемента равен -1 (то есть ничего не выбрано), то список счетчиков (CmbCounterNumber) очищается
            {
                CmbCounterNumber.ItemsSource = new List<Counter>();
            }
            else
            {
                try
                {
                    var bankBook = CmbBankBook.SelectedItem as BankBook;
                    var service = CmbService.SelectedItem as Service;
                    if (desiredCounterType != null) //Если определен тип счетчика (desiredCounterType), то обновляется источник данных для выпадающего списка счетчиков, фильтруя их по критериям
                    {
                        CmbCounterNumber.ItemsSource = DBConnection.DBConnect.Counter.ToList().Where(x =>
                       x.apartmentId == bankBook.apartmentId && x.typeOfCounterId == desiredCounterType.id).ToList();
                    }
                    if (service != null && service.TypeOfService.id == 7)
                        TxbCounterReading.Text = bankBook.Apartment.numberOfResidents.ToString();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        /// <summary>
        /// Ограничитель по выбору времени
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DP_PreviewTextInput(object sender, TextCompositionEventArgs e)
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
    }
}
