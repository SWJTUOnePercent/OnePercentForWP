using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
//using GaoDeMapAPI.Resources;
using System.Windows.Media.Animation;
using Com.AMap.Api.Maps;
using Com.AMap.Api.Maps.Model;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Threading;

namespace DunKe
{
    public partial class MainPage : PhoneApplicationPage
    {

        //故事板
        Storyboard story = null;

        //地图类
        AMap amap = new AMap();

        //标注类 用于显示用户位置
        AMapMarker marker_MyLocation;

        //圆 用于定位
        AMapCircle circle;
        AMapGeolocator mylocation;

        //经纬度 纬度：latitude   经度：longitude
        LatLng latLng__MyLocation;

        //地图交互状态类 
        UiSettings uiset;

        //附近的厕所信息列表
        List<Toilet> aroundToilets = null;

        //定义 点击厕所 弹窗的提示窗口
        AInfoWindow infoWindow;
        //标注类 用于显示附近的厕所
        AMapMarker toiletMarker;

        //地图是否完成了加载 
        bool isloadedMap;


        //新闻列表
        List<News> newsList = null;
        //循环显示的 新闻最大条数
        static int newsCycleCount=7;
        //循环显示新闻计数
        int newsIndex;

        // 构造函数
        public MainPage()
        {
            InitializeComponent();

            //地图 还没 完成加载
            isloadedMap = false;

            //添加地图类控件
            this.ContentPanel.Children.Add(amap);

            //添加双击击屏幕事件
            amap.DoubleTap += Amap_DoubleTap;

            //添加加载 和 加载失败事件
            amap.Loaded += MyLocation_Loaded;
            amap.Unloaded += MyLocation_Unloaded;

            //设置高德地图的 缩放控制  不显示
            uiset = amap.GetUiSettings();
            uiset.ZoomControlsEnabled = false;

            //设置高德地图的 指南针  不显示
            uiset.CompassControlEnabled = false;

            //附近的厕所列表
            aroundToilets = new List<Toilet>();
            //31.171184,108.396034
            //29.568386,103.443446
            //29.570858,103.445917
            aroundToilets.Add(new Toilet { Id = 1, Info = "第一个厕所", Latitude = 29.570858, Longitude = 103.446900 });
            aroundToilets.Add(new Toilet { Id = 2, Info = "第二个厕所", Latitude = 29.568500, Longitude = 103.445917 });
            aroundToilets.Add(new Toilet { Id = 2, Info = "第三个厕所", Latitude = 29.568600, Longitude = 103.455917 });
            //添加 标记点击事件
            amap.MarkerClickListener += amap_MarkerClickListener;

            //添加 地图布局改变后 触发的事件 用于显示或是隐藏 指南针
            amap.LayoutUpdated += amap_LayoutUpdated;
           
            // 加载 附近厕所的 ApplicationBar
            aroundToilets_ApplicationBar();

            //显示时间
            this.timeText.Text = DateTime.Now.ToShortTimeString();
            DispatcherTimer timers = new DispatcherTimer();
            timers.Interval = new TimeSpan(0, 0, 1);
            timers.Tick += new EventHandler(timetick);
            timers.Start();



            //添加新闻列表
            //用于滚动显示新闻
            newsList = new List<News>();
            newsList.Add(new News { NewsId = "1", NewsTitle = "独处却不觉孤寂，真是令人喜悦" });
            newsList.Add(new News { NewsId = "2", NewsTitle = "国台办：习近平就两岸关系提出四个坚定不移国平" });
            newsList.Add(new News { NewsId = "3", NewsTitle = "暗里花儿最沉香" });
            newsList.Add(new News { NewsId = "4", NewsTitle = "生活在你深谋远虑时不期而至" });
            newsList.Add(new News { NewsId = "5", NewsTitle = "周强：对错案深感自责(全文)" });
            newsList.Add(new News { NewsId = "6", NewsTitle = "两会期间已落马两虎 18大后剩6省份无老虎" });
            newsList.Add(new News { NewsId = "7", NewsTitle = " 国务院批准设立中国(杭州)跨境电子商务综合试验区" });

            DispatcherTimer newsCycleTimers = new DispatcherTimer();
            newsCycleTimers.Interval = new TimeSpan(0, 0, 3);
            newsCycleTimers.Tick += new EventHandler(newsCycleTimetick);
            newsCycleTimers.Start();

            //下标指示到 第一条新闻
            newsIndex = 0;

            //显示第一条新闻
            //该条新闻的Id
            this.newsOneIdTextBlock.Text = newsList[newsIndex].NewsId;
            //该条新闻的标题
            this.newsOneTextBlock.Text = newsList[newsIndex].NewsTitle;

            //this.newsOneTextBlock.Visibility = Visibility.Collapsed;
            //this.newsTwoTextBlock.Text = newsList[newsIndex].NewsTitle;
            

            // 用于本地化 ApplicationBar 的示例代码
            //BuildLocalizedApplicationBar();
        }

