using GBUZhilishnikKuncevo.Classes;
using GBUZhilishnikKuncevo.Models;
using GBUZhilishnikKuncevo.Pages.AuthPages;
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

namespace GBUZhilishnikKuncevo.Pages.SuperAdminPages
{
    /// <summary>
    /// Логика взаимодействия для UserEditPage.xaml
    /// </summary>
    public partial class UserEditPage : Page
    {
        string password;
        int userId;
        public UserEditPage(User user)
        {
            InitializeComponent();

            #region Заполняем элементы управления данными из БД
            // Заполняем текстовые блоки готовыми данными из БД
            TxbName.Text = user.PersonalInfo.name.ToString();
            TxbSurname.Text = user.PersonalInfo.surname.ToString();
            if (user.PersonalInfo.patronymic == "") { TxbPatronymic.Text = ""; } else { TxbPatronymic.Text = user.PersonalInfo.patronymic.ToString(); }
            TxbDivisionCode.Text = user.PersonalInfo.Passport.divisionCode.ToString();
            TxbPassportIssuedBy.Text = user.PersonalInfo.Passport.passportIssuedBy.ToString();
            TxbPassportNumber.Text = user.PersonalInfo.Passport.passportNumber.ToString();
            TxbPassportSeries.Text = user.PersonalInfo.Passport.passportSeries.ToString();
            TxbPhoneNumber.Text = user.PersonalInfo.phoneNumber.ToString();
            TxbPlaceOfBirth.Text = user.PersonalInfo.Passport.placeOfBirth.ToString();
            TxbLogin.Text = user.login.ToString();
            //Заполняем поля для выбора готовыми данными из БД
            CmbGender.DisplayMemberPath = "genderName";
            CmbGender.SelectedValuePath = "id";
            CmbGender.ItemsSource = DBConnection.DBConnect.Gender.ToList();
            CmbGender.Text = user.PersonalInfo.Gender.genderName.ToString();
            CmbRole.DisplayMemberPath = "roleName";
            CmbRole.SelectedValuePath = "id";
            CmbRole.ItemsSource = DBConnection.DBConnect.UserRole.ToList();
            CmbRole.Text = user.UserRole.roleName.ToString();
            //Заполняем дата-пикеры готовыми данными из БД
            DPDateOfBirth.Text = user.PersonalInfo.dateOfBirth.ToString();
            DPDateOfIssue.Text = user.PersonalInfo.Passport.dateOfIssue.ToString();
            #endregion
            //Присваиваем ID квартиросъёмщика, которого выбрали, чтобы использовать в дальнейшем
            userId = user.id;
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
        /// Внести изменения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (TxbDivisionCode.Text == "" || TxbName.Text == "" || TxbPassportIssuedBy.Text == "" ||
                TxbPassportNumber.Text == "" || TxbPassportSeries.Text == "" ||
                TxbPhoneNumber.Text == "" || TxbPlaceOfBirth.Text == "" || TxbSurname.Text == "" ||
                CmbGender.Text == "" || DPDateOfBirth.Text == "" ||
                DPDateOfIssue.Text == "" || TxbLogin.Text == ""  || CmbRole.Text == "")
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
                    var user = context.User.Where(item => item.id == userId).FirstOrDefault();
                    user.PersonalInfo.surname = TxbSurname.Text;
                    user.PersonalInfo.name = TxbName.Text;
                    user.PersonalInfo.patronymic = TxbPatronymic.Text;
                    user.PersonalInfo.phoneNumber = TxbPhoneNumber.Text;
                    user.PersonalInfo.genderId = (CmbGender.SelectedItem as Gender).id;
                    user.PersonalInfo.dateOfBirth = DateTime.Parse(DPDateOfBirth.Text);
                    user.PersonalInfo.Passport.placeOfBirth = TxbPlaceOfBirth.Text;
                    user.PersonalInfo.Passport.passportNumber = TxbPassportNumber.Text;
                    user.PersonalInfo.Passport.passportSeries = TxbPassportSeries.Text;
                    user.PersonalInfo.Passport.passportIssuedBy = TxbPassportIssuedBy.Text;
                    user.PersonalInfo.Passport.divisionCode = TxbDivisionCode.Text;
                    user.PersonalInfo.Passport.dateOfIssue = DateTime.Parse(DPDateOfIssue.Text);
                    user.login = TxbLogin.Text;
                    user.userRoleId = (CmbRole.SelectedItem as UserRole).id;
                    if (user.password == password)
                    {
                        
                    }
                    else if (password != null && password != "")
                    {
                        user.password = PasswordChangePage.GetMD5Hash(password).ToUpper();
                        user.passwordLastChanged = DateTime.Now;
                    }
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
    }
}
