using Bll;
using IBll;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
using Common.Lambda;
using Model.Enum;

namespace TicketCheck
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void SreachBtn_Click(object sender, RoutedEventArgs e)
        {
            ITicketinfoService tiService = new TicketinfoService();
            string guid = this.GUID.Text;
            string idcard = this.IdCard.Text;
            string origin = this.Origin.Text;
            string destination = this.Destination.Text;
            string number = this.Number.Text;
            string seatnumber = this.SeatNumber.Text;

            Expression<Func<Ticketinfo, bool>> lambda = u => u.Status == (int)TiStatusEnum.Done && u.IdCard.Contains(idcard) && u.Origin.Contains(origin) && u.Destination.Contains(destination) && u.Number.Contains(number);

            if (guid != "")
            {
                lambda = lambda.And(u => u.GUID == guid);
            }
            if (seatnumber != "")
            {
                try
                {
                    int seat = Convert.ToInt32(seatnumber);
                    lambda = lambda.And(u => u.SeatNumber == seat);
                }
                catch
                {
                    this.SeatNumber.Text = "";
                }
                
            }

            List<Ticketinfo> tilist = tiService.GetEntities(lambda).Take(50).ToList();
            this.TicketData.IsReadOnly = true;
            this.TicketData.ItemsSource = tilist;
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            this.GUID.Text = "";
            this.IdCard.Text = "";
            this.Origin.Text = "";
            this.Destination.Text = "";
            this.Number.Text = "";
            this.SeatNumber.Text = "";
            SreachBtn_Click(null, null);
        }

    }
}
