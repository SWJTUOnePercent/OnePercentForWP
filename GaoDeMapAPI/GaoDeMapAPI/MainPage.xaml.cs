using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GaoDeMapAPI.Resources;
using System.Windows.Media.Animation;
using Com.AMap.Api.Maps;
using Com.AMap.Api.Maps.Model;
using System.Diagnostics;
using System.Windows.Media;

namespace GaoDeMapAPI
{
    public partial class MainPage : PhoneApplicationPage
    {
        //故事板
        Storyboard story = null;

        //地图类
        AMap amap = new AMap();

        //标注类 用于显示用户位置
        AMapMarker marker_MyLocation ;
       
        //圆 用于定位
        AMapCircle circle;
        AMapGeolocator mylocation;

        //经纬度 纬度：latitude   经度：longitude
        LatLng latLng__MyLocation;

        //地图交互状态类 
        UiSettings uiset;

        //附近的厕所信息列表
        List<Toilet> aroundToilets=null;

        //定义 点击厕所 弹窗的提示窗口
        AInfoWindow infoWindow;
        //标注类 用于显示附近的厕所
        AMapMarker toiletMarker;

        // 构造函数
        public MainPage()
        {
            InitializeComponent();

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

            //附近的厕所列表
            aroundToilets =new List<Toilet>();
            //31.171184,108.396034
            //29.568386,103.443446
            //29.570858,103.445917
            aroundToilets.Add(new Toilet { Id = 1, Info = "第一个厕所", Latitude = 29.570858, Longitude = 103.446900 });
            aroundToilets.Add(new Toilet { Id = 2, Info = "第二个厕所", Latitude = 29.568500, Longitude = 103.445917 });
            aroundToilets.Add(new Toilet { Id = 2, Info = "第三个厕所", Latitude = 29.568600, Longitude = 103.455917 });
            //添加 标记点击事件
            amap.MarkerClickListener += amap_MarkerClickListener;

            // 加载 附近厕所的 ApplicationBar
            aroundToilets_ApplicationBar();
        }

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
                foreach(Toilet toilet in aroundToilets)
                {
                    AMapMarker amapMarker=amap.AddMarker(new AMapMarkerOptions()
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
        }
        private void MyLocation_Unloaded(object sender, RoutedEventArgs e)
        {
            if (mylocation != null)
            {
                mylocation.PositionChanged -= mylocation_PositionChanged;
                mylocation.Stop();
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

            // 创建新按钮并将文本值设置为 AppResources 中的本地化字符串。
            ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/refresh.png", UriKind.Relative));
            appBarButton.Text ="刷新";
            appBarButton.Click += RefreshButton_Click;
            ApplicationBar.Buttons.Add(appBarButton);

            appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/add.png", UriKind.Relative));
            appBarButton.Text = "添加";
            appBarButton.Click +=AddButton_Click;
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
            this.NavigationService.Navigate(new Uri("/DetailToiletInfoPage.xaml?id="+"1",UriKind.Relative));
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
    }
}