        #region  切换到下一条新闻
        private void newsCycleTimetick(object sender, EventArgs e)
        {
            //播放 NEWS图标翻转 动画 
            story = (Storyboard)this.FindName("newsLogoOverturnStoryboard");
            story.Begin();

            //切换到下一条新闻
            //故事板
            story = (Storyboard)this.FindName("newsOneTextBlockDisappearingStoryboard");
            story.Completed += newsOneTextBlockCycleStory_Completed;
            story.Begin();

            //切换到下一条新闻
            newsIndex++;
            newsIndex = newsIndex % newsCycleCount;
          
        }

        private void newsOneTextBlockCycleStory_Completed(object sender, EventArgs e)
        {
            //显示下一条新闻
            //该条新闻的Id
            this.newsOneIdTextBlock.Text = newsList[newsIndex].NewsId;
            //该条新闻的标题
            this.newsOneTextBlock.Text = newsList[newsIndex].NewsTitle;

            story = (Storyboard)this.FindName("newsOneTextBlockAppearingStoryboard");
            story.Begin();
        }
        #endregion

        #region 显示时间
        //每秒刷新显示时间
        private void timetick(object sender, EventArgs e)
        {
            //更新时间
            this.timeText.Text = DateTime.Now.ToShortTimeString();
        }
        #endregion


        #region 地图布局更新事件
        //用于 显示 或是 隐藏 指南针
        private void amap_LayoutUpdated(object sender, EventArgs e)
        {
            if (isloadedMap)
            {
                //判断 地图的 旋转角度 
                //如果不为 0度 则显示指南针 
                //否则 不显示指南针 
                if (!amap.Bearing.ToString().Equals("0"))
                {
                    //设置高德地图的 指南针 显示
                    uiset.CompassControlEnabled = true;
                }
                else
                { 
                    //设置高德地图的 指南针 不显示
                    uiset.CompassControlEnabled = false;   
                }
            }
          
        }
        #endregion


        #region 点击 厕所Marker 事件
        //弹出窗口
        //用于显示厕所的简历信息
        private void amap_MarkerClickListener(AMapMarker sender, AMapEventArgs args)
        {
            if (!sender.Equals(marker_MyLocation))
            {
                //加载厕所详细信息的 ApplicationBar
                detailToiletInfo_ApplicationBar();
                if (string.IsNullOrWhiteSpace(sender.Snippet) && string.IsNullOrWhiteSpace(sender.Title))
                    return;
                toiletMarker = sender;
                sender.ShowInfoWindow(infoWindow = new AInfoWindow()
                {
                    Title = sender.Title,
                    ContentText = sender.Snippet,
                });
            }

        }
        #endregion


        #region 加载地图
        private void MyLocation_Loaded(object sender, RoutedEventArgs e)
        {
            //29.568386,103.443446
            amap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(29.568386, 103.443446), 15));
            mylocation = new AMapGeolocator();
            mylocation.Start();
            //触发位置改变事件
            mylocation.PositionChanged += mylocation_PositionChanged;

