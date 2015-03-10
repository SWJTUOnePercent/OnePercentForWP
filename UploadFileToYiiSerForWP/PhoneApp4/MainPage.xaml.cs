using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PhoneApp4.Resources;
using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;
using System.IO;
using System.IO.IsolatedStorage;
using Microsoft.Xna.Framework.Media;
using Microsoft.Phone.BackgroundTransfer;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Storage;
using System.Text;

namespace PhoneApp4
{
    public partial class MainPage : PhoneApplicationPage
    {

        CameraCaptureTask cct;

        string fileName = "呵呵.jpg";
        string fileDirectoty = "shared/transfers";
        string url = "http://121.40.95.61/WP/index.php/UploadFile/Create";
        byte[] fileBytes = null;

        string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");

       // WriteableBitmap Wbmp;
        // 构造函数
        public MainPage()
        {
            InitializeComponent();

            //创建一个捕获相机拍照的选择器 
            cct = new CameraCaptureTask();
            //注册选择器完成的事件
            cct.Completed += new EventHandler<PhotoResult>(cct_Completed); 

            // 用于本地化 ApplicationBar 的示例代码
            //BuildLocalizedApplicationBar();
        }

        private void cct_Completed(object sender, PhotoResult e)
        {
            //判断结果是否成功 
            if (e.TaskResult == TaskResult.OK)
            {
                long streamLength = e.ChosenPhoto.Length;

                Stream fos = e.ChosenPhoto;
                fileBytes = new byte[e.ChosenPhoto.Length];
                fos.Read(fileBytes,0, (int)e.ChosenPhoto.Length);
                fos.Close();

                MemoryStream stream = new MemoryStream(fileBytes);
                BitmapImage bmpSource = new BitmapImage();
                bmpSource.SetSource(stream);

                WriteableBitmap wbmp = new WriteableBitmap(bmpSource);


                //保存图片  到 独立存储
                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (!store.DirectoryExists(fileDirectoty))
                    {
                        store.CreateDirectory(fileDirectoty);
                    }

                    if (!store.FileExists(fileDirectoty+"/"+fileName))
                    {
                        store.DeleteFile(fileDirectoty+"/"+fileName);
                    }
                    using (IsolatedStorageFileStream isoStream = store.OpenFile(fileDirectoty + "/" + fileName, FileMode.OpenOrCreate))
                    {
                        Extensions.SaveJpeg(wbmp, isoStream, wbmp.PixelWidth, wbmp.PixelHeight, 0, 100);
                        isoStream.Close();
                        store.Dispose();
                    }

                }
                image1.Source = bmpSource; 
            }
            else
            {
                image1.Source = null;
            }
        }

        private void buttonTakePic_Click(object sender, RoutedEventArgs e)
        {
            cct.Show();
           
        }

        private async void buttonUploadPic_Click(object sender, RoutedEventArgs e)
        {
            UploadFilesToServer();
        }


       
        private void UploadFilesToServer()
        {

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "multipart/form-data; boundary=" + boundary;
            httpWebRequest.Method = "POST";
            httpWebRequest.BeginGetRequestStream(new AsyncCallback(getRequestStreamCallBack), httpWebRequest);
            
        }

        private void getRequestStreamCallBack(IAsyncResult requestRes)
        {
            HttpWebRequest request = requestRes.AsyncState as HttpWebRequest;
            Stream stream = request.EndGetRequestStream(requestRes);
            var Params = new Dictionary<string, string> { { "userid", "9" } };

            string file = fileDirectoty + "/" + fileName;

            WriteMultipartForm(stream, boundary, Params, file, "image/jpeg", fileBytes);

            stream.Flush();
            stream.Close();

            request.BeginGetResponse(new AsyncCallback(getResponseCallback), request);
        }

        private void getResponseCallback(IAsyncResult requestRes)
        {
            HttpWebRequest request = requestRes.AsyncState as HttpWebRequest;

            HttpWebResponse response = request.EndGetResponse(requestRes) as HttpWebResponse;
            Stream stream = response.GetResponseStream();
            StreamReader streamRead = new StreamReader(stream);
            String responseString = streamRead.ReadToEnd();
            stream.Close();
            streamRead.Close();
            response.Close();
            //操作UI线程的 更新UI界面显示水果的价格
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                MessageBox.Show(responseString);
            });
            //
        }

        /// <summary>
        /// Writes multi part HTTP POST request. Author : Farhan Ghumra
        /// </summary>
        private void WriteMultipartForm(Stream s, string boundary, Dictionary<string, string> data, string fileName, string fileContentType, byte[] fileData)
        {
            /// The first boundary
            byte[] boundarybytes = Encoding.UTF8.GetBytes("--" + boundary + "\r\n");
            /// the last boundary.
            byte[] trailer = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
            /// the form data, properly formatted
            string formdataTemplate1 = "Content-Disposition: form-data; name=\"UploadFile[image]\"\r\n\r\n\r\n";
            /// the form-data file upload, properly formatted
            string fileheaderTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\";\r\nContent-Type: {2}\r\n\r\n";
            string formdataTemplate2 = "Content-Disposition: form-data; name=\"yt0\"\r\n\r\n鎻愪氦r\n";
            ///// Added to track if we need a CRLF or not.
            //bool bNeedsCRLF = false;

            //if (data != null)
            //{
            //    foreach (string key in data.Keys)
            //    {
            //        /// if we need to drop a CRLF, do that.
            //        if (bNeedsCRLF)
            //            WriteToStream(s, "\r\n");

            //        /// Write the boundary.
            //        WriteToStream(s, boundarybytes);

            //        /// Write the key.
            //        WriteToStream(s, string.Format(formdataTemplate, key, data[key]));
            //        bNeedsCRLF = true;
            //    }
            //}

            ///// If we don't have keys, we don't need a crlf.
            //if (bNeedsCRLF)
            //    WriteToStream(s, "\r\n");

            WriteToStream(s, boundarybytes);
            WriteToStream(s, Encoding.UTF8.GetBytes(formdataTemplate1));
            WriteToStream(s, boundarybytes);
            WriteToStream(s, string.Format(fileheaderTemplate, "UploadFile[image]", fileName, fileContentType));
            /// Write the file data to the stream.
            WriteToStream(s, fileData);
            WriteToStream(s, boundarybytes);
            WriteToStream(s, Encoding.UTF8.GetBytes(formdataTemplate2));
            WriteToStream(s, trailer);
        }

        /// <summary>
        /// Writes string to stream. Author : Farhan Ghumra
        /// </summary>
        private void WriteToStream(Stream s, string txt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(txt);
            s.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Writes byte array to stream. Author : Farhan Ghumra
        /// </summary>
        private void WriteToStream(Stream s, byte[] bytes)
        {
            s.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Returns byte array from StorageFile. Author : Farhan Ghumra
        /// </summary>
        private async Task<byte[]> GetBytesAsync(StorageFile file)
        {
            byte[] fileBytes = null;
            using (var stream = await file.OpenReadAsync())
            {
                fileBytes = new byte[stream.Size];
                using (var reader = new DataReader(stream))
                {
                    await reader.LoadAsync((uint)stream.Size);
                    reader.ReadBytes(fileBytes);
                }
            }

            return fileBytes;
        }

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