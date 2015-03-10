using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace GaoDeMapAPI
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
            string toilet = this.NavigationContext.QueryString["id"];
            MessageBox.Show(toilet);
        }
    }
}