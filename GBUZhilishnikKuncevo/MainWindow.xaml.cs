using GBUZhilishnikKuncevo.Classes;
using GBUZhilishnikKuncevo.Models;
using GBUZhilishnikKuncevo.Pages;
using GBUZhilishnikKuncevo.Pages.AuthPages;
using GBUZhilishnikKuncevo.Pages.MenuPages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
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

namespace GBUZhilishnikKuncevo
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static int UserId;

        public MainWindow()
        {
            InitializeComponent();
            Navigation.frameNav = MainFrame;
            MainFrame.Navigate(new AuthorizationPage());
            MenuNavigation.frameNav = MenuFrame;
            MenuFrame.Navigate(new MenuAuthPage());
        }

        /// <summary>
        /// Делает пользователя инактивным при закрытии приложения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //AuthorizationPage authorizationPage = new AuthorizationPage();
            int userId = UserId;
            try
            {
                if (userId == 0)
                {

                }
                else
                {
                    menshakova_publicUtilitiesEntities context = new menshakova_publicUtilitiesEntities();
                    var user = context.User.Where(item => item.id == userId).FirstOrDefault();
                    user.userStatusId = 2;
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
