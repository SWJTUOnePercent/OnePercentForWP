﻿#pragma checksum "F:\程序及项目\WP8.0\OnePercent 1.0\OnePercentForWP\DunKe\DunKe\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "65B6EB371C5400D745BB1BA97AE806E5"
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


namespace DunKe {
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Media.Animation.Storyboard locationStoryboard;
        
        internal System.Windows.Media.Animation.Storyboard switchMapViewStoryboard;
        
        internal System.Windows.Media.Animation.Storyboard mapViewModelStackPanelStoryboard2;
        
        internal System.Windows.Media.Animation.Storyboard mapViewModelStackPanelStoryboard1;
        
        internal System.Windows.Media.Animation.Storyboard loadingMapStoryboard;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.StackPanel mapViewModelStackPanel;
        
        internal System.Windows.Controls.Grid mapRoadViewGrid;
        
        internal System.Windows.Shapes.Rectangle mapRoadViewRectangle;
        
        internal System.Windows.Controls.Grid mapHybridViewGrid;
        
        internal System.Windows.Shapes.Rectangle mapHybridViewRectangle;
        
        internal System.Windows.Controls.Image location;
        
        internal System.Windows.Controls.Image switchMapView;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/DunKe;component/MainPage.xaml", System.UriKind.Relative));
            this.locationStoryboard = ((System.Windows.Media.Animation.Storyboard)(this.FindName("locationStoryboard")));
            this.switchMapViewStoryboard = ((System.Windows.Media.Animation.Storyboard)(this.FindName("switchMapViewStoryboard")));
            this.mapViewModelStackPanelStoryboard2 = ((System.Windows.Media.Animation.Storyboard)(this.FindName("mapViewModelStackPanelStoryboard2")));
            this.mapViewModelStackPanelStoryboard1 = ((System.Windows.Media.Animation.Storyboard)(this.FindName("mapViewModelStackPanelStoryboard1")));
            this.loadingMapStoryboard = ((System.Windows.Media.Animation.Storyboard)(this.FindName("loadingMapStoryboard")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.mapViewModelStackPanel = ((System.Windows.Controls.StackPanel)(this.FindName("mapViewModelStackPanel")));
            this.mapRoadViewGrid = ((System.Windows.Controls.Grid)(this.FindName("mapRoadViewGrid")));
            this.mapRoadViewRectangle = ((System.Windows.Shapes.Rectangle)(this.FindName("mapRoadViewRectangle")));
            this.mapHybridViewGrid = ((System.Windows.Controls.Grid)(this.FindName("mapHybridViewGrid")));
            this.mapHybridViewRectangle = ((System.Windows.Shapes.Rectangle)(this.FindName("mapHybridViewRectangle")));
            this.location = ((System.Windows.Controls.Image)(this.FindName("location")));
            this.switchMapView = ((System.Windows.Controls.Image)(this.FindName("switchMapView")));
        }
    }
}

