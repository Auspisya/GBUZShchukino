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
    /// Логика взаимодействия для BankBookEditPage.xaml
    /// </summary>
    public partial class BankBookEditPage : Page
    {
        private int bankBookId;
        public BankBookEditPage(BankBook bankBook)
        {
            InitializeComponent();

            #region Наполняем элементы управления данными из БД по выбранной записи
            //Наполняем поля выбора из БД
            CmbClient.DisplayMemberPath = "fullName";
            CmbClient.SelectedValuePath = "id";
            CmbClient.ItemsSource = DBConnection.DBConnect.Client.ToList();
            CmbClient.Text = bankBook.Client.PersonalInfo1.fullName.ToString();

            CmbOwnership.DisplayMemberPath = "typeName";
            CmbOwnership.SelectedValuePath = "id";
            CmbOwnership.ItemsSource = DBConnection.DBConnect.Ownership.ToList();
            CmbOwnership.Text = bankBook.Ownership.typeName.ToString();

            CmbProprietary.DisplayMemberPath = "typeName";
            CmbProprietary.SelectedValuePath = "id";
            CmbProprietary.ItemsSource = DBConnection.DBConnect.Proprietary.ToList();
            CmbProprietary.Text = bankBook.Proprietary.typeName.ToString();
            //Наполняем текстовые поля
            TxbApartmentArea.Text = bankBook.Apartment.apartmentArea.ToString();
            TxbApartmentNumber.Text = bankBook.Apartment.Address.apartmentNumber.ToString();
            TxbArea.Text = bankBook.Apartment.Address.area.ToString();
            TxbBankBookNumber.Text = bankBook.bankBookNumber.ToString();

            if (bankBook.Apartment.Address.buildingCorpse == null)
            {
                TxbBuildingCorpse.Text = "";
            }
            else 
            {
                TxbBuildingCorpse.Text = bankBook.Apartment.Address.buildingCorpse.ToString();
            }
            TxbBuildingNumber.Text = bankBook.Apartment.Address.apartmentNumber.ToString();
            TxbCity.Text = bankBook.Apartment.Address.city.ToString();
            TxbEntranceNumber.Text = bankBook.Apartment.Address.entranceNumber.ToString();
            TxbFloorNumber.Text = bankBook.Apartment.Address.floorNumber.ToString();
            TxbNumberOfResidents.Text = bankBook.Apartment.numberOfResidents.ToString();
            TxbOrganization.Text = bankBook.organization.ToString();
            TxbStreet.Text = bankBook.Apartment.Address.street.ToString();
            #endregion
            bankBookId = bankBook.id;
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
        /// Вносит изменённые данные в БД
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (CmbClient.Text == "" || CmbOwnership.Text == "" || CmbProprietary.Text == "" ||
                TxbApartmentArea.Text == "" || TxbApartmentNumber.Text == "" || TxbArea.Text == "" ||
                TxbBankBookNumber.Text == "" || TxbBuildingNumber.Text == "" || TxbCity.Text == "" ||
                TxbEntranceNumber.Text == "" || TxbFloorNumber.Text == "" || TxbNumberOfResidents.Text == "" ||
                TxbOrganization.Text == "" || TxbStreet.Text == "")
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
                    var bankBook = context.BankBook.Where(item => item.id == bankBookId).FirstOrDefault();
                    
                    bankBook.proprietaryId = (CmbProprietary.SelectedItem as Proprietary).id;
                    bankBook.clientId = (CmbClient.SelectedItem as Client).id;
                    bankBook.typeOfOwnershipId = (CmbOwnership.SelectedItem as Ownership).id;
                    bankBook.Apartment.numberOfResidents = int.Parse(TxbNumberOfResidents.Text);
                    bankBook.Apartment.apartmentArea = double.Parse(TxbApartmentArea.Text);
                    bankBook.Apartment.Address.city = TxbCity.Text;
                    bankBook.Apartment.Address.area = TxbArea.Text;
                    bankBook.Apartment.Address.street = TxbStreet.Text;
                    bankBook.Apartment.Address.buildingNumber = TxbBuildingNumber.Text;
                    bankBook.Apartment.Address.buildingCorpse = TxbBuildingCorpse.Text;
                    bankBook.Apartment.Address.entranceNumber = int.Parse(TxbEntranceNumber.Text);
                    bankBook.Apartment.Address.floorNumber = int.Parse(TxbFloorNumber.Text);
                    bankBook.Apartment.Address.apartmentNumber = int.Parse(TxbApartmentNumber.Text);
                    bankBook.bankBookNumber = TxbBankBookNumber.Text;
                    bankBook.organization = TxbOrganization.Text;
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
            string pattern = @"[^0-9+-,.]+";
            if (Regex.IsMatch(e.Text, pattern))
            {
                e.Handled = true;
            }
        }
    }
}