            this.Dispatcher.BeginInvoke(() =>
            {
                foreach (Toilet toilet in aroundToilets)
                {
                    AMapMarker amapMarker = amap.AddMarker(new AMapMarkerOptions()
                    {
                        Position = new LatLng(toilet.Latitude, toilet.Longitude),//图标的位置
                        Title = toilet.Info,
                        IconUri = new Uri("Assets/Images/AZURE.png", UriKind.Relative),//图标的URL
                        Anchor = new Point(0.5, 0.5),//图标中心点
                        //IsDragable = true,//是否允许拖拽
                    });
                }

                //aMapMarker1 = amap.AddMarker(new AMapMarkerOptions()
                //{
                //    Position = new LatLng(39.90403, 116.407525),//图标的位置
                //    Title = "可拖拽点",
                //    IconUri = new Uri("Images/AZURE.png", UriKind.Relative),//图标的URL
                //    Anchor = new Point(0.5, 1),//图标中心点
                //    IsDragable = true,//是否允许拖拽
                //});
                //aMapMarker2 = amap.AddMarker(new AMapMarkerOptions()
                //{
                //    Position = new LatLng(34.7466, 113.625367),//图标的位置
                //    IconUri = new Uri("Images/GREEN.png", UriKind.Relative),//图标的URL
                //    Anchor = new Point(0.5, 1),//图标中心点
                //    Title = "点",
                //    RotateAngle = 90,//旋转角度
                //});
                //aMapMarker3 = amap.AddMarker(new AMapMarkerOptions()
                //{
                //    Position = new LatLng(31.238068, 121.501654),//图标的位置
                //    Title = "上海",
                //    Snippet = "31.238068, 121.501654",
                //    IconUri = new Uri("Images/RED.png", UriKind.Relative),//图标的URL
                //    Anchor = new Point(0.5, 1),//图标中心点
                //});

                //动画播放多图Marker
                //aMapMarker4 = amap.AddMarker(new AMapMarkerOptions()
                //{
                //    Position = new LatLng(30.679879, 104.064855),
                //    Anchor = new Point(0.5, 1),//图标中心点
                //    Title = "成都",
                //    Snippet = "30.679879, 104.064855",
                //    IconUris = new List<Uri> { 
                //        new Uri("Images/AZURE.png", UriKind.Relative),
                //        new Uri("Images/RED.png", UriKind.Relative), 
                //        new Uri("Images/ROSE.png", UriKind.Relative), 
                //        new Uri("Images/BLUE.png", UriKind.Relative), 
                //        new Uri("Images/CYAN.png", UriKind.Relative), 
                //        new Uri("Images/GREEN.png", UriKind.Relative), 
                //        new Uri("Images/MAGENTAV.png", UriKind.Relative), 
                //        new Uri("Images/ORANGE.png", UriKind.Relative), 
                //        new Uri("Images/VIOLET.png", UriKind.Relative), 
                //        new Uri("Images/YELLOW.png", UriKind.Relative), 
                //    },
                //    Periods = 500,//刷新周期，单位毫秒
                //});

            });

