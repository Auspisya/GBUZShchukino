using GBUZhilishnikKuncevo.Classes;
using GBUZhilishnikKuncevo.Models;
using GBUZhilishnikKuncevo.Pages.AdminPages;
using GBUZhilishnikKuncevo.Pages.MenuPages;
using GBUZhilishnikKuncevo.Pages.SuperAdminPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

namespace GBUZhilishnikKuncevo.Pages.AuthPages
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : Page
    {
        string password;
        public AuthorizationPage()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Выполнить вход в систему
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string url = $"http://localhost/sign-in?login={TxbLogin.Text}&password={password}";
                HttpClient client = new HttpClient();
                var response = client.GetAsync(url).Result;
                var responseContent = await response.Content.ReadAsStringAsync();
                //var responseContent = "{\"status\":true, \"description\":\"Авторизация прошла успешно!\",\"RoleUser\":\"SuperAdmin\",\"IdUser\":1}";
                if (TxbLogin.Text == "" || password == "")
                {
                    MessageBox.Show("Заполните все поля!");
                }
                else
                {
                    if (/* true */ response.StatusCode == System.Net.HttpStatusCode.OK )
                    {
                        var _signIn = JsonConvert.DeserializeObject<SignIn>(responseContent);
                        int userId = _signIn.IdUser; // Да, это глупо, но чтобы наверняка
                        //var MainWindow = new MainWindow();
                        MainWindow.UserId = userId; // Так вот он какой static
                        AdminProfilePage.UserId = userId;
                        SuperAdminProfilePage.UserId = userId;
                        MenuPage.Role = _signIn.RoleUser;
                        PasswordChangePage.UserId = userId;

                        menshakova_publicUtilitiesEntities context = new menshakova_publicUtilitiesEntities();
                        var user = context.User.Where(item => item.id == userId).FirstOrDefault();
                        if (user.passwordLastChanged == user.registrationDate || (DateTime.Now - user.passwordLastChanged).Days >= 30)
                        {
                            MessageBox.Show("Пароль устарел");
                            Navigation.frameNav.Navigate(new PasswordChangePage());
                            MenuNavigation.frameNav.Navigate(new MenuAuthPage());
                        }
                        else 
                        { 
                            switch (_signIn.RoleUser)
                            {
                                case "Admin":
                                    MessageBox.Show("Авторизация прошла успешно!");
                                    Navigation.frameNav.Navigate(new WelcomePage());
                                    MenuNavigation.frameNav.Navigate(new MenuPage());
                                    break;
                                case "SuperAdmin":
                                    MessageBox.Show("Авторизация прошла успешно!");
                                    Navigation.frameNav.Navigate(new WelcomePage());
                                    MenuNavigation.frameNav.Navigate(new MenuPage());
                                    break;
                                default:
                                    MessageBox.Show("Неверная обработка данных");
                                    break;
                            }
                        }
                    }
                    else
                    {
                        string descp = JObject.Parse(responseContent).SelectToken("description").ToString();
                        MessageBox.Show(descp);
                        //MessageBox.Show(JsonConvert.DeserializeObject(responseContent).ToString());
                    }
                }
            }
            catch (Exception er)
            {
                er.Message.ToString();
            }
        }

        /// <summary>
        /// Показать/скрыть пароль
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CBShowHidePassword_Click(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox.IsChecked.Value)
            {
                TxbPassword.Text = PsbPassword.Password;
                TxbPassword.Visibility = Visibility.Visible;
                PsbPassword.Visibility = Visibility.Hidden;
                CBShowHidePassword.ToolTip = "Скрыть";
            }
            else
            {
                PsbPassword.Password = TxbPassword.Text; 
                TxbPassword.Visibility = Visibility.Hidden; 
                PsbPassword.Visibility = Visibility.Visible;
                CBShowHidePassword.ToolTip = "Показать";
            }
        }
        /// <summary>
        /// Обновление пароля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PsbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            password = PsbPassword.Password;
        }
        /// <summary>
        /// Обновление пароля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            password = TxbPassword.Text;
        }
    }
}
