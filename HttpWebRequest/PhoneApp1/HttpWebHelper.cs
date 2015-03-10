using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PhoneApp1
{
    class HttpWebHelper
    {
        //服务器的地址
        private string url = "http://121.40.95.61/WP/index.php/";
        /// private string url = "http://killyouad.com/WP/index.php/";
        ///http://killyouad.com/WP/index.php/Fruit/GetPrice?fruitName=梨&id=1
        ///
        public string Url
        {
            get { return url; }
            set { url = value; }
        }
        
        //生成请求的URL
       public void createUrl(Dictionary<string, string> fruit_dic)
       {
           foreach (var dic in fruit_dic)
           {
               if (dic.Key == "controllers")
               {
                   //设置 对请求进行进行处理的 YII 的 controllers
                   url += dic.Value + "/";
               }
               else if (dic.Key == "method")
               {
                   //设置 对请求进行处理的 YII 的 controllers 中的方法 
                   url += dic.Value + "?";
               }
               else
               {
                   //设置 GET方法的 请求参数值
                   url += dic.Key + "=" + dic.Value + "&";
               }
           }     
       }

       public HttpWebRequest AsyncRequest {get;set; }
       public HttpWebResponse AsyncResponse { get; set; }

    }
}