            //地图 已经 完成加载
            isloadedMap = true;
        }
        private void MyLocation_Unloaded(object sender, RoutedEventArgs e)
        {
            if (mylocation != null)
            {
                try
                {
                    mylocation.PositionChanged -= mylocation_PositionChanged;
                    mylocation.Stop();

                    //地图 没有 完成加载
                    isloadedMap = false;
                }
                catch (Exception e1)
                {
                    Debug.WriteLine(e1.ToString());
                }
              
            }
        }
        #endregion


        #region  双击击屏幕事件
        /// <summary>
        /// 双击击屏幕事件 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Amap_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //标尺级别 加1
            //高德地图的BUG 
            //如果双击屏幕标尺 级别不会自动 更新 
            //必须手动添加一个 双击事件 
            //然后在双击事件里面使标尺的级别加1
            amap.Zoom++;
        }
        #endregion


        #region 定位更新时的回调函数
        /// <summary>
        /// 定位更新时的回调函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void mylocation_PositionChanged(AMapGeolocator sender, AMapPositionChangedEventArgs args)
        {
            latLng__MyLocation = args.LngLat;
            //设置当前地图的经纬度和缩放级别
            amap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(args.LngLat, (float)amap.Zoom));
            Debug.WriteLine("定位精度：" + args.Accuracy + "米");
            Debug.WriteLine("定位经纬度：" + args.LngLat);
            this.Dispatcher.BeginInvoke(() =>
            {
                if (marker_MyLocation == null)
                {
                    //添加圆
                    circle = amap.AddCircle(new AMapCircleOptions()
                    {
                        Center = args.LngLat,//圆点位置
                        Radius = (float)args.Accuracy,//半径
                        FillColor = Color.FromArgb(80, 100, 150, 255),
                        StrokeWidth = 2,//边框粗细
                        StrokeColor = Color.FromArgb(80, 0, 0, 255),//边框颜色

                    });

                    //添加点标注，用于标注地图上的点
                    marker_MyLocation = amap.AddMarker(
                    new AMapMarkerOptions()
                    {
                        Position = args.LngLat,//图标的位置
                        Title = "我的位置",
                        Snippet = args.LngLat.ToString(),
                        IconUri = new Uri("Assets/Images/marker_gps_no_sharing.png", UriKind.Relative),//图标的URL
                        Anchor = new Point(0.5, 0.5),//图标中心点
                        RotateAngle = amap.Bearing,
                    });
                }
                else
                {
                    //点标注和圆的位置在当前经纬度
                    marker_MyLocation.Position = args.LngLat;
                    circle.Center = args.LngLat;
                    circle.Radius = (float)args.Accuracy;//圆半径
                }
            });
        }
        #endregion


        #region 点击定位图标 进行定位
        /// <summary>
        /// 定位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void location_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            story = (Storyboard)this.FindName("locationStoryboard");
            story.Begin();

            if (latLng__MyLocation != null)
            {
                //以动画效果的方式移动地图 到地位的点
                amap.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(latLng__MyLocation, (float)amap.Zoom), 1);

                //获取或者设置地图旋转角度,以角度为单位，正北方向到地图方向逆时针旋转的角度，范围为：[0.f, 360.f]
                amap.Bearing = 0;

                Debug.WriteLine("最大缩放级别：" + amap.MaxZoomLevel);
            }
            //else
            //{
            //    mylocation = new AMapGeolocator();
            //    mylocation.Start();
            //    //触发位置改变事件
            //    mylocation.PositionChanged += mylocation_PositionChanged;
            //}
        }
        #endregion


        #region 设置地图显示模式
        //设置地图 显示模式
        //高德地图 Windows Phone SDK 提供两种地图类型（默认为标准地图）：AMapType.Road 和 AMapType.Aerial。
        //AMapType.Road 为标准地图；AMapType.Aerial为卫星地图
        private void switchMapView_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //播放 选择地图模式 动画
            story = (Storyboard)this.FindName("switchMapViewStoryboard");
            story.Begin();
            this.mapViewModelStackPanel.Visibility = Visibility.Visible;
            story = (Storyboard)this.FindName("mapViewModelStackPanelStoryboard1");
            story.Begin();
        }


        private void mapRoadViewGrid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.mapRoadViewRectangle.Opacity = 1.0;
            this.mapHybridViewRectangle.Opacity = 0.2;

            this.mapViewModelStackPanel.Visibility = Visibility.Visible;
            story = (Storyboard)this.FindName("mapViewModelStackPanelStoryboard2");
            story.Begin();

            //设置为街道模式
            amap.MapType = AMap.AMapType.Road;
            // this.map.CartographicMode = Microsoft.Phone.Maps.Controls.MapCartographicMode.Road;
        }

        private void mapHybridViewGrid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.mapRoadViewRectangle.Opacity = 0.2;
            this.mapHybridViewRectangle.Opacity = 1.0;

            this.mapViewModelStackPanel.Visibility = Visibility.Visible;
            story = (Storyboard)this.FindName("mapViewModelStackPanelStoryboard2");
            story.Begin();

            //设置为卫星模式
            amap.MapType = AMap.AMapType.Aerial;
            //this.map.CartographicMode = Microsoft.Phone.Maps.Controls.MapCartographicMode.Hybrid;
        }
        #endregion

        #region 附近厕所的 ApplicationBar
        /// <summary>
        /// 加载 附近厕所的 ApplicationBar
        /// </summary>
        private void aroundToilets_ApplicationBar()
        {
            // 将页面的 ApplicationBar 设置为 ApplicationBar 的新实例。
            ApplicationBar = new ApplicationBar();
            ApplicationBar.Opacity = 0.5;
            // 创建新按钮并将文本值设置为 AppResources 中的本地化字符串。
            ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/refresh.png", UriKind.Relative));
            appBarButton.Text = "刷新";
            appBarButton.Click += RefreshButton_Click;
            ApplicationBar.Buttons.Add(appBarButton);

            appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/add.png", UriKind.Relative));
            appBarButton.Text = "添加";
            appBarButton.Click += AddButton_Click;
            ApplicationBar.Buttons.Add(appBarButton);

            // 使用 AppResources 中的本地化字符串创建新菜单项。
            ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem("设置");
            ApplicationBar.MenuItems.Add(appBarMenuItem);
        }

        /// <summary>
        /// 刷新附近的厕所列表信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("刷新");
        }

        /// <summary>
        /// 添加新的厕所信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("添加");
        }
        #endregion

        #region 厕所详细信息的 ApplicationBar
        private void detailToiletInfo_ApplicationBar()
        {
            // 将页面的 ApplicationBar 设置为 ApplicationBar 的新实例。
            ApplicationBar = new ApplicationBar();

            // 创建新按钮并将文本值设置为 AppResources 中的本地化字符串。
            ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/questionmark.png", UriKind.Relative));
            appBarButton.Text = "详情";
            appBarButton.Click += DetailToiletInfo_Click;
            ApplicationBar.Buttons.Add(appBarButton);

            appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/close.png", UriKind.Relative));
            appBarButton.Text = "关闭";
            appBarButton.Click += CloseAInfoWindow_Click;
            ApplicationBar.Buttons.Add(appBarButton);
        }

        /// <summary>
        /// 跳转到厕所详细页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DetailToiletInfo_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/DetailToiletInfoPage.xaml?id=" + "1", UriKind.Relative));
            //MessageBox.Show("详情");
        }

        /// <summary>
        /// 关闭 AInfoWindow 窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseAInfoWindow_Click(object sender, EventArgs e)
        {
            //关闭 AInfoWindow 窗口
            toiletMarker.HideInfoWindow();

            // 加载 附近厕所的 ApplicationBar
            aroundToilets_ApplicationBar();
        }

        #endregion

        #region NEWS图标的操作
        private void newsGrid_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            //播放 NEWS图标 动画
            story = (Storyboard)this.FindName("newsGrid_ManipulationStartedStoryboard");
            story.Begin();
        }
       
        private void newsGrid_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            //播放 NEWS图标 动画
            story = (Storyboard)this.FindName("newsGrid_ManipulationCompletedStoryboard");
            story.Begin();
        }

        private void newsGrid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //播放 NEWS图标 动画
            story = (Storyboard)this.FindName("newsGrid_TapStoryboard");
            story.Begin();

            //跳转到新闻栏目首页
            this.NavigationService.Navigate(new Uri("/NewsMainPage.xaml", UriKind.Relative));
            
        }
        #endregion

        #region 对新闻的操作
        private void newsOneTextBlock_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            this.newsOneTextBlock.FontSize = 30;
        }

        private void newsOneTextBlock_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            this.newsOneTextBlock.FontSize = 20;
        }

        private void newsOneTextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //跳转到 该条 新闻的 详细页面
            this.NavigationService.Navigate(new Uri("/DetailNewsInfoPage.xaml?id=" + this.newsOneIdTextBlock.Text, UriKind.Relative));
        }
        #endregion

       


        // 用于生成本地化 ApplicationBar 的示例代码
        //private void BuildLocalizedApplicationBar()
        //{
        //    // 将页面的 ApplicationBar 设置为 ApplicationBar 的新实例。
        //    ApplicationBar = new ApplicationBar();

        //    // 创建新按钮并将文本值设置为 AppResources 中的本地化字符串。
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // 使用 AppResources 中的本地化字符串创建新菜单项。
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}