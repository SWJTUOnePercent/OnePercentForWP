﻿#pragma checksum "C:\Users\久鹏\Desktop\Web Service问题 ———替代解决方案\PhoneApp2\PhoneApp1\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "0F986E924026BD65F6286CE16F70C7C8"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.34014
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace PhoneApp1 {
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.StackPanel fruitStackPanel;
        
        internal System.Windows.Controls.TextBlock price;
        
        internal System.Windows.Controls.TextBox textBox_select;
        
        internal System.Windows.Controls.Button button_getPrice;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/PhoneApp1;component/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.fruitStackPanel = ((System.Windows.Controls.StackPanel)(this.FindName("fruitStackPanel")));
            this.price = ((System.Windows.Controls.TextBlock)(this.FindName("price")));
            this.textBox_select = ((System.Windows.Controls.TextBox)(this.FindName("textBox_select")));
            this.button_getPrice = ((System.Windows.Controls.Button)(this.FindName("button_getPrice")));
        }
    }
}

