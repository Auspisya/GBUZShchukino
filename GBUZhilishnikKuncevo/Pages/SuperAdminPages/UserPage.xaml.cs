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
    /// Логика взаимодействия для UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        public UserPage()
        {
            InitializeComponent();
            UserData.ItemsSource = null;
            UserData.ItemsSource = DBConnection.DBConnect.User.ToList();
        }

        /// <summary>
        /// Убирает подсказку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            //Убираем подпись
            TxbSearch.Text = "";
        }

        /// <summary>
        /// Поиск по введённому запросу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TxbSearch.Text != "")
                {
                    string searchString = TxbSearch.Text.ToLower();
                    var itemsList = DBConnection.DBConnect.User.ToList();
                    var searchResults = itemsList.Where(item => item.login.ToLower().Contains(searchString)).ToList();
                    UserData.ItemsSource = searchResults.ToList();
                }
                else
                {
                    UserData.ItemsSource = DBConnection.DBConnect.User.ToList();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Непредвиденная ошибка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Обновляет таблицу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            UserData.ItemsSource = null;
            UserData.ItemsSource = DBConnection.DBConnect.User.ToList();
        }
        /// <summary>
        /// Переход на страницу добавления пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Navigation.frameNav.Navigate(new UserAddPage());
        }

        private void BtnSessionPage_Click(object sender, RoutedEventArgs e)
        {
            Navigation.frameNav.Navigate(new SessionPage());
        }
        /// <summary>
        /// Просмотр полной информации по пользователю
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnShowInfo_Click(object sender, RoutedEventArgs e)
        {
            Navigation.frameNav.Navigate(new UserInfoPage((sender as Button).DataContext as User));
        }
        /// <summary>
        /// Переход на страниу редактирования пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEditInfo_Click(object sender, RoutedEventArgs e)
        {
            Navigation.frameNav.Navigate(new UserEditPage((sender as Button).DataContext as User));
        }
        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите удалить данные?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {

            }
            else
            {
                try
                {
                    for (int i = 0; i < UserData.SelectedItems.Count; i++)
                    {
                        User user = UserData.SelectedItems[i] as User;
                        DBConnection.DBConnect.User.Remove(user);
                    }

                    DBConnection.DBConnect.SaveChanges();
                    MessageBox.Show("Данные успешно удалены!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information
                        );
                    UserData.ItemsSource = null;
                    UserData.ItemsSource = DBConnection.DBConnect.User.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Критическая обработка");
                }
            }
        }
        /// <summary>
        /// Заблокировать/разблокировать пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBlock_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите заблокировать/разблокировать?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {

            }
            else
            {
                try
                {
                    for (int i = 0; i < UserData.SelectedItems.Count; i++)
                    {
                        User user = UserData.SelectedItems[i] as User;
                        user.userStatusId = user.userStatusId == 1 ? 3 : 5 - user.userStatusId;
                    }

                    DBConnection.DBConnect.SaveChanges();
                    MessageBox.Show("Успешно.", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information
                        );
                    UserData.ItemsSource = null;
                    UserData.ItemsSource = DBConnection.DBConnect.User.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Критическая обработка");
                }
            }
        }
        /// <summary>
        /// Инициализация кнопочек в таблице - гениально
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBlock_Initialized(object sender, EventArgs e)
        {
            var row = GetParent<DataGridRow>((Button)sender);
            var index = UserData.Items.IndexOf(row.Item);

            if ((UserData.Items[index] as User).userStatusId != 3)
            {
                (sender as Button).Content = "Заблокировать";
                (sender as Button).Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                (sender as Button).Content = "Разблокировать";
                (sender as Button).Foreground = new SolidColorBrush(Colors.Green);
            }
        }

        /// <summary>
        /// Без него не работает - поиск родителей у кнопки-сиротки :(((
        /// </summary>
        /// <typeparam name="TargetType"></typeparam>
        /// <param name="o"></param>
        /// <returns></returns>
        private TargetType GetParent<TargetType>(DependencyObject o)
            where TargetType : DependencyObject
        {
            if (o == null || o is TargetType) return (TargetType)o;
            return GetParent<TargetType>(VisualTreeHelper.GetParent(o));
        }
        /// <summary>
        /// Переход на предыдущую страницу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Navigation.frameNav.GoBack();
        }
    }
}
