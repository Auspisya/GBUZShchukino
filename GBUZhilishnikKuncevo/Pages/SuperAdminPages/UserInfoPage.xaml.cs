using GBUZhilishnikKuncevo.Classes;
using GBUZhilishnikKuncevo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Логика взаимодействия для UserInfoPage.xaml
    /// </summary>
    public partial class UserInfoPage : Page
    {
        public UserInfoPage(User user)
        {
            InitializeComponent();

            #region Наполняем текстовые блоки информацией из БД
            if (user.PersonalInfo.patronymic == "") { TxbFullName.Text = user.PersonalInfo.surname.ToString() + " " + user.PersonalInfo.name.ToString(); }
            else { TxbFullName.Text = user.PersonalInfo.surname.ToString() + " " + user.PersonalInfo.name.ToString() + " " + user.PersonalInfo.patronymic.ToString(); }
            TxbNumPassport.Text = user.PersonalInfo.Passport.passportNumber.ToString();
            TxbDateOfBirth.Text = user.PersonalInfo.dateOfBirth.ToShortDateString();
            TxbGender.Text = user.PersonalInfo.Gender.genderName.ToString();
            TxbPassportSeries.Text = user.PersonalInfo.Passport.passportSeries.ToString();
            TxbPassportIssuedBy.Text = user.PersonalInfo.Passport.passportIssuedBy.ToString();
            TxbDateOfIssue.Text = user.PersonalInfo.Passport.dateOfIssue.ToShortDateString();
            TxbDivisionCode.Text = user.PersonalInfo.Passport.divisionCode.ToString();
            TxbPlaceOfBirth.Text = user.PersonalInfo.Passport.placeOfBirth.ToString();
            TxbPhoneNumber.Text = user.PersonalInfo.phoneNumber.ToString();
            TxbPasswordLastChanged.Text = user.passwordLastChanged.ToShortDateString();
            #endregion
        }
        /// <summary>
        /// Переход на поедыдущую страницу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Navigation.frameNav.GoBack();
        }
    }
}
