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

namespace GBUZhilishnikKuncevo.Pages
{
    /// <summary>
    /// Логика взаимодействия для ClientEditPage.xaml
    /// </summary>
    public partial class ClientEditPage : Page
    {
        private int clientId;

        public ClientEditPage(Client client)
        {
            InitializeComponent();

            #region Заполняем элементы управления данными из БД
            // Заполняем текстовые блоки готовыми данными из БД
            TxbName.Text = client.PersonalInfo1.name.ToString();
            TxbSurname.Text = client.PersonalInfo1.surname.ToString();
            TxbPhoneNumber.Text = client.PersonalInfo1.phoneNumber.ToString();
            DPDateOfBirth.Text = client.PersonalInfo1.dateOfBirth.ToString();
            //Заполняем поля для выбора готовыми данными из БД
            CmbGender.DisplayMemberPath = "genderName";
            CmbGender.SelectedValuePath = "id";
            CmbGender.ItemsSource = DBConnection.DBConnect.Gender.ToList();
            CmbGender.Text = client.PersonalInfo1.Gender.genderName.ToString();
            if (client.PersonalInfo1.patronymic == "") { TxbPatronymic.Text = ""; } else { TxbPatronymic.Text = client.PersonalInfo1.patronymic.ToString(); }
            if (client.PersonalInfo1.Passport.divisionCode.ToString() == "" || client.PersonalInfo1.Passport.passportNumber.ToString() == "")
            {
                TxbPlaceOfBirthF.Text = client.PersonalInfo1.Passport.placeOfBirth.ToString();
                TxbPassportIssuedByF.Text = client.PersonalInfo1.Passport.passportIssuedBy.ToString();
                TxbPassportNumberF.Text = client.PersonalInfo1.Passport.passportNumber.ToString();
                DPDateOfIssueF.Text = client.PersonalInfo1.Passport.dateOfIssue.ToString();
                PassportGrid.Visibility = Visibility.Collapsed;
                CBShowForeignPassport.IsChecked = true;
            }
            else
            {
                TxbDivisionCode.Text = client.PersonalInfo1.Passport.divisionCode.ToString();
                TxbPassportIssuedBy.Text = client.PersonalInfo1.Passport.passportIssuedBy.ToString();
                TxbPassportNumber.Text = client.PersonalInfo1.Passport.passportNumber.ToString();
                TxbPassportSeries.Text = client.PersonalInfo1.Passport.passportSeries.ToString();
                TxbPlaceOfBirth.Text = client.PersonalInfo1.Passport.placeOfBirth.ToString();
                DPDateOfIssue.Text = client.PersonalInfo1.Passport.dateOfIssue.ToString();
                ForeignPassportGrid.Visibility = Visibility.Collapsed;
                CBShowPassport.IsChecked = true;
            }
            TxbTIN.Text = client.TIN.tinNumber.ToString();
            TxbWhoRegisteredTIN.Text = client.TIN.whoRegistered.ToString();
            DPTINRegistrationDate.Text = client.TIN.registrationDate.ToString();
            //Заполняем дата-пикеры готовыми данными из БД
            TxbSNILS.Text = client.SNILS.snilsNumber.ToString();
            DPSNILSRegistationDate.Text = client.SNILS.registrationDate.ToString();
            #endregion
            //Присваиваем ID квартиросъёмщика, которого выбрали, чтобы использовать в дальнейшем
            clientId = client.id;
            DPDateOfBirth.DisplayDateEnd = DateTime.Now.AddYears(-18);
        }

