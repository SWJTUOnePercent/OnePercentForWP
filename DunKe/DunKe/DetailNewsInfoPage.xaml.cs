using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace DunKe
{
    public partial class DetailNewsInfoPage : PhoneApplicationPage
    {
        public DetailNewsInfoPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            try
            {
                string newsId = this.NavigationContext.QueryString["id"];
                MessageBox.Show("该新闻的id号为：" + newsId);
            }
            catch (Exception)
            {

                //throw;
            }

        }
    }
}