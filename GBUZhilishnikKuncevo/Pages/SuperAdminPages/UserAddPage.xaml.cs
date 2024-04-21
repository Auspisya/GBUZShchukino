using GBUZhilishnikKuncevo.Classes;
using GBUZhilishnikKuncevo.Models;
using GBUZhilishnikKuncevo.Pages.AuthPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

namespace GBUZhilishnikKuncevo.Pages.SuperAdminPages
{
    /// <summary>
    /// Логика взаимодействия для UserAddPage.xaml
    /// </summary>
    public partial class UserAddPage : Page
    {
        string password;
        public UserAddPage()
        {
            InitializeComponent();
            CmbGender.DisplayMemberPath = "genderName";
            CmbGender.SelectedValuePath = "id";
            CmbGender.ItemsSource = DBConnection.DBConnect.Gender.ToList();

            CmbRole.DisplayMemberPath = "roleName";
            CmbRole.SelectedValuePath = "id";
            CmbRole.ItemsSource = DBConnection.DBConnect.UserRole.ToList();
        }

        /// <summary>
        /// Переход на предыдущую страницу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Navigation.frameNav.GoBack();
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
        /// <summary>
        /// Добавить пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (TxbDivisionCode.Text == "" || TxbName.Text == "" || TxbPassportIssuedBy.Text == "" ||
                TxbPassportNumber.Text == "" || TxbPassportSeries.Text == "" ||
                TxbPhoneNumber.Text == "" || TxbPlaceOfBirth.Text == "" || TxbSurname.Text == "" ||
                CmbGender.Text == "" || DPDateOfBirth.Text == "" ||
                DPDateOfIssue.Text == "" || TxbLogin.Text == "" || password == "" || CmbRole.Text == "")
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

                        User user = new User()
                        {
                            login = TxbLogin.Text,
                            password = PasswordChangePage.GetMD5Hash(password).ToUpper(),
                            registrationDate = DateTime.Now,
                            passwordLastChanged = DateTime.Now,
                            UserRole = CmbRole.SelectedItem as UserRole,
                            userStatusId = 2,
                            personalInfoId = info.id
                        };

                        DBConnection.DBConnect.Passport.Add(passport);
                        DBConnection.DBConnect.PersonalInfo.Add(info);
                        DBConnection.DBConnect.User.Add(user);
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

        private void TxbPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            password = TxbPassword.Text;
        }

        private void PsbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            password = PsbPassword.Password;
        }

        /// <summary>
        /// Показать/скрыть пароль
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CBShowHidePassword_Click(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox.IsChecked.Value)
            {
                TxbPassword.Text = PsbPassword.Password;
                TxbPassword.Visibility = Visibility.Visible;
                PsbPassword.Visibility = Visibility.Hidden;
                CBShowHidePassword.ToolTip = "Скрыть";
            }
            else
            {
                PsbPassword.Password = TxbPassword.Text;
                TxbPassword.Visibility = Visibility.Hidden;
                PsbPassword.Visibility = Visibility.Visible;
                CBShowHidePassword.ToolTip = "Показать";
            }
        }
    }
}
