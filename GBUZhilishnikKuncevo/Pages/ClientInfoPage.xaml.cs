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

namespace GBUZhilishnikKuncevo.Pages
{
    /// <summary>
    /// Логика взаимодействия для ClientInfoPage.xaml
    /// </summary>
    public partial class ClientInfoPage : Page
    {
        public ClientInfoPage(Client client)
        {
            InitializeComponent();

            #region Наполняем текстовые блоки информацией из БД
            if (client.PersonalInfo1.patronymic == "") { TxbFullName.Text = client.PersonalInfo1.surname.ToString() + " " + client.PersonalInfo1.name.ToString(); } 
            else { TxbFullName.Text = client.PersonalInfo1.surname.ToString() + " " + client.PersonalInfo1.name.ToString() + " " + client.PersonalInfo1.patronymic.ToString(); }
            TxbNumPassport.Text = client.PersonalInfo1.Passport.passportNumber.ToString();
            TxbDateOfBirth.Text = client.PersonalInfo1.dateOfBirth.ToShortDateString();
            TxbGender.Text = client.PersonalInfo1.Gender.genderName.ToString();
            TxbPassportSeries.Text = client.PersonalInfo1.Passport.passportSeries.ToString();
            TxbPassportIssuedBy.Text = client.PersonalInfo1.Passport.passportIssuedBy.ToString();
            TxbDateOfIssue.Text = client.PersonalInfo1.Passport.dateOfIssue.ToShortDateString();
            TxbDivisionCode.Text = client.PersonalInfo1.Passport.divisionCode.ToString();
            TxbPlaceOfBirth.Text = client.PersonalInfo1.Passport.placeOfBirth.ToString();
            TxbWhoRegisteredTIN.Text = client.TIN.whoRegistered.ToString();
            TxbTINRegistrationDate.Text = client.TIN.registrationDate.ToShortDateString();
            TxbSNILSRegistrationDate.Text = client.SNILS.registrationDate.ToShortDateString();
            TxbSNILS.Text = client.SNILS.snilsNumber.ToString();
            TxbTIN.Text = client.TIN.tinNumber.ToString();

            var bankBookList = DBConnection.DBConnect.BankBook.ToList();
            var bankBook = bankBookList.Where(item => item.clientId == client.id).ToList();


            CmbBankBook.DisplayMemberPath = "bankBookNumber";
            CmbBankBook.SelectedValuePath = "id";
            CmbBankBook.ItemsSource = bankBook;
            if (client.BankBook.Count != 0) { CmbBankBook.Text = bankBook[0].bankBookNumber.ToString(); } else { CmbBankBook.Text = ""; }  
            #endregion
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
    }
}
