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
    /// Логика взаимодействия для BankBookInfoPage.xaml
    /// </summary>
    public partial class BankBookInfoPage : Page
    {
        public BankBookInfoPage(BankBook bankBook)
        {
            InitializeComponent();
            #region Заполнение текстовых блоков информацией из БД
            TxbBankBookNumber.Text = "Номер лицевого счёта: " + "[" + bankBook.bankBookNumber.ToString() + "]";
            if (bankBook.Apartment.Address.buildingCorpse == "")
            {
                TxbAddress.Text = $"{bankBook.Apartment.Address.city}, {bankBook.Apartment.Address.area}, {bankBook.Apartment.Address.street}, " +
                    $"{bankBook.Apartment.Address.buildingNumber}, подъезд {bankBook.Apartment.Address.entranceNumber}, " +
                    $"этаж {bankBook.Apartment.Address.floorNumber}, кв. {bankBook.Apartment.Address.apartmentNumber}";
            }
            else
            {
                TxbAddress.Text = $"{bankBook.Apartment.Address.city}, {bankBook.Apartment.Address.area}, {bankBook.Apartment.Address.street}, " +
                        $"{bankBook.Apartment.Address.buildingNumber}, корпус {bankBook.Apartment.Address.buildingCorpse}, подъезд {bankBook.Apartment.Address.entranceNumber}, " +
                        $"этаж {bankBook.Apartment.Address.floorNumber}, кв. {bankBook.Apartment.Address.apartmentNumber}";
            }
            TxbApartmentArea.Text = bankBook.Apartment.apartmentArea.ToString();
            TxbClient.Text = bankBook.Client.PersonalInfo1.fullName.ToString();
            TxbNumberOfResidents.Text = bankBook.Apartment.numberOfResidents.ToString();
            TxbOrganization.Text = bankBook.organization.ToString();
            TxbOwnership.Text = bankBook.Ownership.typeName.ToString();
            TxbProprietary.Text = bankBook.Proprietary.typeName.ToString();
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
    }
}
