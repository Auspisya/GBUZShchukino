using GBUZhilishnikKuncevo.Classes;
using GBUZhilishnikKuncevo.Models;
using GBUZhilishnikKuncevo.Pages.AuthPages;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            #region Заполнение элементов дизайна информацией из БД
            CmbGender.DisplayMemberPath = "genderName";
            CmbGender.SelectedValuePath = "id";
            CmbGender.ItemsSource = DBConnection.DBConnect.Gender.ToList();

            CmbRole.DisplayMemberPath = "roleName";
            CmbRole.SelectedValuePath = "id";
            CmbRole.ItemsSource = DBConnection.DBConnect.UserRole.ToList();
            ForeignPassportGrid.Visibility = Visibility.Collapsed;
            CBShowPassport.IsChecked = true;
            DPDateOfBirth.DisplayDateEnd = DateTime.Now.AddYears(-18);
            #endregion
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
            if ((TxbDivisionCode.Text == "" && CBShowPassport.IsChecked.Value) || TxbName.Text == "" || (TxbPassportIssuedBy.Text == "" && TxbPassportIssuedByF.Text == "") ||
                (TxbPassportNumber.Text == "" && TxbPassportNumberF.Text == "") || (TxbPassportSeries.Text == "" && CBShowPassport.IsChecked.Value) ||
                TxbPhoneNumber.Text == "" || (TxbPlaceOfBirth.Text == "" && TxbPlaceOfBirthF.Text == "") || TxbSurname.Text == "" ||
                CmbGender.Text == "" || DPDateOfBirth.Text == "" ||
                (DPDateOfIssue.Text == "" && DPDateOfIssueF.Text == "") || TxbLogin.Text == "" || password == "" || CmbRole.Text == "")
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
                        Passport passport = new Passport();
                        if (CBShowPassport.IsChecked.Value)
                        {
                            passport = new Passport()
                            {
                                passportNumber = TxbPassportNumber.Text,
                                passportSeries = TxbPassportSeries.Text,
                                passportIssuedBy = TxbPassportIssuedBy.Text,
                                placeOfBirth = TxbPlaceOfBirth.Text,
                                dateOfIssue = DateTime.Parse(DPDateOfIssue.Text),
                                divisionCode = TxbDivisionCode.Text
                            };
                        }
                        else
                        {
                            passport = new Passport()
                            {
                                passportNumber = TxbPassportNumberF.Text,
                                passportSeries = "",
                                passportIssuedBy = TxbPassportIssuedByF.Text,
                                placeOfBirth = TxbPlaceOfBirthF.Text,
                                dateOfIssue = DateTime.Parse(DPDateOfIssueF.Text),
                                divisionCode = ""
                            };
                        }
                        DBConnection.DBConnect.Passport.Add(passport);
                        DBConnection.DBConnect.SaveChanges();
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
                        DBConnection.DBConnect.PersonalInfo.Add(info);
                        DBConnection.DBConnect.SaveChanges();
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
                        DBConnection.DBConnect.User.Add(user);
                        DBConnection.DBConnect.SaveChanges();
                        MessageBox.Show("Данные успешно добавлены!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
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
        /// Меняет содержимое в зависимости от того, что было введено в поле ввода пароля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            password = TxbPassword.Text;
        }
        /// <summary>
        /// Меняет содержимое в зависимости от того, что было введено в текстовом поле
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// Показать/скрыть поля для заполнения паспортных данных иностранного гражданина
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CBShowForeignPassport_Click(object sender, RoutedEventArgs e)
        {
            if (CBShowForeignPassport.IsChecked.Value)
            {
                ForeignPassportGrid.Visibility = Visibility.Visible;
                PassportGrid.Visibility = Visibility.Collapsed;
                CBShowPassport.IsChecked = false;
            }
            else
            {
                ForeignPassportGrid.Visibility = Visibility.Collapsed;
                PassportGrid.Visibility = Visibility.Visible;
                CBShowPassport.IsChecked = true;
            }
            /// <summary>
            /// Показать/скрыть поля для заполнения паспортных данных РФ
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
        }
        private void CBShowPassport_Click(object sender, RoutedEventArgs e)
        {
            if (CBShowPassport.IsChecked.Value)
            {
                ForeignPassportGrid.Visibility = Visibility.Collapsed;
                PassportGrid.Visibility = Visibility.Visible;
                CBShowForeignPassport.IsChecked = false;
            }
            else
            {
                ForeignPassportGrid.Visibility = Visibility.Visible;
                PassportGrid.Visibility = Visibility.Collapsed;
                CBShowForeignPassport.IsChecked = true;
            }
        }

        /// <summary>
        /// Ограничитель ввода даты
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
                        e.Handled = true;
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
