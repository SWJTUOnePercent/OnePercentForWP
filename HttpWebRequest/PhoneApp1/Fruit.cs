using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Xml.Linq;



namespace PhoneApp1
{
    public class Fruit : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
       
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, args);
        }

        public Fruit()
        {
 
        }

        //水果名称
        private string fruitName;

        public string FruitName
        {
            get { return fruitName; }
            set 
            {
                if (value != fruitName)
                {
                    this.fruitName = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("FruitName"));
                }
            } 

        }



        //水果价格
        private string fruitPrice;

        public string FruitPrice
        {
            get { return fruitPrice; }
            
            set 
            {
                if (value != fruitPrice)
                {
                    this.fruitPrice = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("FruitPrice"));
                }
            } 
        }

        public bool  getPrice()
        {
            Dictionary<string, string> fruit_dic = new Dictionary<string, string>();

            fruit_dic.Add("controllers", "Fruit");

            fruit_dic.Add("method", "GetPrice");

            fruit_dic.Add("fruitName", fruitName);

            fruit_dic.Add("id", "1");

            try
            {
                HttpWebHelper httpWebHelper = new HttpWebHelper();
                httpWebHelper.createUrl(fruit_dic);

                DoHttpWebRequest(httpWebHelper.Url);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            } 
        }


        private void DoHttpWebRequest(string url)
        {
           

            //创建WebRequest类
            WebRequest request = HttpWebRequest.Create(url);
            
            //返回异步操作的状态
            IAsyncResult result = (IAsyncResult)request.BeginGetResponse(ResponseCallback, request);
        }

        private void ResponseCallback(IAsyncResult result)
        {
            //获取异步操作返回的的信息
            HttpWebRequest request = (HttpWebRequest)result.AsyncState;

            //结束对 Internet 资源的异步请求
            WebResponse response = request.EndGetResponse(result);

            //string responseStr;
            List<Fruit>  xmlResponseFruitList = new List<Fruit>();

            //解析XML文件
            //例如以下请求
            //http://killyouad.com/WP/index.php/Fruit/GetPrice?fruitName=%E6%A2%A8&id=1
            //的返回数据是一个XML文件
            //<?xml version="1.0" encoding="UTF-8"?>
            //<Fruit>
            //  <apple>
            //       <id>1</id>
            //       <price>200</price>
            //  </apple>
            //  <apple>
            //        <id>2</id>
            //        <price>300</price>
            //  </apple>
            //</Fruit>
            //
            try
            {
                Stream stream = response.GetResponseStream();
                XElement doc = XElement.Load(stream);
                Fruit fruit = null;
                foreach (XElement xElementfruit in doc.Descendants("apple"))
                {
                    fruit = new Fruit();
                    fruit.FruitPrice = (string)xElementfruit.Element("price").Value;
                    fruit.FruitName = (string)xElementfruit.Element("id").Value;
                    //xmlResponseFruitList.FruitPrice = (string)doc.Element("price").Value;
                    xmlResponseFruitList.Add(fruit);
                }
            }
            catch (Exception)
            {
                
                throw;
            }

            //操作UI线程的 更新UI界面显示水果的价格
            Deployment.Current.Dispatcher.BeginInvoke(() =>
             {
                 this.FruitPrice = xmlResponseFruitList[0].FruitName+"---"+ xmlResponseFruitList[0].FruitPrice;
             });
        }
    }
}
