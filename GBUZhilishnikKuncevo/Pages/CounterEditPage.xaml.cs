using GBUZhilishnikKuncevo.Classes;
using GBUZhilishnikKuncevo.Models;
using System;
using System.Collections.Generic;
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
            if (TxbCounterNumber.Text == "" || CmbAddress.Text == "" || CmbCounterType.Text == "")
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
                    //Подключаемся к БД
                    menshakova_publicUtilitiesEntities context = new menshakova_publicUtilitiesEntities();
                    #region Берем значения из элементов управления и вносим их в базу данных
                    var counter = context.Counter.Where(item => item.id == counterId).FirstOrDefault();
                    counter.counterNumber = TxbCounterNumber.Text;
                    counter.typeOfCounterId = (CmbCounterType.SelectedItem as TypeOfCounter).id;
                    counter.apartmentId = (CmbAddress.SelectedItem as Apartment).id;
                    #endregion
                    //Сохраняем данные в БД
                    context.SaveChanges();
                    MessageBox.Show("Данные успешно изменены!",
                            "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    //Возвращаемся обратно
                    Navigation.frameNav.GoBack();
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
    }
}
