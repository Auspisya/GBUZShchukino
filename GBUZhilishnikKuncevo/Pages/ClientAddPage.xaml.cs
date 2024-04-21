using GBUZhilishnikKuncevo.Classes;
using GBUZhilishnikKuncevo.Models;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace GBUZhilishnikKuncevo.Pages
{
    /// <summary>
    /// Логика взаимодействия для ClientAddPage.xaml
    /// </summary>
    public partial class ClientAddPage : Page
    {
        public ClientAddPage()
        {
            InitializeComponent();

            //Заполняем поле выбора данными из БД
            CmbGender.DisplayMemberPath = "genderName";
            CmbGender.SelectedValuePath = "id";
            CmbGender.ItemsSource = DBConnection.DBConnect.Gender.ToList();
        }

        /// <summary>
        /// Возвращаемся назад
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Navigation.frameNav.GoBack();
        }

        /// <summary>
        /// Добавляем нового пользователя в базу данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (TxbDivisionCode.Text == "" || TxbName.Text == "" || TxbPassportIssuedBy.Text == "" ||
                TxbPassportNumber.Text == "" || TxbPassportSeries.Text == "" ||
                TxbPhoneNumber.Text == "" || TxbPlaceOfBirth.Text == "" || TxbSNILS.Text == "" || TxbSurname.Text == "" ||
                TxbTIN.Text == "" || TxbWhoRegisteredTIN.Text == "" || CmbGender.Text == "" || DPDateOfBirth.Text == "" ||
                DPDateOfIssue.Text == "" || DPSNILSRegistationDate.Text == "" || DPTINRegistrationDate.Text == "")
            {
                MessageBox.Show("Нужно заполнить все поля!",
                    "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                        Passport passport = new Passport()
                        {
                            passportNumber = TxbPassportNumber.Text,
                            passportSeries = TxbPassportSeries.Text,
                            passportIssuedBy = TxbPassportIssuedBy.Text,
                            placeOfBirth = TxbPlaceOfBirth.Text,
                            dateOfIssue = DateTime.Parse(DPDateOfIssue.Text),
                            divisionCode = TxbDivisionCode.Text
                        };

                        SNILS snils = new SNILS()
                        {
                            snilsNumber = TxbSNILS.Text,
                            registrationDate = DateTime.Parse(DPSNILSRegistationDate.Text)
                        };

                        TIN tin = new TIN()
                        {
                            tinNumber = TxbTIN.Text,
                            whoRegistered = TxbWhoRegisteredTIN.Text,
                            registrationDate = DateTime.Parse(DPTINRegistrationDate.Text)
                        };

                        PersonalInfo info = new PersonalInfo()
                        {
                            Gender = CmbGender.SelectedItem as Gender,
                            name = TxbName.Text,
                            surname = TxbSurname.Text,
                            patronymic = TxbPatronymic.Text,
                            phoneNumber = TxbPhoneNumber.Text,
                            dateOfBirth = DateTime.Parse(DPDateOfBirth.Text),
                            passportId = passport.id
                        };

                        Client client = new Client()
                        {
                            snilsId = snils.id,
                            tinId = tin.id,
                            personalInfo = info.id
                        };
                        DBConnection.DBConnect.Client.Add(client);
                        DBConnection.DBConnect.Passport.Add(passport);
                        DBConnection.DBConnect.TIN.Add(tin);
                        DBConnection.DBConnect.SNILS.Add(snils);
                        DBConnection.DBConnect.SaveChanges();
                        MessageBox.Show("Данные успешно добавлены!",
                            "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                        Navigation.frameNav.GoBack();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString(),
                            "Критическая ошибка",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                    }
                }
            }    
        }

        /// <summary>
        /// Разрешение на ввод только букв и некоторых символов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Txb_KeyDown(object sender, KeyEventArgs e)
        {

            Regex pattern = new Regex("^[a-zA-Z]+$");

            if (!pattern.IsMatch(e.Key.ToString()))
            {
                //Отмена нажатия клавиши, если символ не соответствует шаблону
                e.Handled = true;
            }
        }

        /// <summary>
        /// Разрешение на ввод только букв и некоторых символов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Txb_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string pattern = @"[\d\p{P}]";
            if (Regex.IsMatch(e.Text, pattern))
            {
                e.Handled = true;
            }
        }
        /// <summary>
        /// Разрешение на ввод только цифр и некоторых символов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbNum_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string pattern = @"[^0-9+-]+";
            if (Regex.IsMatch(e.Text, pattern))
            {
                e.Handled = true;
            }
        }
    }
}
