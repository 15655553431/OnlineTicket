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
using System.Windows.Shapes;
using System.Windows.Navigation;
using IBll;
using Bll;
using Model.Enum;
using Common.Md5;


namespace TicketCheck
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
        

        public Login()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            this.LoginName.Text = "";
            this.PassWord.Password = "";
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            string login = this.LoginName.Text;
            string pwd = this.PassWord.Password;
            if (login.Length == 0 || pwd.Length == 0)
            {
                MessageBox.Show("账号或密码填写有误！", "提示");
                return;
            }

            IUserInfoService userinfoService = new UserInfoService();
            pwd = pwd.GetMd5();

            if (userinfoService.GetEntities(u => u.Login == login && u.Pwd == pwd && u.Type == (int)UiTypeEnum.Teller).Count() == 1)
            {
                MainWindow wd = new MainWindow();
                wd.Show();

                //关闭当前窗口
                this.Close();

            }
            else
            {
                MessageBox.Show("账号或密码填写有误！", "提示");
                //隐藏当前窗口
                //this.ShowInTaskbar = false;
                //this.Visibility = Visibility.Hidden;
                
                
                //NavigationWindow window = new NavigationWindow();
                //window.Source = new Uri("MainWindow.xaml", UriKind.Relative);

                //window.ShowDialog();
            }
            return;
        }
    }
}
