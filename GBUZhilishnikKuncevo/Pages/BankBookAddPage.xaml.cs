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
    /// Логика взаимодействия для BankBookAddPage.xaml
    /// </summary>
    public partial class BankBookAddPage : Page
    {
        public BankBookAddPage()
        {
            InitializeComponent();

            #region Наполняем элементы управления данными из БД по выбранной записи
            //Наполняем поля выбора из БД
            CmbClient.DisplayMemberPath = "fullName";
            CmbClient.SelectedValuePath = "id";
            CmbClient.ItemsSource = DBConnection.DBConnect.Client.ToList();

            CmbOwnership.DisplayMemberPath = "typeName";
            CmbOwnership.SelectedValuePath = "id";
            CmbOwnership.ItemsSource = DBConnection.DBConnect.Ownership.ToList();

            CmbProprietary.DisplayMemberPath = "typeName";
            CmbProprietary.SelectedValuePath = "id";
            CmbProprietary.ItemsSource = DBConnection.DBConnect.Proprietary.ToList();
            #endregion
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
        /// Добавление новых экземпляров записей в базу данных
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
                if (MessageBox.Show("Вы точно хотите добавить данные?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {

                }
                else
                {
                    try
                    {
                        Address address = new Address()
                        {
                            apartmentNumber = int.Parse(TxbApartmentNumber.Text),
                            area = TxbArea.Text,
                            buildingCorpse = TxbBuildingCorpse.Text,
                            buildingNumber = TxbBuildingNumber.Text,
                            city = TxbCity.Text,
                            entranceNumber = int.Parse(TxbEntranceNumber.Text),
                            floorNumber = int.Parse(TxbFloorNumber.Text),
                            street = TxbStreet.Text
                        };

                        Apartment apartment = new Apartment()
                        {
                            
                            addressId = address.id,
                            apartmentArea = double.Parse(TxbApartmentArea.Text),
                            numberOfResidents = int.Parse(TxbNumberOfResidents.Text),
                            //Address = address
                        };

                        BankBook bankBook = new BankBook()
                        {
                            apartmentId = apartment.id,
                            bankBookNumber = TxbBankBookNumber.Text,
                            clientId = (CmbClient.SelectedItem as Client).id,
                            organization = TxbOrganization.Text,
                            Ownership = CmbOwnership.SelectedItem as Ownership,
                            Proprietary = CmbProprietary.SelectedItem as Proprietary,
                            //Apartment = apartment
                        };

                        DBConnection.DBConnect.Address.Add(address);
                        DBConnection.DBConnect.Apartment.Add(apartment);
                        DBConnection.DBConnect.BankBook.Add(bankBook);
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
            string pattern = @"[^0-9+-,.]+";
            if (Regex.IsMatch(e.Text, pattern))
            {
                e.Handled = true;
            }
        }
    }
}
