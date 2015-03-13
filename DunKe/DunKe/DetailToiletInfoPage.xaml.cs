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
    public partial class DetailToiletInfoPage : PhoneApplicationPage
    {
        public DetailToiletInfoPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            try
            {
                string toilet = this.NavigationContext.QueryString["id"];
                MessageBox.Show("该厕所的id号为："+toilet);
            }
            catch (Exception)
            {

                //throw;
            }
           
        }
    }
}