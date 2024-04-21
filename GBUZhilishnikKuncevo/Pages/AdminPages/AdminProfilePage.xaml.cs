using GBUZhilishnikKuncevo.Classes;
using GBUZhilishnikKuncevo.Models;
using GBUZhilishnikKuncevo.Pages.AuthPages;
using GBUZhilishnikKuncevo.Pages.MenuPages;
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

namespace GBUZhilishnikKuncevo.Pages.AdminPages
{
    /// <summary>
    /// Логика взаимодействия для AdminProfilePage.xaml
    /// </summary>
    public partial class AdminProfilePage : Page
    {
        public static int UserId;
        public AdminProfilePage()
        {
            InitializeComponent();
            menshakova_publicUtilitiesEntities context = new menshakova_publicUtilitiesEntities();
            var user = context.User.Where(item => item.id == UserId).FirstOrDefault();
            if (user.PersonalInfo.patronymic == "") { TxbUserFullName.Text = user.PersonalInfo.surname.ToString() + " " + user.PersonalInfo.name.ToString(); }
            else { TxbUserFullName.Text = user.PersonalInfo.surname.ToString() + " " + user.PersonalInfo.name.ToString() + " " + user.PersonalInfo.patronymic.ToString(); }
            TxbNumPassport.Text = user.PersonalInfo.Passport.passportNumber.ToString();
            TxbDateOfBirth.Text = user.PersonalInfo.dateOfBirth.ToShortDateString();
            TxbGender.Text = user.PersonalInfo.Gender.genderName.ToString();
            TxbPassportSeries.Text = user.PersonalInfo.Passport.passportSeries.ToString();
            TxbPassportIssuedBy.Text = user.PersonalInfo.Passport.passportIssuedBy.ToString();
            TxbDateOfIssue.Text = user.PersonalInfo.Passport.dateOfIssue.ToShortDateString();
            TxbDivisionCode.Text = user.PersonalInfo.Passport.divisionCode.ToString();
            TxbPlaceOfBirth.Text = user.PersonalInfo.Passport.placeOfBirth.ToString();
            TxbUserRole.Text = "Роль: " + user.UserRole.roleName.ToString();
        }

        /// <summary>
        /// Выход из профиля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLogOut_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите выйти?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            { 
            
            }
            else
            {
                Navigation.frameNav.Navigate(new AuthorizationPage());
                MenuNavigation.frameNav.Navigate(new MenuAuthPage());
                menshakova_publicUtilitiesEntities context = new menshakova_publicUtilitiesEntities();
                var user = context.User.Where(item => item.id == UserId).FirstOrDefault();
                user.userStatusId = 2;
                context.SaveChanges();
            }
        }
        /// <summary>
        /// Переход на страницу смены пароля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            PasswordChangePage.UserId = UserId;
            Navigation.frameNav.Navigate(new PasswordChangePage());
        }
        /// <summary>
        /// Показать/скрыть паспортные данные
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CBShowPassportInfo_Click(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox.IsChecked.Value)
            {
                SPPassportInfo.Height = 430;
                SPPassportInfo.Visibility = Visibility.Visible;
                ImageId.Visibility = Visibility.Hidden;
                ImageId.Height = 0;
                ImageId.Width = 0;
                CBShowPassportInfo.ToolTip = "Скрыть";
            }
            else
            {
                SPPassportInfo.Height = 20;
                SPPassportInfo.Visibility = Visibility.Hidden;
                ImageId.Visibility = Visibility.Visible;
                ImageId.Height = 250;
                ImageId.Width = 250;
                CBShowPassportInfo.ToolTip = "Показать";
            }
        }
    }
}
