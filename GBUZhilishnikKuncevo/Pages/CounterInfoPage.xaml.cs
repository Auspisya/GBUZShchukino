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
    /// Логика взаимодействия для CounterInfoPage.xaml
    /// </summary>
    public partial class CounterInfoPage : Page
    {
        public CounterInfoPage(Counter counter)
        {
            InitializeComponent();
            //Наполняем текстовые блоки информацией из БД
            TxbCounterNumber.Text = "Номер счётчика: " + "[" + counter.counterNumber + "]";
            TxbApartmentNumber.Text = counter.Apartment.Address.apartmentNumber.ToString();
            TxbArea.Text = counter.Apartment.Address.area.ToString();
            TxbBuildingCorpse.Text = counter.Apartment.Address.buildingCorpse.ToString();
            TxbBuildingNumber.Text = counter.Apartment.Address.buildingNumber.ToString();
            TxbCity.Text = counter.Apartment.Address.city.ToString();
            TxbEntrance.Text = counter.Apartment.Address.entranceNumber.ToString();
            TxbFloor.Text = counter.Apartment.Address.floorNumber.ToString();
            TxbStreet.Text = counter.Apartment.Address.street.ToString();

            var bankBookList = DBConnection.DBConnect.BankBook.ToList();
            var bankBook = bankBookList.Where(item => item.apartmentId == counter.apartmentId).ToList();
            if (bankBook.Count == 0) { TxbBankBook.Text = "Ещё не присвоен"; } else { TxbBankBook.Text = bankBook[0].bankBookNumber; }
            //TxbBankBook.Text = bankBook[0].bankBookNumber;
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