        /// <summary>
        ///  Вносим изменения в базу данных или отказываемся от этого действия
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if ((TxbDivisionCode.Text == "" && CBShowPassport.IsChecked.Value) || TxbName.Text == "" || (TxbPassportIssuedBy.Text == "" && TxbPassportIssuedByF.Text == "") ||
                (TxbPassportNumber.Text == "" && TxbPassportNumberF.Text == "") || (TxbPassportSeries.Text == "" && CBShowPassport.IsChecked.Value) ||
                TxbPhoneNumber.Text == "" || (TxbPlaceOfBirth.Text == "" && TxbPlaceOfBirthF.Text == "") || TxbSNILS.Text == "" || TxbSurname.Text == "" ||
                TxbTIN.Text == "" || TxbWhoRegisteredTIN.Text == "" || CmbGender.Text == "" || DPDateOfBirth.Text == "" ||
                (DPDateOfIssue.Text == "" && DPDateOfIssueF.Text == "") || DPSNILSRegistationDate.Text == "" || DPTINRegistrationDate.Text == "")
            {
                MessageBox.Show("Нужно заполнить все поля!",
                    "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else {
                if (MessageBox.Show("Вы точно хотите внести изменения?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {

                }
                else
                {

                    try
                    {
                        //Подключаемся к БД
                        //menshakova_publicUtilitiesEntities context = new menshakova_publicUtilitiesEntities();
                        #region Берем значения из элементов управления и вносим их в базу данных
                        var client = DBConnection.DBConnect.Client.Where(item => item.id == clientId).FirstOrDefault();
                        client.PersonalInfo1.surname = TxbSurname.Text;
                        client.PersonalInfo1.name = TxbName.Text;
                        client.PersonalInfo1.patronymic = TxbPatronymic.Text;
                        client.PersonalInfo1.phoneNumber = TxbPhoneNumber.Text;
                        client.PersonalInfo1.genderId = (CmbGender.SelectedItem as Gender).id;
                        client.PersonalInfo1.dateOfBirth = DateTime.Parse(DPDateOfBirth.Text);
                        if (CBShowForeignPassport.IsChecked.Value)
                        {
                            client.PersonalInfo1.Passport.placeOfBirth = TxbPlaceOfBirthF.Text;
                            client.PersonalInfo1.Passport.passportNumber = TxbPassportNumberF.Text;
                            client.PersonalInfo1.Passport.passportSeries = "";
                            client.PersonalInfo1.Passport.passportIssuedBy = TxbPassportIssuedByF.Text;
                            client.PersonalInfo1.Passport.divisionCode = "";
                            client.PersonalInfo1.Passport.dateOfIssue = DateTime.Parse(DPDateOfIssueF.Text);
                        }
                        else
                        {
                            client.PersonalInfo1.Passport.placeOfBirth = TxbPlaceOfBirth.Text;
                            client.PersonalInfo1.Passport.passportNumber = TxbPassportNumber.Text;
                            client.PersonalInfo1.Passport.passportSeries = TxbPassportSeries.Text;
                            client.PersonalInfo1.Passport.passportIssuedBy = TxbPassportIssuedBy.Text;
                            client.PersonalInfo1.Passport.divisionCode = TxbDivisionCode.Text;
                            client.PersonalInfo1.Passport.dateOfIssue = DateTime.Parse(DPDateOfIssue.Text);
                        }


                        var clients_with_snils = DBConnection.DBConnect.Client.Where(item => item.SNILS.snilsNumber == TxbSNILS.Text).ToList();
                        var snilses = DBConnection.DBConnect.SNILS.Where(item => item.snilsNumber == TxbSNILS.Text).ToList();
                        if (clients_with_snils.Count() > 0 && clients_with_snils[0].id != clientId)
                            throw new ArgumentException("СНИЛС с таким номером уже существует");

                        if (snilses.Count() > 0 && snilses[0].id != client.snilsId)
                        {
                            //DBConnection.DBConnect.SNILS.Remove(client.SNILS);
                            //DBConnection.DBConnect.SaveChanges();
                            client.SNILS = snilses[0];
                            //DBConnection.DBConnect.SaveChanges();
                        }
                        else
                        {
                            client.SNILS.snilsNumber = TxbSNILS.Text;
                            client.SNILS.registrationDate = DateTime.Parse(DPSNILSRegistationDate.Text);
                        }

                        var clients_with_tin = DBConnection.DBConnect.Client.Where(item => item.TIN.tinNumber == TxbTIN.Text).ToList();
                        var tins = DBConnection.DBConnect.TIN.Where(item => item.tinNumber == TxbTIN.Text).ToList();
                        if (clients_with_tin.Count() > 0 && clients_with_tin[0].id != clientId)
                            throw new ArgumentException("ИНН с таким номером уже существует");
                        if (tins.Count() > 0 && tins[0].id != client.tinId)
                        {
                            //var tin_to_delete = DBConnection.DBConnect.TIN.Where(item => item.id == client.tinId).FirstOrDefault();
                            ///DBConnection.DBConnect.TIN.Remove(tin_to_delete);
                            //DBConnection.DBConnect.SaveChanges();
                            client.TIN = tins[0];
                        }
                        else
                        {
                            client.TIN.tinNumber = TxbTIN.Text;
                            client.TIN.whoRegistered = TxbWhoRegisteredTIN.Text;
                            client.TIN.registrationDate = DateTime.Parse(DPTINRegistrationDate.Text);
                        }
                        #endregion
                        //Сохраняем данные в БД
                        DBConnection.DBConnect.SaveChanges();
                        MessageBox.Show("Данные успешно изменены!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                        //Возвращаемся обратно
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
        /// Возвращает назад
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
        }
        /// <summary>
        /// Показать/скрыть поля для заполнения паспортных данных гражданина РФ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// Обработчик события изменения выбранной даты в элементе управления DatePicker.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DP_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var dp = (sender as DatePicker);
            // Устанавливаем отображаемую дату в DatePicker равной минимальной из выбранной даты и максимальной даты.
            // Это предотвращает отображение даты, превышающей максимальную дату, установленную для DatePicker.
            dp.DisplayDate = (dp.DisplayDate > dp.DisplayDateEnd.Value ? dp.DisplayDateEnd.Value : dp.DisplayDate);
        }
        /// <summary>
        /// Ограничитель выбора даты
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
