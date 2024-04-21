using GBUZhilishnikKuncevo.Classes;
using GBUZhilishnikKuncevo.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
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

namespace GBUZhilishnikKuncevo.Pages.Resources
{
    /// <summary>
    /// Логика взаимодействия для TotalCheckAddPage.xaml
    /// </summary>
    public partial class TotalCheckAddPage : Page
    {
        public TotalCheckAddPage()
        {
            InitializeComponent();

            ///Наполняем поле выбора из БД
            CmbBankBook.DisplayMemberPath = "bankBookNumber";
            CmbBankBook.SelectedValuePath = "id";
            CmbBankBook.ItemsSource = DBConnection.DBConnect.BankBook.ToList();
            ///Наполняем поле выбора из БД
            CmbBenefit.DisplayMemberPath = "benefitName";
            CmbBenefit.SelectedValuePath = "id";
            CmbBenefit.ItemsSource = DBConnection.DBConnect.Benefit.ToList();
            ///Наполняем поле выбора из БД
            CmbPaymentState.DisplayMemberPath = "paymentStateName";
            CmbPaymentState.SelectedValuePath = "id";
            CmbPaymentState.ItemsSource = DBConnection.DBConnect.PaymentState.ToList();
        }

        /// <summary>
        /// Добавление чека в базу данных, подсчёт итоговой суммы к оплате за месяц, подсчёт пени в случае просрочки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (CmbBankBook.Text == "" || CmbPaymentState.Text == "" ||
                DPPaymentDate.Text == "" || DPRequiredPaymentDate.Text == "")
            {
                MessageBox.Show("Нужно заполнить все поля!",
                    "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                if (MessageBox.Show("Вы точно хотите добавить чек?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {

                }
                else
                {
                    try
                    {
                        //var serviceAccountingCheck = decimal.Parse(TxbCounterReading.Text);

                        if (CmbPaymentState.Text == "Оплачено несвоевременно")
                        {
                            var serviceCheckList = DBConnection.DBConnect.ServiceCheck.ToList();

                            //Определяем дни просрочки
                            var daysOfDelay = (DateTime.Parse(DPPaymentDate.Text) - DateTime.Parse(DPRequiredPaymentDate.Text)).Days;
                            //Ищем по периоду и лицевому счёту нужные квитанции по услугам, чтобы сформировать их в чек
                            var allService = serviceCheckList.Where(item => item.Accounting.BankBook.bankBookNumber.Contains(CmbBankBook.Text));
                            var serviceByDate = allService.Where(item => item.Accounting.accountingEnd == DateTime.Parse(DPRequiredPaymentDate.Text)).ToList();
                            var serviceTotalPayble = serviceByDate.Select(item => item.totalPayble).ToList();
                            decimal totalPaybleCheck = 0;
                            //Считаем итого к оплате
                            for (int i = 0; i < serviceTotalPayble.Count; i++)
                            {
                                totalPaybleCheck = (decimal)(totalPaybleCheck + serviceTotalPayble[i]);
                            }

                            //Считаем скидку
                            decimal benefitDiscount = 1;
                            if (CmbBenefit.SelectedItem as Benefit != null) {
                                benefitDiscount = (decimal)(CmbBenefit.SelectedItem as Benefit).discount/100;
                            }
                            
                            //Формируем новую запись
                            TotalCheck totalCheck = new TotalCheck()
                            {
                                BankBook = CmbBankBook.SelectedItem as BankBook,
                                Benefit = CmbBenefit.SelectedItem as Benefit,
                                PaymentState = CmbPaymentState.SelectedItem as PaymentState,
                                requiredPaymentDate = DateTime.Parse(DPRequiredPaymentDate.Text),
                                paymentDate = DateTime.Parse(DPPaymentDate.Text),
                                totalPayble = totalPaybleCheck * benefitDiscount,
                                //Считаем пени
                                fine = ((daysOfDelay * totalPaybleCheck) / 300000)
                            };
                            //Добавляем данные в БД
                            DBConnection.DBConnect.TotalCheck.Add(totalCheck);
                            DBConnection.DBConnect.SaveChanges();
                            MessageBox.Show("Чек успешно добавлен!",
                                "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                            Navigation.frameNav.GoBack();
                        }
                        else 
                            {
                            //По аналогии выше
                            var serviceCheckList = DBConnection.DBConnect.ServiceCheck.ToList();

                            var allService = serviceCheckList.Where(item => item.Accounting.BankBook.bankBookNumber.Contains(CmbBankBook.Text)).ToList();
                            var serviceByDate = allService.Where(item => item.Accounting.accountingEnd == DateTime.Parse(DPPaymentDate.Text)).ToList();
                            var serviceTotalPayble = serviceByDate.Select(item => item.totalPayble).ToList();
                            decimal totalPaybleCheck = 0;

                            for (int i = 0; i < serviceTotalPayble.Count; i++)
                            {
                                totalPaybleCheck = (decimal)(totalPaybleCheck + serviceTotalPayble[i]);
                            }

                            TotalCheck totalCheck = new TotalCheck()
                            {
                                BankBook = CmbBankBook.SelectedItem as BankBook,
                                Benefit = CmbBenefit.SelectedItem as Benefit,
                                PaymentState = CmbPaymentState.SelectedItem as PaymentState,
                                requiredPaymentDate = DateTime.Parse(DPRequiredPaymentDate.Text),
                                paymentDate = DateTime.Parse(DPPaymentDate.Text),
                                totalPayble = totalPaybleCheck
                            };

                            DBConnection.DBConnect.TotalCheck.Add(totalCheck);
                            DBConnection.DBConnect.SaveChanges();
                            MessageBox.Show("Чек успешно добавлен!",
                                "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                            Navigation.frameNav.GoBack();
                        }
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
