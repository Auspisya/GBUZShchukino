using GBUZhilishnikKuncevo.Classes;
using GBUZhilishnikKuncevo.Models;
using System;
using System.Collections.Generic;
using System.Data;
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
            ForeignPassportGrid.Visibility = Visibility.Collapsed;
            CBShowPassport.IsChecked = true;
            DPDateOfBirth.DisplayDateEnd = DateTime.Now.AddYears(-18);
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
            if ( ( TxbDivisionCode.Text == "" && CBShowPassport.IsChecked.Value) || TxbName.Text == "" || (TxbPassportIssuedBy.Text == "" && TxbPassportIssuedByF.Text == "") ||
                ( TxbPassportNumber.Text == "" && TxbPassportNumberF.Text == "") || (TxbPassportSeries.Text == ""  && CBShowPassport.IsChecked.Value) ||
                TxbPhoneNumber.Text == "" || (TxbPlaceOfBirth.Text == "" && TxbPlaceOfBirthF.Text == "") || TxbSNILS.Text == "" || TxbSurname.Text == "" ||
                TxbTIN.Text == "" || TxbWhoRegisteredTIN.Text == "" || CmbGender.Text == "" || DPDateOfBirth.Text == "" ||
                (DPDateOfIssue.Text == "" && DPDateOfIssueF.Text == "") || DPSNILSRegistationDate.Text == "" || DPTINRegistrationDate.Text == "")
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
                        var clients_with_snils = DBConnection.DBConnect.Client.Where(item => item.SNILS.snilsNumber == TxbSNILS.Text).ToList();
                        var snilses = DBConnection.DBConnect.SNILS.Where(item => item.snilsNumber == TxbSNILS.Text).ToList();
                        if (clients_with_snils.Count() > 0)
                            throw new ArgumentException("СНИЛС с таким номером уже существует");
                        SNILS snils = new SNILS();
                        if (snilses.Count() > 0)
                            snils = snilses[0];
                        else
                        {
                            snils = new SNILS()
                            {
                                snilsNumber = TxbSNILS.Text,
                                registrationDate = DateTime.Parse(DPSNILSRegistationDate.Text)
                            };
                            DBConnection.DBConnect.SNILS.Add(snils);
                            DBConnection.DBConnect.SaveChanges();
                        }
                        
                        var clients_with_tin = DBConnection.DBConnect.Client.Where(item => item.TIN.tinNumber == TxbTIN.Text).ToList();
                        var tins = DBConnection.DBConnect.TIN.Where(item => item.tinNumber == TxbTIN.Text).ToList();
                        if (clients_with_tin.Count() > 0)
                            throw new ArgumentException("ИНН с таким номером уже существует");
                        TIN tin = new TIN();
                        if (tins.Count() > 0)
                            tin = tins[0];
                        else
                        {
                            tin = new TIN()
                            {
                                tinNumber = TxbTIN.Text,
                                whoRegistered = TxbWhoRegisteredTIN.Text,
                                registrationDate = DateTime.Parse(DPTINRegistrationDate.Text)
                            };
                            DBConnection.DBConnect.TIN.Add(tin);
                            DBConnection.DBConnect.SaveChanges();
                        }
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
                        Client client = new Client()
                        {
                            snilsId = snils.id,
                            tinId = tin.id,
                            personalInfo = info.id
                        };
                        DBConnection.DBConnect.Client.Add(client);
                        DBConnection.DBConnect.SaveChanges();
                        MessageBox.Show("Данные успешно добавлены!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void DP_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var dp = (DatePicker)sender;
            dp.DisplayDate = (dp.DisplayDate > dp.DisplayDateEnd.Value ? dp.DisplayDateEnd.Value : dp.DisplayDate);
        }

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
